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
    public partial class FormHiden : Form
    {
        public FormHiden()
        {
            InitializeComponent();
           
        }

        private void FormHiden_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (zeppMailer.Program.abort)
            //{
            //    e.Cancel = true;
            //    Hide();
            //}

            if (e.CloseReason == CloseReason.WindowsShutDown || !Program.abort)
            {
                this.Show();
                Thread.Sleep(1000);

                //MessageBox.Show("Form1_FormClosed turn");

            } else
            {
                //MessageBox.Show("Form1_FormClosed NOT turn");
                e.Cancel = true;
            }
        }

        private void FormHiden_Shown(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            Hide();
        }
               
    }
}
