Readme

# Brainstation Starcoder-24 Demo Project

Welcome to the Brainstation Starcoder-24 demo project github repository. Here, I am going to explain the codebase, how setup and run.

This is a task management web application, where uses can register and login to their individual accounts to read, create, update and delete their tasks. For the backend, I have used ASP.NET Core Web API, Postgresql database and Entity Framework ORM to read, create, update or delete data in the database. For the frontend, I have used ReactJS and Tailwind CSS. Here is complete list of the tech stack, I have used.

|     |     |
| --- | --- |
| Backend | ASP.NET Core Web API |
| Database | Postgresql |
| ORM | Entity Framework |
| Authentication | JWT Tokens |
| Password Hashing | Bcrypt.Net |
| Unit Testing | XUnit |
| Frontend | ReactJS |
| CSS Library | Tailwind CSS |

&nbsp;

# Directory Structure

Here is the directory structure for the project to help the viewer navigate through the project:

- **StarCoderProject** (Root Directory)  
    - **Backend.Tests** (XUnit Test Project)
        - **Backend.Tests.csproj** (CS Project File for Backend.Test Project)
        - **UnitTest1.cs** (Contains Testcases)
        - **Usings.cs** (Contains Global Import Statements)
    - **Backend** (ASP.NET Core Web API Project)
        - **Controllers** (Contains Controller Classes)
            - **AuthController.cs**
            - **TaskController.cs**
            - **WeatherForecastController.cs** (Optional, not necessary)
        - **Dtos** (Contains Data Transfer Object Classes)  
            - **LoginCredentialsDto.cs**
            - **LoginResponseDto.cs**
            - **RegistrationDto.cs**
            - **TaskDto.cs**
        - **Migrations** (Contains Migration Files for Entity Framework ORM)
        - **Models** (Contains Model Classes)
            - **Profile.cs** (Contains Profile Model Class)
            - **ProfileType.cs** (Contains ProfileTypes Enum)
            - **RefreshToken.cs** (Contains RefreshToken Model Class)
            - **Task.cs** (Contains TaskModel Model Class)
            - **TaskStatus.cs** (Contains TaskStatus Enum)
            - **User.cs** (Contains User Model Class)
        - **Properties** (Properties Used by ASP.NET Core Web API)
        - **Services** (Contains Service Classes)
            - **AuthService.cs** (Contains Service for Authentication)
            - **StarDB.cs** (Contains StarDB DbContext Class)
            - **TaskService.cs** (Contains Service for Task Management)
        - **Validators**  
            - **PasswordValidatorAttribute.cs** (Contains PasswordValidatorAttribute Class for Validating Password Field in DTO Classes)
        - **Backend.csproj** (CS Project for Backend)
        - **Program.cs** (Entry Point for the Project)
        - **WeatherForecast.cs** (Optional, Not Used in the Project)
        - **appsettings.Development.json** (Project Configuration)
        - **appsettings.json** (Project Configuration, It Contains JWT Secret for Signing JWT Token and Connection String for Postgresql)
    - **frontend** (ReactJS Frontend Project)
        - **public**
        - **src** (Contains Source Codes)
            - **asset** (Contains Images and Other Assets)
            - **components** (Contains tsx Files for Different Components)  
                - **authentication.tsx** (Contains AuthenticationView, Login and Register Components)
                - **dashboard.tsx** (Contains Dashboard Component)
                - **task.tsx** (Contains TaskListItem Component)
            - **App.css** (Optional, Not Used)
            - **App.tsx** (Contains App Component)
            - **index.css** (Contain Tailwind Directives and Some Global Stylings)
            - **main.tsx** (Entry Point to the React App)
            - **model.ts** (Contains Interfaces equivalent Backend DTO Classes)
        - **.env** (Dotenv File Containing Environment Variables)
        - **package.json** and **package-lock.json** (Package and Package Lock Files Used by Node and NPM)
        - **tailwind.config.js** and **postcss.config.js** (Configuration Files for Tailwind CSS)
        - **\[Other Configuration Files\]** (Used by Node, Typescript and Vite)
    - **.gitignore** (for git to ignore tracking certain files)
    - **StarCoderProject.sln** (Solution file for Backend and Backend.Test)
    - **makefile** (to run the backend project, optional)
    - **registration.json** (optional)

&nbsp;

# API Documentation

This section contains the list of Rest API endpoint and payload structure for each of the endpoints.

## Authentication

- ******Admin Registration Endpoint (Only Included for Testing Purpose, Can be Accessed through Swagger)******
    - **URL:** `/api/register/admin`
    - **Method:** `POST`
    - **Payload:** 
        
        ```
        {
          "firstName": "string",
          "lastname": "string",
          "email": "user@example.com",
          "username": "string",
          "password": "string"
        }
        ```
        
    - **Authentication Necessary:** No
- **Registration Endpoint**
    - **URL:** `/api/register`
    - **Method:** `POST`
    - **Payload:** 
        
        ```json
        {
          "firstName": "string",
          "lastname": "string",
          "email": "user@example.com",
          "username": "string",
          "password": "string"
        }
        ```
        
    - **Authentication Necessary:** No
- **Login Endpoint**  
    - **URL:** `/api/login`
    - **Method:** `POST`
    - **Payload:** 
        
        ```json
        {
          "username": "string",
          "password": "string"
        }
        ```
        
    - **Authentication Necessary:** No
- **Refresh Token Endpoint**  
    - **URL:** `/api/auth/refresh`
    - **Method:** `GET`
    - **Payload:** No Payload
    - **Authentication Necessary:** No
- **Logout Endpoint**  
    - **URL:** `/api/auth/logout`
    - **Method:** `GET`
    - **Payload:** No Payload
    - **Authentication Necessary:** Yes

## Task Management

- **Task Retrieval**
    - **URL:** `/api/tasks`
    - **Method:** `GET`
    - **Payload:** No Payload
    - **Authentication Necessary:** Yes (As user, only the tasks created by the user will retrieved and as an admin, all the tasks created by all the users will be retrieved)
- **Task Creation**
    - **URL:** `/api/tasks`
    - **Method:** `POST`
    - **Payload:** 
        
        ```json
        {
          "title": "string",
          "description": "string"
        }
        ```
        
    - **Authentication Necessary:** Yes (as user; admin users cannot create tasks)
- **Task Update**
    - **URL:** `/api/tasks/{taskId}`
    - **Method:** `PUT`
    - **Payload:** 
        
        ```json
        {
          "title": "string",
          "description": "string",
          "taskStatus": 0
        }
        ```
        
    - **Authentication Necessary:** Yes (Users can only update the task they created and admins can update any task)
- **Task Deletion**
    - **URL:** `/api/tasks/{taskId}`
    - **Method:** `DELETE`
    - **Payload:** No Payload
    - ****Authentication Necessary:**** Yes (Users can only delete the task they created and admins can delete any task)
