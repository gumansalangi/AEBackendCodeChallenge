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


  
Testing API Endpoints    
•	Ports Management:
   - GET /api/Ports/GetAllPorts: Retrieves a list of all ports.
   - Request JSON
   - Result JSON
     [
       {
           "name": "Los Angeles Port",
           "latitude": 34.0522,
           "longitude": -118.2437
       },
       {
           "name": "New York Port",
           "latitude": 40.7128,
           "longitude": -74.006
       },
       {
           "name": "London Port",
           "latitude": 51.5074,
           "longitude": -0.1278
       }
   ]

•	Ships Management:
   - GET /api/Ships/GetShips: Retrieves all ships
   - Request JSON
   - Result JSON
   - [
    {
        "shipId": 3,
        "name": "RG Shipping Line",
        "shipCode": "RGSL001",
        "latitude": -6.13333,
        "longitude": 106.9,
        "velocity": 4,
        "users": [
            {
                "userId": 1,
                "userName": "Rudolf",
                "role": "Admin"
            },
            {
                "userId": 2,
                "userName": "Jhon",
                "role": "User"
            },
            {
                "userId": 2003,
                "userName": "Jeremy",
                "role": "Admin"
            }
        ]
    },
    {
        "shipId": 4,
        "name": "RG Shipping Line 02",
        "shipCode": "RGSL002",
        "latitude": 3,
        "longitude": 101.3999984,
        "velocity": 5,
        "users": [
            {
                "userId": 2,
                "userName": "Jhon",
                "role": "User"
            }
        ]
    },
    {
        "shipId": 1003,
        "name": "RG Shipping Line 02",
        "shipCode": "RGSL002",
        "latitude": 3,
        "longitude": 101.3999984,
        "velocity": 5,
        "users": []
    },
    {
        "shipId": 1004,
        "name": "RG Shipping Line 005",
        "shipCode": "RGSL005",
        "latitude": 53.34609,
        "longitude": -6.20831,
        "velocity": 3,
        "users": []
    },
    {
        "shipId": 2003,
        "name": "RG Shipping Line 007",
        "shipCode": "RGSL007",
        "latitude": 56.319832054,
        "longitude": -133.603997584,
        "velocity": 3,
        "users": [
            {
                "userId": 1003,
                "userName": "Sidney",
                "role": "Admin"
            },
            {
                "userId": 2002,
                "userName": "Albert",
                "role": "Admin"
            }
        ]
    },
    {
        "shipId": 2004,
        "name": "RG Shipping Line 008",
        "shipCode": "RGSL008",
        "latitude": 56.319832054,
        "longitude": -133.603997584,
        "velocity": 10,
        "users": [
            {
                "userId": 2003,
                "userName": "Jeremy",
                "role": "Admin"
            }
        ]
    }
]


o	GET /api/Ships/GetUnassingedShips:
   - Request JSON:
   - Return JSON:
   - [
    {
        "shipId": 1003,
        "name": "RG Shipping Line 02",
        "shipCode": "RGSL002",
        "latitude": 3,
        "longitude": 101.3999984,
        "velocity": 5,
        "users": []
    },
    {
        "shipId": 1004,
        "name": "RG Shipping Line 005",
        "shipCode": "RGSL005",
        "latitude": 53.34609,
        "longitude": -6.20831,
        "velocity": 3,
        "users": []
    }
]


o	POST /api/Ships/CreateShip: Creates a new ship.
   - With users
   - Request JSON:
   - {
  "name": "RG Shipping Line 009",
  "shipId": "RGSL009",
  "latitude": 56.319832054,
  "longitude": -133.603997584,
  "velocity": 3,
  "users": [
    {
      "userId": 2003,
      "userName": "Jeremy",
      "role": "Admin"
    }
  ]
}
   - Return JSON:
   - {
    "shipId": 3003,
    "name": "RG Shipping Line 009",
              "shipId": "RGSL009",
    "shipCode": "RGSL009",
    "latitude": 56.319832054,
    "longitude": -133.603997584,
    "velocity": 3,
    "users": [
        {
            "userId": 2003,
            "userName": "Jeremy",
            "role": "Admin"
        }
    ]
}

Without user
- Request:
- {
  "name": "RG Shipping Line 011",
  "shipId": "RGSL011",
  "shipCode": "RGSL011",
  "latitude": 56.319832054,
  "longitude": -133.603997584,
  "velocity": 3,
  "users": []
}
- Return:
- {
    "shipId": 3004,
    "name": "RG Shipping Line 011",
    "shipCode": "RGSL011",
    "latitude": 56.319832054,
    "longitude": -133.603997584,
    "velocity": 3,
    "users": []
}


o	POST /api/Ships/AssignShipToUser: Assigns a ship to a user.
- Request JSON
- {
  "shipId": 1004,
  "userIds": [
   1003
  ]
}
- Return JSON
- {
    "shipId": 1004,
    "name": "RG Shipping Line 005",
    "shipCode": "RGSL005",
    "latitude": 53.34609,
    "longitude": -6.20831,
    "velocity": 3,
    "users": [
        {
            "userId": 1003,
            "userName": "Sidney",
            "role": "Admin"
        }
    ]
}


o	POST /api/Ships/GetShipBasedOnUserId: Get ship based on user id.
- Request: “https://localhost:7279/api/Ships/GetShipsBasedOnUserID?id=1”
- Return:
- [
    {
        "shipId": 3,
        "name": "RG Shipping Line",
        "shipCode": "RGSL001",
        "latitude": -6.13333,
        "longitude": 106.9,
        "velocity": 4,
        "users": [
            {
                "userId": 1,
                "userName": "Rudolf",
                "role": "Admin"
            }
        ]
    }
]



