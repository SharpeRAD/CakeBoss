#addin "Cake.Slack"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");





///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Start")
    .Does(() =>
{
    Information("---Start Agent---");
	ConfigureAgent(new AgentSettings()
	{
		Port = 8888,

        EnableTerminationCheck = false,
		EnableAPI = true
	}.AddUser("Admin", "Password1"));
           
        

    Information("---Call API---");
    RunRemoteTarget(new RemoteSettings()
	{
        Username = "Admin",
        Password = "Password1",
        Port = 8888,

        Target = "Remote"
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



Task("Remote")
    .IsDependentOn("Slack")
    .Does(() =>
{
    Information("---Remote Webservice Call---");
    Debug("-- Cool Right :)");
});



Task("Slack")
    .Does(() =>
{
	//Get Text
	var text = "Test message";

	// Post Message
    var token = EnvironmentVariable("SLACK_TOKEN");
	var result = Slack.Chat.PostMessage(token, "#code", text);

	if (result.Ok)
	{
		//Posted
		Information("Message was succcessfully sent to Slack.");
	}
	else
	{
		//Error
		Error("Failed to send message to Slack: {0}", result.Error);
	}
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