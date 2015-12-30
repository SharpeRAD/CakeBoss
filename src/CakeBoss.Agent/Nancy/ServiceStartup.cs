#region Using Statements
    using Nancy;
    using Nancy.Responses;
    using Nancy.Bootstrapper;
    using Nancy.Authentication.Basic;

    using Cake.Core;
    using Cake.Core.Diagnostics;
#endregion



namespace CakeBoss.Agent
{
    public class ServiceStaartup : IApplicationStartup
    {
        #region Functions (3)
            public void Initialize(IPipelines pipelines)
            {
                pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(Program.Container.GetInstance<IUserValidator>(), "CakeBoss", UserPromptBehaviour.NonAjax));

                pipelines.OnError.AddItemToEndOfPipeline((context, exception) =>
                {
                    //Log Error
                    ICakeLog log = Program.Container.GetInstance<ICakeLog>();

                    log.Error(exception.Message);



                    //Return Response
                    Response response = new JsonResponse(exception.Message, Program.Container.GetInstance<ISerializer>());

                    response.StatusCode = HttpStatusCode.InternalServerError;

                    return response;
                });
            }
        #endregion
    }
}
