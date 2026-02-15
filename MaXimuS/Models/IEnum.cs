using System.Collections.Generic;
using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("INTERNAL_ENUM")]
    public abstract class IEnum
    {
        public List<string> Values { get; set; } = [];
    }
}
