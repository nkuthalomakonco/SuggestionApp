using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace SuggestionAppUI
{
    public static class RegisterServices
    {

        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            //builder.Services.AddRazorPages();
            //builder.Services.AddServerSideBlazor(); 
            builder.Services.AddMemoryCache();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
        }
    }
}
