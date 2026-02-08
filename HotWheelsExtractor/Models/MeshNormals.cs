using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;

namespace HotWheels.Models
{
    [DisplayName("MESH_NORMALS")]
    public class MeshNormals : ISelfReader
    {
        public List<Vector3> Normals { get; set; }

        public void Read(BinaryReader br, int count)
        {
            long currentPosition = br.BaseStream.Position;
            int offset = count * 9;
            bool compressed = false;
            bool ascii = false;

            // test if this is ascii
            byte[] b = br.ReadBytes(19);

            if (b[0] == 0x09 &&
                b[1] == 0x09 &&
                b[2] == 0x09 &&
                b[3] == 0x2A &&
                b[4] == 0x4D &&
                b[5] == 0x45 &&
                b[6] == 0x53 &&
                b[7] == 0x48 &&
                b[8] == 0x5F &&
                b[9] == 0x46 &&
                b[10] == 0x41 &&
                b[11] == 0x43 &&
                b[12] == 0x45 &&
                b[13] == 0x4E &&
                b[14] == 0x4f &&
                b[15] == 0x52 &&
                b[16] == 0x4d &&
                b[17] == 0x41 &&
                b[18] == 0x4c)
            {
                ascii = true;
            }

            br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);

            if (!ascii)
            {
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

            Normals = [];

            if (compressed)
            {
                Vector3 length = Vector3.One;

                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        float x = length.X * br.ReadByte() / 255f;
                        float y = length.Y * br.ReadByte() / 255f;
                        float z = length.Z * br.ReadByte() / 255f;

                        x -= 0.5f;
                        y -= 0.5f;
                        z -= 0.5f;

                        Normals.Add(new Vector3(x, y, z));
                    }
                }

            }
            else
            {
                if (ascii)
                {
                    for (int i = 0; i < count; i++)
                    {
                        // face normal
                        br.ReadLine();

                        // vertex normals
                        for (int j = 0; j < 3; j++)
                        {
                            string[] parts = br.ReadLine().Split([' ', '\t'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                            Normals.Add(new Vector3(float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4])));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Normals.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));
                        }
                    }
                }
            }
        }
    }
}
