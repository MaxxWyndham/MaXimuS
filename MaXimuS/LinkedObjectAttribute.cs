using System;

namespace MaXimuS
{
    internal class LinkedObjectAttribute : Attribute
    {
        public string LinkedObject { get; set; }

        public LinkedObjectAttribute(string v)
        {
            LinkedObject = v;
        }
    }
}