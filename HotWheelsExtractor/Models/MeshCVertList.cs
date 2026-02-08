using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;

namespace HotWheels.Models
{
    [DisplayName("MESH_CVERTLIST")]
    public class MeshCVertList : ISelfReader
    {
        public List<Vector3> CollisionVertices { get; set; }

        public void Read(BinaryReader br, int count)
        {
            CollisionVertices = [];

            for (int i = 0; i < count; i++)
            {
                CollisionVertices.Add(new Vector3(br.ReadByte(), br.ReadByte(), br.ReadByte()));
            }
        }
    }
}
