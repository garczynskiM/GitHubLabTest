namespace Grafika5___Mateusz_Garczyński.FillPolygon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class AET
    {
        public int yMax;
        public double x;
        public double divideM;
        public int edgeNumber;
        public AET(int yMax, double x, double divideM, int edgeNumber)
        {
            this.yMax = yMax;
            this.x = x;
            this.divideM = divideM;
            this.edgeNumber = edgeNumber;
        }
        public bool stepUp(int newY)
        {
            if (newY > yMax) return false;
            x += divideM;
            return true;
        }
    }
}
