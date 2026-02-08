using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;

namespace HotWheels.Models
{
    [DisplayName("MESH_VERTEX_LIST")]
    public class MeshVertexList : ISelfReader
    {
        public List<Vector3> Vertices { get; set; }

        public void Read(BinaryReader br, int count)
        {
            long currentPosition = br.BaseStream.Position;
            int offset = count * 6 + 24;
            bool compressed = false;
            bool ascii = false;

            // test if this is ascii
            byte[] b = br.ReadBytes(15);

            if (b[0] == 0x09 &&
                b[1] == 0x09 &&
                b[2] == 0x09 &&
                b[3] == 0x2A &&
                b[4] == 0x4D &&
                b[5] == 0x45 &&
                b[6] == 0x53 &&
                b[7] == 0x48 &&
                b[8] == 0x5F &&
                b[9] == 0x56 &&
                b[10] == 0x45 &&
                b[11] == 0x52 &&
                b[12] == 0x54 &&
                b[13] == 0x45 &&
                b[14] == 0x58)
            {
                ascii = true;
            }

            br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);

            if (!ascii)
            {
                // then check if it's compressed

                br.BaseStream.Seek(offset, SeekOrigin.Current);

                byte[] endOfBlock = [0x09, 0x09, 0x7D, 0x0D];

                if (br.ReadByte() == endOfBlock[0] &&
                    br.ReadByte() == endOfBlock[1] &&
                    br.ReadByte() == endOfBlock[2] &&
                    br.ReadByte() == endOfBlock[3])
                {
                    compressed = true;
                }

                br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            }

            Vertices = [];

            if (compressed)
            {
                Vector3 min = new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                Vector3 max = new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                Vector3 length = max - min;

                for (int i = 0; i < count; i++)
                {
                    float x = length.X * br.ReadUInt16() / 65535f;
                    float y = length.Y * br.ReadUInt16() / 65535f;
                    float z = length.Z * br.ReadUInt16() / 65535f;

                    x += min.X;
                    y += min.Y;
                    z += min.Z;

                    Vertices.Add(new Vector3(x, y, z));
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (ascii)
                    {
                        string[] parts = br.ReadLine().Split([' ', '\t'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                        Vertices.Add(new Vector3(float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4])));
                    }
                    else
                    {
                        Vertices.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));
                    }
                }
            }
        }
    }
}
