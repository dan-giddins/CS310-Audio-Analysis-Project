using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.Drawing;

namespace CS310_Audio_Analysis_Project
{
    public partial class MainForm : Form
    {
        private WaveIn waveIn;
        private BufferedWaveProvider bufferedWaveProvider;
        private int currentDevice;
        private int[] values = new int[0];
        private const int SAMPLE_RATE = 44100;
        private const int BUFFER_SIZE = 1024;

        public MainForm()
        {
            InitializeComponent();
            updateAudioDevices();
            test();
        }

        private void test()
        {
            tmrLabel.Enabled = false;
            //create a WaveIn object
            waveIn = new WaveIn();
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

        //add data to the audio recording buffer
        private void waveInDataAvailable(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
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

        private void draw()
        {
            tmrLabel.Enabled = false;
            //read the bytes from the stream
            const int BIT_DEPTH = 16;
            const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
            values = new int[panWaveform.Width];
            int frameSize = values.Length * BYTES_PER_SAMPLE;
            byte[] frameArray = new byte[frameSize];
            bufferedWaveProvider.Read(frameArray, 0, frameArray.Length);
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = (frameArray[i * 2 + 1] << 8) | frameArray[i * 2];
            }
            panWaveform.Invalidate();
            tmrLabel.Enabled = true;
        }

        private void panWaveform_Paint(object sender, PaintEventArgs e)
        {
            var bitmap = new Bitmap(panWaveform.Width, panWaveform.Height);
            var graphics = Graphics.FromImage(bitmap);
            int overFlow = (int)Math.Pow(2, 15);
            int yPosOld = 0;
            int yPosNew = 0;
            double yScale = panWaveform.Height / Math.Pow(2, 16);
            for (int i = 0; i < values.Length; i++)
            {
                yPosOld = yPosNew;
                if (values[i] < overFlow)
                {
                    yPosNew = (int)((overFlow - values[i]) * yScale);
                }
                else
                {
                    yPosNew = (int)((overFlow * 3 - values[i]) * yScale);
                }
                if (i > 1)
                {
                    graphics.DrawLine(Pens.Black, i - 1, yPosOld, i, yPosNew);
                }
            }
            e.Graphics.DrawImage(bitmap, 0, 0, ClientRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
        }

        private void tmrLabel_Tick(object sender, EventArgs e)
        {
            draw();
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            test();
        }
    }
}
