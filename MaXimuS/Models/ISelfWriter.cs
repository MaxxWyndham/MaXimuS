using System.IO;

namespace MaXimuS.Models
{
    public interface ISelfWriter
    {
        public void Write(BinaryWriter bw);
    }
}
