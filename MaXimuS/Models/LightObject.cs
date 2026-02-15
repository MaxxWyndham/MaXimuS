using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("LIGHTOBJECT")]
    public class LightObject : Node
    {
        [DisplayName("LIGHT_TYPE")]
        public string LightType { get; set; }

        [DisplayName("LIGHT_SHADOWS")]
        public string Shadows { get; set; }

        [DisplayName("LIGHT_USELIGHT")]
        public int UseLight { get; set; }

        [DisplayName("LIGHT_SPOTSHAPE")]
        public string SpotShape { get; set; }

        [DisplayName("LIGHT_USEGLOBAL")]
        public int UseGlobal { get; set; }

        [DisplayName("LIGHT_ABSMAPBIAS")]
        public int AbsMapBias { get; set; }

        [DisplayName("LIGHT_OVERSHOOT")]
        public int OverShoot { get; set; }

        [LinkedObject("LIGHT_EXCLUDELIST")]
        public LightExcludeList LightExcludeList { get; set; }

        [LinkedObject("LIGHT_SETTINGS")]
        public LightSettings LightSettings { get; set; }
    }
}
