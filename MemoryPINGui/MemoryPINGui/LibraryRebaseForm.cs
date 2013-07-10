using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MemoryPINGui
{
    public partial class LibraryRebaseForm : Form
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(string lpLibFileName);

        private IList<Library> libraries;

        public IList<Library> Libraries
        {
            get { return libraries; }
            set { libraries = value; }
        }

        public LibraryRebaseForm(ref LibraryResultsProcessor processor)
        {
            InitializeComponent();
            this.Libraries = processor.Libraries;
            this.libraryBindingSource.DataSource = this.Libraries.Select(e => e.Name);
        }

        private void RebaseAllLibraries()
        {

        }

        private void libraryNameListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string name = (string)libraryNameListBox.SelectedValue;
            if (name == null)
                return;

            List<Library> libList = this.Libraries.Where(l => l.Name == name).ToList();
            Library lib = libList.First();
            libraryLoadingLocationTextBox.Text = lib.Loadaddress.ToString("X");
            libraryOriginalLocationTextBox.Text = lib.Originaladdress.ToString("X");
            statusLabel.Text = "";
        }

        private void libraryUpdateButton_Click(object sender, EventArgs e)
        {
            string name = (string)libraryNameListBox.SelectedValue;
            if (name == null)
                return;

            List<Library> libList = this.Libraries.Where(l => l.Name == name).ToList();
            Library lib = libList.First();
            try
            {
                lib.Originaladdress = (uint)Convert.ToInt32(libraryOriginalLocationTextBox.Text, 16);
                statusLabel.Text = "Updated!";
            }
            catch (FormatException ex_fmt)
            {
                statusLabel.Text = "Invalid entry!";
            }
            catch (OverflowException ex_of)
            {
            }
            
        }

    }
}
