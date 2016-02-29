#region Using Statements
    using CakeBoss.Agent;

    using Xunit;
#endregion



namespace CakeBoss.Agent.Tests
{
    public class ServiceTests
    {
        public static IAgentService _Service = null;



        public ServiceTests()
        {
            Startup.CreateContainer();

            _Service = Startup.Container.GetInstance<IAgentService>();
        }



        /*
        [Fact]
        public void Service_Start()
        {
            Assert.True(_Service.RunTarget("Start", true));
        }
        */
    }
}
