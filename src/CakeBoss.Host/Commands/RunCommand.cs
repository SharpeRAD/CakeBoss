#region Using Statements
    using System;
    using System.Collections.Generic;
#endregion



namespace CakeBoss.Host
{
    public class RunCommand
    {
        #region Constructor (1)
            public RunCommand()
            {

            }
        #endregion





        #region Properties (3)
            public string Group { get; set; }



            public string Name { get; set; }

            public IDictionary<string, string> Arguments { get; set; }
        #endregion
    }
}