using Grafika5___Mateusz_Garczyński.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika5___Mateusz_Garczyński.DisplayStuff
{
    class MeshCreatorHelper
    {
        /// <summary>
        /// Creates two triangles - p0p1p3 and p1p2p3
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static List<My4DTriangle> createFromQuad(My4DPoint p0, My4DPoint p1, My4DPoint p2, My4DPoint p3, Color triangleColor)
        {
            List<My4DTriangle> result = new List<My4DTriangle>();
            result.Add(new My4DTriangle(p0, p1, p3, triangleColor));
            result.Add(new My4DTriangle(p1, p2, p3, triangleColor));
            return result;
        }
    }
}
