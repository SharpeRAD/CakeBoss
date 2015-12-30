#region Using Statements
    using System;
#endregion



namespace CakeBoss.Host
{
    public class UpdateCommand
    {
        #region Constructor (1)
            public UpdateCommand()
            {

            }
        #endregion





        #region Properties (3)
            public string Group { get; set; }



            public string Name { get; set; }

            public string Contents { get; set; }
        #endregion
    }
}