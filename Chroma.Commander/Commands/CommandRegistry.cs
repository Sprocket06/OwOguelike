using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chroma.Commander
{
    public class CommandRegistry
    {
        public Dictionary<string, MethodInfo> Commands;
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
            Commands = new Dictionary<string, MethodInfo>();
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
                        Commands.Add(command.Trigger, method);
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
            
            if(_helpCommand)
                Commands.Add("help", GetType().GetMethod(nameof(HelpCommand), BindingFlags.NonPublic | BindingFlags.Instance));
        }

        internal string HelpCommand()
        {
            return "=== Commands ===\n" +
                   string.Concat(Commands.Select(c => c.Key != "help" ? 
                       $"{c.Key}({string.Join(", ", c.Value.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))}) | {c.Value.DeclaringType}\n"
                       : "")) +
                   "=== ConVars ===\n" +
                   string.Concat(ConFields.Select(c => $"{c.Key} | {c.Value.DeclaringType}\n")) +
                   string.Concat(ConProps.Select(c => $"{c.Key} | {c.Value.DeclaringType}\n"));
        }

        internal string Call(string name, params object[] args)
        {
            try
            {
                if (Commands.ContainsKey(name))
                {
                    var pAmount = Commands[name].GetParameters().Length;
                    return Commands[name].Invoke(this, args[0..pAmount])?.ToString();
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