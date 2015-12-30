#region Using Statements
    using System;

    using Nancy;
    using Nancy.Security;
#endregion



namespace CakeBoss.Agent
{
    public class ServiceModule : NancyModule
    {
        #region Fields (1)
            private IAgentService _Service;
        #endregion





        #region Constructors (1)
            public ServiceModule(IAgentService service) 
                : base("/agent")
            {
                if (service == null)
                {
                    throw new ArgumentNullException("service");
                }

                _Service = service;

                this.RequiresAuthentication();



                Get["/current"] = x =>
                {
                    return _Service.CurrentTask;
                };

                Post["/run/{target}"] = x =>
                {
                    return _Service.RunTarget(x.target, false);
                };
            }
        #endregion
    }
}
