namespace Grafika5___Mateusz_Garczyński.FillPolygon
{
    using Grafika5___Mateusz_Garczyński.DisplayStuff;
    using Grafika5___Mateusz_Garczyński.Shapes;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    class PolygonFill
    {
        const double material_ks = 1;
        const double material_kd = 0.5;
        const double material_ka = 0.2;
        const double material_alpha = 4;
        const double light_ambient = 0.4;
        static Color lightColor = Color.White;
        public static void fillPolygonWithColor(My4DTriangle triangle, BmpPixelSnoop snoop, Color color, zBuffElement[,] zBuffer,
            Matrix<double> modelMatrix, MyCamera camera, Matrix<double> inverseViewMatrix, Matrix<double> projectionMatrix, Matrix<double> inverseProjMatrix,
            bool isFromBall, My4DPoint ballCenter, List<MyReflector> lightSources, ShadingType currentShadingType)
        {
            My4DVector normalVector = calculateTriangleNormalVector(triangle);
            My4DTriangle modelTriangle = triangle.applyModelMatrix(modelMatrix);
            My4DTriangle changedTriangle = triangle.applyMatrixes(modelMatrix, camera.ViewMatrix, projectionMatrix);
            double oldW = triangle.L0.P0.OldW;
            List<My4DLine> polygon = new List<My4DLine>();
            polygon.Add(changedTriangle.L0.deepCopy());
            polygon.Add(changedTriangle.L1.deepCopy());
            polygon.Add(changedTriangle.L2.deepCopy());
            for(int i = 0; i < polygon.Count; i++)
            {
                if (polygon[i].P0.X < -1 || polygon[i].P0.X > 1 ||
                    polygon[i].P0.Y < -1 || polygon[i].P0.Y > 1 ||
                    polygon[i].P0.Z < -1 || polygon[i].P0.Z > 1) return; // Nic nie malujemy, bo trójkąt jest poza kamerą
                else
                {
                    polygon[i].P0.scaleToScreen(snoop.Width, snoop.Height);
                    polygon[i].P1.scaleToScreen(snoop.Width, snoop.Height);
                }
            }
            changedTriangle.L0 = polygon[0].deepCopy();
            changedTriangle.L1 = polygon[1].deepCopy();
            changedTriangle.L2 = polygon[2].deepCopy();
            Color constantColor = Color.Black;
            List<(My4DPoint point, Color pointColor)> gouardColors = new List<(My4DPoint, Color)>();
            My4DPoint triangleMiddle = new My4DPoint((modelTriangle.L0.P0.X + modelTriangle.L1.P0.X + modelTriangle.L2.P0.X)/3,
                (modelTriangle.L0.P0.Y + modelTriangle.L1.P0.Y + modelTriangle.L2.P0.Y) / 3,
                (modelTriangle.L0.P0.Z + modelTriangle.L1.P0.Z + modelTriangle.L2.P0.Z) / 3, 1);
            List<MyReflector> changedLights = new List<MyReflector>();
            if (currentShadingType == ShadingType.Constant) constantColor = 
                    calculatePhongReflectionColor(lightSources, normalVector, camera, triangleMiddle, color);
            else if (currentShadingType == ShadingType.Gouard)
            {
                if(isFromBall) // wektor normalny z kuli
                {
                    gouardColors.Add((changedTriangle.L0.P0, calculatePhongReflectionColor(lightSources, 
                        calculateSphereNormalVector(modelTriangle.L0.P0, ballCenter), camera, modelTriangle.L0.P0, color)));
                    gouardColors.Add((changedTriangle.L1.P0, calculatePhongReflectionColor(lightSources,
                        calculateSphereNormalVector(modelTriangle.L1.P0, ballCenter), camera, modelTriangle.L1.P0, color)));
                    gouardColors.Add((changedTriangle.L2.P0, calculatePhongReflectionColor(lightSources,
                        calculateSphereNormalVector(modelTriangle.L2.P0, ballCenter), camera, modelTriangle.L2.P0, color)));
                }
                else // wektor normalny z płaszczyzny
                {
                    gouardColors.Add((changedTriangle.L0.P0, calculatePhongReflectionColor(lightSources, normalVector,
                           camera, modelTriangle.L0.P0, color)));
                    gouardColors.Add((changedTriangle.L1.P0, calculatePhongReflectionColor(lightSources, normalVector,
                           camera, modelTriangle.L1.P0, color)));
                    gouardColors.Add((changedTriangle.L2.P0, calculatePhongReflectionColor(lightSources, normalVector,
                           camera, modelTriangle.L2.P0, color)));
                }
            }
            List<AET> AETList = new List<AET>();
            int currentCrossingNode; // Który wierzchołek przetniemy następny
            int[] ind = new int[polygon.Count]; // tablica indeksów wierzchołków posortowanych rosnąco względem Y
            for (int i = 0; i < polygon.Count;) // pozbywamy się poziomych linii
            {
                if (polygon[i].P0.Y == polygon[i].P1.Y) polygon.RemoveAt(i);
                else i++;
            }
            if (polygon.Count == 0) return;
            organiseEdges(ind, polygon);
            Array.Sort(ind, (int ind1, int ind2) => // posortuj względem rosnącego yStart
            {
                My4DLine l1 = polygon[ind1];
                My4DLine l2 = polygon[ind2];
                if (l1.P0.Y < l2.P0.Y) return -1;
                else if (l1.P0.Y > l2.P0.Y) return 1;
                else
                {
                    if (l1.P1.Y > l2.P1.Y) return 1;
                    else return -1;
                }
            });
            int yMin = Math.Max((int)polygon[ind[0]].P0.Y, 0);
            int yMax = Math.Min((int)polygon[ind[polygon.Count - 1]].P1.Y, snoop.Height - 1);
            currentCrossingNode = 0;
            // Rozpoczynamy jeżdżenie scan-linią
            for (int y = yMin; y <= yMax; y++)
            {
                // Sprawdzamy, czy trzeba wrzucić coś do listy AET
                currentCrossingNode = addToAET(currentCrossingNode, y, polygon, ind, AETList);

                // Malujemy na podstawie listy AET
                if (AETList.Count == 0) return; // Nie powinno nigdy się wywołać, to swego rodzaju zabezpieczenie
                if (y != yMin) polygonColorLine(AETList, snoop, polygon, y, color, zBuffer, changedTriangle, camera,
                    inverseViewMatrix, inverseProjMatrix, normalVector, isFromBall, ballCenter, lightSources, 
                    currentShadingType, constantColor, gouardColors, oldW);
                // Warunek jest tu dlatego, żeby nie wchodzić na trójkąty leżące wyżej

                // Aktualizujemy listę AET
                for (int i = 0; i < AETList.Count;)
                {
                    if (!AETList[i].stepUp(y + 1)) AETList.RemoveAt(i);
                    else i++;
                }
            }
        }
        private static void polygonColorLine(List<AET> AETList, BmpPixelSnoop snoop, List<My4DLine> polygon, int y,
            Color color, zBuffElement[,] zBuffer, My4DTriangle changedTriangle, MyCamera camera,
            Matrix<double> inverseViewMatrix, Matrix<double> inverseProjMatrix, My4DVector normalVector, bool isFromBall, My4DPoint ballCenter,
            List<MyReflector> lightSources, ShadingType currentShadingType, Color constantColor, 
            List<(My4DPoint point, Color pointColor)> gouardColors, double oldW)
        {
            int crossedEdges = 0; // ile krawędzi przecięliśmy - służy do sprawdzania czy jesteśmy wewnątrz wielokąta, czy na zewnątrz
            int currentEdge = 0; // ile krawędzi przejrzeliśmy - służy do chodzenia po AETList
            int xMin = Math.Max((int)AETList[0].x, 0);
            double xMax = Math.Min(AETList[AETList.Count - 1].x, snoop.Width - 1);
            for (int x = xMin; x <= xMax; x++)
            {
                while (currentEdge < AETList.Count && ((crossedEdges % 2 == 1 && (int)AETList[currentEdge].x == x - 1)
                    || (crossedEdges % 2 == 0 && (int)AETList[currentEdge].x == x)))
                // Dopóki znajdujemy się na jakiejś krawędzi
                {
                    if (AETList[currentEdge].yMax == y) currentEdge += checkSpecialCase(AETList, currentEdge, polygon);
                    crossedEdges++;
                    currentEdge++;
                }
                if (crossedEdges % 2 == 1) // Jeśli jesteśmy w figurze
                {
                    Color newPixelColor = color;
                    double z = calculateZ(changedTriangle.L0.P0, changedTriangle.L1.P0, changedTriangle.L2.P0, x, y);
                    //double z1test = calculateZ(new My4DPoint(0, 100, 0, 1), new My4DPoint(100, 0, 0, 1), new My4DPoint(0, 0, 100, 1), 25, 25);
                    if (z < zBuffer[x, y].zValue)
                    {
                        //if (isFromBall) normalVector = calculateSphereNormalVector(x, y, z, ballCenter);
                        //setPixelSafely(snoop, x, y, newPixelColor);
                        zBuffer[x, y].zValue = z;
                        if (currentShadingType == ShadingType.Constant) newPixelColor = constantColor;
                        else if(currentShadingType == ShadingType.Gouard)
                        {
                            newPixelColor = calculateGouardColor(gouardColors, x, y);
                        }
                        else if(currentShadingType == ShadingType.Phong)
                        {
                            My4DPoint point = new My4DPoint(x, y, z, 1);
                            point.descaleFromScreen(snoop.Width, snoop.Height);
                            My4DPoint pointInOldCoords = point.reverseApplyMatrixes(inverseViewMatrix, inverseProjMatrix, oldW);
                            if(isFromBall) newPixelColor = calculatePhongReflectionColor(lightSources, calculateSphereNormalVector(pointInOldCoords, ballCenter),
                                camera, pointInOldCoords, color);
                            else newPixelColor = calculatePhongReflectionColor(lightSources, normalVector,
                                camera, pointInOldCoords, color);

                        }
                        zBuffer[x, y].pixelColor = newPixelColor;
                    }
                }
            }
        }
        private static Color calculatePhongReflectionColor(List<MyReflector> lights, My4DVector normalVector, MyCamera currentCamera, 
            My4DPoint checkedPoint, Color baseColor)
        {
            My4DVector pixelToCamera = new My4DVector(currentCamera.CameraPosition.X - checkedPoint.X,
                currentCamera.CameraPosition.Y - checkedPoint.Y, currentCamera.CameraPosition.Z - checkedPoint.Z);
            pixelToCamera.changeToVersor();
            My4DVector newColor = new My4DVector(material_ka * light_ambient * (baseColor.R / 255.0), 
                material_ka * light_ambient * (baseColor.G / 255.0),
                material_ka * light_ambient * (baseColor.B / 255.0));
            foreach (MyReflector reflector in lights)
            {
                if(reflector.LightTarget == null) // źródło światła
                {
                    My4DVector lightVersor = new My4DVector(reflector.LightPosition.X - checkedPoint.X,
                    reflector.LightPosition.Y - checkedPoint.Y, reflector.LightPosition.Z - checkedPoint.Z);
                    lightVersor.changeToVersor();
                    double lightNormalDotProduct = My4DVector.dotProduct(lightVersor, normalVector);
                    My4DVector reflectionVector = new My4DVector(
                        2 * lightNormalDotProduct * normalVector.X - lightVersor.X,
                        2 * lightNormalDotProduct * normalVector.Y - lightVersor.Y,
                        2 * lightNormalDotProduct * normalVector.Z - lightVersor.Z);
                    reflectionVector.changeToVersor();
                    double cosNL = Math.Max(lightNormalDotProduct, 0);
                    double cosVR = Math.Pow(Math.Max(My4DVector.dotProduct(pixelToCamera, reflectionVector), 0), material_alpha);
                    double lightColorR = (double)lightColor.R / 255;
                    double lightColorG = (double)lightColor.G / 255;
                    double lightColorB = (double)lightColor.B / 255;
                    double polygonColorR = (double)baseColor.R / 255;
                    double polygonColorG = (double)baseColor.G / 255;
                    double polygonColorB = (double)baseColor.B / 255;
                    newColor.X += material_kd * lightColorR * polygonColorR * cosNL + material_ks * lightColorR * polygonColorR * cosVR;
                    newColor.Y += material_kd * lightColorG * polygonColorG * cosNL + material_ks * lightColorG * polygonColorG * cosVR;
                    newColor.Z += material_kd * lightColorB * polygonColorB * cosNL + material_ks * lightColorB * polygonColorB * cosVR;
                }
                else // reflektor
                {
                    My4DVector I_R = new My4DVector(1, 1, 1);
                    My4DVector lightVersor = new My4DVector(reflector.LightPosition.X - checkedPoint.X,
                    reflector.LightPosition.Y - checkedPoint.Y, reflector.LightPosition.Z - checkedPoint.Z);
                    lightVersor.changeToVersor();
                    My4DVector reverseLightVersor = new My4DVector(checkedPoint.X - reflector.LightPosition.X,
                    checkedPoint.Y - reflector.LightPosition.Y, checkedPoint.Z - reflector.LightPosition.Z);
                    reverseLightVersor.changeToVersor();
                    My4DVector reflectorToPosition = new My4DVector(reflector.LightTarget.X - reflector.LightPosition.X,
                    reflector.LightTarget.Y - reflector.LightPosition.Y, reflector.LightTarget.Z - reflector.LightPosition.Z);
                    reflectorToPosition.changeToVersor();

                    var test = Math.Pow(My4DVector.dotProduct(reverseLightVersor, reflectorToPosition), material_alpha);
                    I_R.X *= test;
                    I_R.Y *= test;
                    I_R.Z *= test;
                    var dotResult = My4DVector.dotProduct(normalVector, lightVersor);
                    My4DVector R_R = new My4DVector(
                        2 * dotResult * normalVector.X - lightVersor.X,
                        2 * dotResult * normalVector.Y - lightVersor.Y,
                        2 * dotResult * normalVector.Z - lightVersor.Z
                        );
                    newColor.X += (material_kd * I_R.X * (baseColor.R / 255.0) * My4DVector.dotProduct(normalVector, lightVersor) +
                                           material_ks * I_R.X * (baseColor.R / 255.0) * Math.Pow(My4DVector.dotProduct(pixelToCamera, R_R), material_alpha));
                    newColor.Y += (material_kd * I_R.Y * (baseColor.G / 255.0) * My4DVector.dotProduct(normalVector, lightVersor) +
                                           material_ks * I_R.Y * (baseColor.G / 255.0) * Math.Pow(My4DVector.dotProduct(pixelToCamera, R_R), material_alpha));
                    newColor.Z += (material_kd * I_R.Z * (baseColor.B / 255.0) * My4DVector.dotProduct(normalVector, lightVersor) +
                                           material_ks * I_R.Z * (baseColor.B / 255.0) * Math.Pow(My4DVector.dotProduct(pixelToCamera, R_R), material_alpha));
                }
            }
            newColor.X = Math.Max(Math.Min(newColor.X * 255, 255), 0);
            newColor.Y = Math.Max(Math.Min(newColor.Y * 255, 255), 0);
            newColor.Z = Math.Max(Math.Min(newColor.Z * 255, 255), 0);
            Color result = Color.FromArgb((int)newColor.X, (int)newColor.Y, (int)newColor.Z);
            return result;
        }
        private static Color calculateGouardColor(List<(My4DPoint vertex, Color colorInVertex)> gouardColors, double x, double y)
        {
            double weight0 = ((gouardColors[1].vertex.Y - gouardColors[2].vertex.Y) * (x - gouardColors[2].vertex.X)
                + (gouardColors[2].vertex.X - gouardColors[1].vertex.X) * (y - gouardColors[2].vertex.Y)) /
                ((gouardColors[1].vertex.Y - gouardColors[2].vertex.Y) * (gouardColors[0].vertex.X - gouardColors[2].vertex.X)
                + (gouardColors[2].vertex.X - gouardColors[1].vertex.X) * (gouardColors[0].vertex.Y - gouardColors[2].vertex.Y));
            weight0 = Math.Max(weight0, 0);
            double weight1 = ((gouardColors[2].vertex.Y - gouardColors[0].vertex.Y) * (x - gouardColors[2].vertex.X)
                + (gouardColors[0].vertex.X - gouardColors[2].vertex.X) * (y - gouardColors[2].vertex.Y)) /
                ((gouardColors[1].vertex.Y - gouardColors[2].vertex.Y) * (gouardColors[0].vertex.X - gouardColors[2].vertex.X)
                + (gouardColors[2].vertex.X - gouardColors[1].vertex.X) * (gouardColors[0].vertex.Y - gouardColors[2].vertex.Y));
            weight1 = Math.Max(weight1, 0);
            double weight2 = Math.Max(1 - weight0 - weight1, 0);
            int red = (int)(gouardColors[0].colorInVertex.R * weight0) +
                (int)(gouardColors[1].colorInVertex.R * weight1) +
                (int)(gouardColors[2].colorInVertex.R * weight2);
            red = Math.Max(0, Math.Min(red, 255));
            int green = (int)(gouardColors[0].colorInVertex.G * weight0) +
                (int)(gouardColors[1].colorInVertex.G * weight1) +
                (int)(gouardColors[2].colorInVertex.G * weight2);
            green = Math.Max(0, Math.Min(green, 255));
            int blue = (int)(gouardColors[0].colorInVertex.B * weight0) +
                (int)(gouardColors[1].colorInVertex.B * weight1) +
                (int)(gouardColors[2].colorInVertex.B * weight2);
            blue = Math.Max(0, Math.Min(blue, 255));
            Color result = Color.FromArgb(red, green, blue);
            return result;
        }
        private static My4DVector calculateTriangleNormalVector(My4DTriangle triangle)
        {
            My4DVector AB = new My4DVector(triangle.L0.P0, triangle.L1.P0);
            My4DVector AC = new My4DVector(triangle.L0.P0, triangle.L2.P0);
            My4DVector result = new My4DVector(AB.Y * AC.Z - AB.Z * AC.Y, AB.Z * AC.X - AB.X * AC.Z, AB.X * AC.Y - AB.Y * AC.X);
            result.changeToVersor();
            if (result.Z < 0)
            {
                result.X *= -1;
                result.Y *= -1;
                result.Z *= -1;
            }
            return result;
        }
        private static My4DVector calculateSphereNormalVector(My4DPoint pixel, My4DPoint ballCenter)
        {
            My4DVector result = new My4DVector(pixel.X - ballCenter.X, pixel.Y - ballCenter.Y, pixel.Z - ballCenter.Z);
            result.changeToVersor();
            return result;
        }
        private static My4DVector calculateSphereNormalVector(double x, double y, double z, My4DPoint ballCenter)
        {
            My4DVector result = new My4DVector(x - ballCenter.X, y - ballCenter.Y, z - ballCenter.Z);
            result.changeToVersor();
            return result;
        }
        private static int checkSpecialCase(List<AET> AETList, int currentEdge, List<My4DLine> polygon)
        // Specjalny przypadek - przecinamy wierzchołek który jednocześnie jest końcem jednej krawędzi i początkiem drugiej
        {
            int tempCurrEdge = AETList[currentEdge].edgeNumber;
            int nextEdge = AETList[currentEdge].edgeNumber + 1;
            int prevEdge = AETList[currentEdge].edgeNumber - 1;
            if (nextEdge >= polygon.Count) nextEdge = 0;
            if (prevEdge < 0) prevEdge = polygon.Count - 1;
            if (My4DPoint.comparePositions(polygon[tempCurrEdge].P0, polygon[nextEdge].P1) ||
                My4DPoint.comparePositions(polygon[tempCurrEdge].P1, polygon[nextEdge].P0) ||
                My4DPoint.comparePositions(polygon[tempCurrEdge].P0, polygon[prevEdge].P1) ||
                My4DPoint.comparePositions(polygon[tempCurrEdge].P1,polygon[prevEdge].P0))
                return 1;
            return 0;
        }
        private static double calculateZ(My4DPoint p1, My4DPoint p2, My4DPoint p3, float x, float y)
        {
            double p21x = p2.X - p1.X;
            double p31x = p3.X - p1.X;
            double p21y = p2.Y - p1.Y;
            double p31y = p3.Y - p1.Y;
            double p21z = p2.Z - p1.Z;
            double p31z = p3.Z - p1.Z;
            double z1 = ((p21x * p31z - p31x * p21z) /
                        (p21x * p31y - p31x * p21y)) * (y - p1.Y);
            double z2 = ((p21y * p31z - p31y * p21z) /
                        (p21x * p31y - p31x * p21y)) * (x - p1.X);
            return p1.Z + z1 - z2;
        }
        private static int addToAET(int currentCrossingNode, int y, List<My4DLine> polygon, int[] ind, List<AET> AETList)
        {
            bool addedSomething = false;
            while (currentCrossingNode < polygon.Count && (int)polygon[ind[currentCrossingNode]].P0.Y == y)
            // Jeśli jakiś wierzchołek początkowy leży na scan-linii
            {
                My4DLine currentLine = polygon[ind[currentCrossingNode]];
                double dividedM;
                if ((int)currentLine.P0.Y == (int)currentLine.P1.Y) dividedM = 0;
                else dividedM = (currentLine.P1.X - currentLine.P0.X) / ((int)currentLine.P1.Y - (int)currentLine.P0.Y);
                AETList.Add(new AET((int)currentLine.P1.Y, currentLine.P0.X, dividedM, ind[currentCrossingNode]));
                currentCrossingNode++;
                addedSomething = true;
            }
            if (addedSomething) AETList.Sort(compareAET); // Jeśli dodaliśmy coś do listy, poprawiamy ją
            return currentCrossingNode;
        }
        private static void organiseEdges(int[] array, List<My4DLine> polygon)
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                array[i] = i;
                if (polygon[i].P0.Y > polygon[i].P1.Y) // Dzięki tej operacji dla każdej linii jej P0 będzie zawsze niewyżej niż P1
                {
                    My4DPoint temp;
                    temp = polygon[i].P0;
                    polygon[i].P0 = polygon[i].P1;
                    polygon[i].P1 = temp;
                }
            }
        }
        private static int compareAET(AET a1, AET a2)
        {
            if ((int)a1.x < (int)a2.x) return -1;
            else if ((int)a1.x > (int)a2.x) return 1;
            else
            {
                if (a1.divideM < a2.divideM) return -1;
                else if (a1.divideM > a2.divideM) return 1;
                else
                {
                    if (a1.yMax < a2.yMax) return -1;
                    else return 1;
                }
            }
        }
    }
}
