using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace MaXimuS.Models
{
    [DisplayName("MESH_CFACELIST")]
    public class MeshCFaceList : ISelfReader, ISelfWriter
    {
        public List<Vector3> CollisionFaces { get; set; }

        public void Read(BinaryReader br, int count)
        {
            long currentPosition = br.BaseStream.Position;
            bool wide = true;

            CollisionFaces = [];

            br.BaseStream.Seek(count * 6, SeekOrigin.Current);

            if (br.ReadByte() == 0x09 &&
                br.ReadByte() == 0x09 &&
                br.ReadByte() == 0x7D)
            {
                wide = false;
            }

            if (wide)
            {
                int n = 0;
            }

            br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);

            for (int i = 0; i < count; i++)
            {
                if (wide)
                {
                    CollisionFaces.Add(new Vector3(br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32()));
                }
                else
                {
                    CollisionFaces.Add(new Vector3(br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
                }
            }
        }

        public void Write(BinaryWriter bw) { }
    }
}
