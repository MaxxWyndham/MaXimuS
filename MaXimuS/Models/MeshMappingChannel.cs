using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("MESH_MAPPINGCHANNEL")]
    public class MeshMappingChannel : ISelfClose
    {
        [DisplayName("MESH_NUMTVERTEX")]
        [LinkedCount("MESH_TVERTLIST")]
        public int NumUVs { get; set; }

        [LinkedObject("MESH_TVERTLIST")]
        public MeshTVertList MeshTVertList { get; set; }

        [DisplayName("MESH_NUMTVFACES")]
        [LinkedCount("MESH_TFACELIST")]
        public int NumTextureFaces { get; set; }

        [LinkedObject("MESH_TFACELIST")]
        public MeshTFaceList MeshTFaceList { get; set; }
    }
}
