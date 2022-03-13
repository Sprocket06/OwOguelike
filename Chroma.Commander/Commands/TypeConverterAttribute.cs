using System;

namespace Chroma.Commander;

[AttributeUsage(AttributeTargets.Method)]
public class TypeConverterAttribute : Attribute
{
    public TypeConverterAttribute()
    {
    }
}