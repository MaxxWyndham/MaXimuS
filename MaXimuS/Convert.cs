using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MaXimuS.Formats;
using MaXimuS.Models;

namespace MaXimuS
{
    public class Convert
    {
        public static void FromModel(string[] paths)
        {
            string? objPath = paths.Where(p => p.EndsWith(".obj", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            string? mtlPath = paths.Where(p => p.EndsWith(".mtl", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (string.IsNullOrEmpty(objPath))
            {
                MaXimuS.Warn("Expected an OBJ file!");
                return;
            }

            if (string.IsNullOrEmpty(mtlPath))
            {
                MaXimuS.Warn("Expected an MTL file!");
                return;
            }

            string name = Path.GetFileNameWithoutExtension(objPath);

            OBJ obj = OBJ.Load(objPath);

            string[] points = ["_bk", "_bl", "_bm", "_br", "_fl", "_fr", "_ft", "_hd", "_lt", "_rf", "_rt", "_tk"];
            string[] wheels = ["wheel_frontleft", "wheel_frontright", "wheel_backleft", "wheel_backright"];

            foreach (string point in points)
            {
                if (!obj.Objects.Any(o => o.EndsWith(point)))
                {
                    MaXimuS.Warn($"OBJ missing regpoint!  Expected {point}");
                    return;
                }
            }

            foreach (string wheel in wheels)
            {
                if (!obj.Objects.Any(o => o == wheel))
                {
                    MaXimuS.Warn($"OBJ missing wheel!  Expected {wheel}");
                    return;
                }
            }

            string rootId = MXS.GenerateID();

            MXS mxs = new()
            {
                Engine1ExportFP = 200,
                Comment = $"\"MaXimuS v{MaXimuS.Version} - {DateTime.Now:ddd MMM dd HH:mm:ss yyyy}\"",
                UserUnit = "per sysunit 0.032808",
                Scene = new() { FileName = $"\"{name}.obj\"", },
                MaterialList = new() { Materials = [] },
                HelperObjects =
                {
                    new()
                    {
                        ID = $"\"{rootId}\"",
                        Name = $"\"{name}_regpoints\"",
                        HelperClass = "\"Dummy\"",
                        NodeUserProperties= new() { Values = { "castshadows" } },
                        NodeTM = new()
                        {
                            Name = $"\"{name}_regpoints\"",
                            Row0 = new(1, 0, 0),
                            Row1 = new(0, 1, 0),
                            Row2 = new(0, 0, 1),
                            Row3 = Vector3.Zero,
                            Position = Vector3.Zero
                        },
                        BoundingBoxMin = new Vector3(obj.Vertices.Min(v => v.X), obj.Vertices.Min(v => v.Y), obj.Vertices.Min(v => v.Z)),
                        BoundingBoxMax = new Vector3(obj.Vertices.Max(v => v.X), obj.Vertices.Max(v => v.Y), obj.Vertices.Max(v => v.Z))
                    }
                }
            };

            List<string> materialLookup = [];

            Models.Material m = new()
            {
                Name = $"\"{name}\"",
                Class = "\"Multi/Sub-Object\"",
                Ambient = Vector3.One,
                Diffuse = new Vector3(0.70588237f),
                Specular = new Vector3(0.89999998f),
                SubMaterials = []
            };

            foreach (var material in obj.Materials.Materials)
            {
                materialLookup.Add(material.Name);

                m.SubMaterials.Add(new()
                {
                    Name = $"\"{material.Name}\"",
                    Class = "\"Standard\"",
                    Ambient = Vector3.One,
                    Diffuse = new Vector3(0.70588237f),
                    Specular = new Vector3(0.89999998f),
                    Shading = "Blinn",
                    XPFalloff = 0,
                    Selfillum = 0,
                    Falloff = "In",
                    XPType = "Filter",
                    MapDiffuse = new()
                    {
                        Name = $"\"{material.Name}\"",
                        Class = "\"Bitmap\"",
                        SubNo = 1,
                        Amount = 1f,
                        Bitmap = $"\"{material.Texture}\"",
                        Type = "Screen",
                        UTiling = 1,
                        VTiling = 1,
                        UVWrapFlags = 3,
                        Row0 = new(1, 0, 0),
                        Row1 = new(0, 1, 0),
                        Row2 = new(0, 0, 1),
                        Row3 = new(0, 0, 0),
                        Blur = 1,
                        NoiseAmount = 1,
                        NoiseSize = 1,
                        NoiseLevel = 1,
                        Filter = "Pyramidal"
                    },
                    MapReflect = new()
                    {
                        Name = $"\"{material.Name} #12\"",
                        Class = "\"Bitmap\"",
                        SubNo = 9,
                        Amount = 1f,
                        Bitmap = $"\"@{name}.dds\"",
                        Type = "Spherical",
                        UTiling = 1,
                        VTiling = 1,
                        UVWrapFlags = 3,
                        Row0 = new(1, 0, 0),
                        Row1 = new(0, 1, 0),
                        Row2 = new(0, 0, 1),
                        Row3 = new(0, 0, 0),
                        Blur = 1,
                        NoiseAmount = 1,
                        NoiseSize = 1,
                        NoiseLevel = 1,
                        Filter = "Pyramidal"
                    }
                });

                if (Path.GetFileNameWithoutExtension(material.Texture).EndsWith("-o"))
                {
                    m.SubMaterials.Last().MapOpacity = new()
                    {
                        Name = $"\"{material.Name} #24\"",
                        Class = "\"Bitmap\"",
                        SubNo = 6,
                        Amount = 1f,
                        Bitmap = $"\"{material.Texture}\"",
                        Type = "Screen",
                        UTiling = 1,
                        VTiling = 1,
                        UVWrapFlags = 3,
                        Row0 = new(1, 0, 0),
                        Row1 = new(0, 1, 0),
                        Row2 = new(0, 0, 1),
                        Row3 = new(0, 0, 0),
                        Blur = 1,
                        NoiseAmount = 1,
                        NoiseSize = 1,
                        NoiseLevel = 1,
                        Filter = "Pyramidal"
                    };
                }
            }

            m.NumSubMaterials = m.SubMaterials.Count;
            mxs.MaterialList.Materials.Add(m);
            mxs.MaterialList.MaterialCount = mxs.MaterialList.Materials.Count;

            int offset = 0;
            int uvOffset = 0;

            List<GeomObject> meshes = [];

            foreach (string o in obj.Objects)
            {
                string end = o[^3..];

                GeomObject go = new()
                {
                    ID = $"\"{MXS.GenerateID()}\"",
                    Name = $"\"{o}\"",
                    NodeTM = new()
                    {
                        Name = $"\"{o}\"",
                        Row0 = new(1, 0, 0),
                        Row1 = new(0, 1, 0),
                        Row2 = new(0, 0, 1),
                        Row3 = Vector3.Zero,
                        Position = Vector3.Zero
                    },
                    CastShadow = 1,
                    RecvShadow = 1
                };

                if (points.Contains(end))
                {
                    go.ParentID = $"\"{rootId}\"";
                    go.Parent = $"\"{name}_regpoints\"";
                }

                if (!o.Contains("wheel")) { go.NodeUserProperties = new() { Values = { "castshadows" } }; }

                Mesh mesh = new()
                {
                    MeshVertexList = new() { Vertices = [] },
                    MeshFaceList = new() { Faces = [] },
                    MeshTVertList = new() { UVs = [] },
                    MeshTFaceList = new() { Faces = [] },
                    MeshNormals = new() { Normals = [] }
                };

                var faces = obj.Faces.Where(f => f.Name == o);

                int minDex = Math.Min(faces.Min(f => f.V1), Math.Min(faces.Min(f => f.V2), faces.Min(f => f.V3)));
                int maxDex = Math.Max(faces.Max(f => f.V1), Math.Max(faces.Max(f => f.V2), faces.Max(f => f.V3)));

                for (int i = minDex; i <= maxDex; i++)
                {
                    var v = obj.Vertices[i - 1];

                    mesh.MeshVertexList.Vertices.Add(new(-v.X, v.Y, v.Z));
                }

                minDex = Math.Min(faces.Min(f => f.UV1), Math.Min(faces.Min(f => f.UV2), faces.Min(f => f.UV3)));
                maxDex = Math.Max(faces.Max(f => f.UV1), Math.Max(faces.Max(f => f.UV2), faces.Max(f => f.UV3)));

                for (int i = minDex; i <= maxDex; i++)
                {
                    mesh.MeshTVertList.UVs.Add(obj.UVs[i - 1]);
                }

                foreach (var face in faces)
                {
                    int materialId = Math.Max(materialLookup.IndexOf(face.Material), 0);

                    mesh.MeshFaceList.Faces.Add(new Vector4(face.V1 - 1 - offset, face.V2 - 1 - offset, face.V3 - 1 - offset, materialId));

                    mesh.MeshTFaceList.Faces.Add(new Vector3(face.UV1 - 1 - uvOffset, face.UV2 - 1 - uvOffset, face.UV3 - 1 - uvOffset));

                    mesh.MeshNormals.Normals.Add(obj.Normals[face.N1 - 1]);
                    mesh.MeshNormals.Normals.Add(obj.Normals[face.N2 - 1]);
                    mesh.MeshNormals.Normals.Add(obj.Normals[face.N3 - 1]);
                }

                mesh.NumVertex = mesh.MeshVertexList.Vertices.Count;
                mesh.NumFaces = mesh.MeshFaceList.Faces.Count;
                mesh.NumUVs = mesh.MeshTVertList.UVs.Count;
                mesh.NumTextureFaces = mesh.MeshTFaceList.Faces.Count;

                go.MaterialRef = 0;

                Vector3 min = new(mesh.MeshVertexList.Vertices.Min(v => v.X), mesh.MeshVertexList.Vertices.Min(v => v.Y), mesh.MeshVertexList.Vertices.Min(v => v.Z));
                Vector3 max = new(mesh.MeshVertexList.Vertices.Max(v => v.X), mesh.MeshVertexList.Vertices.Max(v => v.Y), mesh.MeshVertexList.Vertices.Max(v => v.Z));
                Vector3 centre = new(
                        (min.X + max.X) / 2f,
                        (min.Y + max.Y) / 2f,
                        (min.Z + max.Z) / 2f
                    );

                go.NodeTM.Row3 = new(centre.X, centre.Z, centre.Y);
                go.NodeTM.Position = go.NodeTM.Row3;

                offset += mesh.NumVertex;
                uvOffset += mesh.NumUVs;

                go.Mesh = mesh;

                meshes.Add(go);
            }

            int index = -1;

            // now, add the meshes to the mxs in the correct order

            // first the regpoints
            foreach (string point in points)
            {
                index = meshes.FindIndex(m => m.Name.EndsWith($"{point}\""));
                mxs.GeomObjects.Add(meshes[index]);
                meshes.RemoveAt(index);
            }

            // then the primary mesh
            index = meshes.FindIndex(m => m.Name == $"\"{name}\"");
            mxs.GeomObjects.Add(meshes[index]);
            meshes.RemoveAt(index);

            // other meshes
            string[] others = meshes.Where(m => !m.Name.Contains("wheel")).Select(m => m.Name).ToArray();
            foreach (string other in others)
            {
                index = meshes.FindIndex(m => m.Name == other);
                mxs.GeomObjects.Add(meshes[index]);
                meshes.RemoveAt(index);
            }

            // then wheels
            foreach (string wheel in wheels)
            {
                index = meshes.FindIndex(m => m.Name == $"\"{wheel}\"");
                mxs.GeomObjects.Add(meshes[index]);
                meshes.RemoveAt(index);
            }

            string path = Path.GetDirectoryName(objPath);

            Directory.CreateDirectory(Path.Combine(path, name));

            mxs.Save(Path.Combine(path, name, $"{name}.mxs"));
        }
    }
}
