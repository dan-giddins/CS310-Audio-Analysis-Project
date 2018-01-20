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

        public Device(int deviceNo, int channel)
        {
            this.deviceNo = deviceNo;
            this.channelNo = channel;
        }
    }
}
