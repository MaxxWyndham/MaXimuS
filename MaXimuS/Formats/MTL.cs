using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MaXimuS.Formats
{
    public class MTL
    {
        public List<Material> Materials { get; set; } = [];

        public static MTL Load(string path)
        {
            MTL mtl = new();

            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith('#')) { continue; }

                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                switch (parts[0].ToLower())
                {
                    case "newmtl":
                        mtl.Materials.Add(new() { Name = string.Join(' ', parts, 1, parts.Length - 1) });
                        break;

                    case "ka":
                        mtl.Materials.Last().AmbientColour = new Vector3(parts[1], parts[2], parts[3]);
                        break;

                    case "kd":
                        mtl.Materials.Last().DiffuseColour = new Vector3(parts[1], parts[2], parts[3]);
                        break;

                    case "ks":
                        mtl.Materials.Last().SpecularColour = new Vector3(parts[1], parts[2], parts[3]);
                        break;

                    case "map_kd":
                        mtl.Materials.Last().Texture = string.Join(' ', parts, 1, parts.Length - 1);
                        break;

                    default:
                        Console.WriteLine($"Unknown MTL: {parts[0]}");
                        break;
                }
            }

            return mtl;
        }
    }

    public class Material
    {
        public string Name { get; set; }

        public Vector3 AmbientColour { get; set; }

        public Vector3 DiffuseColour { get; set; }

        public Vector3 SpecularColour { get; set; }

        public string Texture { get; set; }
    }
}
