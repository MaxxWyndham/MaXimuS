using System;

namespace MaXimuS
{
    public class HasEndTagAttribute : Attribute
    {
        public string Name { get; set; }

        public HasEndTagAttribute(string v)
        {
            Name = v;
        }
    }
}
