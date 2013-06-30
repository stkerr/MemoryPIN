namespace MemoryPINGui
{
    partial class LibraryRebaseForm
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.libraryLoadLocationLabel = new System.Windows.Forms.Label();
            this.libraryOriginalLocationLabel = new System.Windows.Forms.Label();
            this.libraryLoadingLocationTextBox = new System.Windows.Forms.TextBox();
            this.libraryOriginalLocationTextBox = new System.Windows.Forms.TextBox();
            this.libraryNameListBox = new System.Windows.Forms.ListBox();
            this.libraryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.libraryUpdateButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.libraryBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.05082F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.94918F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 192F));
            this.tableLayoutPanel1.Controls.Add(this.libraryLoadLocationLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.libraryOriginalLocationLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.libraryLoadingLocationTextBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.libraryOriginalLocationTextBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.libraryNameListBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.libraryUpdateButton, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.statusLabel, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(767, 272);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // libraryLoadLocationLabel
            // 
            this.libraryLoadLocationLabel.AutoSize = true;
            this.libraryLoadLocationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryLoadLocationLabel.Location = new System.Drawing.Point(417, 0);
            this.libraryLoadLocationLabel.Name = "libraryLoadLocationLabel";
            this.libraryLoadLocationLabel.Size = new System.Drawing.Size(154, 122);
            this.libraryLoadLocationLabel.TabIndex = 0;
            this.libraryLoadLocationLabel.Text = "Library Loading Location";
            // 
            // libraryOriginalLocationLabel
            // 
            this.libraryOriginalLocationLabel.AutoSize = true;
            this.libraryOriginalLocationLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryOriginalLocationLabel.Location = new System.Drawing.Point(417, 122);
            this.libraryOriginalLocationLabel.Name = "libraryOriginalLocationLabel";
            this.libraryOriginalLocationLabel.Size = new System.Drawing.Size(154, 122);
            this.libraryOriginalLocationLabel.TabIndex = 1;
            this.libraryOriginalLocationLabel.Text = "Library Original Location";
            // 
            // libraryLoadingLocationTextBox
            // 
            this.libraryLoadingLocationTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryLoadingLocationTextBox.Location = new System.Drawing.Point(577, 3);
            this.libraryLoadingLocationTextBox.Name = "libraryLoadingLocationTextBox";
            this.libraryLoadingLocationTextBox.ReadOnly = true;
            this.libraryLoadingLocationTextBox.Size = new System.Drawing.Size(187, 20);
            this.libraryLoadingLocationTextBox.TabIndex = 2;
            // 
            // libraryOriginalLocationTextBox
            // 
            this.libraryOriginalLocationTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryOriginalLocationTextBox.Location = new System.Drawing.Point(577, 125);
            this.libraryOriginalLocationTextBox.Name = "libraryOriginalLocationTextBox";
            this.libraryOriginalLocationTextBox.Size = new System.Drawing.Size(187, 20);
            this.libraryOriginalLocationTextBox.TabIndex = 3;
            // 
            // libraryNameListBox
            // 
            this.libraryNameListBox.DataSource = this.libraryBindingSource;
            this.libraryNameListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryNameListBox.FormattingEnabled = true;
            this.libraryNameListBox.Location = new System.Drawing.Point(3, 3);
            this.libraryNameListBox.Name = "libraryNameListBox";
            this.tableLayoutPanel1.SetRowSpan(this.libraryNameListBox, 3);
            this.libraryNameListBox.Size = new System.Drawing.Size(408, 266);
            this.libraryNameListBox.TabIndex = 4;
            this.libraryNameListBox.SelectedValueChanged += new System.EventHandler(this.libraryNameListBox_SelectedValueChanged);
            // 
            // libraryUpdateButton
            // 
            this.libraryUpdateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryUpdateButton.Location = new System.Drawing.Point(577, 247);
            this.libraryUpdateButton.Name = "libraryUpdateButton";
            this.libraryUpdateButton.Size = new System.Drawing.Size(187, 22);
            this.libraryUpdateButton.TabIndex = 5;
            this.libraryUpdateButton.Text = "Update Original Location";
            this.libraryUpdateButton.UseVisualStyleBackColor = true;
            this.libraryUpdateButton.Click += new System.EventHandler(this.libraryUpdateButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusLabel.Location = new System.Drawing.Point(417, 244);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(154, 28);
            this.statusLabel.TabIndex = 6;
            // 
            // LibraryRebaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 272);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LibraryRebaseForm";
            this.Text = "LibraryRebaseForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.libraryBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label libraryLoadLocationLabel;
        private System.Windows.Forms.Label libraryOriginalLocationLabel;
        private System.Windows.Forms.TextBox libraryLoadingLocationTextBox;
        private System.Windows.Forms.TextBox libraryOriginalLocationTextBox;
        private System.Windows.Forms.ListBox libraryNameListBox;
        private System.Windows.Forms.Button libraryUpdateButton;
        private System.Windows.Forms.BindingSource libraryBindingSource;
        private System.Windows.Forms.Label statusLabel;
    }
}