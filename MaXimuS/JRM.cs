using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MaXimuS
{
    public class JRM
    {
        public JRMModel Model { get; set; }

        public List<string> Materials { get; set; } = [];

        public static JRM Load(string path)
        {
            FileInfo fi = new(path);
            JRM jrm = new();

            using BinaryReader br = new(fi.OpenRead());

            int u1 = br.ReadInt16();
            int u3 = br.ReadInt16();
            int u4 = br.ReadInt16();

            int childIndex = 0;
            JRMModel? model = null;
            JRMModel? parent = null;

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                string node = br.ReadString(4);
                int length = br.ReadInt32();
                int index = 0;

                //Console.Write($"{node} : {length} : {br.BaseStream.Position - 8:x2}");

                switch (node)
                {
                    case "FRAM":
                        //Console.WriteLine();

                        model = jrm.Model = new();
                        break;

                    case "MDLN":
                        string[] parts = br.ReadString(length).Split(',', StringSplitOptions.RemoveEmptyEntries);

                        //Console.WriteLine($" : {parts[0]}");

                        if (!model.Children.Any(c => c.Name == parts[0]))
                        {
                            model.Children.Add(new()
                            {
                                Name = parts[0],
                                Position = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])),
                                Rotation = new Vector3(float.Parse(parts[4]), float.Parse(parts[5]), float.Parse(parts[6])),
                                Scale = new Vector3(float.Parse(parts[7]), float.Parse(parts[8]), float.Parse(parts[9]))
                            });

                            model = model.Children.Last();
                        }
                        else
                        {
                            model = model.Children.First(m => m.Name == parts[0]);
                        }
                        break;

                    case "MTEX":
                        index = br.ReadInt32();

                        //Console.WriteLine($" : {index}");

                        model.MaterialId = index;

                        string materialName = br.ReadString(length);

                        if (jrm.Materials.Count < index) { jrm.Materials.Add(materialName); }
                        break;

                    case "VERT":
                        index = br.ReadInt32();

                        //Console.WriteLine($" : {index}");

                        if (index > model.Meshes.Count) { model.Meshes.Add(new()); }

                        for (int i = 0; i < length; i++)
                        {
                            model.Meshes[index - 1].Vertices.Add(new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));
                        }
                        break;

                    case "TXUV":
                        index = br.ReadInt32();

                        //Console.WriteLine($" : {index}");

                        for (int i = 0; i < length; i++)
                        {
                            model.Meshes[index - 1].UVs.Add(new(br.ReadSingle(), br.ReadSingle()));
                        }
                        break;

                    case "FACE":
                        index = br.ReadInt32();

                        //Console.WriteLine($" : {index}");

                        for (int i = 0; i < length; i++)
                        {
                            model.Meshes[index - 1].Faces.Add(new(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
                        }
                        break;

                    case "CHLD":
                        if (length == 0)
                        {
                            childIndex++;

                            if (childIndex == jrm.Model.Children[0].ChildCount)
                            {
                                model = jrm.Model;

                                childIndex = 0;
                            }
                            else
                            {
                                model = parent;
                            }

                            continue;
                        }

                        model.ChildCount = length;

                        for (int i = 0; i < length; i++)
                        {
                            model.Children.Add(new() { Name = br.ReadNullTerminatedString() });
                        }

                        for (int i = 0; i < length; i++)
                        {
                            model.Children[i].Position = new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            model.Children[i].Rotation = new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            model.Children[i].Scale = new (br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                        }

                        childIndex = 0;
                        parent = model;
                        break;

                    default:
                        //Console.WriteLine($"{node} : {length} : {br.BaseStream.Position - 8:x2}");

                        br.BaseStream.Seek(0, SeekOrigin.End);
                        break;
                }
            }

            br.Close();

            return jrm;
        }
    }

    public class JRMModel
    {
        public string Name { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Scale { get; set; }

        public int MaterialId { get; set; }

        public int ChildCount { get; set; }

        public List<JRMModel> Children { get; set; } = [];

        public List<JRMMesh> Meshes { get; set; } = [];
    }

    public class JRMMesh
    {
        public List<Vector3> Vertices { get; set; } = [];

        public List<Vector2> UVs { get; set; } = [];

        public List<Vector3> Faces { get; set; } = [];
    }
}
