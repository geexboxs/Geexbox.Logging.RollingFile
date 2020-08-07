# Geexbox.Logging

[![NuGet RollingFile](https://img.shields.io/nuget/v/Geexbox.Logging.RollingFile.svg)](https://www.nuget.org/packages/Geexbox.Logging.RollingFile/)

## Geexbox.Logging.RollingFile

A rolling file provider for ASP.NET Core 2.1 [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging), the logging subsystem used by ASP.NET Core. Writes logs to a set of text files, one per day.

### Getting Started 

**First** Install the [Geexbox.Logging.RollingFile](https://nuget.org/packages/Geexbox.Logging.RollingFile) package from NuGet, either using powershell:

```powershell
Install-Package Geexbox.Logging.RollingFile
```

or using the .NET CLI:

```powershell
dotnet add package Geexbox.Logging.RollingFile
```

**Next** configure the provider by calling `AddRollingFile()` on an `ILoggingBuilder` during logger configuration in _Program.cs_.

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(builder => builder.AddRollingFile()) // <- Add this line
            .UseStartup<Startup>()
            .Build();
}
```

It will read `appsettings.json` for configurations
```jsonc
{
  //...
  "Logging": {
    "RollingFile": {
      "LogLevel": {
        "Default": "Information"
      },
      "LogDirectory": "App_Data/Logs",
    "FileName" : "diagnostics-", // The log file prefixes
    "LogDirectory" : "LogFiles", // The directory to write the logs
    "FileSizeLimit" : 20971520, // The maximum log file size (20MB here)
    "Extension" : "log", // The log file extension
    "Periodicity" : "Hourly" // Roll log files hourly instead of daily.
    },
  },
  //...
}
```

You can pass additional options to the Add File by passing an `Action<FileLoggerOptions>`, for example:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(builder => builder.AddRollingFile(options => {
                options.FileName = "diagnostics-"; // The log file prefixes
                options.LogDirectory = "LogFiles"; // The directory to write the logs
                options.FileSizeLimit = 20 * 1024 * 1024; // The maximum log file size (20MB here)
                options.Extension = "txt"; // The log file extension
                options.Periodicity = PeriodicityOptions.Hourly // Roll log files hourly instead of daily.
            })) 
            .UseStartup<Startup>()
            .Build();
}
```

**Finally** The provider will create log files prefixed with the `FileName`, and suffixed with the current date in the `yyyyMMddHHmm` format (using only the portions up to the selected periodicity, `yyyyMMdd` for the default of daily).

```
log-20160631.txt
log-20160701.txt
log-20160702.txt
```

Logs will look something like the following:

```
2017-09-01 18:34:18.083 +01:00 [Information] Microsoft.AspNetCore.Hosting.Internal.WebHost: Request starting HTTP/1.1 GET http://localhost:50037/api/values  
2017-09-01 18:34:18.159 +01:00 [Information] Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker: Executing action method SampleApp.Controllers.ValuesController.Get (SampleApp) with arguments ((null)) - ModelState is Valid
2017-09-01 18:34:18.161 +01:00 [Information] SampleApp.Controllers.ValuesController: Executed Get action
2017-09-01 18:34:18.165 +01:00 [Information] Microsoft.AspNetCore.Mvc.Internal.ObjectResultExecutor: Executing ObjectResult, writing value Microsoft.AspNetCore.Mvc.ControllerContext.
2017-09-01 18:34:18.192 +01:00 [Information] Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker: Executed action SampleApp.Controllers.ValuesController.Get (SampleApp) in 36.3435ms
2017-09-01 18:34:18.195 +01:00 [Information] Microsoft.AspNetCore.Hosting.Internal.WebHost: Request finished in 113.6076ms 200 application/json; charset=utf-8
```

## Credits

This provider is _heavily_ cribbed from the [Azure App Service Logging Provider](https://github.com/aspnet/logging/blob/dev/src/Microsoft.Extensions.Logging.AzureAppServices/Internal/BatchingLoggerProvider.cs) from the ASP.NET team.
