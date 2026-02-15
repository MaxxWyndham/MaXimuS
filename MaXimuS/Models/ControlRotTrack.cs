using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("CONTROL_ROT_TRACK")]
    public class ControlRotTrack
    {
        [DisplayName("CONTROL_ROT_SAMPLE")]
        public Vector4 ControlRotSample { get; set; }
    }
}
