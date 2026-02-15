using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("MAP")]
    public class Map
    {
        [DisplayName("MAP_NAME")]
        public string Name { get; set; }

        [DisplayName("MAP_CLASS")]
        public string Class { get; set; }

        [DisplayName("MAP_SUBNO")]
        public int SubNo { get; set; }

        [DisplayName("MAP_AMOUNT")]
        public float Amount { get; set; }

        [DisplayName("BITMAP")]
        public string Bitmap { get; set; }

        [DisplayName("MAP_TYPE")]
        public string Type { get; set; }

        [DisplayName("UVW_U_OFFSET")]
        public float UOffset { get; set; }

        [DisplayName("UVW_V_OFFSET")]
        public float VOffset { get; set; }

        [DisplayName("UVW_U_TILING")]
        public float UTiling { get; set; }

        [DisplayName("UVW_V_TILING")]
        public float VTiling { get; set; }

        [DisplayName("UVW_UV_WRAPFLAGS")]
        public int? UVWrapFlags { get; set; }

        [DisplayName("UVW_ANGLE")]
        public float Angle { get; set; }

        [DisplayName("UVW_ROW0")]
        public Vector3 Row0 { get; set; }

        [DisplayName("UVW_ROW1")]
        public Vector3 Row1 { get; set; }

        [DisplayName("UVW_ROW2")]
        public Vector3 Row2 { get; set; }

        [DisplayName("UVW_ROW3")]
        public Vector3 Row3 { get; set; }

        [DisplayName("UVW_BLUR")]
        public float Blur { get; set; }

        [DisplayName("UVW_BLUR_OFFSET")]
        public float BlurOffset { get; set; }

        [DisplayName("UVW_NOUSE_AMT")]
        public float NoiseAmount { get; set; }

        [DisplayName("UVW_NOISE_SIZE")]
        public float NoiseSize { get; set; }

        [DisplayName("UVW_NOISE_LEVEL")]
        public int NoiseLevel { get; set; }

        [DisplayName("UVW_NOISE_PHASE")]
        public float NoisePhase { get; set; }

        [DisplayName("BITMAP_FILTER")]
        public string Filter { get; set; }
    }
}
