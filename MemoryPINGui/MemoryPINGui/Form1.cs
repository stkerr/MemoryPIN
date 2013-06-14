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
using System.Runtime.InteropServices;

namespace MemoryPINGui
{
    public partial class Form1 : Form
    {
        string fileOfInterest;
        string pinPath;
        bool manualTracing = false;

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
            string paramString = "-t " + memoryPINDllPath.Text + " ";
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
                paramString += " -instructionTrace ";
            }
            if (regionMonitorCheckBox.Checked)
            {
                try
                {
                    uint regionStart = Convert.ToUInt32(regionStartBox.Text, 16);
                    uint regionEnd = Convert.ToUInt32(regionEndBox.Text, 16);
                    paramString += " -monitorRegion -startRegion " + regionStart + " -endRegion " + regionEnd + " ";
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Invalid region parameters! Please enter hex values.");
                }
                
            }

            paramString += " -- " + textBox1.Text;

            ProcessStartInfo psi = new ProcessStartInfo(pinPathBox.Text);
            psi.Arguments = paramString;
            psi.UseShellExecute = false;
            Process.Start(psi);

            /* Fix up the manual tracing button (if applicable) */
            if (manualTracingCheckbox.Checked == true)
            {
                startManualTracingButton.Enabled = true;
            }
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

        private void instructionTracingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            manualTracingCheckbox.Enabled = true;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr OpenEvent(int desiredAccess, bool inheritHandle, string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean ResetEvent(IntPtr hEvent);

        private void startManualTracingButton_Click(object sender, EventArgs e)
        {
            IntPtr eventHandle = OpenEvent(0x000F0000 | 0x00100000 | 0x03, false, "MonitoringEvent"); // EVENT_ALL_ACCESS

            if (eventHandle == null)
            {
                return;
            }

            if (manualTracing == false)
            {
                SetEvent(eventHandle);
                startManualTracingButton.Text = "Disable Manual Tracing";
            }
            else
            {
                ResetEvent(eventHandle);
                startManualTracingButton.Text = "Enable Manual Tracing";
            }

            manualTracing = !manualTracing;
            
        }
    }
}
