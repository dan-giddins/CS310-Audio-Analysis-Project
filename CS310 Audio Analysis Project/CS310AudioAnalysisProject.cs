using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.Drawing;
using System.Threading;
using System.Numerics;
using System.Timers;
using System.Collections.Generic;
using NAudio.CoreAudioApi;
using System.Linq;

namespace CS310_Audio_Analysis_Project
{
    static class CS310AudioAnalysisProject
    {
        private const int SAMPLE_RATE = 44100;
        internal static int BUFFER_SIZE = (int) Math.Pow(2, 11);
        private const byte BIT_DEPTH = 16;
        private const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
        internal const byte INPUTS = 4;
        internal const double SEPARATION = 1;
        private const byte MAX_CHANNELS = 2;
        private static WasapiCapture waveIn;
        private static BufferedWaveProvider[] bufferedWaveProvider = new BufferedWaveProvider[INPUTS];
        private static Device currentDevice;
        private static short[][] waveValues = new short[INPUTS][];
        private static double[][] frequencyValues = new double[INPUTS][];
        private static byte[][] frameArray = new byte[INPUTS][];
        private static PictureBox[] picWaveform = new PictureBox[INPUTS];
        private static PictureBox[] picFrequency = new PictureBox[INPUTS];
        private static ComboBox boxDevice;
        private static bool[] recording = new bool[INPUTS];
        private static bool allowRecording = false;
        private static bool frequencyDrawing = false;
        private static bool analysis;
        private static ConfigureInputForm configureInputForm;
        private static FrequencyForm frequencyForm;
        private static AnalysisForm analysisForm;
        private static Thread configureInputThread;
        private static Thread frequencyThread;
        private static Thread analysisThread;
        private delegate void Delegate();
        private delegate void ByteObjectDelegate(object o);
        private delegate object DelegateReturnObject();
        private delegate int DelegateReturnInt();
        private static EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static bool readFromFile;
        private static WaveFileReader[] waveFileReader = new WaveFileReader[4];
        private static System.Timers.Timer timer;
        private static List<Device> deviceMap = new List<Device>();

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            configureInputThread = new Thread(runConfigureInputForm);
            configureInputForm = new ConfigureInputForm(configureInputThread);
            configureArrays();
            configureInputThread.Start();
            updateAudioDevices();
            displayDevices();
            updateDeviceSelection();
            allowRecording = true;
            timer = new System.Timers.Timer(15);
            timer.Enabled = false;
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            timer.Start();
            test();
            while (allowRecording)
            {
                eventWaitHandle.WaitOne();
                readData();
                if (analysis)
                {
                    if (analysisForm == null)
                    {
                        startAnalysisThread();
                    }
                    for (byte i = 0; i < INPUTS; i++)
                    {
                        if (readFromFile || i < currentDevice.channelCount)
                        {
                            analysisForm.copyFrequencyData(frequencyValues, i);
                        }             
                    }
                }
                else
                {
                    drawTest();
                }
            }
        }

        private static void timer_Tick(object sender, ElapsedEventArgs e)
        {
            eventWaitHandle.Set();
        }

        private static void runConfigureInputForm()
        {
            Application.Run(configureInputForm);
        }

        private static void runFrequencyForm()
        {
            Application.Run(frequencyForm);
        }

        private static void runAnalysisForm()
        {
            Application.Run(analysisForm);
        }

        private static void displayDevices()
        {
            setSelectedIndex();
            setSelectedItem(getItem());
            updateDeviceSelection();
        }

