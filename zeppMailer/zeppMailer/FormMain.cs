using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;

namespace zeppMailer
{
   
    public partial class FormMain : Form
    {
        
        DateTime date = new DateTime();
       
 // Определение
        static MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
        MySql.Data.MySqlClient.MySqlCommand cmdCherep;
        MySql.Data.MySqlClient.MySqlCommand cmd_in_Workc;
        MySql.Data.MySqlClient.MySqlCommand cmd_on_Workc;
        MySql.Data.MySqlClient.MySqlCommand cmd_Mov;
        MySql.Data.MySqlClient.MySqlCommand cmd_Comment;
        MySql.Data.MySqlClient.MySqlCommand cmd_Del;
        //MySql.Data.MySqlClient.MySqlCommand cmd_Err;
        MySql.Data.MySqlClient.MySqlCommand cmd_Err2;

        public DataGridViewTextBoxColumn idColumn;// = new DataGridViewTextBoxColumn();
        public DataGridViewTextBoxColumn titleColumn;// = new DataGridViewTextBoxColumn();
        public DataGridViewTextBoxColumn release_dateColumn;// = new DataGridViewTextBoxColumn();
        public DataGridViewTextBoxColumn release_idColumn;// = new DataGridViewTextBoxColumn();        
        public DataGridViewTextBoxColumn deployment_toColumn;// = new DataGridViewTextBoxColumn();
        public DataGridViewTextBoxColumn send_dateColumn;
        public DataGridViewTextBoxColumn from_user_idColumn;
        public DataGridViewTextBoxColumn textColumn;
        public DataGridViewTextBoxColumn on_readColumn;
        public DataGridViewTextBoxColumn pochta_idColumn;

        public DataGridViewCell idCell; //= new DataGridViewTextBoxCell();
        public DataGridViewCell textCell;// = new DataGridViewTextBoxCell();
        public DataGridViewCell titleCell;// = new DataGridViewTextBoxCell();
        public DataGridViewCell release_dateCell;// = new DataGridViewTextBoxCell();
        public DataGridViewCell release_idCell;// = new DataGridViewTextBoxCell();       
        public DataGridViewCell deployment_toCell;// = new DataGridViewTextBoxCell();
        public DataGridViewCell send_dateCell;// = new DataGridViewTextBoxColumn();
        public DataGridViewCell from_user_idCell;// = new DataGridViewTextBoxColumn();
        public DataGridViewCell on_readCell;
        public DataGridViewCell pochta_idCell;
        public DataGridViewRow row;// = new DataGridViewRow();               

        public FormMain()
        {
            InitializeComponent();
            mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
            mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;
            if (global::zeppMailer.Properties.Settings.Default.NameBrauser == "")
            {
                выбратьБраузерToolStripMenuItem.ToolTipText = "Установлен по умолчанию";
                сброситьБраузерToolStripMenuItem.CheckState = CheckState.Checked;
                сброситьБраузерToolStripMenuItem.Checked = true;
            } else
            {
                сброситьБраузерToolStripMenuItem.CheckState = CheckState.Unchecked;
                сброситьБраузерToolStripMenuItem.Checked = false;
                выбратьБраузерToolStripMenuItem.ToolTipText = "Установлен " + global::zeppMailer.Properties.Settings.Default.NameBrauser;
            }
        }

        private void ChegeUser(object sender , LibZepp.logoutEvent.logoutEventArgs e)
        {
            //SQL_Alarm();
            //init_onWorkcGrid();
           // Grid_on_Workc();
            
            labelProjOn_TextChanged( sender, e);
            labelProjIn_TextChanged(sender, e);
            labelCherepText_TextChanged(sender, e);


        }

        private void FormMain_Load(object sender, EventArgs e)
        {            
            init_onWorkcGrid();
            init_inWorkcGrid();
            init_CherepGrid();
            this.webBrowserCherep.DocumentText = "";
            init_MovGrid();
            init_CommentGrid();
            init_DelGrid();


            if (Program.user.pochta_chek == 0)
            {
                PochtaChekcToolStripMenuItem.Checked = false;
                PochtaChekcToolStripMenuItem.CheckState = CheckState.Unchecked;
            } else
            {
                PochtaChekcToolStripMenuItem.Checked = true;
                PochtaChekcToolStripMenuItem.CheckState = CheckState.Checked;
            }
            this.Text += zeppMailer.Program.user.user_name;
            Program.z_evt.mailOn += textLabel_Chage; // Пидписываемся на обновление полей из трея
            LibZepp.logoutInDomen.l_evt.UserChege += ChegeUser; // Пописываемся на событие смены пользователя
           // this.comboBox1.SelectedIndex = global::zeppMailer.Properties.Settings.Default.filter;
             //MessageBox.Show(filter());

            if (Program.user.category_id == 11)
            {
                this.linkLabelCherep.Visible = false;
                this.labelBrak.Visible = true;
            } else
            {
                this.linkLabelCherep.Visible = true;
                this.labelBrak.Visible = false;
            }
          
        }            
       
        private void закратьМессенджерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zeppMailer.Program.abort = false;           
            zeppMailer.Program.hiden.Close();
            this.Close();
        }

        /// <summary>
        /// Обработка события обновления полей . Событие создается в Program
        /// </summary>
        /// <param name="labTxt"></param>
        private void textLabel_Chage(ForLabText labTxt)
        {                         
                this.labelCherepText.SetText( labTxt.Cherep().ToString() );            

                this.labelCommentText.SetText( labTxt.Comment().ToString() ) ;
                this.labelMoveText.SetText( labTxt.Move().ToString() );
                this.labelPochtaText.SetText( labTxt.Deleted_proect().ToString() );
                this.labelProjOn.SetText(labTxt.War_projeckt_in().ToString());
                this.labelBrakTxt.SetText(labTxt.Brak().ToString());

                if (!this.pictureBoxCherep.Visible && (int.Parse(this.labelCherepText.Text.ToString()) + int.Parse(this.labelBrakTxt.Text.ToString())) > 0)
                {
                    this.pictureBoxCherep.SetImg(true);
                }
                if (this.pictureBoxCherep.Visible && (int.Parse(this.labelCherepText.Text.ToString()) + int.Parse(this.labelBrakTxt.Text.ToString())) < 1)
                {
                    this.pictureBoxCherep.SetImg(false);
                }

                if (!this.pictureBoxIn.Visible && int.Parse(this.labelProjOn.Text.ToString()) > 0)
                {
                    this.pictureBoxIn.SetImg( true);
                }
                if (this.pictureBoxIn.Visible && int.Parse(this.labelProjOn.Text.ToString()) < 1)
                {
                    this.pictureBoxIn.SetImg(false);
                }

                this.labelProjIn.SetText(labTxt.War_projeckt_on().ToString());
                if (!this.pictureBoxOn.Visible && int.Parse(this.labelProjIn.Text.ToString()) > 0)
                {
                    this.pictureBoxOn.SetImg(true);
                }
                if (this.pictureBoxOn.Visible && int.Parse(this.labelProjIn.Text.ToString()) < 1)
                {
                    this.pictureBoxOn.SetImg(false);
                }
                this.UseWaitCursor = false;
           
        }

        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object test;
            FormLogin formLogin = new FormLogin();
            formLogin.Show();
            test = formLogin.DialogResult;
        }

   
#region //################# События Панели филтра 
    //Сортировка по: 0-номер пректа, 1 - дате, 2-типу. Должно запускаться чтение полей из базы
        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    global::zeppMailer.Properties.Settings.Default.filter = this.comboBox1.SelectedIndex;
        //    global::zeppMailer.Properties.Settings.Default.Save();
        //    //MessageBox.Show(filter());
        //    fillingMov();
        //}

    //Показать не прочитанное. Должно запускаться чтение полей из базы
        private void buttonNotRead_Click(object sender, EventArgs e)
        {
            this.buttonNotRead.Enabled = global::zeppMailer.Properties.Settings.Default.NotRead=false;
            this.buttonToDay.Enabled = global::zeppMailer.Properties.Settings.Default.Todey = true;
            this.buttonWekly.Enabled = global::zeppMailer.Properties.Settings.Default.Wikly = true;
            this.buttonAll.Enabled = global::zeppMailer.Properties.Settings.Default.All = true;
            global::zeppMailer.Properties.Settings.Default.Save();
            //MessageBox.Show(filter());
            fillingMov();
            fillingComment();
        }

    //Показать сегодняшние. Должно запускаться чтение полей из базы
        private void buttonToDay_Click(object sender, EventArgs e)
        {
            this.buttonNotRead.Enabled = global::zeppMailer.Properties.Settings.Default.NotRead = true;
            this.buttonToDay.Enabled = global::zeppMailer.Properties.Settings.Default.Todey = false;
            this.buttonWekly.Enabled = global::zeppMailer.Properties.Settings.Default.Wikly = true;
            this.buttonAll.Enabled = global::zeppMailer.Properties.Settings.Default.All = true;
            global::zeppMailer.Properties.Settings.Default.Save();
            //MessageBox.Show(filter());
            fillingMov();
            fillingComment();
            fillingDel();
        }

    //Показать за неделю. Должно запускаться чтение полей из базы
        private void buttonWekly_Click(object sender, EventArgs e)
        {
            this.buttonNotRead.Enabled = global::zeppMailer.Properties.Settings.Default.NotRead = true;
            this.buttonToDay.Enabled = global::zeppMailer.Properties.Settings.Default.Todey = true;
            this.buttonWekly.Enabled = global::zeppMailer.Properties.Settings.Default.Wikly = false;
            this.buttonAll.Enabled = global::zeppMailer.Properties.Settings.Default.All = true;
            global::zeppMailer.Properties.Settings.Default.Save();
            //MessageBox.Show(filter());
            fillingMov();
            fillingComment();
            fillingDel();
        }

    //Показать все.Должно запускаться чтение полей из базы
        private void buttonAll_Click(object sender, EventArgs e)
        {
            this.buttonNotRead.Enabled = global::zeppMailer.Properties.Settings.Default.NotRead = true;
            this.buttonToDay.Enabled = global::zeppMailer.Properties.Settings.Default.Todey = true;
            this.buttonWekly.Enabled = global::zeppMailer.Properties.Settings.Default.Wikly = true;
            this.buttonAll.Enabled = global::zeppMailer.Properties.Settings.Default.All = false;
            global::zeppMailer.Properties.Settings.Default.Save();
            //MessageBox.Show(filter());
            fillingMov();
            fillingComment();
            fillingDel();
        }

    // SQL запрос согласнстно фильтра на вкладках "передвижения" и "сообщения" 0 - WHERE ; 1- ORDER
        public string[] filter()
        {
            //StringBuilder SQL_SELECT = new StringBuilder("SELECT * FROM jos_zepp_pochta ");
            //StringBuilder SQL_WHERE = new StringBuilder(" WHERE ( to_user_id = " + Program.user.user_id + " ) ");
            StringBuilder[] SQL = new StringBuilder[2];
            SQL[0] = new StringBuilder(" ");
            SQL[1] = new StringBuilder(" ");// ORDER BY ");
            date = DateTime.Now;
            //Фильтер
            //switch (this.comboBox1.SelectedIndex)
            //{
            //    case 0:
            //        SQL[1].Append(" project_id ");
            //        break;

            //    case 1:
            //        SQL[1].Append(" send_date ");
            //        break;

            //    case 2:
            //        SQL[1].Append(" tema ");
            //        break;

            //    default:
            //        SQL[1].Append(" send_date ");
            //        break;
            //}
            //Кнопки
            if (!this.buttonNotRead.Enabled) SQL[0].Append(" AND (on_read = 0) ");
            if (!this.buttonToDay.Enabled) SQL[0].Append(" AND ( `send_date` LIKE  '" + date.ToString("yyyy-MM-dd") + "%' ) ");
            if (!this.buttonWekly.Enabled) SQL[0].Append(" AND ( `send_date` >  '" + date.AddDays(-7).ToString("yyyy-MM-dd") +
                                                         "%' ) AND ( `send_date` < '" + date.AddDays(+1).ToString("yyyy-MM-dd") + "%' ) ");

            string[] SQLstring = new string[2];
            SQLstring[0] = SQL[0].ToString();
            SQLstring[1] = SQL[1].ToString();

            return SQLstring;
        }
