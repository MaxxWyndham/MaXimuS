using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("CAMERA_SETTINGS")]
    public class CameraSettings
    {
        [DisplayName("TIMEVALUE")]
        public int TimeValue { get; set; }

        [DisplayName("CAMERA_NEAR")]
        public float Near { get; set; }

        [DisplayName("CAMERA_FAR")]
        public float Far { get; set; }

        [DisplayName("CAMERA_FOV")]
        public float FOV { get; set; }

        [DisplayName("CAMERA_TDIST")]
        public float TDist { get; set; }
    }
}
