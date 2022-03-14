using System;

namespace Chroma.Commander;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class ConsoleHiddenAttribute : Attribute
{
    public ConsoleHiddenAttribute()
    {
    }
}