using System;
using System.IO;

namespace MaXimuS
{
    public class W3D
    {
        public static W3D Load(string path)
        {
            FileInfo fi = new(path);
            W3D w3d = new();

            BinaryReader br = new(fi.OpenRead());

            int length = 0;

            _ = br.ReadBytes(4); // Magic number 0x49465800

            int u1 = br.ReadInt32(); // always 0x8?
            int u2 = br.ReadInt32(); // always 0x11?
            int u3 = br.ReadInt32(); // length of file

            _ = br.ReadBytes(4); // 0x01ffffff

            int u4 = br.ReadInt32(); // always 0x4?
            int u5 = br.ReadInt32(); // always 0x0?

            _ = br.ReadBytes(4); // 0x02ffffff

            int u6 = br.ReadInt32(); // always 0x3?
            int u7 = br.ReadInt32(); // always 0x000904?

            _ = br.ReadBytes(4); // 0x72ffffff

            int u8 = br.ReadInt32(); // block len?

            length = br.ReadInt32();
            Console.WriteLine(br.ReadString(length));

            length = br.ReadInt32();
            Console.WriteLine(br.ReadString(length));



            br.Close();

            return w3d;
        }
    }
}
