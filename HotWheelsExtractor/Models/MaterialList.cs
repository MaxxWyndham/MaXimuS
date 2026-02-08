using System.Collections.Generic;
using System.ComponentModel;

namespace HotWheels.Models
{
    [DisplayName("MATERIAL_LIST")]
    public class MaterialList
    {
        [DisplayName("MATERIAL_COUNT")]
        public int MaterialCount { get; set; }

        [LinkedObject("MATERIAL")]
        public List<Material> Materials { get; set; } = [];
    }
}
