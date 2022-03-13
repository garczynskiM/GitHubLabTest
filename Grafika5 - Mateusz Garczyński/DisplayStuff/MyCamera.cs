namespace Grafika5___Mateusz_Garczyński.DisplayStuff
{
    using Grafika5___Mateusz_Garczyński.Shapes;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class MyCamera
    {
        private My4DPoint _cameraPosition; // w = 1
        private My4DPoint _cameraTarget; // w = 1
        private Vector<double> _upVector; // w = 0
        private Matrix<double> _viewMatrix;
        public My4DPoint CameraPosition 
        { 
            get => _cameraPosition;
            set
            {
                _cameraPosition = value;
            }
        }
        public My4DPoint CameraTarget
        {
            get => _cameraTarget;
            set
            {
                _cameraTarget = value;
            }
        }
        public Vector<double> UpVector
        {
            get => _upVector;
            set
            {
                _upVector = value;
            }
        }
        public Matrix<double> ViewMatrix
        {
            get => _viewMatrix;
            set
            {
                _viewMatrix = value;
            }
        }
        public MyCamera(My4DPoint cameraPosition, My4DPoint cameraTarget, Vector<double> upVector)
        {
            _cameraPosition = cameraPosition;
            _cameraPosition.W = 1;
            _cameraTarget = cameraTarget;
            _cameraTarget.W = 1;
            _upVector = upVector;
            createViewMatrix();
        }
        public void createViewMatrix()
        {
            _upVector = _upVector.Normalize(2);
            Vector<double> zAxis = DenseVector.OfArray(new double[] {
            _cameraPosition.X - _cameraTarget.X, _cameraPosition.Y - _cameraTarget.Y, _cameraPosition.Z - _cameraTarget.Z }).Normalize(2);
            Vector<double> xAxis = VectorMethods.CrossProduct3DVector(_upVector, zAxis).Normalize(2);
            Vector<double> yAxis = VectorMethods.CrossProduct3DVector(zAxis, xAxis).Normalize(2);
            Matrix<double> reversedViewMatrix = DenseMatrix.OfArray(new double[,]
            {
                { xAxis[0], yAxis[0], zAxis[0], CameraPosition.X },
                { xAxis[1], yAxis[1], zAxis[1], CameraPosition.Y },
                { xAxis[2], yAxis[2], zAxis[2], CameraPosition.Z },
                { 0, 0, 0, 1 }
            });
            _viewMatrix = reversedViewMatrix.Inverse();
        }
    }
}
