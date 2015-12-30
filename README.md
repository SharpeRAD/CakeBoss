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

Task("Startup")
    .Description("The build process on Machine1")
    .Does(() =>
{
    RunRemoteTarget(new RemoteSettings()
	{
        Username = "Admin",
        Password = "Password1",

        Host = "Machine2"
        Port = 8888,

        Target = "Remote"
	});
});
```



## API Usage (Machine2)

```csharp
Task("Startup")
    .Description("Configures the API on Machine2")
    .Does(() =>
{
    Information("---Config Agent---");
	ConfigureAgent(new AgentSettings()
	{
		Port = 8888,

        EnableTerminationCheck = false,
		EnableAPI = true
	}.AddUser("Admin", "Password1"));
});

Task("Remote")
    .Description("Remote target on machine2")
    .Does(() =>
{
    Information("---This would execute on machine2 from the script on machine1---");
});
```



## Example

A complete Cake example can be found [here](https://github.com/SharpeRAD/CakeBoss/blob/master/script/CakeBoss.Agent.cake)