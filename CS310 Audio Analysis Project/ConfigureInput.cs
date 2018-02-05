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
        private Thread configureInputThread;

        internal ConfigureInputForm(Thread configureInputThread)
        {
            this.configureInputThread = configureInputThread;
            InitializeComponent();
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

        private void boxDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.updateDeviceSelection();       
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

        internal ComboBox getBoxDevice()
        {
            return boxDevice;
        }

        internal void clearItems()
        {
            VoidDelegate d = new VoidDelegate(boxDevice.Items.Clear);
            Invoke(d);
        }

        internal void addItems(int deviceCount)
        {
            for (byte j = 0; j < deviceCount; j++)
            {
                WaveInCapabilities capabilities = WaveIn.GetCapabilities(j);
                for (int k = 0; k < capabilities.Channels; k++)
                {
                    string element = j + ": " + capabilities.ProductName + ", Channel: " + k;
                    StringDelegateReturnInt d = new StringDelegateReturnInt(boxDevice.Items.Add);
                    Invoke(d, new object[] { element });
                }
            }
        }

        private void btnFrequencies_Click(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.startFrequencyThread();
        }

        private void chkReadFile_CheckedChanged(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.chkReadFileChanged(chkReadFile.Checked);
        }

        private void btnAnalysise_Click(object sender, EventArgs e)
        {
            CS310AudioAnalysisProject.analyse();
            Close();
        }
    }
}
