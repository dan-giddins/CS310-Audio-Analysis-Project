using NAudio.CoreAudioApi;

namespace CS310_Audio_Analysis_Project
{
    class Device
    {
        public MMDevice device;
        public int channelCount;

        public Device(MMDevice device)
        {
            this.device = device;
            channelCount = device.AudioEndpointVolume.Channels.Count;
        }
    }
}