#endregion
        
#region//################# Вкладка "ВНИМАНИЕ" 
// Три SQL команды для вкладки "Внимание"
        public void SQL_Alarm()
        {
            string SQL_POCHTA = "";
            string strSQL_Workc_user = "";
            string SQL_CHEREP_user = "";
            string SQL_CHEREP = "SELECT jos_projectlog_projects.id , name , deployment_from , release_date , release_id , title , deployment_to , location_spec , cherep_msq "
                              + "FROM jos_projectlog_projects , jos_users"

                              + " WHERE ( (jos_users.id = manager) AND (location_spec <> '') ";
         //                      + " WHERE (jos_zepp_pochta.to_user_id = " + Program.user.user_id
           //                   + ") AND (jos_zepp_pochta.tema = 1) AND ( jos_projectlog_projects.id = jos_zepp_pochta.project_id)  AND (jos_users.id = from_user_id)";
         
                                
            date = DateTime.Now;
            string strSQL_in_Workc = "SELECT * FROM jos_projectlog_projects WHERE (category =  8) AND (release_date < '" + date.ToString("yyyy-MM-dd") + "%' )";
            string strSQL_on_Workc = "SELECT id , release_date , release_id , title , deployment_to FROM jos_projectlog_projects WHERE (category = 7)  AND (deployment_to < '" + date.AddDays(-2).ToString("yyyy-MM-dd") + "%' ) ";


            if (Program.user.category_id != 11)
            {
                strSQL_Workc_user = " AND (manager = " + Program.user.user_id + " ) ";
                SQL_CHEREP_user = " AND (manager = " + Program.user.user_id + " ) ) OR (( category = 13 ) AND (manager = " + Program.user.user_id + " ) AND ( jos_users.id = manager ) );";
            } else
            {
                SQL_CHEREP_user = " );";
            }
            
            SQL_POCHTA = strSQL_in_Workc + strSQL_Workc_user + "ORDER BY release_date";
            cmd_on_Workc = new MySql.Data.MySqlClient.MySqlCommand(SQL_POCHTA, mySqlConnectionSQLZepp);
           
            SQL_POCHTA = strSQL_on_Workc + strSQL_Workc_user + "ORDER BY deployment_to";
            cmd_in_Workc = new MySql.Data.MySqlClient.MySqlCommand(SQL_POCHTA, mySqlConnectionSQLZepp);
            cmdCherep = new MySql.Data.MySqlClient.MySqlCommand(SQL_CHEREP + SQL_CHEREP_user, mySqlConnectionSQLZepp);
            
        }

//Иницилизация грида новые в работу
        public void init_onWorkcGrid()
        {
            idColumn = new DataGridViewTextBoxColumn();
            titleColumn = new DataGridViewTextBoxColumn();
            release_dateColumn = new DataGridViewTextBoxColumn();
            release_idColumn = new DataGridViewTextBoxColumn();
            deployment_toColumn = new DataGridViewTextBoxColumn();

            //onWorkcGrid = new DataGridView();
            //создание столбцов  
            idColumn.HeaderText = "id";
            idColumn.Name = "id";
            idColumn.Visible = false;

            titleColumn.HeaderText = "Название";
            titleColumn.Name = "title";

            release_dateColumn.HeaderText = "Должен быть сдан";
            release_dateColumn.Name = "release_date";
            release_dateColumn.ValueType = typeof(DateTimeConverter);

            release_idColumn.HeaderText = "Номер заказа";
            release_idColumn.Name = "release_id";
            release_idColumn.FillWeight = 62;

            deployment_toColumn.Name = "deployment_to";
            deployment_toColumn.HeaderText = "Поступил в работу";
            release_dateColumn.ValueType = typeof(DateTimeConverter);
            //создание столбцов  
            
            //добавление столбцов
            this.onWorkcGrid.Columns.Add(idColumn);
            this.onWorkcGrid.Columns.Add(release_idColumn);
            this.onWorkcGrid.Columns.Add(titleColumn);
            this.onWorkcGrid.Columns.Add(release_dateColumn);
            this.onWorkcGrid.Columns.Add(deployment_toColumn);
            //добавление столбцов              
           
        }

//Иницилизация грида в работе просроченные
        public void init_inWorkcGrid()
        {
            idColumn = new DataGridViewTextBoxColumn();
            titleColumn = new DataGridViewTextBoxColumn();
            release_dateColumn = new DataGridViewTextBoxColumn();
            release_idColumn = new DataGridViewTextBoxColumn();
            deployment_toColumn = new DataGridViewTextBoxColumn();

            //onWorkcGrid = new DataGridView();
            //создание столбцов  
            idColumn.HeaderText = "id";
            idColumn.Name = "id";
            idColumn.Visible = false;

            titleColumn.HeaderText = "Название";
            titleColumn.Name = "title";

            release_dateColumn.HeaderText = "Должен быть сдан";
            release_dateColumn.Name = "release_date";
            release_dateColumn.ValueType = typeof(DateTimeConverter);

            release_idColumn.HeaderText = "Номер заказа";
            release_idColumn.Name = "release_id";
            release_idColumn.FillWeight = 62;

            deployment_toColumn.Name = "deployment_to";
            deployment_toColumn.HeaderText = "Поступил в работу";
            release_dateColumn.ValueType = typeof(DateTimeConverter);
            //создание столбцов  

            //добавление столбцов
            this.inWorkcGrid.Columns.Add(idColumn);
            this.inWorkcGrid.Columns.Add(release_idColumn);
            this.inWorkcGrid.Columns.Add(titleColumn);
            this.inWorkcGrid.Columns.Add(release_dateColumn);
            this.inWorkcGrid.Columns.Add(deployment_toColumn);
            //добавление столбцов              

        }

//Иницилизация грида Отказанные
        public void init_CherepGrid()
        {
            /* 
             * jos_projectlog_projects.id   -- idColumn
             * name                        --  from_user_idColumn
             * release_date                 -- release_dateColumn
             * release_id                   -- release_idColumn
             * title                        -- titleColumn
             * deployment_to                -- deployment_toColumn
             * deployment_from              -- send_dateColumn
             * location_spec               -- textColumn
             * cherep_msq                  -- textColumn
             */
            idColumn = new DataGridViewTextBoxColumn();
            titleColumn = new DataGridViewTextBoxColumn();
            release_dateColumn = new DataGridViewTextBoxColumn();
            release_idColumn = new DataGridViewTextBoxColumn();
            deployment_toColumn = new DataGridViewTextBoxColumn();
            send_dateColumn = new DataGridViewTextBoxColumn();
            from_user_idColumn = new DataGridViewTextBoxColumn();
            textColumn = new DataGridViewTextBoxColumn();

            //создание столбцов  
            idColumn.HeaderText = "id";
            idColumn.Name = "id";
            idColumn.Visible = false;

            titleColumn.HeaderText = "Название";
            titleColumn.Name = "title";

            release_dateColumn.HeaderText = "Должен быть сдан";
            release_dateColumn.Name = "release_date";
            release_dateColumn.ValueType = typeof(DateTimeConverter);

            release_idColumn.HeaderText = "Номер заказа";
            release_idColumn.Name = "release_id";
            release_idColumn.FillWeight = 62;

            deployment_toColumn.Name = "deployment_to";
            deployment_toColumn.HeaderText = "Поступил в работу";
            deployment_toColumn.ValueType = typeof(DateTimeConverter);

            send_dateColumn.Name = "deployment_from";
            send_dateColumn.HeaderText = "Проблема выявлена";
            send_dateColumn.ValueType = typeof(DateTimeConverter);

            from_user_idColumn.Name = "name";
            from_user_idColumn.HeaderText = "Решение принял";

            textColumn.Name = "text";
            textColumn.HeaderText = "Пояснения";
            textColumn.Visible = false;                       
            //создание столбцов  

            //добавление столбцов
            this.CherepGrid.Columns.Add(idColumn);
            this.CherepGrid.Columns.Add(release_idColumn);
            this.CherepGrid.Columns.Add(titleColumn);
            this.CherepGrid.Columns.Add(release_dateColumn);
            this.CherepGrid.Columns.Add(deployment_toColumn);
            this.CherepGrid.Columns.Add(send_dateColumn);
            this.CherepGrid.Columns.Add(from_user_idColumn);
            this.CherepGrid.Columns.Add(textColumn);
            //добавление столбцов  
        }

        private void CherepGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.webBrowserCherep.DocumentText = "";
            if (e.RowIndex >= 0)
                    this.webBrowserCherep.DocumentText = this.CherepGrid.Rows[e.RowIndex].Cells["text"].Value.ToString();
            
            
        }

        private void webBrowserCherep_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.OriginalString != "about:blank")
            {
                e.Cancel = true;
                if (сброситьБраузерToolStripMenuItem.Checked)
                    System.Diagnostics.Process.Start(e.Url.OriginalString);
                else
                    System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, e.Url.OriginalString);
            }
        }
       

