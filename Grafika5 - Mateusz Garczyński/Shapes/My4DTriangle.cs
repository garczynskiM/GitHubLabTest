using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika5___Mateusz_Garczyński.Shapes
{
    class My4DTriangle
    {
        private My4DLine _l0;
        private My4DLine _l1;
        private My4DLine _l2;
        private Color _color;
        public My4DLine L0
        {
            get => _l0;
            set
            {
                _l0 = value;
            }
        }
        public My4DLine L1
        {
            get => _l1;
            set
            {
                _l1 = value;
            }
        }
        public My4DLine L2
        {
            get => _l2;
            set
            {
                _l2 = value;
            }
        }
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
            }
        }
        public My4DTriangle(My4DPoint p0, My4DPoint p1, My4DPoint p2, Color color)
        {
            /*if(My4DPoint.comparePositions(p0, p1) || My4DPoint.comparePositions(p0, p2) || My4DPoint.comparePositions(p1, p2))
            {
                string message = "Triangle must be made from 3 different points";
                throw new Exception(message);
            }*/
            _l0 = new My4DLine(p0, p1);
            _l1 = new My4DLine(p1, p2);
            _l2 = new My4DLine(p2, p0);
            _color = color;
        }
        public void drawTriangle(Graphics g, Pen p, int width, int height)
        {
            SolidBrush sb = new SolidBrush(_color);
            g.DrawLine(p, Math.Max(1, Math.Min((float)L0.P0.X, width - 1)),
                Math.Max(1, Math.Min((float)L0.P0.Y, height - 1)),
                Math.Max(1, Math.Min((float)L0.P1.X, width - 1)), Math.Max(1, Math.Min((float)L0.P1.Y, height - 1)));
            g.DrawLine(p, Math.Max(1, Math.Min((float)L1.P0.X, width - 1)), 
                Math.Max(1, Math.Min((float)L1.P0.Y, height - 1)),
                Math.Max(1, Math.Min((float)L1.P1.X, width - 1)), Math.Max(1, Math.Min((float)L1.P1.Y, height - 1)));
            g.DrawLine(p, Math.Max(1, Math.Min((float)L2.P0.X, width - 1)), 
                Math.Max(1, Math.Min((float)L2.P0.Y, height - 1)),
                Math.Max(1, Math.Min((float)L2.P1.X, width - 1)), Math.Max(1, Math.Min((float)L2.P1.Y, height - 1)));
            // kolorowanie ścian
        }
        public void scaleToScreen(double width, double height)
        {
            _l0.P0.scaleToScreen(width, height);
            _l1.P0.scaleToScreen(width, height);
            _l2.P0.scaleToScreen(width, height);
        }
        public My4DTriangle rotateTriangle(Matrix<double> rotationMatrix)
        {
            My4DTriangle result = new My4DTriangle(_l0.P0.rotatePoint(rotationMatrix), _l1.P0.rotatePoint(rotationMatrix), _l2.P0.rotatePoint(rotationMatrix), _color);
            return result;
        }
        public My4DTriangle applyMatrixes(Matrix<double> modelMatrix, Matrix<double> viewMatrix, Matrix<double> projectionMatrix)
        {
            My4DTriangle result = new My4DTriangle(_l0.P0.applyMatrixes(modelMatrix, viewMatrix, projectionMatrix),
                _l1.P0.applyMatrixes(modelMatrix, viewMatrix, projectionMatrix), 
                _l2.P0.applyMatrixes(modelMatrix, viewMatrix, projectionMatrix), _color);
            return result;
        }
        public My4DTriangle applyModelMatrix(Matrix<double> modelMatrix)
        {
            My4DTriangle result = new My4DTriangle(_l0.P0.applyModelMatrix(modelMatrix),
                _l1.P0.applyModelMatrix(modelMatrix),
                _l2.P0.applyModelMatrix(modelMatrix), _color);
            return result;
        }
        public My4DTriangle deepCopy()
        {
            My4DTriangle copy = new My4DTriangle(_l0.P0.deepCopy(), _l1.P0.deepCopy(), _l2.P0.deepCopy(), _color);
            return copy;
        }
        public void markAsFinished()
        {
            _l0.P0.markAsFinished();
            _l1.P0.markAsFinished();
            _l2.P0.markAsFinished();
        }
        public void adjustToSphere(double R, My4DPoint center)
        {
            _l0.P0.adjustToSphere(R, center);
            _l1.P0.adjustToSphere(R, center);
            _l2.P0.adjustToSphere(R, center);
        }
    }
}
