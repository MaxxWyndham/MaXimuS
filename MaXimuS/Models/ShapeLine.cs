using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("SHAPE_LINE")]
    public class ShapeLine
    {
        [DisplayName("SHAPE_VERTEXCOUNT")]
        public int VertexCount { get; set; }

        [DisplayName("SHAPE_VERTEX_KNOT")]
        public Vector4 VertexKnot { get; set; }

        [DisplayName("SHAPE_VERTEX_INTERP")]
        public Vector4 VertexInterp { get; set; }
    }
}
