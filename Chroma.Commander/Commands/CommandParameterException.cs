using System;

namespace Chroma.Commander;

public class CommandParameterException : Exception
{
    public CommandParameterException()
    {
    }

    public CommandParameterException(string? message) : base(message)
    {
    }

    public CommandParameterException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}