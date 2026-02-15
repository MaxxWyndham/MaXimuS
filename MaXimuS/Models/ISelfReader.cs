using System.IO;

namespace MaXimuS.Models
{
    public interface ISelfReader
    {
        public void Read(BinaryReader br, int count);
    }
}
