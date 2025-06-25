# 🚗 AutoRegistry API

A clean and well-structured ASP.NET Core 9 Web API project that manages **vehicle inspections**, following **Test-Driven Development (TDD)** and clean architecture principles.

---

## 🧰 Tech Stack

- **ASP.NET Core 9**
- **xUnit** for testing
- **Moq** for mocking
- **Swagger (Swashbuckle)** for API documentation
- **In-Memory Repository** for data storage

---

## 📦 Project Structure
AutoRegistry.sln
├── AutoRegistry.API        # API Layer - Controllers, Program.cs, Swagger
├── AutoRegistry.Core       # Core Logic - Interfaces, Models, Services
├── AutoRegistry.Tests      # xUnit Test Project

---

## 📚 Features

✅ Clean architecture  
✅ Test-Driven Development (TDD)  
✅ CRUD for vehicle inspections  
✅ In-memory repository pattern  
✅ Swagger UI for documentation  
✅ xUnit test coverage  
✅ Moq for mocking dependencies  

---

## 📖 API Endpoints

All endpoints are available and testable via Swagger UI:

📍 `https://localhost:5001/swagger`

Examples:
- `GET /api/vehicles`
- `POST /api/vehicles`
- `PUT /api/vehicles/{id}`
- `DELETE /api/vehicles/{id}`

---

## 📊 API Documentation with Swagger

Swagger is enabled with `Swashbuckle.AspNetCore`. Access it by running:

```bash
dotnet run --project AutoRegistry.API
🧪 Running Tests

All unit tests are written with xUnit and Moq.

To run all tests:

dotnet test







