Pre-requisities: ProblemStatement.txt

To target multi-platforms, I decided to go with .Net core; which needed some research with respect to dependency injection, 
testability & cross-cutting concerns.

https://www.microsoft.com/net/core/platform
===========================================================================================================================================================
How to Test/Run:
- Download and Open up the solution
- Do a clean and then a rebuild, the NuGet package manager will download/restore the packages

- Run the Web API service (HelloWorld.API project). Once the api service starts via a browser, you've 2 options
  Option 1: Detach API service from debugging session
  Option 2: Keep the API service attached to the debugger. 
  
- Testing - You'll have the following options, which are flexible to be tested in combination:
  Option 1: Quick test: Type in the browser 'http://localhost:17729/helloworld/' OR 'http://localhost:17729/helloworld/0' OR 'http://localhost:17729/helloworld//1'
     to quickly verify the response
  Option 2: Console app: Run via Visual studio (the HelloWorld.ConsoleApp project) or Navigate to the BIN directory and run the exe
  Option 3: Web app: Run via Visual studio, the HelloWorld.WebApp website

===========================================================================================================================================================
Plumbing the HelloWorldApi using .Net Standards 1.1 consists of the following 
Assumptions & Pointers:
1. Core:
  - Model [HelloWorld.Model]
    - 'ResponseEntity' to provide the requester with certain responses, based on the Id provided in the request url
    - A placeholder for 'Stats' (model) is used to scale the api to track requests hitting the API service, however, the plumbing is not fleshed out
      
  - DataAccess [HelloWorld.DataAccess] - DataAccess layer honing on Repository pattern
    - Repository layer to vend out model from I/O or memory sources (via ContextSeeder)
    - Entity Framework using Code first patterns is used to simulate a in-memory DbContext of 'Responses' & 'Stats' Entities
    - Repository.cs is a Facade to the internal implementations, with a simple logging concern addressed
    - DataAccessModule.cs modularizes the Dependency injection concern
  
  - Utilities [HelloWorld.Utilities]
    - Common playground of utility files and helper functions - currenly, only contains IModule to create a patternize dependency injection
    
2. Services:
  - ViewModel [HelloWorld.ViewModel] 
    - ResponseStatsViewModel is used at the presentation/api layer
    
  - Services [HelloWorld.Services]
    - Service.cs ensures a complete separation of concern between the API layer and the Data Access Layer. 
      It translates the Entities to a ViewModel using AutoMapper
    - ServiceModule.cs modularizes the Dependency injection concern
    
3. API    
  - HelloWorld.API
    - Properties > launchSettings.json: contains base level settings for the url
    - appSettings.config: (Similar to (App/Web).config)- Extension to use Database can be driven from here!
    - Program.cs: creates a WebHostBuilder, which hosts the web api/app using Kestrel Web server at run time.
    - Startup.cs - stitches the dependencies together, which are constructed when configuring the services and resolved when configuring the 
      Http request pipleline at run-time
    - Mvc pipeline is configured to serializer using Json format & default route to Home controller's Index view
    - Logging: Log4net usage is injected via Startup.cs file using constructs from Microsoft Extension's Logging namespace and 
      Microsoft.Extensions.Logging.Log4Net.AspNetCore. 
      At runtime, a Log folder is generated to trace all the details defined in log4net.config
      Logging configurations are pulled via appsettings.json file.
    - Exception handling is delegated to the Middleware, using the pipeline described in Startup.cs file
    - Other javascript/css/controllers/razor pages/json files were not updated much. 
    - HelloWorldController.cs
      - Get() is plumbed to get a nullable? int from the request url and translate them to effective responses. I've assumed the responses 
        to be served from the backend (like greetings). Another assumption which was not fleshed out is to track the request activity
      - Other REST APIs were not fleshed out.
      - User Authorization was not implmented.
      
   - HelloWorld.API.UnitTests
     - Scope is limited to testing the HelloWorldController via HelloWorldControllerTest.cs.
     - This project could not use NUnit, since NUnit (even in beta version as of 07/15/2017 was not compatible with asp.net core 1.1)
     - xUnit's beta package is referenced to solve some issues with assembly incompatibilities noticed on VS 2017
     
4. Apps (Clients)
   - HelloWorld.ConsoleApp
   - HelloWorld.WebApp
   
   Both define a WebApiProxy that requests a async call to the web api service. Once the Web API service sends the response (within 
   timeout limit), it is then displayed on the output (Console or Browser). 
   - Note that appsettings.json contains the 'Api Url' and the 'Request time out in milliseconds' keys.

Other Notes:
- The Web API can potentially serve more clients, in future