        private static Object getItem()
        {
            if (boxDevice.InvokeRequired)
            {
                DelegateReturnObject d = new DelegateReturnObject(getItem);
                return configureInputForm.Invoke(d, new object[] { });
            }
            else
            {
                try
                {
                    return boxDevice.Items[0];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
            }
        }

        private static void setSelectedItem(object o)
        {
            if (boxDevice.InvokeRequired)
            {
                ByteObjectDelegate d = new ByteObjectDelegate(setSelectedItem);
                configureInputForm.Invoke(d, new object[] {o});
            }
            else
            {
                boxDevice.SelectedItem = o;
            }
        }

        private static void setSelectedIndex()
        {
            if (boxDevice.InvokeRequired)
            {
                Delegate d = new Delegate(setSelectedIndex);
                configureInputForm.Invoke(d, new object[] {});
            }
            else
            {
                try
                {
                    boxDevice.SelectedIndex = 0;
                }
                catch (ArgumentOutOfRangeException)
                {
                    boxDevice.SelectedIndex = -1;
                }
            }
        }

        private static void configureArrays()
        {
            picWaveform[0] = configureInputForm.getPicWaveform0();
            picWaveform[1] = configureInputForm.getPicWaveform1();
            picWaveform[2] = configureInputForm.getPicWaveform2();
            picWaveform[3] = configureInputForm.getPicWaveform3();
            waveIn = new WasapiCapture();
            for (int i = 0; i < INPUTS; i++)
            {
                waveValues[i] = new short[BUFFER_SIZE];
                frameArray[i] = new byte[BUFFER_SIZE * BYTES_PER_SAMPLE * MAX_CHANNELS];
            }
            boxDevice = configureInputForm.getBoxDevice();
        }

        internal static void configureFrequencyArrays()
        {
            picFrequency[0] = frequencyForm.getPicFrequency0();
            picFrequency[1] = frequencyForm.getPicFrequency1();
            picFrequency[2] = frequencyForm.getPicFrequency2();
            picFrequency[3] = frequencyForm.getPicFrequency3();
            for (int i = 0; i < INPUTS; i++)
            {
                frequencyValues[i] = new double[BUFFER_SIZE];
            }
        }

        internal static void configureFileArrays()
        {
            for (int i = 0; i < INPUTS; i++)
            {
                waveFileReader[i] = new WaveFileReader("audio/input.wav");
            }
        }

        internal static void test()
        {
            if (readFromFile)
            {
                configureFileArrays();
            }
            else
            {
                for (byte i = 0; i < INPUTS; i++)
                {
                    configWaveIn(i);
                }
                //waveIn[0].DataAvailable += new EventHandler<WaveInEventArgs>(waveInDataAvailable0);
                waveIn.DataAvailable += waveInDataAvailable;
            }
            for (byte i = 0; i < INPUTS; i++)
            {
                configWaveBuffer(i);
            }
            timer.Enabled = true;
        }

        internal static void analyse()
        {
            analysis = true;
        }

        internal static void chkReadFileChanged(bool checkState)
        {
            stopTest();
            readFromFile = checkState;
            test();
        }

        internal static void startFrequencyThread()
        {
            frequencyThread = new Thread(runFrequencyForm);
            frequencyForm = new FrequencyForm();
            configureFrequencyArrays();
            frequencyThread.Start();

        }

        internal static void startAnalysisThread()
        {
            analysisThread = new Thread(runAnalysisForm);
            analysisForm = new AnalysisForm();
            analysisThread.Start();
        }

        private static void configWaveBuffer(byte i)
        {
            WaveFormat waveFormat;
            if (readFromFile)
            {
                waveFormat = waveFileReader[i].WaveFormat;
            }
            else
            {
                waveFormat = waveIn[i].WaveFormat;
            }
            bufferedWaveProvider[i] = new BufferedWaveProvider(waveFormat);
            bufferedWaveProvider[i].BufferLength = BUFFER_SIZE * 2;
            bufferedWaveProvider[i].ReadFully = false;
            if (readFromFile)
            {
                bufferedWaveProvider[i].DiscardOnBufferOverflow = false;
                int length = (int) waveFileReader[i].Length;
                length = BUFFER_SIZE;
                byte[] tempArray = new byte[length];
                waveFileReader[i].Read(tempArray, 0, length);
                bufferedWaveProvider[i].AddSamples(tempArray, 0, length);
            }
            else
            {
                bufferedWaveProvider[i].DiscardOnBufferOverflow = true;
                startRecording(i); 
            }
            
        }

        private static void startRecording(byte i)
        {
            if (!recording[i] && i < currentDevice.channelCount)
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
            if (i < currentDevice.channelCount)
            {
                // Event sync may be false here
                waveIn = new WasapiCapture(currentDevice.device, true, (int)((double)BUFFER_SIZE / SAMPLE_RATE * 1000.0));
                waveIn.WaveFormat = new WaveFormat(SAMPLE_RATE, currentDevice[i].channelCount);
            }
        }

        internal static void stopTest()
        {
            timer.Enabled = false;
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

        private static void waveInDataAvailable(object sender, WaveInEventArgs e)
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
            var deviceEnum = new MMDeviceEnumerator();
            List<MMDevice> devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            int deviceCount = devices.Count();
            for (byte i = 0; i < deviceCount; i++)
            {
                deviceMap.Add(new Device(devices[i]));
            }
            for (byte i = 0; i < INPUTS; i++)
            {
                configureInputForm.clearItems();
                configureInputForm.addItems(deviceCount);
            }

        }

        internal static void drawTest()
        {
            timer.Enabled = false;
            for (byte i = 0; i < INPUTS; i++)
            {
                if (readFromFile || i < currentDevice.channelCount)
                {
                    picWaveform[i].Invalidate();
                    if (frequencyDrawing)
                    {
                        picFrequency[i].Invalidate();
                    }
                }
            }
            timer.Enabled = true;
        }

        private static void readData()
        {
            for (byte i = 0; i < INPUTS; i++)
            {
                if (readFromFile || i < currentDevice.channelCount)
                {
                    bufferedWaveProvider[i].Read(frameArray[i], 0, frameArray[i].Length * currentDevice[i].channelCount / MAX_CHANNELS);
                    int k = currentDevice[i].channelNo;
                    for (int j = 0; j < waveValues[i].Length; j++)
                    {
                        waveValues[i][j] = (short)((frameArray[i][k * 2 + 1] << 8) | frameArray[i][k * 2]);
                        k += currentDevice[i].channelCount;
                    }
                    frequencyValues[i] = FFT(waveValues[i]);
                }
            }
        }

        internal static void paintWaveform(PaintEventArgs e, int i)
        {
            if (picWaveform[i].Image != null)
            {
                picWaveform[i].Image.Dispose();
            }
            Bitmap bitmap = new Bitmap(picWaveform[i].Width, picWaveform[i].Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            short yPosOld = 0;
            short yPosNew = 0;
            double yScale = (picWaveform[i].Height - 1) / Math.Pow(2, 16);
            for (int j = 0; j < picWaveform[i].Width; j++)
            {
                yPosOld = yPosNew;
                yPosNew = (short) ((waveValues[i][j]) * yScale + picWaveform[i].Height / 2);
                if (j > 1)
                {
                    graphics.DrawLine(Pens.Black, j - 1, yPosOld, j, yPosNew);
                }
            }
            e.Graphics.DrawImage(bitmap, 0, 0, configureInputForm.ClientRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
        }

        internal static void paintFrequency(PaintEventArgs e, int i)
        {
            if (picFrequency[i].Image != null)
            {
                picFrequency[i].Image.Dispose();
            }
            Bitmap bitmap = new Bitmap(picFrequency[i].Width, picFrequency[i].Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            int xScale = 1;
            int yPosOld = 0;
            int yPosNew = 0;
            double yScale = 0.1;
            for (int j = 0; j < picFrequency[i].Width / xScale; j++)
            {
                yPosOld = yPosNew;
                yPosNew = (int)(picFrequency[i].Height - frequencyValues[i][j] * yScale);
                if (j > 1)
                {
                    graphics.DrawLine(Pens.Black, (j - 1) * xScale, yPosOld, j * xScale, yPosNew);
                }
            }
            e.Graphics.DrawImage(bitmap, 0, 0, frequencyForm.ClientRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
        }

        internal static void updateDeviceSelection()
        {
            try
            {
                currentDevice = deviceMap[getSelectedIndex()];
                Console.Out.WriteLine("Selected device: " + currentDevice.device.FriendlyName);
            } catch (ArgumentOutOfRangeException)
            {
                currentDevice = null;
            }
            
            if (allowRecording)
            {
                stopTest();
                test();
            }
        }

        private static int getSelectedIndex()
        {
            if (boxDevice.InvokeRequired)
            {
                DelegateReturnInt d = new DelegateReturnInt(getSelectedIndex);
                return (int) configureInputForm.Invoke(d, new object[] {});
            }
            else
            {
                return boxDevice.SelectedIndex;
            }
        }

        internal static bool getAllowRecording()
        {
            return allowRecording;
        }

        internal static void disableRecording()
        {
            allowRecording = false;
        }

        private static double[] FFT(short[] data)
        {
            double[] fft = new double[data.Length];
            Complex[] fftComplex = new Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                fftComplex[i] = new Complex(data[i], 0.0);
            }
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < data.Length; i++)
            {
                fft[i] = fftComplex[i].Magnitude;
            }
            return fft;
        }

        internal static void allowFrequencyDrawing()
        {
            frequencyDrawing = true;
        }

        internal static void disallowFrequencyDrawing()
        {
            frequencyDrawing = false;
        }
    }
}
