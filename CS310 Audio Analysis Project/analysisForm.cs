using Accord;
using ColorMine.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
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
        private static DoublePoint[] frequencyPoints = new DoublePoint[BUFFER_SIZE];
        private static float SCALE = 200;
        private static int SIZE = 10;
        private static System.Timers.Timer timer;

        public AnalysisForm()
        {
            timer = new System.Timers.Timer(15);
            timer.Enabled = false;
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            timer.Start();
            InitializeComponent();
        }

        internal void copyFrequencyData(double[][] frequencyValues, byte i)
        {
            AnalysisForm.frequencyValues[i] = new double[BUFFER_SIZE];
            Array.Copy(frequencyValues[i], AnalysisForm.frequencyValues[i], BUFFER_SIZE);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            locateFrequencies();
            timer.Start();
        }

        private void locateFrequencies()
        {
            for (int j = 0; j < BUFFER_SIZE; j++)
            {
                //r = 1/8d - d/2
                //a = d + r
                //y = (1/2)(yB+yA) + (1/2)(yB-yA)(rA2-rB2)/d2 ± -2(xB-xA)K/d2 
                double fl = Math.Sqrt(frequencyValues[0][j]);
                double fr = Math.Sqrt(frequencyValues[1][j]);
                double bl = Math.Sqrt(frequencyValues[2][j]);
                double br = Math.Sqrt(frequencyValues[3][j]);
                double front = (fr / (fl + fr)) - 0.5;
                double back = (br / (bl + br)) - 0.5;
                double left = (fl / (fl + bl)) - 0.5;
                double right = (fr / (fr + br)) - 0.5;
                //double leftDiag = fl / (fl + br) - ROOTTWO / 2;
                //double rightDiag = fr / (fr + bl) - ROOTTWO / 2;
                //front and right
                double frontR = 1 / (8 * front) - front / 2;
                double backR = 1 / (8 * back) - back / 2;
                double leftR = 1 / (8 * left) - left / 2;
                double rightR = 1 / (8 * right) - right / 2;
                Circle circleFront = new Circle(new DoublePoint((front + frontR) * SEPARATION, 0.5 * SEPARATION), frontR * SEPARATION);
                Circle circleBack = new Circle(new DoublePoint((back + backR) * SEPARATION, -0.5 * SEPARATION), backR * SEPARATION);
                Circle circleLeft = new Circle(new DoublePoint(-0.5 * SEPARATION, (left + leftR) * SEPARATION), leftR * SEPARATION);
                Circle circleRight = new Circle(new DoublePoint(0.5 * SEPARATION, (right + rightR) * SEPARATION), rightR * SEPARATION);
                DoublePoint[][] points = new DoublePoint[4][];
                points[0] = circleFront.intersect(circleLeft);
                points[1] = circleLeft.intersect(circleBack);
                points[2] = circleBack.intersect(circleRight);
                points[3] = circleRight.intersect(circleFront);
                List<Double> bestPointsX = new List<Double>();
                List<Double> bestPointsY = new List<Double>();
                for (int i = 0; i < points.Length; i++)
                {
                    int next = (i + 1) % points.Length;
                    double distance = points[i][0].DistanceTo(points[next][0]);
                    DoublePoint[] closestPoints = new DoublePoint[2] { points[i][0], points[next][0] };
                    double newDistance = points[i][0].DistanceTo(points[next][1]);
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        closestPoints = new DoublePoint[2] { points[i][0], points[next][1] };
                    }
                    newDistance = points[i][1].DistanceTo(points[next][1]);
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        closestPoints = new DoublePoint[2] { points[i][1], points[next][1] };
                    }
                    newDistance = points[i][1].DistanceTo(points[next][0]);
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        closestPoints = new DoublePoint[2] { points[i][1], points[next][0] };
                    }
                    bestPointsX.Add(closestPoints[0].X);
                    bestPointsX.Add(closestPoints[1].X);
                    bestPointsY.Add(closestPoints[0].Y);
                    bestPointsY.Add(closestPoints[1].Y);
                }
                frequencyPoints[j] = new DoublePoint(bestPointsX.Average(), bestPointsY.Average());
            }
            drawPoints();
        }

        private void drawPoints()
        {
            picAnalysis.Invalidate();
        }

        private void picAnalysis_Paint(object sender, PaintEventArgs e)
        {
            if (picAnalysis.Image != null)
            {
                picAnalysis.Image.Dispose();
            }
            Bitmap bitmap = new Bitmap(picAnalysis.Width, picAnalysis.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            for (int i = 0; i < frequencyPoints.Length; i++)
            {
                var myRgb = (new Hsv((double) i / frequencyPoints.Length, 1, 1)).To<Rgb>();
                Color colour = Color.FromArgb((int)(myRgb.R), (int)(myRgb.G), (int)(myRgb.B));
                graphics.FillEllipse(
                    new SolidBrush(colour),
                    (int)(picAnalysis.Width / 2 + (frequencyPoints[i].X * SCALE) - (SIZE / 2)),
                    (int)(picAnalysis.Height / 2 - (frequencyPoints[i].Y * SCALE) - (SIZE / 2)),
                    SIZE,
                    SIZE);
            }
            e.Graphics.DrawImage(bitmap, 0, 0, ClientRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
        }
    }
}
