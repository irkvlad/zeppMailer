using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zeppMailer
{
    public partial class FormStrConect : Form
    {
        MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
        //MySql.Data.MySqlClient.MySqlCommand cmd;

        public FormStrConect()
        {
            InitializeComponent();
            this.labelSetHTTP.Text = global::zeppMailer.Properties.Settings.Default.http_zepp;
            string str = global::zeppMailer.Properties.Settings.Default.ConectToZepp;
            
            string[] strS = str.Split(';');
            if (strS.Length > 3)
            {
                strS[2] = "password=****";
                foreach (string s in strS)
                {
                    this.labelSetCon.Text += s + ";";
                }
            }
        }

        private void checkNewCon()
        {
            this.labelOK.Visible = false;
            this.labelERR.Visible = false;
            if (this.textBoxConStr.Text != "")
            {
                try
                {

                    mySqlConnectionSQLZepp.ConnectionString = this.textBoxConStr.Text;
                    if (Program.pingServer(mySqlConnectionSQLZepp)) this.labelOK.Visible = true;
                    else this.labelERR.Visible = true;

                }
                catch
                {
                    this.labelERR.Visible = true;
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkNewCon();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (true)
            {
                if (this.labelERR.Visible) return;
                if (this.labelOK.Visible)
                {
                    global::zeppMailer.Properties.Settings.Default.ConectToZepp = mySqlConnectionSQLZepp.ConnectionString;
                    global::zeppMailer.Properties.Settings.Default.Save();
                    MessageBox.Show("Для приминения настроек перезапустите программу");
                    zeppMailer.Program.abort = false;
                    zeppMailer.Program.hiden.Close();
                    Program.main.Close();
                    break;                    
                } else checkNewCon();
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.textBoxHTTP.Text != "")
                global::zeppMailer.Properties.Settings.Default.http_zepp = this.textBoxHTTP.Text;
            global::zeppMailer.Properties.Settings.Default.Save();
        }

        
    }
}
