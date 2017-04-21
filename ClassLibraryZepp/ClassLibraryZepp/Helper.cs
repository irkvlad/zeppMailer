using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibZepp
{
    /// <summary>
    /// Вспомогательные функции
    /// </summary>
    public static class Helper
    {
        static public bool updateMySQL = false;

        #region Менеджеры
        /// <summary>
        /// Список менеджеров
        /// </summary>
        public static List<User> managers;
        static User manager;
        /// <summary>
        /// Получает список менеджера из таблицы jos_contact_details
        /// </summary>
        /// <returns> Список менеджеров</returns>
        public static List<User> GetManagers()
        {
            StringBuilder where = new StringBuilder();
            if (User.userSet.company_id > 0) where.Append(" AND company = " + User.userSet.company_id + " ");

            managers = new List<User>();
            string queryString = @" SELECT 
                                         user_id
                                    FROM  jos_contact_details  
                                        WHERE  catid = 3 AND published = 1 " + where.ToString();
                                ;

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
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
                                object ttt = (int)dr["user_id"];
                                manager = new User((int)ttt);

                                
                               managers.Add(manager);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return managers;
        }
        /// <summary>
        /// Получает массив буквеных индентификаторов менеджеров
        /// </summary>
        /// <param name="managers">набор с менеджерами</param>
        /// <returns> масив строк </returns>
        public static string[] GetCharID(List<User> managers)
        {
            string[] charS = new string[managers.Count()+1];
            charS[0] = "Б/К";
            int i = 1;
            foreach (User man in managers) { charS[i] = man.project_user_id; i++; }
            return charS;
        }

        /// <summary>
        /// Находит менеджера по буквенному индентификатору
        /// </summary>
        /// <param name="managers">Набор менеджеров</param>
        /// <param name="charS">буквенный индентификатор</param>
        /// <returns>данные менеджера</returns>
        public static User GetUserOnCharS(List<User> managers, string charS)
        {
            foreach (User man in managers) if (man.project_user_id == charS) return man;
            return new User();
        }
        #endregion
        
        /// <summary>
        /// Записываем файлы для полноцвета в базу на сайте
        /// </summary>
        /// <param name="List<Polnocvet>">Список файлов</param>
        /// <returns>Все задания созданы</returns>
        public static bool SavePolnocvetToMYSQL(List<Polnocvet> files, string linkFile)
        {            
            bool retr = true;
            updateMySQL = true;
            // Какие данные записывать?
            foreach (Polnocvet f in files)
            {
                string link = "<a href =\"" + linkFile + "/" + f.ToString() + "\" > Сcылка на файл</a >";
                if (f.FileOnServer)
                {
                    string queryString = @"INSERT INTO  `jos_zepp_polnocvet` SET"
                            + " `link` = '" + link + "' "                         // Ссылка на файл 
                            + " , `name_file` = '" + f.ToString() + "' "          // Имя файла
                            + " , `file` = '" + f.Startfile.Name + "' "           // Исходный файл
                            + " , `manager_id` = " + f.manager_id                 // Номер менеджера
                            + " , `ploschad` = " + (float.Parse(f.sizeM.ToString()) / 1000 * float.Parse(f.sizeL.ToString()) / 1000).ToString("F2", new CultureInfo("en-US")) // Площадь печати на станке
                            + " , `stanok` = '" + f.stanok.ToString()[0] + "' "                 // Станок
                            + " , `user_id` = '" + User.userSet.user_id + "' "
                            + " , `company` = '" + f.company + "' "
                            + "; SELECT id FROM  `jos_zepp_polnocvet` WHERE `link` = '" + link + "' "
                        ;
                    int id_polnocvet = 0;

                    using (MySqlConnection con = new MySqlConnection())
                    {
                        con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
                        MySqlCommand com = new MySqlCommand(queryString, con);
                        try
                        {
                            con.Open();
                            id_polnocvet = (int)com.ExecuteScalar();
                            con.Close();
                            f.id_file = id_polnocvet;
                            if (f.id_file == 0)
                                retr = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при сохранении на сайт:" + ex.Message);
                            retr = false;
                        }
                    }

                }
            }
            updateMySQL = false;
            return retr;
        }
        public static bool SavePolnocvetToMYSQL(List<PrintFile> files, string linkFile) {
            bool retr = true;
            updateMySQL = true;
            // Какие данные записывать?
            foreach (PrintFile f in files) {
                string link = "<a href =\"" + linkFile + "/" + f.ToString() + "\" > Сcылка на файл</a >";
                if (f.fileOnServer) {
                    string queryString = @"INSERT INTO  `jos_zepp_polnocvet` SET"
                            + " `link` = '" + link + "' "                         // Ссылка на файл 
                            + " , `name_file` = '" + f.ToString() + "' "          // Имя файла
                            + " , `file` = '" + f.startfile.Name + "' "           // Исходный файл
                            + " , `manager_id` = " + PrintFile.selectManager.id                  // Номер менеджера
                            + " , `ploschad` = " + (float.Parse(f.sizeM.ToString()) / 1000 * float.Parse(f.sizeL.ToString()) / 1000).ToString("F2", new CultureInfo("en-US")) // Площадь печати на станке
                            + " , `stanok` = '" + f.stanok.ToString()[0] + "' "                 // Станок
                            + " , `user_id` = '" + User.userSet.user_id + "' "
                            + " , `company` = '" + f.company + "' "
                            + "; SELECT id FROM  `jos_zepp_polnocvet` WHERE `link` = '" + link + "' "
                        ;
                    int id_polnocvet = 0;

                    using (MySqlConnection con = new MySqlConnection()) {
                        con.ConnectionString = User.mySqlConnectionSQLZepp.ConnectionString;
                        MySqlCommand com = new MySqlCommand(queryString, con);
                        try {
                            con.Open();
                            id_polnocvet = (int)com.ExecuteScalar();
                            con.Close();
                            f.id_file = id_polnocvet;
                            if (f.id_file == 0)
                                retr = false;
                        }
                        catch (Exception ex) {
                            MessageBox.Show("Ошибка при сохранении на сайт:" + ex.Message);
                            retr = false;
                        }
                    }

                }
            }
            updateMySQL = false;
            return retr;
        }


    }
}
