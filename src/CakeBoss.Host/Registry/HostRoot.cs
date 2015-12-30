#region Using Statements
    using System;

    using Cake.Host;
    using Cake.Host.Arguments;
    using Cake.Host.Commands;
    using Cake.Core;
    using Cake.Core.Diagnostics;
    using Cake.Core.IO;
    using Cake.Core.IO.NuGet;
    using Cake.Core.IO.NuGet.Parsing;
    using Cake.Core.Scripting;
    using Cake.Core.Scripting.Analysis;

    using Cake.Host.Diagnostics;
    using Cake.Host.NuGet;
    using Cake.Host.Scripting;
    using Cake.Host.Scripting.Mono;
    using Cake.Host.Scripting.Roslyn;
    using Cake.Host.Scripting.Roslyn.Nightly;
    using Cake.Host.Scripting.Roslyn.Stable;

    using LightInject;

    using Nancy;
    using Nancy.Authentication.Basic;
    using Nancy.Serialization.JsonNet;
#endregion



namespace CakeBoss.Host
{
    public class HostRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry container)
        {
            //Core Services
            container.Register<ICakeEngine, CakeEngine>(new PerContainerLifetime());
            container.Register<IFileSystem, FileSystem>(new PerContainerLifetime());
            container.Register<ICakeEnvironment, CakeEnvironment>(new PerContainerLifetime());
            container.Register<ICakeArguments, CakeArguments>(new PerContainerLifetime());
            container.Register<IGlobber, Globber>(new PerContainerLifetime());
            container.Register<IProcessRunner, ProcessRunner>(new PerContainerLifetime());
            container.Register<IScriptAliasFinder, ScriptAliasFinder>(new PerContainerLifetime());
            container.Register<ICakeReportPrinter, CakeReportPrinter>(new PerContainerLifetime());
            container.Register<IConsole, HostConsole>(new PerContainerLifetime());

            container.Register<IScriptAnalyzer, ScriptAnalyzer>(new PerContainerLifetime());
            container.Register<IScriptProcessor, ScriptProcessor>(new PerContainerLifetime());
            container.Register<IScriptConventions, ScriptConventions>(new PerContainerLifetime());

            container.Register<INuGetToolResolver, NuGetToolResolver>(new PerContainerLifetime());
            container.Register<INuGetPackageInstaller, NuGetPackageInstaller>(new PerContainerLifetime());

            container.Register<IRegistry, WindowsRegistry>(new PerContainerLifetime());
            container.Register<ICakeContext, CakeContext>(new PerContainerLifetime());



            // NuGet addins support
            container.Register<INuGetFrameworkCompatibilityFilter, NuGetVersionUtilityAdapter>(new PerContainerLifetime());
            container.Register<IFrameworkNameParser, NuGetVersionUtilityAdapter>(new PerContainerLifetime());

            container.Register<INuGetPackageAssembliesLocator, NuGetPackageAssembliesLocator>(new PerContainerLifetime());
            container.Register<IPackageReferenceBundler, PackageReferenceBundler>(new PerContainerLifetime());
            container.Register<INuGetAssemblyCompatibilityFilter, NuGetAssemblyCompatibilityFilter>(new PerContainerLifetime());
            container.Register<IAssemblyFilePathFrameworkNameParser, AssemblyFilePathFrameworkNameParser>(new PerContainerLifetime());



            if (Type.GetType("Mono.Runtime") != null)
            {
                //Mono Scripting
                container.Register<IScriptEngine, MonoScriptEngine>(new PerContainerLifetime());
            }
            else
            {
                //Roslyn
                container.Register<IScriptEngine, RoslynScriptEngine>(new PerContainerLifetime());
                container.Register<RoslynScriptSessionFactory>(new PerContainerLifetime());
                container.Register<RoslynNightlyScriptSessionFactory>(new PerContainerLifetime());
            }



            //Cake Services
            container.Register<IArgumentParser, ArgumentParser>(new PerContainerLifetime());
            container.Register<ICommandFactory, CommandFactory>(new PerContainerLifetime());
            container.Register<HostApplication>(new PerContainerLifetime());
            container.Register<IScriptRunner, ScriptRunner>(new PerContainerLifetime());

            container.Register<ICakeLog, HostLog>(new PerContainerLifetime());
            container.Register<IVerbosityAwareLog, HostLog>(new PerContainerLifetime());



            //Script Hosts
            container.Register<BuildScriptHost>(new PerContainerLifetime());
            container.Register<DescriptionScriptHost>(new PerContainerLifetime());
            container.Register<DryRunScriptHost>(new PerContainerLifetime());



            //Script Commands
            container.Register<BuildCommand>(new PerRequestLifeTime());
            container.Register<DescriptionCommand>(new PerRequestLifeTime());
            container.Register<DryRunCommand>(new PerRequestLifeTime());
            container.Register<HelpCommand>(new PerRequestLifeTime());
            container.Register<VersionCommand>(new PerRequestLifeTime());



            //CakeBoss Host
            container.Register<HostSettings>(new PerContainerLifetime());



            //Nancy
            container.Register<IAuthenticationService, AuthenticationService>();
            container.Register<IUserValidator, UserValidator>();

            container.Register<ISerializer, JsonNetSerializer>(new PerRequestLifeTime());
        }
    }
}