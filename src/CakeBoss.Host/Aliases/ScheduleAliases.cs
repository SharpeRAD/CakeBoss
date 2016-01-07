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
        /// Starts a Nuget server using the specified information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The task you want to schedule.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Schedule")]
        public static Schedule ScheduleTask(this ICakeContext context, string target)
        {
            TaskRegistry instance = context.GetContainer().GetInstance<TaskRegistry>();

            return instance.Schedule(target);
        }

        /// <summary>
        /// Starts a Nuget server using the specified information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The task you want to shedule.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Schedule")]
        public static void StartSchedules(this ICakeContext context)
        {
            context.Log.Information("Starting scheduled tasks.");

            TaskRegistry instance = context.GetContainer().GetInstance<TaskRegistry>();
            TaskManager.Initialize(instance);
        }
    }
}

