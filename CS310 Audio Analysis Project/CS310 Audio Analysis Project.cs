using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.Drawing;

namespace CS310_Audio_Analysis_Project
{
    public partial class MainForm : Form
    {
        private const int SAMPLE_RATE = 44100;
        private const int BUFFER_SIZE = 1024;
        private const int BIT_DEPTH = 16;
        private const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
        private WaveIn waveIn;
        private BufferedWaveProvider bufferedWaveProvider;
        private int currentDevice;
        private int[] values;
        private int frameSize;
        private byte[] frameArray;

        public MainForm()
        {
            InitializeComponent();
            waveIn = new WaveIn();
            updateAudioDevices();
            values = new int[picWaveform.Width];
            frameSize = values.Length * BYTES_PER_SAMPLE;
            frameArray = new byte[frameSize];
            test();
        }

        private void test()
        {
            //create a WaveIn object
            waveIn = new WaveIn();
            waveIn.DeviceNumber = currentDevice;
            waveIn.WaveFormat = new WaveFormat(SAMPLE_RATE, 1);
            //create a wave buffer and start the recording
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable);
            bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);
            bufferedWaveProvider.BufferLength = values.Length * BYTES_PER_SAMPLE * 2;
            bufferedWaveProvider.DiscardOnBufferOverflow = true;
            bufferedWaveProvider.ReadFully = false;
            waveIn.StartRecording();
            tmrLabel.Enabled = true;
        }

        private void stopTest()
        {
            tmrLabel.Enabled = false;
            waveIn.StopRecording();
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
            bufferedWaveProvider.Read(frameArray, 0, frameArray.Length);
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = (frameArray[i * 2 + 1] << 8) | frameArray[i * 2];
            }
            picWaveform.Invalidate();
            tmrLabel.Enabled = true;
        }

        private void picWaveform_Paint(object sender, PaintEventArgs e)
        {
            if (picWaveform.Image != null)
            {
                picWaveform.Image.Dispose();
            }
            Bitmap bitmap = new Bitmap(picWaveform.Width, picWaveform.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            int overFlow = (int)Math.Pow(2, 15);
            int yPosOld = 0;
            int yPosNew = 0;
            double yScale = (picWaveform.Height - 1) / Math.Pow(2, 16);
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
            stopTest();
            test();
        }
    }
}
