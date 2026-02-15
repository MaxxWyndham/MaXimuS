using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("HELPEROBJECT")]
    [HasEndTag("HELPER")]
    public class HelperObject
    {
        [DisplayName("NODE_ID")]
        public string? ID { get; set; }

        [DisplayName("NODE_PARENTID")]
        public string? ParentID { get; set; }

        [DisplayName("NODE_NAME")]
        public string? Name { get; set; }

        [DisplayName("NODE_PARENT")]
        public string? Parent { get; set; }

        [DisplayName("HELPER_CLASS")]
        public string? HelperClass { get; set; }

        [LinkedObject("NODE_USERPROPERTIES")]
        public NodeUserProperties NodeUserProperties { get; set; }

        [LinkedObject("NODE_TM")]
        public NodeTM NodeTM { get; set; }

        [DisplayName("BOUNDINGBOX_MIN")]
        public Vector3? BoundingBoxMin { get; set; }

        [DisplayName("BOUNDINGBOX_MAX")]
        public Vector3? BoundingBoxMax { get; set; }

        [LinkedObject("TM_ANIMATION")]
        public TMAnimation TMAnimation { get; set; }
    }
}
