REST API Solution Documentation Overview

This solution is a RESTful API built using ASP.NET Core that manages users, ships, and ports. 
The API allows for CRUD operations on users and ships, and includes functionality to find the closest port to a given ship 
and calculate the estimated arrival time based on the ship's velocity and geolocation. 
The data is stored in a local MDF database file.

Technologies Used
- Language: C#
- Framework: ASP.NET Core
- Database: SQL Server with a local MDF file
- Testing: xUnit
- Validation: FluentValidation
- Containerization: Docker

Solution Architecture

Eentities
- User
   -    Id (int): Unique identifier for the user.
   -    Name (string): Name of the user.
   -    Role (string): Role of the user.
   -    Relationships: A user can be assigned to multiple ships (many-to-many relationship).
-    Ship
     - Id   (int): Unique identifier for the ship.
     - Name (string): Name of the ship.
     - Longitude (double): Current longitude of the ship.
     - Latitude (double): Current latitude of the ship.
     - Velocity (double): Current velocity of the ship (in knots).
     - Relationships: A ship can be assigned to multiple users (many-to-many relationship).
-    Port
     - Id (int): Unique identifier for the port.
     - Name (string): Name of the port.
     - Longitude (double): Longitude of the port.
     - Latitude (double): Latitude of the port.
     - Relationships: Ports are not directly related to users or ships, but are used to calculate proximity to ships.       
  

Database Structure
   - Tables:
        - Users
        - Ships
        - Ports
        - UserShips
   - Relationships:
        - Many-to-many relationship between Users and Ships via the UserShips join table.


Database Creation
   - Approach: Using code first approach, this enable the code to be created prior to database. Using the migration feature from the framework that will allow automatic table creation.
   - Maintenance: Each time there is a change on database i/e table structurer change or new addition table after update on the solution model can trigger the migration to update the database

 
Command used:
   - Add-Migration “nameformigrationfile” this command will create a migration file based on the latest models.
   - Update-Database this command will execute the migration file to the database 


API Endpoints
   - Ports Management:
        - GET /api/Ports/GetAllPorts: Retrieves a list of all ports.
        - POST /api/Ports/AddPort: Adds a new port.
        - POST /api/Ports/UpdatePorts: Updates an existing port.

   - Ships Management:
        - GET /api/Ships/GetShips: Retrieves all ships.
        - GET /api/Ships/GetUnassingedShips: Retrieves all ships.
        - POST /api/Ships/CreateShip: Creates a new ship.POST /api/Ships/AssignShipToUser: Assigns a ship to a user.
        - POST /api/Ships/GetShipBasedOnUserId: Get ship based on user id.
        - POST /api/Ships/UpdateVelocity: Updates the velocity of a ship.
        - POST /api/Ships/GetClosestPort: Calculates and retrieves the closest port to a given ship.
 
   - Users Management:
        - GET /api/Users/GetUsers: Retrieves all users.
        - POST /api/Users/CreateUser: Creates a new user.
        - POST /api/Users/AssignUserToShips: Assigns ships to a user.
 

Validation
   - FluentValidation is used to validate inputs for all APIs.
   - Validators are registered in the Startup.cs and handle validation for entities like User, Ship, and Port.


Error Handling
   - Global error handling is implemented to catch and return meaningful error messages.
   - Specific error handling is implemented in the service layer to deal with scenarios like null values, missing entities, or invalid input data.


Testing
   - xUnit Tests
   - Unit tests are created using xUnit to cover all CRUD operations for users, ships, and ports, as well as the proximity calculation feature.
   - Each test validates both successful and failure scenarios.


Integration Testing
   - Integration tests are included to ensure that the APIs work together correctly.
   - Tests include scenarios for all major functionalities like user-ship assignments and proximity calculations.


Dockerization
   - Dockerfile is included to containerize the application.
   - The Docker image uses .NET 8 as the base image.
   - The application is set up to run in a container with a connection string pointing to a local MDF file.


Pre testing steps
1. Get master branch to your local or testing environment

2. Run Database Migrations
   - Open Command Line or Terminal from visual studio
   - Navigate to the directory where your .NET Core project is located. "..\AEBackendCodeChallenge\AEBackendCodeChallenge"
   - Run the Migration Command:
     - Use the following command to apply the migrations: "dotnet ef database update"
     - Ensure that the dotnet-ef tool is installed. If it’s not, you can install it globally using: "dotnet tool install --global dotnet-ef"
   - Verify the migration by checking the database for updated schema
     - From Visual Studio open SQL Server Object Explorer, this can be open y clicking View --> SQL Server Object Explorer
     - On the SQL Server Object Explorer navigate to SQL Server --> Localdb --> Databases --> ShipDb --> Tables --> Port
     - If the migration success, on the port table when right click and select show data it will show the port data that has been seed on the code.
      
3. Build the Docker Image 
   - After the database is up-to-date, the next step is to build the Docker image for the application.
   - Make sure the docker already installed on the testing machine
   - Navigate to the Project Directory "..\AEBackendCodeChallenge\AEBackendCodeChallenge":
   - Run the following command to build the Docker image: "docker build -t aebackendcodechallenge ."
   - Run the Docker Container : "docker run -d -p 7279:80 --name testcontainer aebackendcodechallenge"
  
Testing
   Please look in to "Testing Script.docx" for reference
   
