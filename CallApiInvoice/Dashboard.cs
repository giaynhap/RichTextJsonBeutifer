using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CallApiInvoice
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            this.tabControl1.HandleCreated += tabControl1_HandleCreated;
         //   this.tabControl1.TabPages[0].Controls.Add(new Form1());
        }
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int TCM_SETMINTABWIDTH = 0x1300 + 49;
        private void tabControl1_HandleCreated(object sender, EventArgs e)
        {
            SendMessage(this.tabControl1.Handle, TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == this.tabControl1.TabCount - 1)
                e.Cancel = true;
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            TabControl tc = (TabControl)sender;
            var lastIndex = this.tabControl1.TabCount - 1;
            if (this.tabControl1.GetTabRect(lastIndex).Contains(e.Location))
            {
                var addedForm = new AddNew();
                if (addedForm.ShowDialog(this) == DialogResult.OK)
                {
                    var title = addedForm.resultValue;
                    try
                    {
                        var url = new Uri(addedForm.resultValue);
                        title = url.Segments[url.Segments.Count() - 1];
                    }
                    catch
                    {

                    }
                    var newForm = new Form1();
                     
                    newForm.setEndpoint(addedForm.resultValue);
                    this.tabControl1.TabPages.Insert(lastIndex, title);
                    this.tabControl1.TabPages[lastIndex].BackColor = Color.White;
                    this.tabControl1.TabPages[lastIndex].Controls.Add(newForm);
                    this.tabControl1.SelectedIndex = lastIndex;
                }

            }else
            {
                var xData = this.tabControl1.GetTabRect(tc.SelectedIndex);
                var pos = xData.Width - e.X+xData.X ;
                if (pos<16)
                {
                    this.tabControl1.Controls.RemoveAt(tc.SelectedIndex);
                }
            }
           
        }
    }
}
