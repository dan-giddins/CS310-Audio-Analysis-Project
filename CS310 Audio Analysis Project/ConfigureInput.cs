using NAudio.Wave;
using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace CS310_Audio_Analysis_Project
{
    public partial class ConfigureInputForm : Form
    {
        private delegate void VoidDelegate();
        private delegate int StringDelegateReturnInt(string s);
        private EventWaitHandle drawHandel;
        private System.Timers.Timer timerDraw;
        private Thread configureInputThread;

        internal ConfigureInputForm(EventWaitHandle drawHandel, Thread configureInputThread)
        {
            this.drawHandel = drawHandel;
            this.configureInputThread = configureInputThread;
            timerDraw = new System.Timers.Timer(15);
            timerDraw.Enabled = false;
            timerDraw.Elapsed += new ElapsedEventHandler(timerDraw_Tick);
            timerDraw.Start();
            InitializeComponent();
        }

        private void timerDraw_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke((MethodInvoker)delegate
                    {
                        timerDraw_Tick(sender, e);
                    });
                }
                catch (Exception exception)
                {
                    if (exception is ObjectDisposedException || exception is InvalidAsynchronousStateException)
                    {
                        Console.Out.WriteLine("Closing Configure Input");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                drawHandel.Set();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (CS310AudioAnalysisProject.getAllowRecording())
            {
                CS310AudioAnalysisProject.updateAudioDevices();
            }  
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (CS310AudioAnalysisProject.getAllowRecording())
            {
                CS310AudioAnalysisProject.stopTest();
                CS310AudioAnalysisProject.test();
            }
        }

        private void boxDevices0_SelectedIndexChanged(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.updateDeviceSelection(0);       
        }

        private void boxDevices1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.updateDeviceSelection(1);
        }

        private void boxDevices2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.updateDeviceSelection(2);
        }

        private void boxDevices3_SelectedIndexChanged(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.updateDeviceSelection(3);
        }

        private void picWaveform0_Paint(object sender, PaintEventArgs e)
        {
            CS310AudioAnalysisProject.paintWaveform(e, 0);
        }

        private void picWaveform1_Paint(object sender, PaintEventArgs e)
        {
            CS310AudioAnalysisProject.paintWaveform(e, 1);
        }

        private void picWaveform2_Paint(object sender, PaintEventArgs e)
        {
            CS310AudioAnalysisProject.paintWaveform(e, 2);
        }

        private void picWaveform3_Paint(object sender, PaintEventArgs e)
        {
            CS310AudioAnalysisProject.paintWaveform(e, 3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.breakPoint();
        }

        internal PictureBox getPicWaveform0()
        {
            return picWaveform0;
        }

        internal PictureBox getPicWaveform1()
        {
            return picWaveform1;
        }

        internal PictureBox getPicWaveform2()
        {
            return picWaveform2;
        }

        internal PictureBox getPicWaveform3()
        {
            return picWaveform3;
        }

        internal ComboBox getBoxDevices0()
        {
            return boxDevices0;
        }

        internal ComboBox getBoxDevices1()
        {
            return boxDevices1;
        }

        internal ComboBox getBoxDevices2()
        {
            return boxDevices2;
        }

        internal ComboBox getBoxDevices3()
        {
            return boxDevices3;
        }

        internal void clearItems(byte i)
        {
            VoidDelegate d = null;
            switch (i)
            {
                case 0:
                    d = new VoidDelegate(boxDevices0.Items.Clear);
                    break;
                case 1:
                    d = new VoidDelegate(boxDevices1.Items.Clear);
                    break;
                case 2:
                    d = new VoidDelegate(boxDevices2.Items.Clear);
                    break;
                case 3:
                    d = new VoidDelegate(boxDevices3.Items.Clear);
                    break;
            }
            Invoke(d);
        }

        internal void addItems(byte i, int deviceCount)
        {
            for (byte j = 0; j < deviceCount; j++)
            {
                WaveInCapabilities capabilities = WaveIn.GetCapabilities(j);
                String element = j + ": " + capabilities.ProductName + ", Channels: " + capabilities.Channels;
                StringDelegateReturnInt d = null;
                switch (i)
                {
                    case 0:
                        d = new StringDelegateReturnInt(boxDevices0.Items.Add);
                        break;
                    case 1:
                        d = new StringDelegateReturnInt(boxDevices1.Items.Add);
                        break;
                    case 2:
                        d = new StringDelegateReturnInt(boxDevices2.Items.Add);
                        break;
                    case 3:
                        d = new StringDelegateReturnInt(boxDevices3.Items.Add);
                        break;
                }
                Invoke(d, new object[] { element });
            }
        }

        internal void enableTimer()
        {
            timerDraw.Enabled = true;
        }

        internal void disableTimer()
        {
            timerDraw.Enabled = false;
        }

        private void ConfigureInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CS310AudioAnalysisProject.enableAnalysis();
            CS310AudioAnalysisProject.disableRecording();
            drawHandel.Set();
        }

        private void btnFrequencies_Click(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.startFrequencyThread();
        }
    }
}
