using System.Collections.Generic;
using System.Reflection;

namespace Chroma.Commander
{
    public class CommandRegistry
    {
        public Dictionary<string, MethodInfo> Commands;
        public Dictionary<string, PropertyInfo> ConVars;

        internal CommandRegistry()
        {
        }
    }
}