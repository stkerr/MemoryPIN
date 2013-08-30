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
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr OpenEvent(int desiredAccess, bool inheritHandle, string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean ResetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetLastError();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CreateEvent(IntPtr lpEventAttributes, Boolean bManualReset, Boolean bInitialState, string lpName);

        string fileOfInterest;
        //string pinPath;

        IntPtr hMonitoringEvent;
        IntPtr hSnapshotEvent;

        InstructionProcessor processor;
        LibraryResultsProcessor libraryProcessor;

        Color[] colorBank;

        public Form1()
        {
            hMonitoringEvent = OpenEvent(0x000F0000 | 0x00100000 | 0x03, false, "MonitoringEvent"); // EVENT_ALL_ACCESS
            if (hMonitoringEvent.ToInt32() == 0)
            {
                int error = GetLastError();
                if (error == 2)
                {
                    // event is not created, let's make it
                    hMonitoringEvent = CreateEvent(IntPtr.Zero, true, false, "MonitoringEvent".ToString());
                }
            }
            hSnapshotEvent = OpenEvent(0x000F0000 | 0x00100000 | 0x03, false, "SnapshotEvent"); // EVENT_ALL_ACCESS
            if (hSnapshotEvent.ToInt32() == 0)
            {
                int error = GetLastError();
                if (error == 2)
                {
                    // event is not created, let's make it
                    hSnapshotEvent = CreateEvent(IntPtr.Zero, true, false, "SnapshotEvent".ToString());
                }
            }

            InitializeComponent();

            timer1.Interval = 500;
            timer1.Start();
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
                paramString += " -resultsfile " + resultsFileBox.Text + " ";
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
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid region parameters! Please enter hex values.");
                }

            }

            if (traceOnStartCheckbox.Checked)
            {
                // enable tracing before we start executing
                SetEvent(hMonitoringEvent);
            }

            paramString += " -- " + textBox1.Text;

            tabContainer.SelectTab(1); // switch to control tab

            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
            psi.Arguments = "/K " + pinPathBox.Text + " " + paramString;
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

        private void instructionTracingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (hMonitoringEvent == null)
            {
                return;
            }

        }

        private void snapshotButton_Click(object sender, EventArgs e)
        {
            if (hSnapshotEvent == null)
            {
                return;
            }

            SetEvent(hSnapshotEvent);

        }

        private void tracingStatusLabel_Paint(object sender, PaintEventArgs e)
        {
            if (hMonitoringEvent == null || hMonitoringEvent.ToInt32() == 0)
            {
                tracingStatusLabel.Text = "Tracing Status: Error Retrieving";
                return;
            }

            int result = WaitForSingleObject(hMonitoringEvent, 0);

            if (result == 0) // WAIT_OBJECT_0
            {
                tracingStatusLabel.Text = "Tracing Status: Enabled";
                startManualTracingButton.Text = "Disable Tracing";
            }
            else
            {
                tracingStatusLabel.Text = "Tracing Status: Disabled";
                startManualTracingButton.Text = "Enable Tracing";
            }

        }

        private void snapshotStatusLabel_Paint(object sender, PaintEventArgs e)
        {
            if (hSnapshotEvent == null || hSnapshotEvent.ToInt32() == 0)
            {
                snapshotStatusLabel.Text = "Snapshot Status: Error Retrieving";
                return;
            }

            int result = WaitForSingleObject(hSnapshotEvent, 0);

            if (result == 0) // WAIT_OBJECT_0
            {
                snapshotStatusLabel.Text = "Snapshot Status: Requested";
            }
            else
            {
                snapshotStatusLabel.Text = "Snapshot Status: Disabled";
            }
        }

        private void snapshotButton_Click_1(object sender, EventArgs e)
        {
            int result = WaitForSingleObject(hSnapshotEvent, 0);

            if (result == 0) // WAIT_OBJECT_0
            {
                // signaled, so reset
                ResetEvent(hSnapshotEvent);
                snapshotButton.Text = "Request Snapshot";
            }
            else
            {
                // not signaled, so set it
                SetEvent(hSnapshotEvent);
                snapshotButton.Text = "Revoke Snapshot Request";
            }
        }

        private void startManualTracingButton_Click(object sender, EventArgs e)
        {
            int result = WaitForSingleObject(hMonitoringEvent, 0);

            if (result == 0) // WAIT_OBJECT_0
            {
                // signaled, so reset
                ResetEvent(hMonitoringEvent);
            }
            else
            {
                // not signaled, so set it
                SetEvent(hMonitoringEvent);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void processInstructionFileButton_Click(object sender, EventArgs e)
        {
            libraryProcessor = new LibraryResultsProcessor("memorypin.txt");
            processor = new InstructionProcessor("instructionTrace.txt", libraryProcessor.Libraries);

            librariesBindingSource.DataSource = processor.Libraries.Select(x => x.Name);
            instructionBindingSource.DataSource = processor.Instructions;
            threadBindingSource.DataSource = processor.Threads;

            // remove the data source changed handler briefly, since it slows things down
            this.resultsGridView.DataSourceChanged -= new System.EventHandler(this.resultsGridView_DataSourceChanged);

            for (int i = 0; i < processor.Libraries.Count; i++)
            {
                loadedLibraryList.SetSelected(i, true); // select all the libraries initially
            }

            for (int i = 0; i < processor.Threads.Count; i++)
            {
                loadedThreadList.SetSelected(i, true); // select all threads initially
            }

            // re add the event handler for data source adds
            this.resultsGridView.DataSourceChanged += new System.EventHandler(this.resultsGridView_DataSourceChanged);
            
            // invoke the data source change handler since we were ignoring it before
            resultsGridView_DataSourceChanged(null, null);
        }

        private void resultsPageActive(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void resultsPageInactive(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void loadedLibraryList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (processor == null || processor.Libraries.Count == 0)
                return;

            // refresh all the filtered libraries
            processor.IncludedLibraries.RemoveAll(x => x != null);
            foreach (string item in loadedLibraryList.SelectedItems)
            {
                if (!processor.IncludedLibraries.Contains(item))
                    processor.IncludedLibraries.Add(item);
            }

            // update the data source
            resultsGridView.DataSource = processor.Instructions;
        }

        private void loadedThreadList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (processor == null || processor.Threads.Count == 0)
                return;

            // refresh all the filtered libraries
            processor.IncludedThreads.RemoveAll(x => x != 0);
            foreach (int item in loadedThreadList.SelectedItems)
            {
                if (!processor.IncludedThreads.Contains(item))
                    processor.IncludedThreads.Add(item);
            }

            // update the data source
            resultsGridView.DataSource = processor.Instructions; // refresh the instruction view
        }

        private void loadedLibraryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadedLibraryList.Items.Count == 0)
            {
                return;
            }
            return;
        }

        private void loadedLibraryList_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                // open some options on right click
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LibraryRebaseForm rebaseForm = new LibraryRebaseForm(ref libraryProcessor);
            rebaseForm.ShowDialog();
            resultsGridView.DataSource = processor.Instructions;
        }

        private void histogramData_Enter(object sender, EventArgs e)
        {
            if (libraryProcessor == null)
                libraryProcessor = new LibraryResultsProcessor("memorypin.txt");
            if (processor == null)
                processor = new InstructionProcessor("instructionTrace.txt", libraryProcessor.Libraries);

            IList<Instruction> instrs = processor.Instructions;


            Dictionary<uint, uint> histogram = new Dictionary<uint, uint>(); ; // <instruction address, count>


            foreach (Instruction i in instrs)
            {
                if (histogram.ContainsKey(i.Address) == false)
                {
                    histogram.Add(i.Address, 1);
                }
                else
                {
                    histogram[i.Address]++;
                }
            }

            List<HistogramEntry> convertedHistogram = new List<HistogramEntry>();
            foreach (uint key in histogram.Keys)
            {
                convertedHistogram.Add(new HistogramEntry(key, histogram[key]));
            }

            convertedHistogram.Sort();
            convertedHistogram.Reverse();

            histogramDataView.DataSource = convertedHistogram;
            histogramDataView.DefaultCellStyle.Format = "X08";


        }

        private Boolean RGBColorCompare(Color one, Color two)
        {
            return (one.R == two.R && one.B == two.B && one.G == two.G);
        }

        private void resultsGridView_DataSourceChanged(object sender, EventArgs e)
        {
            // apply the colors to the instructions
            for (int i = 0; i < resultsGridView.Rows.Count; i++)
            {     
                resultsGridView.Rows[i].DefaultCellStyle.BackColor = ((Instruction)(resultsGridView.Rows[i].DataBoundItem)).Color;
            }
        }

        private void depthFilterChanged(object sender, EventArgs e)
        {
            int highDepth;
            int lowDepth;
            if (!Int32.TryParse(highDepthTextBox.Text, out highDepth))
            {
                highDepthTextBox.Text = "Invalid numeric depth!";
                return;
            }

            if (!Int32.TryParse(lowDepthTextBox.Text, out lowDepth))
            {
                lowDepthTextBox.Text = "Invalid numeric depth!";
                return;
            }

            if (lowDepth > highDepth)
                return;

            processor.HighFilterDepth = highDepth;
            processor.LowFilterDepth = lowDepth;

            // update the data source
            resultsGridView.DataSource = processor.Instructions;
        }

    }
}