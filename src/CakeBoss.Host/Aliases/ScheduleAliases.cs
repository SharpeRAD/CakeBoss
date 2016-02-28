#region Using Statements
    using Cake.Core;
    using Cake.Core.Annotations;
    using Cake.Core.Diagnostics;

    using FluentScheduler;
    using FluentScheduler.Model;
#endregion



namespace CakeBoss.Host
{
    [CakeAliasCategory("CakeBoss")]
    [CakeNamespaceImport("FluentScheduler")]
    [CakeNamespaceImport("FluentScheduler.Model")]
    public static class ScheduleAliases
    {
        /// <summary>
        /// Starts the scheduled task manager
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Schedule")]
        public static void StartSchedules(this ICakeContext context)
        {
            TaskRegistry instance = context.GetContainer().GetInstance<TaskRegistry>();
            TaskManager.Initialize(instance);

            context.Log.Information("Scheduled tasks started.");
        }
        
        /// <summary>
        /// Stops the scheduled task manager
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Schedule")]
        public static void StopSchedules(this ICakeContext context)
        {
            TaskManager.Stop();

            context.Log.Information("Scheduled tasks stopped.");
        }



        /// <summary>
        /// Adds a scheduled task to the task manager
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The task you want to schedule.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Schedule")]
        public static Schedule ScheduleTask(this ICakeContext context, string target)
        {
            context.Log.Information("Scheduling task '{0}'.", target);

            TaskRegistry instance = context.GetContainer().GetInstance<TaskRegistry>();
            return instance.Schedule(target);
        }
    }
}

