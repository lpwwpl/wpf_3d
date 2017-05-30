using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryViz3D.Model;

namespace GeometryViz3D.ViewModels.ModelTree
{
    public class PlaneItemViewModel : ModelTreeItemViewModel
    {
        G3DPlane m_plane;

        public PlaneItemViewModel(G3DPlane plane, IMainViewModel mainVM)
            : base(mainVM)
        {
            m_plane = plane;
        }
    }
}
