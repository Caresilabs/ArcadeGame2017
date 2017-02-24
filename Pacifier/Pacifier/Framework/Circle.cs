using Microsoft.Xna.Framework;

namespace CloudColony.Framework
{
    public class Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool Contains(Vector2 point)
        {
            return ((point - Center).Length() <= Radius);
        }

        public bool Intersects(Circle other)
        {
            float dx = Center.X - other.Center.X;
            float dy = Center.Y - other.Center.Y;
            float distance = dx * dx + dy * dy;
            float radiusSum = Radius + other.Radius;

            return distance < radiusSum * radiusSum;
        }
    }
}
