using System;
using System.Collections.Generic;
using System.IO;

namespace MaXimuS.Formats
{
    public class OBJ
    {
        public MTL Materials { get; set; }

        public List<Vector3> Vertices { get; set; }

        public List<Vector3> Normals { get; set; }

        public List<Vector2> UVs { get; set; }

        public List<Face> Faces { get; set; }

        public List<string> Objects { get; set; }

        public static OBJ Load(string path)
        {
            OBJ obj = new()
            {
                Objects = [],
                Vertices = [],
                Normals = [],
                UVs = [],
                Faces = []
            };

            string[] lines = File.ReadAllLines(path);
            string name = "default";
            string? material = null;

            foreach (string line in lines)
            {
                if (line.StartsWith('#')) { continue; }

                string[] parts = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                switch (parts[0].ToLower())
                {
                    case "mtllib":
                        obj.Materials = MTL.Load(Path.Combine(Path.GetDirectoryName(path), string.Join(' ', parts, 1, parts.Length - 1)));
                        break;

                    case "o":
                        name = string.Join(" ", parts, 1, parts.Length - 1);

                        obj.Objects.Add(name);
                        break;

                    case "v":
                        obj.Vertices.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;

                    case "f":
                        string[] f1 = parts[1].Split('/');
                        string[] f2 = parts[2].Split('/');
                        string[] f3 = parts[3].Split('/');

                        obj.Faces.Add(new Face
                        {
                            Name = name,
                            Material = material,
                            V1 = int.Parse(f1[0]),
                            V2 = int.Parse(f2[0]),
                            V3 = int.Parse(f3[0]),
                            UV1 = f1.Length > 1 ? int.Parse(f1[1]) : 0,
                            UV2 = f2.Length > 1 ? int.Parse(f2[1]) : 0,
                            UV3 = f3.Length > 1 ? int.Parse(f3[1]) : 0,
                            N1 = f1.Length > 2 ? int.Parse(f1[2]) : 0,
                            N2 = f2.Length > 2 ? int.Parse(f2[2]) : 0,
                            N3 = f3.Length > 2 ? int.Parse(f3[2]) : 0
                        });
                        break;

                    case "vt":
                        obj.UVs.Add(new Vector2(float.Parse(parts[1]), float.Parse(parts[2])));
                        break;

                    case "vn":
                        obj.Normals.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;

                    case "s":
                        // ?
                        break;

                    case "usemtl":
                        material = string.Join(" ", parts, 1, parts.Length - 1);
                        break;

                    default:
                        Console.WriteLine($"Unknown: {parts[0].ToLower()}");
                        break;
                }
            }

            return obj;
        }
    }

    public class Face
    {
        public string Name { get; set; }

        public string? Material { get; set; }

        public int V1 { get; set; }

        public int V2 { get; set; }

        public int V3 { get; set; }

        public int UV1 { get; set; }

        public int UV2 { get; set; }

        public int UV3 { get; set; }

        public int N1 { get; set; }

        public int N2 { get; set; }

        public int N3 { get; set; }
    }
}
