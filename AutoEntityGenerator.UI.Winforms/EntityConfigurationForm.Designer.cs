namespace AutoEntityGenerator.UI
{
    partial class EntityConfigurationForm
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
            this.MappingDirection = new System.Windows.Forms.ComboBox();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.Browes = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Ok = new System.Windows.Forms.Button();
            this.DeselectAll = new System.Windows.Forms.Button();
            this.SelectAll = new System.Windows.Forms.Button();
            this.Properties = new System.Windows.Forms.DataGridView();
            this.DestinationFolder = new System.Windows.Forms.TextBox();
            this.GeneratedFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.DTOName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // MappingDirection
            // 
            this.MappingDirection.FormattingEnabled = true;
            this.MappingDirection.Items.AddRange(new object[] {
            "From DTO To Model",
            "From Model To DTO"});
            this.MappingDirection.Location = new System.Drawing.Point(12, 29);
            this.MappingDirection.Name = "MappingDirection";
            this.MappingDirection.Size = new System.Drawing.Size(308, 21);
            this.MappingDirection.TabIndex = 22;
            // 
            // Check
            // 
            this.Check.HeaderText = "";
            this.Check.Name = "Check";
            this.Check.Width = 30;
            // 
            // PropertyName
            // 
            this.PropertyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PropertyName.HeaderText = "Property Name";
            this.PropertyName.Name = "PropertyName";
            this.PropertyName.ReadOnly = true;
            // 
            // Browes
            // 
            this.Browes.Location = new System.Drawing.Point(245, 193);
            this.Browes.Name = "Browes";
            this.Browes.Size = new System.Drawing.Size(75, 23);
            this.Browes.TabIndex = 21;
            this.Browes.Text = "Browes";
            this.Browes.UseVisualStyleBackColor = true;
            this.Browes.Click += new System.EventHandler(this.Browes_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(12, 459);
            this.Cancel.Margin = new System.Windows.Forms.Padding(2);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(80, 20);
            this.Cancel.TabIndex = 17;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(334, 459);
            this.Ok.Margin = new System.Windows.Forms.Padding(2);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(80, 20);
            this.Ok.TabIndex = 18;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // DeselectAll
            // 
            this.DeselectAll.Location = new System.Drawing.Point(334, 345);
            this.DeselectAll.Margin = new System.Windows.Forms.Padding(2);
            this.DeselectAll.Name = "DeselectAll";
            this.DeselectAll.Size = new System.Drawing.Size(80, 20);
            this.DeselectAll.TabIndex = 19;
            this.DeselectAll.Text = "Deselect All";
            this.DeselectAll.UseVisualStyleBackColor = true;
            this.DeselectAll.Click += new System.EventHandler(this.DeselectAll_Click);
            // 
            // SelectAll
            // 
            this.SelectAll.Location = new System.Drawing.Point(334, 320);
            this.SelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(80, 20);
            this.SelectAll.TabIndex = 20;
            this.SelectAll.Text = "Select All";
            this.SelectAll.UseVisualStyleBackColor = true;
            this.SelectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // Properties
            // 
            this.Properties.AllowUserToAddRows = false;
            this.Properties.AllowUserToDeleteRows = false;
            this.Properties.AllowUserToResizeColumns = false;
            this.Properties.AllowUserToResizeRows = false;
            this.Properties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Properties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.PropertyName});
            this.Properties.Location = new System.Drawing.Point(12, 242);
            this.Properties.Margin = new System.Windows.Forms.Padding(2);
            this.Properties.Name = "Properties";
            this.Properties.RowHeadersVisible = false;
            this.Properties.Size = new System.Drawing.Size(308, 197);
            this.Properties.TabIndex = 16;
            // 
            // DestinationFolder
            // 
            this.DestinationFolder.Location = new System.Drawing.Point(13, 165);
            this.DestinationFolder.Margin = new System.Windows.Forms.Padding(2);
            this.DestinationFolder.Name = "DestinationFolder";
            this.DestinationFolder.Size = new System.Drawing.Size(308, 20);
            this.DestinationFolder.TabIndex = 15;
            // 
            // GeneratedFileName
            // 
            this.GeneratedFileName.AcceptsReturn = true;
            this.GeneratedFileName.Location = new System.Drawing.Point(12, 120);
            this.GeneratedFileName.Margin = new System.Windows.Forms.Padding(2);
            this.GeneratedFileName.Name = "GeneratedFileName";
            this.GeneratedFileName.Size = new System.Drawing.Size(308, 20);
            this.GeneratedFileName.TabIndex = 13;
            this.GeneratedFileName.Leave += new System.EventHandler(this.GeneratedFileName_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 220);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Select properties to include:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 147);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Destination folder:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Generated file name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 12);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Mapping direction:";
            // 
            // DTOName
            // 
            this.DTOName.Location = new System.Drawing.Point(12, 75);
            this.DTOName.Margin = new System.Windows.Forms.Padding(2);
            this.DTOName.Name = "DTOName";
            this.DTOName.Size = new System.Drawing.Size(308, 20);
            this.DTOName.TabIndex = 14;
            this.DTOName.TextChanged += new System.EventHandler(this.DTOName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "DTO name:";
            // 
            // ConfigureEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 492);
            this.Controls.Add(this.MappingDirection);
            this.Controls.Add(this.Browes);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.DeselectAll);
            this.Controls.Add(this.SelectAll);
            this.Controls.Add(this.Properties);
            this.Controls.Add(this.DestinationFolder);
            this.Controls.Add(this.GeneratedFileName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.DTOName);
            this.Controls.Add(this.label1);
            this.Name = "ConfigureEntity";
            this.Text = "ConfigureEntity";
            ((System.ComponentModel.ISupportInitialize)(this.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MappingDirection;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyName;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button Browes;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button DeselectAll;
        private System.Windows.Forms.Button SelectAll;
        private System.Windows.Forms.DataGridView Properties;
        private System.Windows.Forms.TextBox DestinationFolder;
        private System.Windows.Forms.TextBox GeneratedFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox DTOName;
        private System.Windows.Forms.Label label1;
    }
}