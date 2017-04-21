using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace zeppMailer
{
    public partial class smsForm : Form
    {
        public smsForm()
        {
            InitializeComponent();
        }

        private void buttonSmsOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void buttonSmsOnlook_Click(object sender, EventArgs e)
        //{
        //    //if (Program.main == null || Program.main.IsDisposed)
        //    //{
        //    //    Program.main = new FormMain();
        //    //    Program.mainRun = new Thread(delegate() { Application.Run(Program.main); });
        //    //}

        //    //if (!Program.main.Visible) Program.main.Show();
        //    //else
        //    //{
        //    //    if (Program.main.WindowState == FormWindowState.Normal) Program.main.WindowState = FormWindowState.Minimized;
        //    //    else Program.main.WindowState = FormWindowState.Normal;
        //    //}
        //    Program.onjnMSG();
        //    this.Close();
        //}       
    }
}
