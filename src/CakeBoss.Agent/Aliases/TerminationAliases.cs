#region Using Statements
    using Cake.Core;
    using Cake.Core.Annotations;
    using Cake.Core.Diagnostics;

    using CakeBoss.Host;
#endregion



namespace CakeBoss.Agent
{
    [CakeAliasCategory("CakeBoss")]
    public static class TerminationAliases
    {
        /// <summary>
        /// Starts the termination service
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Termination")]
        public static void StartTerminationCheck(this ICakeContext context)
        {
            context.GetContainer().GetInstance<ITerminationService>().Enabled = true;

            context.Log.Information("Termination service started.");
        }
        
        /// <summary>
        /// Stops the termination service
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Termination")]
        public static void StopTerminationCheck(this ICakeContext context)
        {
            context.GetContainer().GetInstance<ITerminationService>().Enabled = false;

            context.Log.Information("Termination service stopped.");
        }
    }
}

