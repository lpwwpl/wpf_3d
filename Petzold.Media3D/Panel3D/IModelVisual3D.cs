using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Panel3D
{
    public class ModelVisual3DFilter
    {
        public bool Triangles { get; set; }
        public bool Texts3D { get; set; }
        public bool Texts2D { get; set; }
        public bool Image2D { get; set; }
        public bool Image3D { get; set; }
        public bool Lines { get; set; }
        private ModelVisual3DFilter(bool EnableTriangles,
          bool EnableTexts3D,
          bool EnableTexts2D,
          bool EnableImages2D,
          bool EnableImages3D,
          bool EnableLines)
        {
            Triangles = EnableTriangles;
            Texts3D = EnableTexts3D;
            Texts2D = EnableTexts3D;
            Image2D = EnableImages2D;
            Image3D = EnableImages3D;
            Lines = EnableLines;
        }
        public static ModelVisual3DFilter AllOn
        {
            get
            {
                return new ModelVisual3DFilter(true, true, true, true,true,true);
            }
        }
        public static ModelVisual3DFilter AllOnExceptTexts
        {
            get
            {
                return new ModelVisual3DFilter(true, false, false, true,true,true);
            }
        }
        public static ModelVisual3DFilter AllOff
        {
            get
            {
                return new ModelVisual3DFilter(false, false, false, false,false,false);
            }
        }
    }
    public interface IModelVisual3D
    {
        ModelVisual3D GetModelVisual3D(ModelVisual3DFilter FilterSettings);
        UIElement GetUIElement(ModelVisual3DFilter FilterSettings, Viewport3D DestinationViewport3D);
        void UpdateViewToLookDirection(Vector3D LookDirection);
        //void UpdateRotation();
    }
}
