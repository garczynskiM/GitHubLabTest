using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika5___Mateusz_Garczyński.Shapes
{
    class My4DLine
    {
        private My4DPoint _p0;
        private My4DPoint _p1;
        public My4DPoint P0
        {
            get => _p0;
            set
            {
                _p0 = value;
            }
        }
        public My4DPoint P1
        {
            get => _p1;
            set
            {
                _p1 = value;
            }
        }
        public My4DLine(My4DPoint p0, My4DPoint p1)
        {
            /*if (My4DPoint.comparePositions(p0, p1))
            {
                string message = "Line must be made from 2 different points";
                throw new Exception(message);
            }*/
            _p0 = p0;
            _p1 = p1;
        }
        public My4DLine deepCopy()
        {
            return new My4DLine(_p0.deepCopy(), _p1.deepCopy());
        }
    }
}
