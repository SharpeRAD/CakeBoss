#region Using Statements
    using System;
    using System.Collections.Generic;

    using Cake.Core;
    using Cake.Host.Scripting;

    using CakeBoss.Host;
#endregion



namespace CakeBoss.Agent
{
    public class AgentService : IAgentService
    {
        #region Fields (2)
            private HostApplication _Application;
            private BuildScriptHost _Host;
        #endregion





        #region Constructor (1)
            public AgentService(HostApplication application, BuildScriptHost host)
            {
                if (application == null)
                {
                    throw new ArgumentNullException("application");
                }
                if (host == null)
                {
                    throw new ArgumentNullException("host");
                }

                _Application = application;
                _Host = host;
            }
        #endregion





        #region Properties (1)
            public string CurrentTask { get; set; }
        #endregion





        #region Functions (7)
            //Events
            public void Start(IDictionary<string, object> arguments) 
            {
                string target = UptimeUtils.IsStartup() ? "Startup" : "Start";

                if ((arguments != null) && arguments.ContainsKey("target") && !String.IsNullOrWhiteSpace(arguments["target"].ToString()))
                {
                    target = arguments["target"].ToString();
                }

                this.RunTarget(target, true);
            }

            public void Stop() 
            {
                this.RunTarget("Stop", false);
            }

            public void Shutdown()
            {
                this.RunTarget("Shutdown", false);
            }

            public void Terminate()
            {
                this.RunTarget("Terminate", false);
            }



            //Run
            private IList<string> GetArgs(string target)
            {
                var args = new List<string>();

                args.Add(@"./CakeBoss.Agent.cake");

                if (!String.IsNullOrEmpty(target))
                {
                    args.Add("-target=" + target);
                }

                if (Type.GetType("Mono.Runtime") != null)
                {
                    args.Add("-mono");
                }

                args.Add("-experimental=true");
                args.Add("-verbosity=verbose");

                return args;
            }

            public bool RunTarget(string target, bool load)
            {
                this.CurrentTask = target;
                bool result;

                if (load)
                {
                    IList<string> args = this.GetArgs(target);
             
                    result = _Application.Run(args);
                }
                else
                {
                    CakeReport report = _Host.RunTarget(target);

                    result = (report != null && !report.IsEmpty);
                }

                this.CurrentTask = "";
                return result;
            }
        #endregion
    }
}
