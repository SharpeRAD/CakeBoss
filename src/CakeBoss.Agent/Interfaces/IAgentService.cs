#region Using Statements
    using System;
#endregion



namespace CakeBoss.Agent
{
    public interface IAgentService
    {
        #region Properties (1)
            string CurrentTask { get; set; }
        #endregion





        #region Functions (5)
            //Events
            void Start();

            void Stop();

            void Shutdown();

            void Terminate();



            //Run
            bool RunTarget(string target, bool load);
        #endregion
    }
}