//>> Когда событие из трея обновило поля на Информационной вкладке:
        private void labelProjOn_TextChanged(object sender, EventArgs e)
        {
        // Обновляем поля "Взято но должно быть сдано"
            //init_onWorkcGrid();
            this.onWorkcGrid.Rows.Clear();
            //if (Program.pingServer(Program.mySqlConnectionSQLZepp))
           // {
                if (this.onWorkcGrid.ColumnCount > 0)
                {
                    SQL_Alarm();
                    try
                    {
                        while (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                        {
                            Thread.Sleep(1000);
                        }
                        mySqlConnectionSQLZepp.Open();
                        MySql.Data.MySqlClient.MySqlDataReader rdr = cmd_on_Workc.ExecuteReader();

                        while (rdr.Read())
                        {
                            idCell = new DataGridViewTextBoxCell();
                            release_idCell = new DataGridViewTextBoxCell();
                            titleCell = new DataGridViewTextBoxCell();
                            release_dateCell = new DataGridViewTextBoxCell();
                            deployment_toCell = new DataGridViewTextBoxCell();
                            row = new DataGridViewRow();
                            idCell.Value = rdr["id"].ToString();
                            release_idCell.Value = rdr["release_id"].ToString();
                            titleCell.Value = rdr["title"].ToString();
                            try { release_dateCell.Value = rdr["release_date"]; }
                            catch { release_dateCell.ErrorText = "Не установленна дата"; }
                            try { deployment_toCell.Value = rdr["deployment_to"]; }
                            catch { deployment_toCell.ErrorText = "Не установленна дата"; }
                            row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell);
                            this.onWorkcGrid.Rows.Add(row);
                        }
                        mySqlConnectionSQLZepp.Close();
                    }
                    catch
                    {
                        idCell = new DataGridViewTextBoxCell();
                        release_idCell = new DataGridViewTextBoxCell();
                        titleCell = new DataGridViewTextBoxCell();
                        release_dateCell = new DataGridViewTextBoxCell();
                        deployment_toCell = new DataGridViewTextBoxCell();
                        row = new DataGridViewRow();
                        idCell.Value = "0";
                        release_idCell.Value = "Ошибка при чтении базы (FormMain 001)";
                        titleCell.Value = "";
                        release_dateCell.Value = "";
                        deployment_toCell.Value = "";
                        row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell);
                        this.onWorkcGrid.Rows.Add(row);
                        if (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                                mySqlConnectionSQLZepp.Close();
                    }
                }
                this.Height = this.Height - 1; this.Height = this.Height + 1;
            //}
        }

        private void labelProjIn_TextChanged(object sender, EventArgs e)
        {           
            // Обновляем поля "Взято но должно быть сдано"
           // init_inWorkcGrid();
            this.inWorkcGrid.Rows.Clear();
           // if (Program.pingServer(Program.mySqlConnectionSQLZepp))
           // {
                if (this.inWorkcGrid.ColumnCount > 0)
                {
                    SQL_Alarm();
                    try
                    {
                        while (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                        {
                            Thread.Sleep(1000);
                        }
                        mySqlConnectionSQLZepp.Open();
                        MySql.Data.MySqlClient.MySqlDataReader rdr = cmd_in_Workc.ExecuteReader();

                        while (rdr.Read())
                        {
                            idCell = new DataGridViewTextBoxCell();
                            release_idCell = new DataGridViewTextBoxCell();
                            titleCell = new DataGridViewTextBoxCell();
                            release_dateCell = new DataGridViewTextBoxCell();
                            deployment_toCell = new DataGridViewTextBoxCell();
                            row = new DataGridViewRow();
                            idCell.Value = rdr["id"].ToString();
                            release_idCell.Value = rdr["release_id"].ToString();
                            titleCell.Value = rdr["title"].ToString();
                            try { release_dateCell.Value = rdr["release_date"]; }
                            catch { release_dateCell.ErrorText = "Не установленна дата"; }
                            try { deployment_toCell.Value = rdr["deployment_to"]; }
                            catch { deployment_toCell.ErrorText = "Не установленна дата"; }
                            row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell);
                            this.inWorkcGrid.Rows.Add(row);
                        }
                        mySqlConnectionSQLZepp.Close();
                    }
                    catch
                    {
                        idCell = new DataGridViewTextBoxCell();
                        release_idCell = new DataGridViewTextBoxCell();
                        titleCell = new DataGridViewTextBoxCell();
                        release_dateCell = new DataGridViewTextBoxCell();
                        deployment_toCell = new DataGridViewTextBoxCell();
                        row = new DataGridViewRow();
                        idCell.Value = "0";
                        release_idCell.Value = "Ошибка при чтении базы (FormMain 002)";
                        titleCell.Value = "";
                        release_dateCell.Value = "";
                        deployment_toCell.Value = "";
                        row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell);
                        this.inWorkcGrid.Rows.Add(row);
                       if( mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                            mySqlConnectionSQLZepp.Close();
                    }
                }
                this.Height = this.Height - 1; this.Height = this.Height + 1;
            //}
        }

        private void labelCherepText_TextChanged(object sender, EventArgs e)
        {
            if (this.CherepGrid.ColumnCount > 0)
            {
                // Обновляем поля "Отказанно"
                //init_CherepGrid();
                if (Program.user.category_id == 11)
                {
                    this.linkLabelCherep.Visible = false;
                    this.labelBrak.Visible = true;
                } else
                {
                    this.linkLabelCherep.Visible = true;
                    this.labelBrak.Visible = false;
                }
                this.CherepGrid.Rows.Clear();
                // this.webBrowserCherep.DocumentText = "";
                //if (Program.pingServer(Program.mySqlConnectionSQLZepp))
                //{
                    SQL_Alarm();

                    try
                    {
                        while (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                        {
                            Thread.Sleep(1000);
                        }
                        mySqlConnectionSQLZepp.Open();
                        MySql.Data.MySqlClient.MySqlDataReader rdr = cmdCherep.ExecuteReader();


                        while (rdr.Read())
                        {
                            /* 
                            * jos_projectlog_projects.id   -- idColumn
                            * name                        --  from_user_idColumn
                            * release_date                 -- release_dateColumn
                            * release_id                   -- release_idColumn
                            * title                        -- titleColumn
                            * deployment_to                -- deployment_toColumn
                            * deployment_from              -- send_dateColumn
                            * location_spec               -- textColumn
                            * cherep_msq                  -- textColumn
                            */
                            idCell = new DataGridViewTextBoxCell();
                            release_idCell = new DataGridViewTextBoxCell();
                            titleCell = new DataGridViewTextBoxCell();
                            release_dateCell = new DataGridViewTextBoxCell();
                            deployment_toCell = new DataGridViewTextBoxCell();
                            send_dateCell = new DataGridViewTextBoxCell();
                            from_user_idCell = new DataGridViewTextBoxCell();
                            textCell = new DataGridViewTextBoxCell();

                            row = new DataGridViewRow();


                            idCell.Value = rdr["id"].ToString();
                            release_idCell.Value = rdr["release_id"].ToString();
                            titleCell.Value = rdr["title"].ToString();
                            try { release_dateCell.Value = rdr["release_date"]; }
                            catch { release_dateCell.ErrorText = "Не установленна дата"; }
                            try { deployment_toCell.Value = rdr["deployment_to"]; }
                            catch { deployment_toCell.ErrorText = "Не установленна дата"; }

                            try { send_dateCell.Value = rdr["deployment_from"]; }
                            catch { send_dateCell.ErrorText = "Програмнная ошибка"; }

                            textCell.Value = rdr["location_spec"].ToString();
                            from_user_idCell.Value = rdr["name"].ToString();
                            if (textCell.Value.ToString() == "")
                            {
                                textCell.Value = "<strong>Выполнение под угрозой </strong> <br /> <b> Причина: </b> <i> " + rdr["cherep_msq"].ToString() + " </i>";
                                from_user_idCell.Value = "Начальник производства";
                            } else
                            {
                                textCell.Value = "<strong> Выявлен БРАК !!! </strong> <br /><b>Комментарий: </b> <i> " + textCell.Value + " </i>";
                            }


                            row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell, send_dateCell,
                                from_user_idCell, textCell);
                            this.CherepGrid.Rows.Add(row);
                        }
                        mySqlConnectionSQLZepp.Close();
                    }
                    catch
                    {
                        idCell = new DataGridViewTextBoxCell();
                        release_idCell = new DataGridViewTextBoxCell();
                        titleCell = new DataGridViewTextBoxCell();
                        release_dateCell = new DataGridViewTextBoxCell();
                        deployment_toCell = new DataGridViewTextBoxCell();
                        textCell = new DataGridViewTextBoxCell();
                        row = new DataGridViewRow();
                        send_dateCell = new DataGridViewTextBoxCell();
                        from_user_idCell = new DataGridViewTextBoxCell();

                        idCell.Value = "0";
                        release_idCell.Value = "Ошибка при чтении базы (FormMain 003)";
                        titleCell.Value = "";
                        release_dateCell.Value = "";
                        deployment_toCell.Value = "";
                        textCell.Value = "";
                        send_dateCell.Value = "";
                        from_user_idCell.Value = "";

                        row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell, send_dateCell,
                            from_user_idCell, textCell);
                        this.CherepGrid.Rows.Add(row);
                        if (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                            mySqlConnectionSQLZepp.Close();
                    }
               // }
                this.Height = this.Height - 1; this.Height = this.Height + 1;
                int H = this.CherepGrid.Rows.GetRowsHeight(DataGridViewElementStates.Visible);

                if ((this.CherepGrid.Rows.Count > 2) && (this.CherepGrid.Rows.Count < 7))
                {
                    int last = panel1.Height;
                    panel1.Height = 60 + H;
                    panel2.Height = panel2.Height - (panel1.Height - last);
                    //DockStyle.Fill; //
                    int newY = panel2.Location.Y + (panel1.Height - last);
                    panel2.Location = new Point(000, newY);
                }
                if (this.CherepGrid.Rows.Count >6)
                {
                    int last = panel1.Height;
                    panel1.Height =192;
                    panel2.Height = panel2.Height - (192 - last);
                    //DockStyle.Fill; //
                    int newY = panel2.Location.Y + (192 - last);
                    panel2.Location = new Point(000, newY);
                }
                if (this.CherepGrid.Rows.Count < 3)
                {
                    panel1.Height = 110;
                    panel2.Height = 290;
                    panel2.Location = new Point(000, 113);
                }
                this.Height = this.Height - 1; this.Height = this.Height + 1;
                
            }
        }
 //<< Когда событие из трея обновило поля на Информационной вкладке:

