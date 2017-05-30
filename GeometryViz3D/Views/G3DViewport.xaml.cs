using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using Petzold.Media3D;
using GeometryViz3D.Utils;
using GeometryViz3D.ViewModels;
using Panel3D;

namespace GeometryViz3D.Views
{
    /// <summary>
    /// Interaction logic for viewport.xaml
    /// </summary>
    public partial class G3DViewport : UserControl
    {
        public G3DViewport()
        {
            InitializeComponent();
            this.DataContext = G3DViewportViewModel;
            G3DViewportViewModel.SetView(this);
        }
        public void Reset(double x_angle, double z_angle, double x_offset, double y_offset, double z_offset)
        {
            //initial transform
            g3DViewportViewModel.InitalZAngle = -180;
            g3DViewportViewModel.InitalYAngle = 120;
            g3DViewportViewModel.InitalXAngle = 0;
            g3DViewportViewModel.Z_Angle = z_angle;
            g3DViewportViewModel.Y_Angle = 0;
            g3DViewportViewModel.X_Angle = x_angle;
            g3DViewportViewModel.Offset_x = x_offset;
            g3DViewportViewModel.Offset_y = y_offset;
            g3DViewportViewModel.Offset_z = z_offset;

            slider_x.Value = g3DViewportViewModel.InitalXAngle;
            slider_y.Value = g3DViewportViewModel.InitalYAngle;
            slider_z.Value = g3DViewportViewModel.InitalZAngle;

            rotationZ_BaseAxes.Angle = g3DViewportViewModel.InitalZAngle;
            rotationY_BaseAxes.Angle = g3DViewportViewModel.InitalYAngle;
            rotationX_BaseAxes.Angle = g3DViewportViewModel.InitalXAngle;
            rotationZ_Reg.Angle = g3DViewportViewModel.InitalZAngle;
            rotationY_Reg.Angle = g3DViewportViewModel.InitalYAngle;
            rotationX_Reg.Angle = g3DViewportViewModel.InitalXAngle;
            translate3D_Reg.OffsetX = 0;
            translate3D_Reg.OffsetY = 0;
            translate3D_Reg.OffsetZ = 0;

            RotationRegAxes();
            TranslateRegAxes();
        }
        public void Init(double _extent, double x_angle,double z_angle,double x_offset,double y_offset,double z_offset)
        {
            g3DViewportViewModel.Extent = _extent;

            camera.UpDirection = new Vector3D(0, 1, 0);
            camera.LookDirection = new Vector3D(0, 0, -1);
            camera.FieldOfView = 45;

            rotationX_Camera.Angle = -15;
            rotationY_Camera.Angle = 15;
            rotationZ_Camera.Angle = 0;


            directionalLight.Color = Color.FromRgb(0x80, 0x80, 0x80);
            directionalLight.Direction = new Vector3D(0, 0, -1);
            ambientLight.Color = Color.FromRgb(0x80, 0x80, 0x80);

            var extent = g3DViewportViewModel.Extent * 2;
            extent = extent * 5;
            BaseAxes.UnitsPerBigTick = (int)extent / 5;
            RegAxes.UnitsPerBigTick = (int)extent * 3 / (5 * 5);

            BaseAxes.Extent = extent;
            BaseAxes.FontSize = g3DViewportViewModel.AXIS_TEXT_SIZE;
            BaseAxes.Thickness = g3DViewportViewModel.LINE_THICKNESS * 1.5;

            RegAxes.Extent = extent / 3 * 2;
            RegAxes.FontSize = g3DViewportViewModel.AXIS_TEXT_SIZE;
            RegAxes.Thickness = g3DViewportViewModel.LINE_THICKNESS * 1.5;

            //initial transform
            g3DViewportViewModel.InitalZAngle = -180;
            g3DViewportViewModel.InitalYAngle = 120;
            g3DViewportViewModel.InitalXAngle = 0;
            g3DViewportViewModel.Z_Angle = z_angle;
            g3DViewportViewModel.Y_Angle = 0;
            g3DViewportViewModel.X_Angle = x_angle;
            g3DViewportViewModel.Offset_x = x_offset;
            g3DViewportViewModel.Offset_y = y_offset;
            g3DViewportViewModel.Offset_z = z_offset;

            slider_x.Value = g3DViewportViewModel.InitalXAngle;
            slider_y.Value = g3DViewportViewModel.InitalYAngle;
            slider_z.Value = g3DViewportViewModel.InitalZAngle;

            rotationZ_BaseAxes.Angle = g3DViewportViewModel.InitalZAngle;
            rotationY_BaseAxes.Angle = g3DViewportViewModel.InitalYAngle;
            rotationX_BaseAxes.Angle = g3DViewportViewModel.InitalXAngle;
            rotationZ_Reg.Angle = g3DViewportViewModel.InitalZAngle;
            rotationY_Reg.Angle = g3DViewportViewModel.InitalYAngle;
            rotationX_Reg.Angle = g3DViewportViewModel.InitalXAngle;
            translate3D_Reg.OffsetX = 0;
            translate3D_Reg.OffsetY = 0;
            translate3D_Reg.OffsetZ = 0;
            camera.Position = new Point3D(0, 0, extent * 3);

            RotationRegAxes();
            TranslateRegAxes();

            g3DViewportViewModel.AddLabeledElements(extent, mainViewport);
            g3DViewportViewModel.AddOffsetArrowElements(mainViewport);


            G3DViewportViewModel.InitTransStateMachine();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init(2,-5,-5,-2,-2,-2);
        }
        public void SetCamera()
        {
            if (mainViewport != null)
            {
                PerspectiveCamera camera = (PerspectiveCamera)mainViewport.Camera;
                Point3D position = new Point3D(G3DViewportViewModel.CamLocation_x, G3DViewportViewModel.CamLocation_y, G3DViewportViewModel.CamLocation_z);
                Vector3D lookDirection = new Vector3D(-G3DViewportViewModel.CamLocation_x, -G3DViewportViewModel.CamLocation_y, -G3DViewportViewModel.CamLocation_z);
                camera.Position = position;
                camera.LookDirection = lookDirection;
                directionalLight.Direction = new Vector3D(-G3DViewportViewModel.CamLocation_x, -G3DViewportViewModel.CamLocation_y, -G3DViewportViewModel.CamLocation_z);
            }
        } 
        public void RotationRegAxes()
        {
            rotationX_Reg.Angle = rotationX_Reg.Angle + G3DViewportViewModel.X_Angle;
            rotationZ_Reg.Angle = rotationZ_Reg.Angle + G3DViewportViewModel.Z_Angle;
        }

