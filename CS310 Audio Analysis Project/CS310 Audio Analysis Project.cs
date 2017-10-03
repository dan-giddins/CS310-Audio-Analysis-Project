using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace CS310_Audio_Analysis_Project
{
    public partial class MainForm : Form
    {
        public WaveIn waveIn;
        public BufferedWaveProvider bufferedWaveProvider;
        private const int SAMPLE_RATE = 44100;
        private const int BUFFER_SIZE = 4096;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            //get all avaible audio devices
            int deviceCount = WaveIn.DeviceCount;
            Console.Out.WriteLine("Device Count: {0}.", deviceCount);
            //create a WaveIn object
            WaveIn waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(SAMPLE_RATE, 1);
            //create a wave buffer and start the recording
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);
            bufferedWaveProvider.BufferLength = BUFFER_SIZE * 2;
            bufferedWaveProvider.DiscardOnBufferOverflow = true;
            waveIn.StartRecording();
        }

        //add data to the audio recording buffer
        private void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void update_label()
        {
            //read the bytes from the stream
            const int BIT_DEPTH = 16;
            const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
            int[] values = new int[8];
            int frameSize = values.Length * BYTES_PER_SAMPLE;
            byte[] frameArray = new byte[frameSize];
            tmrLabel.Enabled = false;
            bufferedWaveProvider.Read(frameArray, 0, frameArray.Length);

            tmrLabel.Enabled = true;
        }

        private void tmrLabel_Tick(object sender, EventArgs e)
        {
            update_label();
        }
    }
}
