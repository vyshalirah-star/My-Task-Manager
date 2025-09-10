# TaskManager API

TaskManager API is a .NET 9 Web API project for managing tasks and categories.  
It allows creating, updating, retrieving, and deleting tasks and categories, with support for priorities, archival status, and category-task relationships.

---


## Technologies

- .NET 9  
- ASP.NET Core Web API  
- Entity Framework Core (Code First)  
- SQLite 
- SQLite Viewer
- Swashbuckle (Swagger/OpenAPI) for API documentation  

---

## Features

- Manage **Tasks** with fields: Title, Description, Priority, DueDate, IsCompleted, IsArchived, CreatedAt, UpdatedAt, Tags.  
- Manage **Categories** with fields: Name, Description.  
- Archive and complete tasks.  
- Filter tasks by category, task ID, and active/archived status.
- Task Operations: Retrieve all tasks, get a task by ID, create a new task, and delete a task. 
- Category Operations:-Retrieve all categories, create a new category. 
                      - Cascade deletion: deleting a category deletes all associated tasks.  
- Due Date Tracking: Tasks include due dates and support automated reminders or notifications, which can be tested via the console or Swagger UI.
- Supports scalar and list responses for easy consumption.  

---

## Getting Started

### Clone the repository

```bash
git clone <your-repo-url>
cd TaskManager
# My-Task-Manager