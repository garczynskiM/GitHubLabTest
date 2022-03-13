using Grafika5___Mateusz_Garczyński.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika5___Mateusz_Garczyński.DisplayStuff
{
    class MyReflector
    {
        private My4DPoint _lightPosition;
        private My4DPoint _lightTarget;

        public My4DPoint LightPosition { get => _lightPosition; set => _lightPosition = value; }
        public My4DPoint LightTarget { get => _lightTarget; set => _lightTarget = value; }
        public MyReflector(My4DPoint lightPosition, My4DPoint lightTarget)
        {
            _lightPosition = lightPosition;
            _lightTarget = lightTarget;
        }
        
    }
}
