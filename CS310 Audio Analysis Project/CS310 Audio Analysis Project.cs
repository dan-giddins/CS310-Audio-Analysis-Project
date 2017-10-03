using System;
using System.Windows.Forms;
using NAudio.Wave;

namespace CS310_Audio_Analysis_Project
{
    public partial class MainForm : Form
    {
        public WaveIn waveIn;
        public BufferedWaveProvider bufferedWaveProvider;
        private int currentDevice;
        private const int SAMPLE_RATE = 44100;
        private const int BUFFER_SIZE = 16;

        public MainForm()
        {
            InitializeComponent();
            updateAudioDevices();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            tmrLabel.Enabled = false;
            //create a WaveIn object
            WaveIn waveIn = new WaveIn();
            waveIn.DeviceNumber = currentDevice;
            waveIn.WaveFormat = new WaveFormat(SAMPLE_RATE, 1);
            //create a wave buffer and start the recording
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable);
            bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);
            bufferedWaveProvider.BufferLength = BUFFER_SIZE * 2;
            bufferedWaveProvider.DiscardOnBufferOverflow = true;
            waveIn.StartRecording();
            tmrLabel.Enabled = true;
        }

        private void updateAudioDevices()
        {
            //get all avaible audio devices
            int deviceCount = WaveIn.DeviceCount;
            Console.Out.WriteLine("Device Count: " + deviceCount);
            boxDevices.Items.Clear();
            for (int i = 0; i < deviceCount; i++)
            {
                WaveInCapabilities capabilities = WaveIn.GetCapabilities(i);
                boxDevices.Items.Add(i + ": " + capabilities.ProductName + ", Channels: " + capabilities.Channels);
            }
        }

        //add data to the audio recording buffer
        private void waveInDataAvailable(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void update_label()
        {
            tmrLabel.Enabled = false;
            //read the bytes from the stream
            const int BIT_DEPTH = 16;
            const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
            int[] values = new int[BUFFER_SIZE / BYTES_PER_SAMPLE];
            int frameSize = values.Length * BYTES_PER_SAMPLE;
            byte[] frameArray = new byte[frameSize];
            bufferedWaveProvider.Read(frameArray, 0, frameArray.Length);
            for (int i=0; i<values.Length; i++)
            {
                values[i] = (frameArray[i * 2 + 1] << 8) | frameArray[i * 2];
            }
            tmrLabel.Enabled = true;
        }

        private void tmrLabel_Tick(object sender, EventArgs e)
        {
            update_label();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            updateAudioDevices();
        }

        private void boxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentDevice = boxDevices.SelectedIndex;
            Console.Out.WriteLine("Selected device: " + currentDevice);
        }
    }
}
