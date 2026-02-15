using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("GEOMOBJECT")]
    public class GeomObject
    {
        [DisplayName("NODE_ID")]
        public string? ID { get; set; }

        [DisplayName("NODE_PARENTID")]
        public string? ParentID { get; set; }

        [DisplayName("NODE_NAME")]
        public string? Name { get; set; }

        [DisplayName("NODE_PARENT")]
        public string? Parent { get; set; }

        [LinkedObject("NODE_USERPROPERTIES")]
        public NodeUserProperties? NodeUserProperties { get; set; }

        [LinkedObject("NODE_TM")]
        public NodeTM? NodeTM { get; set; }

        [LinkedObject("MESH")]
        public Mesh? Mesh { get; set; }

        [DisplayName("PROP_MOTIONBLUR")]
        public int MotionBlur { get; set; }

        [DisplayName("PROP_CASTSHADOW")]
        public int CastShadow { get; set; }

        [DisplayName("PROP_RECVSHADOW")]
        public int RecvShadow { get; set; }

        [LinkedObject("TM_ANIMATION")]
        public TMAnimation TMAnimation { get; set; }

        [LinkedObject("NODE_VISIBILITY_TRACK")]
        public NodeVisibilityTrack? NodeVisibilityTrack { get; set; }

        [DisplayName("MATERIAL_REF")]
        public int MaterialRef { get; set; }

        [DisplayName("WIREFRAME_COLOR")]
        public Vector3? WireframeColour { get; set; }
    }
}
