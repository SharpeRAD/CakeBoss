#region Using Statements
using CakeBoss.Host;
#endregion



namespace CakeBoss.Agent
{
    /// <summary>
    /// Settings used to configure the agent
    /// </summary>
    public class AgentSettings : HostSettings
    {
        #region Properties (2)
            /// <summary>
            /// Gets or sets if AWS termination time should be polled
            /// </summary>
            public bool EnableTerminationCheck { get; set; }

            /// <summary>
            /// Gets or sets if the nancy agent service should be enabled
            /// </summary>
            public bool EnableAPI { get; set; }
        #endregion
    }
}