//>> Ссылки
        private void onWorkcGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            object test = e;
            if (сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.onWorkcGrid.Rows[e.RowIndex].Cells["id"].Value);
            else
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.onWorkcGrid.Rows[e.RowIndex].Cells["id"].Value);          
        }

        private void inWorkcGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            object test = e;
            if (сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.inWorkcGrid.Rows[e.RowIndex].Cells["id"].Value);
            else
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.inWorkcGrid.Rows[e.RowIndex].Cells["id"].Value);

        }

        private void CherepGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            object test = e;
            if (сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp
                +"index.php?option=com_projectlog&view=project&id="+this.CherepGrid.Rows[e.RowIndex].Cells["id"].Value);
            else
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.CherepGrid.Rows[e.RowIndex].Cells["id"].Value);
            
        }

        private void linkLabelCherep_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp
                            + "index.php?filter=3&search=&limit=20&option=com_projectlog&view=cat&Itemid=50&filter_order=p.release_date&filter_order_Dir=DESC&project_edit=&id=13&task=");
            else
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp
                            + "index.php?filter=3&search=&limit=20&option=com_projectlog&view=cat&Itemid=50&filter_order=p.release_date&filter_order_Dir=DESC&project_edit=&id=13&task=");
        }

        private void linkLabelOn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp
                +"index.php?filter=3&search=&limit=20&option=com_projectlog&view=cat&Itemid=50&filter_order=p.release_date&filter_order_Dir=DESC&project_edit=&id=7&task=");
            else
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?filter=3&search=&limit=20&option=com_projectlog&view=cat&Itemid=50&filter_order=p.release_date&filter_order_Dir=DESC&project_edit=&id=7&task=");
        }

        private void linkLabelIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp
                +"index.php?filter=3&search=&limit=20&option=com_projectlog&view=cat&Itemid=51&filter_order=p.release_date&filter_order_Dir=DESC&project_edit=&id=8&task=");
            else
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?filter=3&search=&limit=20&option=com_projectlog&view=cat&Itemid=51&filter_order=p.release_date&filter_order_Dir=DESC&project_edit=&id=8&task=");
        }
        //<< Ссылки
#endregion

#region//################# Вкладка "Передвижения" 
        // Активировона вкладка передвижение
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            this.tabPageMove.Controls.Add(this.panelFilter);
            this.tabPageMove.Controls.Add(this.panelDeleted);
            fillingMov();
        }
        
//Иницилизация грида Передвижение
        public void init_MovGrid()
        {
            /* id
             * title Название ,
             * from_user_id Кто принял решение,
             * send_date Когда принято решение,
             * project_id ID проекта,
             * release_date Дата реализации,
             * release_id , Номер проекта
             * text  Пояснение ,
             * deployment_to Дата передвижения"
             */
            idColumn = new DataGridViewTextBoxColumn();
            titleColumn = new DataGridViewTextBoxColumn();
            release_dateColumn = new DataGridViewTextBoxColumn();
            release_idColumn = new DataGridViewTextBoxColumn();
            deployment_toColumn = new DataGridViewTextBoxColumn();
            send_dateColumn = new DataGridViewTextBoxColumn();
            from_user_idColumn = new DataGridViewTextBoxColumn();
            textColumn = new DataGridViewTextBoxColumn();
            pochta_idColumn = new DataGridViewTextBoxColumn();
            on_readColumn = new DataGridViewTextBoxColumn();
                        
            //создание столбцов  
            idColumn.HeaderText = "id";
            idColumn.Name = "id";
            idColumn.Visible = false;

            titleColumn.HeaderText = "Название";
            titleColumn.Name = "title";

            release_dateColumn.HeaderText = "Должен быть сдан";
            release_dateColumn.Name = "release_date";
            release_dateColumn.ValueType = typeof(DateTimeConverter);
            release_dateColumn.DefaultCellStyle.Format = "dd'.'MM'.'yyyy";

            release_idColumn.HeaderText = "Номер заказа";
            release_idColumn.Name = "release_id";
            release_idColumn.FillWeight = 62;

            deployment_toColumn.Name = "deployment_to";
            deployment_toColumn.HeaderText = "Поступил в работу";
            deployment_toColumn.ValueType = typeof(DateTimeConverter);

            send_dateColumn.Name = "send_date";
            send_dateColumn.HeaderText = "Дата";
            send_dateColumn.ValueType = typeof(DateTimeConverter);

            from_user_idColumn.Name = "name";
            from_user_idColumn.HeaderText = "Решение принял";

            textColumn.Name = "text";
            textColumn.HeaderText = "Пояснения";
            textColumn.Visible = false;

            pochta_idColumn.Name = "pochta_id";
            pochta_idColumn.HeaderText = "pochta_idColumn";
            pochta_idColumn.Visible = false;

            on_readColumn.Name = "on_read";
            on_readColumn.HeaderText = "on_readColumn";
            on_readColumn.Visible = false;
            //создание столбцов  

            //добавление столбцов
            //row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell, from_user_idCell, send_dateCell, textCell);
            this.dataGridViewMove.Columns.Add(idColumn);
            this.dataGridViewMove.Columns.Add(release_idColumn);
            this.dataGridViewMove.Columns.Add(titleColumn);
            this.dataGridViewMove.Columns.Add(release_dateColumn);
            this.dataGridViewMove.Columns.Add(deployment_toColumn);            
            this.dataGridViewMove.Columns.Add(from_user_idColumn);
            this.dataGridViewMove.Columns.Add(send_dateColumn);
            this.dataGridViewMove.Columns.Add(textColumn);
            this.dataGridViewMove.Columns.Add(pochta_idColumn);
            this.dataGridViewMove.Columns.Add(on_readColumn);
            //добавление столбцов              

        }

// SQL команда для вкладки "Передвижение"
        public void SQL_Mov()
        {
            date = DateTime.Now;
            string[] SQL = new string[2];
            SQL = filter();
            string SQL_Mov =
            "SELECT jos_projectlog_projects.id , name , text , from_user_id , send_date , project_id ,  release_date , release_id , title , deployment_to , pochta_id , on_read "
                              + "FROM jos_zepp_pochta , jos_projectlog_projects , jos_users"
                              + " WHERE ( to_user_id = " + Program.user.user_id + " ) "
                              + " AND (jos_zepp_pochta.tema = 3) AND ( jos_projectlog_projects.id = jos_zepp_pochta.project_id) AND (jos_users.id = from_user_id)"
                                + SQL[0] + SQL[1];
          
            cmd_Mov = new MySql.Data.MySqlClient.MySqlCommand(SQL_Mov, mySqlConnectionSQLZepp);
          

        }

