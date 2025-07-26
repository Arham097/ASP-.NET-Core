# ASP.NET Core 

This workspace contains multiple ASP.NET Core Concepts, each demonstrating different aspects of web application development. Below is an overview of the concepts and functionalities.

## Projects

### 1. Basic Requests

- **Purpose**: Demonstrates handling basic HTTP requests.
- **Key Files**:
  - `Program.cs`: Contains the main application logic.
  - `wwwroot/`: Static files like CSS, JavaScript, and libraries.

### 2. CRUD Operations with Proper Validation

- **Purpose**: Implements CRUD operations with proper validation.
- **Key Features**:
  - Custom validation attributes (`Employee_EnsureSalary`).
  - Endpoints for managing employee data.
  - Repository pattern for data management.
- **Key Files**:
  - `Program.cs`: Sets up middleware and endpoints.
  - `Endpoints/Employee_Endpoints.cs`: Maps endpoints for CRUD operations.
  - `Models/Employee.cs`: Defines the Employee model.
  - `Models/EmployeesRespository.cs`: Manages employee data.

### 3. Endpoints Handling

- **Purpose**: Focuses on handling endpoints efficiently.
- **Key Files**:
  - `Program.cs`: Configures the application.
  - `Models/`: Contains models like `Employee` and `EmployeesRespository`.

### 4. Middleware Pipeline

- **Purpose**: Demonstrates the use of middleware in ASP.NET Core.
- **Key Features**:
  - Custom middleware (`MyCustomMiddleware`, `MyCustomExceptionHandler`).
- **Key Files**:
  - `Custom_Middleware/`: Contains custom middleware implementations.
  - `Program.cs`: Configures the middleware pipeline.

### 5. Model Binding

- **Purpose**: Explores model binding in ASP.NET Core.
- **Key Files**:
  - `Models/Employee.cs`: Defines the Employee model.
  - `Models/EmployeesRespository.cs`: Manages employee data.
  - `Program.cs`: Configures the application.

### 6. Model Validation

- **Purpose**: Demonstrates model validation techniques.
- **Key Features**:
  - Custom validation attributes (`Employee_EnsureSalary`).
- **Key Files**:
  - `Models/Employee.cs`: Defines the Employee model.
  - `Employee_EnsureSalary.cs`: Implements custom validation.
  - `Program.cs`: Configures the application.

## How to Run

1. Open the desired project folder.
2. Build the project using Visual Studio or the .NET CLI.
3. Run the application.
4. Use tools like Postman or curl to interact with the endpoints.

## License

This workspace is licensed under the MIT License.
