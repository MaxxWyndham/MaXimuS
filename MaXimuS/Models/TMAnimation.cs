using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("TM_ANIMATION")]
    public class TMAnimation
    {
        [DisplayName("NODE_NAME")]
        public string Name { get; set; }

        [LinkedObject("CONTROL_POS_TRACK")]
        public ControlPosTrack ControlPosTrack { get; set; }

        [LinkedObject("CONTROL_ROT_TRACK")]
        public ControlRotTrack ControlRotTrack { get; set; }

        [LinkedObject("CONTROL_SCALE_TRACK")]
        public ControlScaleTrack ControlScaleTrack { get; set; }
    }
}
