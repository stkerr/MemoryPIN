﻿namespace MemoryPINGui
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
            this.components = new System.ComponentModel.Container();
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
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.snapshotStatusLabel = new System.Windows.Forms.Label();
            this.tracingStatusLabel = new System.Windows.Forms.Label();
            this.startManualTracingButton = new System.Windows.Forms.Button();
            this.snapshotButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.processLibraryLoadButton = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabContainer.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(13, 12);
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
            this.button1.Location = new System.Drawing.Point(6, 312);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(532, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Execute PIN tool";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // regionMonitorCheckBox
            // 
            this.regionMonitorCheckBox.AutoSize = true;
            this.regionMonitorCheckBox.Location = new System.Drawing.Point(13, 101);
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
            this.alternateResultsCheckbox.Location = new System.Drawing.Point(13, 128);
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
            this.instructionTracingCheckbox.Location = new System.Drawing.Point(13, 153);
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
            this.libraryLoadMonitoringCheckbox.Location = new System.Drawing.Point(13, 176);
            this.libraryLoadMonitoringCheckbox.Name = "libraryLoadMonitoringCheckbox";
            this.libraryLoadMonitoringCheckbox.Size = new System.Drawing.Size(136, 17);
            this.libraryLoadMonitoringCheckbox.TabIndex = 11;
            this.libraryLoadMonitoringCheckbox.Text = "Library Load Monitoring";
            this.libraryLoadMonitoringCheckbox.UseVisualStyleBackColor = true;
            // 
            // regionStartBox
            // 
            this.regionStartBox.Enabled = false;
            this.regionStartBox.Location = new System.Drawing.Point(150, 98);
            this.regionStartBox.Name = "regionStartBox";
            this.regionStartBox.Size = new System.Drawing.Size(175, 20);
            this.regionStartBox.TabIndex = 4;
            // 
            // regionEndBox
            // 
            this.regionEndBox.Enabled = false;
            this.regionEndBox.Location = new System.Drawing.Point(331, 98);
            this.regionEndBox.Name = "regionEndBox";
            this.regionEndBox.Size = new System.Drawing.Size(175, 20);
            this.regionEndBox.TabIndex = 5;
            // 
            // resultsFileBox
            // 
            this.resultsFileBox.Enabled = false;
            this.resultsFileBox.Location = new System.Drawing.Point(150, 125);
            this.resultsFileBox.Name = "resultsFileBox";
            this.resultsFileBox.Size = new System.Drawing.Size(356, 20);
            this.resultsFileBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(325, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Executable to Instrument";
            // 
            // memoryPINDllPath
            // 
            this.memoryPINDllPath.AllowDrop = true;
            this.memoryPINDllPath.Location = new System.Drawing.Point(13, 38);
            this.memoryPINDllPath.Name = "memoryPINDllPath";
            this.memoryPINDllPath.Size = new System.Drawing.Size(306, 20);
            this.memoryPINDllPath.TabIndex = 1;
            this.memoryPINDllPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.memorypinBox_DragDrop);
            this.memoryPINDllPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.memorypinBox_DragEnter);
            // 
            // pinPathBox
            // 
            this.pinPathBox.AllowDrop = true;
            this.pinPathBox.Location = new System.Drawing.Point(13, 64);
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
            this.label2.Location = new System.Drawing.Point(326, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "MemoryPIN DLL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(328, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Intel PIN Executable";
            // 
            // tabContainer
            // 
            this.tabContainer.Controls.Add(this.tabPage1);
            this.tabContainer.Controls.Add(this.tabPage2);
            this.tabContainer.Controls.Add(this.tabPage3);
            this.tabContainer.Location = new System.Drawing.Point(12, 12);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabContainer.SelectedIndex = 0;
            this.tabContainer.Size = new System.Drawing.Size(552, 367);
            this.tabContainer.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.memoryPINDllPath);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.pinPathBox);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.regionMonitorCheckBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.alternateResultsCheckbox);
            this.tabPage1.Controls.Add(this.resultsFileBox);
            this.tabPage1.Controls.Add(this.instructionTracingCheckbox);
            this.tabPage1.Controls.Add(this.regionEndBox);
            this.tabPage1.Controls.Add(this.libraryLoadMonitoringCheckbox);
            this.tabPage1.Controls.Add(this.regionStartBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(544, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Configuration Page";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(150, 153);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(190, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Enable Instruction Tracing on Start";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.snapshotStatusLabel);
            this.tabPage2.Controls.Add(this.tracingStatusLabel);
            this.tabPage2.Controls.Add(this.startManualTracingButton);
            this.tabPage2.Controls.Add(this.snapshotButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(544, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Control Page";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // snapshotStatusLabel
            // 
            this.snapshotStatusLabel.AutoSize = true;
            this.snapshotStatusLabel.Location = new System.Drawing.Point(179, 40);
            this.snapshotStatusLabel.Name = "snapshotStatusLabel";
            this.snapshotStatusLabel.Size = new System.Drawing.Size(91, 13);
            this.snapshotStatusLabel.TabIndex = 17;
            this.snapshotStatusLabel.Text = "Snapshot Status: ";
            this.snapshotStatusLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.snapshotStatusLabel_Paint);
            // 
            // tracingStatusLabel
            // 
            this.tracingStatusLabel.AutoSize = true;
            this.tracingStatusLabel.Location = new System.Drawing.Point(179, 11);
            this.tracingStatusLabel.Name = "tracingStatusLabel";
            this.tracingStatusLabel.Size = new System.Drawing.Size(82, 13);
            this.tracingStatusLabel.TabIndex = 16;
            this.tracingStatusLabel.Text = "Tracing Status: ";
            this.tracingStatusLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.tracingStatusLabel_Paint);
            // 
            // startManualTracingButton
            // 
            this.startManualTracingButton.Location = new System.Drawing.Point(3, 6);
            this.startManualTracingButton.Name = "startManualTracingButton";
            this.startManualTracingButton.Size = new System.Drawing.Size(170, 23);
            this.startManualTracingButton.TabIndex = 15;
            this.startManualTracingButton.Text = "Enable Tracing";
            this.startManualTracingButton.UseVisualStyleBackColor = true;
            this.startManualTracingButton.Click += new System.EventHandler(this.startManualTracingButton_Click);
            // 
            // snapshotButton
            // 
            this.snapshotButton.Location = new System.Drawing.Point(3, 35);
            this.snapshotButton.Name = "snapshotButton";
            this.snapshotButton.Size = new System.Drawing.Size(170, 23);
            this.snapshotButton.TabIndex = 14;
            this.snapshotButton.Text = "Signal Snapshot";
            this.snapshotButton.UseVisualStyleBackColor = true;
            this.snapshotButton.Click += new System.EventHandler(this.snapshotButton_Click_1);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.processLibraryLoadButton);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(544, 341);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Results Page";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // processLibraryLoadButton
            // 
            this.processLibraryLoadButton.Location = new System.Drawing.Point(4, 7);
            this.processLibraryLoadButton.Name = "processLibraryLoadButton";
            this.processLibraryLoadButton.Size = new System.Drawing.Size(149, 23);
            this.processLibraryLoadButton.TabIndex = 0;
            this.processLibraryLoadButton.Text = "Process Library Loads";
            this.processLibraryLoadButton.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(4, 83);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(534, 252);
            this.textBox2.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 392);
            this.Controls.Add(this.tabContainer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabContainer.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button startManualTracingButton;
        private System.Windows.Forms.Button snapshotButton;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label snapshotStatusLabel;
        private System.Windows.Forms.Label tracingStatusLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button processLibraryLoadButton;
    }
}
