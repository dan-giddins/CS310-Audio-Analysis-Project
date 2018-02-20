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
        private const bool DRAW_SPHERES = false;
        private const bool DRAW_INTERSECTIONS = false;
        private const bool DRAW_CIRCLES = true;
        private const bool DRAW_POINTS = false;
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
        private static double fl, fr, bl, br, front, back, left, right, frontR, backR, leftR, rightR, distance, newDistance;
        private static int next;
        private static DoublePoint[] closestPoints;
        private static List<FrequencyPoint> frequencyPoints = new List<FrequencyPoint>();
        private delegate void VoidDelegate();

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
                    Sphere[] spheres = new Sphere[4];
                    spheres[0] = new Sphere(new DoublePoint((front + frontR) * SEPARATION, 0.5 * SEPARATION), frontR * SEPARATION);
                    spheres[1] = new Sphere(new DoublePoint(-0.5 * SEPARATION, (left + leftR) * SEPARATION), leftR * SEPARATION);
                    spheres[2] = new Sphere(new DoublePoint((back + backR) * SEPARATION, -0.5 * SEPARATION), backR * SEPARATION);
                    spheres[3] = new Sphere(new DoublePoint(0.5 * SEPARATION, (right + rightR) * SEPARATION), rightR * SEPARATION);
                    Circle[] circles = new Circle[6];
                    circles[0] = spheres[0].intersect(spheres[1]);
                    circles[1] = spheres[0].intersect(spheres[2]);
                    circles[2] = spheres[0].intersect(spheres[3]);
                    circles[3] = spheres[1].intersect(spheres[2]);
                    circles[4] = spheres[1].intersect(spheres[3]);
                    circles[5] = spheres[2].intersect(spheres[3]);
                    List<DoublePoint3D> bestPoints = new List<DoublePoint3D>();
                    /*for (int j = 0; j < circles.Length; j++)
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
                    }*/
                    for (int j = 0; j < circles.Length; j++)
                    {
                        if (!double.IsNaN(circles[j].radius))
                        {
                            for (int k = j + 1; k < circles.Length; k++)
                            {
                                if (!double.IsNaN(circles[k].radius))
                                {
                                    DoublePoint3D point = circles[j].intersect(circles[k]);
                                    if (!double.IsNaN(point.x))
                                    {
                                        bestPoints.Add(point);
                                    }
                                }
                            }
                        }
                    }
                    DoublePoint3D avgPoint = new DoublePoint3D(Double.NaN, Double.NaN, Double.NaN);
                    if (bestPoints.Count() > 0)
                    {
                        for (int j = 0; j < bestPoints.Count(); j++)
                        {
                            avgPoint.x += bestPoints[j].x;
                            avgPoint.y += bestPoints[j].y;
                            avgPoint.z += bestPoints[j].z;
                        }
                        avgPoint.x = avgPoint.x / bestPoints.Count();
                        avgPoint.y = avgPoint.y / bestPoints.Count();
                        avgPoint.z = avgPoint.z / bestPoints.Count();
                    }
                    frequencyPoints.Add(new FrequencyPoint(
                        avgPoint,
                        bestPoints,
                        spheres,
                        circles,
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
                    if (DRAW_SPHERES)
                    {
                        Sphere[] spheres = frequencyPoints[i].spheres;
                        for (int j = 0; j < spheres.Length; j++)
                        {
                            Sphere sphere = spheres[j];
                            if (!Double.IsInfinity(sphere.radius))
                            {
                                graphics.DrawEllipse(
                                    new Pen(colour),
                                    (int)((picAnalysis.Width * 0.5) + (sphere.center.X - sphere.radius) * SCALE),
                                    (int)((picAnalysis.Height * 0.5) - (sphere.center.Y + sphere.radius) * SCALE),
                                    (int)(sphere.radius * SCALE * 2),
                                    (int)(sphere.radius * SCALE * 2));
                            }
                        }
                    }
                    if (DRAW_INTERSECTIONS)
                    {
                        Circle[] circles = frequencyPoints[i].circles;
                        for (int j = 0; j < circles.Length; j++)
                        {
                            Circle circle = circles[j];
                            if (!double.IsNaN(circle.points[0].X))
                            {
                                graphics.FillEllipse(
                                    new SolidBrush(colour),
                                    (int)((picAnalysis.Width * 0.5) + (circle.points[0].X * SCALE) - (SIZE * 0.5)),
                                    (int)((picAnalysis.Height * 0.5) - (circle.points[0].Y * SCALE) - (SIZE * 0.5)),
                                    SIZE,
                                    SIZE);
                                graphics.FillEllipse(
                                    new SolidBrush(colour),
                                    (int)((picAnalysis.Width * 0.5) + (circle.points[1].X * SCALE) - (SIZE * 0.5)),
                                    (int)((picAnalysis.Height * 0.5) - (circle.points[1].Y * SCALE) - (SIZE * 0.5)),
                                    SIZE,
                                    SIZE);
                            }
                        }
                    }
                    if (DRAW_POINTS)
                    {
                        DoublePoint3D doublePoint = frequencyPoints[i].doublePoint;
                        if (!double.IsNaN(doublePoint.x))
                        {
                            graphics.FillEllipse(
                                new SolidBrush(colour),
                                (int)((picAnalysis.Width * 0.5) + (doublePoint.x * SCALE) - (SIZE * 0.5)),
                                (int)((picAnalysis.Height * 0.5) - (doublePoint.y * SCALE) - (SIZE * 0.5)),
                                SIZE,
                                SIZE);
                        }
                    }
                    if (DRAW_CIRCLES)
                    {
                        Circle[] circles = frequencyPoints[i].circles;
                        for (int j = 0; j < circles.Length; j++)
                        {
                            Circle circle = circles[j];
                            if (!double.IsNaN(circle.points[0].X))
                            {
                                graphics.DrawLine(
                                    new Pen(colour),
                                    (int)((picAnalysis.Width * 0.5) + (circle.points[0].X) * SCALE),
                                    (int)((picAnalysis.Height * 0.5) - (circle.points[0].Y) * SCALE),
                                    (int)((picAnalysis.Width * 0.5) + (circle.points[1].X) * SCALE),
                                    (int)((picAnalysis.Height * 0.5) - (circle.points[1].Y) * SCALE));
                            }
                        }
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
