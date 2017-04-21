using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace LibZepp
{
    /// <summary>
    /// Предостовляет данные пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// Пользователь авторизованный в программе
        /// </summary>
        public static logoutInDomen userSet;
        /// <summary>
        /// Строка подлючения к базе
        /// </summary>
        public static MySqlConnection mySqlConnectionSQLZepp;

       
        /// <summary>
        /// Номер пользователя в базе jos_users
        /// </summary>
        public int id { get; private set; }        
        /// <summary>
        /// Фамилия Имя в базе jos_users
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// Логин пользователя в базе jos_users
        /// </summary>
        public string username { get; private set; }        
        /// <summary>
        /// Уровень привилегий (23 - менеджер; 24 - администратор) в базе jos_users
        /// </summary>
        public int gid { get; private set; }
        /// <summary>
        ///  Должность пользователя по проектам (23 - менеджер; 24 - администратор) в базе jos_users
        /// </summary>
        public int category_userProject { get; private set; }        
        /// <summary>
        /// Состояние пользователя (1 - выключен) в базе jos_users
        /// </summary>
        public int block { get; private set; }
        /// <summary>
        /// Цвет букв в календарике для пользователя в базе jos_users
        /// </summary>
        public string color { get; private set; }
        /// <summary>
        /// Цвет фона в календаоике для пользователя в базе jos_users
        /// </summary>
        public string bgcolor { get; private set; }
        /// <summary>
        /// Инициалы пользователя - индентификатр в 1С
        /// </summary>
        public string project_user_id {
            get { return _project_user_id; }
            private set {
                string st = "Б\\К";
                _project_user_id = value;
                if (_project_user_id == st) _project_user_id = "Б/К";
            }
        }
        string _project_user_id;
        /// <summary>
        ///  Домен пользователя (zepp.local) в базе jos_users
        /// </summary>
        public string domen { get; private set; }
        
        /// <summary>
        /// Имя компьютера пользователя в базе jos_users
        /// </summary>
        public string comp { get; private set; }
        
        /// <summary>
        /// Доменный логин пользователя в базе jos_users
        /// </summary>
        public string domenNick { get; private set; }

        /// <summary>
        /// Получает ли пользователь сообщения на почту из проектов в базе jos_users
        /// </summary>
        public int pochta_chek { get; private set; }

        /// <summary>
        /// Резрешено ли пользователю управлять печатью полноцвета в базе jos_users
        /// </summary>
        public int polnocvet { get; private set; }

        /// <summary>
        /// Резрешено ли пользователю управлять печатью полноцвета в базе jos_users
        /// </summary>
        public bool inSetUsers { get; private set; }

        /// <summary>
        ///  Резрешено ли пользователю управлять печатью полноцвета в базе jos_users
        /// </summary>
        public bool inSetContactDetails { get; private set; }
        /// <summary>
        /// Коментарий к должности в базе jos_contact_details
        /// </summary>
        public string con_position { get; private set; }

        /// <summary>
        /// Офис в базе jos_contact_details
        /// </summary>
        public string address { get; private set; }
        
        /// <summary>
        /// почта в базе jos_contact_details
        /// </summary>
        public string email_to { get; private set; }
        
        /// <summary>
        /// 1-опубликован в базе jos_contact_details
        /// </summary>
        public int published { get; private set; }

        /// <summary>
        /// номер должности в базе jos_contact_details
        /// </summary>
        public int catid { get; private set; }

        /// <summary>
        /// Номер мобильного телефона в базе jos_contact_details
        /// </summary>
        public string mobile { get; private set; }

        /// <summary>
        /// Название по номеру должности в базе jos_contact_details
        /// </summary>
        public string title { get; private set; }
        /// <summary>
        /// Веб страница, отличить Цехком от Цепелина
        /// </summary>
        public int company { get; private set; }

        /// <summary>
        /// Конструктор поумолчанию
        /// </summary>
        public User()
        {
            if (userSet.user_id != 114)
            {
                id                      = userSet.user_id;
                name                    = userSet.user_name;
                category_userProject    = userSet.category_id;
                comp                    = userSet.comp;
                domen                   = userSet.domen;
                gid                     = userSet.gid;
                domenNick               = userSet.user;
                pochta_chek             = userSet.pochta_chek;
                company                 = userSet.company_id;
                inSetUsers              = Get_josUsersLite();
                inSetContactDetails     = Get_josContactDetails(userSet.user_id);
                //managers = Helper.GetManagers();
            }

        }

        /// <summary>
        /// Конструктор по указанному id
        /// </summary>
        /// <param name="user_id"></param>
        public User(int user_id)
        {
            if (user_id != 114 && user_id > 0)
            {
                id = user_id;
                inSetUsers = Get_josUsers(id);
                inSetContactDetails = Get_josContactDetails(id);
                //managers = Helper.GetManagers();
            }

        }
        
        /// <summary>
        /// Заполняет поля объекта из базы jos_users по id
        /// </summary>
        /// <param name="id">Ид пользователя</param>
        /// <returns>Объект с данными пользователя из базы</returns>
        private bool Get_josUsers(int id)
        {
           string queryString = @"SELECT 
                                    name, 
                                    username, 
                                    gid, 
                                    block, 
                                    color, 
                                    bgcolor, 
                                    pr_user_id, 
                                    domen, 
                                    user , 
                                    comp , 
                                    pochta_chek,
                                    dol_user, 
                                    polnocvet 
                                FROM jos_users 
                                WHERE id = '" + id + "'; ";

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mySqlConnectionSQLZepp.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);

                try
                {
                    con.Open();

                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                username                = (string)dr["username"];
                                block                   = int.Parse(dr["block"].ToString());
                                color                   = (string)dr["color"];
                                bgcolor                 = (string)dr["bgcolor"];
                                project_user_id         = (string)dr["pr_user_id"];
                                polnocvet               = (dr["polnocvet"] == DBNull.Value) ? 0 : int.Parse(dr["polnocvet"].ToString());
                                name                    = (string)dr["name"];
                                category_userProject    = (dr["dol_user"] == DBNull.Value) ? 0 : (int)dr["dol_user"];
                                comp                    = (dr["comp"] == DBNull.Value) ? "" : (string)dr["comp"]; 
                                domen                   = (dr["domen"] == DBNull.Value) ? "" : (string)dr["domen"]; 
                                gid                     = int.Parse(dr["gid"].ToString());
                                domenNick               = (dr["user"] == DBNull.Value) ? "" : (string)dr["user"];
                                pochta_chek             = (int)dr["pochta_chek"];
                            }
                            
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///  Заполняет поля объекта из базы jos_users, без имеющихся полей
        /// </summary>
        /// <returns>Объект с данными пользователя из базы</returns>
        private bool Get_josUsersLite()
        {
            string queryString = @"SELECT 
                                    username, 
                                    block, 
                                    color, 
                                    bgcolor, 
                                    pr_user_id, 
                                    polnocvet 
                                FROM jos_users 
                                WHERE id = '" + userSet.user_id + "'; ";

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mySqlConnectionSQLZepp.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();

                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                username = (string)dr["username"];
                                block = int.Parse(dr["block"].ToString());//(int)dr["block"];
                                color = (string)dr["color"];
                                bgcolor = (string)dr["bgcolor"];
                                project_user_id = (string)dr["pr_user_id"];
                                polnocvet = (dr["polnocvet"] == DBNull.Value) ? 0 : int.Parse(dr["polnocvet"].ToString());
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Заполняет объект полями из jos_contact_details
        /// Структура jos_contact_details
        /// name - Имя
        /// con_position - должность коментарии
        /// address - офис
        /// email_to - email
        /// published - 1-включен
        /// catid - должность категория 
        /// mobile
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>Объект с данными пользователя из базы</returns>
        private bool Get_josContactDetails(int id)
        {
            string queryString = @"SELECT 
                                    u.con_position,
                                    u.address,
                                    u.email_to,
                                    u.published, 
                                    u.catid,
                                    u.mobile,
                                    u.company,
                                    c.title

                                   FROM  jos_contact_details as u, jos_categories as c 
                                   WHERE u.user_id = '" + id + "' AND c.id = u.catid; ";

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mySqlConnectionSQLZepp.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();

                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {                                
                                con_position    = (dr["con_position"] == DBNull.Value) ? "" : (string)dr["con_position"];
                                address         = (dr["address"] == DBNull.Value) ? "" : (string)dr["address"];
                                email_to        = (dr["email_to"] == DBNull.Value) ? "" : (string)dr["email_to"];
                                published       = int.Parse(dr["published"].ToString());
                                catid           = int.Parse(dr["catid"].ToString());
                                mobile          = (string)dr["mobile"];
                                company          = (dr["company"] == DBNull.Value) ? 0 : int.Parse(dr["company"].ToString());
                                title           = (string)dr["title"];
                            }
                            userSet.company_id = company;
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return true;
        }

    }
}
