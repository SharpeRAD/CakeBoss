#region Using Statements
    using System;
    using System.Collections.Generic;

    using Cake.Core.IO;
#endregion



namespace Cake.CakeBoss
{
    /// <summary>
    /// The settings used when running a remote target
    /// </summary>
    public class RemoteSettings
    {
        #region Fields (1)
            private string _Url;
        #endregion





        #region Constructor (1)
            /// <summary>
            /// Initializes a new instance of the <see cref="RemoteSettings" /> class.
            /// </summary>
            public RemoteSettings()
            {
                this.Host = "localhost";
                this.Port = 8080;

                _Url = "";
            }
        #endregion





        #region Properties (5)
            /// <summary>
            /// Gets or sets the credentials to use when connecting
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// Gets or sets the credentials to use when connecting
            /// </summary>
            public string Password { get; set; }



            /// <summary>
            /// Gets or sets the computer to connect to
            /// </summary>
            public string Host { get; set; }

            /// <summary>
            /// Gets or sets the remote port to connect on
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Gets or sets the full URI to connect to
            /// </summary>
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