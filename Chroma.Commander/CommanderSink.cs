using System;
using Chroma.Diagnostics.Logging;
using Chroma.Diagnostics.Logging.Base;
using Chroma.Graphics;

namespace Chroma.Commander;

public class CommanderSink : Sink
{
    private InGameConsole _console;
    
    public CommanderSink(InGameConsole console)
    {
        _console = console;
    }
    
    public override void Write(LogLevel logLevel, string message, params object[] args)
    {
        _console.PushString(new ConsoleLine(message, logLevel switch
        {
            LogLevel.Info => Color.White,
            LogLevel.Warning => Color.Yellow,
            LogLevel.Error => Color.Red,
            LogLevel.Debug => Color.Blue,
            LogLevel.Exception => Color.Red,
            LogLevel.Everything => Color.White,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        }));
        if (args.Length != 1 || !(args[0] is Exception e))
            return;
    }
}