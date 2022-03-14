using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chroma.Commander;

public class CommandRegistry : ICommandRegistry
{
    public Dictionary<string, List<MethodInfo>> Commands;
    public Dictionary<string, PropertyInfo> ConProps;
    public Dictionary<string, FieldInfo> ConFields;
    public Dictionary<Type, List<MethodInfo>> Converters;
    public List<string> HiddenKeys;

    private Assembly _assembly;
    private bool _helpCommand;

    internal CommandRegistry(Assembly assembly, bool addHelp = true)
    {
        _assembly = assembly;
        _helpCommand = addHelp;
        RefreshItems();
    }

    public void RefreshItems()
    {
        Commands = new();
        ConProps = new();
        ConFields = new();
        Converters = new();
        HiddenKeys = new();

        var types = _assembly.GetTypes().Concat(Assembly.GetExecutingAssembly().GetTypes());
        foreach (var type in types)
        {
            // Add Converters
            foreach (var converterMethod in type.GetMethods(BindingFlags.NonPublic |
                                                            BindingFlags.Public |
                                                            BindingFlags.Instance |
                                                            BindingFlags.Static))
            {
                var attr = converterMethod.GetCustomAttribute<TypeConverterAttribute>();
                if (attr is not null)
                {
                    if (!Converters.ContainsKey(converterMethod.ReturnType))
                        Converters.Add(converterMethod.ReturnType, new List<MethodInfo>() { converterMethod });
                    else
                        Converters[converterMethod.ReturnType].Add(converterMethod);
                }
            }

            foreach (var method in type.GetMethods(BindingFlags.Static |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.Public))
            {
                var command = method.GetCustomAttribute<ConsoleCommandAttribute>();
                if (command is not null)
                {
                    if (!Commands.ContainsKey(command.Trigger))
                        Commands.Add(command.Trigger, new() { method });
                    else
                        Commands[command.Trigger].Add(method);

                    var hidden = method.GetCustomAttribute<ConsoleHiddenAttribute>();
                    if (hidden is not null)
                        HiddenKeys.Add(command.Trigger);
                }
            }

            foreach (var property in type.GetProperties(BindingFlags.Static |
                                                        BindingFlags.NonPublic |
                                                        BindingFlags.Public))
            {
                var prop = property.GetCustomAttribute<ConsoleVariableAttribute>();
                if (prop is not null)
                {
                    ConProps.Add(prop.Name, property);

                    var hidden = property.GetCustomAttribute<ConsoleHiddenAttribute>();
                    if (hidden is not null)
                        HiddenKeys.Add(prop.Name);
                }
            }

            foreach (var field in type.GetFields(BindingFlags.Static |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Public))
            {
                var fld = field.GetCustomAttribute<ConsoleVariableAttribute>();
                if (fld is not null)
                {
                    ConFields.Add(fld.Name, field);

                    var hidden = field.GetCustomAttribute<ConsoleHiddenAttribute>();
                    if (hidden is not null)
                        HiddenKeys.Add(fld.Name);
                }
            }
        }

        if (_helpCommand)
        {
            Commands.Add("help", new()
            {
                GetType().GetMethod(nameof(HelpCommand), BindingFlags.NonPublic | BindingFlags.Instance)
            });
        }
    }

