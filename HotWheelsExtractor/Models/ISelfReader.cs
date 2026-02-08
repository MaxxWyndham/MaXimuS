using System.IO;

namespace HotWheels.Models
{
    public interface ISelfReader
    {
        public void Read(BinaryReader br, int count);
    }
}
