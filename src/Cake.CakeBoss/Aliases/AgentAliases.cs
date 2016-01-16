#region Using Statements
    using System;
    using System.Net;

    using Cake.Core;
    using Cake.Core.Annotations;
    using Cake.Core.Diagnostics;

    using RestSharp;
    using RestSharp.Authenticators;
#endregion



namespace Cake.CakeBoss
{
    /// <summary>
    /// Contains Cake aliases for calling CakeBoss agents
    /// </summary>
    [CakeAliasCategory("CakeBoss")]
    public static class AgentAliases
    {
        /// <summary>
        /// Run a target on a remote CakeBoss agent
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="target">The script target to run.</param>
        /// <param name="settings">The information about the remote target to run.</param>
        /// <returns>If the target ran successfully</returns>
        [CakeMethodAlias]
        public static bool RunRemoteTarget(this ICakeContext context, string target, RemoteSettings settings)
        {
            if (String.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException("target");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (String.IsNullOrEmpty(settings.Username))
            {
                throw new ArgumentNullException("settings.Username");
            }
            if (String.IsNullOrEmpty(settings.Password))
            {
                throw new ArgumentNullException("settings.Password");
            }

            if (String.IsNullOrEmpty(settings.Url))
            {
                throw new ArgumentNullException("settings.Url");
            }



            //Client
            string url = settings.Url;

            if (!url.EndsWith("/agent"))
            {
                if (!url.EndsWith("/"))
                {
                    url += "/";
                }

                url += "agent";
            }

            RestClient client = new RestClient(url);
            client.Authenticator = new HttpBasicAuthenticator(settings.Username, settings.Password);



            //Request
            RestRequest request = new RestRequest("/run/" + target, Method.POST);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                context.Log.Information("Target {0} ran on agent {1}.", target, url);
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                context.Log.Error("Unable to authorise. Check the username / password.");
            }
            else if (response.ErrorMessage == "Unable to connect to the remote server")
            {
                context.Log.Error("Unable to connect. Check the url for the remote host {1}.", target, url);
            }
            else
            {
                context.Log.Error(response.ErrorMessage);
            }

            return false;
        }
    }
}
