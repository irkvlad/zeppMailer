using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Diagnostics;
using System.Media;


namespace zeppMailer
{
     class ForLabText
    {
        object lockOn = new object();

        public int cherep = 0;
        public int comment = 0;
        public int move = 0;
        public int pochta = 0;
        public int war_projeckt_on = 0;
        public int war_projeckt_in = 0;
        public int brak = 0;
        public int deleted_proect = 0;

        public int Cherep() { lock (lockOn) { return cherep; } }
        public int Comment() { lock (lockOn) { return comment; } }
        public int Move() {lock (lockOn) {return move; } }
        public int Pochta() { lock (lockOn) { return pochta; } }
        public int War_projeckt_on() { lock (lockOn) { return war_projeckt_on; } }
        public int War_projeckt_in() { lock (lockOn) { return war_projeckt_in; } }
        public int Brak() { lock (lockOn) { return brak; } }
        public int Deleted_proect() { lock (lockOn) { return deleted_proect; } }

        public  int sum()
        {
            lock (lockOn) { return comment + move + pochta + deleted_proect; }
        }
        public int sumWar()
        {
            lock (lockOn) { return war_projeckt_on + war_projeckt_in + brak + cherep ; }
        }
    }

    delegate void ZeppEventMail(ForLabText labTxt);// Событие обновления цифр

    class ZeppEvent // Обработчик события обновления
    {
        public event ZeppEventMail mailOn;
        // Этот метод вызывается для запуска события, 
        public void mailOnChek(ForLabText labTxt)
        {
            if (mailOn != null)
                mailOn(labTxt);
        }
    }  

    
    static class Program
    {
        public static DateTime stamp = new DateTime();
        public static DateTime stamp2 = new DateTime();
        public static FormHiden hiden;
        //Для трея
        public static NotifyIcon notifyIcon = null;
        //public static NotifyIcon notifyIconWarn = null;         
        public static MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
        public static LibZepp.logoutInDomen user;        
        public static bool on_look = false;
        public static FormMain main;       
        public static ZeppEvent z_evt = new ZeppEvent();
        public static Thread mailConectZepp;
        public static Thread mainRun;
        public static Thread WarnMSG = new Thread(delegate() { });
        public static Thread jnMSG = new Thread(delegate() { });
        public static Thread FormMSG = new Thread(delegate() { });
        public static bool abort = true;
        public static bool onEvent = false;
        public static ForLabText labTxt = new ForLabText();
       

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]

        static void Main()
        {            
            Thread.CurrentThread.Name = "mainaTh";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Properties.Resources.favicon;
            notifyIcon.Text = "Новые сообщения отсутствуют";
            notifyIcon.Tag = 0;
            
            mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
            mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;

            string str = global::zeppMailer.Properties.Settings.Default.ConectToZepp;
            string[] strS = str.Split(';');
            if (strS.Length < 6 || strS[4] == "database=test")
                MessageBox.Show("В настоящий момент, у вас запущена демонстрационная версия программы \n"
                                    + "Сообщения и прочие данные НЕ СООТВЕТСТВУЮТ действительности \n"
                                    +"для подключения к реальной базе данных нужно изменить строку подключения \n"
                                    +"Строку подключения можно изменить в настройках программы или в конфиг-файле программы");
           
            
            hiden = new FormHiden();
            hiden.Opacity = 0;
            hiden.Show();
            while (hiden.Opacity < 1)
            {
                hiden.Opacity += 0.01;
                Thread.Sleep(10);
            }
            notifyIcon.Visible = true;            
            hiden.Opacity = 0;
            witeConect();
            user = new LibZepp.logoutInDomen(mySqlConnectionSQLZepp);
            
            if (user.user_id == 114)
            {
                MessageBox.Show("Пользователь под которым вы работаете на этом компьютере не зарегистрирован на сервере либо нет соединения с сервером");
                notifyIcon.Icon = Properties.Resources.faviconof;
                notifyIcon.Text = "Нет регистрации на сервере";
            }
          
            mailConectZepp = new Thread(
               delegate()
               {
                   chekMail();
               }
               );
            mailConectZepp.IsBackground = true;
            mailConectZepp.Priority = ThreadPriority.Lowest;
            mailConectZepp.Name = "mailerTh";                   
            mailConectZepp.Start();
            notifyIcon.Click += notifyIcon_Click;
            notifyIcon.BalloonTipClicked += notifyIcon_Click;
            Application.Run(hiden);
           
            notifyIcon.Visible = false;
            mailConectZepp.Abort();
            Application.Exit();
        }

