#region Using Statements
    using System;
    using System.Collections.Generic;

    using Serilog;
    using Cake.Core;
#endregion



namespace CakeBoss.Host
{
    public sealed class HostConsole : IConsole
    {
        #region Fields (3)
            private ConsoleColor _ForegroundColor;
            private ConsoleColor _BackgroundColor;

            private IList<ConsoleOutput> _Output;
        #endregion





        #region Constructor (1)
            public HostConsole()
            {
                _Output = new List<ConsoleOutput>();

                this.ResetColor();
            }
        #endregion





        #region Properties (2)
            /// <summary>
            /// Gets or sets the foreground color.
            /// </summary>
            /// <value>The foreground color.</value>
            public ConsoleColor ForegroundColor
            {
                get
                {
                    return _ForegroundColor;
                }
                set
                {
                    _ForegroundColor = value;
                }
            }

            /// <summary>
            /// Gets or sets the background color.
            /// </summary>
            /// <value>The background color.</value>
            public ConsoleColor BackgroundColor
            {
                get
                {
                    return _BackgroundColor;
                }
                set
                {
                    _BackgroundColor = value;
                }
            }
        #endregion





        #region Functions (4)
            /// <summary>
            /// Writes the text representation of the specified array of objects to the
            /// console output using the specified format information.
            /// </summary>
            /// <param name="format">A composite format string</param>
            /// <param name="arg">An array of objects to write using format.</param>
            public void Write(string format, params object[] arg)
            {
                this.CreateOutput(String.Format(format, arg), this.ForegroundColor);
            }

            /// <summary>
            /// Writes the text representation of the specified array of objects, followed
            /// by the current line terminator, to the console output using the specified
            /// format information.
            /// </summary>
            /// <param name="format">A composite format string</param>
            /// <param name="arg">An array of objects to write using format.</param>
            public void WriteLine(string format, params object[] arg)
            {
                this.CreateOutput(String.Format(format, arg) + Environment.NewLine, this.ForegroundColor);
            }



            /// <summary>
            /// Sets the foreground and background console colors to their defaults.
            /// </summary>
            public void ResetColor()
            {
                _ForegroundColor = ConsoleColor.White;
                _BackgroundColor = ConsoleColor.DarkBlue;
            }



            private void CreateOutput(string text, ConsoleColor color)
            {
                /*if (!String.IsNullOrEmpty(text))
                {
                    //Format
                    text = text.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);

                    //Build
                    ConsoleOutput output = new ConsoleOutput(text, color);
                    _Output.Add(output);

                    //Flush
                    if (text.Contains(Environment.NewLine))
                    {
                        string info = "";

                        foreach (ConsoleOutput part in _Output)
                        {
                            info += part.Text;
                        }

                        Log.Logger.Information(info);
                        _Output.Clear();
                    }
                }*/
            }
        #endregion
    }
}