        public void TranslateRegAxes()
        {
            translate3D_Reg.OffsetX = translate3D_Reg.OffsetX + G3DViewportViewModel.Offset_x;
            translate3D_Reg.OffsetY = translate3D_Reg.OffsetY + G3DViewportViewModel.Offset_y;
            translate3D_Reg.OffsetZ = translate3D_Reg.OffsetZ + G3DViewportViewModel.Offset_z;
        }
        G3DViewportViewModel g3DViewportViewModel = new G3DViewportViewModel();
        public G3DViewportViewModel G3DViewportViewModel
        {
            get
            {
                return g3DViewportViewModel;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {

            }
            else
            {
                mDown = true;
                Point pos = Mouse.GetPosition(mainViewport);
                mLastPos = new Point(pos.X - mainViewport.ActualWidth / 2, mainViewport.ActualHeight / 2 - pos.Y);
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mDown = false;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (mDown)
            {
                Point pos = Mouse.GetPosition(mainViewport);
                Point actualPos = new Point(pos.X - mainViewport.ActualWidth / 2, mainViewport.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - mLastPos.X, dy = actualPos.Y - mLastPos.Y;

                double mouseAngle = 0;
                if (dx != 0 && dy != 0)
                {
                    mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                    if (dx < 0 && dy > 0) mouseAngle += Math.PI / 2;
                    else if (dx < 0 && dy < 0) mouseAngle += Math.PI;
                    else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;
                }
                else if (dx == 0 && dy != 0) mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
                else if (dx != 0 && dy == 0) mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;

                double axisAngle = mouseAngle + Math.PI / 2;

                Vector3D axis = new Vector3D(Math.Cos(axisAngle) * 4, Math.Sin(axisAngle) * 4, 0);

                double rotation = 0.01 * Math.Sqrt(Math.Pow(dx / 10D, 2) + Math.Pow(dy / 10D, 2));

                Transform3DGroup group = camera.Transform as Transform3DGroup;
                QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
                group.Children.Add(new RotateTransform3D(r));

                mLastPos = actualPos;
            }
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - e.Delta / 100D * 3);
        }

        private void slider_x_changed(object sender, RoutedEventArgs args)
        {
            double val = System.Math.Round(slider_x.Value, 2);
            switch (m_sliderfunctionality)
            {
                case SliderFunctionality.Rotator:
                    rotationX_BaseAxes.Angle = val;
                    rotationX_Reg.Angle = val;
                    G3DViewportViewModel.RefreshArrowNLabeled(slider_x.Value, OptCode.Rotate_x, mainViewport);
                    break;
                case SliderFunctionality.CameraLocation:
                    G3DViewportViewModel.CamLocation_x = val;
                    SetCamera();
                    break;
            }
            G3DViewportViewModel.RefreshCustomElements(mainViewport);
        }

        private void slider_y_changed(object sender, RoutedEventArgs args)
        {
            double val = System.Math.Round(slider_y.Value, 2);
            switch (m_sliderfunctionality)
            {
                case SliderFunctionality.Rotator:
                    rotationY_BaseAxes.Angle = val;
                    rotationY_Reg.Angle = val;
                    G3DViewportViewModel.RefreshArrowNLabeled(slider_y.Value, OptCode.Rotate_y, mainViewport);
                    break;
                case SliderFunctionality.CameraLocation:
                    G3DViewportViewModel.CamLocation_y = val;
                    SetCamera();
                    break;
            }
            G3DViewportViewModel.RefreshCustomElements(mainViewport);
        }

        private void slider_z_changed(object sender, RoutedEventArgs args)
        {
            double val = System.Math.Round(slider_z.Value, 2);
            switch (m_sliderfunctionality)
            {
                case SliderFunctionality.Rotator:
                    rotationZ_BaseAxes.Angle = val;
                    rotationZ_Reg.Angle = val;
                    G3DViewportViewModel.RefreshArrowNLabeled(slider_z.Value, OptCode.Rotate_z, mainViewport);
                    break;
                case SliderFunctionality.CameraLocation:
                    G3DViewportViewModel.CamLocation_z = val;
                    SetCamera();
                    break;
            }
            G3DViewportViewModel.RefreshCustomElements(mainViewport);
        }
        private SliderFunctionality m_sliderfunctionality = SliderFunctionality.Rotator;
        private void ChangeSliderFunctionality(SliderFunctionality functionality)
        {
            m_sliderfunctionality = functionality;
        }
        private bool mDown;
        private Point mLastPos;
    }
}
