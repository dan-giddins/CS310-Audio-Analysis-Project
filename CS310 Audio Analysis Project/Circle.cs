using Accord;
using System;

namespace CS310_Audio_Analysis_Project
{
    internal class Circle
    {
        internal DoublePoint center;
        internal double radius;
        internal double gradient;
        internal double intercept;
        internal DoublePoint[] points;

        public Circle(DoublePoint center, double radius, double gradient, double intercept, DoublePoint[] points)
        {
            this.center = center;
            this.radius = Math.Abs(radius);
            this.gradient = gradient;
            this.intercept = intercept;
            this.points = points;
        }

        public DoublePoint3D intersect(Circle c)
        {
            double x = (c.intercept - intercept) / (gradient - c.gradient);
            double y = gradient * x + intercept;
            double lenghtA = Math.Sqrt(Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2));
            double lenghtB = Math.Sqrt(Math.Pow(x - c.center.X, 2) + Math.Pow(y - c.center.Y, 2));
            if (lenghtA > radius || lenghtB > c.radius) {
                return new DoublePoint3D(double.NaN, double.NaN, double.NaN);
            }
            else
            {
                double zA = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(lenghtA, 2));
                double zB = Math.Sqrt(Math.Pow(c.radius, 2) - Math.Pow(lenghtB, 2));
                return new DoublePoint3D(x, y, (zA + zB) / 2);
            }
        }
    }
}
