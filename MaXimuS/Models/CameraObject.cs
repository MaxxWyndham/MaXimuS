using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("CAMERAOBJECT")]
    public class CameraObject : Node
    {
        [DisplayName("CAMERA_TYPE")]
        public string CameraType { get; set; }

        [LinkedObject("CAMERA_SETTINGS")]
        public CameraSettings CameraSettings { get; set; }
    }
}
