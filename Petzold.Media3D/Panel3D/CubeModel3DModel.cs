using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Panel3D
{
    public class CubeModel3DModel : IModelVisual3D
    {
        static private ModelVisual3D CreateCubeModel3DModel(double X, double Y, double Z,
      double sizeX, double sizeY, double sizeZ)
        {

            Model3DGroup cube = CreateCubeModel3DGroup(X, Y, Z, sizeX, sizeY, sizeZ);
            ModelVisual3D model = new ModelVisual3D();
            model.Content = cube;
            return model;
        }
        static private Model3DGroup CreateCubeModel3DGroup
              (double X, double Y, double Z,
              double sizeX, double sizeY, double sizeZ)
        {
            Model3DGroup cube = new Model3DGroup();
            Point3D p0 = new Point3D(X - sizeX / 2, Y - sizeY / 2, Z - sizeZ / 2);
            Point3D p1 = new Point3D(X + sizeX / 2, Y - sizeY / 2, Z - sizeZ / 2);
            Point3D p2 = new Point3D(X + sizeX / 2, Y - sizeY / 2, Z + sizeZ / 2);
            Point3D p3 = new Point3D(X - sizeX / 2, Y - sizeY / 2, Z + sizeZ / 2);
            Point3D p4 = new Point3D(X - sizeX / 2, Y + sizeY / 2, Z - sizeZ / 2);
            Point3D p5 = new Point3D(X + sizeX / 2, Y + sizeY / 2, Z - sizeZ / 2);
            Point3D p6 = new Point3D(X + sizeX / 2, Y + sizeY / 2, Z + sizeZ / 2);
            Point3D p7 = new Point3D(X - sizeX / 2, Y + sizeY / 2, Z + sizeZ / 2);

            //front side triangles
            cube.Children.Add(CreateTriangleModel(p3, p2, p6));
            cube.Children.Add(CreateTriangleModel(p3, p6, p7));
            //right side triangles
            cube.Children.Add(CreateTriangleModel(p2, p1, p5));
            cube.Children.Add(CreateTriangleModel(p2, p5, p6));
            //back side triangles
            cube.Children.Add(CreateTriangleModel(p1, p0, p4));
            cube.Children.Add(CreateTriangleModel(p1, p4, p5));
            //left side triangles
            cube.Children.Add(CreateTriangleModel(p0, p3, p7));
            cube.Children.Add(CreateTriangleModel(p0, p7, p4));
            //top side triangles
            cube.Children.Add(CreateTriangleModel(p7, p6, p5));
            cube.Children.Add(CreateTriangleModel(p7, p5, p4));
            //bottom side triangles
            cube.Children.Add(CreateTriangleModel(p2, p3, p0));
            cube.Children.Add(CreateTriangleModel(p2, p0, p1));
            return cube;
        }
        static private Model3DGroup CreateTriangleModel(Point3D p0, Point3D p1, Point3D p2)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            Vector3D normal = CalculateNormal(p0, p1, p2);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            Material material = new DiffuseMaterial(
                new SolidColorBrush(Colors.DarkKhaki));
            GeometryModel3D model = new GeometryModel3D(
                mesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }
        static private Vector3D CalculateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(
                p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(
                p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }
        public bool Centric { get; set; }
        public CubeModel3DModel()
        {
            X = Y = Z = 0;
            SizeX = SizeY = SizeZ = 1;
            Centric = true;
        }
        public CubeModel3DModel(double X, double Y, double Z,
      double sizeX, double sizeY, double sizeZ, bool centric)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            this.SizeZ = sizeZ;
            this.Centric = centric;
        }
        #region IModelVisual3D Members
        ModelVisual3D IModelVisual3D.GetModelVisual3D(ModelVisual3DFilter FilterSettings)
        {
            if (FilterSettings.Triangles)
            {
                if (Centric)
                {
                    return CreateCubeModel3DModel(X, Y, Z, SizeX, SizeY, SizeZ);
                }
                else
                {
                    return CreateCubeModel3DModel(X + SizeX / 2, Y + SizeY / 2, Z + SizeZ / 2, SizeX, SizeY, SizeZ);
                }
            }
            else
                return new ModelVisual3D();
        }
        void IModelVisual3D.UpdateViewToLookDirection(Vector3D LookDirection)
        {
            // do nothing - it is not necessary to do anything
        }
        UIElement IModelVisual3D.GetUIElement(ModelVisual3DFilter FilterSettings, Viewport3D DestinationViewport3D)
        {
            return new UIElement();
        }
        #endregion //IModelVisual3D Members
    }
}
