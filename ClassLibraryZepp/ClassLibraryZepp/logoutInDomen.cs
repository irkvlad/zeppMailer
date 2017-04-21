using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace LibZepp
{
    public delegate void logoutEventChegeUser(object t, LibZepp.logoutEvent.logoutEventArgs e);
    public class logoutEvent // Обработчик события обновления
    {
        public class logoutEventArgs : EventArgs
        {
            public bool chegeUser { get; set; }
        }

        public event logoutEventChegeUser UserChege;
        
        // Этот метод вызывается для запуска события, 
        public void UserCheger()
        {
            logoutEventArgs e = new logoutEventArgs();
            e.chegeUser = true;

            if (UserChege != null)
                UserChege(this, e);

            //l_evt.UserCheger(); - запуск события
            // LibZepp.l_evt.UserChege += {Функция Обработчик}; - подписка на событие
        }
    }  

    public class logoutInDomen
    {
        public static logoutEvent l_evt = new logoutEvent();

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        public string domen = "";


        /// <summary>
        /// Имя под которым вошли в систему.
        /// </summary>
        public string user = "";

        /// <summary>
        /// Имя копъютера.
        /// </summary>
        public string comp = "";


        /// <summary>
        /// ID пользователя зарегистрированный на сервере.
        /// Устанавливается после проверки на сервере.
        /// </summary>
        public int user_id = 114;



        /// <summary>
        /// Права доступа пользователя зарегистрированные на сервере.
        /// /// Устанавливается после проверки на сервере.
        /// </summary>
        public int gid = 0;

        /// <summary>
        /// Почтовые сообщения.
        /// /// Устанавливается после проверки на сервере.
        /// </summary>
        public int pochta_chek = 0;



        /// <summary>
        /// ИМЯ пользователя зарегистрированное на сервере.
        /// Устанавливается после проверки на сервере.
        /// </summary>
        public string user_name = "";

        /// <summary>
        /// Должность
        /// Устанавливается после проверки на сервере.
        /// </summary>
        public int category_id = 0;

        /// <summary>
        /// Компания
        /// Устанавливается на стороне.
        /// </summary>
        public int company_id = 0;


        /// <summary>
        /// Конструктор. 
        /// Поля : domen, user , comp , user_name , user_id .        
        /// </summary>
        public logoutInDomen(MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp)
        {
            domen = Environment.UserDomainName.ToString();
            user = Environment.UserName;
            comp = Environment.MachineName;
            user_name = "НЕ ОПРЕДЕЛЕН";
            gid = 0;
            category_id = 0;
            pochta_chek = 0;
            user_id = chekUser(mySqlConnectionSQLZepp);
            User.userSet = this;
            User.mySqlConnectionSQLZepp = mySqlConnectionSQLZepp;
        }

        public logoutInDomen()
        {
            domen = "";//Environment.UserDomainName.ToString();
            user = "";//Environment.UserName;
            comp = "";// Environment.MachineName;
            user_name = "НЕ ОПРЕДЕЛЕН";
            gid = 0;
            category_id = 0;
            pochta_chek = 0;
            user_id = 114; //chekUser(mySqlConnectionSQLZepp);
            User.userSet = this;                      
        }

        /// <summary>
        /// Сравнивает текущее доменое имя пользователя, текущее имя входа в систему, имя компъютера с 
        /// данными на сервере в таблице jos_zepp_users. Поля : domen , user , comp , id , name.
        /// Если текущие установки не совпадают ни сождной записью то пользователю присвается значение:
        /// user_id = 114(id), user_name = "НЕ УСТАНОВЛЕН"(name)
        /// </summary>
        public int chekUser(MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp )
        {
            string passwordMD5;
            int chek = 114;

 //Шифрование данных
            passwordMD5 = hashGen(user, comp);

            string strSQL = "SELECT domen , user , comp , id , gid , name , pochta_chek FROM jos_users WHERE comp = '" + comp + "' ;";
            MySql.Data.MySqlClient.MySqlCommand cmd;
            cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);
           
            try
            {
                mySqlConnectionSQLZepp.Open();
                MySql.Data.MySqlClient.MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr["domen"].ToString() == domen &&
                        rdr["user"].ToString() == passwordMD5 && // Зашифровано
                        rdr["comp"].ToString() == comp)
                    {
                        user_id=chek = (int)rdr["id"];
                        user_name = (string)rdr["name"];
                        gid = (byte)(rdr["gid"]);
                        pochta_chek = (int)rdr["pochta_chek"];
                        mySqlConnectionSQLZepp.Close();

 // Устанавливаем должность
                        mySqlConnectionSQLZepp.Open();
                        cmd.CommandText = "SELECT group_id FROM jos_projectlog_groups_mid WHERE user_id = " + user_id + " ;";
                        MySql.Data.MySqlClient.MySqlDataReader rdr2 = cmd.ExecuteReader();
                        while (rdr2.Read())
                        {
                            category_id = (int)rdr2[0];
                        }
                        return chek;
                    }

                }
                mySqlConnectionSQLZepp.Close();
                
                cmd.CommandText = "INSERT INTO  jos_zepp_notlogin (domen , user , comp) VALUES ( '" + domen +
                    "' , '" + passwordMD5 +
                    "' , '" + comp + "' ) ";
                mySqlConnectionSQLZepp.Open();
                int intRecordsAffected = cmd.ExecuteNonQuery();
                if (intRecordsAffected == 1)
                {
 //Console.WriteLineC"Update succeeded");
                }
                else
                {
//ERR002
                    //Если intRecordsAffected = 0
                    //Console.WriteLine("Update failed");
                    System.Windows.Forms.MessageBox.Show("Произошла ошибка при регистрации текущего соединения" +
                    " \n Доменное имя:" + domen +
                    "\n Системное имя: " + user +
                    "\n TABLE = jos_zepp_notlogin;" +
                    "\n CONECT STRING =" + "server=192.168.5.200;User Id=admin;Persist Security Info=True;database=zepp" +
                    "\n Обратитесь к системному администратору (logoutInDomen ERR002)");
                }
                mySqlConnectionSQLZepp.Close();
            }
            catch (Exception e)
            {
 //ERR001
                System.Windows.Forms.MessageBox.Show("Произошла ошибка при регистрации пользователя на сервере" +
                    " \n Доменное имя:" + domen +
                    "\n Системное имя: " + user +
                    "\n TABLE = jos_zepp_users;" +
                   e.Message +
                   
                    "\n Обратитесь к системному администратору (logoutInDomen ERR001)");


            }
            finally
            {
                mySqlConnectionSQLZepp.Close();
            }
            //throw new System.NotImplementedException();
            return chek;
        }

        /// <summary>
        /// Получает подключение, введенные логин и пароль и находит пользователя устанавливая 
        /// соответсвующие значения полей. Если не находит выдает "false"
        /// </summary>
        /// <param name="mySqlConnectionSQLZepp"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool chekPassword(MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp, string login, string password)
        {
            string strSQL = "SELECT name , username , password , id , gid , pochta_chek FROM jos_users WHERE username = '" + login + "' ;";
            MySql.Data.MySqlClient.MySqlCommand cmd;
            cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);
            string _password = "",passwordMD5="";
            string[] split = new string[2];
          
            try
            {
                mySqlConnectionSQLZepp.Open();
                MySql.Data.MySqlClient.MySqlDataReader rdr = cmd.ExecuteReader();
                

                if (!rdr.HasRows) return false;

                while (rdr.Read())
                {
                    user_id = (int)rdr["id"];
                    pochta_chek = (int)rdr["pochta_chek"];
                    user_name = (string)rdr["name"];
                    gid = (byte)(rdr["gid"]);
                    _password = (string)rdr["password"];
                    split = _password.Split(new char[] { ':' }, 2);

                    passwordMD5 = hashGen(password, split[1]);


                    // Сравниваем пароли
                    if (passwordMD5 == split[0])
                    {
                        mySqlConnectionSQLZepp.Close();
                        // Устанавливаем должность
                        category_id = 0;
                        cmd.CommandText = "SELECT group_id FROM jos_projectlog_groups_mid WHERE user_id = " + user_id + " ;";
                        mySqlConnectionSQLZepp.Open();
                        MySql.Data.MySqlClient.MySqlDataReader rdr2 = cmd.ExecuteReader();
                        while (rdr2.Read())
                        {
                            category_id = (int)rdr2[0];
                        }
                        
                        return true;
                    }
                }
               

            }
            catch (Exception e)
            {
//ERR 003
                System.Windows.Forms.MessageBox.Show("Произошла ошибка при обращении к серверу" +
                    e.Message +
                    "\n Обратитесь к системному администратору (logoutInDomen ERR003)");
            }
            finally
            {
                mySqlConnectionSQLZepp.Close();
                l_evt.UserCheger();
            }
            return false;
        }

        /// <summary>
        /// Получает два ключа, выдает мд5 код по этим ключам
        /// </summary>
        /// <returns></returns>
        public string hashGen(string password , string split) 
        {            
            // создаем объект этого класса. Отмечу, что он создается не через new, а вызовом метода Create
            MD5 md5Hasher = MD5.Create();
            // Создаем новый Stringbuilder (Изменяемую строку) для набора байт
            StringBuilder sBuilder = new StringBuilder();
            string  passwordMD5 = "";

            passwordMD5 = password + split;

            // Преобразуем входную строку в массив байт и вычисляем хэш
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(passwordMD5));

            // Преобразуем каждый байт хэша в шестнадцатеричную строку
            for (int i = 0; i < data.Length; i++)
            {
                //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
