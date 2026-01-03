# CreateLotteryLambda

A simple Lambda shell structure with layered architecture for lottery creation functionality.

## Architecture

This project follows a clean, layered architecture pattern:

```
src/
├── CreateLotteryLambda.sln
├── CreateLotteryLambda.Application/     (Startup project - Console App)
│   ├── CreateLotteryLambda.Application.csproj
│   ├── Program.cs
│   └── appsettings.json
├── CreateLotteryLambda.Domain/          (Class Library)
│   ├── CreateLotteryLambda.Domain.csproj
│   └── Interfaces/
│       └── Infrastructure/
│           └── Context/
│               └── IDbConnectionContext.cs
└── CreateLotteryLambda.Infrastructure/  (Class Library)
    ├── CreateLotteryLambda.Infrastructure.csproj
    └── Context/
        └── DbConnectionContext.cs
```

## Technologies

- **.NET 8.0**: Target framework for all projects
- **Npgsql 9.0.3**: PostgreSQL database provider
- **Dapper 2.1.66**: Lightweight ORM for data access
- **Microsoft.Extensions.Configuration**: Configuration management

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- PostgreSQL database (optional - for full database connectivity testing)

## Configuration

The application reads database connection settings from `appsettings.json` located in the `CreateLotteryLambda.Application` project.

Update the connection string in `src/CreateLotteryLambda.Application/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=lottery_db;Username=your_username;Password=your_password"
  }
}
```

Replace the placeholder values with your actual PostgreSQL connection details:
- `localhost` - Your PostgreSQL server host
- `5432` - PostgreSQL port (default is 5432)
- `lottery_db` - Your database name
- `your_username` - Your PostgreSQL username
- `your_password` - Your PostgreSQL password

## Building the Solution

Navigate to the `src` directory and build the solution:

```bash
cd src
dotnet build CreateLotteryLambda.sln
```

## Running the Application

Navigate to the Application project directory and run:

```bash
cd src/CreateLotteryLambda.Application
dotnet run
```

Or run from the solution directory:

```bash
cd src
dotnet run --project CreateLotteryLambda.Application/CreateLotteryLambda.Application.csproj
```

## What the Application Does

When executed, the application performs the following operations:

1. **Mocks an SQS Message**: Creates and displays a simulated SQS message with a MessageId and Body
2. **Connects to PostgreSQL**: Attempts to establish a database connection using the configured connection string
3. **Displays Results**: Shows success or failure messages for each operation
4. **Completes Execution**: Gracefully terminates

### Expected Output

```
Received SQS Message: {"MessageId":"123","Body":"Test lottery creation message"}

Attempting to connect to PostgreSQL database...
Successfully connected to PostgreSQL database!

Execution completed.
```

**Note**: If PostgreSQL is not running or the connection string is incorrect, the application will display an appropriate error message but will still complete successfully.

## Project Structure Details

### Domain Layer (`CreateLotteryLambda.Domain`)
- Contains domain interfaces and contracts
- Defines `IDbConnectionContext` interface for database operations
- No external dependencies (pure domain logic)

### Infrastructure Layer (`CreateLotteryLambda.Infrastructure`)
- Implements infrastructure concerns
- Contains `DbConnectionContext` implementation for PostgreSQL connectivity
- Depends on Domain layer
- Uses Npgsql and Dapper for data access

### Application Layer (`CreateLotteryLambda.Application`)
- Entry point for the application
- Console application that orchestrates the flow
- Depends on both Domain and Infrastructure layers
- Handles configuration and dependency management

## Future Enhancements

This is a shell/skeleton structure. Future enhancements may include:
- Actual AWS Lambda integration with AWS SDK
- Real SQS message processing
- Business logic for lottery creation
- Database repositories and data access layer
- Unit and integration tests
- Docker support for local development

## License

[Add your license information here]
