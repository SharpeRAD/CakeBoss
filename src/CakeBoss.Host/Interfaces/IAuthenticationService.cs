#region Using Statements
    using System;
#endregion



namespace CakeBoss.Host
{
    public interface IAuthenticationService
    {
        bool ValidUser(string username, string password);

        bool ValidApiKey(string apiKey);
    }
}