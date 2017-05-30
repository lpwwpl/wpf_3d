using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryViz3D.Model;

namespace GeometryViz3D.ViewModels.ModelTree
{
    public class FolderItemViewModel : ModelTreeItemViewModel
    {
        string m_folderName;
        G3DModel m_model;

        public FolderItemViewModel(string folderName, IMainViewModel mainVM)
            : base(mainVM)
        {
            m_folderName = folderName;
        }

        public string Name
        {
            get { return m_folderName; }
        }

        public G3DModel Model
        {
            get { return m_model; }
            set
            {
                m_model = value;
                LoadChildren();
            }
        }

        private void LoadChildren()
        {
            RemoveAllChilden();

            switch (m_folderName.ToUpper())
            {
                case "POINTS":
                    LoadPoints();
                    break;

                case "LINES":
                    LoadLines();
                    break;

                case "PLANES":
                    LoadPlanes();
                    break;
            }
        }

        private void LoadPoints()
        {
            foreach (var point in m_model.Points)
            {
                AddChild(new PointItemViewModel(point, MainViewModel));
            }
        }

        private void LoadLines()
        {
            foreach (var line in m_model.Lines)
            {
                AddChild(new LineItemViewModel(line, MainViewModel));
            }
        }

        private void LoadPlanes()
        {
            foreach (var plane in m_model.Planes)
            {
                AddChild(new PlaneItemViewModel(plane, MainViewModel));
            }
        }
    }
}
