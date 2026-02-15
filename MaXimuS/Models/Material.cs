using System.Collections.Generic;
using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("MATERIAL")]
    public class Material
    {
        [DisplayName("MATERIAL_NAME")]
        public string? Name { get; set; }

        [DisplayName("MATERIAL_CLASS")]
        public string? Class { get; set; }

        [DisplayName("MATERIAL_AMBIENT")]
        public Vector3? Ambient { get; set; }

        [DisplayName("MATERIAL_DIFFUSE")]
        public Vector3? Diffuse { get; set; }

        [DisplayName("MATERIAL_SPECULAR")]
        public Vector3? Specular { get; set; }

        [DisplayName("MATERIAL_SHINE")]
        public float Shine { get; set; } = 0.25f;

        [DisplayName("MATERIAL_SHINESTRENGTH")]
        public float ShineStrength { get; set; } = 0.05f;

        [DisplayName("MATERIAL_TRANSPARENCY")]
        public float Transparency { get; set; }

        [DisplayName("MATERIAL_WIRESIZE")]
        public float WireSize { get; set; } = 1f;

        [DisplayName("MATERIAL_SHADING")]
        public string? Shading { get; set; }

        [DisplayName("MATERIAL_XP_FALLOFF")]
        public float? XPFalloff { get; set; }

        [DisplayName("MATERIAL_SELFILLUM")]
        public float? Selfillum { get; set; }

        [DisplayName("MATERIAL_FALLOFF")]
        public string? Falloff { get; set; }

        [DisplayName("MATERIAL_XP_TYPE")]
        public string? XPType { get; set; }

        [DisplayName("NUMSUBMTLS")]
        public int? NumSubMaterials { get; set; }

        [LinkedObject("SUBMATERIAL")]
        public List<SubMaterial> SubMaterials { get; set; } = [];

        [LinkedObject("MAP_DIFFUSE")]
        public MapDiffuse? MapDiffuse { get; set; }

        [LinkedObject("MAP_OPACITY")]
        public MapOpacity? MapOpacity { get; set; }

        [LinkedObject("MAP_REFLECT")]
        public MapReflect? MapReflect { get; set; }
    }
}
