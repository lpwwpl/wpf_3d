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

namespace GeometryViz3D.Views
{
    /// <summary>
    /// Interaction logic for NavicatorViewport.xaml
    /// </summary>
    public partial class NavicatorViewport : UserControl
    {
        public NavicatorViewport()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            camera.UpDirection = new Vector3D(0, 1, 0);
            camera.LookDirection = new Vector3D(0, 0, -1);
            camera.FieldOfView = 45;

            rotationX_Camera.Angle = -15;
            rotationY_Camera.Angle = 15;
            rotationZ_Camera.Angle = 0;

            directionalLight.Color = Color.FromRgb(0x80, 0x80, 0x80);
            directionalLight.Direction = new Vector3D(0, 0, -1);
            ambientLight.Color = Color.FromRgb(0x80, 0x80, 0x80);

            camera.Position = new Point3D(0, 0, 10);
        }
    }
}