    // TODO: This sucks so bad I hate it I hate it I hate it I hate it I hate it I hate it I hate it I hate it I ha
    internal string HelpCommand()
    {
        var cmdHeader = "=== Commands ===";
        var varHeader = "=== ConVars ===";
        var typeHeader = "=== Declaring Type ===";
        var firstColumn = new List<string>() { cmdHeader }.Concat(Commands
                .Where(kvp => kvp.Key != "help" && !HiddenKeys.Contains(kvp.Key))
                .Select(c => $"{c.Key}({GetSignature(c.Value.OrderBy(m => m.GetParameters().Length).Last())})"))
            .Concat(new List<string>() { varHeader })
            .Concat(ConFields.Where(c => !HiddenKeys.Contains(c.Key)).Select(c => c.Key))
            .Concat(ConProps.Where(c => !HiddenKeys.Contains(c.Key)).Select(c => c.Key));
        var secondColumn = new List<string>() { typeHeader }.Concat(Commands
                .Where(kvp => kvp.Key != "help" && !HiddenKeys.Contains(kvp.Key))
                .Select(c => $"| {c.Value.First().DeclaringType}"))
            .Concat(new List<string>() { typeHeader })
            .Concat(ConFields.Where(c => !HiddenKeys.Contains(c.Key)).Select(c => c.Value.DeclaringType!.ToString()))
            .Concat(ConProps.Where(c => !HiddenKeys.Contains(c.Key)).Select(c => c.Value.DeclaringType!.ToString()));

        var pad = firstColumn.OrderBy(s => s.Length).Last().Length;

        return string.Join('\n', firstColumn.Zip(secondColumn, (a, b) => a.PadRight(pad) + b));
    }

    internal string GetSignature(MethodInfo m)
        => string.Join(", ",
            m.GetParameters().Select(p =>
                $"{p.ParameterType.Name} {p.Name}" + (p.HasDefaultValue ? " = " + p.DefaultValue : "")));

    internal ParameterInfo[] GetRequiredParameters(MethodInfo m)
        => m.GetParameters().Where(p => !p.HasDefaultValue).ToArray();

    internal object[] FillOptionals(object[] orig, MethodInfo m)
    {
        var parameters = m.GetParameters();
        var args = new object[parameters.Length];
        for (var i = 0; i < args.Length; i++)
        {
            if (i < orig.Length)
                args[i] = ConvertObjToType(orig[i], parameters[i].ParameterType);
            else if (parameters[i].HasDefaultValue)
                args[i] = parameters[i].DefaultValue!;
            else
                throw new ArgumentException("Not enough arguments provided");
        }

        return args;
    }

    public string? GetAutoComplete(string input, int offset) =>
        Commands.Keys.Concat(ConProps.Keys).Concat(ConFields.Keys).Where(s => !HiddenKeys.Contains(s) && s.StartsWith(input)).OrderBy(s => s)
            .ElementAtOrDefault(offset);

    public string Call(string name, params object[] args)
    {
        try
        {
            if (Commands.ContainsKey(name))
            {
                var toCall = Commands[name].LastOrDefault(c => GetRequiredParameters(c).Length <= args.Length);
                if (toCall is null)
                    return "Not enough arguments.";

                return ObjToString(toCall.Invoke(this, FillOptionals(args, toCall))?.ToString() ?? string.Empty);
            }
            else if (ConProps.ContainsKey(name))
            {
                if (args.Length != 0)
                    ConProps[name].SetValue(null, ConvertObjToType(args[0], ConProps[name].PropertyType));

                return ObjToString(ConProps[name].GetValue(this));
            }
            else if (ConFields.ContainsKey(name))
            {
                if (args.Length != 0)
                    ConFields[name].SetValue(null, ConvertObjToType(args[0], ConFields[name].FieldType));

                return ObjToString(ConFields[name].GetValue(this));
            }

            return "This command was not recognized.";
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    private object ConvertObjToType(object obj, Type type)
    {
        try
        {
            if (type.IsEnum)
                return obj is string s ? Enum.Parse(type, s) : Enum.ToObject(type, (int)obj);

            return Converters[type].First().Invoke(obj, new[] { obj })!;
        }
        catch (TargetInvocationException e)
        {
            throw new CommandParameterException(
                $"\"{obj}\" is not parseable as required parameter type \"{type.Name}\"", e);
        }
    }

    private string ObjToString(object? obj)
    {
        if (obj is null)
            return "null";

        if (Converters.First(kvp => kvp.Key == typeof(string)).Value
            .Any(m => m.GetParameters().First().ParameterType == obj.GetType()))
        {
            return (string)Converters.First(kvp => kvp.Key == typeof(string)).Value
                .First(m => m.GetParameters().First().ParameterType == obj.GetType())
                .Invoke(obj, new[] { obj }) ?? string.Empty;
        }

        return obj.ToString()!;
    }
}