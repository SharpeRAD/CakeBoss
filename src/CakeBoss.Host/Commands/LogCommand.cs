#region Using Statements
    using System;
#endregion



namespace CakeBoss.Host
{
    public class LogCommand
    {
        #region Constructor (1)
            public LogCommand()
            {

            }
        #endregion





        #region Properties (2)
            public string Client { get; set; }

            public ConsoleOutput Output { get; set; }
        #endregion
    }
}