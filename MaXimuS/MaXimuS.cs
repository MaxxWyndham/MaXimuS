using System;

namespace MaXimuS
{
    public static class MaXimuS
    {
        public static Version Version { get; set; } = new(1, 0, 5);

        public static void Warn(string message)
        {
            Console.WriteLine($"OH NO! {message}");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
