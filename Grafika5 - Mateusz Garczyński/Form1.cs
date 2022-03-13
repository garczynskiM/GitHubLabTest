namespace Grafika5___Mateusz_Garczyński
{
    using Grafika5___Mateusz_Garczyński.DisplayStuff;
    using Grafika5___Mateusz_Garczyński.FillPolygon;
    using Grafika5___Mateusz_Garczyński.Shapes;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    public partial class Form1 : Form
    {

        private const float zInf = 1000000;
        private const int ballRadius = 40;
        private double tableUpperEdgeHeight;
        private double tableBottomEdgeHeight;
        private bool ballGoingUp = true;
        private int complexity = 3;

        private My4DPoint ballCenter;
        private List<My4DShape> shapes;
        private MyCamera topDownViewCamera;
        private MyCamera sideViewCamera;
        private MyCamera ballViewCamera;
        private MyCamera currentCamera;
        private Bitmap bitmapToDraw;

        private MyReflector overheadLight;
        private MyReflector sideLight;
        private MyReflector ballLight;
        private ShadingType currentShadingType;

        private Matrix<double> projectionMatrix;
        private Matrix<double> tableModelMatrix;
        private Matrix<double> ballModelMatrix;

        private Timer refreshTimer;
        private const int targetFrameRate = 30;
        public Form1()
        {
            InitializeComponent();
            InitializeBitmap();
            InitializeModelMatrixes();
            InitializeShapes();
            InitializeCameras();
            InitializeLight();
            InitializeTimer();
        }
        private void InitializeBitmap()
        {
            bitmapToDraw = new Bitmap(drawingPanel.Width, drawingPanel.Height);
        }
        private void InitializeModelMatrixes()
        {
            tableModelMatrix = Matrix<double>.Build.DenseIdentity(4);
            ballModelMatrix = Matrix<double>.Build.DenseIdentity(4);
            /*moveUpModelMatrix = Matrix<double>.Build.DenseIdentity(4);
            moveUpModelMatrix[1, 3] = 1;
            moveDownModelMatrix = Matrix<double>.Build.DenseIdentity(4);
            moveDownModelMatrix[1, 3] = -1;*/
        }
        private void InitializeShapes()
        {
            shapes = new List<My4DShape>();
            InitializeTable();
            InitializeBall();
        }
        private void InitializeTable()
        {
            My4DPoint tableCenter = new My4DPoint(0, 0, 0, 1);
            int tableBottomHeight = -200;
            int tableTopHeight = -150;
            int tableBottomLength = 800;
            int tableBottomWidth = 600;
            int outerEdgeWidth = 100;
            tableUpperEdgeHeight = tableCenter.Y + tableBottomWidth / 2;
            tableBottomEdgeHeight = tableCenter.Y - tableBottomWidth / 2;
            My4DPoint topLeft, topRight, bottomLeft, bottomRight;
            Color floorColor = Color.Green;
            Color wallColor = Color.Brown;
            Color outerEdgeColor = Color.SandyBrown;
            List<My4DTriangle> table = new List<My4DTriangle>();

            // Floor
            {
                topLeft = new My4DPoint(tableCenter.X - tableBottomLength / 2, tableCenter.Y + tableBottomWidth / 2, tableBottomHeight, 1);
                topRight = new My4DPoint(tableCenter.X + tableBottomLength / 2, tableCenter.Y + tableBottomWidth / 2, tableBottomHeight, 1);
                bottomRight = new My4DPoint(tableCenter.X + tableBottomLength / 2, tableCenter.Y - tableBottomWidth / 2, tableBottomHeight, 1);
                bottomLeft = new My4DPoint(tableCenter.X - tableBottomLength / 2, tableCenter.Y - tableBottomWidth / 2, tableBottomHeight, 1);
                table.AddRange(MeshCreatorHelper.createFromQuad(topLeft, topRight, bottomRight, bottomLeft, floorColor));
            }

            My4DPoint upperTopLeft = new My4DPoint
                    (tableCenter.X - tableBottomLength / 2, tableCenter.Y + tableBottomWidth / 2, tableTopHeight, 1);
            My4DPoint upperTopRight = new My4DPoint
                (tableCenter.X + tableBottomLength / 2, tableCenter.Y + tableBottomWidth / 2, tableTopHeight, 1);
            My4DPoint upperBottomRight = new My4DPoint
                (tableCenter.X + tableBottomLength / 2, tableCenter.Y - tableBottomWidth / 2, tableTopHeight, 1);
            My4DPoint upperBottomLeft = new My4DPoint
                (tableCenter.X - tableBottomLength / 2, tableCenter.Y - tableBottomWidth / 2, tableTopHeight, 1);

            // Walls
            {
                // Left wall
                {
                    table.AddRange(MeshCreatorHelper.createFromQuad(upperBottomLeft, upperTopLeft, topLeft, bottomLeft, wallColor));
                }

                // Top wall
                {
                    table.AddRange(MeshCreatorHelper.createFromQuad(upperTopLeft, upperTopRight, topRight, topLeft, wallColor));
                }

                // Right wall
                {
                    table.AddRange(MeshCreatorHelper.createFromQuad(upperTopRight, upperBottomRight, bottomRight, topRight, wallColor));
                }

                // Bottom wall
                {
                    table.AddRange(MeshCreatorHelper.createFromQuad(upperBottomRight, upperBottomLeft, bottomLeft, bottomRight, wallColor));
                }
            }

            My4DPoint outerTopLeft = new My4DPoint
                    (tableCenter.X - tableBottomLength / 2 - outerEdgeWidth, tableCenter.Y + tableBottomWidth / 2 + outerEdgeWidth, tableTopHeight, 1);
            My4DPoint outerTopRight = new My4DPoint
                (tableCenter.X + tableBottomLength / 2 + outerEdgeWidth, tableCenter.Y + tableBottomWidth / 2 + outerEdgeWidth, tableTopHeight, 1);
            My4DPoint outerBottomRight = new My4DPoint
                (tableCenter.X + tableBottomLength / 2 + outerEdgeWidth, tableCenter.Y - tableBottomWidth / 2 - outerEdgeWidth, tableTopHeight, 1);
            My4DPoint outerBottomLeft = new My4DPoint
                (tableCenter.X - tableBottomLength / 2 - outerEdgeWidth, tableCenter.Y - tableBottomWidth / 2 - outerEdgeWidth, tableTopHeight, 1);

            // Table edges
            {
                // Left edge
                {
                    table.AddRange(MeshCreatorHelper.createFromQuad(upperBottomLeft, outerBottomLeft, outerTopLeft, upperTopLeft, outerEdgeColor));
                }

                // Top Edge
                {
                    table.AddRange(MeshCreatorHelper.createFromQuad(upperTopLeft, outerTopLeft, outerTopRight, upperTopRight, outerEdgeColor));
                }

                // Right Edge
                {
                    table.AddRange(MeshCreatorHelper.createFromQuad(upperTopRight, outerTopRight, outerBottomRight, upperBottomRight, outerEdgeColor));
                }

                // Bottom Edge
                {
                    table.AddRange(MeshCreatorHelper.createFromQuad(upperBottomRight, outerBottomRight, outerBottomLeft, upperBottomLeft, outerEdgeColor));
                }
            }
            for (int i = 0; i < complexity; i++)
            {
                createNewTriangles(table);
            }
            shapes.Add(new My4DShape(table, tableModelMatrix, false));
        }
        private void InitializeBall()
        {
            ballCenter = new My4DPoint(0, 0, -160, 1);
            double x = ballCenter.X;
            double y = ballCenter.Y;
            double z = ballCenter.Z;
            double w = ballCenter.W;
            Color blue = Color.Blue;
            My4DPoint up = new My4DPoint(x, y + ballRadius, z, w);
            My4DPoint left = new My4DPoint(x - ballRadius, y, z, w);
            My4DPoint right = new My4DPoint(x + ballRadius, y, z, w);
            My4DPoint down = new My4DPoint(x, y - ballRadius, z, w);
            My4DPoint above = new My4DPoint(x, y, z + ballRadius, w);
            My4DPoint below = new My4DPoint(x, y, z - ballRadius, w);
            List<My4DTriangle> ball = new List<My4DTriangle>();
            ball.Add(new My4DTriangle(left, up, above, blue));
            ball.Add(new My4DTriangle(left, up, below, blue));
            ball.Add(new My4DTriangle(up, right, above, blue));
            ball.Add(new My4DTriangle(up, right, below, blue));
            ball.Add(new My4DTriangle(right, down, above, blue));
            ball.Add(new My4DTriangle(right, down, below, blue));
            ball.Add(new My4DTriangle(down, left, above, blue));
            ball.Add(new My4DTriangle(down, left, below, blue));
            for(int i = 0; i < complexity; i++)
            {
                createNewTriangles(ball);
                adjustTriangles(ball, ballRadius, ballCenter);
            }
            shapes.Add(new My4DShape(ball, ballModelMatrix, true));
        }
        private void createNewTriangles(List<My4DTriangle> triangles)
        {
            List<My4DTriangle> tempList;
            My4DPoint p0, p1, p2, p01, p12, p20;
            Color newColor;
            for (int i = 0; i < triangles.Count; i += 4)
            {
                newColor = triangles[i].Color;
                tempList = new List<My4DTriangle>();
                p0 = triangles[i].L0.P0;
                p1 = triangles[i].L1.P0;
                p2 = triangles[i].L2.P0;
                p01 = My4DPoint.getMiddleOfLine(triangles[i].L0.P0, triangles[i].L0.P1);
                p12 = My4DPoint.getMiddleOfLine(triangles[i].L1.P0, triangles[i].L1.P1);
                p20 = My4DPoint.getMiddleOfLine(triangles[i].L2.P0, triangles[i].L2.P1);
                tempList.Add(new My4DTriangle(p0, p01, p20, newColor));
                tempList.Add(new My4DTriangle(p01, p1, p12, newColor));
                tempList.Add(new My4DTriangle(p20, p12, p2, newColor));
                tempList.Add(new My4DTriangle(p12, p20, p01, newColor));
                triangles.RemoveAt(i);
                triangles.InsertRange(i, tempList);
            }
        }
        private void adjustTriangles(List<My4DTriangle> triangles, double R, My4DPoint center)
        {
            foreach (My4DTriangle t in triangles)
            {
                t.adjustToSphere(R, center);
            }
        }
        private void InitializeCameras()
        {
            // top down camera
            topDownViewCamera = new MyCamera(new My4DPoint(0, 0, 200, 1), new My4DPoint(0, 0, 0, 1),
                DenseVector.OfArray(new double[] { 0, 1, 0 }));

            // side view camera
            sideViewCamera = new MyCamera(new My4DPoint(-700, 0, 100, 1),
                new My4DPoint(ballCenter.X, ballCenter.Y, ballCenter.Z, 1), DenseVector.OfArray(new double[] { 0, 0, -1 }));

            // ball camera
            ballViewCamera = new MyCamera(new My4DPoint(ballCenter.X - 200, ballCenter.Y, ballCenter.Z + ballRadius, 1),
                new My4DPoint(ballCenter.X + 200, ballCenter.Y, ballCenter.Z + ballRadius, 1),
                DenseVector.OfArray(new double[] { 0, 0, -1 }));

            currentCamera = topDownViewCamera;
        }
        private void InitializeLight()
        {
            overheadLight = new MyReflector(new My4DPoint(0, 0, 190, 1), null);
            sideLight = new MyReflector(new My4DPoint(-650, 0, 100, 1), null);
            ballLight = new MyReflector(new My4DPoint(ballCenter.X + ballRadius + 1, ballCenter.Y, ballCenter.Z, 1),
                new My4DPoint(ballCenter.X + ballRadius + 50, ballCenter.Y, ballCenter.Z, 1));
            currentShadingType = ShadingType.Constant;
        }
        private void InitializeTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 1000 / targetFrameRate;
            refreshTimer.Tick += new EventHandler(Refresh);
            refreshTimer.Start();
        }
        private void Refresh(Object myObject, EventArgs myEventArgs)
        {
            AdjustBallMatrix();
            UpdateCameras();
            UpdateBallReflector();
            double radians = FOVTrackBar.Value * 2 * Math.PI / 360;
            double e = 1 / Math.Tan(radians / 2);
            double n = 20;
            double f = 1500;
            double currentWidth = drawingPanel.Width;
            double currentHeight = drawingPanel.Height;
            double a = currentWidth / currentHeight;
            projectionMatrix = DenseMatrix.OfArray(new double[,]
            {
                { e, 0, 0, 0 },
                { 0, e/a, 0, 0 },
                { 0, 0, -1 * (f + n)/(f - n), -1 * (2 * f * n)/(f - n) },
                { 0, 0, -1, 0 }
            });
            prepareNewBitmap();
        }
        private void AdjustBallMatrix()
        {
            double shiftPerFrame = 5;
            if(ballGoingUp)
            {
                shapes[1].ModelMatrix[1, 3] += shiftPerFrame;
                if (ballCenter.Y + ballRadius + shapes[1].ModelMatrix[1, 3] >= tableUpperEdgeHeight) ballGoingUp = !ballGoingUp;
            }
            else
            {
                shapes[1].ModelMatrix[1, 3] -= shiftPerFrame;
                if (ballCenter.Y - ballRadius + shapes[1].ModelMatrix[1, 3] <= tableBottomEdgeHeight) ballGoingUp = !ballGoingUp;
            }
            
        }
        private void UpdateCameras()
        {
            // side view camera
            sideViewCamera.CameraTarget.Y = ballCenter.Y + shapes[1].ModelMatrix[1, 3];
            sideViewCamera.createViewMatrix();

            // ball camera
            ballViewCamera.CameraPosition.Y = ballCenter.Y + shapes[1].ModelMatrix[1, 3];
            ballViewCamera.CameraTarget.Y = ballCenter.Y + shapes[1].ModelMatrix[1, 3];
            ballViewCamera.createViewMatrix();
        }
        private void UpdateBallReflector()
        {
            ballLight.LightPosition.Y = ballCenter.Y + shapes[1].ModelMatrix[1, 3];
            ballLight.LightTarget.Y = ballCenter.Y + (angleTrackBar.Value - 50) + shapes[1].ModelMatrix[1, 3];
        }
        private void prepareNewBitmap()
        {
            zBuffElement[,] zBuffer = new zBuffElement[drawingPanel.Width, drawingPanel.Height];
            for (int i = 0; i < drawingPanel.Width; i++)
            {
                for (int j = 0; j < drawingPanel.Height; j++)
                {
                    zBuffer[i, j].pixelColor = Color.Gray;
                    zBuffer[i, j].zValue = zInf;
                }
            }
            bitmapToDraw = new Bitmap(drawingPanel.Width, drawingPanel.Height);
            Graphics g = Graphics.FromImage(bitmapToDraw);
            Pen blackPen = new Pen(Color.Black);
            Pen redPen = new Pen(Color.Red);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush grayBrush = new SolidBrush(Color.Gray);
            g.FillRectangle(grayBrush, 0, 0, bitmapToDraw.Width, bitmapToDraw.Height);
            My4DPoint currentBallCenter = ballCenter.deepCopy();
            currentBallCenter.Y += shapes[1].ModelMatrix[1, 3];
            List<MyReflector> reflectors = new List<MyReflector>();
            reflectors.Add(overheadLight);
            reflectors.Add(sideLight);
            reflectors.Add(ballLight);
            Matrix<double> inverseViewMatrix = currentCamera.ViewMatrix.Inverse();
            Matrix<double> inverseProjMatrix = projectionMatrix.Inverse();
            using (BmpPixelSnoop snoop = new BmpPixelSnoop(bitmapToDraw))
            {
                Parallel.ForEach(shapes, shape =>
                Parallel.ForEach(shape.Triangles, triangle => 
                PolygonFill.fillPolygonWithColor(triangle, snoop, triangle.Color,
                    zBuffer, shape.ModelMatrix, currentCamera, inverseViewMatrix, projectionMatrix, inverseProjMatrix, shape.IsABall, currentBallCenter,
                reflectors, currentShadingType)));
                /*for (int i = 0; i < shapes.Count; i++)
                {
                    for(int j = 0; j < shapes[i].Triangles.Count; j++)
                    {
                        PolygonFill.fillPolygonWithColor(shapes[i].Triangles[j], snoop, shapes[i].Triangles[j].Color,
                    zBuffer, shapes[i].ModelMatrix, currentCamera, inverseViewMatrix, projectionMatrix, inverseProjMatrix, shapes[i].IsABall, currentBallCenter,
                    reflectors, currentShadingType);
                    }
                }*/
                for (int i = 0; i < shapes.Count; i++)
                {
                    for (int j = 0; j < shapes[i].Triangles.Count; j++)
                    {
                        shapes[i].markAsFinished();
                    }
                }
                Parallel.For(0, drawingPanel.Width, (i) =>
                {
                    Parallel.For(0, drawingPanel.Height, (j) =>
                    {
                        if (zBuffer[i, j].zValue != zInf) snoop.SetPixel(i, j, zBuffer[i, j].pixelColor);
                    });
                });
            }
            g = drawingPanel.CreateGraphics();
            g.DrawImageUnscaled(bitmapToDraw, 0, 0);
        }
        private void FOVTrackBar_Scroll(object sender, EventArgs e)
        {
            FOVTrackBarLabel.Text = $"FOV = " + FOVTrackBar.Value;
        }

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(bitmapToDraw, 0, 0);
        }

        private void topDownViewRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (topDownViewRadioButton.Checked) currentCamera = topDownViewCamera;
        }

        private void sideViewRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sideViewRadioButton.Checked) currentCamera = sideViewCamera;
        }

        private void ballViewRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ballViewRadioButton.Checked) currentCamera = ballViewCamera;
        }

        private void constantShadingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            currentShadingType = ShadingType.Constant;
        }

        private void gouardShadingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            currentShadingType = ShadingType.Gouard;
        }

        private void phongShadingRadioBUtton_CheckedChanged(object sender, EventArgs e)
        {
            currentShadingType = ShadingType.Phong;
        }

        private void noShadingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            currentShadingType = ShadingType.None;
        }

        private void angleTrackBar_Scroll(object sender, EventArgs e)
        {
            double tangent = Math.Tan((angleTrackBar.Value - 50) / 50.0 * (Math.PI / 180));
            double angle = Math.Atan(tangent) * (180.0 / Math.PI) * 45;
            angleLabel.Text = "Angle = " + angle.ToString();
        }
    }
}
