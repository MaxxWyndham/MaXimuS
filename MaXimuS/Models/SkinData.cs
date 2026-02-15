using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("SKIN_DATA")]
    public class SkinData : Node
    {
        [LinkedObject("BONE_LIST")]
        public BoneList BoneList { get; set; }

        [LinkedObject("SKIN_VERTEX_DATA")]
        public SkinVertexData SkinVertexData { get; set; }
    }
}
