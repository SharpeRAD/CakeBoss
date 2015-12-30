#region Using Statements
    using System;
    using System.Collections.Generic;

    using LightInject;
    using Cake.Core;
#endregion



namespace CakeBoss.Host
{
    public static class CakeContextExtensions
    {
        #region Functions (1)
            public static IServiceContainer Container;
        #endregion





        #region Functions (3)
            public static IServiceContainer GetContainer(this ICakeContext context)
            {
                return Container;
            }

            public static TService GetInstance<TService>(this ICakeContext context)
            {
                return context.GetContainer().GetInstance<TService>();
            }

            public static TService GetInstance<TService>(this ICakeContext context, string serviceName)
            {
                return context.GetContainer().GetInstance<TService>(serviceName);
            }
        #endregion
    }
}