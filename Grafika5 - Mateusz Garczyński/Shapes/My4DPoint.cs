using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika5___Mateusz_Garczyński.Shapes
{
    class My4DPoint
    {
        private double _x;
        private double _y;
        private double _z;
        private double _w;
        private My4DPoint _changedPoint;
        private bool _askedToApplyMatrixes;
        private bool _askedToScale;
        private double _oldW;
        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }
        public double Z { get => _z; set => _z = value; }
        public double W { get => _w; set => _w = value; }
        public bool AskedToApplyMatrixes { get => _askedToApplyMatrixes; }
        public bool AskedToScale { get => _askedToScale; }
        public double OldW { get => _oldW; set => _oldW = value; }

        public My4DPoint(double x, double y, double z, double w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
            _askedToApplyMatrixes = _askedToScale = false;
            _changedPoint = new My4DPoint(_x, _y, _z, _w, false);
        }
        private My4DPoint(double x, double y, double z, double w, bool createAnotherChangedPoint)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
            _askedToApplyMatrixes = _askedToScale = false;
            _changedPoint = null;
        }
        public My4DPoint deepCopy()
        {
            return new My4DPoint(_x, _y, _z, _w);
        }
        public My4DPoint applyMatrixes(Matrix<double> modelMatrix, Matrix<double> viewMatrix, Matrix<double> projectionMatrix)
        {
            if(!_askedToApplyMatrixes)
            {
                _askedToApplyMatrixes = true;
                Vector<double> pointVector = DenseVector.OfArray(new double[] { _x, _y, _z, _w });
                pointVector = modelMatrix.Multiply(pointVector);
                pointVector = viewMatrix.Multiply(pointVector);
                pointVector = projectionMatrix.Multiply(pointVector);
                _oldW = pointVector[3];
                _changedPoint.X = pointVector[0] / pointVector[3];
                _changedPoint.Y = pointVector[1] / pointVector[3];
                _changedPoint.Z = pointVector[2] / pointVector[3];
                _changedPoint.W = pointVector[3] / pointVector[3];
            }
            return _changedPoint;
        }
        public My4DPoint reverseApplyMatrixes(Matrix<double> reverseViewMatrix, Matrix<double> reverseProjectionMatrix, double oldW)
        {
            Vector<double> pointVector = DenseVector.OfArray(new double[] { _x, _y, _z, _w });
            pointVector[0] *= oldW;
            pointVector[1] *= oldW;
            pointVector[2] *= oldW;
            pointVector[3] *= oldW;
            pointVector = reverseProjectionMatrix.Multiply(pointVector);
            pointVector = reverseViewMatrix.Multiply(pointVector);
            pointVector[0] = pointVector[0] / pointVector[3];
            pointVector[1] = pointVector[1] / pointVector[3];
            pointVector[2] = pointVector[2] / pointVector[3];
            pointVector[3] = pointVector[3] / pointVector[3];
            return new My4DPoint(pointVector[0], pointVector[1], pointVector[2], pointVector[3]);
        }
        public My4DPoint applyModelMatrix(Matrix<double> modelMatrix)
        {
            Vector<double> pointVector = DenseVector.OfArray(new double[] { _x, _y, _z, _w });
            pointVector = modelMatrix.Multiply(pointVector);
            return new My4DPoint(pointVector[0], pointVector[1], pointVector[2], pointVector[3]);
        }
        public My4DPoint rotatePoint(Matrix<double> rotationMatrix)
        {
            My4DPoint result;
            Vector<double> pointVector = DenseVector.OfArray(new double[] { _x, _y, _z, _w });
            pointVector = rotationMatrix.Multiply(pointVector);
            //cutVectorToScreen(pointVector);
            result = new My4DPoint(pointVector[0], pointVector[1], pointVector[2], pointVector[3]);
            return result;
        }
        public void markAsFinished()
        {
            _askedToApplyMatrixes = _askedToScale = false;
        }
        public void scaleToScreen(double width, double height)
        {
            if(!_askedToScale)
            {
                _askedToScale = true;
                _x = width / 2 + (width / 2 * _x);
                _y = height / 2 + (height / 2 * _y);
            }
        }
        public void descaleFromScreen(double width, double height)
        {
            _x = (2 * _x - width) / width;
            _y = (2 * _y - height) / height;
        }
        public static void drawLine(My4DPoint p1, My4DPoint p2, Graphics g, Pen p)
        {
            g.DrawLine(p, (float)p1.X, (float)p1.Y, (float)p2.X, (float)p2.Y);
        }
        public static My4DPoint getMiddleOfLine(My4DPoint p0, My4DPoint p1)
        {
            if(p0.W != 1 || p1.W != 1)
            {
                string message = "Points must have w equal to 1";
                throw new Exception(message);
            }
            return new My4DPoint(p0.X + (p1.X - p0.X) / 2, p0.Y + (p1.Y - p0.Y) / 2, p0.Z + (p1.Z - p0.Z) / 2, p0.W);
        }
        public static bool comparePositions(My4DPoint p0, My4DPoint p1)
        {
            if (p0.W == 0 || p1.W == 0)
            {
                string message = "Points must have w different than 0";
                throw new Exception(message);
            }
            double epsilon = 0.001;
            if (Math.Abs(p0.X - p1.X) < epsilon && Math.Abs(p0.Y - p1.Y) < epsilon && Math.Abs(p0.Z - p1.Z) < epsilon) return true;
            return false;
        }
        public static double distanceBetweenPoints(My4DPoint p0, My4DPoint p1)
        {
            return Math.Sqrt(Math.Pow(p0.X - p1.X, 2) + Math.Pow(p0.Y - p1.Y, 2) + Math.Pow(p0.Z - p1.Z, 2));
        }
        public static double distanceBetweenPointAndCoords(My4DPoint p0, double x, double y, double z)
        {
            return Math.Sqrt(Math.Pow(p0.X - x, 2) + Math.Pow(p0.Y - y, 2) + Math.Pow(p0.Z - z, 2));
        }
        public void adjustToSphere(double R, My4DPoint center)
        {
            double dist = Math.Sqrt((center.X - _x) * (center.X - _x) + (center.Y - _y) * (center.Y - _y) + (center.Z - _z)
                * (center.Z - _z));
            _x -= center.X;
            _y -= center.Y;
            _z -= center.Z;

            _x = _x * R / dist;
            _y = _y * R / dist;
            _z = _z * R / dist;

            _x += center.X;
            _y += center.Y;
            _z += center.Z;
            return;
        }
    }
}