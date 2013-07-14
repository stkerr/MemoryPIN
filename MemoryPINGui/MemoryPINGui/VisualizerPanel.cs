using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MemoryPINGui
{
    public partial class VisualizerPanel : Form
    {
        public VisualizerPanel()
        {
            InitializeComponent();
        }

        private void VisualizerPanel_Enter(object sender, EventArgs e)
        {

            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Glee.GraphViewerGdi.GViewer viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("graph");
            //create the graph content 
            for (int i = 0; i < 10000; i++)
            {
                graph.AddEdge(i.ToString(), (i + 1).ToString());
            }
            /*
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").EdgeAttr.Color = Microsoft.Glee.Drawing.Color.Green;
            graph.FindNode("A").Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.Fillcolor = Microsoft.Glee.Drawing.Color.MistyRose;
            
            Microsoft.Glee.Drawing.Node c = graph.FindNode("C");
            c.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.PaleGreen;
            c.Attr.Shape = Microsoft.Glee.Drawing.Shape.Diamond;
             */
            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(viewer);
            this.ResumeLayout();

/*            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.ShowDialog(); 
 */
        }
    }
}