// Заполняем грид Передвижение
        private void  fillingMov()
        {
            this.dataGridViewMove.Rows.Clear();
            this.webBrowserMov.DocumentText = "";
           // if (Program.pingServer(Program.mySqlConnectionSQLZepp))
            //{
                // Обновляем поля "Взято но должно быть сдано"
                SQL_Mov();
                //MessageBox.Show(cmd_Mov.CommandText.ToString());
                try
                {
                    while (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                    {
                        Thread.Sleep(1000);
                    }

                    mySqlConnectionSQLZepp.Open();
                    MySql.Data.MySqlClient.MySqlDataReader rdr = cmd_Mov.ExecuteReader();
                    

                    while (rdr.Read())
                    {
                        idCell = new DataGridViewTextBoxCell();
                        release_idCell = new DataGridViewTextBoxCell();
                        titleCell = new DataGridViewTextBoxCell();
                        release_dateCell = new DataGridViewTextBoxCell();
                        deployment_toCell = new DataGridViewTextBoxCell();
                        send_dateCell = new DataGridViewTextBoxCell();
                        from_user_idCell = new DataGridViewTextBoxCell();
                        textCell = new DataGridViewTextBoxCell();
                        pochta_idCell = new DataGridViewTextBoxCell();
                        on_readCell = new DataGridViewTextBoxCell();
                        row = new DataGridViewRow();

                        idCell.Value = rdr["id"].ToString();
                        release_idCell.Value = rdr["release_id"].ToString();
                        titleCell.Value = rdr["title"].ToString();
                        try { release_dateCell.Value = rdr["release_date"]; }
                        catch { release_dateCell.ErrorText = "Не установленна дата"; }
                        try { deployment_toCell.Value = rdr["deployment_to"]; }
                        catch { deployment_toCell.ErrorText = "Не установленна дата"; }
                        try { send_dateCell.Value = rdr["send_date"]; }
                        catch { send_dateCell.ErrorText = "Не установленна дата"; }
                        from_user_idCell.Value = rdr["name"].ToString();
                        textCell.Value = rdr["text"].ToString();
                        pochta_idCell.Value = rdr["pochta_id"];
                        on_readCell.Value = rdr["on_read"];

                        if ((int)on_readCell.Value == 0) row.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);

                        row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell, from_user_idCell, send_dateCell, textCell, pochta_idCell, on_readCell);
                        this.dataGridViewMove.Rows.Add(row);
                    }
                    mySqlConnectionSQLZepp.Close();
                }
                catch
                {
                    idCell = new DataGridViewTextBoxCell();
                    release_idCell = new DataGridViewTextBoxCell();
                    titleCell = new DataGridViewTextBoxCell();
                    release_dateCell = new DataGridViewTextBoxCell();
                    deployment_toCell = new DataGridViewTextBoxCell();
                    send_dateCell = new DataGridViewTextBoxCell();
                    from_user_idCell = new DataGridViewTextBoxCell();
                    textCell = new DataGridViewTextBoxCell();
                    pochta_idCell = new DataGridViewTextBoxCell();
                    on_readCell = new DataGridViewTextBoxCell();
                    row = new DataGridViewRow();

                    idCell.Value = "0";
                    release_idCell.Value = "Ошибка при чтении базы (FormMain 004)";
                    titleCell.Value = "";
                    release_dateCell.Value = "";
                    deployment_toCell.Value = "";
                    send_dateCell.Value = "";
                    from_user_idCell.Value = "";
                    textCell.Value = "";
                    pochta_idCell.Value = -1;
                    on_readCell.Value = -1;
                    row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell, from_user_idCell, send_dateCell, textCell, pochta_idCell, on_readCell);
                    this.dataGridViewMove.Rows.Add(row);
                    if (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                         mySqlConnectionSQLZepp.Close();
                }
            //}
        }        

        private void dataGridViewMove_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&
               ((int)this.dataGridViewMove.Rows[e.RowIndex].Cells["on_read"].Value == 0)
               && Program.pingServer(mySqlConnectionSQLZepp)
               )
            {
                //MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
                //mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
                //mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;


                //Изменить флаг прочитано в таблице почты
                string strSQL = "UPDATE  `jos_zepp_pochta` SET  `on_read` =  '1', `read_date` = NOW( ) WHERE  `jos_zepp_pochta`.`pochta_id` ="
                    + this.dataGridViewMove.Rows[e.RowIndex].Cells["pochta_id"].Value.ToString()
                    + " LIMIT 1 ;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);
                while (true)
                {
                    int intRecordsAffected = 0;
                    try
                    {
                        mySqlConnectionSQLZepp.Open();
                        intRecordsAffected = cmd.ExecuteNonQuery();
                        mySqlConnectionSQLZepp.Close();
                    }
                    catch {
                    }
                    if (intRecordsAffected > 0)
                    {
                        // Обновить браузер программы
                        this.dataGridViewMove.Rows[e.RowIndex].Cells["on_read"].Value = 1;
                        this.webBrowserMov.DocumentText = "";
                        this.dataGridViewMove.Rows[e.RowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Regular);
                        if (e.RowIndex >= 0)
                            this.webBrowserMov.DocumentText = this.dataGridViewMove.Rows[e.RowIndex].Cells["text"].Value.ToString();

                        break;
                        //Console.WriteLineC"Update succeeded");
                    }
                    Thread.Sleep(1000);
                }
            } 
            this.webBrowserMov.DocumentText = "";
            if (e.RowIndex >= 0)
               this.webBrowserMov.DocumentText = this.dataGridViewMove.Rows[e.RowIndex].Cells["text"].Value.ToString();
            this.Height = this.Height - 1; this.Height = this.Height + 1;
        }

        private void webBrowserMov_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.OriginalString != "about:blank")
            {
                e.Cancel = true;
                if (сброситьБраузерToolStripMenuItem.Checked)
                    System.Diagnostics.Process.Start(e.Url.OriginalString);
                else
                    System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, e.Url.OriginalString);
            }
           
        }

//>> Когда событие из трея обновило поля на Информационной вкладке:
        private void labelMoveText_TextChanged(object sender, EventArgs e)
        {
            if (this.tabPageMove.Focused && !this.buttonNotRead.Enabled)
            {
                fillingMov();                
            }
            this.Height = this.Height - 1; this.Height = this.Height + 1;
        }
//<< Когда событие из трея обновило поля на Информационной вкладке:
        private void dataGridViewMove_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            object test = e;
            if (сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.dataGridViewMove.Rows[e.RowIndex].Cells["id"].Value);
            else
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.dataGridViewMove.Rows[e.RowIndex].Cells["id"].Value);

        }

#endregion

#region//################# Вкладка Сообщений
// Активировона вкладка Сообщений
        private void tabPage4_Enter(object sender, EventArgs e)
        {
            this.tabPageComment.Controls.Add(this.panelFilter);
            this.tabPageComment.Controls.Add(this.panelDeleted);
            fillingComment();
        }

// Заполняем грид Сообщений
        private void fillingComment()
        {
            this.dataGridViewComment.Rows.Clear();
            this.webBrowserComment.DocumentText = "";
           // if (Program.pingServer(Program.mySqlConnectionSQLZepp))
           // {
                // Обновляем поля 
                SQL_Comment();
                //MessageBox.Show(cmd_Mov.CommandText.ToString());
                try
                {
                    while (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                    {
                        Thread.Sleep(1000);
                    }
                    mySqlConnectionSQLZepp.Open();
                    MySql.Data.MySqlClient.MySqlDataReader rdr = cmd_Comment.ExecuteReader();


                    while (rdr.Read())
                    {
            /* id              -idCell     -               
             * title            -titleCell  -Название ,
             * from_user_id     -from_user_idCell   -Кто принял решение,
             * send_date        -send_dateCell  -Когда принято решение,
             * project_id ID    -pochta_idCell  -проекта,
             * release_date     -release_dateCell   -Дата реализации,
             * release_id       -release_idCell     - Номер проекта 
             * text             -textCell   -Пояснение ,
             * deployment_to    -deployment_toCell  -Дата передвижения
             * on_read          -on_readCell
             */
                        idCell = new DataGridViewTextBoxCell();
                        release_idCell = new DataGridViewTextBoxCell();
                        titleCell = new DataGridViewTextBoxCell();
                        release_dateCell = new DataGridViewTextBoxCell();
                        deployment_toCell = new DataGridViewTextBoxCell();
                        send_dateCell = new DataGridViewTextBoxCell();
                        from_user_idCell = new DataGridViewTextBoxCell();
                        textCell = new DataGridViewTextBoxCell();
                        pochta_idCell = new DataGridViewTextBoxCell();
                        on_readCell = new DataGridViewTextBoxCell();
                        row = new DataGridViewRow();

                        idCell.Value = rdr["id"].ToString();
                        release_idCell.Value = rdr["release_id"].ToString();
                        titleCell.Value = rdr["title"].ToString();
                        try { release_dateCell.Value = rdr["release_date"]; }
                        catch { release_dateCell.ErrorText = "Не установленна дата"; }
                        try { deployment_toCell.Value = rdr["deployment_to"]; }
                        catch { deployment_toCell.ErrorText = "Не установленна дата"; }
                        try { send_dateCell.Value = rdr["send_date"]; }
                        catch { send_dateCell.ErrorText = "Не установленна дата"; }
                        from_user_idCell.Value = rdr["name"].ToString();
                        textCell.Value = rdr["text"].ToString();
                        pochta_idCell.Value = rdr["pochta_id"];
                        on_readCell.Value = rdr["on_read"];


                        if ((int)on_readCell.Value == 0) row.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);

                        row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell,
                            deployment_toCell, from_user_idCell, send_dateCell, textCell, pochta_idCell, on_readCell);
                        this.dataGridViewComment.Rows.Add(row);
                    }
                    mySqlConnectionSQLZepp.Close();
                }
                catch
                {
                    idCell = new DataGridViewTextBoxCell();
                    release_idCell = new DataGridViewTextBoxCell();
                    titleCell = new DataGridViewTextBoxCell();
                    release_dateCell = new DataGridViewTextBoxCell();
                    deployment_toCell = new DataGridViewTextBoxCell();
                    send_dateCell = new DataGridViewTextBoxCell();
                    from_user_idCell = new DataGridViewTextBoxCell();
                    textCell = new DataGridViewTextBoxCell();
                    pochta_idCell = new DataGridViewTextBoxCell();
                    on_readCell = new DataGridViewTextBoxCell();
                    row = new DataGridViewRow();

                   idCell.Value = "0";
                   release_idCell.Value = "Ошибка при чтении базы (FormMain 005)";
                   titleCell.Value = "";
                   release_dateCell.Value = "";
                   deployment_toCell.Value = "";
                   send_dateCell.Value = "";
                   from_user_idCell.Value = "";
                   textCell.Value = "";
                   pochta_idCell.Value = -1;
                   on_readCell.Value = -1;
                   row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell, from_user_idCell,
                       send_dateCell, textCell, pochta_idCell, on_readCell);
                   this.dataGridViewComment.Rows.Add(row);
                   if (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                    mySqlConnectionSQLZepp.Close();
               }
            //}
        }

