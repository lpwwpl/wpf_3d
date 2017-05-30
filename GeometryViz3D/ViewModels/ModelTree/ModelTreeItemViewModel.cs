using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreMVVM.ViewModels;
using System.Collections.ObjectModel;

namespace GeometryViz3D.ViewModels.ModelTree
{
    public class ModelTreeItemViewModel : ViewModelBase
    {
        readonly ObservableCollection<ModelTreeItemViewModel> m_children 
            = new ObservableCollection<ModelTreeItemViewModel>();

        bool m_isSelected;

        IMainViewModel m_mainVM;

        public ModelTreeItemViewModel(IMainViewModel mainVM)
        {
            m_mainVM = mainVM;
        }

        #region Properties

        #region Children

        /// <summary>
        /// Returns the logical child items of this object.
        /// </summary>
        public ObservableCollection<ModelTreeItemViewModel> Children
        {
            get { return m_children; }
        }

        protected IMainViewModel MainViewModel
        {
            get { return m_mainVM; }
        }

        #endregion // Children

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return m_isSelected; }
            set
            {
                if (value != m_isSelected)
                {
                    m_isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }

                //m_mainViewModel.SelectedItem = value ? this : null;
            }
        }

        #endregion // IsSelected

        #endregion

        protected void OnModelChanged()
        {
            m_mainVM.OnModelChanged();
        }

        public void RemoveAllChilden()
        {
            m_children.Clear();
        }

        public void AddChild(ModelTreeItemViewModel child)
        {
            m_children.Add(child);
        }
    }
}
