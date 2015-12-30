#region Using Statements
    using System;
#endregion



namespace CakeBoss.Host
{
    public class User
    {
        #region Constructor (1)
            public User(string username, string password)
            {
                this.Username = username;
                this.Password = password;
            }
        #endregion





        #region Properties (2)
            public string Username { get; set; }

            public string Password { get; set; }
        #endregion
    }
}