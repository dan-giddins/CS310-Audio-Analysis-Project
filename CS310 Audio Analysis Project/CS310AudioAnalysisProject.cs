using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.Drawing;
using System.Threading;
using System.Numerics;

namespace CS310_Audio_Analysis_Project
{
    static class CS310AudioAnalysisProject
    {
        private const int SAMPLE_RATE = 44100;
        private static int BUFFER_SIZE = (int) Math.Pow(2, 13);
        private const byte BIT_DEPTH = 16;
        private const int BYTES_PER_SAMPLE = BIT_DEPTH / 8;
        private const byte INPUTS = 4;
        private static WaveInEvent[] waveIn;
        private static BufferedWaveProvider[] bufferedWaveProvider;
        private static int[] currentDevice;
        private static int[][] waveValues;
        private static double[][] frequencyValues;
        private static byte[][] frameArray;
        private static PictureBox[] picWaveform;
        private static PictureBox[] picFrequency;
        private static ComboBox[] boxDevices;
        private static bool[] recording;
        private static bool allowRecording = false;
        private static bool frequencyDrawing = false;
        private static bool analysis = false;
        private static ConfigureInputForm configureInputForm;
        private static FrequencyForm frequencyForm;
        //private static AnalysisForm analysisForm;
        private static Thread configureInputThread;
        private static Thread frequencyThread;
        private static Thread analysisThread;
        private delegate void IntDelegate(int i);
        private delegate void ByteObjectDelegate(byte b, object o);
        private delegate object ByteDelegateReturnObject(byte b);
        private delegate int ByteDelegateReturnInt(byte b);
        private static EventWaitHandle drawHandel = new EventWaitHandle(false, EventResetMode.AutoReset);
        

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            configureInputThread = new Thread(runConfigureInputForm);
            configureInputForm = new ConfigureInputForm(drawHandel, configureInputThread);
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
            stopTest();
            test();
            while (allowRecording)
            {
                drawHandel.WaitOne();
                drawTest();
            }
            if (analysis)
            {
                analysisThread = new Thread(runAnalysisForm);
            }
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
            //Application.Run(analysisForm);
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
            waveValues = new int[INPUTS][];
            picWaveform = new PictureBox[INPUTS];
            picWaveform[0] = configureInputForm.getPicWaveform0();
            picWaveform[1] = configureInputForm.getPicWaveform1();
            picWaveform[2] = configureInputForm.getPicWaveform2();
            picWaveform[3] = configureInputForm.getPicWaveform3();
            frameArray = new byte[INPUTS][];
            for (int i = 0; i < INPUTS; i++)
            {
                waveIn[i] = new WaveInEvent();
                waveValues[i] = new int[BUFFER_SIZE];
                frameArray[i] = new byte[BUFFER_SIZE * BYTES_PER_SAMPLE];
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

        internal static void generateFrequencyArrays()
        {
            frequencyValues = new double[INPUTS][];
            picFrequency = new PictureBox[INPUTS];
            picFrequency[0] = frequencyForm.getPicFrequency0();
            picFrequency[1] = frequencyForm.getPicFrequency1();
            picFrequency[2] = frequencyForm.getPicFrequency2();
            picFrequency[3] = frequencyForm.getPicFrequency3();
            for (int i = 0; i < INPUTS; i++)
            {
                frequencyValues[i] = new double[BUFFER_SIZE];
            }
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

        internal static void startFrequencyThread()
        {
            frequencyThread = new Thread(runFrequencyForm);
            frequencyForm = new FrequencyForm();
            generateFrequencyArrays();
            frequencyThread.Start();

        }

        private static void configWaveBuffer(byte i)
        {
            bufferedWaveProvider[i] = new BufferedWaveProvider(waveIn[i].WaveFormat);
            bufferedWaveProvider[i].BufferLength = BUFFER_SIZE * 2;
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

        internal static void enableAnalysis()
        {
            analysis = true;
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
            int deviceCount = WaveIn.DeviceCount;
            for (byte i = 0; i < INPUTS; i++)
            {
                configureInputForm.clearItems(i);
                configureInputForm.addItems(i, deviceCount);
            }

        }

        internal static void drawTest()
        {
            configureInputForm.disableTimer();
            for (byte i = 0; i < INPUTS; i++)
            {
                if (currentDevice[i] > -1)
                {
                    bufferedWaveProvider[i].Read(frameArray[i], 0, frameArray[i].Length);
                    for (int j = 0; j < waveValues[i].Length; j++)
                    {
                        waveValues[i][j] = (frameArray[i][j * 2 + 1] << 8) | frameArray[i][j * 2];
                    }
                    picWaveform[i].Invalidate();
                    if (frequencyDrawing)
                    {
                        frequencyValues[i] = FFT(waveValues[i]);
                        picFrequency[i].Invalidate();
                    }
                }
            }
            configureInputForm.enableTimer();
        }

        internal static void paintWaveform(PaintEventArgs e, int i)
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
            for (int j = 0; j < waveValues[i].Length; j++)
            {
                yPosOld = yPosNew;
                if (waveValues[i][j] < overFlow)
                {
                    yPosNew = (int)((overFlow - waveValues[i][j]) * yScale);
                }
                else
                {
                    yPosNew = (int)((overFlow * 3 - waveValues[i][j]) * yScale);
                }
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
            double yScale = 0.01;
            for (int j = 0; j < frequencyValues[i].Length / xScale; j++)
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

        internal static void disableRecording()
        {
            allowRecording = false;
        }

        private static double[] FFT(int[] data)
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
