# CakeBoss

CakeBoss is the DevOpps tool for the C# masocistc that knows Chef and Puppet are better but blindly chooses something different for the sole reason that they don't like being told they can't write their scripts in C#!

[![Build status](https://ci.appveyor.com/api/projects/status/8s5w8ier41krrqpd?svg=true)](https://ci.appveyor.com/project/PhillipSharpe/cakeboss)

[![cakebuild.net](https://img.shields.io/badge/WWW-cakebuild.net-blue.svg)](http://cakebuild.net/)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)



## Agent Events

CakeBoss.Agent is a windows service that subscribes to the following events, executing the corresponding Cake-Build task when the event is triggered. The agent can even poll AWS instance meta-data every 5 seconds to trigger the Termination event.

* Start Service
* Stop Service
* Startup Server
* Shutdown Server
* Terminate Server



## Agent Event Usage

```csharp
Task("Start")
    .Description("Service Start.")
    .Does(() =>
{
	Information("---Configure the agent---");
    ConfigureAgent(new AgentSettings()
	{
		Port = 8888,

        EnableTerminationCheck = true,
		EnableAPI = true
	}.AddUser("Admin", "Password1"));
});

Task("Stop")
    .Description("Service Stop.")
    .Does(() =>
{

});

Task("Startup")
    .Description("Server Startup.")
    .Does(() =>
{

});

Task("Shutdown")
    .Description("Server Shutdown.")
    .Does(() =>
{

});

Task("Terminate")
    .Description("AWS Instance Termination.")
    .Does(() =>
{

});
```





## Agent API

A Nancy based api enables remote calls from the "Cake.CakeBoss" nuget package, allowing a central machine to execute tasks on individual nodes. Should go without saying but please do NOT enabled the api on a public machine!



## Client Referencing

[![NuGet Version](http://img.shields.io/nuget/v/Cake.CakeBoss.svg?style=flat)](https://www.nuget.org/packages/Cake.CakeBoss/) [![NuGet Downloads](http://img.shields.io/nuget/dt/Cake.CakeBoss.svg?style=flat)](https://www.nuget.org/packages/Cake.CakeBoss/)

Cake.CakeBoss is available as a nuget package from the package manager console:

```csharp
Install-Package Cake.CakeBoss
```

or directly in your build script via a cake addin:

```csharp
#addin "Cake.CakeBoss"
```



## API Usage (Machine1)

```csharp
#addin "Cake.CakeBoss"

Task("Deploy")
    .Description("The deployment process on Machine1")
    .Does(() =>
{
    Information("---Call the agent API on another machine---");
    RunRemoteTarget(new RemoteSettings()
	{
        Username = "Admin",
        Password = "Password1",

        Host = "Machine2"
        Port = 8888,

        Target = "Deploy"
	});
});
```



## API Usage (Machine2)

```csharp
Task("Start")
    .Description("Configures the API on Machine2")
    .Does(() =>
{
    Information("---Configure the agent---");
	ConfigureAgent(new AgentSettings()
	{
		Port = 8888,

        EnableTerminationCheck = false,
		EnableAPI = true
	}.AddUser("Admin", "Password1"));
});

Task("Deploy")
    .Description("Remote target on machine2")
    .Does(() =>
{
    Information("---This would execute on machine2 from the script on machine1---");
});
```





## Scheduled Tasks

CakeBoss uses [FluentScheduler](https://github.com/fluentscheduler/FluentScheduler) to enable scheduled tasks, please consulte their documetnation for the fluent interface.



## Scheduled Task Usage

```csharp
Task("Start")
    .Description("Service Start.")
    .Does(() =>
{
    //Every two hours
	ScheduleTask("Timed-Critical-Task")
        .ToRunNow().AndEvery(2).Hours();

    //15 minute delay
    ScheduleTask("Timed-Critical-Task")
        .ToRunOnceIn(15).Minutes();
        
    //Every morning
    ScheduleTask("Timed-Critical-Task")
        .ToRunEvery(1).Days().At(8, 30);



    Information("---Configure the agent---");
	ConfigureAgent(new AgentSettings()
	{
        EnableScheduledTasks = true
	});
});

Task("Timed-Critical-Task")
    .Description("A task that needs to run at a particular time")
    .Does(() =>
{

});
```





## Example

A complete Cake example can be found [here](https://github.com/SharpeRAD/CakeBoss/blob/master/script/CakeBoss.Agent.cake)