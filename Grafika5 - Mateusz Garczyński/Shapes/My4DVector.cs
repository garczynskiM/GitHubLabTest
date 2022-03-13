using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika5___Mateusz_Garczyński.Shapes
{
    class My4DVector
    {
        private double _x;
        private double _y;
        private double _z;
        private double _w;
        public double X
        {
            get => _x;
            set
            {
                _x = value;
            }
        }
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
            }
        }
        public double Z
        {
            get => _z;
            set
            {
                _z = value;
            }
        }

        public double W { get => _w; set => _w = value; }

        public My4DVector(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = 0;
        }
        public My4DVector(My4DPoint start, My4DPoint end)
        {
            _x = end.X - start.X;
            _y = end.Y - start.Y;
            _z = end.Z - start.Z;
            _w = 0;
        }
        public void changeToVersor()
        {
            double divider = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
            X /= divider;
            Y /= divider;
            Z /= divider;
        }
        public My4DVector deepCopy()
        {
            return new My4DVector(X, Y, Z);
        }
        public static My4DVector crossProduct(My4DVector left, My4DVector right)
        {
            //http://james-ramsden.com/calculate-the-cross-product-c-code/
            double x, y, z;
            x = left.Y * right.Z - right.Y * left.Z;
            y = (left.X * right.Z - right.X * left.Z) * -1;
            z = left.X * right.Y - right.X * left.Y;
            return new My4DVector(x, y, z);
        }
        public static double dotProduct(My4DVector left, My4DVector right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }
        public static My4DVector operator *(My4DVector left, double right)
        {
            left.X *= right;
            left.Y *= right;
            left.Z *= right;
            return left;
        }
    }
}
