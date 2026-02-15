using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("NODE_TM")]
    public class NodeTM
    {
        [DisplayName("NODE_NAME")]
        public string Name { get; set; }

        [DisplayName("INHERIT_POS")]
        public Vector3 InheritPos { get; set; } = Vector3.Zero;

        [DisplayName("INHERIT_ROT")]
        public Vector3 InheritRot { get; set; } = Vector3.Zero;

        [DisplayName("INHERIT_SCL")]
        public Vector3 InheritScl { get; set; } = Vector3.Zero;

        [DisplayName("TM_ROW0")]
        public Vector3 Row0 { get; set; }

        [DisplayName("TM_ROW1")]
        public Vector3 Row1 { get; set; }

        [DisplayName("TM_ROW2")]
        public Vector3 Row2 { get; set; }

        [DisplayName("TM_ROW3")]
        public Vector3 Row3 { get; set; }

        [DisplayName("TM_POS")]
        public Vector3 Position { get; set; }

        [DisplayName("TM_ROTAXIS")]
        public Vector3 RotAxis { get; set; } = Vector3.Zero;

        [DisplayName("TM_ROTANGLE")]
        public float RotAngle { get; set; }

        [DisplayName("TM_SCALE")]
        public Vector3 Scale { get; set; } = Vector3.One;

        [DisplayName("TM_SCALEAXIS")]
        public Vector3 ScaleAxis { get; set; } = Vector3.Zero;

        [DisplayName("TM_SCALEAXISANG")]
        public float ScaleAxisAng { get; set; }
    }
}
