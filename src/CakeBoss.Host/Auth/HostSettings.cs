#region Using Statements
    using System;
    using System.Collections.Generic;

    using Cake.Core.IO;
#endregion



namespace CakeBoss.Host
{
    public class HostSettings
    {
        #region Fields (1)
            private string _Url;
        #endregion





        #region Constructor (1)
            public HostSettings()
            {
                this.Users = new List<User>();
                this.ApiKeys = new List<string>();

                this.Host = "localhost";
                this.Port = 8080;

                _Url = "";
            }
        #endregion





        #region Properties (4)
            public IList<User> Users { get; set; }

            public IList<string> ApiKeys { get; set; }



            public string Host { get; set; }

            public int Port { get; set; }



            public string Url
            {
                set
                {
                    _Url = value;
                }
                get
                {
                    if (!String.IsNullOrEmpty(_Url))
                    {
                        return _Url;
                    }
                    else
                    {
                        return "http://" + this.Host + ":" + this.Port.ToString();
                    }
                }
            }
        #endregion
    }
}