# ToDoApp

## Configuration

This project uses **environment variables** to configure the database connection string and MySQL root password. By default, a default connection string and password are used.  

If you want to override them, create a `.env` file in the `/infra` directory with your custom values. For example:

```dotenv
DB_CONNECTION_STRING=Server=localhost;Port=3306;Database=todo_db;User=root;Password=P@ssw0rd;
MYSQL_ROOT_PASSWORD=P@ssw0rd
