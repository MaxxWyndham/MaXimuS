using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace MaXimuS.Models
{
    [DisplayName("MESH_TVERTLIST")]
    public class MeshTVertList : ISelfReader, ISelfWriter
    {
        public List<Vector2> UVs { get; set; }

        public void Read(BinaryReader br, int count)
        {
            long currentPosition = br.BaseStream.Position;
            int offset = count * 4 + 16;
            bool ascii = false;
            bool compressed = false;

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
                b[10] == 0x56 &&
                b[11] == 0x45 &&
                b[12] == 0x52 &&
                b[13] == 0x54)
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

            UVs = [];

            if (compressed)
            {
                Vector2 min = new(br.ReadSingle(), br.ReadSingle());
                Vector2 max = new(br.ReadSingle(), br.ReadSingle());
                Vector2 length = max - min;

                for (int i = 0; i < count; i++)
                {
                    float x = length.X * br.ReadUInt16() / 65535f;
                    float y = length.Y * br.ReadUInt16() / 65535f;

                    x += min.X;
                    y += min.Y;

                    UVs.Add(new Vector2(x, y));
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (ascii)
                    {
                        string[] parts = br.ReadLine().Split([' ', '\t'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                        UVs.Add(new Vector2(float.Parse(parts[2]), float.Parse(parts[3])));
                    }
                    else
                    {
                        UVs.Add(new Vector2(br.ReadSingle(), br.ReadSingle()));
                    }
                }
            }
        }

        public void Write(BinaryWriter bw) 
        {
            Vector2 min = new(UVs.Select(v => v.X).Min(), UVs.Select(v => v.Y).Min());
            Vector2 max = new(UVs.Select(v => v.X).Max(), UVs.Select(v => v.Y).Max());
            Vector2 length = max - min;

            bw.Write(min.X);
            bw.Write(min.Y);

            bw.Write(max.X);
            bw.Write(max.Y);

            foreach (Vector2 uv in UVs)
            {
                float x = (uv.X - min.X) / length.X * 65535f;
                float y = (uv.Y - min.Y) / length.Y * 65535f;

                bw.Write((ushort)x);
                bw.Write((ushort)y);
            }
        }
    }
}
