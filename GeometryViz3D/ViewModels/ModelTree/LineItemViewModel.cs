using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryViz3D.Model;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace GeometryViz3D.ViewModels.ModelTree
{
    public class LineItemViewModel : ModelTreeItemViewModel
    {
        G3DLine m_line;
        PointItemViewModel m_startPoint, m_endPoint;

        public LineItemViewModel(G3DLine line, IMainViewModel mainVM)
            : base(mainVM)
        {
            m_line = line;
            GetEndPoints();
        }

        public string Label
        {
            get { return m_line.Label; }
        }

        public PointItemViewModel StartPoint
        {
            get { return m_startPoint; }
            set
            {
                m_startPoint = value;
                m_line.StartPoint = value.Point;

                OnModelChanged();
                
                OnPropertyChanged("StartPoint");
            }
        }

        public PointItemViewModel EndPoint
        {
            get { return m_endPoint; }
            set
            {
                m_endPoint = value;
                m_line.EndPoint = value.Point;

                OnModelChanged();
                
                OnPropertyChanged("EndPoint");
            }
        }

        public string Color
        {
            get { return G3DColors.GetColorName(m_line.Color.ToString()); }
            set
            {
                m_line.Color = G3DColors.GetColor(value);

                OnModelChanged();

                OnPropertyChanged("Color");
            }
        }

        public ObservableCollection<PointItemViewModel> AllPoints
        {
            get { return MainViewModel.AllPoints; }
        }

        private void GetEndPoints()
        {
            m_startPoint = null;
            m_endPoint = null;

            foreach (var p in AllPoints)
            {
                if (p.Point.Equals(m_line.StartPoint))
                {
                    m_startPoint = p;
                }
                else if (p.Point.Equals(m_line.EndPoint))
                {
                    m_endPoint = p;
                }

                if (m_startPoint != null && m_endPoint != null)
                {
                    break;
                }
            }
        }
    }
}
