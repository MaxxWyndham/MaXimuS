using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

using MaXimuS.Models;

namespace MaXimuS
{
    public static class ExtensionMethods
    {
        public static string ReadLine(this BinaryReader br)
        {
            string r = "";
            bool loop = true;
            char c = (char)0;

            do
            {
                try
                {
                    c = br.ReadChar();
                }
                catch
                {
                    Console.WriteLine(br.BaseStream.Position.ToString("x2"));
                    return null;
                }

                if (c == 0xa)
                {
                    loop = false;
                }
                else if (c == 0xd)
                {
                }
                else
                {
                    r += c;
                }
            } while (loop && br.BaseStream.Position < br.BaseStream.Length);

            return r;
        }

        public static string ReadString(this BinaryReader br, int length)
        {
            if (length == 0) { return null; }

            char[] c = br.ReadChars(length);
            int l = length;

            for (int i = 0; i < length; i++)
            {
                if (c[i] == 0)
                {
                    l = i;
                    break;
                }
            }

            return new string(c, 0, l);
        }

        public static string ReadNullTerminatedString(this BinaryReader br)
        {
            string r = "";
            char c = (char)0;

            do
            {
                c = br.ReadChar();
                if (c > 0) { r += c; }
            } while (c > 0);

            return r;
        }

        public static void WriteObject(this BinaryWriter bw, object? o, int indent = 0, int? index = null)
        {
            if (o is null) { return; }

            string prefix = new('\t', indent);
            string objectName = o.GetType().GetCustomAttribute<DisplayNameAttribute>().DisplayName;

            if (index != null)
            {
                bw.Write($"{prefix}*{objectName} {index} {{\r\n".ToCharArray());
            }
            else
            {
                bw.Write($"{prefix}*{objectName} {{\r\n".ToCharArray());
            }

            if (o is IEnum)
            {
                foreach (string s in (o as IEnum).Values)
                {
                    bw.Write($"{prefix}\t{s}\r\n".ToCharArray());
                }
            }
            else if (o is ISelfWriter)
            {
                (o as ISelfWriter).Write(bw);
            }
            else
            {
                foreach (var p in o.GetType().GetProperties())
                {
                    if (p.GetCustomAttribute<LinkedObjectAttribute>() != null)
                    {
                        var pv = p.GetValue(o, null);

                        if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            var list = (IEnumerable)pv;
                            int i = 0;

                            foreach (var e in list)
                            {
                                bw.WriteObject(e, indent + 1, i++);
                            }
                        }
                        else
                        {
                            bw.WriteObject(pv, indent + 1);
                        }
                    }
                    else
                    {
                        var value = p.GetValue(o, null);
                        bool set = (p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null) != value;

                        if (p.PropertyType == typeof(float) || p.PropertyType == typeof(float?))
                        {
                            if (set) { bw.Write($"{prefix}\t*{p.GetCustomAttribute<DisplayNameAttribute>().DisplayName} {value:0.00000000}\r\n".ToCharArray()); }
                        }
                        else
                        {
                            if (set) { bw.Write($"{prefix}\t*{p.GetCustomAttribute<DisplayNameAttribute>().DisplayName} {value}\r\n".ToCharArray()); }
                        }

                    }
                }
            }

            HasEndTagAttribute? endTag = o.GetType().GetCustomAttribute<HasEndTagAttribute>();

            if (endTag != null)
            {
                bw.Write($"{prefix}\t*{endTag.Name}_END\r\n".ToCharArray());
            }

            bw.Write($"{prefix}}}\r\n".ToCharArray());
        }

        public static string DisplayName(this object o)
        {
            var displayName = o.GetType().GetCustomAttribute<DisplayNameAttribute>();

            return displayName is not null ? displayName.DisplayName : null;
        }

        public static int GetCount(this object parent, string name)
        {
            var properties = parent.GetType().GetProperties();

            foreach (var p in properties)
            {
                LinkedCountAttribute linkedCount = p.GetCustomAttributes<LinkedCountAttribute>().Where(a => a.LinkedCount == name).FirstOrDefault();

                if (linkedCount is not null)
                {
                    return (int)p.GetValue(parent, null);
                }
            }

            return 0;
        }

        public static void SetObject(this object o, object value)
        {
            string displayName = value.DisplayName();

            var property = o.GetType().GetProperties().Where(p => p.GetCustomAttribute<LinkedObjectAttribute>()?.LinkedObject == displayName).FirstOrDefault();

            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                property.PropertyType.GetMethod("Add").Invoke(property.GetValue(o, null), [value]);
            }
            else
            {
                property.SetValue(o, value, null);
            }
        }

        public static bool HasProperty(this object o, string propertyName)
        {
            return o.GetType().GetProperties().Any(p => p.Name == propertyName);
        }

        public static void SetProperty(this object o, string propertyName, object[] propertyValue)
        {
            if (propertyValue.Length == 0) { return; }

            var properties = o.GetType().GetProperties();

            PropertyInfo property = null;

            foreach (var p in properties)
            {
                DisplayNameAttribute displayName = p.GetCustomAttribute<DisplayNameAttribute>();

                if (displayName is not null && displayName.DisplayName == propertyName)
                {
                    property = p;
                    break;
                }
            }

            if (property is null)
            {
                Console.WriteLine($"Unknown {o.DisplayName()} Property: {propertyName} of value {string.Join(' ', propertyValue)}");
                throw new ArgumentException($"Unknown {o.DisplayName()} Property: {propertyName} of value {string.Join(' ', propertyValue)}");
            }

            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                if (property.GetValue(o) == null)
                {
                    property.SetValue(o, Activator.CreateInstance(typeof(List<>).MakeGenericType(property.PropertyType.GetGenericArguments())));
                }

                property.PropertyType.GetMethod("Add").Invoke(property.GetValue(o), [System.Convert.ChangeType(propertyValue, property.PropertyType.GenericTypeArguments[0])]);
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(property.PropertyType);

                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(o, string.Join(' ', propertyValue), null);
                }
                else if (propertyValue.Length == 1)
                {
                    property.SetValue(o, converter.ConvertFrom(propertyValue[0]), null);
                }
                else
                {
                    property.SetValue(o, converter.ConvertFrom(propertyValue), null);
                }
            }
        }
    }
}
