using Accord;
using System;

namespace CS310_Audio_Analysis_Project
{
    internal class Sphere
    {
        internal DoublePoint center;
        internal double radius;
        // orientation

        public Sphere(DoublePoint center, double radius)
        {
            this.center = center;
            this.radius = Math.Abs(radius);
        }

        public Circle intersect(Sphere s)
        {
            double distanceSquared = Math.Pow(center.DistanceTo(s.center), 2);
            double area = Math.Sqrt((Math.Pow(radius + s.radius, 2) - distanceSquared) * (distanceSquared - Math.Pow(radius - s.radius, 2))) / 4;
            double xa = (s.center.X + center.X)/2 + (s.center.X - center.X)*(Math.Pow(radius, 2) - Math.Pow(s.radius, 2)) / (2 * distanceSquared);
            double xb = 2 * (s.center.Y - center.Y) * area / distanceSquared;
            double ya = (s.center.Y + center.Y) / 2 + (s.center.Y - center.Y) * (Math.Pow(radius, 2) - Math.Pow(s.radius, 2)) / (2 * distanceSquared);
            double yb = - 2 * (s.center.X - center.X) * area / distanceSquared;

            return new DoublePoint[] { new DoublePoint(xa + xb, ya + yb), new DoublePoint(xa - xb, ya - yb)};
        }
    }
}
