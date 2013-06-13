namespace MemoryPINGui
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.regionMonitorCheckBox = new System.Windows.Forms.CheckBox();
            this.alternateResultsCheckbox = new System.Windows.Forms.CheckBox();
            this.instructionTracingCheckbox = new System.Windows.Forms.CheckBox();
            this.libraryLoadMonitoringCheckbox = new System.Windows.Forms.CheckBox();
            this.regionStartBox = new System.Windows.Forms.TextBox();
            this.regionEndBox = new System.Windows.Forms.TextBox();
            this.resultsFileBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.memoryPINDllPath = new System.Windows.Forms.TextBox();
            this.pinPathBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.startManualTracingButton = new System.Windows.Forms.Button();
            this.manualTracingCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(306, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(12, 291);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(493, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Execute PIN tool";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // regionMonitorCheckBox
            // 
            this.regionMonitorCheckBox.AutoSize = true;
            this.regionMonitorCheckBox.Location = new System.Drawing.Point(12, 126);
            this.regionMonitorCheckBox.Name = "regionMonitorCheckBox";
            this.regionMonitorCheckBox.Size = new System.Drawing.Size(112, 17);
            this.regionMonitorCheckBox.TabIndex = 3;
            this.regionMonitorCheckBox.Text = "Region Monitoring";
            this.regionMonitorCheckBox.UseVisualStyleBackColor = true;
            this.regionMonitorCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // alternateResultsCheckbox
            // 
            this.alternateResultsCheckbox.AutoSize = true;
            this.alternateResultsCheckbox.Location = new System.Drawing.Point(12, 153);
            this.alternateResultsCheckbox.Name = "alternateResultsCheckbox";
            this.alternateResultsCheckbox.Size = new System.Drawing.Size(125, 17);
            this.alternateResultsCheckbox.TabIndex = 6;
            this.alternateResultsCheckbox.Text = "Alternate Results File";
            this.alternateResultsCheckbox.UseVisualStyleBackColor = true;
            this.alternateResultsCheckbox.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // instructionTracingCheckbox
            // 
            this.instructionTracingCheckbox.AutoSize = true;
            this.instructionTracingCheckbox.Location = new System.Drawing.Point(12, 178);
            this.instructionTracingCheckbox.Name = "instructionTracingCheckbox";
            this.instructionTracingCheckbox.Size = new System.Drawing.Size(114, 17);
            this.instructionTracingCheckbox.TabIndex = 8;
            this.instructionTracingCheckbox.Text = "Instruction Tracing";
            this.instructionTracingCheckbox.UseVisualStyleBackColor = true;
            this.instructionTracingCheckbox.CheckedChanged += new System.EventHandler(this.instructionTracingCheckbox_CheckedChanged);
            // 
            // libraryLoadMonitoringCheckbox
            // 
            this.libraryLoadMonitoringCheckbox.AutoSize = true;
            this.libraryLoadMonitoringCheckbox.Location = new System.Drawing.Point(12, 201);
            this.libraryLoadMonitoringCheckbox.Name = "libraryLoadMonitoringCheckbox";
            this.libraryLoadMonitoringCheckbox.Size = new System.Drawing.Size(136, 17);
            this.libraryLoadMonitoringCheckbox.TabIndex = 11;
            this.libraryLoadMonitoringCheckbox.Text = "Library Load Monitoring";
            this.libraryLoadMonitoringCheckbox.UseVisualStyleBackColor = true;
            // 
            // regionStartBox
            // 
            this.regionStartBox.Enabled = false;
            this.regionStartBox.Location = new System.Drawing.Point(149, 123);
            this.regionStartBox.Name = "regionStartBox";
            this.regionStartBox.Size = new System.Drawing.Size(175, 20);
            this.regionStartBox.TabIndex = 4;
            // 
            // regionEndBox
            // 
            this.regionEndBox.Enabled = false;
            this.regionEndBox.Location = new System.Drawing.Point(330, 123);
            this.regionEndBox.Name = "regionEndBox";
            this.regionEndBox.Size = new System.Drawing.Size(175, 20);
            this.regionEndBox.TabIndex = 5;
            // 
            // resultsFileBox
            // 
            this.resultsFileBox.Enabled = false;
            this.resultsFileBox.Location = new System.Drawing.Point(149, 150);
            this.resultsFileBox.Name = "resultsFileBox";
            this.resultsFileBox.Size = new System.Drawing.Size(356, 20);
            this.resultsFileBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(324, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Executable to Instrument";
            // 
            // memoryPINDllPath
            // 
            this.memoryPINDllPath.AllowDrop = true;
            this.memoryPINDllPath.Location = new System.Drawing.Point(12, 38);
            this.memoryPINDllPath.Name = "memoryPINDllPath";
            this.memoryPINDllPath.Size = new System.Drawing.Size(306, 20);
            this.memoryPINDllPath.TabIndex = 1;
            this.memoryPINDllPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.memorypinBox_DragDrop);
            this.memoryPINDllPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.memorypinBox_DragEnter);
            // 
            // pinPathBox
            // 
            this.pinPathBox.AllowDrop = true;
            this.pinPathBox.Location = new System.Drawing.Point(12, 64);
            this.pinPathBox.Name = "pinPathBox";
            this.pinPathBox.Size = new System.Drawing.Size(306, 20);
            this.pinPathBox.TabIndex = 2;
            this.pinPathBox.Text = "pin";
            this.pinPathBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.pinpath_DragDrop);
            this.pinPathBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.pinpath_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "MemoryPIN DLL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(327, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Intel PIN Executable";
            // 
            // startManualTracingButton
            // 
            this.startManualTracingButton.Enabled = false;
            this.startManualTracingButton.Location = new System.Drawing.Point(335, 172);
            this.startManualTracingButton.Name = "startManualTracingButton";
            this.startManualTracingButton.Size = new System.Drawing.Size(170, 23);
            this.startManualTracingButton.TabIndex = 10;
            this.startManualTracingButton.Text = "Enable Manual Tracing";
            this.startManualTracingButton.UseVisualStyleBackColor = true;
            this.startManualTracingButton.Click += new System.EventHandler(this.startManualTracingButton_Click);
            // 
            // manualTracingCheckbox
            // 
            this.manualTracingCheckbox.AutoSize = true;
            this.manualTracingCheckbox.Enabled = false;
            this.manualTracingCheckbox.Location = new System.Drawing.Point(149, 176);
            this.manualTracingCheckbox.Name = "manualTracingCheckbox";
            this.manualTracingCheckbox.Size = new System.Drawing.Size(155, 17);
            this.manualTracingCheckbox.TabIndex = 9;
            this.manualTracingCheckbox.Text = "Manual Tracing Start Mode";
            this.manualTracingCheckbox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 326);
            this.Controls.Add(this.manualTracingCheckbox);
            this.Controls.Add(this.startManualTracingButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resultsFileBox);
            this.Controls.Add(this.regionEndBox);
            this.Controls.Add(this.regionStartBox);
            this.Controls.Add(this.libraryLoadMonitoringCheckbox);
            this.Controls.Add(this.instructionTracingCheckbox);
            this.Controls.Add(this.alternateResultsCheckbox);
            this.Controls.Add(this.regionMonitorCheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pinPathBox);
            this.Controls.Add(this.memoryPINDllPath);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox regionMonitorCheckBox;
        private System.Windows.Forms.CheckBox alternateResultsCheckbox;
        private System.Windows.Forms.CheckBox instructionTracingCheckbox;
        private System.Windows.Forms.CheckBox libraryLoadMonitoringCheckbox;
        private System.Windows.Forms.TextBox regionStartBox;
        private System.Windows.Forms.TextBox regionEndBox;
        private System.Windows.Forms.TextBox resultsFileBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox memoryPINDllPath;
        private System.Windows.Forms.TextBox pinPathBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button startManualTracingButton;
        private System.Windows.Forms.CheckBox manualTracingCheckbox;
    }
}

