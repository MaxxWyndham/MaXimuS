using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("NODE")]
    public class Node
    {
        [DisplayName("NODE_ID")]
        public string ID { get; set; }

        [DisplayName("NODE_NAME")]
        public string Name { get; set; }

        [DisplayName("NODE_PARENTID")]
        public string ParentID { get; set; }

        [DisplayName("NODE_PARENT")]
        public string Parent { get; set; }

        [LinkedObject("NODE_USERPROPERTIES")]
        public NodeUserProperties NodeUserProperties { get; set; }

        [LinkedObject("NODE_TM")]
        public NodeTM NodeTM { get; set; }

        [LinkedObject("TM_ANIMATION")]
        public TMAnimation TMAnimation { get; set; }
    }
}
