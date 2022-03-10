using System;

namespace Chroma.Commander
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConsoleVariableAttribute : Attribute
    {
        public string Name { get; }

        public ConsoleVariableAttribute(string name)
        {
            Name = name;
        }
    }
}