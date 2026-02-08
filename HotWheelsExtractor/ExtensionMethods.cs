using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HotWheels
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
            } while (loop);

            return r;
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

                property.PropertyType.GetMethod("Add").Invoke(property.GetValue(o), new[] { Convert.ChangeType(propertyValue, property.PropertyType.GenericTypeArguments[0]) });
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
