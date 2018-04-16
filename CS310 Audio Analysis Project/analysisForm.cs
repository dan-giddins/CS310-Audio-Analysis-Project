using Accord;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace CS310_Audio_Analysis_Project
{
    // analyis view and computation
    public partial class AnalysisForm : Form
    {
        private static int THRESHOLD;
        private static bool DRAW_CIRCLES;
        private static bool DRAW_INTERSECTIONS;
        private static bool DRAW_POINTS;
        private static int SOLO_FREQ;
        private static bool SOLO;
        private static double EXPANDER;
        private static double GROUP;
        private static bool ENTITIES;
        private static bool ALL;
        private static float SCALE;
        private static int SIZE;
        private static double SEPARATION;
        private static byte INPUTS = CS310AudioAnalysisProject.INPUTS;
        private static double[][] frequencyValues = new double[INPUTS][];
        private static int BUFFER_SIZE = CS310AudioAnalysisProject.BUFFER_SIZE;      
        private static double ROOTTWO = Math.Sqrt(2);
        private static System.Timers.Timer timer;
        private static bool update = true;
        private static DoublePoint[][] points = new DoublePoint[4][];
        private static double fl, fr, bl, br, front, back, left, right, frontR, backR, leftR, rightR;
        private static Circle circleFront, circleBack, circleLeft, circleRight;
        private static List<FrequencyPoint> frequencyPoints = new List<FrequencyPoint>();
        private static List<FrequencyPoint> entityPoints = new List<FrequencyPoint>();
        private delegate void VoidDelegate();
        private static Double avgX = 0;
        private static Double avgY = 0;

        // ui update controls
        private void txtScale_TextChanged(object sender, EventArgs e)
        {
            float.TryParse(txtScale.Text, out SCALE);
        }

        private void txtSize_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtSize.Text, out SIZE);
        }

        private void txtSeparation_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(txtSeparation.Text, out SEPARATION);
        }

        private void txtThreshold_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtThreshold.Text, out THRESHOLD);
        }

        private void txtFreq_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtFreq.Text, out SOLO_FREQ);
        }

        private void txtExpander_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(txtExpander.Text, out EXPANDER);
        }

        private void txtGroup_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(txtGroup.Text, out GROUP);
        }

        private void chkCircles_CheckedChanged(object sender, EventArgs e)
        {
            DRAW_CIRCLES = chkCircles.Checked;
        }

        private void chkIntersections_CheckedChanged(object sender, EventArgs e)
        {
            DRAW_INTERSECTIONS = chkIntersections.Checked;
        }

        private void chkPoints_CheckedChanged(object sender, EventArgs e)
        {
            DRAW_POINTS = chkPoints.Checked;
        }

        private void chkSolo_CheckedChanged(object sender, EventArgs e)
        {
            SOLO = chkSolo.Checked;
        }

        private void chkEntities_CheckedChanged(object sender, EventArgs e)
        {
            ENTITIES = chkEntities.Checked;
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            ALL = chkAll.Checked;
        }

        public AnalysisForm()
        {
            InitializeComponent();
            float.TryParse(txtScale.Text, out SCALE);
            int.TryParse(txtSize.Text, out SIZE);
            double.TryParse(txtSeparation.Text, out SEPARATION);
            int.TryParse(txtThreshold.Text, out THRESHOLD);
            int.TryParse(txtFreq.Text, out SOLO_FREQ);
            double.TryParse(txtExpander.Text, out EXPANDER);
            double.TryParse(txtGroup.Text, out GROUP);
            DRAW_CIRCLES = chkCircles.Checked;
            DRAW_INTERSECTIONS = chkIntersections.Checked;
            DRAW_POINTS = chkPoints.Checked;
            SOLO = chkSolo.Checked;
            ENTITIES = chkEntities.Checked;
            ALL = chkAll.Checked;
            // drawing timer
            timer = new System.Timers.Timer(15);
            timer.Enabled = false;
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
        }

        internal void copyFrequencyData(double[][] frequencyValues, byte i)
        {
            // copy data into thread
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
            // perform location calculations
            locateFrequencies();
            timer.Start();
            update = true;
        }

        private void locateFrequencies()
        {
            frequencyPoints = new List<FrequencyPoint>();
            entityPoints = new List<FrequencyPoint>();
            // for each frequency
            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                if (i == 20) {
                    i = 20;
                }
                fl = Math.Sqrt(frequencyValues[0][i]);
                fr = Math.Sqrt(frequencyValues[1][i]);
                bl = Math.Sqrt(frequencyValues[2][i]);
                br = Math.Sqrt(frequencyValues[3][i]);
                // if frequency values are above a threshold
                if (fl + fr + bl + br > THRESHOLD)
                {
                    //r = 1/8d - d/2
                    //a = d + r
                    //y = (1/2)(yB+yA) + (1/2)(yB-yA)(rA2-rB2)/d2 ± -2(xB-xA)K/d2 
                    // calculate ratios
                    front = ((fr/ (fl + fr)) - 0.5) * EXPANDER;
                    back = ((br / (bl + br)) - 0.5) * EXPANDER;
                    left = ((fl / (fl + bl)) - 0.5) * EXPANDER;
                    right = ((fr / (fr + br)) - 0.5) * EXPANDER;
                    //leftDiag = fl / (fl + br) - ROOTTWO * 0.5;
                    //rightDiag = fr / (fr + bl) - ROOTTWO * 0.5;
                    //front and right
                    // radiuses
                    frontR = 1 / (8 * front) - front * 0.5;
                    backR = 1 / (8 * back) - back * 0.5;
                    leftR = 1 / (8 * left) - left * 0.5;
                    rightR = 1 / (8 * right) - right * 0.5;
                    // circles
                    circleFront = new Circle(new DoublePoint((front + frontR) * SEPARATION, 0.5 * SEPARATION), frontR * SEPARATION);
                    circleBack = new Circle(new DoublePoint((back + backR) * SEPARATION, -0.5 * SEPARATION), backR * SEPARATION);
                    circleLeft = new Circle(new DoublePoint(-0.5 * SEPARATION, (left + leftR) * SEPARATION), leftR * SEPARATION);
                    circleRight = new Circle(new DoublePoint(0.5 * SEPARATION, (right + rightR) * SEPARATION), rightR * SEPARATION);
                    // intersection points
                    points[0] = circleFront.intersect(circleLeft);
                    points[1] = circleFront.intersect(circleRight);
                    points[2] = circleBack.intersect(circleLeft);
                    points[3] = circleBack.intersect(circleRight);
                    List<DoublePoint> bestPoints = new List<DoublePoint>();
                    // get internal points
                    for (int j = 0; j < points.Length; j++)
                    {
                        if (!double.IsNaN(points[j][0].X))
                        {
                            if (Math.Abs(points[j][0].X) + Math.Abs(points[j][0].Y) < Math.Abs(points[j][1].X) + Math.Abs(points[j][1].Y))
                            {
                                bestPoints.Add(points[j][0]);
                            }
                            else
                            {
                                bestPoints.Add(points[j][1]);
                            }
                        }
                    }
                    // calculate average
                    if (bestPoints.Count() == 0)
                    {
                        avgX = double.NaN;
                        avgY = double.NaN;
                    }
                    else
                    {
                        avgX = 0;
                        avgY = 0;
                        for (int j = 0; j < bestPoints.Count(); j++)
                        {
                            avgX += bestPoints[j].X;
                            avgY += bestPoints[j].Y;
                        }
                        avgX = avgX / bestPoints.Count();
                        avgY = avgY / bestPoints.Count();
                    }
                    // add to list
                    frequencyPoints.Add(new FrequencyPoint(
                        new DoublePoint(avgX, avgY),
                        bestPoints,
                        new Circle[] { circleFront, circleBack, circleLeft, circleRight },
                        i));
                }
            }
            for (int i = 0; i < frequencyPoints.Count(); i++)
            {
                if (!double.IsNaN(frequencyPoints[i].doublePoint.X))
                {
                    bool added = false;
                    for (int j = 0; j < entityPoints.Count(); j++)
                    {
                        // group close points together as an entity
                        if (frequencyPoints[i].doublePoint.DistanceTo(entityPoints[j].doublePoint) < GROUP)
                        {
                            entityPoints[j].points.Add(frequencyPoints[i].doublePoint);
                            added = true;
                            break;
                        }
                    }
                    if (!added)
                    {
                        // make a new entity if point is not close to other entity
                        List<DoublePoint> list = new List<DoublePoint>();
                        list.Add(frequencyPoints[i].doublePoint);
                        entityPoints.Add(new FrequencyPoint(
                            new DoublePoint(frequencyPoints[i].doublePoint.X, frequencyPoints[i].doublePoint.Y),
                            list,
                            null,
                            frequencyPoints[i].frequency));
                    }
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
            // draw microphones
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
            // draw entites
            if (ENTITIES)
            {
                for (int i = 0; i < entityPoints.Count(); i++)
                {

                    Color colour = getColour(entityPoints[i].frequency);
                    if (ALL)
                    {
                        for (int j = 0; j < entityPoints[i].points.Count(); j++)
                        {
                            // draw every frequency point
                            graphics.FillEllipse(
                                new SolidBrush(colour),
                                (int)((picAnalysis.Width * 0.5) + (entityPoints[i].points[j].X * SCALE) - (SIZE * 0.5)),
                                (int)((picAnalysis.Height * 0.5) - (entityPoints[i].points[j].Y * SCALE) - (SIZE * 0.5)),
                                SIZE,
                                SIZE);
                        }
                    }
                    else
                    {
                        graphics.FillEllipse(
                                new SolidBrush(colour),
                                (int)((picAnalysis.Width * 0.5) + (entityPoints[i].doublePoint.X * SCALE) - (SIZE * 0.5)),
                                (int)((picAnalysis.Height * 0.5) - (entityPoints[i].doublePoint.Y * SCALE) - (SIZE * 0.5)),
                                SIZE,
                                SIZE);
                    }
                }
            }
            else
            {
                for (int i = 0; i < frequencyPoints.Count(); i++)
                {
                    Color colour = Color.Black;
                    bool flag = false;
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
                        colour = getColour(frequencyPoints[i].frequency);
                    }
                    if (flag)
                    {
                        if (DRAW_CIRCLES)
                        {
                            // draw loci circles
                            Circle[] circles = frequencyPoints[i].circles;
                            for (int j = 0; j < circles.Length; j++)
                            {
                                Circle circle = circles[j];
                                try
                                {
                                    graphics.DrawEllipse(
                                        new Pen(colour),
                                        (int)((picAnalysis.Width * 0.5) + (circle.Center.X - circle.Radius) * SCALE),
                                        (int)((picAnalysis.Height * 0.5) - (circle.Center.Y + circle.Radius) * SCALE),
                                        (int)(circle.Radius * SCALE * 2),
                                        (int)(circle.Radius * SCALE * 2));
                                } catch (OverflowException) { }
                            }
                        }
                        if (DRAW_INTERSECTIONS)
                        {
                            // draw intersection points
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
                            // draw frequency points
                            DoublePoint doublePoint = frequencyPoints[i].doublePoint;
                            if (!double.IsNaN(doublePoint.X))
                            {
                                graphics.FillEllipse(
                                    new SolidBrush(colour),
                                    (int)((picAnalysis.Width * 0.5) + (doublePoint.X * SCALE) - (SIZE * 0.5)),
                                    (int)((picAnalysis.Height * 0.5) - (doublePoint.Y * SCALE) - (SIZE * 0.5)),
                                    SIZE,
                                    SIZE);
                            }
                            if (SOLO)
                            {
                                // calculate and draw error 
                                lblX.Text = "X: " + (float)doublePoint.X;
                                lblY.Text = "Y: " + (float)doublePoint.Y;
                                float x = 0;
                                float y = 0;
                                float.TryParse(txtX.Text, out x);
                                float.TryParse(txtY.Text, out y);
                                float loss = (float)Math.Sqrt(Math.Pow(doublePoint.X - x, 2) + Math.Pow(doublePoint.Y - y, 2));
                                lblError.Text = "Error: " + loss;
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
            // calculate drawing colour for a given frequency
            double ratio = ((double)i * 100 / (BUFFER_SIZE - 1)) % 1.0;
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
            // handle form closing event and cleanup
            CS310AudioAnalysisProject.allowRecording = false;
            CS310AudioAnalysisProject.closing = true;
            CS310AudioAnalysisProject.analysis = false;
            CS310AudioAnalysisProject.eventWaitHandle.Set();
        }
    }
}
