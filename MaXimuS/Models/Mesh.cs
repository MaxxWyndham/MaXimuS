using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("MESH")]
    [HasEndTag("MESH")]
    public class Mesh
    {
        [DisplayName("TIMEVALUE")]
        public int TimeValue { get; set; }

        [DisplayName("MESH_NUMVERTEX")]
        [LinkedCount("MESH_VERTEX_LIST")]
        public int NumVertex { get; set; }

        [DisplayName("MESH_NUMFACES")]
        [LinkedCount("MESH_FACE_LIST")]
        [LinkedCount("MESH_NORMALS")]
        public int NumFaces { get; set; }

        [LinkedObject("MESH_VERTEX_LIST")]
        public MeshVertexList MeshVertexList { get; set; }

        [LinkedObject("MESH_FACE_LIST")]
        public MeshFaceList MeshFaceList { get; set; }

        [DisplayName("MESH_NUMTVERTEX")]
        [LinkedCount("MESH_TVERTLIST")]
        public int NumUVs { get; set; }

        [LinkedObject("MESH_TVERTLIST")]
        public MeshTVertList MeshTVertList { get; set; }

        [LinkedObject("MESH_MAPPINGCHANNEL")]
        public MeshMappingChannel MappingChannel { get; set; }

        [DisplayName("MESH_NUMTVFACES")]
        [LinkedCount("MESH_TFACELIST")]
        public int NumTextureFaces { get; set; }

        [LinkedObject("MESH_TFACELIST")]
        public MeshTFaceList MeshTFaceList { get; set; }

        [DisplayName("MESH_NUMCVERTEX")]
        [LinkedCount("MESH_CVERTLIST")]
        public int NumCVertex { get; set; }

        [LinkedObject("MESH_CVERTLIST")]
        public MeshCVertList MeshCVertList { get; set; }

        [DisplayName("MESH_NUMCVFACES")]
        [LinkedCount("MESH_CFACELIST")]
        public int? NumCFaces { get; set; }

        [LinkedObject("MESH_CFACELIST")]
        public MeshCFaceList MeshCFaceList { get; set; }

        [LinkedObject("MESH_NORMALS")]
        public MeshNormals MeshNormals { get; set; }
    }
}
