using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess
{
    public class MongoSuggestionData : ISuggestionData
    {
        private readonly IMemoryCache _cache;
        private readonly IDbConnection _database;
        private readonly IMongoCollection<SuggestionModel> _suggestions;
        private readonly IUserData _userData;
        private const string CacheName = "SuggestionData";

        public MongoSuggestionData(IDbConnection database, IUserData userData, IMemoryCache cache)
        {
            _database = database;
            _userData = userData;
            _cache = cache;
            _suggestions = database.SuggestionCollection;
        }
        public async Task<List<SuggestionModel>> GetAllSuggestions()
        {
            var output = _cache.Get<List<SuggestionModel>>(CacheName);

            if (output is null)
            {
                var results = await _suggestions.FindAsync(s => s.Archived == false);
                output = results.ToList();

                _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
            }

            return output;
        }
        public async Task<List<SuggestionModel>> GetAllApprovedSuggestion()
        {
            var output = await GetAllSuggestions();
            return output.Where(s => s.ApprovedForRelease == true).ToList();
        }
        public async Task<SuggestionModel> GetSuggestion(string id)
        {
            var results = await _suggestions.FindAsync(s => s.Id == id);
            return results.FirstOrDefault();
        }
        public async Task<List<SuggestionModel>> GetAllSuggestionsWaitingForApproval()
        {
            var output = await GetAllSuggestions();
            output = output.Where(x => !x.ApprovedForRelease && !x.Rejected).ToList();
            return output;
        }
        public async Task UpdateSuggestion(SuggestionModel suggestion)
        {
            await _suggestions.ReplaceOneAsync(s => s.Id == suggestion.Id, suggestion);
            _cache.Remove(CacheName);
        }
        public async Task UpvoteSuggestion(string suggestionId, string userId)
        {
            var client = _database.Client;

            using var session = await client.StartSessionAsync();

            session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_database.DbName);
                var suggestionsInTransaction = db.GetCollection<SuggestionModel>(_database.SuggestionCollectionName);
                var suggestion = (await suggestionsInTransaction.FindAsync(s => s.Id == suggestionId)).First();

                var isUpvote = suggestion.UserVotes.Add(userId);
                if (isUpvote == false)
                {
                    suggestion.UserVotes.Remove(userId);
                }

                await suggestionsInTransaction.ReplaceOneAsync(session, s => s.Id == suggestionId, suggestion);

                var usersInTransaction = db.GetCollection<UserModel>(_database.UserCollectionName);
                var user = await _userData.GetUser(userId);

                if (isUpvote)
                {
                    user.VotedOnSuggestions.Add(new BasicSuggestionModel(suggestion));
                }
                else
                {
                    var suggestionToRemove = user.VotedOnSuggestions.First(s => s.Id == suggestionId);
                    user.VotedOnSuggestions.Remove(suggestionToRemove);
                }

                await usersInTransaction.ReplaceOneAsync(session, u => u.Id == userId, user);

                await session.CommitTransactionAsync();

                _cache.Remove(CacheName);
            }
            catch
            {
                await session.AbortTransactionAsync();
                //throw;
            }
        }
        //public async Task CreateSuggestion(SuggestionModel suggestion)
        //{
        //    var client = _database.Client;
        //    using var session = await client.StartSessionAsync();
        //    session.StartTransaction();

        //    try
        //    {
        //        var db = client.GetDatabase(_database.DbName);
        //        var suggestionsInTransaction = db.GetCollection<SuggestionModel>(_database.SuggestionCollectionName);
        //        await suggestionsInTransaction.InsertOneAsync(session, suggestion);

        //        var usersInTransaction = db.GetCollection<UserModel>(_database.UserCollectionName);
        //        var user = await _userData.GetUser(suggestion.Author.Id);

        //        user.AuthoredSuggestions.Add(new BasicSuggestionModel(suggestion));
        //        await usersInTransaction.ReplaceOneAsync(session, u => u.Id == user.Id, user);

        //        await session.CommitTransactionAsync();

        //        // we dont invalidate the cache since we require approval for each suggestion.
        //    }
        //    catch
        //    {
        //        await session.AbortTransactionAsync();
        //        throw;
        //    }
        //}
        public async Task CreateSuggestion(SuggestionModel suggestion)
        {
            var client = _database.Client;

            bool transactionsSupported = client.Cluster.Description.Type != MongoDB.Driver.Core.Clusters.ClusterType.Standalone;

            if (transactionsSupported)
            {
                using var session = await client.StartSessionAsync();
                session.StartTransaction();

                try
                {
                    var db = client.GetDatabase(_database.DbName);
                    var suggestionsInTransaction = db.GetCollection<SuggestionModel>(_database.SuggestionCollectionName);
                    await suggestionsInTransaction.InsertOneAsync(session, suggestion);

                    var usersInTransaction = db.GetCollection<UserModel>(_database.UserCollectionName);
                    var user = await _userData.GetUser(suggestion.Author.Id);

                    user.AuthoredSuggestions.Add(new BasicSuggestionModel(suggestion));
                    await usersInTransaction.ReplaceOneAsync(session, u => u.Id == user.Id, user);

                    await session.CommitTransactionAsync();
                }
                catch
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
            else
            {
                // No transaction fallback for standalone MongoDB
                var db = client.GetDatabase(_database.DbName);
                var suggestions = db.GetCollection<SuggestionModel>(_database.SuggestionCollectionName);
                await suggestions.InsertOneAsync(suggestion);

                var users = db.GetCollection<UserModel>(_database.UserCollectionName);
                var user = await _userData.GetUser(suggestion.Author.Id);

                user.AuthoredSuggestions.Add(new BasicSuggestionModel(suggestion));
                await users.ReplaceOneAsync(u => u.Id == user.Id, user);
            }
        }

        public async Task<List<SuggestionModel>> GetUsersSuggestions(string userId)
        {
            var output = _cache.Get<List<SuggestionModel>>(userId);
            if (output is null)
            {
                var results = await _suggestions.FindAsync(s => s.Author.Id == userId);
                output = results.ToList();

                _cache.Set(userId, output, TimeSpan.FromMinutes(1));
            }

            return output;
        }
    }
}
