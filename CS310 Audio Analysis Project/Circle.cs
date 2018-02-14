using Accord;
using System;

namespace CS310_Audio_Analysis_Project
{
    internal class Circle
    {
        private DoublePoint center;
        private double radius;

        public Circle(DoublePoint center, double radius)
        {
            this.center = center;
            this.radius = Math.Abs(radius);
        }

        public DoublePoint Center
        {
            get
            {
                return center;
            }
        }

        public double Radius
        {
            get
            {
                return radius;
            }
        }

        public DoublePoint[] intersect(Circle c)
        {
            double distanceSquared = Math.Pow(center.DistanceTo(c.center), 2);
            double area = Math.Sqrt((Math.Pow(radius + c.radius, 2) - distanceSquared) * (distanceSquared - Math.Pow(radius - c.radius, 2))) / 4;
            double xa = (c.center.X + center.X)/2 + (c.center.X - center.X)*(Math.Pow(radius, 2) - Math.Pow(c.radius, 2)) / (2 * distanceSquared);
            double xb = 2 * (c.center.Y - center.Y) * area / distanceSquared;
            double ya = (c.center.Y + center.Y) / 2 + (c.center.Y - center.Y) * (Math.Pow(radius, 2) - Math.Pow(c.radius, 2)) / (2 * distanceSquared);
            double yb = - 2 * (c.center.X - center.X) * area / distanceSquared;
            return new DoublePoint[] { new DoublePoint(xa + xb, ya + yb), new DoublePoint(xa - xb, ya - yb)};
        }
    }
}
