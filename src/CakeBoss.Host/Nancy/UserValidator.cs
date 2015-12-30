#region Using Statements
    using System;

    using Nancy;
    using Nancy.Security;
    using Nancy.Authentication.Basic;
#endregion



namespace CakeBoss.Host
{
    public class UserValidator : IUserValidator
    {
        #region Fields (1)
            private IAuthenticationService _AuthenticationService;
        #endregion





        #region Constructor (1)
            public UserValidator(IAuthenticationService authenticationService)
            {
                if (authenticationService == null)
                {
                    throw new ArgumentNullException("authenticationService");
                }

                _AuthenticationService = authenticationService;
            }
        #endregion





        #region Functions (1)
            public IUserIdentity Validate(string username, string password)
            {
                if (_AuthenticationService.ValidUser(username, password))
                {
                    return new UserIdentity(username);
                }
                else
                {
                    return null;
                }
            }
        #endregion
    }
}