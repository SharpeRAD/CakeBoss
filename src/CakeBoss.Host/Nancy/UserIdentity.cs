#region Using Statements
    using System;
    using System.Collections.Generic;

    using Nancy.Security;
#endregion



namespace CakeBoss.Host
{
    public class UserIdentity : IUserIdentity
    {
        #region Constructor (2)
            public UserIdentity()
            {

            }

            public UserIdentity(string username)
            {
                this.UserName = username;
            }
        #endregion





        #region Properties (2)
            public string UserName { get; set; }

            public IEnumerable<string> Claims { get; set; }
        #endregion
    }
}