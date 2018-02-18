using System.Collections.Generic;
using Accord;

namespace CS310_Audio_Analysis_Project
{
    internal class FrequencyPoint
    {
        internal DoublePoint3D doublePoint;
        internal Sphere[] spheres;
        internal Circle[] circles;
        internal int frequency;
        internal List<DoublePoint3D> points;

        public FrequencyPoint(DoublePoint3D doublePoint, List<DoublePoint3D> points, Sphere[] spheres, Circle[] circles, int frequency)
        {
            this.doublePoint = doublePoint;
            this.points = points;
            this.spheres = spheres;
            this.frequency = frequency;
            this.circles = circles;
        }
    }
}