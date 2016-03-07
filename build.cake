#addin "Cake.Slack"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var tools = Argument("tools", "./tools");

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var appName = "CakeBoss";





//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////

// Get whether or not this is a local build.
var local = BuildSystem.IsLocalBuild;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;

// Parse release notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

// Get version.
var buildNumber = AppVeyor.Environment.Build.Number;
var version = releaseNotes.Version.ToString();
var semVersion = local ? version : (version + string.Concat("-build-", buildNumber));

// Define directories.
var buildAddinDir = "./src/Cake.CakeBoss/bin/" + configuration;
var buildAgentDir = "./src/CakeBoss.Agent/bin/" + configuration;

var buildResultDir = "./build/v" + semVersion;
var testResultsDir = buildResultDir + "/test-results";

var nugetRoot = buildResultDir + "/nuget";
var binAddinDir = buildResultDir + "/binAddin";
var binAgentDir = buildResultDir + "/binAgent";

//Get Solutions
var solutions = GetFiles("./src/*.sln");

// Package
var zipPackage = buildResultDir + "/CakeBoss-v" + semVersion + ".zip";





///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
	// Executed BEFORE the first task.
	Information("Building version {0} of {1}.", semVersion, appName);
	Information("Tools dir: {0}.", tools);
	
	// Install Tools
	NuGetInstall("xunit.runner.console", new NuGetInstallSettings
	{
		ExcludeVersion  = true,
		OutputDirectory = tools
    });

	NuGetInstall("gitreleasemanager", new NuGetInstallSettings
	{
		ExcludeVersion  = true,
		OutputDirectory = tools
    });

	// Install local nuget for VS copy task
	if (!DirectoryExists("./tools"))
	{
		CreateDirectory("./tools");
		CopyFile(EnvironmentVariable("NUGET_EXE"), "./tools/nuget.exe");
	}
});

Teardown(() =>
{
	// Executed AFTER the last task.
	Information("Finished building version {0} of {1}.", semVersion, appName);
});





///////////////////////////////////////////////////////////////////////////////
// PREPARE
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
    // Clean solution directories.
	Information("Cleaning old files");

	CleanDirectories(new DirectoryPath[]
	{
        buildAgentDir, buildAddinDir,
        buildResultDir,
        binAgentDir, binAddinDir,
        testResultsDir, nugetRoot
	});
});

Task("Restore-Nuget-Packages")
	.IsDependentOn("Clean")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}", solution);
        
        NuGetRestore(solution);
    }
});





///////////////////////////////////////////////////////////////////////////////
// BUILD
///////////////////////////////////////////////////////////////////////////////

Task("Patch-Assembly-Info")
    .IsDependentOn("Restore-Nuget-Packages")
    .Does(() =>
{
    var file = "./src/SolutionInfo.cs";

    CreateAssemblyInfo(file, new AssemblyInfoSettings
	{
		Product = appName,
        Version = version,
        FileVersion = version,
        InformationalVersion = semVersion,
        Copyright = "Copyright (c) 2015 - " + DateTime.Now.Year.ToString() + " Phillip Sharpe"
    });
});

Task("Build")
    .IsDependentOn("Patch-Assembly-Info")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
		Information("Building {0}", solution);

		MSBuild(solution, settings =>
			settings.SetPlatformTarget(PlatformTarget.MSIL)
				    .WithProperty("TreatWarningsAsErrors","true")
				    .WithTarget("Build")
				    .SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2("./src/**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings
	{
		ToolPath = tools + "/xunit.runner.console/tools/xunit.console.exe",
		
        OutputDirectory = testResultsDir,
        XmlReportV1 = true
    });
});





///////////////////////////////////////////////////////////////////////////////
// PACKAGE
///////////////////////////////////////////////////////////////////////////////

Task("Copy-Files")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    //Agent
    CopyFiles(buildAgentDir + "/*.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/CakeBoss.Agent.exe", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/CakeBoss.Agent.exe.config", binAgentDir);

    CopyFileToDirectory("./script/Install.bat", binAgentDir);
    CopyFileToDirectory("./script/Uninstall.bat", binAgentDir);
    CopyFileToDirectory("./script/Deploy.bat", binAgentDir);

    CopyFile("./script/Release.cake", binAgentDir + "/CakeBoss.Agent.cake");

    CreateDirectory(binAgentDir + "/Tools/");
    CreateDirectory(binAgentDir + "/Addins/");

    CopyFileToDirectory("./tools/nuget.exe", binAgentDir + "/Tools/");
    CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binAgentDir);



    //Addin
    CopyFileToDirectory(buildAgentDir + "/Cake.CakeBoss.dll", binAddinDir);
    CopyFileToDirectory(buildAgentDir + "/Cake.CakeBoss.pdb", binAddinDir);
    CopyFileToDirectory(buildAgentDir + "/Cake.CakeBoss.xml", binAddinDir);

    CopyFileToDirectory(buildAgentDir + "/RestSharp.dll", binAddinDir);

    CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binAddinDir);
});

Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .Does(() =>
{
    Zip(binAgentDir, zipPackage);
});



Task("Create-NuGet-Packages")
    .IsDependentOn("Zip-Files")
    .Does(() =>
{
    NuGetPack("./nuspec/Cake.CakeBoss.nuspec", new NuGetPackSettings
    {
        Version = version,
        ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binAddinDir,
        OutputDirectory = nugetRoot,
        Symbols = false,
        NoPackageAnalysis = true
    });
});

Task("Publish-Nuget")
	.IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => isRunningOnAppVeyor)
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("NUGET_API_KEY");

    if(string.IsNullOrEmpty(apiKey))
	{
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }



    // Push the package.
    var package = nugetRoot + "/Cake.CakeBoss." + version + ".nupkg";

    NuGetPush(package, new NuGetPushSettings
	{
        ApiKey = apiKey
    });
});





///////////////////////////////////////////////////////////////////////////////
// APPVEYOR
///////////////////////////////////////////////////////////////////////////////

Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(semVersion);
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Zip-Files")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UploadArtifact(zipPackage);
});





///////////////////////////////////////////////////////////////////////////////
// MESSAGE
///////////////////////////////////////////////////////////////////////////////

Task("Slack")
	.WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    // Resolve the API key.
    var token = EnvironmentVariable("SLACK_TOKEN");

    if(string.IsNullOrEmpty(token))
	{
        throw new InvalidOperationException("Could not resolve Slack token.");
    }



	// Post Message
	var text = "Published " + appName + " v" + version;

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

Task("Package")
	.IsDependentOn("Zip-Files")
    .IsDependentOn("Create-NuGet-Packages");

Task("Publish")
    .IsDependentOn("Publish-Nuget");

Task("AppVeyor")
	.IsDependentOn("Publish")
    .IsDependentOn("Update-AppVeyor-Build-Number")
    .IsDependentOn("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Slack");



Task("Default")
    .IsDependentOn("Package");





///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);