//Иницилизация грида Сообщений
        public void init_CommentGrid()
        {
            /* id
             * title Название ,
             * from_user_id Кто принял решение,
             * send_date Когда принято решение,
             * project_id ID проекта,
             * release_date Дата реализации,
             * release_id , Номер проекта
             * text  Пояснение ,
             * deployment_to Дата передвижения"
             */
            idColumn = new DataGridViewTextBoxColumn();
            titleColumn = new DataGridViewTextBoxColumn();
            release_dateColumn = new DataGridViewTextBoxColumn();
            release_idColumn = new DataGridViewTextBoxColumn();
            deployment_toColumn = new DataGridViewTextBoxColumn();
            send_dateColumn = new DataGridViewTextBoxColumn();
            from_user_idColumn = new DataGridViewTextBoxColumn();
            textColumn = new DataGridViewTextBoxColumn();
            pochta_idColumn = new DataGridViewTextBoxColumn();
            on_readColumn = new DataGridViewTextBoxColumn();

            //создание столбцов  
            idColumn.HeaderText = "id";
            idColumn.Name = "id";
            idColumn.Visible = false;

            titleColumn.HeaderText = "Название";
            titleColumn.Name = "title";

            release_dateColumn.HeaderText = "Должен быть сдан";
            release_dateColumn.Name = "release_date";
            release_dateColumn.ValueType = typeof(DateTimeConverter);
            release_dateColumn.DefaultCellStyle.Format = "dd'.'MM'.'yyyy";

            release_idColumn.HeaderText = "Номер заказа";
            release_idColumn.Name = "release_id";
            release_idColumn.FillWeight = 62;

            deployment_toColumn.Name = "deployment_to";
            deployment_toColumn.HeaderText = "Поступил в работу";
            deployment_toColumn.ValueType = typeof(DateTimeConverter);

            send_dateColumn.Name = "send_date";
            send_dateColumn.HeaderText = "Дата";
            send_dateColumn.ValueType = typeof(DateTimeConverter);

            from_user_idColumn.Name = "name";
            from_user_idColumn.HeaderText = "Решение принял";

            textColumn.Name = "text";
            textColumn.HeaderText = "Поснения";
            textColumn.Visible = false;

            pochta_idColumn.Name = "pochta_id";
            pochta_idColumn.HeaderText = "pochta_idColumn";
            pochta_idColumn.Visible = false;

            on_readColumn.Name = "on_read";
            on_readColumn.HeaderText = "on_readColumn";
            on_readColumn.Visible = false;
            //создание столбцов  

            //добавление столбцов
            //row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell, from_user_idCell, send_dateCell, textCell);
            this.dataGridViewComment.Columns.Add(idColumn);
            this.dataGridViewComment.Columns.Add(release_idColumn);
            this.dataGridViewComment.Columns.Add(titleColumn);
            this.dataGridViewComment.Columns.Add(release_dateColumn);
            this.dataGridViewComment.Columns.Add(deployment_toColumn);
            this.dataGridViewComment.Columns.Add(from_user_idColumn);
            this.dataGridViewComment.Columns.Add(send_dateColumn);
            this.dataGridViewComment.Columns.Add(textColumn);
            this.dataGridViewComment.Columns.Add(pochta_idColumn);
            this.dataGridViewComment.Columns.Add(on_readColumn);
            //добавление столбцов              

        }


// SQL команда для вкладки "Сообщений"
        public void SQL_Comment()
        {
            /* id  title from_user_id send_date project_id ID release_date release_id text deployment_to on_read      
             */
            date = DateTime.Now;
            string[] SQL = new string[2];
            SQL = filter();
            string SQL_Comment =
            "SELECT jos_projectlog_projects.id , name , text , from_user_id , send_date , project_id ,  release_date , release_id , title , deployment_to , pochta_id , on_read "
                              + "FROM jos_zepp_pochta , jos_projectlog_projects , jos_users"
                              + " WHERE ( to_user_id = " + Program.user.user_id + " ) "
                              + " AND (jos_zepp_pochta.tema = 4) AND ( jos_projectlog_projects.id = jos_zepp_pochta.project_id) AND (jos_users.id = from_user_id)"
                                + SQL[0] + SQL[1];

            cmd_Comment = new MySql.Data.MySqlClient.MySqlCommand(SQL_Comment, mySqlConnectionSQLZepp);

        }

        private void dataGridViewComment_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&
              ((int)this.dataGridViewComment.Rows[e.RowIndex].Cells["on_read"].Value == 0)
              && Program.pingServer(mySqlConnectionSQLZepp)
              )
            {
                //MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
                //mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
                //mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;


                //Изменить флаг прочитано в таблице почты
                string strSQL = "UPDATE  `jos_zepp_pochta` SET  `on_read` =  '1', `read_date` = NOW( ) WHERE  `jos_zepp_pochta`.`pochta_id` ="
                    + this.dataGridViewComment.Rows[e.RowIndex].Cells["pochta_id"].Value.ToString()
                    + " LIMIT 1 ;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);
                while (true)
                {
                    int intRecordsAffected = 0;
                    try
                    {
                        mySqlConnectionSQLZepp.Open();
                        intRecordsAffected = cmd.ExecuteNonQuery();
                        mySqlConnectionSQLZepp.Close();
                    }
                    catch
                    {
                    }
                    if (intRecordsAffected > 0)
                    {
                        // Обновить браузер программы
                        this.dataGridViewComment.Rows[e.RowIndex].Cells["on_read"].Value = 1;
                        this.dataGridViewComment.Rows[e.RowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Regular);
                        this.webBrowserComment.DocumentText = "";
                        if (e.RowIndex >= 0)
                            this.webBrowserComment.DocumentText = this.dataGridViewComment.Rows[e.RowIndex].Cells["text"].Value.ToString();

                        break;
                        //Console.WriteLineC"Update succeeded");
                    }
                    Thread.Sleep(1000);
                }
            }
            this.webBrowserComment.DocumentText = "";
            if (e.RowIndex >= 0)
                this.webBrowserComment.DocumentText = this.dataGridViewComment.Rows[e.RowIndex].Cells["text"].Value.ToString();
            //DrawItemEventArgs w = null;
            this.Height = this.Height - 1; this.Height = this.Height + 1;

        }

        private void webBrowserComment_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.OriginalString != "about:blank")
            {
                e.Cancel = true;
                if (сброситьБраузерToolStripMenuItem.Checked)
                    System.Diagnostics.Process.Start(e.Url.OriginalString);
                else
                    System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, e.Url.OriginalString);
            }

        }

//>> Когда событие из трея обновило поля на Информационной вкладке:
        private void labelCommentText_TextChanged(object sender, EventArgs e)
        {
            //tabPage4_Enter(sender, e);
            if (this.tabPageComment.Focused && !this.buttonNotRead.Enabled)
            {
                fillingComment();               
            }
            this.Height = this.Height - 1; this.Height = this.Height + 1;
        }
      
 //<< Когда событие из трея обновило поля на Информационной вкладке:
        private void dataGridViewComment_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            object test = e;

            if (сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.dataGridViewComment.Rows[e.RowIndex].Cells["id"].Value);
            else 
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp
                + "index.php?option=com_projectlog&view=project&id=" + this.dataGridViewComment.Rows[e.RowIndex].Cells["id"].Value);

            dataGridViewComment_RowEnter(sender, e);
        }

        
#endregion

