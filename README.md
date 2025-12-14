# SmartStock

An enterprise-grade inventory management API with AI-powered demand forecasting and automated restock alerts.

---

## Key Features

* **JWT Authentication & RBAC** – Secure role-based access control (Admin / Staff).
* **Inventory Management** – Full CRUD operations with soft delete and restore support.
* **Stock Transactions** – Real-time stock-in and stock-out with complete transaction history.
* **Audit Trail System** – Automatic logging of critical operations with user context.
* **Auto Re-stock Alerts** – Background service checks stock levels hourly and generates alerts.
* **AI Demand Forecasting** – Pure C# linear regression implementation for predicting future demand.
* **Rate Limiting Middleware** – Protection against abuse with role-based request limits.
* **Swagger Documentation** – Interactive API documentation with examples and schemas.


---

## Tech Stack

### Backend

* **ASP.NET Core 8.0** – Web API framework
* **Entity Framework Core** – ORM for database operations
* **Microsoft Identity** – Authentication and authorization
* **AutoMapper** – Object-to-object mapping
* **Swashbuckle** – Swagger / OpenAPI documentation

### Database

* **SQL Server** – Relational database
* **Code-First Migrations** – Schema versioning and management

### Architecture

* **Clean Architecture** – Clear separation of concerns
* **Repository Pattern** – Data access abstraction
* **Dependency Injection** – Loose coupling between components
* **Middleware Pipeline** – Centralized HTTP request handling

---

## Installation

### Prerequisites

* .NET 8.0 SDK
* SQL Server 2022 Express or LocalDB
* Visual Studio 2022 or VS Code (with C# extension)

### Setup Steps

1. Clone the repository:

```bash
git clone https://github.com/your-username/SmartStock.git
cd SmartStock
```

2. Create the configuration file from the template:

```bash
cp appsettings.Development.json appsettings.json
```

3. Generate a secure JWT key (minimum 32 characters) and update `appsettings.json`:

```json
"Jwt": {
  "Key": "YOUR_SECURE_32_CHAR_JWT_KEY",
  "Issuer": "SmartStock",
  "Audience": "SmartStockUsers"
}
```

4. Update the database connection string in `appsettings.json`.

5. Run database migrations:

```bash
dotnet ef database update
```

6. Start the application:

```bash
dotnet run
```

7. Access Swagger API documentation:

```
https://localhost:5001/swagger
```


## Project Structure

```bash
SmartStock/
├── Controllers/           # RESTful API controllers
├── Data/                  # DbContext & database configuration
├── Dtos/                  # Request / Response DTOs
├── Exceptions/            # Custom exception definitions
├── Extensions/            # Application & service extensions
├── Helpers/               # Utility and helper classes
├── Interfaces/            # Service & repository contracts
├── Mappers/               # AutoMapper profiles
├── Middlewares/           # Custom HTTP middlewares
├── Migrations/            # EF Core migrations
├── Models/                # Domain entities
├── Repositories/          # Data access implementations
├── Services/              # Business logic layer
├── appsettings.Example.json # Environment configuration template
├── Program.cs             # Application entry point
├── SmartStockAI.csproj    # Project configuration
├── SmartStockAI.http      # HTTP request samples
└── SmartStockAI.sln       # Solution file
```
## API Usage Examples

### Authentication – Login

  ```http
    POST /api/auth/login
    Content-Type: application/json

    {
      "email": "admin@smartstock.ai",
      "password": "Admin123!"
    }
  ```

### Create Item (Admin Only)

```json
POST /api/items
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Gaming Laptop",
  "categoryId": "f4637157-d4b9-4cc5-9244-88cec2b6f037",
  "stock": 15,
  "price": 15000000.00,
  "minimumThreshold": 5
}
```

### AI Demand Forecasting

```bash
GET /api/forecast/f4637157-d4b9-4cc5-9244-88cec2b6f037?daysAhead=7
Authorization: Bearer <token>
```

### Stock Transaction – Stock In

```json
POST /api/stock/in
Authorization: Bearer <token>
Content-Type: application/json

{
  "itemId": "f4637157-d4b9-4cc5-9244-88cec2b6f037",
  "quantity": 10
}
```

## License

Distributed under the **MIT License**. See `LICENSE` for more information.