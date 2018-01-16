using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS310_Audio_Analysis_Project
{
    public partial class AnalysisForm : Form
    {
        private static double[][] frequencyValues = new double[CS310AudioAnalysisProject.INPUTS][];

        public AnalysisForm()
        {
            InitializeComponent();
        }

        internal void copyFrequencyData(double[][] frequencyValues, byte i)
        {
            AnalysisForm.frequencyValues[i] = new double[CS310AudioAnalysisProject.BUFFER_SIZE];
            Array.Copy(frequencyValues[i], AnalysisForm.frequencyValues[i], CS310AudioAnalysisProject.BUFFER_SIZE);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            locateFrequencies();
        }

        private void locateFrequencies()
        {
            for (int j = 0; j < CS310AudioAnalysisProject.BUFFER_SIZE; j++)
            {
                double leftFront = frequencyValues[0][j] / frequencyValues[1][j];
                double leftBack = frequencyValues[2][j] / frequencyValues[3][j];
                double leftSide = frequencyValues[0][j] / frequencyValues[2][j];
                double rightSide = frequencyValues[1][j] / frequencyValues[3][j];
                double leftDiag = frequencyValues[0][j] / frequencyValues[3][j];
                double rightDiag = frequencyValues[1][j] / frequencyValues[2][j];
            }
        }
    }
}
