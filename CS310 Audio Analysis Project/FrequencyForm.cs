using System.Windows.Forms;

namespace CS310_Audio_Analysis_Project
{
    public partial class FrequencyForm : Form
    {
        public FrequencyForm()
        {
            InitializeComponent();
            CS310AudioAnalysisProject.allowFrequencyDrawing();
        }

        internal PictureBox getPicFrequency0()
        {
            return picFrequency0;
        }

        internal PictureBox getPicFrequency1()
        {
            return picFrequency1;
        }

        internal PictureBox getPicFrequency2()
        {
            return picFrequency2;
        }

        internal PictureBox getPicFrequency3()
        {
            return picFrequency3;
        }

        private void FrequencyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CS310AudioAnalysisProject.disallowFrequencyDrawing();
        }

        private void picFrequency0_Paint(object sender, PaintEventArgs e)
        {
            CS310AudioAnalysisProject.paintFrequency(e, 0);
        }

        private void picFrequency1_Paint(object sender, PaintEventArgs e)
        {
            CS310AudioAnalysisProject.paintFrequency(e, 1);
        }

        private void picFrequency2_Paint(object sender, PaintEventArgs e)
        {
            CS310AudioAnalysisProject.paintFrequency(e, 2);
        }

        private void picFrequency3_Paint(object sender, PaintEventArgs e)
        {
            CS310AudioAnalysisProject.paintFrequency(e, 3);
        }
    }
}
