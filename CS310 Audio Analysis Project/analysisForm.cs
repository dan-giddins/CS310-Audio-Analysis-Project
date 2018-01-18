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
        private static byte INPUTS = CS310AudioAnalysisProject.INPUTS;
        private static double[][] frequencyValues = new double[INPUTS][];
        private static int BUFFER_SIZE = CS310AudioAnalysisProject.BUFFER_SIZE;
        private static double SEPARATION = CS310AudioAnalysisProject.SEPARATION;
        private static double ROOTTWO = Math.Sqrt(2);

        public AnalysisForm()
        {
            InitializeComponent();
        }

        internal void copyFrequencyData(double[][] frequencyValues, byte i)
        {
            AnalysisForm.frequencyValues[i] = new double[BUFFER_SIZE];
            Array.Copy(frequencyValues[i], AnalysisForm.frequencyValues[i], BUFFER_SIZE);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            locateFrequencies();
        }

        private void locateFrequencies()
        {
            for (int j = 0; j < BUFFER_SIZE; j++)
            {
                //r = 1/8d - d/2
                //a = d + r
                double fl = Math.Sqrt(frequencyValues[0][j]);
                double fr = Math.Sqrt(frequencyValues[1][j]);
                double bl = Math.Sqrt(frequencyValues[2][j]);
                double br = Math.Sqrt(frequencyValues[3][j]);
                double front = (fr / (fl + fr)) - 0.5;
                double back = (br / (bl + br)) - 0.5;
                double leftSide = (fl / (fl + bl)) - 0.5;
                double rightSide = (fr / (fr + br)) - 0.5;
                double leftDiag = fl / (fl + br) - ROOTTWO / 2;
                double rightDiag = fr / (fr + bl) - ROOTTWO / 2;

            }
        }
    }
}
