#region Using Statements
    using System;
    using System.Collections.Generic;

    using Cake.Core.Diagnostics;
    using Cake.Core.IO;
#endregion



namespace CakeBoss.Host
{
    /// <summary>
    /// Output from the cake console
    /// </summary>
    public sealed class ConsoleOutput
    {
        #region Constructor (1)
            public ConsoleOutput(string text, ConsoleColor color)
            {
                this.Text = text;
                this.ForegroundColor = color;
            }
        #endregion





        #region Properties (2)
            /// <summary>
            /// Gets or sets the text
            /// </summary>
            /// <value>The text.</value>
            public string Text { get; set; }

            /// <summary>
            /// Gets or sets the foreground color.
            /// </summary>
            /// <value>The foreground color.</value>
            public ConsoleColor ForegroundColor { get; set; }
        #endregion
    }
}
