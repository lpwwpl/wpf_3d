namespace GeometryViz3D.Views
{
    partial class EditModelDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bnOK = new System.Windows.Forms.Button();
            this.bnCancel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewPoints = new System.Windows.Forms.DataGridView();
            this.PColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PColumnX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PColumnY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PColumnZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PColumnLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewLines = new System.Windows.Forms.DataGridView();
            this.LColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LColumnPoint1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LColumnPoint2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LColumnColor = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLines)).BeginInit();
            this.SuspendLayout();
            // 
            // bnOK
            // 
            this.bnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bnOK.Location = new System.Drawing.Point(515, 12);
            this.bnOK.Name = "bnOK";
            this.bnOK.Size = new System.Drawing.Size(75, 23);
            this.bnOK.TabIndex = 0;
            this.bnOK.Text = "OK";
            this.bnOK.UseVisualStyleBackColor = true;
            // 
            // bnCancel
            // 
            this.bnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bnCancel.Location = new System.Drawing.Point(515, 41);
            this.bnCancel.Name = "bnCancel";
            this.bnCancel.Size = new System.Drawing.Size(75, 23);
            this.bnCancel.TabIndex = 1;
            this.bnCancel.Text = "Cancel";
            this.bnCancel.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewPoints);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewLines);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(497, 344);
            this.splitContainer1.SplitterDistance = 165;
            this.splitContainer1.TabIndex = 2;
            // 
            // dataGridViewPoints
            // 
            this.dataGridViewPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPoints.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PColumnName,
            this.PColumnX,
            this.PColumnY,
            this.PColumnZ,
            this.PColumnLabel});
            this.dataGridViewPoints.Location = new System.Drawing.Point(7, 21);
            this.dataGridViewPoints.Name = "dataGridViewPoints";
            this.dataGridViewPoints.Size = new System.Drawing.Size(487, 141);
            this.dataGridViewPoints.TabIndex = 1;
            this.dataGridViewPoints.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPoints_CellValueChanged);
            this.dataGridViewPoints.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewPoints_RowsRemoved);
            // 
            // PColumnName
            // 
            this.PColumnName.HeaderText = "Name";
            this.PColumnName.Name = "PColumnName";
            // 
            // PColumnX
            // 
            this.PColumnX.HeaderText = "X";
            this.PColumnX.Name = "PColumnX";
            // 
            // PColumnY
            // 
            this.PColumnY.HeaderText = "Y";
            this.PColumnY.Name = "PColumnY";
            // 
            // PColumnZ
            // 
            this.PColumnZ.HeaderText = "Z";
            this.PColumnZ.Name = "PColumnZ";
            // 
            // PColumnLabel
            // 
            this.PColumnLabel.HeaderText = "Label";
            this.PColumnLabel.Name = "PColumnLabel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Points:";
            // 
            // dataGridViewLines
            // 
            this.dataGridViewLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LColumnName,
            this.LColumnPoint1,
            this.LColumnPoint2,
            this.LColumnColor});
            this.dataGridViewLines.Location = new System.Drawing.Point(7, 20);
            this.dataGridViewLines.Name = "dataGridViewLines";
            this.dataGridViewLines.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewLines.Size = new System.Drawing.Size(487, 152);
            this.dataGridViewLines.TabIndex = 1;
            // 
            // LColumnName
            // 
            this.LColumnName.HeaderText = "Name";
            this.LColumnName.Name = "LColumnName";
            // 
            // LColumnPoint1
            // 
            this.LColumnPoint1.HeaderText = "Point 1";
            this.LColumnPoint1.Name = "LColumnPoint1";
            this.LColumnPoint1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LColumnPoint1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LColumnPoint2
            // 
            this.LColumnPoint2.HeaderText = "Point 2";
            this.LColumnPoint2.Name = "LColumnPoint2";
            // 
            // LColumnColor
            // 
            this.LColumnColor.HeaderText = "Color";
            this.LColumnColor.Name = "LColumnColor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Lines:";
            // 
            // EditModelDialog
            // 
            this.AcceptButton = this.bnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bnCancel;
            this.ClientSize = new System.Drawing.Size(602, 368);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.bnCancel);
            this.Controls.Add(this.bnOK);
            this.Name = "EditModelDialog";
            this.Text = "Edit Model";
            this.Load += new System.EventHandler(this.EditModelDialog_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLines)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bnOK;
        private System.Windows.Forms.Button bnCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridViewPoints;
        private System.Windows.Forms.DataGridView dataGridViewLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn PColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PColumnX;
        private System.Windows.Forms.DataGridViewTextBoxColumn PColumnY;
        private System.Windows.Forms.DataGridViewTextBoxColumn PColumnZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn PColumnLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn LColumnName;
        private System.Windows.Forms.DataGridViewComboBoxColumn LColumnPoint1;
        private System.Windows.Forms.DataGridViewComboBoxColumn LColumnPoint2;
        private System.Windows.Forms.DataGridViewComboBoxColumn LColumnColor;
    }
}