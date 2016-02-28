#region Using Statements
    using System;

    using Cake.Core;
    using Cake.Core.Annotations;
    using Cake.Core.Diagnostics;

    using Nancy.Hosting.Self;

    using CakeBoss.Host;
#endregion



namespace CakeBoss.Agent
{
    [CakeAliasCategory("CakeBoss")]
    public static class ApiAliases
    {
        /// <summary>
        /// Starts the agent api
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Api")]
        public static void StartApi(this ICakeContext context)
        {
            if (Program.Host == null)
            {
                HostSettings hostSettings = context.GetContainer().GetInstance<HostSettings>();

                if (!String.IsNullOrEmpty(hostSettings.Url))
                {
                    Program.Host = new NancyHost(new Uri(hostSettings.Url), new ServiceBootstrapper(), new HostConfiguration()
                    {
                        EnableClientCertificates = true,

                        UrlReservations = new UrlReservations()
                        {
                            CreateAutomatically = true
                        }
                    });

                    Program.Host.Start();

                    context.Log.Information("Starting agent api on url '{0}'.", hostSettings.Url);
                }
                else
                {
                    context.Log.Error("Invalid host url.");
                }
            }
            else
            {
                context.Log.Error("Agent api has already been configured.");
            }
        }
        
        /// <summary>
        /// Stops the agent Api
        /// </summary>
        /// <param name="context">The context.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Api")]
        public static void StopApi(this ICakeContext context)
        {
            if (Program.Host != null)
            {
                Program.Host.Stop();
                Program.Host = null;

                context.Log.Information("Agent api stopped.");
            }
            else
            {
                context.Log.Error("Agent api is not running.");
            }
        }
    }
}

