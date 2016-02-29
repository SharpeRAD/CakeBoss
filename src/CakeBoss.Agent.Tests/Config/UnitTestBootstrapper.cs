#region Using Statements
    using LightInject;
    using LightInject.Nancy;

    using Nancy;
    using Nancy.Responses;
    using Nancy.Bootstrapper;
    using Nancy.Authentication.Basic;

    using Cake.Core;
    using Cake.Core.Diagnostics;
#endregion



namespace CakeBoss.Agent.Tests
{
    public class UnitTestBootstrapper : LightInjectNancyBootstrapper
    {
        #region Functions (3)
            protected override IServiceContainer GetServiceContainer()
            {
                return Startup.Container;
            }

            protected override void ConfigureApplicationContainer(IServiceContainer container)
            {
                base.ConfigureApplicationContainer(container);
            }



            protected override void ApplicationStartup(IServiceContainer container, IPipelines pipelines)
            {
                base.ApplicationStartup(container, pipelines);

                pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(container.GetInstance<IUserValidator>(), "CakeBoss", UserPromptBehaviour.NonAjax));

                pipelines.OnError.AddItemToEndOfPipeline((context, exception) =>
                {
                    //Log Error
                    ICakeLog log = container.GetInstance<ICakeLog>();

                    log.Error(exception.Message);



                    //Return Response
                    Response response = new JsonResponse(exception.Message, container.GetInstance<ISerializer>());

                    response.StatusCode = HttpStatusCode.InternalServerError;

                    return response;
                });
            }
        #endregion
    }
}