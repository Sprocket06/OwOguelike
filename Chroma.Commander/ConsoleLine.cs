using System;
using Chroma.Graphics;

namespace Chroma.Commander;

public class ConsoleLine
{
    public string Line;
    public Color Color;

    public ConsoleLine(string str, Color? clr = null)
    {
        Line = str;
        Color = clr ?? Color.White;
    }

    public static implicit operator string(ConsoleLine l) => l.Line;
}