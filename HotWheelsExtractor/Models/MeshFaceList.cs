using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;

namespace HotWheels.Models
{
    [DisplayName("MESH_FACE_LIST")]
    public class MeshFaceList : ISelfReader
    {
        public List<Vector4> Faces { get; set; }

        public void Read(BinaryReader br, int count)
        {
            long currentPosition = br.BaseStream.Position;
            bool ascii = false;

            // test if this is ascii
            byte[] b = br.ReadBytes(13);

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
                b[12] == 0x45)
            {
                ascii = true;
            }

            br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);

            Faces = [];

            for (int i = 0; i < count; i++)
            {
                if (ascii)
                {
                    string[] parts = br.ReadLine().Split([' ', '\t'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                    Faces.Add(new Vector4(float.Parse(parts[3]), float.Parse(parts[5]), float.Parse(parts[7]), 0));
                }
                else
                {
                    Faces.Add(new Vector4(br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
                }
            }
        }
    }
}
