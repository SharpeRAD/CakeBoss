#region Using Statements
    using System;

    using LightInject;
    using Nancy.Hosting.Self;

    using CakeBoss.Agent;
    using CakeBoss.Host;
#endregion



namespace CakeBoss.Agent.Tests
{
    public static class Startup
    {
        #region Fields (2)
            public static IServiceContainer Container = null;
            public static NancyHost Host = null;
        #endregion





        #region Functions (2)
            public static void CreateContainer()
            {
                if (Container == null)
                {
                    var container = new ServiceContainer();

                    container.EnableAnnotatedPropertyInjection();

                    container.RegisterFrom<HostRoot>();
                    container.RegisterFrom<AgentRoot>();

                    Container = container;
                }
            }

            public static void ConfigureHost()
            {
                if (Host == null)
                {
                    Host = new NancyHost(new Uri("http://localhost:2001"), new UnitTestBootstrapper(), new HostConfiguration()
                    {
                        UrlReservations = new UrlReservations()
                        {
                            CreateAutomatically = true
                        }
                    });

                    Host.Start();
                }
            }
        #endregion
    }
}