o	POST /api/Ships/UpdateVelocity: Updates the velocity of a ship.
- Request:
- {
  "shipId": "2004",
  "velocity": 10
}
- Response:
- {
    "shipId": 2004,
    "name": "RG Shipping Line 008",
    "shipCode": "RGSL008",
    "latitude": 56.319832054,
    "longitude": -133.603997584,
    "velocity": 10,
    "users": [
        {
            "userId": 2003,
            "userName": "Jeremy",
            "role": "Admin"
        }
    ]
}



o	POST /api/Ships/GetClosestPort: Calculates and retrieves the closest port to a given ship.
- Request:
- {
  "id": 2004
}
- Return:
- {
    "portInformation": {
        "portInfo": {
            "name": "London Port",
            "latitude": 51.5074,
            "longitude": -0.1278
        },
        "estimatedDistance": "1438,34 Nautical Miles",
        "estimatedArrivalTime": "Estimated Arrival Time 6 days and 0 hours."
    },
    "shipDetailInfo": {
        "shipId": 2004,
        "name": "RG Shipping Line 008",
        "shipCode": "RGSL008",
        "latitude": 56.319832054,
        "longitude": -133.603997584,
        "velocity": 10,
        "users": [
            {
                "userId": 2003,
                "userName": "Jeremy",
                "role": "Admin"
            }
        ]
    }
}



•	Users Management:
o	GET /api/Users/GetUsers: Retrieves all users.
- Request:
- Return:
- [
  {
    "userId": 1,
    "userName": "Rudolf",
    "role": "Admin",
    "ships": [
      {
        "shipId": 3,
        "shipCode": null,
        "name": "RG Shipping Line",
        "latitude": -6.13333,
        "longitude": 106.9,
        "velocity": 4
      }
    ]
  },
  {
    "userId": 2,
    "userName": "Jhon",
    "role": "User",
    "ships": [
      {
        "shipId": 3,
        "shipCode": null,
        "name": "RG Shipping Line",
        "latitude": -6.13333,
        "longitude": 106.9,
        "velocity": 4
      },
      {
        "shipId": 4,
        "shipCode": null,
        "name": "RG Shipping Line 02",
        "latitude": 3,
        "longitude": 101.3999984,
        "velocity": 5
      }
    ]
  },
  {
    "userId": 1002,
    "userName": "Doe",
    "role": "Admin",
    "ships": []
  },
  {
    "userId": 1003,
    "userName": "Sidney",
    "role": "Admin",
    "ships": [
      {
        "shipId": 2003,
        "shipCode": null,
        "name": "RG Shipping Line 007",
        "latitude": 56.319832054,
        "longitude": -133.603997584,
        "velocity": 3
      }
    ]
  },
  {
    "userId": 2002,
    "userName": "Albert",
    "role": "Admin",
    "ships": [
      {
        "shipId": 2003,
        "shipCode": null,
        "name": "RG Shipping Line 007",
        "latitude": 56.319832054,
        "longitude": -133.603997584,
        "velocity": 3
      }
    ]
  },
  {
    "userId": 2003,
    "userName": "Jeremy",
    "role": "Admin",
    "ships": [
      {
        "shipId": 3,
        "shipCode": null,
        "name": "RG Shipping Line",
        "latitude": -6.13333,
        "longitude": 106.9,
        "velocity": 4
      },
      {
        "shipId": 2004,
        "shipCode": null,
        "name": "RG Shipping Line 008",
        "latitude": 56.319832054,
        "longitude": -133.603997584,
        "velocity": 10
      },
      {
        "shipId": 3003,
        "shipCode": null,
        "name": "RG Shipping Line 009",
        "latitude": 56.319832054,
        "longitude": -133.603997584,
        "velocity": 3
      }
    ]
  }
]



o	POST /api/Users/CreateUser: Creates a new user.
- Request:
- {
  "userName": "Bosch",
  "role": "Admin",
  "ships": [
    {
        "shipId": 3,
        "name": "RG Shipping Line",
        "ShipCode": "",
        "latitude": -6.13333,
        "longitude": 106.9,
        "velocity": 3
    }
  ]
}
- Return
- {
    "userId": 3002,
    "userName": "Bosch",
    "role": "Admin",
    "ships": [
        {
            "shipId": 3,
            "shipCode": null,
            "name": "RG Shipping Line",
            "latitude": -6.13333,
            "longitude": 106.9,
            "velocity": 4
        }
    ]
}

- Create user with no ship
  - Request:
  - {
   "userName": "Tate",
   "role": "Admin",
   "ships": []
}
   - Return:
   - {
    "userId": 3003,
    "userName": "Tate",
    "role": "Admin",
    "ships": []
}


o	POST /api/Users/AssignUserToShips: Assigns ships to a user.
- Request:
- {
  "usersId": 2,
  "shipIds": [
    3,4
  ]
}

- Return:
- {
    "userId": 2,
    "userName": "Jhon",
    "role": "User",
    "ships": [
        {
            "shipId": 3,
            "shipCode": null,
            "name": "RG Shipping Line",
            "latitude": -6.13333,
            "longitude": 106.9,
            "velocity": 4
        },
        {
            "shipId": 4,
            "shipCode": null,
            "name": "RG Shipping Line 02",
            "latitude": 3,
            "longitude": 101.3999984,
            "velocity": 5
        }
    ]
}



