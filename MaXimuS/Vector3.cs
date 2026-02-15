namespace MaXimuS
{
    public class Vector3
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public static Vector3 One => new(1);

        public static Vector3 Zero => new(0);

        public Vector3(float n)
        {
            X = n;
            Y = n;
            Z = n;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(string x, string y, string z)
        {
            X = float.Parse(x);
            Y = float.Parse(y);
            Z = float.Parse(z);
        }

        public static Vector3 operator -(Vector3 x)
        {
            return new Vector3(-x.X, -x.Y, -x.Z);
        }

        public static Vector3 operator -(Vector3 x, Vector3 y)
        {
            return new Vector3(x.X - y.X, x.Y - y.Y, x.Z - y.Z);
        }

        public override string ToString()
        {
            return $"{X:0.00000000}\t{Y:0.00000000}\t{Z:0.00000000}";
        }
    }
}
