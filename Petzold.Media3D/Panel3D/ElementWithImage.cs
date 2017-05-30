using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;

namespace Panel3D
{
    class ElementWithImage : IModelVisual3D
    {
        public static ModelVisual3D CreateImageLabel3D(
            BitmapImage image,
            Brush textColor,
            bool isDoubleSided,
            double size,
            Point3D basePoint,
            bool isBasePointCenterPoint,
            Vector3D vectorOver,
            Vector3D vectorUp)
        {
            Image imageControl = new Image();
            imageControl.Width = size;
            imageControl.Height = size;
            imageControl.Source = image;
            DiffuseMaterial mataterialWithLabel = new DiffuseMaterial();
            mataterialWithLabel.Brush = new VisualBrush(imageControl);
            double width = imageControl.Width;
            Point3D p0 = basePoint;
            // when the base point is the center point we have to set it up in different way
            if (isBasePointCenterPoint)
                p0 = basePoint - width / 2 * vectorOver - size / 2 * vectorUp;
            Point3D p1 = p0 + vectorUp * 1 * size;
            Point3D p2 = p0 + vectorOver * width;
            Point3D p3 = p0 + vectorUp * 1 * size + vectorOver * width;
            // we are going to create object in 3D now:
            // this object will be painted using the (text) brush created before
            // the object is rectangle made of two triangles (on each side).
            MeshGeometry3D mg_RestangleIn3D = new MeshGeometry3D();
            mg_RestangleIn3D.Positions = new Point3DCollection();
            mg_RestangleIn3D.Positions.Add(p0);    // 0
            mg_RestangleIn3D.Positions.Add(p1);    // 1
            mg_RestangleIn3D.Positions.Add(p2);    // 2
            mg_RestangleIn3D.Positions.Add(p3);    // 3
            // when we want to see the text on both sides:
            if (isDoubleSided)
            {
                mg_RestangleIn3D.Positions.Add(p0);    // 4
                mg_RestangleIn3D.Positions.Add(p1);    // 5
                mg_RestangleIn3D.Positions.Add(p2);    // 6
                mg_RestangleIn3D.Positions.Add(p3);    // 7
            }
            mg_RestangleIn3D.TriangleIndices.Add(0);
            mg_RestangleIn3D.TriangleIndices.Add(3);
            mg_RestangleIn3D.TriangleIndices.Add(1);
            mg_RestangleIn3D.TriangleIndices.Add(0);
            mg_RestangleIn3D.TriangleIndices.Add(2);
            mg_RestangleIn3D.TriangleIndices.Add(3);
            // when we want to see the text on both sides:
            if (isDoubleSided)
            {
                mg_RestangleIn3D.TriangleIndices.Add(4);
                mg_RestangleIn3D.TriangleIndices.Add(5);
                mg_RestangleIn3D.TriangleIndices.Add(7);
                mg_RestangleIn3D.TriangleIndices.Add(4);
                mg_RestangleIn3D.TriangleIndices.Add(7);
                mg_RestangleIn3D.TriangleIndices.Add(6);
            }
            // texture coordinates must be set to
            // stretch the TextBox brush to cover 
            // the full side of the 3D label.
            mg_RestangleIn3D.TextureCoordinates.Add(new Point(0, 1));
            mg_RestangleIn3D.TextureCoordinates.Add(new Point(0, 0));
            mg_RestangleIn3D.TextureCoordinates.Add(new Point(1, 1));
            mg_RestangleIn3D.TextureCoordinates.Add(new Point(1, 0));
            // when the label is double sided:
            if (isDoubleSided)
            {
                mg_RestangleIn3D.TextureCoordinates.Add(new Point(1, 1));
                mg_RestangleIn3D.TextureCoordinates.Add(new Point(1, 0));
                mg_RestangleIn3D.TextureCoordinates.Add(new Point(0, 1));
                mg_RestangleIn3D.TextureCoordinates.Add(new Point(0, 0));
            }
            // Now it is time to create ModelVisual3D (that we are goint ot return):
            ModelVisual3D result = new ModelVisual3D();
            // we are setting the content:
            // our 3D rectangle object covered with materila that is made of label (TextBox with text)
            result.Content = new GeometryModel3D(mg_RestangleIn3D, mataterialWithLabel); ;
            return result;
        }
        private Vector3D TextVectorOver;
        private Vector3D TextVectorUp;
        private double x, y, z, size;
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;                
            }
        }
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;                
            }
        }
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;                
            }
        }
        public double Size { get { return size; } set { size = value; } }
        public string Description { get; set; }
        public BitmapImage Image { get; set; }
        public Point3D Point3D
        {
            get
            {
                return new Point3D(x, y, z);
            }
        }
        public ElementWithImage(double x, double y, double z, double size, BitmapImage image)
        {
            this.size = size;
            this.x = x;
            this.y = y;
            this.z = z;
            this.Image = image;
            TextVectorOver = new Vector3D(1, 0, 0);
            TextVectorUp = new Vector3D(0, 1, 0);
        }

        #region IModelVisual3D Members

        ModelVisual3D IModelVisual3D.GetModelVisual3D(ModelVisual3DFilter FilterSettings)
        {
            ModelVisual3D model;
            if (FilterSettings.Image3D)
            {
                model = CreateImageLabel3D(
                  Image, new SolidColorBrush(Colors.Black),
                  true, Size, new Point3D(X, Y, Z), true,
                 TextVectorOver, TextVectorUp);
            }
            else model = new ModelVisual3D();
            return model;
        }
        void IModelVisual3D.UpdateViewToLookDirection(Vector3D LookDirection)
        {
            Vector3D look = LookDirection;
            look.Normalize();
            TextVectorOver = new Vector3D(-look.Y, look.X, 0);
            TextVectorUp = new Vector3D(-look.Z * look.X, -look.Z * look.Y, look.Y * look.Y + look.X * look.X);
            TextVectorOver.Normalize();
            TextVectorUp.Normalize();
        }
        UIElement IModelVisual3D.GetUIElement(ModelVisual3DFilter FilterSettings, Viewport3D DestinationViewport3D)
        {
            if (FilterSettings.Image2D)
            {
                Image image = new Image();
                image.Source = Image;
                Point p2d = Panel3DMath.Get2DPoint(this.Point3D, DestinationViewport3D);
                Canvas.SetTop(image, p2d.Y + 0);
                Canvas.SetLeft(image, p2d.X + 10);
                return image;
            }
            else
                return new UIElement();
        }
        #endregion //IModelVisual3D Members
    }
}
