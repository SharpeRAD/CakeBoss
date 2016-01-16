#addin "Cake.Powershell"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");





///////////////////////////////////////////////////////////////////////////////
// SERVICE EVENTS
///////////////////////////////////////////////////////////////////////////////

Task("Start")
    .Does(() =>
{
    Information("---Add Scheduled Tasks---");
    ScheduleTask("Timed")
        .ToRunOnceIn(15).Seconds();



    Information("---Start Agent---");
	ConfigureAgent(new AgentSettings()
	{
		Port = 8888,

        EnableTerminationCheck = false,
		EnableAPI = true,
        EnableScheduledTasks = true
	}.AddUser("Admin", "Password1"));
           
        

    Information("---Call API---");
    RunRemoteTarget("Remote", new RemoteSettings()
	{
        Username = "Admin",
        Password = "Password1",
        Port = 8888
	});
});

Task("Stop")
    .Does(() =>
{
    Information("---Stop---");
});

Task("Shutdown")
    .Does(() =>
{
    Information("---Shutdown---");
});





///////////////////////////////////////////////////////////////////////////////
// SPECIAL EVENTS
///////////////////////////////////////////////////////////////////////////////

Task("Remote")
    .Does(() =>
{
    Information("---Remote Webservice Call---");
    StartPowershellScript("Write-Host", args => 
        { 
            args.AppendQuoted("Triggering remote deployment"); 
        });
});

Task("Timed")
    .Does(() =>
{
    Information("---Timed Task Call---");
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