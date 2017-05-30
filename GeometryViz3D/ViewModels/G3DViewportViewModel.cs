using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Panel3D;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using GeometryViz3D.StateMachine;
using Sanford.StateMachineToolkit;
using Microsoft.Practices.Prism.ViewModel;
using GeometryViz3D.StateMachine;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using GeometryViz3D.Views;

namespace GeometryViz3D.ViewModels
{
    public enum EventID
    {

        RotateX = 0,

        RotateZ,

        Offset,

        OffsetX,

        OffsetY,

        OffsetZ,

        Reset,

        End,
    }
    public enum SliderFunctionality
    {
        Rotator = 0,
        CameraLocation
    }
    public enum OptCode
    {
        Rotate_x = 0,
        Rotate_y,
        Rotate_z,
        Offset_x,
        Offset_y,
        Offset_z
    }
    public class G3DViewportViewModel : NotificationObject
    {
        private double m_Extent;
        public double Extent
        {
            get
            {
                return m_Extent;
            }
            set
            {
                m_Extent = value;
                RaisePropertyChanged(() => Extent);
            }
        }
        public G3DViewportViewModel()
        {
            elements = new List<IModelVisual3D>();     
        }

        private double _LINE_THICKNESS = 1;
        public double LINE_THICKNESS
        {
            get
            {
                return _LINE_THICKNESS;
            }
            set
            {
                _LINE_THICKNESS = value;
                RaisePropertyChanged(() => LINE_THICKNESS);
            }
        }

        private double _AXIS_TEXT_SIZE = 0.3;
        public double AXIS_TEXT_SIZE
        {
            get
            {
                return _AXIS_TEXT_SIZE;
            }
            set
            {
                _AXIS_TEXT_SIZE = value;
                RaisePropertyChanged(() => AXIS_TEXT_SIZE);
            }
        }
        private double _SliderZoomValue = 1;
        public double SliderZoomValue
        {
            get
            {
                return _SliderZoomValue;
            }
            set
            {
                _SliderZoomValue = value;
                RaisePropertyChanged(() => SliderZoomValue);
            }
        }
        public double X_Angle
        {
            get;
            set;
        }
        public double Y_Angle
        {
            get;
            set;
        }
        public double Z_Angle
        {
            get;
            set;
        }
        public double Offset_x
        {
            get;
            set;
        }
        public double Offset_y
        {
            get;
            set;
        }
        public double Offset_z
        {
            get;
            set;
        }
        public double InitalXAngle
        {
            get;
            set;
        }
        public double InitalZAngle
        {
            get;
            set;
        }
        public double InitalYAngle
        {
            get;
            set;
        }

