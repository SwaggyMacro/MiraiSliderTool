using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiraiSliderTool
{
    public partial class BrowserForm : Form
    {
        private ChromiumWebBrowser browser;
        public static BrowserForm form;
        
        public BrowserForm()
        {
            InitializeComponent();
            InitBrowser();
        }

        private void InitBrowser()
        {
            try
            {
                Cef.Initialize(new CefSettings());
                browser = new ChromiumWebBrowser();
                browser.Parent = panel1;
                browser.Dock = DockStyle.Fill;
                browser.RequestHandler = new CustomRequestHandler();

                form = this;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public void LoadUrl(string url)
        {
            browser.LoadUrl(url);
        }

        private void BrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }
        public static void showTicket(String ticket) {
            BrowserForm.form.BeginInvoke(new Action(() => { BrowserForm.form.Visible = false; }));
            DialogResult dra = MessageBox.Show("已完成滑块条验证码，是否复制内容？", "Tips", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dra == DialogResult.Yes) {
                DialogResult drb = MessageBox.Show("点击是仅复制Ticket，点击否复制原始内容", "Tips", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drb == DialogResult.Yes)
                {
                    JObject o= JObject.Parse(ticket);
                    try
                    {
                        Clipboard.SetText((string)o["ticket"]);
                    }
                    catch { 
                    }
                    
                }
                else if (drb == DialogResult.No) {
                    try
                    {
                        Clipboard.SetText(ticket);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
