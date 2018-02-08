using Accord;
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
        private const int THRESHOLD = 20;
        private static byte INPUTS = CS310AudioAnalysisProject.INPUTS;
        private static double[][] frequencyValues = new double[INPUTS][];
        private static int BUFFER_SIZE = CS310AudioAnalysisProject.BUFFER_SIZE;
        private static double SEPARATION = 0.5;
        private static double ROOTTWO = Math.Sqrt(2);
        private static float SCALE = 200;
        private static int SIZE = 10;
        private static System.Timers.Timer timer;
        private static bool update = true;
        private static DoublePoint[][] points = new DoublePoint[4][];
        private static double a, b, c, d, fl, fr, bl, br, front, back, left, right, frontR, backR, leftR, rightR, distance, newDistance;
        private static Circle circleFront, circleBack, circleLeft, circleRight;
        private static List<Double> bestPointsX, bestPointsY;
        private static int next;
        private static DoublePoint[] closestPoints;
        private static List<FrequencyPoint> frequencyPoints = new List<FrequencyPoint>();

        public AnalysisForm()
        {
            timer = new System.Timers.Timer(15);
            timer.Enabled = false;
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            InitializeComponent();
        }

        internal void copyFrequencyData(double[][] frequencyValues, byte i)
        {
            if (update)
            {
                timer.Stop();
                AnalysisForm.frequencyValues[i] = new double[BUFFER_SIZE];
                Array.Copy(frequencyValues[i], AnalysisForm.frequencyValues[i], BUFFER_SIZE);
                if (i == (byte)(INPUTS - 1))
                {
                    update = false;
                    timer.Start();
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            update = false;
            timer.Stop();
            locateFrequencies();
            timer.Start();
            update = true;
        }

        private void locateFrequencies()
        {
            frequencyPoints = new List<FrequencyPoint>();
            for (int j = 0; j < BUFFER_SIZE; j++)
            {
                a = Math.Sqrt(frequencyValues[0][j]);
                b = Math.Sqrt(frequencyValues[1][j]);
                c = Math.Sqrt(frequencyValues[2][j]);
                d = Math.Sqrt(frequencyValues[3][j]);
                if (a + b + c + d > THRESHOLD)
                {
                    //r = 1/8d - d/2
                    //a = d + r
                    //y = (1/2)(yB+yA) + (1/2)(yB-yA)(rA2-rB2)/d2 ± -2(xB-xA)K/d2 
                    fl = Math.Sqrt(a);
                    fr = Math.Sqrt(b);
                    bl = Math.Sqrt(c);
                    br = Math.Sqrt(d);
                    front = (fr / (fl + fr)) - 0.5;
                    back = (br / (bl + br)) - 0.5;
                    left = (fl / (fl + bl)) - 0.5;
                    right = (fr / (fr + br)) - 0.5;
                    //leftDiag = fl / (fl + br) - ROOTTWO / 2;
                    //rightDiag = fr / (fr + bl) - ROOTTWO / 2;
                    //front and right
                    frontR = 1 / (8 * front) - front / 2;
                    backR = 1 / (8 * back) - back / 2;
                    leftR = 1 / (8 * left) - left / 2;
                    rightR = 1 / (8 * right) - right / 2;
                    circleFront = new Circle(new DoublePoint((front + frontR) * SEPARATION, 0.5 * SEPARATION), frontR * SEPARATION);
                    circleBack = new Circle(new DoublePoint((back + backR) * SEPARATION, -0.5 * SEPARATION), backR * SEPARATION);
                    circleLeft = new Circle(new DoublePoint(-0.5 * SEPARATION, (left + leftR) * SEPARATION), leftR * SEPARATION);
                    circleRight = new Circle(new DoublePoint(0.5 * SEPARATION, (right + rightR) * SEPARATION), rightR * SEPARATION);
                    points[0] = circleFront.intersect(circleLeft);
                    points[1] = circleLeft.intersect(circleBack);
                    points[2] = circleBack.intersect(circleRight);
                    points[3] = circleRight.intersect(circleFront);
                    bestPointsX = new List<Double>();
                    bestPointsY = new List<Double>();
                    for (int i = 0; i < points.Length; i++)
                    {
                        next = (i + 1) % points.Length;
                        distance = points[i][0].DistanceTo(points[next][0]);
                        closestPoints = new DoublePoint[2] { points[i][0], points[next][0] };
                        newDistance = points[i][0].DistanceTo(points[next][1]);
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
                    frequencyPoints.Add(new FrequencyPoint(new DoublePoint(bestPointsX.Average(), bestPointsY.Average()), j));
                }
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
            for (int i = 0; i < frequencyPoints.Count(); i++)
            {
                DoublePoint doublePoint = frequencyPoints[i].DoublePoint;
                if (!(double.IsNaN(doublePoint.X)) && !(double.IsNaN(doublePoint.Y)))
                {
                    double ratio = ((double) frequencyPoints[i].Frequency * 100 / (BUFFER_SIZE - 1)) % 1.0;
                    byte red = 0;
                    byte green = 0;
                    byte blue = 0;
                    if (ratio <= 1.0 / 6.0)
                    {
                        red = 255;
                        green = (byte)(ratio * 6.0 * 255);
                    }
                    else if (ratio <= 2.0 / 6.0)
                    {
                        red = (byte)(((2.0 / 6.0) - ratio) * 6.0 * 255);
                        green = 255;
                    }
                    else if (ratio <= 3.0 / 6.0)
                    {
                        green = 255;
                        blue = (byte)((ratio - (2.0 / 6.0)) * 6.0 * 255);
                         
                    }
                    else if (ratio <= 4.0 / 6.0)
                    {
                        green = (byte)(((4.0 / 6.0) - ratio) * 6.0 * 255);
                        blue = 255;
                    }
                    else if (ratio <= 5.0 / 6.0)
                    {
                        red = (byte)((ratio - (4.0 / 6.0)) * 6.0 * 255);
                        blue = 255;
                    }
                    else
                    {
                        red = 255;
                        blue = (byte)(((6.0 / 6.0) - ratio) * 6.0 * 255);
                    }
                    Color colour = Color.FromArgb(red, green, blue);
                    graphics.FillEllipse(
                        new SolidBrush(colour),
                        (int)(picAnalysis.Width / 2 + (doublePoint.X * SCALE) - (SIZE / 2)),
                        (int)(picAnalysis.Height / 2 - (doublePoint.Y * SCALE) - (SIZE / 2)),
                        SIZE,
                        SIZE);
                }
            }
            e.Graphics.DrawImage(bitmap, 0, 0, ClientRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
        }
    }
}
