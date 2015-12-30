using System;
using Cake.Core.Scripting;
using Cake.Host.Scripting;

namespace Cake.Host.Commands
{
    /// <summary>
    /// A command that dry runs a build script.
    /// </summary>
    public sealed class DryRunCommand : ICommand
    {
        private readonly IScriptRunner _scriptRunner;
        private readonly DryRunScriptHost _host;

        // Delegate factory used by Autofac.
        public delegate DryRunCommand Factory();

        public DryRunCommand(IScriptRunner scriptRunner, DryRunScriptHost host)
        {
            _scriptRunner = scriptRunner;
            _host = host;
        }

        public bool Execute(CakeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            _scriptRunner.Run(_host, options.Script, options.Arguments);
            return true;
        }
    }
}