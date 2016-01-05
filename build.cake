#addin "Cake.Slack"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var appName = "CakeBoss";





//////////////////////////////////////////////////////////////////////
// PREPARATION
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
var testResultDir = buildResultDir + "/tests";
var nugetRoot = buildResultDir + "/nuget";

var binAddinDir = buildResultDir + "/binAddin";
var binAgentDir = buildResultDir + "/binAgent";

//Get Solutions
var solutions       = GetFiles("./**/*.sln");





///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
	//Executed BEFORE the first task.
	Information("Building version {0} of {1}.", semVersion, appName);

	NuGetInstall("xunit.runner.console", new NuGetInstallSettings 
	{
		ExcludeVersion  = true,
		OutputDirectory = "./tools"
    });
});



Teardown(() =>
{
	// Executed AFTER the last task.
	Information("Finished building version {0} of {1}.", semVersion, appName);
});





///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
    // Clean solution directories.
	Information("Cleaning old files");
	CleanDirectories(new DirectoryPath[] 
	{
        buildResultDir, binAgentDir, binAddinDir, testResultDir, nugetRoot
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
        Copyright = "Copyright (c) Phillip Sharpe 2015"
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
        OutputDirectory = testResultDir,
        XmlReportV1 = true
    });
});



Task("Copy-Files")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    //Agent
    CopyFileToDirectory(buildAgentDir + "/Cake.Core.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Cake.Common.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Cake.Host.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Cake.CakeBoss.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/CakeBoss.Host.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/CakeBoss.Agent.exe", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/CakeBoss.Agent.exe.config", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/FluentScheduler.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/LightInject.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/LightInject.Annotation.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/Microsoft.CodeAnalysis.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Microsoft.CodeAnalysis.Desktop.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Microsoft.CodeAnalysis.Scripting.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Microsoft.CodeAnalysis.Scripting.CSharp.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Microsoft.CodeAnalysis.Scripting.VisualBasic.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/Microsoft.Web.XmlTransform.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Mono.CSharp.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/Nancy.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Nancy.Authentication.Basic.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Nancy.Hosting.Self.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Nancy.Serialization.JsonNet.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/Newtonsoft.Json.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/NuGet.Core.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/Roslyn.Compilers.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Roslyn.Compilers.CSharp.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/Serilog.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Serilog.FullNetFx.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/Topshelf.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/Topshelf.Serilog.dll", binAgentDir);

    CopyFileToDirectory(buildAgentDir + "/System.Collections.Immutable.dll", binAgentDir);
    CopyFileToDirectory(buildAgentDir + "/System.Reflection.Metadata.dll", binAgentDir);

    CopyFileToDirectory("./script/Install.bat", binAgentDir);
    CopyFileToDirectory("./script/Uninstall.bat", binAgentDir);
    CopyFileToDirectory("./script/CakeBoss.Agent.cake", binAgentDir);

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
    var filename = buildResultDir + "/CakeBoss-v" + semVersion + ".zip";
    Zip(binAgentDir, filename);
});



Task("Create-NuGet-Packages")
    .IsDependentOn("Zip-Files")
    .Does(() =>
{
    NuGetPack("./nuspec/Cake.CakeBoss.nuspec", new NuGetPackSettings {
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

    // Get the path to the package.
    var package = nugetRoot + "/Cake.CakeBoss." + version + ".nupkg";

    // Push the package.
    NuGetPush(package, new NuGetPushSettings 
	{
        ApiKey = apiKey
    }); 
});



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
    var artifact = new FilePath(buildResultDir + "/Cake-CakeBoss-v" + semVersion + ".zip");
    AppVeyor.UploadArtifact(artifact);
}); 



Task("Slack")
    .Does(() =>
{
	//Get Text
	var text = "";

    if (isPullRequest)
    {
        text = "PR submitted for " + appName;
    }
    else
    {
        text = "Published " + appName + " v" + version;
    }

	// Post Message
	var result = Slack.Chat.PostMessage(EnvironmentVariable("SLACK_TOKEN"), "#code", text);

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

Task("Default")
    .IsDependentOn("Package");

Task("AppVeyor")
    .IsDependentOn("Update-AppVeyor-Build-Number")
    .IsDependentOn("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Publish-Nuget")
    .IsDependentOn("Slack");





///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);