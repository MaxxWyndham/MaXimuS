using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("LIGHT_SETTINGS")]
    public class LightSettings
    {
        [DisplayName("TIMEVALUE")]
        public int TimeValue { get; set; }

        [DisplayName("LIGHT_COLOR")]
        public Vector3 Colour { get; set; }

        [DisplayName("LIGHT_INTENS")]
        public float Intensity { get; set; }

        [DisplayName("LIGHT_ASPECT")]
        public float Aspect { get; set; }

        [DisplayName("LIGHT_HOTSPOT")]
        public float HotSpot { get; set; }

        [DisplayName("LIGHT_ATTNEND")]
        public float Attnend { get; set; }

        [DisplayName("LIGHT_FALLOFF")]
        public float Falloff { get; set; }

        [DisplayName("LIGHT_TDIST")]
        public float TDist { get; set; }

        [DisplayName("LIGHT_MAPBIAS")]
        public float MapBias { get; set; }

        [DisplayName("LIGHT_MAPRANGE")]
        public float MapRange { get; set; }

        [DisplayName("LIGHT_MAPSIZE")]
        public int MapSize { get; set; }

        [DisplayName("LIGHT_RAYBIAS")]
        public float RayBias { get; set; }
    }
}
