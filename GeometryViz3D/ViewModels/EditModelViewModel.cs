using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreMVVM.ViewModels;
using System.Collections.ObjectModel;
using GeometryViz3D.Model;
using System.Windows.Input;
using CoreMVVM.Commands;
using System.ComponentModel;

namespace GeometryViz3D.ViewModels
{
    public class EditModelViewModel : ViewModelBase
    {
        G3DModel m_model;

        ElementCollection<LineViewModel> m_lines = new ElementCollection<LineViewModel>();
        ElementCollection<PointViewModel> m_points = new ElementCollection<PointViewModel>();
        ObservableCollection<string> m_colors;

        PointViewModel m_selectedPoint;
        LineViewModel m_selectedLine;

        ICommand m_okCommand;
        ICommand m_cancelCommand;

        ICommand m_addLineCommand;
        ICommand m_addPointCommand;

        ICommand m_deletePointCommand;
        ICommand m_deleteLineCommand;

        bool m_newModel = false;

        public EditModelViewModel()
            : this(new G3DModel())
        {
            m_newModel = true;
        }

        public EditModelViewModel(G3DModel model)
        {
            m_model = model;

            m_colors = new ObservableCollection<string>(G3DColors.Colors);

            foreach (var point in model.Points)
            {
                PointViewModel pvm = new PointViewModel(point);
                pvm.PropertyChanged += new PropertyChangedEventHandler(PointViewModel_PropertyChanged);
                m_points.Add(pvm);
            }

            foreach (var line in model.Lines)
            {
                LineViewModel lvm = new LineViewModel(this, line);
                lvm.PropertyChanged += new PropertyChangedEventHandler(LineViewModel_PropertyChanged);
                m_lines.Add(lvm);
            }
        }

        void LineViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Do nothing for now.
        }

        void PointViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ID":
                case "X":
                case "Y":
                case "Z":
                    m_points.UpdateCollection();
                    OnPropertyChanged("Points");
                    break;
            }
        }

        #region Properties

        public G3DModel Model
        {
            get 
            {
                G3DModel model = new G3DModel();

                foreach (var point in m_points)
                {
                    model.AddPoint(point.Label, point.Position);
                }

                // make the lines have valid end points.
                foreach (var line in m_lines)
                {
                    if (line.StartPoint != null && line.EndPoint != null)
                    {
                        if (ExistsPoint(line.StartPoint.Point) && ExistsPoint(line.EndPoint.Point))
                        {
                            model.AddLine(line.StartPoint.Point,
                                line.EndPoint.Point, G3DColors.GetColor(line.Color));
                        }
                    }
                }

                return model; 
            }
        }

        public ObservableCollection<LineViewModel> Lines
        {
            get { return m_lines; }
        }

        public ObservableCollection<PointViewModel> Points
        {
            get { return m_points; }
        }

        public ObservableCollection<string> Colors
        {
            get { return m_colors; }
        }

        public string Title
        {
            get
            {
                return m_newModel ? "New Model" : "Edit Model";
            }
        }

        public PointViewModel SelectedPoint
        {
            get { return m_selectedPoint; }
            set
            {
                m_selectedPoint = value;
                OnPropertyChanged("SelectedPoint");
            }
        }

        public LineViewModel SelectedLine
        {
            get { return m_selectedLine; }
            set
            {
                m_selectedLine = value;
                OnPropertyChanged("SelectedLine");
            }
        }

        #endregion

        #region Commands

        public ICommand OKCommand
        {
            get
            {
                if (m_okCommand == null)
                {
                    m_okCommand = new DelegateCommand(Accept, CanAccept);
                }

                return m_okCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (m_cancelCommand == null)
                {
                    m_cancelCommand = new DelegateCommand(Cancel, CanCancel);
                }

                return m_cancelCommand;
            }
        }

        public ICommand AddLineCommand
        {
            get
            {
                if (m_addLineCommand == null)
                {
                    m_addLineCommand = new DelegateCommand(AddLine, CanAddLine);
                }

                return m_addLineCommand;
            }
        }

        public ICommand AddPointCommand
        {
            get
            {
                if (m_addPointCommand == null)
                {
                    m_addPointCommand = new DelegateCommand(AddPoint, CanAddPoint);
                }

                return m_addPointCommand;
            }
        }

        public ICommand DeleteLineCommand
        {
            get
            {
                if (m_deleteLineCommand == null)
                {
                    m_deleteLineCommand = new DelegateCommand(DeleteLine, CanDeleteLine);
                }

                return m_deleteLineCommand;
            }
        }

        public ICommand DeletePointCommand
        {
            get
            {
                if (m_deletePointCommand == null)
                {
                    m_deletePointCommand = new DelegateCommand(DeletePoint, CanDeletePoint);
                }

                return m_deletePointCommand;
            }
        }

        #endregion

        #region Event Handlers

        private void Accept()
        {
            RaiseCloseRequest(true);
        }

        private bool CanAccept()
        {
            return true;
        }

        private bool IsModelValid()
        {
            foreach (var point in m_points)
            {
                if (string.IsNullOrEmpty(point.Label))
                {
                    return false;
                }
            }

            foreach (var line in m_lines)
            {
                if (string.IsNullOrEmpty(line.Label) ||
                    line.StartPoint == null || line.EndPoint == null)
                {
                    return false;
                }
            }

            return true;
        }

        private void Cancel()
        {
            RaiseCloseRequest(false);
        }

        private bool CanCancel()
        {
            return true;
        }

        private void AddLine()
        {
            G3DLine line = new G3DLine();
            LineViewModel lvm = new LineViewModel(this, line);
            lvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(LineViewModel_PropertyChanged);
            m_lines.Add(lvm);
            OnPropertyChanged("Lines");
        }

        private bool CanAddLine()
        {
            return true;
        }

        private void AddPoint()
        {
            G3DPoint point = new G3DPoint();
            PointViewModel pvm = new PointViewModel(point);
            pvm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PointViewModel_PropertyChanged);
            m_points.Add(pvm);
            OnPropertyChanged("Points");
        }

        private bool CanAddPoint()
        {
            return true;
        }

        private void DeleteLine()
        {
            if (m_selectedLine != null)
            {
                m_lines.Remove(m_selectedLine);
                OnPropertyChanged("Lines");
            }
        }

        private bool CanDeleteLine()
        {
            return m_selectedLine != null;
        }

        private void DeletePoint()
        {
            if (m_selectedPoint != null)
            {
                m_points.Remove(m_selectedPoint);
                OnPropertyChanged("Points");
            }
        }

        private bool CanDeletePoint()
        {
            return m_selectedPoint != null;
        }

        private bool ExistsPoint(G3DPoint point)
        {
            foreach (var p in m_points)
            {
                if (point.Equals(p.Point))
                    return true;
            }

            return false;
        }

        #endregion
    }
}
