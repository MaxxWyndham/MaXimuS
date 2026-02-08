using HotWheels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HotWheels
{
    public class MXS
    {
        [DisplayName("3DSMAX_ENGINE1EXPORT")]
        public int Engine1Export { get; set; }

        [DisplayName("3DSMAX_ENGINE1EXPORTFP")]
        public int Engine1ExportFP { get; set; }

        [DisplayName("3DSMAX_ENGINE1EXPORTB2")]
        public int Engine1ExportB2 { get; set; }

        [DisplayName("COMMENT")]
        public string? Comment { get; set; }

        [DisplayName("userunit")]
        public string? UserUnit { get; set; }

        [LinkedObject("SCENE")]
        public Scene Scene { get; set; }

        [LinkedObject("MATERIAL_LIST")]
        public MaterialList MaterialList { get; set; }

        [LinkedObject("HELPEROBJECT")]
        public HelperObject HelperObject { get; set; }

        [LinkedObject("BIPEDOBJECT")]
        public BipedObject BipedObject { get; set; }

        [LinkedObject("BONEOBJECT")]
        public BoneObject BoneObject { get; set; }

        [LinkedObject("SKIN_DATA")]
        public SkinData SkinData { get; set; }

        [LinkedObject("SHAPEOBJECT")]
        public ShapeObject ShapeObject { get; set; }

        [LinkedObject("CAMERAOBJECT")]
        public CameraObject CameraObject { get; set; }

        [LinkedObject("LIGHTOBJECT")]
        public LightObject LightObject { get; set; }

        [LinkedObject("GEOMOBJECT")]
        public List<GeomObject> GeomObjects { get; set; } = [];

        public static MXS Load(string path)
        {
            FileInfo fi = new(path);
            MXS mxs = new();

            Stack<object> stack = [];
            object currentObject = mxs;

            stack.Push(mxs);

            using (BinaryReader br = new(fi.OpenRead()))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    string rawline = br.ReadLine();
                    string line = rawline.Trim();

                    if (line.StartsWith('*'))
                    {
                        if (line.EndsWith('{'))
                        {
                            string[] parts = line.Substring(1).Split([' ', '\t'], StringSplitOptions.TrimEntries);
                            string objectName = parts[0];

                            currentObject = CreateObject(objectName);

                            if (currentObject is IInlineCount)
                            {
                                (currentObject as IInlineCount).Count = int.Parse(parts[1]);
                            }

                            stack.Peek().SetObject(currentObject);
                            stack.Push(currentObject);
                        }
                        else
                        {
                            string[] parts = line.Substring(1).Split([' ', '\t'], StringSplitOptions.TrimEntries);

                            if (currentObject is ISelfClose && !currentObject.HasProperty(parts[0]))
                            {
                                stack.Pop();

                                currentObject = stack.Peek();
                            }
                            

                            currentObject.SetProperty(parts[0], parts[1..]);
                        }
                    }
                    else if (line.Trim() == "}")
                    {
                        stack.Pop();

                        currentObject = stack.Peek();
                    }
                    else
                    {
                        if (currentObject is IEnum)
                        {
                            (currentObject as IEnum).Values.Add(line.Trim());
                        }
                        else
                        {
                            Console.WriteLine($"Unexpected: {line}");
                            throw new ArgumentException($"Unexpected: {line}");
                        }
                    }

                    if (currentObject.GetType().GetInterfaces().Any(x => x == typeof(ISelfReader)))
                    {
                        string displayName = currentObject.DisplayName();
                        int count = stack.Where(o => o.GetType().GetProperties().Where(p => p.GetCustomAttributes<LinkedCountAttribute>().Where(a => a.LinkedCount == displayName).Any()).Any()).First().GetCount(displayName);

                        currentObject.GetType().GetMethod("Read").Invoke(currentObject, [br, count]);
                    }
                }
            }

            return mxs;
        }

        public static object CreateObject(string name)
        {
            var models = Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "HotWheels.Models", StringComparison.OrdinalIgnoreCase) && t.IsClass).ToArray();

            foreach (var model in models)
            {
                if (model.GetCustomAttribute<DisplayNameAttribute>().DisplayName == name)
                {
                    return Activator.CreateInstance(Type.GetType($"HotWheels.Models.{model.Name}", true, true));
                }
            }

            Console.WriteLine($"Unknown Model: {name}");
            throw new ArgumentException($"Unknown Model: {name}");
        }
    }
}