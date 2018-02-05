using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS310_Audio_Analysis_Project
{
    class Device
    {
        public MMDevice device;
        public int channelNo;
        public int channelCount;

        public Device(MMDevice device, int channelNo)
        {
            this.device = device;
            this.channelNo = channelNo;
            channelCount = device.AudioEndpointVolume.Channels.Count;
        }
    }
}
