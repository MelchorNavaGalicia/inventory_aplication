# Inventory Application API

RESTful API for managing products, categories, inventory movements,
and users. Demonstrates best practices such as SOLID principles,
Clean Code, Repository Pattern, CQRS with MediatR, JWT authentication,
and is fully dockerized using SQL Server.

---

## Technologies

- .NET 10  
- ASP.NET Core Web API  
- Entity Framework Core  
- MediatR (CQRS)  
- SQL Server 2022  
- JWT Authentication  
- Docker & Docker Compose  
- Swagger / OpenAPI  
- xUnit + Moq (Unit Testing)

---

## Running with Docker Compose

### Prerequisites

- Docker Desktop  
- Docker Compose enabled  

### Start the application

From the repository root, run:

docker compose up --build

### Services started

- SQL Server -> Port 1433  
- API -> Port 5000  

### Available URLs

- API: http://localhost:5000  
- Swagger: http://localhost:5000/swagger  

---
## Healt Check
The api includes a heatlh check endpoind to verify the service availability.

| Method | Endpoind | Description |
|--------|----------|-------------| 
| Get | /health | Verifies that API is running |

**Note:** The health check endpoind is not part of Swagger, as it is designed for infrastructure and monitoring usage.

---

## Authentication

The API uses JWT (JSON Web Tokens).

### Public Endpoints

- POST /api/Auth/register -> Register a new user  
- POST /api/Auth/login -> Login  

### Protected Endpoints

All other endpoints require the following header:

Authorization: Bearer {token}

---

## Main Endpoints

### Products

| Method | Endpoint | Description |  
|--------|----------|-------------|  
| GET | /api/Product | Get all products |  
| GET | /api/Product/{id} | Get product by ID |  
| GET | /api/Product/filter | Filter products |  
| POST | /api/Product | Create product |  
| PUT | /api/Product/{id} | Update product |  
| DELETE | /api/Product/{id} | Delete product |  

---

### Categories

| Method | Endpoint | Description |  
|--------|----------|-------------|  
| GET | /api/Category | Get all categories |  
| GET | /api/Category/{id} | Get category by ID |  
| GET | /api/Category/filter | Filter categories |  
| POST | /api/Category | Create category |  
| PUT | /api/Category/{id} | Update category |  
| DELETE | /api/Category/{id} | Delete category |  

---

### Inventory Movements

| Method | Endpoint | Description |  
|--------|----------|-------------|  
| GET | /api/InventoryMovement | Get all movements |  
| GET | /api/InventoryMovement/filter | Filter movements |  
| POST | /api/InventoryMovement | Create movement |  

---

### Users

| Method | Endpoint | Description |  
|--------|----------|-------------|  
| GET | /api/User | Get all users |  
| DELETE | /api/User/{id} | Delete user |  

---

## Unit Tests

Unit tests cover:

- Controllers  
- Command & Query Handlers  
- Repositories  
- Authentication Services  

Run tests:

dotnet test

---

## Best Practices Applied

- SOLID principles  
- Clean Architecture  
- Repository Pattern  
- CQRS with MediatR  
- Clear separation of concerns  
- DTOs and Result wrappers  
- Consistent error handling  

---

## Additional Notes

- The database is automatically created via Docker.  
- No FOREIGN KEYS are used in the schema (as required).  
- Swagger is enabled for easy API testing and evaluation.

