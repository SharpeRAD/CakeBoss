using Cake.Core.Scripting;

namespace Cake.Host.Scripting.Mono
{
    /// <summary>
    /// Mono script host proxy.
    /// </summary>
    public class MonoScriptHostProxy
    {
        /// <summary>
        /// Gets or sets the script host.
        /// </summary>
        /// <value>The script host.</value>
        public static IScriptHost ScriptHost { get; set; }
    }
}