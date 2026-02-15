namespace MaXimuS
{
    public class Vector2
    {
        public float X { get; set; }

        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            X = x; 
            Y = y; 
        }

        public static Vector2 operator -(Vector2 x, Vector2 y)
        {
            return new Vector2(x.X - y.X, x.Y - y.Y);
        }
    }
}
