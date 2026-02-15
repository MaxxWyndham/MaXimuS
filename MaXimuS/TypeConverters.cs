using System.ComponentModel;

namespace MaXimuS
{
    class Vector2Converter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value != null)
            {
                return new Vector2(float.Parse(((string[])value)[0]), float.Parse(((string[])value)[1]));
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    class Vector3Converter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value != null)
            {
                return new Vector3(float.Parse(((string[])value)[0]), float.Parse(((string[])value)[1]), float.Parse(((string[])value)[2]));
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    class Vector4Converter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value != null)
            {
                return new Vector4(float.Parse(((string[])value)[0]), float.Parse(((string[])value)[1]), float.Parse(((string[])value)[2]), float.Parse(((string[])value)[3]));
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
