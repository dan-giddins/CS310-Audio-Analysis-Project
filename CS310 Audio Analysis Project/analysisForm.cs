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
        private const bool DRAW_CIRCLES = false;
        private const bool DRAW_INTERSECTIONS = false;
        private const bool DRAW_POINTS = true;
        private const int SOLO_FREQ = 30;
        private const bool SOLO = false;
        private static byte INPUTS = CS310AudioAnalysisProject.INPUTS;
        private static double[][] frequencyValues = new double[INPUTS][];
        private static int BUFFER_SIZE = CS310AudioAnalysisProject.BUFFER_SIZE;
        private static double SEPARATION = 0.6;
        private static double ROOTTWO = Math.Sqrt(2);
        private static float SCALE = 200;
        private static int SIZE = 10;
        private static System.Timers.Timer timer;
        private static bool update = true;
        private static DoublePoint[][] points = new DoublePoint[6][];
        private static double fl, fr, bl, br, front, back, left, right, frontR, backR, leftR, rightR, distance, newDistance;
        private static Sphere sphereFront, sphereBack, sphereLeft, sphereRight;
        private static List<DoublePoint> bestPoints;
        private static int next;
        private static DoublePoint[] closestPoints;
        private static List<FrequencyPoint> frequencyPoints = new List<FrequencyPoint>();
        private delegate void VoidDelegate();
        private static Double avgX = 0;
        private static Double avgY = 0;

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
            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                if (i == 20) {
                    i = 20;
                }
                fl = Math.Sqrt(frequencyValues[0][i]);
                fr = Math.Sqrt(frequencyValues[1][i]);
                bl = Math.Sqrt(frequencyValues[2][i]);
                br = Math.Sqrt(frequencyValues[3][i]);
                if (fl + fr + bl + br > THRESHOLD)
                {
                    //r = 1/8d - d/2
                    //a = d + r
                    //y = (1/2)(yB+yA) + (1/2)(yB-yA)(rA2-rB2)/d2 ± -2(xB-xA)K/d2 
                    front = (fr / (fl + fr)) - 0.5;
                    back = (br / (bl + br)) - 0.5;
                    left = (fl / (fl + bl)) - 0.5;
                    right = (fr / (fr + br)) - 0.5;
                    //leftDiag = fl / (fl + br) - ROOTTWO * 0.5;
                    //rightDiag = fr / (fr + bl) - ROOTTWO * 0.5;
                    //front and right
                    frontR = 1 / (8 * front) - front * 0.5;
                    backR = 1 / (8 * back) - back * 0.5;
                    leftR = 1 / (8 * left) - left * 0.5;
                    rightR = 1 / (8 * right) - right * 0.5;
                    sphereFront = new Sphere(new DoublePoint((front + frontR) * SEPARATION, 0.5 * SEPARATION), frontR * SEPARATION);
                    sphereBack = new Sphere(new DoublePoint((back + backR) * SEPARATION, -0.5 * SEPARATION), backR * SEPARATION);
                    sphereLeft = new Sphere(new DoublePoint(-0.5 * SEPARATION, (left + leftR) * SEPARATION), leftR * SEPARATION);
                    sphereRight = new Sphere(new DoublePoint(0.5 * SEPARATION, (right + rightR) * SEPARATION), rightR * SEPARATION);
                    points[0] = sphereFront.intersect(sphereLeft);
                    points[1] = sphereFront.intersect(sphereBack);
                    points[2] = sphereFront.intersect(sphereRight);
                    points[3] = sphereLeft.intersect(sphereBack);
                    points[4] = sphereLeft.intersect(sphereRight);
                    points[5] = sphereBack.intersect(sphereRight);
                    bestPoints = new List<DoublePoint>();
                    for (int j = 0; j < points.Length; j++)
                    {
                        next = (j + 1) % points.Length;
                        distance = points[j][0].DistanceTo(points[next][0]);
                        closestPoints = new DoublePoint[2] { points[j][0], points[next][0] };
                        newDistance = points[j][0].DistanceTo(points[next][1]);
                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            closestPoints = new DoublePoint[2] { points[j][0], points[next][1] };
                        }
                        newDistance = points[j][1].DistanceTo(points[next][1]);
                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            closestPoints = new DoublePoint[2] { points[j][1], points[next][1] };
                        }
                        newDistance = points[j][1].DistanceTo(points[next][0]);
                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            closestPoints = new DoublePoint[2] { points[j][1], points[next][0] };
                        }
                        for (int k = 0; k < closestPoints.Length; k++)
                        {
                            if (!double.IsNaN(closestPoints[k].X))
                            {
                                bestPoints.Add(closestPoints[k]);
                            }
                        }
                    }
                    /*{
                        for (int k = 0; k < points[j].Length; k++)
                        {
                            if (!double.IsNaN(points[j][k].X))
                            {
                                bestPoints.Add(points[j][k]);
                            }
                        }
                    }*/
                    if (bestPoints.Count() == 0)
                    {
                        avgX = 0;
                        avgY = 0;
                    }
                    else
                    {
                        for (int j = 0; j < bestPoints.Count(); j++)
                        {
                            avgX += bestPoints[j].X;
                            avgY += bestPoints[j].Y;
                        }
                        avgX = avgX / bestPoints.Count();
                        avgY = avgY / bestPoints.Count();
                    }
                    frequencyPoints.Add(new FrequencyPoint(
                        new DoublePoint(avgX, avgY),
                        bestPoints,
                        new Circle[] { sphereFront, sphereBack, sphereLeft, sphereRight },
                        i));
                }
            }
            drawPoints();
        }

        private void drawPoints()
        {
            picAnalysis.Invalidate();
            updatePic();
        }

        private void updatePic()
        {
            try
            {
                if (picAnalysis.InvokeRequired)
                {
                    VoidDelegate d = new VoidDelegate(updatePic);
                    Invoke(d, new object[] { });
                }
                else
                {
                    picAnalysis.Update();
                }
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e);
            }
        }

        private void picAnalysis_Paint(object sender, PaintEventArgs e)
        {
            if (picAnalysis.Image != null)
            {
                picAnalysis.Image.Dispose();
            }
            Bitmap bitmap = new Bitmap(picAnalysis.Width, picAnalysis.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            graphics.FillRectangle(
                blackBrush,
                (int)((picAnalysis.Width * 0.5) + (SEPARATION * SCALE * 0.5) - (SIZE * 0.5)),
                (int)((picAnalysis.Height * 0.5) - (SEPARATION * SCALE * 0.5) - (SIZE * 0.5)),
                SIZE,
                SIZE);
            graphics.FillRectangle(
                blackBrush,
                (int)((picAnalysis.Width * 0.5) + (-SEPARATION * SCALE * 0.5) - (SIZE * 0.5)),
                (int)((picAnalysis.Height * 0.5) - (SEPARATION * SCALE * 0.5) - (SIZE * 0.5)),
                SIZE,
                SIZE);
            graphics.FillRectangle(
                blackBrush,
                (int)((picAnalysis.Width * 0.5) + (SEPARATION * SCALE * 0.5) - (SIZE * 0.5)),
                (int)((picAnalysis.Height * 0.5) - (-SEPARATION * SCALE * 0.5) - (SIZE * 0.5)),
                SIZE,
                SIZE);
            graphics.FillRectangle(
                blackBrush,
                (int)((picAnalysis.Width * 0.5) + (-SEPARATION * SCALE * 0.5) - (SIZE * 0.5)),
                (int)((picAnalysis.Height * 0.5) - (-SEPARATION * SCALE * 0.5) - (SIZE * 0.5)),
                SIZE,
                SIZE);
            for (int i = 0; i < frequencyPoints.Count(); i++)
            {
                bool flag = false;
                Color colour = Color.Black;
                if (SOLO)
                {
                    if (frequencyPoints[i].frequency == SOLO_FREQ)
                    {
                        flag = true;
                    }
                }
                else
                {
                    flag = true;
                    colour = getColour(i);
                }
                if (flag)
                {
                    if (DRAW_CIRCLES)
                    {
                        Circle[] circles = frequencyPoints[i].circles;
                        for (int j = 0; j < circles.Length; j++)
                        {
                            Circle circle = circles[j];
                            graphics.DrawEllipse(
                                new Pen(colour),
                                (int)((picAnalysis.Width * 0.5) + (circle.Center.X - circle.Radius) * SCALE),
                                (int)((picAnalysis.Height * 0.5) - (circle.Center.Y + circle.Radius) * SCALE),
                                (int)(circle.Radius * SCALE * 2),
                                (int)(circle.Radius * SCALE * 2));
                        }
                    }
                    if (DRAW_INTERSECTIONS)
                    {
                        for (int j = 0; j < frequencyPoints[i].points.Count(); j++)
                        {
                            DoublePoint doublePointInter = frequencyPoints[i].points[j];
                            graphics.FillEllipse(
                                new SolidBrush(colour),
                                (int)((picAnalysis.Width * 0.5) + (doublePointInter.X * SCALE) - (SIZE * 0.5)),
                                (int)((picAnalysis.Height * 0.5) - (doublePointInter.Y * SCALE) - (SIZE * 0.5)),
                                SIZE,
                                SIZE);
                        }
                    }
                    if (DRAW_POINTS)
                    {
                        DoublePoint doublePoint = frequencyPoints[i].doublePoint;
                        graphics.FillEllipse(
                            new SolidBrush(colour),
                            (int)((picAnalysis.Width * 0.5) + (doublePoint.X * SCALE) - (SIZE * 0.5)),
                            (int)((picAnalysis.Height * 0.5) - (doublePoint.Y * SCALE) - (SIZE * 0.5)),
                            SIZE,
                            SIZE);
                    }
                }
            }
            e.Graphics.DrawImage(bitmap, 0, 0, ClientRectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
        }

        private Color getColour(int i)
        {
            double ratio = ((double)frequencyPoints[i].frequency * 100 / (BUFFER_SIZE - 1)) % 1.0;
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
            return Color.FromArgb(red, green, blue);
        }

        private void AnalysisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CS310AudioAnalysisProject.allowRecording = false;
            CS310AudioAnalysisProject.closing = true;
            CS310AudioAnalysisProject.analysis = false;
            CS310AudioAnalysisProject.eventWaitHandle.Set();
        }
    }
}
