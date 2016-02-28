//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var server = System.Environment.MachineName.ToLower();





//////////////////////////////////////////////////////////////////////
// SERVICE TASKS
//////////////////////////////////////////////////////////////////////

Task("Start")
    .Description("Service Start.")
    .Does(() =>
{
    Information("---Add Scheduled Tasks---");
    ScheduleTask("Deploy")
        .ToRunOnceIn(15).Seconds();



	Information("---Configure the agent---");
    ConfigureAgent(new AgentSettings()
	{
		Port = 8888,

        EnableTerminationCheck = false,
		EnableAPI = false,
        EnableScheduledTasks = true
	});
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





//////////////////////////////////////////////////////////////////////
// DEPLOY TASKS
//////////////////////////////////////////////////////////////////////

Task("Deploy")
    .Does(() =>
{
    Information("---Example Deployment---");
});

Task("Update")
    .Does(() =>
{
    Information("---Update Config---");
});





//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Start");





///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);