using System;
using Cake.Core.Scripting;
using Cake.Host.Scripting;

namespace Cake.Host.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly BuildCommand _buildCommandFactory;
        private readonly DescriptionCommand _descriptionCommandFactory;
        private readonly DryRunCommand _dryRunCommandFactory;
        private readonly HelpCommand _helpCommandFactory;
        private readonly VersionCommand _versionCommandFactory;

        public CommandFactory(
            BuildCommand buildCommandFactory,
            DescriptionCommand descriptionCommandFactory,
            DryRunCommand dryRunCommandFactory,
            HelpCommand helpCommandFactory,
            VersionCommand versionCommandFactory)
        {
            _buildCommandFactory = buildCommandFactory;
            _descriptionCommandFactory = descriptionCommandFactory;
            _dryRunCommandFactory = dryRunCommandFactory;
            _helpCommandFactory = helpCommandFactory;
            _versionCommandFactory = versionCommandFactory;
        }

        public ICommand CreateBuildCommand()
        {
            return _buildCommandFactory;
        }

        public ICommand CreateDescriptionCommand()
        {
            return _descriptionCommandFactory;
        }

        public ICommand CreateDryRunCommand()
        {
            return _dryRunCommandFactory;
        }

        public ICommand CreateHelpCommand()
        {
            return _helpCommandFactory;
        }

        public ICommand CreateVersionCommand()
        {
            return _versionCommandFactory;
        }
    }
}
