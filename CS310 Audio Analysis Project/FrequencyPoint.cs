using System.Collections.Generic;
using Accord;

namespace CS310_Audio_Analysis_Project
{
    internal class FrequencyPoint
    {
        internal DoublePoint doublePoint;
        internal Circle[] circles;
        internal int frequency;
        internal List<DoublePoint> points;

        public FrequencyPoint(DoublePoint doublePoint, Circle[] circles, int frequency)
        {
            this.doublePoint = doublePoint;
            this.circles = circles;
            this.frequency = frequency;
        }

        public FrequencyPoint(DoublePoint doublePoint, List<DoublePoint> points, Circle[] circles, int frequency)
        {
            this.doublePoint = doublePoint;
            this.points = points;
            this.circles = circles;
            this.frequency = frequency;
        }
    }
}