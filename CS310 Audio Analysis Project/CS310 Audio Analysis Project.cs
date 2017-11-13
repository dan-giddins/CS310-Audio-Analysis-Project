using System;
using System.Windows.Forms;
using System.Drawing;
using NAudio.Wave;

namespace CS310_Audio_Analysis_Project
{
    public partial class MainForm : Form
    {
        private const int SAMPLE_RATE = 44100;
        private const int BUFFER_SIZE = 1024;
        private const byte BIT_DEPTH = 16;
        private const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
        private const byte INPUTS = 4;
        private WaveIn[] waveIn;
        private BufferedWaveProvider[] bufferedWaveProvider;
        private int[] currentDevice;
        private int[][] values;
        private byte[][] frameArray;
        private PictureBox[] picWaveform;
        private ComboBox[] boxDevices;
        private bool[] recording;
        private bool allowRecording = false;

        public MainForm()
        {
            InitializeComponent();
            generateArrays();
            updateAudioDevices();
            displayDevices();
            allowRecording = true;
            for (byte i = 0; i < INPUTS; i++)
            {
                updateDeviceSelection(i);
                if (recording[i])
                {
                    try
                    {
                        waveIn[i].StartRecording();
                    } catch (System.InvalidOperationException e) {
                        Console.Out.WriteLine(i + " - " + e.Message);
                    }
                }
            }
            
        }

        private void displayDevices()
        {
            for (byte i = 0; i < INPUTS; i++)
            {
                try
                {
                    boxDevices[i].SelectedIndex = i;
                    boxDevices[i].SelectedItem = boxDevices[i].Items[boxDevices[i].SelectedIndex];
                }
                catch (ArgumentOutOfRangeException)
                {
                    boxDevices[i].SelectedIndex = -1;
                }
                updateDeviceSelection(i);
            }
        }

        private void generateArrays()
        {
            waveIn = new WaveIn[INPUTS];
            values = new int[INPUTS][];
            picWaveform = new PictureBox[INPUTS];
            picWaveform[0] = picWaveform0;
            picWaveform[1] = picWaveform1;
            picWaveform[2] = picWaveform2;
            picWaveform[3] = picWaveform3;
            frameArray = new byte[INPUTS][];
            for (int i = 0; i < INPUTS; i++)
            {
                waveIn[i] = new WaveIn();
                values[i] = new int[picWaveform[i].Width];
                frameArray[i] = new byte[picWaveform[i].Width * BYTES_PER_SAMPLE];
            }
            boxDevices = new ComboBox[INPUTS];
            boxDevices[0] = boxDevices0;
            boxDevices[1] = boxDevices1;
            boxDevices[2] = boxDevices2;
            boxDevices[3] = boxDevices3;
            bufferedWaveProvider = new BufferedWaveProvider[INPUTS];
            currentDevice = new int[INPUTS];
            recording = new bool[4];
        }

