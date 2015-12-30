#region Using Statements
    using System;
#endregion



namespace CakeBoss.Agent
{
    public interface ITerminationService
    {
        #region Properties (2)
            bool Enabled { get; set; }

            int Interval { get; set; }
        #endregion





        #region Functions (1)
            bool Termination();
        #endregion
    }
}
