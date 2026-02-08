using System.Collections.Generic;
using System.ComponentModel;

namespace HotWheels.Models
{
    [DisplayName("INTERNAL_BINARY")]
    public abstract class IBinary
    {
        public List<string> Values { get; set; } = [];
    }
}
