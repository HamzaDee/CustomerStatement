# Customer Statement API

A small ASP.NET Core Web API for generating and retrieving monthly customer account statements.

The system calculates monthly account balances from customer transactions, generates a statement for a requested month, associates the month's transactions with that statement, and sends the generated statement through an email service.

The API also supports retrieving customer statements by customer ID and optional statement-month range.

## Key Features

- Generate monthly customer account statements
- Calculate opening and closing balances
- Associate transactions with generated statements
- Retrieve statements by customer ID
- Filter statements by month range
- Prevent duplicate statements for the same customer and month
- FluentValidation for request validation
- Global exception handling
- Swagger/OpenAPI documentation with request and response examples
- Email service abstraction
- SQL Server persistence using Entity Framework Core

## Architecture

The project follows **Clean Architecture** with a clear separation of responsibilities.

```text
CustomerStatement
│
├── CustomerStatement.Api
│   └── Controllers, Swagger, Exception Handling
│
├── CustomerStatement.Application
│   ├── CQRS Commands & Queries
│   ├── MediatR Handlers
│   ├── FluentValidation
│   ├── DTOs
│   └── Application Interfaces
│
├── CustomerStatement.Domain
│   └── Domain Entities
│
└── CustomerStatement.Infrastructure
    ├── Entity Framework Core
    ├── SQL Server
    ├── Repositories
    ├── Email Service
    └── Database Migrations

    ## Project Structure

```text
CustomerStatement/
│
├── CustomerStatement.Api/
│   ├── Controllers/
│   ├── Exceptions/
│   ├── Swagger/
│   ├── Program.cs
│   └── appsettings.json
│
├── CustomerStatement.Application/
│   ├── Behaviors/
│   ├── DTOs/
│   ├── Features/
│   │   └── Statements/
│   │       ├── Commands/
│   │       └── Queries/
│   └── Interfaces/
│
├── CustomerStatement.Domain/
│   └── Entities/
│
├── CustomerStatement.Infrastructure/
│   ├── Data/
│   ├── Migrations/
│   ├── Repositories/
│   └── Services/
│
├── CustomerStatement.sln
├── .gitignore
└── README.md

## API Endpoints

Base route:

```text
/api/Statements


```markdown
### Get Statements

Retrieves customer account statements by customer ID.

Optional month filters can be used to retrieve statements within a specific range.

Example:

```http
GET /api/Statements?customerId=1&fromMonth=2026-01-01&toMonth=2026-06-01
```

### Generate Monthly Statement

Generates a monthly account statement for a customer and sends it by email.

Example request:

```http
POST /api/Statements/generate
Content-Type: application/json
```

```json
{
  "customerId": 1,
  "statementMonth": "2026-06-01"
}
```

Example response:

```http
HTTP/1.1 200 OK
Content-Type: application/json
```

```json
{
  "statementId": 6,
  "message": "Monthly statement generated successfully."
}
```

## GET Response

Example response:

```json
[
  {
    "id": 1,
    "customerId": 1,
    "customerName": "John Smith",
    "customerEmail": "john.smith@example.com",
    "statementMonth": "2026-01-01",
    "openingBalance": 1000.00,
    "closingBalance": 1250.00,
    "generatedAt": "2026-01-31T10:00:00",
    "transactions": [
      {
        "id": 1,
        "transactionDate": "2026-01-05",
        "description": "Salary",
        "debit": 0.00,
        "credit": 500.00
      },
      {
        "id": 2,
        "transactionDate": "2026-01-10",
        "description": "Online Purchase",
        "debit": 100.00,
        "credit": 0.00
      }
    ]
  }
]
```

## Database Setup

The application uses **SQL Server** with **Entity Framework Core**.

### Connection String

The database connection string is stored using **ASP.NET Core User Secrets** and is not committed to source control.

### Apply Migrations

From the solution directory, run:

```powershell
dotnet ef database update --project CustomerStatement.Infrastructure --startup-project CustomerStatement.Api
```

This creates or updates the `CustomerStatementDb` database using the Entity Framework Core migrations included in the Infrastructure project.

## How to Run

### Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or Visual Studio Code

### Steps

1. Clone the repository.
2. Configure the database connection using ASP.NET Core User Secrets.
3. Apply the Entity Framework Core migrations.
4. Build the solution.
5. Run the API project.

The Swagger UI will be available when the application starts.

## Validation and Error Handling

The API uses **FluentValidation** with a MediatR pipeline behavior to validate incoming commands and queries before they reach their handlers.

Validation includes:

- Customer ID must be greater than zero.
- Statement months must use the first day of the month.
- The `FromMonth` filter must be less than or equal to `ToMonth`.

The API also uses a global exception handler to return consistent HTTP responses.

### Example Validation Response

```json
{
  "type": "https://tools.ietf.org/html/rfc9110",
  "title": "Validation failed.",
  "status": 400,
  "errors": {
    "CustomerId": [
      "CustomerId must be greater than 0."
    ]
  }
}
```

### Example Not Found Response

```json
{
  "title": "Resource not found.",
  "status": 404,
  "detail": "Customer not found."
}
```

## Email Service

The application uses an `IEmailService` abstraction so that email delivery is separated from the application/business logic.

The current implementation is a simple development implementation that writes the generated email to the console instead of sending through a real SMTP provider.

This allows the application to demonstrate the email workflow without requiring external email credentials.

### Email Flow

```text
GenerateMonthlyStatementCommand
            |
            v
GenerateMonthlyStatementCommandHandler
            |
            +-- Generate statement
            |
            +-- Calculate balances
            |
            +-- Associate transactions
            |
            +-- Save statement
            |
            v
       IEmailService
            |
            v
       EmailService
            |
            v
      Console Output
```

For a production environment, `IEmailService` could be replaced with an implementation using SMTP, SendGrid, Amazon SES, or another email provider without changing the application/business logic.