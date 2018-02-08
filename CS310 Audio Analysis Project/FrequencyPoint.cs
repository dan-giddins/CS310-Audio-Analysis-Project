using Accord;

namespace CS310_Audio_Analysis_Project
{
    internal class FrequencyPoint
    {
        private DoublePoint doublePoint;
        private Circle[] circles;
        private int frequency;

        public FrequencyPoint(DoublePoint doublePoint, Circle[] circles, int frequency)
        {
            this.doublePoint = doublePoint;
            this.circles = circles;
            this.frequency = frequency;
        }

        public DoublePoint DoublePoint
        {
            get
            {
                return doublePoint;
            }
        }

        public Circle[] Circles
        {
            get
            {
                return circles;
            }
        }

        public int Frequency
        {
            get
            {
                return frequency;
            }
        }
    }
}