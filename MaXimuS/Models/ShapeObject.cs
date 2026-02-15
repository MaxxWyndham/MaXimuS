using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("SHAPEOBJECT")]
    public class ShapeObject : Node
    {
        [DisplayName("SHAPE_LINECOUNT")]
        public int LineCount { get; set; }

        [LinkedObject("SHAPE_LINE")]
        public ShapeLine ShapeLine { get; set; }
    }
}
