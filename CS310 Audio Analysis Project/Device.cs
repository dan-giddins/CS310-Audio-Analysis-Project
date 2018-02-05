using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS310_Audio_Analysis_Project
{
    class Device
    {
        public int deviceNo;
        public int channelNo;
        public int channelCount;
         
        public Device(int deviceNo, int channelNo, int channelCount)
        {
            this.deviceNo = deviceNo;
            this.channelNo = channelNo;
            this.channelCount = channelCount;
        }
    }
}