#region//################# Вкладка Удалено

        // Активировона вкладка 
        private void tabPageDel_Enter(object sender, EventArgs e)
        {
            this.tabPageDel.Controls.Add(this.panelFilter);
            this.tabPageDel.Controls.Add(this.panelDeleted);
            fillingDel();
        }

        // Заполняем грид 
        private void fillingDel()
        {
            this.dataGridViewDel.Rows.Clear();
            this.webBrowserDel.DocumentText = "";
           // if (Program.pingServer(Program.mySqlConnectionSQLZepp))
           // {
                // Обновляем поля 
                SQL_Del();                
                //MessageBox.Show(cmd_Mov.CommandText.ToString());
                try
                {
                    while (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                    {
                        Thread.Sleep(1000);
                    }
                    // Заполняем удаленные
                   
                    mySqlConnectionSQLZepp.Open();
                    MySql.Data.MySqlClient.MySqlDataReader rdr = cmd_Del.ExecuteReader();
                                       
                    while (rdr.Read())
                    {
             /* name ,
             * text ,           textColumn
             * from_user_id ,   from_user_idColumn
             * send_date ,      send_dateColumn
             * project_id ,     idColumn
             * pochta_id ,      pochta_idColumn
             * on_read "        on_readColumn
             */
                        idCell = new DataGridViewTextBoxCell();
                        send_dateCell = new DataGridViewTextBoxCell();
                        from_user_idCell = new DataGridViewTextBoxCell();
                        textCell = new DataGridViewTextBoxCell();
                        pochta_idCell = new DataGridViewTextBoxCell();
                        on_readCell = new DataGridViewTextBoxCell();
                        row = new DataGridViewRow();

                        idCell.Value = rdr["project_id"].ToString();
                        try { send_dateCell.Value = rdr["send_date"]; }
                        catch { send_dateCell.ErrorText = "Не установленна дата"; }
                        from_user_idCell.Value = rdr["name"].ToString();
                        textCell.Value = rdr["text"].ToString();
                        pochta_idCell.Value = rdr["pochta_id"];
                        on_readCell.Value = rdr["on_read"];

                        if ((int)on_readCell.Value == 0) row.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);

                        row.Cells.AddRange(idCell, from_user_idCell, send_dateCell, textCell, pochta_idCell, on_readCell);
                        this.dataGridViewDel.Rows.Add(row);
                    }
                    mySqlConnectionSQLZepp.Close();

                    mySqlConnectionSQLZepp.Open();
                    
                    rdr = cmd_Err2.ExecuteReader();
                                       
                    
                    if (rdr.HasRows)
                    {
                        idCell = new DataGridViewTextBoxCell();
                        send_dateCell = new DataGridViewTextBoxCell();
                        from_user_idCell = new DataGridViewTextBoxCell();
                        textCell = new DataGridViewTextBoxCell();
                        pochta_idCell = new DataGridViewTextBoxCell();
                        on_readCell = new DataGridViewTextBoxCell();
                        row = new DataGridViewRow();

                        idCell.Value = "";
                        send_dateCell.Value = " ни одной категории:";
                        send_dateCell.Style.BackColor = Color.LightGray;
                        from_user_idCell.Value = "Эти проекты не принадлежат ";
                        from_user_idCell.Style.BackColor = Color.LightGray;
                        pochta_idCell.Value ="";
                        textCell.Value = "";
                        on_readCell.Value = 1;

                        row.Cells.AddRange(idCell, from_user_idCell, send_dateCell, textCell, pochta_idCell, on_readCell);
                        this.dataGridViewDel.Rows.Add(row);
                    }

                    while (rdr.Read())
                    {
                        idCell = new DataGridViewTextBoxCell();
                        send_dateCell = new DataGridViewTextBoxCell();
                        from_user_idCell = new DataGridViewTextBoxCell();
                        textCell = new DataGridViewTextBoxCell();
                        pochta_idCell = new DataGridViewTextBoxCell();
                        on_readCell = new DataGridViewTextBoxCell();
                        row = new DataGridViewRow();

                        idCell.Value = rdr["id"].ToString();
                        send_dateCell.Value = rdr["release_id"]; 
                        from_user_idCell.Value = rdr["title"].ToString();
                        pochta_idCell.Value = "";
                        textCell.Value = "<h2> Ошибка в поекте </h2><br>Название: " + rdr["title"].ToString()
                            + "<br>Номер проекта: " + send_dateCell.Value.ToString()
                            + "<br> Обратись к администратору ID проекта: " + idCell.Value.ToString();

                        on_readCell.Value = 1;

                        row.Cells.AddRange(idCell, from_user_idCell, send_dateCell, textCell, pochta_idCell, on_readCell);
                        this.dataGridViewDel.Rows.Add(row);
                    }
                    mySqlConnectionSQLZepp.Close();
                }
                catch
                {
                    idCell = new DataGridViewTextBoxCell();
                    send_dateCell = new DataGridViewTextBoxCell();
                    from_user_idCell = new DataGridViewTextBoxCell();
                    textCell = new DataGridViewTextBoxCell();
                    pochta_idCell = new DataGridViewTextBoxCell();
                    on_readCell = new DataGridViewTextBoxCell();
                    row = new DataGridViewRow();
                    
                    idCell.Value = "0";
                    send_dateCell.Value = "";
                    from_user_idCell.Value = "";
                    textCell.Value = "";
                    pochta_idCell.Value = -1;
                    on_readCell.Value = -1;
                    row.Cells.AddRange(idCell, from_user_idCell,
                        send_dateCell, textCell, pochta_idCell, on_readCell);
                    this.dataGridViewDel.Rows.Add(row);
                    if (mySqlConnectionSQLZepp.State == System.Data.ConnectionState.Open)
                        mySqlConnectionSQLZepp.Close();
                }
            //}
        }

        //Иницилизация грида 
        public void init_DelGrid()
        {
            /* name ,
             * text ,           textColumn
             * from_user_id ,   from_user_idColumn
             * send_date ,      send_dateColumn
             * project_id ,     idColumn
             * pochta_id ,      pochta_idColumn
             * on_read "        on_readColumn
             */
            idColumn = new DataGridViewTextBoxColumn();
            send_dateColumn = new DataGridViewTextBoxColumn();
            from_user_idColumn = new DataGridViewTextBoxColumn();
            textColumn = new DataGridViewTextBoxColumn();
            pochta_idColumn = new DataGridViewTextBoxColumn();
            on_readColumn = new DataGridViewTextBoxColumn();

            //создание столбцов  
            idColumn.HeaderText = "id";
            idColumn.Name = "id";
            idColumn.Visible = false;

            send_dateColumn.Name = "send_date";
            send_dateColumn.HeaderText = "Дата";
            send_dateColumn.ValueType = typeof(DateTimeConverter);

            from_user_idColumn.Name = "name";
            from_user_idColumn.HeaderText = "Решение принял";

            textColumn.Name = "text";
            textColumn.HeaderText = "Поснения";
            textColumn.Visible = false;

            pochta_idColumn.Name = "pochta_id";
            pochta_idColumn.HeaderText = "pochta_idColumn";
            pochta_idColumn.Visible = false;

            on_readColumn.Name = "on_read";
            on_readColumn.HeaderText = "on_readColumn";
            on_readColumn.Visible = false;
            //создание столбцов  

            //добавление столбцов
            //row.Cells.AddRange(idCell, release_idCell, titleCell, release_dateCell, deployment_toCell, from_user_idCell, send_dateCell, textCell);
            this.dataGridViewDel.Columns.Add(idColumn);           
            this.dataGridViewDel.Columns.Add(from_user_idColumn);
            this.dataGridViewDel.Columns.Add(send_dateColumn);
            this.dataGridViewDel.Columns.Add(textColumn);
            this.dataGridViewDel.Columns.Add(pochta_idColumn);
            this.dataGridViewDel.Columns.Add(on_readColumn);
            //добавление столбцов              

        }

        // SQL команда для вкладки 
        public void SQL_Del()
        {  
            date = DateTime.Now;
            string[] SQL = new string[2];
            SQL = filter();
            string SQL_Del =
            "SELECT name , text , from_user_id , send_date , project_id ,  pochta_id , on_read "
                              + "FROM jos_zepp_pochta , jos_users"
                              + " WHERE ( to_user_id = " + Program.user.user_id + " ) "
                              + " AND (jos_zepp_pochta.tema = 2) AND (jos_users.id = from_user_id)"
                                + SQL[0] + SQL[1];
           // string SQL_Err =
           //" SELECT  name , text , from_user_id , send_date , project_id ,  pochta_id , on_read " 
           // + " FROM jos_zepp_pochta , jos_users , jos_projectlog_projects "
           // + " WHERE ( to_user_id = " +   Program.user.user_id + " ) "
           // + " AND (jos_users.id = from_user_id) "
           // + " AND NOT(jos_projectlog_projects.id = project_id) "
           // + " GROUP BY `pochta_id` "
           // + " ORDER BY jos_zepp_pochta.project_id , `send_date` "
           //                    + SQL[0] + SQL[1];

            string SQL_Err2 = "SELECT id , release_id , title FROM jos_projectlog_projects WHERE (category = 0 ) AND ( manager = " + Program.user.user_id+ " ) ;";

            cmd_Del = new MySql.Data.MySqlClient.MySqlCommand(SQL_Del, mySqlConnectionSQLZepp);
            //cmd_Err = new MySql.Data.MySqlClient.MySqlCommand(SQL_Err, mySqlConnectionSQLZepp);
            cmd_Err2 = new MySql.Data.MySqlClient.MySqlCommand(SQL_Err2, mySqlConnectionSQLZepp);

        }

        private void dataGridViewDel_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&
               ((int)this.dataGridViewDel.Rows[e.RowIndex].Cells["on_read"].Value == 0)
               && Program.pingServer(mySqlConnectionSQLZepp)
               )
            {
               // MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
                //mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
                //mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;


                //Изменить флаг прочитано в таблице почты
                string strSQL = "UPDATE  `jos_zepp_pochta` SET  `on_read` =  '1', `read_date` = NOW( ) WHERE  `jos_zepp_pochta`.`pochta_id` ="
                    + this.dataGridViewDel.Rows[e.RowIndex].Cells["pochta_id"].Value.ToString()
                    + " LIMIT 1 ;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);
                while (true)
                {
                    int intRecordsAffected = 0;
                    try
                    {
                        mySqlConnectionSQLZepp.Open();
                        intRecordsAffected = cmd.ExecuteNonQuery();
                        mySqlConnectionSQLZepp.Close();
                    }
                    catch { 
                    }
                    if (intRecordsAffected > 0)
                    {
                        // Обновить браузер программы
                        this.dataGridViewDel.Rows[e.RowIndex].Cells["on_read"].Value = 1;
                        this.dataGridViewDel.Rows[e.RowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Regular);
                        this.webBrowserDel.DocumentText = "";
                        if (e.RowIndex >= 0)
                            this.webBrowserDel.DocumentText = this.dataGridViewDel.Rows[e.RowIndex].Cells["text"].Value.ToString();

                        break;
                        //Console.WriteLineC"Update succeeded");
                    }
                    Thread.Sleep(1000);
                }
            }
            this.webBrowserDel.DocumentText = "";
            if (e.RowIndex >= 0)
                this.webBrowserDel.DocumentText = this.dataGridViewDel.Rows[e.RowIndex].Cells["text"].Value.ToString();
            this.Height = this.Height - 1; this.Height = this.Height + 1;
        }

        private void webBrowserDel_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.OriginalString != "about:blank")
            {
                e.Cancel = true;
                if (сброситьБраузерToolStripMenuItem.Checked)
                    System.Diagnostics.Process.Start(e.Url.OriginalString);
                else
                    System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, e.Url.OriginalString);
            }

        }

//>> Когда событие из трея обновило поля на Информационной вкладке:
        private void labelPochtaText_TextChanged(object sender, EventArgs e)
        {
            if (this.tabPageDel.Focused && !this.buttonNotRead.Enabled)
            {
                fillingDel();                
            }
            this.Height = this.Height - 1; this.Height = this.Height + 1;
        }
//<< Когда событие из трея обновило поля на Информационной вкладке:   
        private void dataGridViewDel_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            object test = e;
            if(сброситьБраузерToolStripMenuItem.Checked)
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.http_zepp + "index.php?option=com_projectlog&view=project&id=" + this.dataGridViewDel.Rows[e.RowIndex].Cells[0].Value);
            else
                System.Diagnostics.Process.Start(zeppMailer.Properties.Settings.Default.DirBrauser, zeppMailer.Properties.Settings.Default.http_zepp + "index.php?option=com_projectlog&view=project&id=" + this.dataGridViewDel.Rows[e.RowIndex].Cells[0].Value);

        }

