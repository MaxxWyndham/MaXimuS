using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace MaXimuS.Models
{
    [DisplayName("MESH_TFACELIST")]
    public class MeshTFaceList : ISelfReader, ISelfWriter
    {
        public List<Vector3> Faces { get; set; }

        public void Read(BinaryReader br, int count)
        {
            long currentPosition = br.BaseStream.Position;
            bool wide = true;
            bool ascii = false;

            // test if this is ascii
            byte[] b = br.ReadBytes(14);

            if (b[0] == 0x09 &&
                b[1] == 0x09 &&
                b[2] == 0x09 &&
                b[3] == 0x2A &&
                b[4] == 0x4D &&
                b[5] == 0x45 &&
                b[6] == 0x53 &&
                b[7] == 0x48 &&
                b[8] == 0x5F &&
                b[9] == 0x54 &&
                b[10] == 0x46 &&
                b[11] == 0x41 &&
                b[12] == 0x43 &&
                b[13] == 0x45)
            {
                ascii = true;
                wide = false;
            }

            br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);

            if (!ascii)
            {
                br.BaseStream.Seek(count * 6, SeekOrigin.Current);

                if (br.ReadByte() == 0x09 &&
                    br.ReadByte() == 0x09 &&
                    br.ReadByte() == 0x7D)
                {
                    wide = false;
                }

                br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            }

            Faces = [];

            for (int i = 0; i < count; i++)
            {
                if (ascii)
                {
                    string[] parts = br.ReadLine().Split([' ', '\t'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                    Faces.Add(new Vector3(int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4])));
                }
                else
                {
                    if (wide)
                    {
                        Faces.Add(new Vector3(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
                    }
                    else
                    {
                        Faces.Add(new Vector3(br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
                    }
                }
            }
        }

        public void Write(BinaryWriter bw) 
        {
            foreach (Vector3 face in Faces)
            {
                bw.Write((ushort)face.X);
                bw.Write((ushort)face.Y);
                bw.Write((ushort)face.Z);
            }
        }
    }
}
