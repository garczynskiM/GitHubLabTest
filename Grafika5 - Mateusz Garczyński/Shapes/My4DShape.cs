using Grafika5___Mateusz_Garczyński.DisplayStuff;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika5___Mateusz_Garczyński.Shapes
{
    class My4DShape
    {
        private List<My4DTriangle> _triangles;
        private Matrix<double> _modelMatrix;
        private bool _isABall;
        public List<My4DTriangle> Triangles
        {
            get => _triangles;
            set
            {
                _triangles = value;
            }
        }
        public Matrix<double> ModelMatrix
        {
            get => _modelMatrix;
            set
            {
                _modelMatrix = value;
            }
        }

        public bool IsABall { get => _isABall; set => _isABall = value; }

        public My4DShape(List<My4DTriangle> newTriangles, Matrix<double> modelMatrix, bool isABall)
        {
            _triangles = new List<My4DTriangle>(newTriangles);
            mergePoints();
            _modelMatrix = modelMatrix;
            _isABall = isABall;
        }
        public void mergePoints()
        {
            My4DPoint pointToMerge;
            for (int i = 0; i < _triangles.Count; i++)
            {
                // 1st triangle vertex
                pointToMerge = _triangles[i].L0.P0;
                if (My4DPoint.comparePositions(pointToMerge, _triangles[i].L1.P0)) _triangles[i].L1.P0 = pointToMerge;
                if (My4DPoint.comparePositions(pointToMerge, _triangles[i].L2.P0)) _triangles[i].L2.P0 = pointToMerge;
                for(int j = i + 1; j < _triangles.Count; j++)
                {
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L0.P0)) _triangles[j].L0.P0 = pointToMerge;
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L1.P0)) _triangles[j].L1.P0 = pointToMerge;
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L2.P0)) _triangles[j].L2.P0 = pointToMerge;
                }

                // 2nd triangle vertex
                pointToMerge = _triangles[i].L1.P0;
                if (My4DPoint.comparePositions(pointToMerge, _triangles[i].L2.P0)) _triangles[i].L2.P0 = pointToMerge;
                for (int j = i + 1; j < _triangles.Count; j++)
                {
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L0.P0)) _triangles[j].L0.P0 = pointToMerge;
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L1.P0)) _triangles[j].L1.P0 = pointToMerge;
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L2.P0)) _triangles[j].L2.P0 = pointToMerge;
                }

                // 3rd triangle vertex
                pointToMerge = _triangles[i].L2.P0;
                for (int j = i + 1; j < _triangles.Count; j++)
                {
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L0.P0)) _triangles[j].L0.P0 = pointToMerge;
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L1.P0)) _triangles[j].L1.P0 = pointToMerge;
                    if (My4DPoint.comparePositions(pointToMerge, _triangles[j].L2.P0)) _triangles[j].L2.P0 = pointToMerge;
                }
            }
        }
        public void drawShape(Graphics g, Pen p, int width, int height)
        {
            foreach (My4DTriangle t in _triangles) t.drawTriangle(g, p, width, height);
        }
        public My4DShape applyMatrixes(Matrix<double> projectionMatrix, MyCamera camera, int width, int height)
        {
            List<My4DTriangle> newTriangles = new List<My4DTriangle>();
            foreach(My4DTriangle t in _triangles) newTriangles.Add(t.applyMatrixes(_modelMatrix, camera.ViewMatrix, projectionMatrix));
            return new My4DShape(newTriangles, _modelMatrix, _isABall);
        }
        public My4DShape deepCopy()
        {
            List<My4DTriangle> newTriangles = new List<My4DTriangle>();
            foreach (My4DTriangle t in _triangles) newTriangles.Add(t.deepCopy());
            return new My4DShape(newTriangles, _modelMatrix.Clone(), _isABall);
        }
        /*public void scaleToScreen(double width, double height)
        {
            foreach (My4DTriangle t in _triangles) t.scaleToScreen(width, height);
        }*/
        public void markAsFinished()
        {
            foreach (My4DTriangle t in _triangles) t.markAsFinished();
        }
    }
}
