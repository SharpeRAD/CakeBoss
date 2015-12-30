#region Using Statements
    using LightInject;
    using Nancy;
#endregion



namespace CakeBoss.Agent
{
    public class AgentRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry container)
        {
            //Register Agent
            container.Register<IAgentService, AgentService>(new PerContainerLifetime());
            container.Register<ITerminationService, TerminationService>(new PerContainerLifetime());



            //Register Nancy
            container.Register<INancyModule, ServiceModule>("Agent");
        }
    }
}