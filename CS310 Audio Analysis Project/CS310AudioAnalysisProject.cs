using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Numerics;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace CS310_Audio_Analysis_Project
{
    static class CS310AudioAnalysisProject
    {
        private const int SAMPLE_RATE = 44100;
        internal static int BUFFER_SIZE = (int) Math.Pow(2, 11);
        private const byte BIT_DEPTH = 16;
        private const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
        internal const byte INPUTS = 4;
        private const byte MAX_CHANNELS = 4;
        private static BufferedWaveProvider bufferedWaveProvider;
        private static Device currentDevice;
        private static short[][] waveValues = new short[INPUTS][];
        private static double[][] frequencyValues = new double[INPUTS][];
        private static byte[] frameArray = new byte[BUFFER_SIZE * BYTES_PER_SAMPLE * MAX_CHANNELS];
        private static float[] samples = new float[BUFFER_SIZE * MAX_CHANNELS];
        private static byte[] sampleBytes = new byte[BUFFER_SIZE * BYTES_PER_SAMPLE * MAX_CHANNELS];
        private static PictureBox[] picWaveform = new PictureBox[INPUTS];
        private static PictureBox[] picFrequency = new PictureBox[INPUTS];
        private static ComboBox boxDevice;
        private static bool recording;
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
        private static WaveFileReader waveFileReader;
        private static System.Timers.Timer timer;
        private static List<Device> deviceMap = new List<Device>();

        [STAThread]
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
            for (int i = 0; i < INPUTS; i++)
            {
                waveValues[i] = new short[BUFFER_SIZE];
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
                waveFileReader = new WaveFileReader("audio/input.wav");
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
                configWaveIn();
                currentDevice.device.AudioAvailable += OnAsioOutAudioAvailable;
            }
            configWaveBuffer();
            timer.Enabled = true;
        }

        private static void OnAsioOutAudioAvailable(object sender, AsioAudioAvailableEventArgs e)
        {
            e.GetAsInterleavedSamples(samples);
            int sample;
            int j;
            for (int i = 0; i < samples.Length; i++)
            {
                sample = (int)(samples[i] * Math.Pow(2, 15));
                j = i * 2;
                sampleBytes[j] = (byte) sample;
                sampleBytes[j + 1] = (byte) (sample >> 8);
            }
            bufferedWaveProvider.AddSamples(sampleBytes, 0, sampleBytes.Length);
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

        private static void configWaveBuffer()
        {
            WaveFormat waveFormat;
            if (readFromFile)
            {
                waveFormat = waveFileReader.WaveFormat;
            }
            else
            {
                waveFormat = new WaveFormat(SAMPLE_RATE, currentDevice.channelCount);
            }
            bufferedWaveProvider = new BufferedWaveProvider(waveFormat);
            bufferedWaveProvider.BufferLength = BUFFER_SIZE * 2;
            bufferedWaveProvider.ReadFully = false;
            if (readFromFile)
            {
                bufferedWaveProvider.DiscardOnBufferOverflow = false;
                int length = (int) waveFileReader.Length;
                length = BUFFER_SIZE;
                byte[] tempArray = new byte[length];
                waveFileReader.Read(tempArray, 0, length);
                bufferedWaveProvider.AddSamples(tempArray, 0, length);
            }
            else
            {
                bufferedWaveProvider.DiscardOnBufferOverflow = true;
                startRecording(); 
            }
            
        }

        private static void startRecording()
        {
            if (!recording)
            {
                try
                {
                    currentDevice.device.Play();
                    recording = true;
                }
                catch (NAudio.MmException e)
                {
                    Console.Out.WriteLine(e);
                }
            }
        }

        private static void configWaveIn()
        {
            //asioOut.InputChannelOffset = 4;
            currentDevice.device.InitRecordAndPlayback(null, currentDevice.channelCount, SAMPLE_RATE);
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
            if (recording)
            {
                try
                {
                    currentDevice.device.Stop();
                }
                catch (NAudio.MmException e)
                {
                    Console.Out.WriteLine(e);
                }
                recording = false;
            }
        }

        private static void waveInDataAvailable(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        internal static void updateAudioDevices()
        {
            var deviceEnum = new MMDeviceEnumerator();
            String[] devices = AsioOut.GetDriverNames();
            deviceMap.Add(new Device(new AsioOut("UMC ASIO Driver")));
            int deviceCount = devices.Count();
            deviceCount = 0;
            for (byte i = 0; i < deviceCount; i++)
            {
                try
                {
                    deviceMap.Add(new Device(new AsioOut(devices[i])));
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(i + ": " + devices[i] + " failed with error:");
                    Console.WriteLine(e);
                }
            }
            configureInputForm.clearItems();
            configureInputForm.addItems(deviceMap);
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
            bufferedWaveProvider.Read(frameArray, 0, frameArray.Length * currentDevice.channelCount / MAX_CHANNELS);
            for (byte i = 0; i < INPUTS; i++)
            {
                if (readFromFile || i < currentDevice.channelCount)
                {
                    int k = i;
                    for (int j = 0; j < waveValues[i].Length; j++)
                    {
                        waveValues[i][j] = (short)((frameArray[k * 2 + 1] << 8) | frameArray[k * 2]);
                        k += currentDevice.channelCount;
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
                Console.Out.WriteLine("Selected device: " + currentDevice.device.DriverName);
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