        #region (Command)
        private Visibility _RotateVisibility = Visibility.Visible;
        public Visibility RotateVisibility
        {
            get
            {
                return _RotateVisibility;
            }
            set
            {
                _RotateVisibility = value;
                RaisePropertyChanged(() => RotateVisibility);
            }
        }
        private Visibility _OffsetVisibility = Visibility.Visible;
        public Visibility OffsetVisibility
        {
            get
            {
                return _OffsetVisibility;
            }
            set
            {
                _OffsetVisibility = value;
                RaisePropertyChanged(() => OffsetVisibility);
            }
        }
        private Visibility _ResetVisibility = Visibility.Visible;
        public Visibility ResetVisibility
        {
            get
            {
                return _ResetVisibility;
            }
            set
            {
                _ResetVisibility = value;
                RaisePropertyChanged(() => ResetVisibility);
            }
        }
        protected DelegateCommand<object> rotateCommand = null;
        public DelegateCommand<object> RotateCommand
        {
            get
            {
                if (rotateCommand == null)
                {
                    rotateCommand = new DelegateCommand<object>(OnRotate, CanRotate);
                }

                return rotateCommand;
            }
        }
        private void OnRotate(object pParameter)
        {
            try
            {
                MyEventArgs tempEventArgs = new MyEventArgs((int)EventID.RotateX, X_Angle);
                transStateMachine.Send((int)TransStateMachine.EventID.RotateX, tempEventArgs);
                transStateMachine.Execute();

                tempEventArgs = new MyEventArgs((int)EventID.RotateZ, Z_Angle);
                transStateMachine.Send((int)TransStateMachine.EventID.RotateZ, tempEventArgs);
                transStateMachine.Execute();

                tempEventArgs = new MyEventArgs((int)EventID.Offset,0);
                transStateMachine.Send((int)TransStateMachine.EventID.Offset, tempEventArgs);
                transStateMachine.Execute();
            }
            catch (Exception err)
            {

            }
        }
        private bool CanRotate(object para)
        {
            return true;
        }
        protected DelegateCommand<object> offsetCommand = null;
        public DelegateCommand<object> OffsetCommand
        {
            get
            {
                if (offsetCommand == null)
                {
                    offsetCommand = new DelegateCommand<object>(OnOffset, CanOffset);
                }

                return offsetCommand;
            }
        }
        private void OnOffset(object pParameter)
        {
            try
            {
                MyEventArgs tempEventArgs = new MyEventArgs((int)EventID.OffsetX, Offset_x);
                transStateMachine.Send((int)TransStateMachine.EventID.OffsetX, tempEventArgs);
                transStateMachine.Execute();

                tempEventArgs = new MyEventArgs((int)EventID.OffsetY, Offset_y);
                transStateMachine.Send((int)TransStateMachine.EventID.OffsetY, tempEventArgs);
                transStateMachine.Execute();

                tempEventArgs = new MyEventArgs((int)EventID.OffsetZ, Offset_z);
                transStateMachine.Send((int)TransStateMachine.EventID.OffsetZ, tempEventArgs);
                transStateMachine.Execute();

                tempEventArgs = new MyEventArgs((int)EventID.End,0);
                transStateMachine.Send((int)TransStateMachine.EventID.End, tempEventArgs);
                transStateMachine.Execute();
            }
            catch (Exception err)
            {

            }
        }
        private bool CanOffset(object para)
        {
            return true;
        }
        protected DelegateCommand<object> resetCommand = null;
        public DelegateCommand<object> ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new DelegateCommand<object>(OnReset, CanReset);
                }

