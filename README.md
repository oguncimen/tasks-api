# Tasks API

A simple .NET 8 Web API with EF Core + SQLite.  
Provides CRUD endpoints for managing tasks.

## Run locally
```bash
dotnet ef database update
dotnet run --urls http://localhost:5178

Endpoints
GET /api/tasks
POST /api/tasks
PUT /api/tasks/{id}
DELETE /api/tasks/{id}

