using HotWheels;
using HotWheels.Models;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Threading;

TypeDescriptor.AddAttributes(typeof(Vector2), new TypeConverterAttribute(typeof(Vector2Converter)));
TypeDescriptor.AddAttributes(typeof(Vector3), new TypeConverterAttribute(typeof(Vector3Converter)));
TypeDescriptor.AddAttributes(typeof(Vector4), new TypeConverterAttribute(typeof(Vector4Converter)));

Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

process(AppDomain.CurrentDomain.BaseDirectory);

void process(string path)
{
    foreach (var item in new DirectoryInfo(path).GetFiles("*.mxs"))
    {
        Console.WriteLine($"Processing {item.FullName}...");

        MXS mxs = MXS.Load(item.FullName);

        string name = Path.GetFileNameWithoutExtension(item.FullName);

        using TextWriter mtl = new StreamWriter(Path.Combine(path, $"{name}.mtl"));

        for (int i = 0; i < mxs.MaterialList.MaterialCount; i++)
        {
            Material material = mxs.MaterialList.Materials[i];

            if (material.NumSubMaterials > 0)
            {
                for (int j = 0; j < material.NumSubMaterials; j++)
                {
                    if (material.SubMaterials[j].MapDiffuse is null) { continue; }

                    mtl.WriteLine($"newmtl {i}.{j}");
                    mtl.WriteLine("Ka 1.000000 1.000000 1.000000");
                    mtl.WriteLine("Kd 1.000000 1.000000 1.000000");
                    mtl.WriteLine("Ks 0.000000 0.000000 0.000000");
                    mtl.WriteLine($"map_Kd {Path.GetFileName(material.SubMaterials[j].MapDiffuse.Bitmap.Replace('"', ' '))}");
                }
            }
            else
            {
                if (material.MapDiffuse is null) { continue; }

                mtl.WriteLine($"newmtl {i}.0");
                mtl.WriteLine("Ka 1.000000 1.000000 1.000000");
                mtl.WriteLine("Kd 1.000000 1.000000 1.000000");
                mtl.WriteLine("Ks 0.000000 0.000000 0.000000");
                mtl.WriteLine($"map_Kd {Path.GetFileNameWithoutExtension(material.MapDiffuse.Bitmap.Replace('"', ' '))}.dds");
            }
        }

        mtl.Close();

        using TextWriter dw = new StreamWriter(Path.Combine(path, $"{name}.obj"));

        dw.WriteLine($"mtllib {name}.MTL");

        int vertoffset = 0;
        int tvertoffset = 0;

        string lastMaterial = string.Empty;

        foreach (GeomObject g in mxs.GeomObjects)
        {
            if (g.Name.StartsWith("\"chk", StringComparison.OrdinalIgnoreCase)) { continue; }
            if (g.Mesh is null) { continue; }

            dw.WriteLine($"o {g.Name}");

            Mesh mesh = g.Mesh;

            bool hasUVs = mesh.NumUVs > 0;


            if (hasUVs)
            {
                foreach (Vector2 uv in mesh.MeshTVertList.UVs)
                {
                    dw.WriteLine($"vt {uv.X} {uv.Y}");
                }
            }

            if (mesh.MeshNormals is not null)
            {
                foreach (Vector3 normal in mesh.MeshNormals.Normals)
                {
                    dw.WriteLine($"vn {normal.X} {normal.Y} {normal.Z}");
                }
            }

            foreach (Vector3 vert in mesh.MeshVertexList.Vertices)
            {
                dw.WriteLine($"v {-vert.X} {vert.Y} {vert.Z} 1");
            }

            for (int i = 0; i < mesh.NumFaces; i++)
            {
                Vector4 face = mesh.MeshFaceList.Faces[i];

                string m = $"{g.MaterialRef}.{(int)face.W}";

                if (lastMaterial != m)
                {
                    dw.WriteLine($"usemtl {m}");

                    lastMaterial = m;
                }

                if (hasUVs)
                {
                    Vector3 tface = mesh.MeshTFaceList.Faces[i];

                    dw.WriteLine($"f {face.X + 1 + vertoffset}/{tface.X + 1 + tvertoffset} {face.Y + 1 + vertoffset}/{tface.Y + 1 + tvertoffset} {face.Z + 1 + vertoffset}/{tface.Z + 1 + tvertoffset}");
                }
                else
                {
                    dw.WriteLine($"f {face.X + 1 + vertoffset} {face.Y + 1 + vertoffset} {face.Z + 1 + vertoffset}");
                }
            }

            vertoffset += mesh.NumVertex;
            tvertoffset += mesh.NumUVs;
        }

        dw.Close();
    }

    foreach (var folder in new DirectoryInfo(path).GetDirectories())
    {
        process(folder.FullName);
    }
}