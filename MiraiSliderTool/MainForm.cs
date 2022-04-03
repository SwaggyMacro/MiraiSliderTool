using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiraiSliderTool
{
    public partial class MainForm : Form
    {
        private BrowserForm browserForm;
        public MainForm()
        {
            InitializeComponent();
            
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            //browser.LoadUrl(textBox1.Text);
            if (browserForm == null )
            {
                browserForm = new BrowserForm();
                //browserForm.ShowDialog();
            }
            browserForm.Visible = true;
            browserForm.Focus();
            browserForm.LoadUrl(textBox1.Text);
           
        }

    }
}
