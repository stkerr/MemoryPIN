using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Specialized;

namespace MemoryPINGui
{
    public partial class Form1 : Form
    {
        string fileOfInterest;
        string pinPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effect = DragDropEffects.Copy;
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] droppedFilename = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            fileOfInterest = droppedFilename[0];
            textBox1.Text = droppedFilename[0];
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            regionStartBox.Enabled = !regionStartBox.Enabled;
            regionEndBox.Enabled = !regionEndBox.Enabled;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            resultsFileBox.Enabled = !resultsFileBox.Enabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string paramString = "-t " + memoryPINDllPath.Text;
            if (alternateResultsCheckbox.Checked)
            {
                paramString += " -resultsfile " + alternateResultsCheckbox.Text + " ";
            }
            if (libraryLoadMonitoringCheckbox.Checked)
            {
                paramString += " -libraryLoadTrace ";
            }
            if (instructionTracingCheckbox.Checked)
            {
                paramString += "-instructionTrace ";
            }
            if (regionMonitorCheckBox.Checked)
            {
                try
                {
                    uint regionStart;
                    uint regionEnd;
                    if (UInt32.TryParse(regionStartBox.Text, out regionStart) == true)
                    {
                        if (UInt32.TryParse(regionStartBox.Text, out regionEnd) == true)
                        {
                            paramString += " -monitorRegion " + regionStart + " " + regionEnd + " ";
                        }
                    }
                }
                catch(Exception ex)
                {
                }
                
            }

            paramString += " -- " + textBox1.Text;

            ProcessStartInfo psi = new ProcessStartInfo(pinPathBox.Text);
            psi.Arguments = paramString;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }

        private void memorypinBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] droppedFilename = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            memoryPINDllPath.Text = droppedFilename[0];
        }

        private void memorypinBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effect = DragDropEffects.Copy;
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void pinpath_DragDrop(object sender, DragEventArgs e)
        {
            string[] droppedFilename = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            memoryPINDllPath.Text = droppedFilename[0];
        }

        private void pinpath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effect = DragDropEffects.Copy;
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
