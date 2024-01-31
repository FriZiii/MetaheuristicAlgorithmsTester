## Launching application

### Creating an Azure Storage service

You need to create an Azure Storage service with 4 containers with the following names: 

![image](https://github.com/FriZiii/MetaheuristicAlgorithmsTester/assets/38819844/bb5a602a-d7dd-4708-9c5d-89b27226dbe3)

### Update appsettings.json
Get the connection string from AzureStorage and replace it in appsettings.json.

### Create a database
To create a database, just type `update-database` in the `nuget` console.
You can view the database in AzureDataStudio by connecting to the `(localdb)\\mssqllocaldb` server.

### Start the Api
In the `/backend` project directory, you can run the application from the .sln file
