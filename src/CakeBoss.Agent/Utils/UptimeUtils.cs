#region Using Statements
    using System;
    using System.Diagnostics;
#endregion



namespace CakeBoss.Agent
{
    public static class UptimeUtils
    {
        #region Functions (2)
            public static TimeSpan GetUptime()
            {
                using (var uptime = new PerformanceCounter("System", "System Up Time"))
                {
                    uptime.NextValue();

                    return TimeSpan.FromSeconds(uptime.NextValue());
                }
            }

            public static bool IsStartup()
            {
                return GetUptime() < new TimeSpan(0, 5, 0);
            }
        #endregion
    }
}
