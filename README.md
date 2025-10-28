# 💰 Finance Tracker API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-Atlas-47A248?logo=mongodb)](https://www.mongodb.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A modern, secure, and scalable RESTful API for personal finance management built with ASP.NET Core 8.0 and MongoDB.

## 📋 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [API Documentation](#-api-documentation)
- [Project Structure](#-project-structure)
- [Security](#-security)
- [Development](#-development)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

- **User Management**: Secure user registration and authentication with JWT tokens
- **Transaction Tracking**: Record and categorize income/expense transactions
- **Budget Management**: Set and monitor budgets by category
- **Savings Goals**: Create and track progress toward financial goals
- **Category Organization**: Custom categories for income and expenses
- **Financial Reports**: Generate insights and summaries of spending patterns
- **RESTful API**: Clean, well-documented endpoints following REST principles
- **Secure Authentication**: JWT-based authentication with password hashing (PBKDF2)

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│        FinanceTracker.API               │  ← Presentation Layer (Controllers, Auth)
├─────────────────────────────────────────┤
│      FinanceTracker.Services            │  ← Business Logic Layer (Services, DTOs)
├─────────────────────────────────────────┤
│    FinanceTracker.Infrastructure        │  ← Data Access Layer (Repositories, DB)
├─────────────────────────────────────────┤
│       FinanceTracker.Domain             │  ← Core Domain Models (Entities)
└─────────────────────────────────────────┘
```

### Layers

- **Domain**: Core business entities (User, Transaction, Budget, Category, SavingGoal, Report)
- **Infrastructure**: Database context, repositories, and data access implementations
- **Services**: Business logic, DTOs, AutoMapper profiles, and service interfaces
- **API**: Controllers, authentication, middleware, and API configuration

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: MongoDB Atlas (NoSQL)
- **Authentication**: JWT Bearer Tokens
- **Password Hashing**: PBKDF2 (RFC2898) with SHA256
- **Object Mapping**: AutoMapper
- **API Documentation**: Swagger/OpenAPI
- **Dependency Injection**: Built-in ASP.NET Core DI Container

### Key NuGet Packages

- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT authentication
- `MongoDB.Driver` - MongoDB .NET driver
- `AutoMapper` - Object-to-object mapping
- `Swashbuckle.AspNetCore` - Swagger/OpenAPI support

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [MongoDB Atlas](https://www.mongodb.com/cloud/atlas) account (or local MongoDB instance)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) / [Rider](https://www.jetbrains.com/rider/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/motuen66/FinanceTrackerApp.git
   cd FinanceTrackerApp
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

### Configuration

> ⚠️ **Security Notice**: Never commit credentials to version control. Use User Secrets for development and environment variables for production.

#### Required Configuration Keys

The application requires the following configuration sections:

**JwtConfig:**
- `Issuer` - JWT token issuer
- `Audience` - JWT token audience
- `Key` - Secret key for signing tokens (minimum 32 characters)
- `TokenValidityMins` - Token expiration time in minutes

**MongoDbSettings:**
- `ConnectionString` - MongoDB connection string
- `DatabaseName` - Database name

#### Setup with User Secrets (Recommended)

```bash
cd FinanceTracker.API
dotnet user-secrets init
dotnet user-secrets set "JwtConfig:Issuer" "<your-issuer>"
dotnet user-secrets set "JwtConfig:Audience" "<your-audience>"
dotnet user-secrets set "JwtConfig:Key" "<your-secret-key>"
dotnet user-secrets set "JwtConfig:TokenValidityMins" "60"
dotnet user-secrets set "MongoDbSettings:ConnectionString" "<your-mongodb-connection-string>"
dotnet user-secrets set "MongoDbSettings:DatabaseName" "<your-database-name>"
```

#### Setup with Environment Variables (Production)

Set environment variables with the prefix `ASPNETCORE_`:
- `ASPNETCORE_JwtConfig__Issuer`
- `ASPNETCORE_JwtConfig__Audience`
- `ASPNETCORE_JwtConfig__Key`
- `ASPNETCORE_JwtConfig__TokenValidityMins`
- `ASPNETCORE_MongoDbSettings__ConnectionString`
- `ASPNETCORE_MongoDbSettings__DatabaseName`

### Running the Application

1. **Run the API**
   ```bash
   cd FinanceTracker.API
   dotnet run
   ```
   Or use the watch mode for hot reload:
   ```bash
   dotnet watch run
   ```

2. **Access Swagger UI**
   - Navigate to: `https://localhost:7239/` or `http://localhost:5000/`
   - Swagger documentation will be displayed automatically

## 📚 API Documentation

### Authentication

All endpoints (except login and registration) require JWT authentication.

#### Login
```http
POST /api/Account/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "yourPassword"
}
```

**Response:**
```json
{
  "email": "user@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600
}
```

### Core Endpoints

| Resource | Endpoint | Method | Description |
|----------|----------|--------|-------------|
| **Users** | `/api/Users` | GET | Get all users |
| | `/api/Users/{id}` | GET | Get user by ID |
| | `/api/Users` | POST | Create new user |
| | `/api/Users/{id}` | PUT | Update user |
| | `/api/Users/{id}` | DELETE | Delete user |
| **Transactions** | `/api/Transactions` | GET | Get all transactions |
| | `/api/Transactions/{id}` | GET | Get transaction by ID |
| | `/api/Transactions` | POST | Create transaction |
| | `/api/Transactions/{id}` | PUT | Update transaction |
| | `/api/Transactions/{id}` | DELETE | Delete transaction |
| **Categories** | `/api/Categories` | GET | Get all categories |
| | `/api/Categories/{id}` | GET | Get category by ID |
| | `/api/Categories` | POST | Create category |
| | `/api/Categories/{id}` | PUT | Update category |
| | `/api/Categories/{id}` | DELETE | Delete category |
| **Budgets** | `/api/Budgets` | GET | Get all budgets |
| | `/api/Budgets/{id}` | GET | Get budget by ID |
| | `/api/Budgets` | POST | Create budget |
| | `/api/Budgets/{id}` | PUT | Update budget |
| | `/api/Budgets/{id}` | DELETE | Delete budget |
| **Saving Goals** | `/api/SavingGoals` | GET | Get all saving goals |
| | `/api/SavingGoals/{id}` | GET | Get saving goal by ID |
| | `/api/SavingGoals` | POST | Create saving goal |
| | `/api/SavingGoals/{id}` | PUT | Update saving goal |
| | `/api/SavingGoals/{id}` | DELETE | Delete saving goal |
| **Reports** | `/api/Reports` | GET | Get all reports |
| | `/api/Reports/{id}` | GET | Get report by ID |

### Using Authentication

Add the JWT token to the `Authorization` header:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📁 Project Structure

```
FinanceTrackerApp/
├── FinanceTracker.API/              # Presentation Layer
│   ├── Controllers/                 # API endpoints
│   │   ├── AccountController.cs     # Authentication
│   │   ├── UsersController.cs
│   │   ├── TransactionsController.cs
│   │   ├── CategoriesController.cs
│   │   ├── BudgetsController.cs
│   │   ├── SavingGoalsController.cs
│   │   └── ReportsController.cs
│   ├── Handlers/                    # Utility handlers
│   │   └── PasswordHashHandler.cs   # Password hashing
│   ├── Models/                      # API models
│   │   └── API/
│   │       ├── LoginRequestModel.cs
│   │       └── LoginResponseModel.cs
│   ├── Services/                    # API-level services
│   │   └── JwtService.cs            # JWT token generation
│   ├── Program.cs                   # App entry point
│   └── appsettings.json             # Configuration template
│
├── FinanceTracker.Services/         # Business Logic Layer
│   ├── DTOs/                        # Data Transfer Objects
│   │   ├── MappingProfile.cs        # AutoMapper configuration
│   │   ├── UserDtos/
│   │   ├── TransactionDtos/
│   │   ├── CategoryDtos/
│   │   ├── BudgetDtos/
│   │   ├── SavingGoalDtos/
│   │   └── ReportDtos/
│   ├── Interfaces/                  # Service & Repository contracts
│   │   ├── Services/
│   │   └── Repositories/
│   └── [Entity]Service.cs           # Business logic implementations
│
├── FinanceTracker.Infrastructure/   # Data Access Layer
│   ├── Database/
│   │   ├── MongoDbContext.cs        # DB context
│   │   └── MongoDbSettings.cs       # DB configuration
│   └── Repositories/                # Data access implementations
│       ├── GenericRepository.cs
│       ├── UserRepository.cs
│       ├── TransactionRepository.cs
│       ├── CategoryRepository.cs
│       ├── BudgetRepository.cs
│       ├── SavingGoalRepository.cs
│       └── ReportRepository.cs
│
└── FinanceTracker.Domain/           # Core Domain Layer
    ├── User.cs
    ├── Transaction.cs
    ├── Category.cs
    ├── Budget.cs
    ├── SavingGoal.cs
    ├── Report.cs
    └── BsonCollectionAttribute.cs   # MongoDB collection attribute
```

## 🔒 Security

### Authentication & Authorization

- **JWT Tokens**: Stateless authentication using JWT Bearer tokens
- **Token Expiry**: Configurable token validity (default: 60 minutes)
- **Secure Headers**: HTTPS enforcement in production

### Password Security

- **Hashing Algorithm**: PBKDF2 (RFC2898) with HMAC-SHA256
- **Salt**: 128-bit cryptographically random salt per password
- **Iterations**: 10,000 iterations for key derivation
- **Key Size**: 256-bit derived key
- **Storage Format**: `{iterations}.{base64-salt}.{base64-hash}`

### Best Practices

- ✅ Store secrets in User Secrets (dev) or environment variables (prod)
- ✅ Never commit `appsettings.Development.json` with credentials
- ✅ Use HTTPS in production
- ✅ Rotate JWT keys periodically
- ✅ Implement rate limiting for production
- ✅ Enable MongoDB IP whitelisting
- ✅ Use strong, unique JWT signing keys (minimum 256 bits)

## 💻 Development

### Build and Test

```bash
# Build the solution
dotnet build

# Run tests (if available)
dotnet test

# Run with hot reload
dotnet watch run --project FinanceTracker.API
```

### Code Style

- Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Keep controllers thin, business logic in services
- Use dependency injection for loose coupling

### Adding a New Feature

1. **Domain**: Add/modify entity in `FinanceTracker.Domain`
2. **Infrastructure**: Create/update repository in `FinanceTracker.Infrastructure`
3. **Services**: Implement business logic and DTOs in `FinanceTracker.Services`
4. **API**: Add controller endpoints in `FinanceTracker.API`
5. **Register**: Update DI registrations in `Program.cs`

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📧 Contact

- **Author**: motuen66
- **Repository**: [https://github.com/motuen66/FinanceTrackerApp](https://github.com/motuen66/FinanceTrackerApp)

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- MongoDB for the flexible NoSQL database
- All contributors and open-source libraries used in this project

---

**Made with ❤️ using .NET 8.0 and MongoDB**