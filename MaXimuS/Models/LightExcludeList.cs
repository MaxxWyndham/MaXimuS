using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("LIGHT_EXCLUDELIST")]
    public class LightExcludeList
    {
        [DisplayName("LIGHT_NUMEXCLUDED")]
        public int NumExcluded { get; set; }

        [DisplayName("LIGHT_EXCLUDED_INCLUDE")]
        public int ExcludedInclude { get; set; }

        [DisplayName("LIGHT_EXCLUDED_AFFECT_ILLUM")]
        public int AffectIllum { get; set; }

        [DisplayName("LIGHT_EXCLUDED_AFFECT_SHADOW")]
        public int AffectShadow { get; set; }

        [DisplayName("LIGHT_EXCLUDED")]
        public string Excluded { get; set; }
    }
}
