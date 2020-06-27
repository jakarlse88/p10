# Abarnathy Clinic
This is the 10th and final project of the [OpenClassrooms .NET Back-End Developer Path](https://openclassrooms.com/en/paths/156-back-end-developer-net), the objectives of which are to implement an application for a fictional medical company using a microservices architecture. 

Technologies used:
- Docker
- ASP.NET Core Web API
- ASP.NET Core Blazor/WebAssembly
- MS SQL Server
- MongoDb
- Swagger
- Serilog
- Bootstrap

## How to run
Download or clone this repository, and then either run the application using the Docker CLI, or using Visual Studio.

### Docker CLI
Open your terminal of choice, and then:
1. Navigate to the project's root directory
2. Bring up the project by entering ``docker-compose up -d``
3. Enjoy a cup of coffee while waiting for Docker to do its thing and for the services to come online; note that there is a 90 second delay imposed on the ``demographics-service``to give SQL Server time to come up
4. Test app functionality (see URL overview below), optionally view logs using ``docker logs --details <CONTAINER_ID>``
5. Clean up using ``docker-compose down`` 

### Visual Studio
Using Visual Studio:
1. Open ``Abarnathy.sln``
2. **Critical:** set the configuration to ``Release`` (see below)
3. Run the project by hitting ``CTRL+F5``, let enjoy a cup of coffee while VS/Docker do their things

![Release config](https://i.imgur.com/YH2SIlI.png)

### Port overview
| Service | Port  |
| ------- | ----- |
| Client  | 8081  |
| Demographcis | 8080 |
| History | 8082 |
| Assessment | 8083 |

To view the Swagger UI for the API services, simply navigate to `http://localhost:<port>` in your browser.

## Issues

This project is as of writing affected by the following issues:
- [aspnetcore/#20613](github.com/dotnet/aspnetcore/issues/20613): Blazor WebAssembly ASP.NET Core hosted project does not copy the wwwroot to output, therefore the Blazor client will not work if the project is launched using `docker-compose` with the `Debug` configuration from Visual Studio (Windows). Fix: run using the commandline, or from VS using the `Release` configuration.
