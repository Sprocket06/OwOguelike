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

        internal CommandRegistry(Assembly assembly)
        {
            _assembly = assembly;
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
        }

        internal string Call(string name, params object[] args)
        {
            try
            {
                if (Commands.ContainsKey(name))
                {
                    var pAmount = Commands[name].GetParameters().Length;
                    return Commands[name].Invoke(null, args[0..pAmount])?.ToString();
                }
                else if (ConProps.ContainsKey(name))
                {
                    if (args.Length != 0)
                        ConProps[name].SetValue(null, CastObjTo(args[0], ConProps[name].PropertyType));

                    return ToString(ConProps[name].GetValue(null));
                }
                else if (ConFields.ContainsKey(name))
                {
                    if (args.Length != 0)
                        ConFields[name].SetValue(null, CastObjTo(args[0], ConFields[name].FieldType));
                    
                    return ToString(ConFields[name].GetValue(null));
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