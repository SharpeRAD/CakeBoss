#region Using Statements
    using System;
#endregion



namespace CakeBoss.Host
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields (1)
            private HostSettings _Settings;
        #endregion





        #region Constructor (1)
            public AuthenticationService(HostSettings settings)
            {
                if (settings == null)
                {
                    throw new ArgumentNullException("settings");
                }

                _Settings = settings;
            }
        #endregion





        #region Functions (2)
            public bool ValidUser(string username, string password)
            {
                return _Settings.ContainsUser(username, password);
            }

            public bool ValidApiKey(string apiKey)
            {
                return _Settings.ContainsApiKey(apiKey);
            }
        #endregion
    }
}
