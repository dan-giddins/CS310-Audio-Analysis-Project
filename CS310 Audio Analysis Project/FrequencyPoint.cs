using System.Collections.Generic;
using Accord;

namespace CS310_Audio_Analysis_Project
{
    internal class FrequencyPoint
    {
        internal DoublePoint doublePoint;
        internal Circle[] circles;
        internal int frequency;
        internal List<double> bestPointsX;
        internal List<double> bestPointsY;

        public FrequencyPoint(DoublePoint doublePoint, Circle[] circles, int frequency)
        {
            this.doublePoint = doublePoint;
            this.circles = circles;
            this.frequency = frequency;
        }

        public FrequencyPoint(DoublePoint doublePoint, List<double> bestPointsX, List<double> bestPointsY, Circle[] circles, int frequency)
        {
            this.doublePoint = doublePoint;
            this.bestPointsX = bestPointsX;
            this.bestPointsY = bestPointsY;
            this.circles = circles;
            this.frequency = frequency;
        }
    }
}