        /// <summary>
        /// Чтение таблицы zepp_cheked для обнаружения не прочитанных сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void chekMail()
        {
            DateTime date = new DateTime();
            date = DateTime.Now; 
            //ForLabText labTxt = new ForLabText();
            labTxt = new ForLabText();
            string strSQL_in_Workc_user = "";
            string strSQL_on_Workc_user = "";
            string strSQL_cherep_user = "";
            string strSQL_brak_user = "";
                        
            string strSQL_in_Workc = "SELECT count(*) FROM jos_projectlog_projects WHERE (category =  8) AND (release_date < '" + date.ToString("yyyy-MM-dd") + "' )";
            string strSQL_on_Workc = "SELECT count(*) FROM jos_projectlog_projects WHERE (category = 7)  AND (deployment_to < '" + date.AddDays(-2).ToString("yyyy-MM-dd") + "' )";
            string strSQL_cherep = "SELECT count(*) FROM jos_projectlog_projects WHERE (category = 13)  ";
            string strSQLmove = "SELECT count(*) FROM jos_zepp_pochta , jos_projectlog_projects WHERE (tema =  3) AND (on_read = 0) AND ( jos_projectlog_projects.id = project_id) AND (to_user_id =";
            string strSQLcomment = "SELECT count(*) FROM jos_zepp_pochta , jos_projectlog_projects WHERE (tema =  4) AND (on_read = 0) AND ( jos_projectlog_projects.id = project_id)  AND (to_user_id =";
            string strSQL_brak = "SELECT count(*) FROM jos_projectlog_projects WHERE (category = 12) AND (location_spec <> '') ";
            string strSQL_del = "SELECT count(*) FROM jos_zepp_pochta WHERE (tema =  2) AND (on_read = 0) AND (to_user_id =";
            if (user.category_id != 11)
            {
                strSQL_in_Workc_user=strSQL_in_Workc + " AND (manager = " + user.user_id + " );";
                strSQL_on_Workc_user = strSQL_on_Workc +" AND (manager = " + user.user_id + " );";
                strSQL_cherep_user = strSQL_cherep + " AND (manager = " + user.user_id + " );";
                strSQL_brak_user = strSQL_brak + " AND (manager = " + user.user_id + " );";
            }
                       
            MySql.Data.MySqlClient.MySqlCommand cmd_in_Workc;
            MySql.Data.MySqlClient.MySqlCommand cmd_on_Workc;
            MySql.Data.MySqlClient.MySqlCommand cmd_cherep;
            MySql.Data.MySqlClient.MySqlCommand cmd_move;
            MySql.Data.MySqlClient.MySqlCommand cmd_comment;
            MySql.Data.MySqlClient.MySqlCommand cmd_brak;
            MySql.Data.MySqlClient.MySqlCommand cmd_del;
                        
            cmd_in_Workc = new MySql.Data.MySqlClient.MySqlCommand(strSQL_in_Workc, mySqlConnectionSQLZepp);
            cmd_on_Workc = new MySql.Data.MySqlClient.MySqlCommand(strSQL_on_Workc, mySqlConnectionSQLZepp);
            cmd_cherep = new MySql.Data.MySqlClient.MySqlCommand(strSQL_cherep, mySqlConnectionSQLZepp);
            cmd_move = new MySql.Data.MySqlClient.MySqlCommand(strSQLmove, mySqlConnectionSQLZepp);
            cmd_comment = new MySql.Data.MySqlClient.MySqlCommand(strSQLcomment, mySqlConnectionSQLZepp);
            cmd_brak = new MySql.Data.MySqlClient.MySqlCommand(strSQL_brak, mySqlConnectionSQLZepp);
            cmd_del = new MySql.Data.MySqlClient.MySqlCommand(strSQL_del, mySqlConnectionSQLZepp);

            onEvent = false;
            stamp = DateTime.Now;                      
            int inerval = Properties.Settings.Default.interval;

            //notifyIconWarn = new NotifyIcon();
            //notifyIconWarn.Visible = false;
            //notifyIconWarn.Icon = Properties.Resources.Wran2;
            //notifyIconWarn.Text = "Важные сообщения!!!";
            //notifyIconWarn.Click += notifyIcon_Click;

            stamp2 = stamp;

while (true)
{
    int chek = 0;
//** Начало цыкла, этот код крутися в трее
    try
    {
        #region //Чтение сообщений на сервере
        if (user.user_id != 87) stamp = DateTime.Now;
        int interval2 = inerval;
        witeConect();

        // Проверка просрочки
        if (user.category_id != 11)
        {
            cmd_in_Workc.CommandText = strSQL_in_Workc + " AND (manager = " + user.user_id + " );";
            cmd_on_Workc.CommandText = strSQL_on_Workc + " AND (manager = " + user.user_id + " );";
            cmd_cherep.CommandText = strSQL_cherep + " AND (manager = " + user.user_id + " );";
            cmd_brak.CommandText = strSQL_brak + " AND (manager = " + user.user_id + " );";
        } else
        {
            cmd_in_Workc.CommandText = strSQL_in_Workc;
            cmd_on_Workc.CommandText = strSQL_on_Workc;
            cmd_cherep.CommandText = strSQL_cherep;
            cmd_brak.CommandText = strSQL_brak;
        }

        cmd_move.CommandText = strSQLmove + user.user_id + " ) ;";
        cmd_comment.CommandText = strSQLcomment + user.user_id + " ) ;";
        cmd_del.CommandText = strSQL_del + user.user_id + " ) ;";

        int last = labTxt.sumWar();
        int last_move = labTxt.move;
        int Last_comment = labTxt.comment;
        int last_del = labTxt.deleted_proect;

        mySqlConnectionSQLZepp.Open();
        labTxt.war_projeckt_in = int.Parse(cmd_in_Workc.ExecuteScalar().ToString());
        labTxt.war_projeckt_on = int.Parse(cmd_on_Workc.ExecuteScalar().ToString());
        labTxt.move = int.Parse(cmd_move.ExecuteScalar().ToString());
        labTxt.comment = int.Parse(cmd_comment.ExecuteScalar().ToString());
        labTxt.brak = int.Parse(cmd_brak.ExecuteScalar().ToString());
        labTxt.deleted_proect = int.Parse(cmd_del.ExecuteScalar().ToString());

        if (user.category_id != 11) labTxt.cherep = int.Parse(cmd_cherep.ExecuteScalar().ToString());
        else labTxt.cherep = 0;
        mySqlConnectionSQLZepp.Close();
        #endregion       

        #region Установка меток\событий для показа сообщений, и обновления главной формы
        //Если количество сообщений больше чем было: уведомить об этом , и событие обновления главой формы
        if (labTxt.move > last_move || labTxt.comment > Last_comment || labTxt.deleted_proect > last_del)
        {
            onEvent = true;
            on_look = true;            
        }
        //Если просрочка изменилась и это не шилова: уведомить об этом , и событие обновления главой формы
        if (last != labTxt.sumWar() && user.user_id != 87)
        {
            onEvent = true;
            //stamp2 = stamp;
        }
        if (last < labTxt.sumWar() && user.user_id != 87)
        {
            onEvent = true;
            stamp2 = stamp;
        }
        //Если количество сообщений изменилось : событие обновления главой формы
        if (last_move + Last_comment + last_del != labTxt.sum())
        {
            onEvent = true;            
        }  

        //Если имеются не прочитанные сообщения: событие обновления главой формы
        if (labTxt.sum() > 0)
        {
            onEvent = true;
            chek++;
        }

        //Если есть просрочка
        if (labTxt.sumWar() > 0)
        {
            onEvent = true;
            chek = chek + 2;
        }
        #endregion

        #region // Показываем сообщение просрочки
        //Если просрочка имеется и это не Шилова : событие обновления главой формы
        if (labTxt.sumWar() > 0 && user.user_id != 87)
        {                 
            if (stamp > stamp2) //Вышел временной интервал напомнить повторно
            {
                interval2 = interval2 - 5 * labTxt.sumWar();
                if (interval2 < 5) interval2 = 5;
                stamp2 = stamp.AddMinutes(interval2);

                string MSG = "";
                if (labTxt.cherep > 0) MSG += "\n Проекты, выполнение которых под угрозой " + labTxt.cherep + " шт. ";
                if (labTxt.war_projeckt_in > 0) MSG += " \n  Прошел срок реализации: " + labTxt.war_projeckt_in + " шт. ";
                if (labTxt.war_projeckt_on > 0) MSG += " \n  Не прнято в работу: " + labTxt.war_projeckt_on + " шт.";
                if (labTxt.brak > 0) MSG += " \n  Выявлен брак: " + labTxt.brak + " шт.";

                //Запущен ли уже поток(открыто окно) - сообщение в трей
                if (WarnMSG.IsAlive)
                {
                    SystemSounds.Beep.Play();
                    notifyIcon.ShowBalloonTip(500, "Внимание!!! Срочно примите решение!", MSG, ToolTipIcon.Warning);
                    Thread.Sleep(500);
                    SystemSounds.Beep.Play(); Thread.Sleep(400);
                    SystemSounds.Beep.Play(); Thread.Sleep(100);
                    SystemSounds.Beep.Play();
                } else //Вывести окно
                {
                    WarnMSG = new Thread(
                        delegate()
                        {
                            onWarnMSG(MSG, interval2);
                        }
                        );

                    WarnMSG.IsBackground = false;
                    WarnMSG.Priority = ThreadPriority.Lowest;
                    WarnMSG.Name = "WarnMSG";
                    WarnMSG.Start();
                }
            }
        }
        #endregion

        #region//Если есть необходимость обновить главную форму / И уведомить о новых сообщениях
        if (onEvent)
        {

            // Если окно активно - Запустить исключение изминения количества сообщения
            if (!(main == null || main.IsDisposed))
            {
                z_evt.mailOnChek(labTxt);
                onEvent = false;
            }

            if (on_look) //Если уведомления не показывались - показать
            {                
                SystemSounds.Exclamation.Play();
                string MSG = "Имеются новые сообщения всего " + labTxt.sum() + " шт.";

                SystemSounds.Beep.Play(); Thread.Sleep(500);
                notifyIcon.ShowBalloonTip(5000, "Новые сообщения", MSG, ToolTipIcon.Info);                
                SystemSounds.Beep.Play(); Thread.Sleep(300);
                on_look = false;
            }
        }
        #endregion
        #region //Установка иконки
        switch (chek)
        {
            case 0:
                if ((int)notifyIcon.Tag != 0)
                {
                    notifyIcon.Tag = 0;
                    notifyIcon.Text = "Новые сообщения отсутсвуют";
                    notifyIcon.Icon = Properties.Resources.favicon;
                }
                break;
            case 1:
                if ((int)notifyIcon.Tag != 1)
                {
                    notifyIcon.Tag = 1;
                    notifyIcon.Text = "Есть новые сообщения: " + labTxt.sum();
                    notifyIcon.Icon = Properties.Resources.faviconCH;
                }
                break;
            case 2:
                if ( (int)notifyIcon.Tag != 2)
                {
                    notifyIcon.Tag = 2;
                    notifyIcon.Text = "Срочно примите решение: " + labTxt.sumWar();
                    notifyIcon.Icon = Properties.Resources.faviconWarn;
                }
                break;
            case 3:
                if ( (int)notifyIcon.Tag != 3)
                {
                    notifyIcon.Tag = 3;
                    notifyIcon.Text = "Срочно примите решение: " + labTxt.sumWar() +
                        "\nЕсть новые сообщения: " + labTxt.sum(); ;
                    notifyIcon.Icon = Properties.Resources.faviconCHWARN;
                }
                break;
        }
        #endregion
    }

    catch (Exception e)
    {
        //ERR 001
        MessageBox.Show("Ошибка причтении данных с сервера"
            + e.Message
            + "(Program ERR001)");
    }
            finally
            {
                if (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                    mySqlConnectionSQLZepp.Close();
            }

            Thread.Sleep(5000);
}
//** Конец цыкла
        
        }

        private static void notifyIcon_Click(object sender, EventArgs e)
        {           
            if (main == null || main.IsDisposed)
            {
                main = new FormMain();
                mainRun = new Thread( delegate() { Application.Run(main); } );               
            }

            if (!main.Visible)
            {
                main.UseWaitCursor = true;
                main.Show();                
                Thread.Sleep(100);
                z_evt.mailOnChek(labTxt);
                Thread.Sleep(100);
                main.UseWaitCursor = false;
            } else
            {
                if (main.WindowState == FormWindowState.Normal) main.WindowState = FormWindowState.Minimized;
                else main.WindowState = FormWindowState.Normal;
            }
        }


        /// <summary>
        /// Открывает закрывает соединение с сервером true  - без "иключений", false - при "исключении"
        /// </summary>
        /// <param name="mySqlConnectionSQLZepp"></param>
        /// <returns></returns>
        public static bool pingServer(MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp)
        {
            try
            {
                if(mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Closed)
                    mySqlConnectionSQLZepp.Open();
                if (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                mySqlConnectionSQLZepp.Close();
                return true;
            }
            catch
            {
                if (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                mySqlConnectionSQLZepp.Close();
                return false;
            }
        }

        private static void witeConect()
        {
            string nText = "Новые сообщения отсутствуют";
            bool on_conect = pingServer(mySqlConnectionSQLZepp);
            int nI = 0;

            if (!on_conect)
            {
                nText = notifyIcon.Text;
                nI = (int)notifyIcon.Tag;
                notifyIcon.Tag = notifyIcon.Icon;
                MessageBox.Show("Не могу соединтся с базой\n");
                notifyIcon.Icon = Properties.Resources.faviconof;
               
                notifyIcon.Text = "Нет соединения с сервером";
            }
            while (!on_conect)
            {
                Thread.Sleep(5000);
                on_conect = pingServer(mySqlConnectionSQLZepp);
                if (on_conect)
                {
                    notifyIcon.Icon = (System.Drawing.Icon)notifyIcon.Tag;
                    notifyIcon.Text = nText;
                    notifyIcon.Tag = nI;
                    MessageBox.Show("Сервер доступен");
                }
            }
        }

        public static void onWarnMSG(string MSG, int interval2)
        {
            SystemSounds.Beep.Play(); SystemSounds.Beep.Play(); SystemSounds.Beep.Play(); SystemSounds.Beep.Play();
            MessageBox.Show(MSG + "\n\n Интервал критических сообщений " + interval2 + " минут.", "Внимание !!! Срочно примите решение!!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }

         public static void  onjnMSG()//string MSG)
        {
             object sender = null; 
             EventArgs e = null;
            notifyIcon_Click( sender,  e);
        }
       
    }

   
      
   

}
