namespace Chroma.Commander;

public class BuiltinTypeConverters
{
    [TypeConverter]
    public static string StringConvert(string input) => input;
    
    [TypeConverter]
    public static int Int32Convert(string input) => int.Parse(input);

    [TypeConverter]
    public static short Int16Convert(string input) => short.Parse(input);

    [TypeConverter]
    public static byte ByteConvert(string input) => byte.Parse(input);

    [TypeConverter]
    public static float FloatConvert(string input) => float.Parse(input);

    [TypeConverter]
    public static bool BoolConvert(string input) => input.ToLower() switch
    {
        "true" or "1" => true,
        "false" or "0" => false,
        _ => throw new CommandParameterException($"{input} is not a boolean type.")
    };

    [TypeConverter]
    public static string BoolToString(bool input) => input ? "1" : "0";
}