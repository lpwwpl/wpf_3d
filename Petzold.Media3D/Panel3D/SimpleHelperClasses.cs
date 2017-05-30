using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using _3DTools;
using System.Windows.Media.Imaging;

namespace Panel3D
{
    public class Panel3DMath
    {
        #region  Helpers
        public static Vector3D Transform3DVector(Transform3D transform, Vector3D vector)
        {
            Point3D input = new Point3D(vector.X, vector.Y, vector.Z);
            Point3D output;
            if (transform.TryTransform(input, out output))
            {
                return new Vector3D(output.X, output.Y, output.Z);
            }
            return vector;
        }
        public static Point Get2DPoint(Point3D p3d, Viewport3D vp)
        {
            bool TransformationResultOK;
            Viewport3DVisual vp3Dv = VisualTreeHelper.GetParent(
              vp.Children[0]) as Viewport3DVisual;
            Matrix3D m = MathUtils.TryWorldToViewportTransform(vp3Dv, out TransformationResultOK);
            if (!TransformationResultOK) return new Point(0, 0);
            Point3D pb = m.Transform(p3d);
            Point p2d = new Point(pb.X, pb.Y);
            return p2d;
        }
        #endregion
    }
    public class Panel3DPoint3D : ICloneable
    {
        private double x, y, z;
        protected IModelVisual3D MyModelVisual3D;
        protected virtual void UpdateMyModelVisual3D() {/*do nothing*/}

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                UpdateMyModelVisual3D();
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
                UpdateMyModelVisual3D();
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
                UpdateMyModelVisual3D();
            }
        }
        public Point3D Point3D
        {
            get
            {
                return new Point3D(X, Y, Z);
            }
        }
        public Panel3DPoint3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        #region ICloneable Members
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        private TranslateTransform3D SlidersRotator_center;
        private AxisAngleRotation3D SlidersRotator_rot_x;
        private AxisAngleRotation3D SlidersRotator_rot_y;
        private AxisAngleRotation3D SlidersRotator_rot_z;
        private ScaleTransform3D SlidersRotator_zoom;
        public void slider_x_changed(int m_sliderfunctionality, double slider_x_value)
        {
            double val = System.Math.Round(slider_x_value, 2);
            switch (m_sliderfunctionality)
            {
                case 0:
                    SlidersRotator_rot_x.Angle = val;
                    break;
                case 1:
                    //SetCamera();
                    break;
            }
        }
        public void slider_y_changed(int m_sliderfunctionality, double slider_y_value)
        {
            double val = System.Math.Round(slider_y_value, 2);
            switch (m_sliderfunctionality)
            {
                case 0:
                    SlidersRotator_rot_y.Angle = val;
                    break;
                case 1:
                    //SetCamera();
                    break;
            }
        }
        public void slider_z_changed(int m_sliderfunctionality, double slider_z_value)
        {
            double val = System.Math.Round(slider_z_value, 2);
            switch (m_sliderfunctionality)
            {
                case 0:
                    SlidersRotator_rot_z.Angle = val;
                    break;
                case 1:
                    //SetCamera();
                    break;
            }
        }
        #endregion
    }
    public class Point3DSized : Panel3DPoint3D, IModelVisual3D
    {
        private double size;
        protected override void UpdateMyModelVisual3D()
        {
            MyModelVisual3D = new CubeModel3DModel(X, Y, Z, Size, Size, Size, true);
        }
        public double Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                UpdateMyModelVisual3D();
            }
        }
        public Point3DSized(double x, double y, double z, double size)
            : base(x, y, z)
        {
            Size = size;
            this.UpdateMyModelVisual3D();
        }

        #region IModelVisual3D Members
        public virtual ModelVisual3D GetModelVisual3D(ModelVisual3DFilter FilterSettings)
        {
            return MyModelVisual3D.GetModelVisual3D(FilterSettings);
        }
        public virtual void UpdateViewToLookDirection(Vector3D LookDirection)
        {
            MyModelVisual3D.UpdateViewToLookDirection(LookDirection);
        }
        public virtual UIElement GetUIElement(ModelVisual3DFilter FilterSettings, Viewport3D DestinationViewport3D)
        {
            return new UIElement();
        }
        #endregion
    }
    public class Point3DSizedLabeled : Point3DSized
    {
        private string label;
        protected override void UpdateMyModelVisual3D()
        {
            MyModelVisual3D = new ElementWithDescription(X, Y, Z, Size, Label);
        }
        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
                UpdateMyModelVisual3D();
            }
        }
        public Point3DSizedLabeled(double x, double y, double z, double size, string label)
            : base(x, y, z, size)
        {
            Label = label;
            this.UpdateMyModelVisual3D();
        }
        #region IModelVisual3D Members
        public override ModelVisual3D GetModelVisual3D(ModelVisual3DFilter FilterSettings)
        {
            return MyModelVisual3D.GetModelVisual3D(FilterSettings);
        }
        public override UIElement GetUIElement(ModelVisual3DFilter FilterSettings, Viewport3D DestinationViewport3D)
        {
            return MyModelVisual3D.GetUIElement(FilterSettings, DestinationViewport3D);
        }
        #endregion
    }
    public class Point3DSizedImaged : Point3DSized
    {
        private BitmapImage image;
        protected override void UpdateMyModelVisual3D()
        {
            MyModelVisual3D = new ElementWithImage(X, Y, Z, Size, image);
        }
        public BitmapImage Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                UpdateMyModelVisual3D();
            }
        }
        public Point3DSizedImaged(double x, double y, double z, double size, BitmapImage image)
            : base(x, y, z, size)
        {
            Image = image;
            this.UpdateMyModelVisual3D();
        }
        #region IModelVisual3D Members
        public override ModelVisual3D GetModelVisual3D(ModelVisual3DFilter FilterSettings)
        {
            return MyModelVisual3D.GetModelVisual3D(FilterSettings);
        }
        public override UIElement GetUIElement(ModelVisual3DFilter FilterSettings, Viewport3D DestinationViewport3D)
        {
            return MyModelVisual3D.GetUIElement(FilterSettings, DestinationViewport3D);
        }
        #endregion
    }
    public class Connection3D : IModelVisual3D
    {
        public Panel3DPoint3D Start { get; set; }
        public Panel3DPoint3D End { get; set; }
        public Connection3D(Panel3DPoint3D start, Panel3DPoint3D end)
        {
            Start = start;
            End = end;
        }
        #region IModelVisual3D Members
        ModelVisual3D IModelVisual3D.GetModelVisual3D(ModelVisual3DFilter FilterSettings)
        {
            if (FilterSettings.Lines)
            {
                ScreenSpaceLines3D line = new ScreenSpaceLines3D();
                line.Thickness = 1;
                line.Color = Colors.Black;
                line.Points.Add(Start.Point3D);
                line.Points.Add(End.Point3D);
                return line;
            }
            else return new ModelVisual3D();
        }
        void IModelVisual3D.UpdateViewToLookDirection(Vector3D LookDirection)
        {
            //do nothing
        }
        UIElement IModelVisual3D.GetUIElement(ModelVisual3DFilter FilterSettings, Viewport3D DestinationViewport3D)
        {
            return new UIElement();
        }
        #endregion
    }
}
