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
    public static class AgentAliases
    {
        [CakeMethodAlias]
        public static void ConfigureAgent(this ICakeContext context, AgentSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            //Config
            context.ConfigureHost(settings);

            //Termination
            if (settings.EnableTerminationCheck)
            {
                context.GetContainer().GetInstance<ITerminationService>().Enabled = true;

                context.Log.Information("Starting agent termination service.");
            }

            //API
            if (settings.EnableAPI)
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
        }
    }
}
