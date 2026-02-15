using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

using MaXimuS.Models;

namespace MaXimuS
{
    public class MXS
    {
        [Flags]
        public enum ReadMode
        {
            ASCII,
            Compressed,
            Wide,
            Binary
        }

        [DisplayName("3DSMAX_ENGINE1EXPORT")]
        public int Engine1Export { get; set; }

        [DisplayName("3DSMAX_ENGINE1EXPORTFP")]
        public int Engine1ExportFP { get; set; }

        [DisplayName("3DSMAX_ENGINE1EXPORTB2")]
        public int Engine1ExportB2 { get; set; }

        public ReadMode FileReadMode
        {
            get
            {
                if (Engine1Export > 0) { return ReadMode.ASCII; }

                return ReadMode.Binary;
            }
        }

        [DisplayName("COMMENT")]
        public string? Comment { get; set; }

        [DisplayName("userunit")]
        public string? UserUnit { get; set; }

        [LinkedObject("SCENE")]
        public Scene? Scene { get; set; }

        [LinkedObject("MATERIAL_LIST")]
        public MaterialList? MaterialList { get; set; }

        [LinkedObject("HELPEROBJECT")]
        public List<HelperObject> HelperObjects { get; set; } = [];

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

                    if (string.IsNullOrEmpty(rawline)) { continue; }

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

        public void Save(string path)
        {
            FileInfo fi = new(path);

            using BinaryWriter bw = new(fi.OpenWrite());

            bw.BaseStream.SetLength(0);

            if (Engine1Export > 0) { bw.Write($"*3DSMAX_ENGINE1EXPORT\t{Engine1Export}\r\n".ToCharArray()); }
            if (Engine1ExportFP > 0) { bw.Write($"*3DSMAX_ENGINE1EXPORTFP\t{Engine1ExportFP}\r\n".ToCharArray()); }
            if (Engine1ExportB2 > 0) { bw.Write($"*3DSMAX_ENGINE1EXPORTB2\t{Engine1ExportB2}\r\n".ToCharArray()); }

            if (!string.IsNullOrEmpty(Comment)) { bw.Write($"*COMMENT {Comment}\r\n".ToCharArray()); }

            if (!string.IsNullOrEmpty(UserUnit)) { bw.Write($"*userunit {UserUnit}\n".ToCharArray()); }

            bw.WriteObject(Scene);
            bw.WriteObject(MaterialList);
            foreach (HelperObject ho in HelperObjects) { bw.WriteObject(ho); }
            foreach (GeomObject go in GeomObjects) { bw.WriteObject(go); }

            bw.Close();
        }

        public static object CreateObject(string name)
        {
            var models = Assembly.GetExecutingAssembly().GetTypes().Where(t => string.Equals(t.Namespace, "HotWheels.Models", StringComparison.OrdinalIgnoreCase) && t.IsClass).ToArray();

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

        public static string GenerateID()
        {
            var chars = "abcdef0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new(stringChars);
        }
    }
}