                return resetCommand;
            }
        }
        private void OnReset(object pParameter)
        {
            try
            {
                MyEventArgs tempEventArgs = new MyEventArgs((int)EventID.Reset, 0);
                tempEventArgs = new MyEventArgs((int)EventID.Reset, 0);
                transStateMachine.Send((int)TransStateMachine.EventID.Reset, tempEventArgs);
                transStateMachine.Execute();
            }
            catch (Exception err)
            {

            }
        }
        private bool CanReset(object para)
        {
            return true;
        }
        
        #endregion


        public void InitTransStateMachine()
        {
            RotateVisibility = Visibility.Visible;
            OffsetVisibility = Visibility.Collapsed;

            transStateMachine = new TransStateMachine();
            transStateMachine.SetVMCtrl(this);
            transStateMachine.TransitionCompleted += new TransitionCompletedEventHandler(HandleTransitionCompleted);
            transStateMachine.Execute();
        }
        public void InitXYZArrowMesh(Model3DGroup model_group)
        {
            // X = Red.
            double pointer = arrow_length;
            pointer = Offset_x > 0 ? arrow_length : -arrow_length;
            x_arrow_mesh.AddArrow(origin_x, new Point3D(pointer, 0, 0),
                new Vector3D(0, 1, 0), arrowhead_length);
            DiffuseMaterial x_arrow_material = new DiffuseMaterial(Brushes.Green);
            XArrowModel = new GeometryModel3D(x_arrow_mesh, x_arrow_material);
            model_group.Children.Add(XArrowModel);

            // Y = Green.
            MeshGeometry3D y_arrow_mesh = new MeshGeometry3D();
            pointer = Offset_y > 0 ? arrow_length : -arrow_length;
            y_arrow_mesh.AddArrow(origin_y, new Point3D(0, pointer, 0),
                new Vector3D(1, 0, 0), arrowhead_length);
            DiffuseMaterial y_arrow_material = new DiffuseMaterial(Brushes.Green);
            YArrowModel = new GeometryModel3D(y_arrow_mesh, y_arrow_material);
            model_group.Children.Add(YArrowModel);

            // Z = Blue.
            MeshGeometry3D z_arrow_mesh = new MeshGeometry3D();
            pointer = Offset_z > 0 ? arrow_length : -arrow_length;
            z_arrow_mesh.AddArrow(origin_z, new Point3D(0, 0, pointer),
                new Vector3D(0, 1, 0), arrowhead_length);
            DiffuseMaterial z_arrow_material = new DiffuseMaterial(Brushes.Green);
            ZArrowModel = new GeometryModel3D(z_arrow_mesh, z_arrow_material);
            model_group.Children.Add(ZArrowModel);
        }
        private void HandleTransitionCompleted(object sender, TransitionCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                switch (e.StateID)
                {
                    case (int)TransStateMachine.StateID.StateIdle:
                        OffsetVisibility = Visibility.Collapsed;
                        ResetVisibility = Visibility.Visible;
                        break;
                    case (int)TransStateMachine.StateID.StateRotate:
                        RotateVisibility = Visibility.Collapsed;
                        OffsetVisibility = Visibility.Visible;
                        ResetVisibility = Visibility.Visible;
                        break;
                    case (int)TransStateMachine.StateID.StateOffset:
                        ClearSizedImagedViewport();
                        ResetVisibility = Visibility.Visible;
                        break;
                    case (int)TransStateMachine.StateID.StateEnd:
                        OffsetVisibility = Visibility.Collapsed;
                        ResetVisibility = Visibility.Visible;
                        ClearArrowViewport();
                        break;
                    default:
                        break;
                }
            }
            else
            {

            }
        }
        public void ResetOffsetArrowElements()
        {
            z_axisRotation_arrow.Angle = InitalZAngle;
            x_axisRotation_arrow.Angle = InitalXAngle;
            y_axisRotation_arrow.Angle = InitalYAngle;
            x_axisRotation_arrow.Angle = x_axisRotation_arrow.Angle + X_Angle;
            z_axisRotation_arrow.Angle = z_axisRotation_arrow.Angle + Z_Angle;
            translate_arrow.OffsetX = 0;
            translate_arrow.OffsetY = 0;
            translate_arrow.OffsetZ = 0;
            translate_arrow.OffsetX = translate_arrow.OffsetX + Offset_x;
            translate_arrow.OffsetY = translate_arrow.OffsetY + Offset_y;
            translate_arrow.OffsetZ = translate_arrow.OffsetZ + Offset_z;
            if (!view.mainViewport.Children.Contains(model_arrow_visual))
            {
                view.mainViewport.Children.Add(model_arrow_visual);
            }
        }
        public void ResetPointedImaged()
        {
            foreach (ModelVisual3D element in model_PointedImaged_visual)
            {
                if(!view.mainViewport.Children.Contains(element))
                    view.mainViewport.Children.Add(element);
            }
        }
        public void AddOffsetArrowElements(Viewport3D mainViewport)
        {            
            model_arrow_visual.Content = Model3DArrowGroup;
            InitXYZArrowMesh(Model3DArrowGroup);

            transform3DGroup_arrow.Children.Add(x_rotation_arrow);
            transform3DGroup_arrow.Children.Add(y_rotation_arrow);
            transform3DGroup_arrow.Children.Add(z_rotation_arrow);
            transform3DGroup_arrow.Children.Add(translate_arrow);

            x_rotation_arrow.Rotation = x_axisRotation_arrow;
            x_axisRotation_arrow.Angle = 0;
            x_axisRotation_arrow.Axis = new Vector3D(1, 0, 0);

            y_rotation_arrow.Rotation = y_axisRotation_arrow;
            y_axisRotation_arrow.Angle = 0;
            y_axisRotation_arrow.Axis = new Vector3D(0, 1, 0);

            z_rotation_arrow.Rotation = z_axisRotation_arrow;
            z_axisRotation_arrow.Angle = 0;
            z_axisRotation_arrow.Axis = new Vector3D(0, 0, 1);

            z_axisRotation_arrow.Angle = InitalZAngle;
            y_axisRotation_arrow.Angle = InitalYAngle;


            x_axisRotation_arrow.Angle = x_axisRotation_arrow.Angle + X_Angle;
            z_axisRotation_arrow.Angle = z_axisRotation_arrow.Angle + Z_Angle;
            translate_arrow.OffsetX = translate_arrow.OffsetX + Offset_x;
            translate_arrow.OffsetY = translate_arrow.OffsetY + Offset_y;
            translate_arrow.OffsetZ = translate_arrow.OffsetZ + Offset_z;

            Model3DArrowGroup.Transform = transform3DGroup_arrow;

            mainViewport.Children.Add(model_arrow_visual);
        }

        public void AddLabeledElements(double extent, Viewport3D mainViewport)
        {
            BitmapImage xbitmap_anticlock = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/x_anticlock.bmp"));
            BitmapImage xbitmap_clock = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/x_clock.bmp"));
            Point3DSizedImaged xImage_anticlock = new Point3DSizedImaged(extent/2 * 0.9, 0, 0, defaultsize, xbitmap_anticlock);
            Point3DSizedImaged xImage_clock = new Point3DSizedImaged(extent / 2 * 0.9, 0, 0, defaultsize, xbitmap_clock);

            BitmapImage ybitmap_anticlock = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/y_anticlock.bmp"));
            BitmapImage ybitmap_clock = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/y_anticlock.bmp"));
            Point3DSizedImaged yImage_anticlock = new Point3DSizedImaged(0, extent / 2 * 0.9, 0, defaultsize, ybitmap_anticlock);
            Point3DSizedImaged yImage_clock = new Point3DSizedImaged(0, extent / 2 * 0.9, 0, defaultsize, ybitmap_clock);

            BitmapImage zbitmap_anticlock = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/z_anticlock.bmp"));
            BitmapImage zbitmap_clock = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/y_anticlock.bmp"));
            Point3DSizedImaged zImage_anticlock = new Point3DSizedImaged(0, 0, extent / 2 * 0.9, defaultsize, zbitmap_anticlock);
            Point3DSizedImaged zImage_clock = new Point3DSizedImaged(0, 0, extent / 2 * 0.9, defaultsize, zbitmap_clock);

            BitmapImage petctbitmap = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/pet_ct.bmp"));
            Point3DSizedImaged petctImage = new Point3DSizedImaged(0, 0, 0, 20, petctbitmap);
            if(X_Angle > 0)
            {
                Elements.Add(xImage_anticlock);
            }
            else
            {
                Elements.Add(xImage_clock);
            }
            if (Z_Angle > 0)
            {
                Elements.Add(zImage_anticlock);
            }
            else
            {
                Elements.Add(zImage_clock);
            }
            //Elements.Add(petctImage);
            Transform3DGroup transform3DGroup = new Transform3DGroup();
            RotateTransform3D x_rotation = new RotateTransform3D();
            RotateTransform3D y_rotation = new RotateTransform3D();
            RotateTransform3D z_rotation = new RotateTransform3D();
            TranslateTransform3D translate = new TranslateTransform3D();
            transform3DGroup.Children.Add(x_rotation);
            transform3DGroup.Children.Add(y_rotation);
            transform3DGroup.Children.Add(z_rotation);
            transform3DGroup.Children.Add(translate);

            x_rotation.Rotation = x_axisRotation_Imaged;
            x_axisRotation_Imaged.Angle = 0;
            x_axisRotation_Imaged.Axis = new Vector3D(1, 0, 0);

            y_rotation.Rotation = y_axisRotation_Imaged;
            y_axisRotation_Imaged.Angle = 0;
            y_axisRotation_Imaged.Axis = new Vector3D(0, 1, 0);

            z_rotation.Rotation = z_axisRotation_Imaged;
            z_axisRotation_Imaged.Angle = 0;
            z_axisRotation_Imaged.Axis = new Vector3D(0, 0, 1);

            z_axisRotation_Imaged.Angle = InitalZAngle;
            y_axisRotation_Imaged.Angle = InitalYAngle;

            translate.OffsetX = translate.OffsetX + Offset_x;
            translate.OffsetY = translate.OffsetY + Offset_y;
            translate.OffsetZ = translate.OffsetZ + Offset_z;
            x_axisRotation_Imaged.Angle = x_axisRotation_Imaged.Angle + X_Angle;
            z_axisRotation_Imaged.Angle = z_axisRotation_Imaged.Angle + Z_Angle;

            if (elements != null && mainViewport != null)
            {

                foreach (IModelVisual3D element in elements)
                {
                    ModelVisual3D modelVisual3DElement = element.GetModelVisual3D(filter);
                    model_PointedImaged_visual.Add(modelVisual3DElement);
                    modelVisual3DElement.Transform = transform3DGroup;
                    mainViewport.Children.Add(modelVisual3DElement);
                }
            }
        }

        public void RefreshCustomElements(Viewport3D mainViewport)
        {
            if (elements != null && mainViewport != null)
            {
                PerspectiveCamera _camera = (PerspectiveCamera)mainViewport.Camera;
                Vector3D newlookdirection = Panel3DMath.Transform3DVector(_camera.Transform, _camera.LookDirection);
                foreach (IModelVisual3D element in elements)
                {
                    element.UpdateViewToLookDirection(newlookdirection);
                }
            }
        }

        public void RefreshArrowNLabeled(double value, OptCode type, Viewport3D mainViewport)
        {
            switch (type)
            {
                case OptCode.Rotate_x:
                    x_axisRotation_arrow.Angle = value;
                    x_axisRotation_Imaged.Angle = value;
                    break;
                case OptCode.Rotate_y:
                    y_axisRotation_arrow.Angle = value;
                    y_axisRotation_Imaged.Angle = value;
                    break;
                case OptCode.Rotate_z:
                    z_axisRotation_arrow.Angle = value;
                    z_axisRotation_Imaged.Angle = value;
                    break;
                case OptCode.Offset_x:
                    break;
                case OptCode.Offset_y:
                    break;
                case OptCode.Offset_z:
                    break;
                default:
                    break;
            }
            if (elements != null && mainViewport != null)
            {
                PerspectiveCamera camera = (PerspectiveCamera)mainViewport.Camera;
                Vector3D newlookdirection = Panel3DMath.Transform3DVector(camera.Transform, camera.LookDirection);
                foreach (IModelVisual3D element in elements)
                {
                    element.UpdateViewToLookDirection(newlookdirection);
                }
            }
        }

        public void ClearSizedImagedViewport()
        {
            if (view.mainViewport != null)
            {
                ModelVisual3D m;
                for (int i = model_PointedImaged_visual.Count - 1; i >= 0; i--)
                {
                    m = (ModelVisual3D)model_PointedImaged_visual[i];
                    view.mainViewport.Children.Remove(m);
                }
            }
        }

        public void ClearArrowViewport()
        {
            if (view.mainViewport != null)
            {
                view.mainViewport.Children.Remove(model_arrow_visual);
            }
        }

        #region ContextMenuItem
        public void Rotate(MyEventArgs args)
        {
            double value = args.value;
            switch (args.eventId)
            {
                case (int)EventID.RotateX:
                    view.rotationX_Reg.Angle -= value;
                    x_axisRotation_arrow.Angle -= value;                    
                    break;
                case (int)EventID.RotateZ:
                    view.rotationZ_Reg.Angle -= value;
                    z_axisRotation_arrow.Angle -= value;
                    break;
                default:
                    break;
            }
        }
        public void Offset(MyEventArgs args)
        {
            double value = args.value;
            switch (args.eventId)
            {
                case (int)EventID.OffsetX:
                    view.translate3D_Reg.OffsetX -= value;
                    translate_arrow.OffsetX -= value;
                    break;
                case (int)EventID.OffsetY:
                    view.translate3D_Reg.OffsetY -= value;
                    translate_arrow.OffsetY -= value;
                    break;
                case (int)EventID.OffsetZ:
                    view.translate3D_Reg.OffsetZ -= value;
                    translate_arrow.OffsetZ -= value;
                    break;
                default:
                    break;
            }
        }
        public void Reset(MyEventArgs args)
        {
            view.Reset(-5, -5, -2, -2, -2);
            ResetOffsetArrowElements();
            ResetPointedImaged();
        }
        #endregion

        #region member
        private G3DViewport view;
        public void SetView(G3DViewport _view)
        {
            view = _view;   
        }
        private List<IModelVisual3D> elements;
        public List<IModelVisual3D> Elements
        {
            get
            {
                return elements;
            }
        }
        const double defaultsize = 2;
        public double CamLocation_x = defaultCamLocation_x;
        public double CamLocation_y = defaultCamLocation_y;
        public double CamLocation_z = defaultCamLocation_z;
        const double defaultCamLocation_x = 10;
        const double defaultCamLocation_y = 10;
        const double defaultCamLocation_z = 10;
        private ModelVisual3DFilter filter = ModelVisual3DFilter.AllOn;

        private Model3DGroup Model3DArrowGroup = new Model3DGroup();
        //private Model3DGroup Model3DLabeledGroupd = new Model3DGroup();

        private Point3D origin_x = new Point3D(0, 0, 0);
        private Point3D origin_y = new Point3D(0, 0, 0);
        private Point3D origin_z = new Point3D(0, 0, 0);
        private MeshGeometry3D x_arrow_mesh = new MeshGeometry3D();
        private MeshGeometry3D y_arrow_mesh = new MeshGeometry3D();
        private MeshGeometry3D z_arrow_mesh = new MeshGeometry3D();
        const double arrow_length = 4;
        const double arrowhead_length = 1;
        private GeometryModel3D
            XArrowModel, YArrowModel, ZArrowModel;

        private Transform3DGroup transform3DGroup_arrow = new Transform3DGroup();
        private RotateTransform3D x_rotation_arrow = new RotateTransform3D();
        private RotateTransform3D y_rotation_arrow = new RotateTransform3D();
        private RotateTransform3D z_rotation_arrow = new RotateTransform3D();
        private TranslateTransform3D translate_arrow = new TranslateTransform3D();
        private AxisAngleRotation3D x_axisRotation_arrow = new AxisAngleRotation3D();
        private AxisAngleRotation3D y_axisRotation_arrow = new AxisAngleRotation3D();
        private AxisAngleRotation3D z_axisRotation_arrow = new AxisAngleRotation3D();
        private TransStateMachine transStateMachine = null;
        private ModelVisual3D model_arrow_visual = new ModelVisual3D();


        private AxisAngleRotation3D x_axisRotation_Imaged = new AxisAngleRotation3D();
        private AxisAngleRotation3D y_axisRotation_Imaged = new AxisAngleRotation3D();
        private AxisAngleRotation3D z_axisRotation_Imaged = new AxisAngleRotation3D();

        private List<ModelVisual3D> model_PointedImaged_visual = new List<ModelVisual3D>();
        #endregion
    }
}
