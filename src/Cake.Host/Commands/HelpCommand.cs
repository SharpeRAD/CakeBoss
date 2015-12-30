using System;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Host.Commands
{
    /// <summary>
    /// A command that displays help information.
    /// </summary>
    public sealed class HelpCommand : ICommand
    {
        private readonly IConsole _console;

        // Delegate factory used by Autofac.
        public delegate HelpCommand Factory();

        public HelpCommand(IConsole console)
        {
            _console = console;
        }

        public bool Execute(CakeOptions options)
        {
            _console.WriteLine();
            _console.WriteLine("Usage: Cake.Host.exe [build-script] [-verbosity=value]");
            _console.WriteLine("                [-showdescription] [-dryrun] [..]");
            _console.WriteLine();
            _console.WriteLine("Example: Cake.Host.exe");
            _console.WriteLine("Example: Cake.Host.exe build.Cake.Host -verbosity=quiet");
            _console.WriteLine("Example: Cake.Host.exe build.Cake.Host -showdescription");
            _console.WriteLine();
            _console.WriteLine("Options:");
            _console.WriteLine("    -verbosity=value    Specifies the amount of information to be displayed.");
            _console.WriteLine("                        ({0})",
                string.Join(", ", Enum.GetNames(typeof(Verbosity))));
            _console.WriteLine("    -showdescription    Shows description about tasks.");
            _console.WriteLine("    -dryrun             Performs a dry run.");
            _console.WriteLine("    -version            Displays version information.");
            _console.WriteLine("    -help               Displays usage information.");
            _console.WriteLine("    -experimental       Uses the nightly builds of Roslyn script engine.");
            _console.WriteLine();

            return true;
        }
    }
}