#endregion

        private void buttonDellSelect_Click(object sender, EventArgs e)
        {
            SplitContainer Split = new SplitContainer();
            DataGridView Grid = new DataGridView();
            WebBrowser Web = new WebBrowser();
            //MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
            //mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
            //mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;

            string strSQL = " DELETE FROM jos_zepp_pochta WHERE pochta_id = ";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);

            Split = (SplitContainer)this.tabControl.SelectedTab.Controls[0];
            Grid = (DataGridView)Split.Panel1.Controls[0];
            Web = (WebBrowser)Split.Panel2.Controls[0];
            int intRecordsAffected = 0;

            foreach (DataGridViewRow row in Grid.SelectedRows)
            {
                cmd.CommandText = strSQL + row.Cells["pochta_id"].Value;

                try
                {
                    mySqlConnectionSQLZepp.Open();
                    intRecordsAffected += cmd.ExecuteNonQuery();
                    mySqlConnectionSQLZepp.Close();
                    row.Visible = false;
                }
                catch { }

            }

            if (intRecordsAffected != Grid.SelectedRows.Count)
            {
            }
            
            // Обновить браузер программы
            Web.DocumentText = "";
            //if (Grid.Name == "dataGridViewMove") fillingMov();
            //if (Grid.Name == "dataGridViewComment") fillingComment();
            //if (Grid.Name == "dataGridViewDel") fillingDel();

        }

        private void buttonDellYear_Click(object sender, EventArgs e)
        {
            SplitContainer Split = new SplitContainer();
            DataGridView Grid = new DataGridView();
            //MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
            //mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
            //mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;

            string strSQL = " DELETE FROM jos_zepp_pochta WHERE send_date < '";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);

            Split = (SplitContainer)this.tabControl.SelectedTab.Controls[0];
            Grid = (DataGridView)Split.Panel1.Controls[0];
            int intRecordsAffected = 0;
            DateTime date = DateTime.Now;
            
            cmd.CommandText = strSQL + date.AddYears(-1).ToString("yyyy-MM-dd") + "%'  ";
                try
                {
                    mySqlConnectionSQLZepp.Open();
                    intRecordsAffected += cmd.ExecuteNonQuery();
                    mySqlConnectionSQLZepp.Close();
                }
                catch { }

            

            if (intRecordsAffected == 0 )
            {
            }

            // Обновить браузер программы
            //this.webBrowserDel.DocumentText = "";
            if (Grid.Name == "dataGridViewMove") fillingMov();
            if (Grid.Name == "dataGridViewComment") fillingComment();
            if (Grid.Name == "dataGridViewDel") fillingDel();

        }

        private void buttonDellMoon_Click(object sender, EventArgs e)
        {
            SplitContainer Split = new SplitContainer();
            DataGridView Grid = new DataGridView();
            //MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
            //mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
            //mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;

            string strSQL = " DELETE FROM jos_zepp_pochta WHERE send_date < '";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);

            Split = (SplitContainer)this.tabControl.SelectedTab.Controls[0];
            Grid = (DataGridView)Split.Panel1.Controls[0];
            int intRecordsAffected = 0;
            DateTime date = DateTime.Now;

            cmd.CommandText = strSQL + date.AddMonths(-6).ToString("yyyy-MM-dd") + "%'  ";
            try
            {
                mySqlConnectionSQLZepp.Open();
                intRecordsAffected += cmd.ExecuteNonQuery();
                mySqlConnectionSQLZepp.Close();
            }
            catch { }



            if (intRecordsAffected == 0)
            {
            }

            // Обновить браузер программы
            //this.webBrowserDel.DocumentText = "";
            if (Grid.Name == "dataGridViewMove") fillingMov();
            if (Grid.Name == "dataGridViewComment") fillingComment();
            if (Grid.Name == "dataGridViewDel") fillingDel();
        }

        private void buttonDellWeekly_Click(object sender, EventArgs e)
        {
            SplitContainer Split = new SplitContainer();
            DataGridView Grid = new DataGridView();
            //MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp;
            //mySqlConnectionSQLZepp = new MySql.Data.MySqlClient.MySqlConnection();
            //mySqlConnectionSQLZepp.ConnectionString = global::zeppMailer.Properties.Settings.Default.ConectToZepp;

            string strSQL = " DELETE FROM jos_zepp_pochta WHERE send_date < '";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);

            Split = (SplitContainer)this.tabControl.SelectedTab.Controls[0];
            Grid = (DataGridView)Split.Panel1.Controls[0];
            int intRecordsAffected = 0;
            DateTime date = DateTime.Now;

            cmd.CommandText = strSQL + date.AddDays(-8).ToString("yyyy-MM-dd") + "%'  ";
            try
            {
                mySqlConnectionSQLZepp.Open();
                intRecordsAffected += cmd.ExecuteNonQuery();
                mySqlConnectionSQLZepp.Close();
            }
            catch { }



            if (intRecordsAffected == 0)
            {
            }

            // Обновить браузер программы
            //this.webBrowserDel.DocumentText = "";
            if (Grid.Name == "dataGridViewMove") fillingMov();
            if (Grid.Name == "dataGridViewComment") fillingComment();
            if (Grid.Name == "dataGridViewDel") fillingDel();
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics grap = e.Graphics;
            Font[] font = new Font[5];
            string[] text = new String[5] { tabPageInfo.Text, tabPageAlarm.Text, tabPageMove.Text, tabPageComment.Text, tabPageDel.Text };

            SolidBrush brush = new SolidBrush(Color.Black);
                   

            for (int i = 0; i < 5; i++)
            {
                //text[i] = tabControl.Controls[i].Text;
                font[i] = new Font("Verdana", 8, FontStyle.Regular);
            }
            

            if (int.Parse(labelProjIn.Text) > 0
                || int.Parse(labelBrakTxt.Text) > 0
                || int.Parse(labelCherepText.Text) > 0
                || int.Parse(labelProjOn.Text) > 0)
            {
                font[1] = new Font("Verdana", 8, FontStyle.Bold);                
            }

            if (int.Parse(labelMoveText.Text) > 0)
            {
                font[2] = new Font("Verdana", 8, FontStyle.Bold);
                text[2] = "Перемещения (" + labelMoveText.Text + ")";
            }

            if (int.Parse(labelCommentText.Text) > 0)
            {
                font[3] = new Font("Verdana", 8, FontStyle.Bold);
                text[3] = "Сообщения (" + labelCommentText.Text + ")";
            }


            if (int.Parse(labelPochtaText.Text) > 0)
            {
                font[4] = new Font("Verdana", 8, FontStyle.Bold);
                text[4] = "Удалены (" + labelPochtaText.Text + ")";
            }

           // Pen pencil = new Pen(Color.Blue);
          // Font font = new Font("Verdana", 8, FontStyle.Regular);
            
           
            for (int i = 0; i < tabControl.TabCount; i++)
            {
                Rectangle tabRect = tabControl.GetTabRect(i);
                tabRect.X += 4; tabRect.Y += 2; tabRect.Width -= 4; tabRect.Height -= 2;
                //grap.DrawRectangle(pencil, tabControl1.GetTabRect(0)); // в GetTabRect указываем вкладку
                grap.DrawString(text[i], font[i], brush, tabRect);  // а здесь 2-ая вкладка
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            //this.Hide();

        }

        private void получатьСообщенияНаПочтуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.user.user_id != 114)
            {
                string strSQL = "";
                //MySql.Data.MySqlClient.MySqlConnection mySqlConnectionSQLZepp = zeppMailer.Program.mySqlConnectionSQLZepp;
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(strSQL, mySqlConnectionSQLZepp);

                System.Windows.Forms.ToolStripMenuItem send = (ToolStripMenuItem)sender;
                if (send.Checked)
                {
                    strSQL = "UPDATE jos_users SET pochta_chek = 0 " +
                               " WHERE id = " + Program.user.user_id + " ;";
                    try
                    {
                        cmd.CommandText = strSQL;
                        mySqlConnectionSQLZepp.Open();
                        int intRecordsAffected = cmd.ExecuteNonQuery();
                        mySqlConnectionSQLZepp.Close();
                        if (intRecordsAffected < 1)
                        {
                            //ERR 103
                            MessageBox.Show("Ошибка при обновлении записи! (  FormМаин 103)\n");
                            return;
                        }
                        send.Checked = false;
                        send.CheckState = CheckState.Unchecked;

                    }
                    catch
                    {
                        //ERR 104
                        MessageBox.Show("Ошибка при обновлении записи! ( FormМаин 104)\n");
                        mySqlConnectionSQLZepp.Close();
                        return;
                    }


                } else
                {
                    strSQL = "UPDATE jos_users SET pochta_chek = 1 " +
                               " WHERE id = " + Program.user.user_id + " ;";
                    try
                    {
                        cmd.CommandText = strSQL;
                        mySqlConnectionSQLZepp.Open();
                        int intRecordsAffected = cmd.ExecuteNonQuery();
                        mySqlConnectionSQLZepp.Close();
                        if (intRecordsAffected < 1)
                        {
                            //ERR 105
                            MessageBox.Show("Ошибка при обновлении записи! (  FormМаин 105)\n");
                            return;
                        }
                        send.Checked = true;
                        send.CheckState = CheckState.Checked;
                    }
                    catch
                    {
                        //ERR 106
                        MessageBox.Show("Ошибка при обновлении записи! ( FormМаин 106)\n");
                        mySqlConnectionSQLZepp.Close();
                        return;
                    }


                }
            }
        }               

        private void строкаПодключенияКБазеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStrConect formStrConect = new FormStrConect();
            formStrConect.Show();        

        }

        private void CherepGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView send = (DataGridView)sender;

            send.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void CherepGrid_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

             DataGridView send = (DataGridView)sender;

            send.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void выбратьБраузерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dig = new OpenFileDialog(); 
            dig.Multiselect = false;
            dig.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            dig.Filter = "(*.exe)|*.exe"; 

            if (dig.ShowDialog() == DialogResult.OK)
            {
                //foreach (string s in dig.FileNames) 
                // Отображение имен файлов в списке. 
                global::zeppMailer.Properties.Settings.Default.NameBrauser = dig.SafeFileName.ToString();
                global::zeppMailer.Properties.Settings.Default.DirBrauser = dig.FileName;
                global::zeppMailer.Properties.Settings.Default.Save();
                сброситьБраузерToolStripMenuItem.CheckState = CheckState.Unchecked;
                сброситьБраузерToolStripMenuItem.Checked = false;
                выбратьБраузерToolStripMenuItem.ToolTipText = "Установлен " + global::zeppMailer.Properties.Settings.Default.NameBrauser;
            }
        }

        private void сброситьБраузерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            global::zeppMailer.Properties.Settings.Default.NameBrauser =
                global::zeppMailer.Properties.Settings.Default.DirBrauser = "";
            global::zeppMailer.Properties.Settings.Default.Save();
            выбратьБраузерToolStripMenuItem.ToolTipText = "Установлен по умолчанию";
            сброситьБраузерToolStripMenuItem.CheckState = CheckState.Checked;
            сброситьБраузерToolStripMenuItem.Checked = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void CherepGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void полноцветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormПолноцвет фПолноцвет = new FormПолноцвет();
            фПолноцвет.Show();
        }

        private void полноцветToolStripMenuItem1_Click(object sender, EventArgs e) {
            FormPolnocvet fPolnocvet = new FormPolnocvet();
            fPolnocvet.Show();
        }
    }




    // Разделяем потоки трея( chekMail ) и этой формы
    public static class ThreadSafeHelpers
    {
        public static void SetText(this Label varLabel, string newText)
        {
            if (varLabel.InvokeRequired)
            {
                varLabel.BeginInvoke(new MethodInvoker(() => SetText(varLabel, newText)));
            }
            else
            {
                varLabel.Text = newText;
            }
        }

        public static void SetImg(this PictureBox varBox, bool newVar)
        {
            if (varBox.InvokeRequired)
            {
                varBox.BeginInvoke(new MethodInvoker(() => SetImg(varBox, newVar)));
            }
            else
            {
                varBox.Visible = newVar;
            }
        }

       
    }
}
