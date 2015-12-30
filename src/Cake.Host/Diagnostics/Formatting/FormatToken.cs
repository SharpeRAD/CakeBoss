namespace Cake.Host.Diagnostics.Formatting
{
    public abstract class FormatToken
    {
        public abstract string Render(object[] args);
    }
}