#region Using Statements
    using System.IO;
    using System.Diagnostics;

    using Topshelf;
    using Serilog;

    using LightInject;
    using Nancy.Hosting.Self;

    using CakeBoss.Host;
#endregion



namespace CakeBoss.Agent
{
    class Program
    {
        #region Fields (2)
            public static IServiceContainer Container = null;
            public static NancyHost Host = null;
        #endregion




        static void Main(string[] args)
        {
            //Logging
            string logFile = @"./Logs/CakeBoss.Agent-{Date}.log";

            if (!Debugger.IsAttached && Directory.Exists("C:/Logs/CakeBoss/"))
            {
                logFile = @"C:/Logs/Agent-{Date}.log";
            }

            Log.Logger = new LoggerConfiguration()
                                .WriteTo.RollingFile(logFile)
                                .WriteTo.ColoredConsole()
                                .MinimumLevel.Verbose()
                                .CreateLogger();



            //Container
            Program.Container = Program.CreateContainer();



            //TopShelf
            HostFactory.Run(x =>
            {
                //Methods
                x.Service<IAgentService>(s =>
                {
                    s.ConstructUsing(name => Program.Container.GetInstance<IAgentService>());

                    s.WhenStarted(ser => ser.Start());
                    s.WhenStopped(ser => ser.Stop());
                    s.WhenShutdown(ser => ser.Shutdown());
                });



                //Settings
                x.StartAutomatically();
                x.EnableShutdown();

                x.RunAsNetworkService();

                x.SetDescription("Cake scripting agent for automating deployment tasks");
                x.SetDisplayName("CakeBoss - Agent");
                x.SetServiceName("CakeBoss.Agent");

                x.UseSerilog();



                //Recovery
                x.EnableServiceRecovery(r =>
                {
                    //Actions
                    r.RestartService(0);
                    r.RestartService(1);
                    r.RestartService(2);

                    //Settings
                    r.OnCrashOnly();
                    r.SetResetPeriod(1);
                });
            });    
        }



        private static IServiceContainer CreateContainer()
        {
            var container = new ServiceContainer();

            container.EnableAnnotatedPropertyInjection();

            container.RegisterFrom<HostRoot>();
            container.RegisterFrom<AgentRoot>();

            CakeContextExtensions.Container = container;

            return container;
        }
    }
}
