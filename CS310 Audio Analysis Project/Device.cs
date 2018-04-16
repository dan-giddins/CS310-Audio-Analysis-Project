using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace CS310_Audio_Analysis_Project
{
    // class to encapsulate device and number of channels
    class Device
    {
        public AsioOut device;
        public int channelCount;

        public Device(AsioOut device)
        {
            this.device = device;
            channelCount = device.DriverInputChannelCount;
        }
    }
}
