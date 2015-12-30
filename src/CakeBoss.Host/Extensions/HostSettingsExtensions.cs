#region Using Statements
    using System;
    using System.Linq;
#endregion



namespace CakeBoss.Host
{
    public static class HostSettingsExtensions
    {
        #region Functions (6)
            public static T AddUser<T>(this T settings, string username, string password) where T : HostSettings
            {
                settings.Users.Add(new User(username, password));

                return settings;
            }

            public static HostSettings AddApiKey<T>(this T settings, string key) where T : HostSettings
            {
                settings.ApiKeys.Add(key);

                return settings;
            }



            public static bool ContainsUser<T>(this T settings, string username, string password) where T : HostSettings
            {
                return (settings.Users.FirstOrDefault(u => u.Username == username && u.Password == password) != null);
            }

            public static bool ContainsApiKey<T>(this T settings, string apiKey) where T : HostSettings
            {
                return settings.ApiKeys.Contains(apiKey);
            }



            public static T SetUrl<T>(this T settings, string url) where T : HostSettings
            {
                settings.Url = url;

                return settings;
            }

            public static T SetPort<T>(this T settings, int port) where T : HostSettings
            {
                settings.Port = port;

                return settings;
            }
        #endregion
    }
}