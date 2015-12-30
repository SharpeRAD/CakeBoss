#region Using Statements
    using System;
    using System.Timers;
    using System.Net;
#endregion



namespace CakeBoss.Agent
{
    public class TerminationService : ITerminationService
    {
        #region Fields (3)
            private IAgentService _Agent;

            private Timer _Timer;
            private object _Lock;
        #endregion





        #region Constructor (1)
            public TerminationService(IAgentService agent)
            {
                if (agent == null)
                {
                    throw new ArgumentNullException("agent");
                }

                _Agent = agent;

                _Timer = new Timer(5000);
                _Timer.Elapsed += this.Tick;

                _Lock = new Object();
            }
        #endregion





        #region Properties (2)
            public bool Enabled
            {
                get
                {
                    return _Timer.Enabled;
                }
                set
                {
                    _Timer.Enabled = value;
                }
            }

            public int Interval
            {
                get
                {
                    return Convert.ToInt32(_Timer.Interval);
                }
                set
                {
                    _Timer.Interval = value;
                }
            }
        #endregion





        #region Functions (2)
            public void Tick(object sender, EventArgs args)
            {
                lock (_Lock)
                {
                    if (!_Timer.Enabled)
                    {
                        return;
                    }

                    if (this.Termination())
                    {
                        _Timer.Stop();
                        _Agent.Terminate();
                    }
                }
            }

            public bool Termination()
            {
                try
                {
                    string value = new WebClient().DownloadString(new Uri("http://169.254.169.254/latest/meta-data/spot/termination-time"));
                    return !String.IsNullOrEmpty(value);
                }
                catch
                {
                    return false;
                }
            }
        #endregion
    }
}
