# NewshoreAssessment

This is the source code of the assessment, in order to make this work please ensure that the machine has the required [SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) (.NET 7.0) and a compatible IDE to compile, run tests, visualize the code and run the application.

Once the code is downloaded or cloned, if the selected IDE is Visual Studio Code, make sure that the C# extension is installed, use the dotnet [commands](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet) to make it compile and run; If the IDE is Visual Studio use the latest version available compatible with .NET 7.0, make sure to restore the required Nuget packages to run the unit test (Nunit Test) if desired and set the initial project is set to _Newshore.API_, open the folder and compile the solution, this is a WebApi application, it can be executed from the IDE using the **HTTP** protocol version.

## CORS

The project is set to allow request from _localhost:4200_, is it possible to test from Swagger or another client like Postman or Insomnia as well, but in order to perform integration test from the Angular application it is important to keep the current configuration.

### Note: 
Although the document suggest to return a simple _Journey_ object from the request, it is more valuable to return multiple options from based on the same parameters, consecuently the endpoint will return a list of _Journey_, everything else was made according to the specificatins.
