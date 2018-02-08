using Accord;

namespace CS310_Audio_Analysis_Project
{
    internal class FrequencyPoint
    {
        private DoublePoint doublePoint;
        private int frequency;

        public FrequencyPoint(DoublePoint doublePoint, int frequency)
        {
            this.doublePoint = doublePoint;
            this.frequency = frequency;
        }

        public DoublePoint DoublePoint
        {
            get
            {
                return doublePoint;
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