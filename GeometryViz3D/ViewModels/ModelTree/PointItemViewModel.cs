using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using GeometryViz3D.Model;

namespace GeometryViz3D.ViewModels.ModelTree
{
    public class PointItemViewModel : ModelTreeItemViewModel
    {
        G3DPoint m_point;

        public PointItemViewModel(G3DPoint point, IMainViewModel mainVM)
            : base(mainVM)
        {
            m_point = point;
        }

        public G3DPoint Point
        {
            get { return m_point; }
        }

        public double X
        {
            get { return m_point.Position.X; }
            set 
            { 
                m_point.Position = new Point3D(value, m_point.Position.Y, m_point.Position.Z);
                OnPropertyChanged("X");
                OnModelChanged();
            }
        }

        public double Y
        {
            get { return m_point.Position.Y; }
            set
            {
                m_point.Position = new Point3D(m_point.Position.X, value, m_point.Position.Z);
                OnPropertyChanged("Y");
                OnModelChanged();
            }
        }

        public double Z
        {
            get { return m_point.Position.Z; }
            set
            {
                m_point.Position = new Point3D(m_point.Position.X, m_point.Position.Y, value);
                OnPropertyChanged("Z");
                OnModelChanged();
            }
        }

        public string Label
        {
            get { return m_point.Label; }
            set 
            { 
                m_point.Label = value;
                OnPropertyChanged("Label");
                OnPropertyChanged("Model");
            }
        }

        public override string ToString()
        {
            return m_point.ToString();
        }
    }
}
