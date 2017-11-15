namespace CS310_Audio_Analysis_Project
{
    public partial class ConfigureInput : Form
    {
        public ConfigureInput()
        {
            InitializeComponent();
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
