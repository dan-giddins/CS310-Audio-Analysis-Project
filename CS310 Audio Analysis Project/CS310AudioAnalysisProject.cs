using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.Drawing;
using System.Threading;

namespace CS310_Audio_Analysis_Project
{
    static class CS310AudioAnalysisProject
    {
        private const int SAMPLE_RATE = 44100;
        private const int BUFFER_SIZE = 1024;
        private const byte BIT_DEPTH = 16;
        private const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
        private const byte INPUTS = 4;
        private static WaveInEvent[] waveIn;
        private static BufferedWaveProvider[] bufferedWaveProvider;
        private static int[] currentDevice;
        private static int[][] values;
        private static byte[][] frameArray;
        private static PictureBox[] picWaveform;
        private static ComboBox[] boxDevices;
        private static bool[] recording;
        private static bool allowRecording = false;
        private static ConfigureInputForm configureInputForm;
        private static Thread configureInputThread = new Thread(runConfigureInputForm);
        private delegate void IntDelegate(int i);
        private delegate void ByteObjectDelegate(byte b, object o);
        private delegate object ByteDelegateReturnObject(byte b);
        private delegate int ByteDelegateReturnInt(byte b);
        private static EventWaitHandle drawHandel = new EventWaitHandle(false, EventResetMode.AutoReset);

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            configureInputForm = new ConfigureInputForm(drawHandel);
            generateArrays();
            configureInputThread.Start();
            updateAudioDevices();
            displayDevices();
            for (byte i = 0; i < INPUTS; i++)
            {
                updateDeviceSelection(i);
            }
            allowRecording = true;
            test();
            while (true)
            {
                drawHandel.WaitOne();
                draw();
            }
        }

        private static void runConfigureInputForm()
        {
            Application.Run(configureInputForm);
        }

        private static void displayDevices()
        {
            for (byte i = 0; i < INPUTS; i++)
            {
                setSelectedIndex(i);
                setSelectedItem(i, getItem(i));
                updateDeviceSelection(i);
            }
        }

