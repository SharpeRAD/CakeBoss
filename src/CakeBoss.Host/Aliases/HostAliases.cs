#region Using Statements
    using System;
    using System.Collections.ObjectModel;

    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Core.Annotations;
    using Cake.Core.Diagnostics;

    using LightInject;
#endregion



namespace CakeBoss.Host
{
    [CakeAliasCategory("CakeBoss")]
    public static class HostAliases
    {
        /// <summary>
        /// Starts a Nuget server using the specified information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The Nuget server settings.</param>
        [CakeMethodAlias]
        public static void ConfigureHost(this ICakeContext context, HostSettings settings)
        {
            HostSettings instance = context.GetContainer().GetInstance<HostSettings>();

            foreach (User user in settings.Users)
            {
                instance.AddUser(user.Username, user.Password);
            }

            foreach (string key in settings.ApiKeys)
            {
                instance.AddApiKey(key);
            }

            instance.Host = settings.Host;
            instance.Port = settings.Port;
            instance.Url = settings.Url;

            context.Log.Information("Host configured.");
        }
    }
}

