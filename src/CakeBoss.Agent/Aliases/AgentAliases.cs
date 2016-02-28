#region Using Statements
    using System;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Annotations;
    using Cake.Core.Diagnostics;
    using Cake.Common.IO;

    using LightInject;
    using Nancy.Hosting.Self;

    using CakeBoss.Host;
#endregion



namespace CakeBoss.Agent
{
    [CakeAliasCategory("CakeBoss")]
    public static class AgentAliases
    {
        /// <summary>
        /// Configures the agent using the  specified information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The agent settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Agent")]
        public static void ConfigureAgent(this ICakeContext context, AgentSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }



            //Configure
            context.ConfigureHost(settings);



            //Start
            if (settings.EnableTerminationCheck)
            {
                context.StartTerminationCheck();
            }

            if (settings.EnableAPI)
            {
                context.StartApi();
            }

            if (settings.EnableScheduledTasks)
            {
                context.StartSchedules();
            }
        }



        /// <summary>
        /// reloads the agent from the existing config file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The path to the new config file.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Agent")]
        public static void ReconfigureAgent(this ICakeContext context, FilePath path)
        {
            // Update config file
            context.CopyFile(path, FilePath.FromString("./CakeBoss.Agent.cake"));
            context.Log.Information("Config file updated.");

            context.RestartAgent();
        }

        /// <summary>r
        /// Reloads the agent from the existing config file
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Agent")]
        public static void RestartAgent(this ICakeContext context)
        {
            // Stop services
            context.StartTerminationCheck();
            context.StopSchedules();
            context.StopApi();

            // Clear all register references
            Program.Container = Program.CreateContainer();

            // Restart the agent
            IAgentService service = context.GetContainer().GetInstance<IAgentService>();
            service.RunTarget("Start", true);
            context.Log.Information("Cake engine reloaded.");
        }
    }
}