        private void test()
        {
            for (byte i = 0; i < INPUTS; i++)
            {
                configWaveIn(i);
            }
            waveIn[0].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable0);
            waveIn[1].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable1);
            waveIn[2].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable2);
            waveIn[3].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable3);
            for (int i = 0; i < INPUTS; i++)
            {
                configWaveBuffer(i);
            }
            tmrLabel.Enabled = true;
        }

        private void configWaveBuffer(int i)
        {
            //create a wave buffer and start the recording
            bufferedWaveProvider[i] = new BufferedWaveProvider(waveIn[i].WaveFormat);
            bufferedWaveProvider[i].BufferLength = values[i].Length * BYTES_PER_SAMPLE * 2;
            bufferedWaveProvider[i].DiscardOnBufferOverflow = true;
            bufferedWaveProvider[i].ReadFully = false;
            if (!recording[i] && currentDevice[i] > -1)
            {
                try
                {
                    waveIn[i].StartRecording();
                }
                catch (System.InvalidOperationException e)
                {
                    Console.Out.WriteLine(i + " - " + e.Message);
                }
                recording[i] = true;
            }
        }

        private void configWaveIn(int i)
        {
            if (currentDevice[i] > -1)
            {
                waveIn[i].DeviceNumber = currentDevice[i];
            }
            waveIn[i].WaveFormat = new WaveFormat(SAMPLE_RATE, 1);
        }

        private void stopTest()
        {
            tmrLabel.Enabled = false;
            for (int i = 0; i < INPUTS; i++)
            {
                if (recording[i])
                {
                    waveIn[i].StopRecording();
                    recording[i] = false;
                }
            }
        }

        //add data to the audio recording buffer
        private void waveInDataAvailable0(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider[0].AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void waveInDataAvailable1(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider[1].AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void waveInDataAvailable2(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider[2].AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void waveInDataAvailable3(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider[3].AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void updateAudioDevices()
        {
            //get all avaible audio devices
            int deviceCount = WaveIn.DeviceCount;
            boxDevices0.Items.Clear();
            boxDevices1.Items.Clear();
            boxDevices2.Items.Clear();
            boxDevices3.Items.Clear();
            for (int i = 0; i < deviceCount; i++)
            {
                WaveInCapabilities capabilities = WaveIn.GetCapabilities(i);
                boxDevices0.Items.Add(i + ": " + capabilities.ProductName + ", Channels: " + capabilities.Channels);
                boxDevices1.Items.Add(i + ": " + capabilities.ProductName + ", Channels: " + capabilities.Channels);
                boxDevices2.Items.Add(i + ": " + capabilities.ProductName + ", Channels: " + capabilities.Channels);
                boxDevices3.Items.Add(i + ": " + capabilities.ProductName + ", Channels: " + capabilities.Channels);
            }
        }

        private void draw()
        {
            tmrLabel.Enabled = false;
            //read the bytes from the streams
            for (byte i = 0; i < INPUTS; i++)
            {
                if (currentDevice[i] > -1)
                {
                    bufferedWaveProvider[i].Read(frameArray[i], 0, frameArray[i].Length);
                    for (int j = 0; j < values[i].Length; j++)
                    {
                        values[i][j] = (frameArray[i][j * 2 + 1] << 8) | frameArray[i][j * 2];
                    }
                    picWaveform[i].Invalidate();
                }
            }
            tmrLabel.Enabled = true;
        }

        private void paint(PaintEventArgs e, int i)
        {
            if (currentDevice[i] > -1)
            {
                if (picWaveform[i].Image != null)
                {
                    picWaveform[i].Image.Dispose();
                }
                Bitmap bitmap = new Bitmap(picWaveform[i].Width, picWaveform[i].Height);
                Graphics graphics = Graphics.FromImage(bitmap);
                int overFlow = (int)Math.Pow(2, 15);
                int yPosOld = 0;
                int yPosNew = 0;
                double yScale = (picWaveform[i].Height - 1) / Math.Pow(2, 16);
                for (int j = 0; j < values[i].Length; j++)
                {
                    yPosOld = yPosNew;
                    if (values[i][j] < overFlow)
                    {
                        yPosNew = (int)((overFlow - values[i][j]) * yScale);
                    }
                    else
                    {
                        yPosNew = (int)((overFlow * 3 - values[i][j]) * yScale);
                    }
                    if (j > 1)
                    {
                        graphics.DrawLine(Pens.Black, j - 1, yPosOld, j, yPosNew);
                    }
                }
                e.Graphics.DrawImage(bitmap, 0, 0, ClientRectangle, GraphicsUnit.Pixel);
                graphics.Dispose();
            }
        }

        private void tmrLabel_Tick(object sender, EventArgs e)
        {
            draw();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (allowRecording)
            {
                updateAudioDevices();
            }  
        }

        private void updateDeviceSelection(byte i)
        {
            if (allowRecording)
            {
                currentDevice[i] = boxDevices[i].SelectedIndex;
                Console.Out.WriteLine(i + " - selected device: " + currentDevice[i]);
                stopTest();
                test();
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (allowRecording)
            {
                stopTest();
                test();
            }
        }

        private void boxDevices0_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDeviceSelection(0);       
        }

        private void boxDevices1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDeviceSelection(1);
        }

        private void boxDevices2_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDeviceSelection(2);
        }

        private void boxDevices3_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateDeviceSelection(3);
        }

        private void picWaveform0_Paint(object sender, PaintEventArgs e)
        {
            paint(e, 0);
        }

        private void picWaveform1_Paint(object sender, PaintEventArgs e)
        {
            paint(e, 1);
        }

        private void picWaveform2_Paint(object sender, PaintEventArgs e)
        {
            paint(e, 2);
        }

        private void picWaveform3_Paint(object sender, PaintEventArgs e)
        {
            paint(e, 3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
