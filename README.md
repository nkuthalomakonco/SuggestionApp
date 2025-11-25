# Suggestion Site Application

Suggestion Site Application
A modern, scalable web application built with .NET, Blazor Server, and MongoDB for handling user suggestions with authentication and administrative workflows.

🚀 Features
User Management: Azure Active Directory B2C authentication & authorization

Suggestion System: Create, view, and manage user suggestions

Admin Controls: Role-based access for administrative functions

Categories & Statuses: Organized suggestion tracking

Upvote System: Community engagement through voting

Responsive Design: Mobile-friendly UI

Caching: Optimized performance with IMemoryCache

🏗️ Architecture
Technology Stack
Frontend: Blazor Server

Backend: .NET 7/8

Database: MongoDB

Authentication: Azure AD B2C

Caching: IMemoryCache

Project Structure
text
Suggestion-Site-Application/
├── UI/                 # Blazor components and pages
├── DataAccess/         # MongoDB data layer
├── Models/            # Data models and entities
├── Logic/             # Business logic layer
└── Configuration/     # App settings and configs
📋 Core Modules
Module 1: Planning & Design
Project requirements and architecture

WOULD Planning Method (Wants, Outcomes, Use cases, Limitations, Development)

Data model design (User, Suggestion, Category, Status, Upvotes)

Module 2: Project Setup
Blazor Server project creation

Class library organization

Dependency Injection configuration

AppSettings and caching setup

Module 3: Data Access Layer
MongoDB connection and context

CRUD operations for:

Users

Categories

Statuses

Suggestions

Upvote logic implementation

Module 4: Sample Data
Initial data seeding

Default categories and statuses

Demo suggestions

Module 5: Blazor UI Development
Layout and reusable components

Suggestion listing with filtering and pagination

Suggestion creation and details pages

User profile and admin dashboard

Module 6: Authentication & Authorization
Azure B2C tenant setup

Authentication middleware integration

Role-based authorization (Admin vs Regular users)

Login/Logout UI components

Module 7: Styling & Polish
CSS isolation and responsive design

Mobile-friendly layout

User experience enhancements

Module 8: Testing & Debugging
Happy path testing

Edge case handling

Authentication debugging

Module 9: Refactoring & Optimization
Code cleanup and performance improvements

Folder structure reorganization

Module 10: .NET Upgrades
Migration to .NET 7/8

Package updates and breaking changes

🗄️ Data Models
Key Entities
User: Authentication and profile data

Suggestion: Core content with title, description, category

Category: Suggestion classification

Status: Workflow tracking (New, In Progress, Completed, etc.)

Upvotes: User voting system

🔐 Authentication
Azure AD B2C integration

Custom user flows (SignUpSignIn, PasswordReset)

Role-based claims (Admin policy)

Secure page authorization

🎯 Admin Features
Suggestion approval workflow

Status management

User administration

Analytics and reporting views

🚀 Getting Started
Prerequisites
.NET 7/8 SDK

MongoDB instance

Azure AD B2C tenant

Installation
Clone the repository

Configure appsettings.json with your MongoDB connection string

Set up Azure B2C authentication

Run database seeding

Launch the application

📱 UI Components
Reusable Components
SuggestionCard: Display suggestion preview

StatusBadge: Visual status indicators

CategoryChip: Category tags

UpvoteButton: Voting interaction

Key Pages
Home: Suggestion listing with filters

Suggestion Details: Full suggestion view with upvotes

Create Suggestion: Submission form

Profile: User suggestions and preferences

Admin Dashboard: Management interface

🔧 Configuration
AppSettings
json
{
  "MongoDB": {
    "ConnectionString": "",
    "DatabaseName": ""
  },
  "AzureB2C": {
    "Instance": "",
    "ClientId": "",
    "Domain": ""
  }
}
🧪 Testing
Comprehensive testing coverage including:

User authentication flows

Suggestion creation and management

Admin functionality

Edge cases and error handling

📈 Scalability
Designed to handle thousands of visitors with:

Efficient MongoDB queries

Strategic caching implementation

Optimized Blazor Server components

Azure B2C scalable authentication

🎓 Learning Outcomes
This project demonstrates enterprise-level application development using:

Modern .NET ecosystem

NoSQL database integration

Cloud authentication services

Responsive UI design

Full-stack development practices

Built with ❤️ using Blazor Server, MongoDB, and Azure B2C
