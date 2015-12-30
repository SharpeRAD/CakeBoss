using System.Collections.Generic;

namespace Cake.Host.Arguments
{
    /// <summary>
    /// Represents an argument parser.
    /// </summary>
    public interface IArgumentParser
    {
        /// <summary>
        /// Parses the specified arguments.
        /// </summary>
        /// <param name="args">The arguments to parse.</param>
        /// <returns>A <see cref="CakeOptions"/> instance representing the arguments.</returns>
        CakeOptions Parse(IEnumerable<string> args);
    }
}