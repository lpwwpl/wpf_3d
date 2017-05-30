using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeometryViz3D.Model;
using System.Windows.Media.Media3D;

namespace GeometryViz3D.Views
{
    public partial class EditModelDialog : Form
    {
        G3DModel m_model;

        public EditModelDialog(G3DModel model)
        {
            m_model = model;

            InitializeComponent();
        }

        public G3DModel Model
        {
            get 
            { 
                return BuildModel(); 
            }
        }

        private void EditModelDialog_Load(object sender, EventArgs e)
        {
            dataGridViewLines.DataError += new DataGridViewDataErrorEventHandler(dataGridViewLines_DataError);
            FillPoints();
            FillLines();
        }

        void dataGridViewLines_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void FillPoints()
        {
            if (m_model != null)
            {
                foreach (var p in m_model.Points)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                    nameCell.Value = p.ID;
                    row.Cells.Add(nameCell);

                    AddCoordinateCell(row, p.Position.X);
                    AddCoordinateCell(row, p.Position.Y);
                    AddCoordinateCell(row, p.Position.Z);

                    if (!string.IsNullOrEmpty(p.Label))
                    {
                        DataGridViewTextBoxCell labelCell = new DataGridViewTextBoxCell();
                        labelCell.Value = p.Label;
                        row.Cells.Add(labelCell);
                    }

                    dataGridViewPoints.Rows.Add(row);
                }
            }
        }

        private static void AddCoordinateCell(DataGridViewRow row, double v)
        {
            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
            cell.Value = v.ToString();
            row.Cells.Add(cell);
        }

        private void FillLines()
        {
            LColumnColor.DataSource = G3DColors.Colors;

            if (m_model != null)
            {
                LColumnPoint1.DataSource = GetPointsInModel();
                LColumnPoint2.DataSource = GetPointsInModel();

                foreach (var l in m_model.Lines)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    InitializeRow(row, l);

                    dataGridViewLines.Rows.Add(row);
                }
            }
        }

        private List<string> GetPointsInModel()
        {
            var points = new List<string>();

            if (m_model != null)
            {
                foreach (var point in m_model.Points)
                {
                    points.Add(point.ID);
                }
            }

            return points;
        }

        private List<string> GetPointsInDataGrid()
        {
            var points = new List<string>();

            foreach (DataGridViewRow row in dataGridViewPoints.Rows)
            {
                object name = row.Cells[PColumnName.Index].Value;
                if (name != null)
                {
                    points.Add(name.ToString());
                }
            }

            return points;
        }

        private void dataGridViewPoints_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == PColumnName.Index)
            {
                LColumnPoint1.DataSource = GetPointsInDataGrid();
                LColumnPoint2.DataSource = GetPointsInDataGrid();
            }
        }

        private void dataGridViewPoints_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            LColumnPoint1.DataSource = GetPointsInDataGrid();
            LColumnPoint2.DataSource = GetPointsInDataGrid();
        }

        private void InitializeRow(DataGridViewRow row, G3DLine l)
        {
            DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
            nameCell.Value = l.ID;
            row.Cells.Add(nameCell);

            AddPointCell(row, l.StartPoint);
            AddPointCell(row, l.EndPoint);

            AddColorCell(row, l.Color);
        }

        private void AddPointCell(DataGridViewRow row, G3DPoint point)
        {
            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();

            cell.DataSource = m_model.Points;
            cell.ValueMember = "ID";
            cell.DisplayMember = "ID";

            cell.Value = point.ID;

            row.Cells.Add(cell);
        }

        private void AddColorCell(DataGridViewRow row, System.Windows.Media.Color color)
        {
            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();

            cell.DataSource = G3DColors.Colors;
            cell.Value = G3DColors.GetColorName(color.ToString());

            row.Cells.Add(cell);
        }

        private G3DModel BuildModel()
        {
            ModelBuilder builder = new ModelBuilder();

            foreach (DataGridViewRow row in dataGridViewPoints.Rows)
            {
                AddPoint(builder, row);
            }

            foreach (DataGridViewRow row in dataGridViewLines.Rows)
            {
                AddLine(builder, row);
            }

            return builder.Model;
        }

        private void AddLine(ModelBuilder builder, DataGridViewRow row)
        {
            object name = row.Cells[LColumnName.Index].Value;
            object p1 = row.Cells[LColumnPoint1.Index].Value;
            object p2 = row.Cells[LColumnPoint2.Index].Value;
            object color = row.Cells[LColumnColor.Index].Value;

            if (name != null && p1 != null && p2 != null)
            {
                builder.AddLine
                    (
                    name.ToString(),
                    p1.ToString(),
                    p2.ToString(),
                    color != null ? color.ToString() : null
                    );
            }
        }

        private void AddPoint(ModelBuilder builder, DataGridViewRow row)
        {
            object name = row.Cells[PColumnName.Index].Value;
            object x = row.Cells[PColumnX.Index].Value;
            object y = row.Cells[PColumnY.Index].Value;
            object z = row.Cells[PColumnZ.Index].Value;

            if (name != null && x != null && y != null && z != null)
            {
                object color = row.Cells[PColumnLabel.Index].Value;

                builder.AddPoint
                    (
                    name.ToString(),
                    x.ToString(),
                    y.ToString(),
                    z.ToString(),
                    color != null ? color.ToString() : null
                    );
            }
        }
    }
}
