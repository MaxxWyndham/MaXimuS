using System;

namespace MaXimuS
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    internal class LinkedCountAttribute : Attribute
    {
        public string LinkedCount { get; set; }

        public LinkedCountAttribute(string v)
        {
            LinkedCount = v;
        }
    }
}