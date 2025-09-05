# 📝 ToDoApp

A simple **ToDo application** built with modern technologies, demonstrating clean architecture, repository pattern, and integration with Docker and Testcontainers.  

---

## 📌 Features

- **Task entity (`Task`)** with full task management API:
  - Get all tasks
  - Get all uncompleted tasks
  - Get task
  - Get incoming ToDo(for today/ next day/ current week) 
  - Create tasks
  - Update tasks
  - Delete tasks
  - Mark task as **completed**
  - Set Todo percent complete

- Automatic initial database migrations via `DatabaseInitializer`
- Integration with **MySQL** and **Seq** for logging

---

## ⚙️ Tech Stack

- **.NET 9.0** – Web API
- **MySQL** – relational database
- **Entity Framework Core** – ORM
- **xUnit** – unit & integration tests
- **Testcontainers.MySql** – integration testing of repositories
- **Seq** – structured logging (running in Docker)

---

## 🐳 Dockerized Setup

Both **MySQL** and **Seq** run inside Docker containers.  
The application uses **environment variables** for the database connection string and root password.  

- Default values are provided in `docker-compose.yml`
- You can override them by creating a `.env` file inside the `/infra` folder:

```env
DB_CONNECTION_STRING=Server=localhost;Port=3306;Database=todo_db;User=root;Password=P@ssw0rd;
MYSQL_ROOT_PASSWORD=P@ssw0rd
```

## ▶️ Getting Started

### Run infrastructure in /infra (MySQL + Seq)

```bash
docker compose up -d
```

This is the only script you need.
The database and Seq will start automatically.
Migrations are applied at application startup by the DatabaseInitializer.

## 🧪 Testing

- **Unit Tests**: Written using xUnit

- **Integration Tests**: Repository layer tested with Testcontainers.MySql

    - Spawns a temporary MySQL container for real DB interaction
    - Ensures production-like conditions in tests

```bash
dotnet test
```

## 📊 Logging with Seq

Seq provides structured logs for debugging and monitoring.
It is available at:

👉 http://localhost:5341