using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chroma.Commander
{
    public class CommandRegistry
    {
        public Dictionary<string, List<MethodInfo>> Commands;
        public Dictionary<string, PropertyInfo> ConProps;
        public Dictionary<string, FieldInfo> ConFields;

        private Assembly _assembly;
        private bool _helpCommand;

        internal CommandRegistry(Assembly assembly, bool addHelp = true)
        {
            _assembly = assembly;
            _helpCommand = addHelp;
            RefreshItems();
        }

        internal void RefreshItems()
        {
            Commands = new Dictionary<string, List<MethodInfo>>();
            ConProps = new Dictionary<string, PropertyInfo>();
            ConFields = new Dictionary<string, FieldInfo>();

            var types = _assembly.GetTypes();
            foreach (var type in types)
            {
                foreach (var method in type.GetMethods(BindingFlags.Static |
                                                       BindingFlags.NonPublic |
                                                       BindingFlags.Public))
                {
                    var command = method.GetCustomAttribute<ConsoleCommandAttribute>();
                    if (command != null)
                    {
                        if (!Commands.ContainsKey(command.Trigger))
                            Commands.Add(command.Trigger, new() { method });
                        else
                            Commands[command.Trigger].Add(method);
                    }
                }

                foreach (var property in type.GetProperties(BindingFlags.Static |
                                                            BindingFlags.NonPublic |
                                                            BindingFlags.Public))
                {
                    var prop = property.GetCustomAttribute<ConsoleVariableAttribute>();
                    if (prop != null)
                    {
                        ConProps.Add(prop.Name, property);
                    }
                }

                foreach (var field in type.GetFields(BindingFlags.Static |
                                                     BindingFlags.NonPublic |
                                                     BindingFlags.Public))
                {
                    var fld = field.GetCustomAttribute<ConsoleVariableAttribute>();
                    if (fld != null)
                    {
                        ConFields.Add(fld.Name, field);
                    }
                }
            }

            if (_helpCommand)
                Commands.Add("help",
                    new()
                    {
                        GetType().GetMethod(nameof(HelpCommand), BindingFlags.NonPublic | BindingFlags.Instance)
                    });
        }

        // TODO: This sucks so bad I hate it I hate it I hate it I hate it I hate it I hate it I hate it I hate it I ha
        internal string HelpCommand()
        {
            var cmdHeader = "=== Commands ===";
            var varHeader = "=== ConVars ===";
            var typeHeader = "=== Declaring Type ===";
            var firstColumn = new List<string>() {cmdHeader}.Concat(Commands.Where(kvp => kvp.Key != "help")
                .Select(c => $"{c.Key}({GetSignature(c.Value.OrderBy(m => m.GetParameters().Length).Last())})"))
                .Concat(new List<string>() {varHeader})
                .Concat(ConFields.Select(c => c.Key))
                .Concat(ConProps.Select(c => c.Key));
            var secondColumn = new List<string>() {typeHeader}.Concat(Commands.Where(kvp => kvp.Key != "help")
                .Select(c => $"| {c.Value.First().DeclaringType}"))
                .Concat(new List<string>() {typeHeader})
                .Concat(ConFields.Select(c => c.Value.DeclaringType!.ToString()))
                .Concat(ConProps.Select(c => c.Value.DeclaringType!.ToString()));

            var pad = firstColumn.OrderBy(s => s.Length).Last().Length;

            return string.Join('\n', firstColumn.Zip(secondColumn, (a, b) => a.PadRight(pad) + b));
        }

        internal string GetSignature(MethodInfo m) 
            => string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}" + (p.HasDefaultValue ? " = " + p.DefaultValue : "")));

        internal ParameterInfo[] GetRequiredParameters(MethodInfo m) 
            => m.GetParameters().Where(p => !p.HasDefaultValue).ToArray();

        internal object[] FillOptionals(object[] orig, MethodInfo m)
        {
            var parameters = m.GetParameters();
            var args = new object[parameters.Length];
            for (var i = 0; i < args.Length; i++)
            {
                if (i < orig.Length)
                    args[i] = orig[i];
                else if (parameters[i].HasDefaultValue)
                    args[i] = parameters[i].DefaultValue!;
                else
                    throw new ArgumentException("Not enough arguments provided");
            }

            return args;
        }

        internal string Call(string name, params object[] args)
        {
            try
            {
                if (Commands.ContainsKey(name))
                {
                    var toCall = Commands[name].LastOrDefault(c => GetRequiredParameters(c).Length <= args.Length);
                    if (toCall is null)
                        return "Not enough arguments.";
                    
                    return toCall.Invoke(this, FillOptionals(args, toCall))?.ToString() ?? string.Empty;
                }
                else if (ConProps.ContainsKey(name))
                {
                    if (args.Length != 0)
                        ConProps[name].SetValue(null, CastObjTo(args[0], ConProps[name].PropertyType));

                    return ToString(ConProps[name].GetValue(this));
                }
                else if (ConFields.ContainsKey(name))
                {
                    if (args.Length != 0)
                        ConFields[name].SetValue(null, CastObjTo(args[0], ConFields[name].FieldType));

                    return ToString(ConFields[name].GetValue(this));
                }

                return "This command was not recognized.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string ToString(object obj)
        {
            if (obj is bool b)
            {
                return b ? "1" : "0";
            }
            else
            {
                return obj.ToString();
            }
        }

        private object CastObjTo(object obj, Type type)
        {
            if (type == typeof(bool))
            {
                if (obj is int i)
                {
                    return i >= 1;
                }
                else if (obj is float f)
                {
                    return f >= 1;
                }
                else if (obj is string s)
                {
                    return s.ToLower() switch
                    {
                        "true" => true,
                        "false" => false,
                        _ => obj
                    };
                }
            }

            return obj;
        }
    }
}