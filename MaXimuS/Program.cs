using MaXimuS;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

TypeDescriptor.AddAttributes(typeof(Vector2), new TypeConverterAttribute(typeof(Vector2Converter)));
TypeDescriptor.AddAttributes(typeof(Vector3), new TypeConverterAttribute(typeof(Vector3Converter)));
TypeDescriptor.AddAttributes(typeof(Vector4), new TypeConverterAttribute(typeof(Vector4Converter)));

Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

if (args.Length > 0)
{
    MaXimuS.Convert.FromModel(args);
}
else
{
    Batch.Process(AppDomain.CurrentDomain.BaseDirectory);
}

//W3D w3d = W3D.Load(@"D:\HotwheelsGames\carena\TCR2TOGOTEXTUREBANK.W3D");


