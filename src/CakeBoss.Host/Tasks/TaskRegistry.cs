#region Using Statements
    using System;

    using Cake.Host.Scripting;

    using FluentScheduler;
    using FluentScheduler.Model;
#endregion



namespace CakeBoss.Host
{
    public class TaskRegistry : Registry
    {
        #region Fields (1)
            private BuildScriptHost _Host;
        #endregion





        #region Constructor (1)
            public TaskRegistry(BuildScriptHost host)
            {
                if (host == null)
                {
                    throw new ArgumentNullException("host");
                }

                _Host = host;
            }
        #endregion





        #region Functions (1)
            public Schedule Schedule(string target)
            {
                return this.Schedule(() => _Host.RunTarget(target));
            }
        #endregion
    }
}