        private static Object getItem(byte i)
        {
            if (boxDevices[i].InvokeRequired)
            {
                ByteDelegateReturnObject d = new ByteDelegateReturnObject(getItem);
                return configureInputForm.Invoke(d, new object[] { i });
            }
            else
            {
                try
                {
                    return boxDevices[i].Items[i];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
            }
        }

        private static void setSelectedItem(byte i, object o)
        {
            if (boxDevices[i].InvokeRequired)
            {
                ByteObjectDelegate d = new ByteObjectDelegate(setSelectedItem);
                configureInputForm.Invoke(d, new object[] { i, o});
            }
            else
            {
                boxDevices[i].SelectedItem = o;
            }
        }

        private static void setSelectedIndex(int i)
        {
            if (boxDevices[i].InvokeRequired)
            {
                IntDelegate d = new IntDelegate(setSelectedIndex);
                configureInputForm.Invoke(d, new object[] { i });
            }
            else
            {
                try
                {
                    boxDevices[i].SelectedIndex = i;
                }
                catch (ArgumentOutOfRangeException)
                {
                    boxDevices[i].SelectedIndex = -1;
                }
            }
        }
        internal static void breakPoint()
        {
            Console.Out.WriteLine("Breaking...");
        }

        private static void generateArrays()
        {
            waveIn = new WaveInEvent[INPUTS];
            values = new int[INPUTS][];
            picWaveform = new PictureBox[INPUTS];
            picWaveform[0] = configureInputForm.getPicWaveform0();
            picWaveform[1] = configureInputForm.getPicWaveform1();
            picWaveform[2] = configureInputForm.getPicWaveform2();
            picWaveform[3] = configureInputForm.getPicWaveform3();
            frameArray = new byte[INPUTS][];
            for (int i = 0; i < INPUTS; i++)
            {
                waveIn[i] = new WaveInEvent();
                values[i] = new int[picWaveform[i].Width];
                frameArray[i] = new byte[picWaveform[i].Width * BYTES_PER_SAMPLE];
            }
            boxDevices = new ComboBox[INPUTS];
            boxDevices[0] = configureInputForm.getBoxDevices0();
            boxDevices[1] = configureInputForm.getBoxDevices1();
            boxDevices[2] = configureInputForm.getBoxDevices2();
            boxDevices[3] = configureInputForm.getBoxDevices3();
            bufferedWaveProvider = new BufferedWaveProvider[INPUTS];
            currentDevice = new int[INPUTS];
            recording = new bool[4];
        }

        internal static void test()
        {
            for (byte i = 0; i < INPUTS; i++)
            {
                configWaveIn(i);
            }
            waveIn[0].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable0);
            waveIn[1].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable1);
            waveIn[2].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable2);
            waveIn[3].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable3);
            for (byte i = 0; i < INPUTS; i++)
            {
                configWaveBuffer(i);
            }
            configureInputForm.enableTimer();
        }

        private static void configWaveBuffer(byte i)
        {
            //create a wave buffer and start the recording
            bufferedWaveProvider[i] = new BufferedWaveProvider(waveIn[i].WaveFormat);
            bufferedWaveProvider[i].BufferLength = values[i].Length * BYTES_PER_SAMPLE * 2;
            bufferedWaveProvider[i].DiscardOnBufferOverflow = true;
            bufferedWaveProvider[i].ReadFully = false;
            startRecording(i);
        }

        private static void startRecording(byte i)
        {
            if (!recording[i] && currentDevice[i] > -1)
            {
                try
                {
                    waveIn[i].StartRecording();
                    recording[i] = true;
                }
                catch (NAudio.MmException e)
                {
                    Console.Out.WriteLine(e);
                }
            }
        }

        private static void configWaveIn(int i)
        {
            if (currentDevice[i] > -1)
            {
                waveIn[i].DeviceNumber = currentDevice[i];
            }
            waveIn[i].WaveFormat = new WaveFormat(SAMPLE_RATE, 1);
        }

        internal static void stopTest()
        {
            configureInputForm.disableTimer();
            for (byte i = 0; i < INPUTS; i++)
            {
                stopRecording(i);
            }
        }

        private static void stopRecording(byte i)
        {
            if (recording[i])
            {
                try
                {
                    waveIn[i].StopRecording();
                }
                catch (NAudio.MmException e)
                {
                    Console.Out.WriteLine(e);
                }
                recording[i] = false;
            }
        }

        private static void waveInDataAvailable0(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider[0].AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private static void waveInDataAvailable1(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider[1].AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private static void waveInDataAvailable2(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider[2].AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private static void waveInDataAvailable3(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider[3].AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        internal static void updateAudioDevices()
        {
            //get all avaible audio devices
            int deviceCount = WaveIn.DeviceCount;
            for (byte i = 0; i < INPUTS; i++)
            {
                configureInputForm.clearItems(i);
                configureInputForm.addItems(i, deviceCount);
            }

        }

        internal static void draw()
        {
            configureInputForm.disableTimer();
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
            configureInputForm.enableTimer();
        }

        internal static void paint(PaintEventArgs e, int i)
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
                e.Graphics.DrawImage(bitmap, 0, 0, configureInputForm.ClientRectangle, GraphicsUnit.Pixel);
                graphics.Dispose();
            }
        }

        internal static void updateDeviceSelection(byte i)
        {
            currentDevice[i] = getSelectedIndex(i);
            Console.Out.WriteLine(i + " - selected device: " + currentDevice[i]);
            if (allowRecording)
            {
                stopTest();
                test();
            }
        }

        private static int getSelectedIndex(byte i)
        {
            if (boxDevices[i].InvokeRequired)
            {
                ByteDelegateReturnInt d = new ByteDelegateReturnInt(getSelectedIndex);
                return (int) configureInputForm.Invoke(d, new object[] { i });
            }
            else
            {
                return boxDevices[i].SelectedIndex;
            }
        }

        internal static bool getAllowRecording()
        {
            return allowRecording;
        }
    }
}
