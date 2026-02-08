using System.ComponentModel;
using System.IO;

namespace HotWheels.Models
{
    [DisplayName("SKIN_VERTEX_DATA")]
    public class SkinVertexData : ISelfReader, IInlineCount
    {
        [DisplayName("SKINNUM")]
        [LinkedCount("SKIN_VERTEX_DATA")]
        public int Count { get; set; }

        public void Read(BinaryReader br, int count)
        {
            for (int i = 0; i < count; i++)
            {
                br.ReadBytes(12);
            }
        }
    }
}
