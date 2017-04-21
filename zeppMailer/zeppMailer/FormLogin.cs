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
    public partial class FormLogin : Form
    {
        
        public FormLogin()
        {
            InitializeComponent();
        }

        private void buttonEsc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {           
            string userMD5;
            int lastUser_id = zeppMailer.Program.user.user_id;
            string lastUser_name = zeppMailer.Program.user.user_name;
            string strSQL="";
            MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp = zeppMailer.Program.mySqlConnectionSQLZepp;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);

 // Вызов Функции авторизации
            if (zeppMailer.Program.user.chekPassword(mySqlConnectionSQLZepp, this.textBoxLogin.Text, this.maskedTextBox.Text))
            {
                this.label1.Text = "Вы аторизованы в системе как : " + zeppMailer.Program.user.user_name;
                zeppMailer.Program.main.Text = "ZeppMesendger                  Сообщения для :   " + zeppMailer.Program.user.user_name;
                MessageBox.Show("Здравствуйте " + zeppMailer.Program.user.user_name + ". \n Вы авторизованы!");
 // Привязать комп к учетке
                if (this.checkBoxSave.Checked)
                {
                    DialogResult result = System.Windows.Forms.DialogResult.No;
  // Проверить првязана ли уже запись
                    if (lastUser_id != 114)
                    {
                        result = MessageBox.Show(
                        "Этот компьютер уже привязан к учетной записи: " + lastUser_name +
                        ". \n Вы хотите изменить привязку к учетной записи:" + zeppMailer.Program.user.user_name +
                        "\n Сохранить изменения?", "", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
 // Обнулить предыдущую привязку 
                            strSQL = "UPDATE jos_users SET domen = '' , user = '' , comp = '' , pochta_chek = 1 " +
                                " WHERE id = " + lastUser_id + " ;";
                            try
                            {
                                cmd.CommandText = strSQL;
                                mySqlConnectionSQLZepp.Open();
                                int intRecordsAffected = cmd.ExecuteNonQuery();
                                mySqlConnectionSQLZepp.Close();
                                if (intRecordsAffected < 1)
                                {
 //ERR 003
                                    MessageBox.Show("Ошибка при обновлении записи! (  FormLogin 003)\n Вход выполнен");
                                    this.Close(); 
                                }

                            }
                            catch
                            {
//ERR 004
                                MessageBox.Show("Ошибка при обновлении записи! ( FormLogin 004)\n Вход выполнен");
                                mySqlConnectionSQLZepp.Close();
                                this.Close(); 
                            }

                        }
                        else
                        {
                            MessageBox.Show(" Вход выполнен, првязка компьютера  к учетной записи осталась прежней!");
                            this.Close(); 
                        }
                    }
                        

                    userMD5 =zeppMailer.Program.user.hashGen(
                    zeppMailer.Program.user.user, //Логин
                    zeppMailer.Program.user.comp); //Комп
 // Привязывем комп к учетке
                     strSQL = "UPDATE jos_users SET domen = '"+zeppMailer.Program.user.domen+
                        "' , user = '"+userMD5+
                        "' , comp = '"+zeppMailer.Program.user.comp+"' "+
                        " , pochta_chek = " + 0 +
                        " WHERE id = "+ zeppMailer.Program.user.user_id+" ;";
                    try
                    {
                        cmd.CommandText  = strSQL;
                        mySqlConnectionSQLZepp.Open();
                        int intRecordsAffected = cmd.ExecuteNonQuery();
                        mySqlConnectionSQLZepp.Close();
                        if (intRecordsAffected < 1)
                        {
                            MessageBox.Show("Теперь вы будете авторизовываться автоматически!");
                        }

                    }
                    catch
                    {
//ERR 002
                        MessageBox.Show("Ошибка при обновлении записи! ( FormLogin ERR002)");
                        mySqlConnectionSQLZepp.Close();
                    }

                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Или ЛОГИН или ПАРОЛЬ не верны!!");
            }
            

        }

       
        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}
