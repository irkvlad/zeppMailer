using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibZepp;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace zeppMailer
{
    public partial class FormПолноцвет : Form
    {
        /// <summary>
        /// флаг для событий 
        /// </summary>
        bool onChenge = false;
        /// <summary>
        /// Данные авторизованного пользователя
        /// </summary>
        public User theUser;
        /// <summary>
        /// Список менеджеров
        /// </summary>
        public List<User> managers;
        /// <summary>
        /// Менеджер для файлов
        /// </summary>
        public User selectManager;
        /// <summary>
        /// Разобранные по подстрокам имена файлов
        /// </summary>
        public List<Polnocvet> files;
        public Polnocvet file;
        /// <summary>
        /// Делегат для фонового копирования файлов на сервер
        /// </summary>
        BackgroundWorker worker; // = sender as BackgroundWorker;
        /// <summary>
        /// для формирования паки на сервере (разделить файлы по датам)
        /// </summary>
        static string dataPatch;// = DateTime.Today.Year.ToString() + "\\" + DateTime.Today.Month.ToString() + "\\" + DateTime.Today.Day.ToString();

        //static string _dataPatch = DateTime.Now.ToString("MM") + DateTime.Now.ToString("MMMM").ToLower() + "\\" + DateTime.Now.ToString("dd");

        /// <summary>
        /// Папка установленная в настройках , куда нужно помещать файлы.
        /// </summary>
        DirectoryInfo destDir; // = new DirectoryInfo(global::zeppMailer.Properties.Settings.Default.PutchPolnocvet + "\\" + dataPatch);
        /// <summary>
        /// Ссылка для сайта
        /// </summary>
        string linkFile; // = global::zeppMailer.Properties.Settings.Default.linkPolnocvet + DateTime.Today.Year.ToString().Trim(' ') + "/" + DateTime.Today.Month.ToString().Trim(' ') + "/" + DateTime.Today.Day.ToString().Trim(' ');

        bool abort = false;
        public FormПолноцвет()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormПолнцвет_Load(object sender, EventArgs e)
        {
            theUser = new User(); 
            onChenge = false;
            managers = Helper.GetManagers();
            this.files = new List<Polnocvet>();
            dataPatch = DateTime.Today.Year.ToString() + "\\" + DateTime.Today.Month.ToString() + "\\" + DateTime.Today.Day.ToString();
            destDir = new DirectoryInfo(global::zeppMailer.Properties.Settings.Default.PutchPolnocvet + "\\" + dataPatch);
            linkFile = global::zeppMailer.Properties.Settings.Default.linkPolnocvet + DateTime.Today.Year.ToString().Trim(' ') + "/" + DateTime.Today.Month.ToString().Trim(' ') + "/" + DateTime.Today.Day.ToString().Trim(' ');


            if (theUser.comp == null)
            {
                this.Close(); return;
            }

            comboBoxManager.DataSource = managers;
            comboBoxManager.DisplayMember = "name";
            comboBoxManager.ValueMember = "project_user_id";            

            comboBoxManager.SelectedValue = theUser.project_user_id;
            comboBoxManager.Enabled = false;
            if (comboBoxManager.SelectedValue == null) comboBoxManager.BackColor = Color.Yellow;


            string[] stanok = Enum.GetNames(typeof(Polnocvet.Stanok));
            comboBoxStanok.DataSource = stanok;
            string[] material = Enum.GetNames(typeof(Polnocvet.Material));
            comboBoxMaterial.DataSource = material;
            string[] laminaciy = Enum.GetNames(typeof(Polnocvet.Тransparency));
            comboBoxLaminaciy.DataSource = laminaciy;
            string[] Lam2 = Enum.GetNames(typeof(Polnocvet.Тexture));
            comboBoxLam2.DataSource = Lam2;
            string[] charId = Helper.GetCharID(managers);
            comboBoxCharId.DataSource = charId;
            onChenge = true;

            switch (global::zeppMailer.Properties.Settings.Default.renemePolnocvet)
            {
                case 0:
                    radioButtonRenameNo.Checked = true;
                    break;
                case 1:
                    radioButtonRenameNo.Checked = true;
                    break;
                case 3:
                    radioButtonRenameEs.Checked = true;
                    break;
            } 

        }

        /// <summary>
        /// Удаляет файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDel_Click(object sender, EventArgs e)
        {
            int i = listBoxFile.SelectedIndex;
            //file = files[listBoxFile.SelectedIndex];
            if (listBoxFile.SelectedIndex > -1)
            {
                files.RemoveAt(listBoxFile.SelectedIndex);
                listBoxFile.Items.RemoveAt(listBoxFile.SelectedIndex);

                file = null;
                if (files.Count-1 >= i) { listBoxFile.SelectedIndex = i; file = files[i]; }
                else if (files.Count == 0) { file = new Polnocvet(); }
                else { listBoxFile.SelectedIndex = files.Count - 1; file = files[files.Count - 1]; }
                listBoxFile_SelectedIndexChanged(new object(), new EventArgs());
            }
        }

        /// <summary>
        /// В выборе файлов нажали кнопку дел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete) buttonDel_Click(new object(), new EventArgs());
        }

        /// <summary>
        /// Выбор файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFile_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            onChenge = false;
            // получаем выбранный файл

            string[] filename = openFileDialog1.FileNames;
            StringBuilder str = new StringBuilder();
            foreach (string s in filename)
            {
                listBoxFile.Items.Add(s);
                this.file = new Polnocvet();
                this.file.ParsingName(s);

                if (comboBoxManager.SelectedIndex >= 0)
                {
                    User manager = comboBoxManager.SelectedItem as User;
                    file.manager_id = manager.id;
                    //if (file.charId == "Б/К") file.charId = theUser.project_user_id;
                    file.charId = manager.project_user_id;
                    file.company = manager.company;
                }
                this.files.Add(file);
                comboBoxStanok.Enabled = true;
                comboBoxManager.Enabled = true;
                textBoxFileName.Enabled = true;
            }
                
            if (listBoxFile.SelectedIndex == -1) listBoxFile.SelectedIndex = 0;
            else file = files[listBoxFile.SelectedIndex];

            onChenge = true;
            comboBoxMaterial_SelectedIndexChanged(new object(), new EventArgs());
            textBoxFileName.Text = file.ToString();

        }

        /// <summary>
        /// Щелкнули по файлу в окне выбора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            onChenge = false;
            if(listBoxFile.SelectedIndex > -1 ) file = files[listBoxFile.SelectedIndex];
            EnabledBoxes();
            InitialValueBoxes();
            onChenge = true;
        }

        /// <summary>
        /// Присвоение полям формы значений из полей текущего file
        /// </summary>
        private void InitialValueBoxes()
        {
            comboBoxStanok.SelectedItem = file.stanok.ToString();
            comboBoxMaterial.SelectedItem = file.мaterial.ToString();
            if (file.мaterial != Polnocvet.Material.Нет) { onChenge = true; comboBoxMaterial_SelectedIndexChanged(new object(), new EventArgs()); onChenge = false; }
            if (!file.onOff[5])
            {
                comboBoxMaterial.BackColor = Color.Yellow;
                labelMaterial.Text = "Укажите материал";
            }
            else
            {
                comboBoxMaterial.BackColor = Color.FromName("Window");
                labelMaterial.Text = "Материал";
            }
            textBoxPadding.Text = file.padding.ToString();
            textBoxStick.Text = file.stick.ToString();
            textBoxCringle.Text = file.cringle.ToString();
            if (file.sizeM == 0)
            {
                textBoxSiseM.ForeColor = Color.Red;
                labelSiseM.Text = "Должно быть не ноль";
            }else
            {
                textBoxSiseM.ForeColor = Color.Black;
                labelSiseM.Text = "Большая сторона";                
            }
            textBoxSiseM.Text = file.sizeM.ToString();
            if (file.sizeL == 0)
            {
                textBoxSiseL.ForeColor = Color.Red;
                labelSiseL.Text = "Должно быть не ноль";
            }
            else
            {
                textBoxSiseL.ForeColor = Color.Black;
                labelSiseL.Text = "Меньшая сторона";
            }
            textBoxSiseL.Text = file.sizeL.ToString();
            textBoxCopyes.Text = file.copyes.ToString();
            comboBoxLaminaciy.SelectedItem = file.laminaciy.transparency.ToString();
            comboBoxLam2.SelectedItem = file.laminaciy.texture.ToString();
            if (file.charId == "Б/К" && comboBoxManager.SelectedValue != null ) file.charId = comboBoxManager.SelectedValue.ToString();
            comboBoxCharId.SelectedItem = file.charId.ToString();
            if(file.number == "")
            {
                textBoxNumber.BackColor = Color.Yellow;
                labelNumber.Text = "Укажите номер";
            }
            else
            {
                textBoxNumber.BackColor = Color.FromName("Window");
                labelNumber.Text = "Номер заказа";
            }
            textBoxNumber.Text = file.number.ToString();
            textBoxComent.Text = file.coment.ToString();
            checkBoxCringleOnM.Checked = false;
            checkBoxCringleOnL.Checked = false;
            checkBoxCringleOnM2.Checked = false;
            checkBoxCringleOnL2.Checked = false;
            checkBoxCringleON4.Checked = (file.cringle > 0) ? true : false;
            if (file.stick != 0)
            {
                checkStikM.Checked = (file.stick == file.sizeM) ? true : false;
                checkStickL.Checked = (file.stick == file.sizeL) ? true : false;
                checkBoxStick4.Checked = (file.stick == (file.sizeM + file.sizeL) * 2) ? true : false;
            }

            textBoxFileName.Text = file.ToString();
            if(files.Count == 0)
            {
                buttonLinck.Visible = true;
                buttonLinck.Enabled = true;
                button1.Enabled = false;
                button1.Visible = false;
            }
            else
            {
                buttonLinck.Visible = false;
                buttonLinck.Enabled = false;
                button1.Enabled = true;
                button1.Visible = true;
            }
        }

        /// <summary>
        /// открытие полей формы взависимости от станка текущего file
        /// </summary>
        /// <param name="st">текущий станок</param>
        private void EnabledBoxes()
        {

            switch (file.stanok)
            {
                case Polnocvet.Stanok.Нет:
                    //comboBoxManager.Enabled=
                    //comboBoxStanok.Enabled =
                    comboBoxMaterial.Enabled =
                            comboBoxBasis.Enabled=
                            comboBoxТexture.Enabled=
                        textBoxPadding.Enabled =
                        textBoxStick.Enabled =
                        textBoxCringle.Enabled =
                        textBoxSiseM.Enabled =
                        textBoxSiseL.Enabled =
                        textBoxCopyes.Enabled =
                        comboBoxLaminaciy.Enabled =
                            comboBoxLam2.Enabled =
                        comboBoxCharId.Enabled =
                        textBoxNumber.Enabled =
                        textBoxComent.Enabled =
                        checkBoxCringleOnM.Enabled =
                        checkBoxCringleOnL.Enabled =
                        checkBoxCringleOnM2.Enabled =
                        checkBoxCringleOnL2.Enabled =
                        checkBoxCringleON4.Enabled =
                        checkStikM.Enabled =
                        checkStickL.Enabled =
                        checkBoxStick4.Enabled = false;
                    break;

                case Polnocvet.Stanok.Ламинация:
                    comboBoxТexture.Enabled =
                        comboBoxТexture.Enabled =
                        textBoxSiseM.Enabled =
                        textBoxSiseL.Enabled =
                        textBoxCopyes.Enabled =
                        comboBoxCharId.Enabled =
                        textBoxNumber.Enabled =
                        textBoxComent.Enabled = true;


                    comboBoxMaterial.Enabled =
                       comboBoxBasis.Enabled =                         
                       textBoxPadding.Enabled =
                       textBoxStick.Enabled =
                       textBoxCringle.Enabled =
                       comboBoxLaminaciy.Enabled =
                       comboBoxLam2.Enabled =
                       checkBoxCringleOnM.Enabled =
                       checkBoxCringleOnL.Enabled =
                       checkBoxCringleOnM2.Enabled =
                       checkBoxCringleOnL2.Enabled =
                       checkBoxCringleON4.Enabled =
                       checkStikM.Enabled =
                       checkStickL.Enabled =
                       checkBoxStick4.Enabled = false;

                    string[] texture = Enum.GetNames(typeof(Polnocvet.Тexture));
                    comboBoxТexture.DataSource = texture;
                    file.мaterial = Polnocvet.Material.Нет;
                    break;

                case Polnocvet.Stanok.Роланд:
                case Polnocvet.Stanok.Феникс:
                    comboBoxMaterial.Enabled =
                         comboBoxBasis.Enabled =
                         comboBoxТexture.Enabled =
                        textBoxPadding.Enabled = 
                        textBoxStick.Enabled = 
                        textBoxCringle.Enabled = 
                        textBoxSiseM.Enabled = 
                        textBoxSiseL.Enabled = 
                        textBoxCopyes.Enabled = 
                        comboBoxLaminaciy.Enabled = 
                        comboBoxLam2.Enabled = 
                        comboBoxCharId.Enabled = 
                        textBoxNumber.Enabled = 
                        textBoxComent.Enabled = 
                        checkBoxCringleOnM.Enabled = 
                        checkBoxCringleOnL.Enabled = 
                        checkBoxCringleOnM2.Enabled = 
                        checkBoxCringleOnL2.Enabled = 
                        checkBoxCringleON4.Enabled = 
                        checkStikM.Enabled = 
                        checkStickL.Enabled = 
                        checkBoxStick4.Enabled = true;
                    break;

                case Polnocvet.Stanok.УФ:
                    comboBoxMaterial.Enabled =
                        comboBoxBasis.Enabled =
                        comboBoxТexture.Enabled =
                    textBoxSiseM.Enabled =
                    textBoxSiseL.Enabled =
                    textBoxCopyes.Enabled =
                    comboBoxCharId.Enabled =
                    textBoxNumber.Enabled =
                    textBoxComent.Enabled = true;
                                                                    
                    textBoxPadding.Enabled =
                    textBoxStick.Enabled =
                    textBoxCringle.Enabled =
                    comboBoxLaminaciy.Enabled =
                    comboBoxLam2.Enabled =
                    checkBoxCringleOnM.Enabled =
                    checkBoxCringleOnL.Enabled =
                    checkBoxCringleOnM2.Enabled =
                    checkBoxCringleOnL2.Enabled =
                    checkBoxCringleON4.Enabled =
                    checkStikM.Enabled =
                    checkStickL.Enabled =
                    checkBoxStick4.Enabled = false;

                    labelBasis.Text = "Белый цвет";
                    string[] transparency2 = Enum.GetNames(typeof(Polnocvet.Тransparency));
                    comboBoxBasis.DataSource = transparency2;
                    comboBoxBasis.Enabled = true;
                    comboBoxBasis.SelectedItem = file.transparency.ToString();
                    string[] texture2 = Enum.GetNames(typeof(Polnocvet.Тexture));
                    comboBoxТexture.DataSource = texture2;
                    comboBoxТexture.Enabled = true;
                    comboBoxТexture.SelectedItem = file.texture.ToString();                    
                    break;
            }
        }

        #region Поля
        /// <summary>
        /// Изминение зависимых полей при смене менеджера 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxManager_SelectedIndexChanged(object sender, EventArgs e)
        {           
            if (onChenge) comboBoxManager_Validating(sender, new CancelEventArgs());
        }
        private void comboBoxManager_Validating(object sender, CancelEventArgs e)
        {
            if (onChenge)
            {
                if (comboBoxManager.SelectedValue != null)
                {
                    comboBoxCharId.SelectedItem = comboBoxManager.SelectedValue;
                    comboBoxManager.BackColor = Color.FromName("Window");
                    foreach(Polnocvet f in files)
                    if (f != null)
                    {
                        f.charId = comboBoxManager.SelectedValue.ToString();                        
                        User manager = comboBoxManager.SelectedItem as User;
                        f.manager_id = manager.id;
                        f.company = manager.company;
                    }
                    textBoxFileName.Text = file.ToString();
                }
            }
        }

        /// <summary>
        /// Изминение блокировок полей при смене станка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxStanok_SelectedIndexChanged(object sender, EventArgs e)
        {
            var result = (Polnocvet.Stanok)Polnocvet.Stanok.Parse(typeof(Polnocvet.Stanok), comboBoxStanok.SelectedItem.ToString());
            if (comboBoxStanok.SelectedItem.ToString() != "Нет")
            {
                comboBoxStanok.ForeColor = Color.Black;
                labelStanok.Text = "Станок";
                if (onChenge)
                {

                    file.stanok = result;
                    textBoxFileName.Text = file.ToString();
                    
                }

                EnabledBoxes();
            }
            else
            {
                file.stanok = result;
                comboBoxStanok.ForeColor = Color.Red;
                labelStanok.Text = "Укажите станок";
                EnabledBoxes();
            }
        }

        /// <summary>
        /// Изминили размер полей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPadding_Validating(object sender, CancelEventArgs e)
        {
            int p;
            if (int.TryParse(textBoxPadding.Text.ToString(), out p))
            {
                int os = p % 10;
                if (os != 0)
                {
                    textBoxPadding.ForeColor = Color.Red;
                    labelPadding.Text = "Должно быть кратно 10";
                    file.padding = p;
                    textBoxFileName.Text = file.ToString();
                }
                else
                {

                    file.padding = p;
                    textBoxPadding.ForeColor = Color.Black;
                    labelPadding.Text = "Поля";
                    textBoxFileName.Text = file.ToString();
                }

            }
            else
            {
                textBoxPadding.ForeColor = Color.Red;
                labelPadding.Text = "Должно быть число";
            }
        }

        /// <summary>
        /// Изминили размеры склея
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxStick_Validating(object sender, CancelEventArgs e)
        {
            int p;
            if (int.TryParse(textBoxStick.Text.ToString(), out p))
            {
               // if (p != 0)
               // {
                    checkStikM.Checked = (p == file.sizeM) ? true : false;
                    checkStickL.Checked = (p == file.sizeL) ? true : false;
                    checkBoxStick4.Checked = (p == (file.sizeM + file.sizeL) * 2) ? true : false;
                    if (!checkStikM.Checked && !checkStickL.Checked && !checkBoxStick4.Checked && p != 0)
                    {
                        textBoxStick.ForeColor = Color.Red;
                        labelStick.Text = "Нет такого размера";
                        textBoxStick.Text = "XXX "+p.ToString()+" XXX";
                    }
                    else
                    {
                        file.stick = p;
                        textBoxStick.ForeColor = Color.Black;
                        labelStick.Text = "Склей";
                        textBoxFileName.Text = file.ToString();
                    }
              /*  } else
                {
                    file.stick = p;
                    textBoxStick.ForeColor = Color.Black;
                    labelStick.Text = "Склей";
                    textBoxFileName.Text = file.ToString();
                }*/
                

            }
            else
            {
                textBoxStick.ForeColor = Color.Red;
                labelStick.Text = "Должно быть число";
            }
        }

        /// <summary>
        /// Изменили количество люверсов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCringle_Validating(object sender, CancelEventArgs e)
        {
            int p;
            checkBoxCringleOnL.Checked = false;
            checkBoxCringleOnL2.Checked = false;
            checkBoxCringleOnM.Checked = false;
            checkBoxCringleOnM2.Checked = false;
            checkBoxCringleON4.Checked = false;

            if (int.TryParse(textBoxCringle.Text.ToString(), out p))
            {
                file.cringle = p;
                textBoxCringle.ForeColor = Color.Black;
                labelCringle.Text = "Люверсы";
                textBoxFileName.Text = file.ToString();
                textBoxCringle.Text = p.ToString();
               /* if (p != 0)
                {
                    if (file.sizeM > 600)
                        checkBoxCringleOnM.Checked = (p == (int)(file.sizeM / 300)) ? true : false;
                    if (file.sizeL > 600)
                        checkBoxCringleOnL.Checked = (p == (int)(file.sizeL / 300)) ? true : false;
                    if (file.sizeM > 600)
                        checkBoxCringleOnM2.Checked = (p == (int)(file.sizeM * 2 / 300)) ? true : false;
                    if (file.sizeL > 600)
                        checkBoxCringleOnL2.Checked = (p == (int)(file.sizeL * 2 / 300)) ? true : false;
                    if (file.sizeM > 600 && file.sizeM > 600)
                        checkBoxCringleON4.Checked = (p == (int)((file.sizeL + file.sizeM) * 2 / 300)) ? true : false;
                   

                        if (!checkBoxCringleOnM.Checked
                            && !checkBoxCringleOnL.Checked
                            && !checkBoxCringleOnM2.Checked
                            && !checkBoxCringleOnL2.Checked
                            && !checkBoxCringleON4.Checked
                            )
                        {
                            file.cringle = p;
                            labelCringle.Text = "Не кратно 300";
                            textBoxFileName.Text = file.ToString();
                        }

                  }
                else
                  {
                        file.cringle = p;
                        labelCringle.Text = "Люверсы";
                        textBoxFileName.Text = file.ToString();
                  }
                 */

            }
            else
            {
                textBoxCringle.ForeColor = Color.Red;
                labelCringle.Text = "Должно быть число";
                
            }
        }

        /// <summary>
        /// Изменили больший размер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSiseM_Validating(object sender, CancelEventArgs e)
        {
            int p;
            if (int.TryParse(textBoxSiseM.Text.ToString(), out p))
            {
                if (p != 0)
                {
                    if (checkBoxCringleOnM.Checked) textBoxCringle.Text = ((int)(p / 300)).ToString();
                    else if (checkBoxCringleOnM2.Checked) textBoxCringle.Text = ((int)(p / 300) * 2).ToString();
                    else if (checkBoxCringleON4.Checked) textBoxCringle.Text = ((int)((p + file.sizeL) / 300) * 2).ToString();

                    if (checkStikM.Checked) textBoxStick.Text = p.ToString();
                    if (checkBoxStick4.Checked) textBoxStick.Text = ((file.sizeL + p) * 2).ToString();
                    file.sizeM = int.Parse(textBoxSiseM.Text);
                    textBoxSiseM.ForeColor = Color.Black;
                    labelSiseM.Text = "Большая сторона";
                    textBoxFileName.Text = file.ToString();
                }
                else
                {
                    textBoxSiseM.ForeColor = Color.Red;
                    labelSiseM.Text = "Должно быть не ноль";
                }
            }
            else
            {
                textBoxSiseM.ForeColor = Color.Red;
                labelSiseM.Text = "Должно быть число";
            }
        }

        /// <summary>
        /// Изменили меньший размер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSiseL_Validating(object sender, CancelEventArgs e)
        {
            int p;
            if (int.TryParse(textBoxSiseL.Text.ToString(), out p))
            {
                if (p != 0)
                {
                    if (checkBoxCringleOnL.Checked) textBoxCringle.Text = ((int)(p / 300)).ToString();
                    else if (checkBoxCringleOnL2.Checked) textBoxCringle.Text = ((int)(p / 300) * 2).ToString();
                    else if (checkBoxCringleON4.Checked) textBoxCringle.Text = ((int)((p + file.sizeM) / 300) * 2).ToString();

                    if (checkStickL.Checked) textBoxStick.Text = p.ToString();
                    if (checkBoxStick4.Checked) textBoxStick.Text = ((file.sizeM + p) * 2).ToString();

                    file.sizeL = int.Parse(textBoxSiseL.Text);
                    textBoxSiseL.ForeColor = Color.Black;
                    labelSiseL.Text = "Меньшая сторона";
                    textBoxFileName.Text = file.ToString();
                }
                else
                {
                    textBoxSiseL.ForeColor = Color.Red;
                    labelSiseL.Text = "Должно быть не ноль";
                }

            }
            else
            {
                textBoxSiseL.ForeColor = Color.Red;
                labelSiseL.Text = "Должно быть число";
            }
        }

        /// <summary>
        /// Изменили количество копий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCopyes_Validating(object sender, CancelEventArgs e)
        {
            int p;
            if (int.TryParse(textBoxCopyes.Text.ToString(), out p))
            {
                if (p >= 0)
                {
                    file.copyes = p;
                    textBoxCopyes.ForeColor = Color.Black;
                    labelCopyes.Text = "Количество копий";
                    textBoxFileName.Text = file.ToString();
                }

            }
            else
            {
                textBoxCopyes.ForeColor = Color.Red;
                labelCopyes.Text = "Должно быть число";
            }
        }

        /// <summary>
        /// Изменили инициалы менеджеров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxCharId_Validating(object sender, CancelEventArgs e)
        {
            comboBoxManager.SelectedValue = comboBoxCharId.SelectedValue;
            if (file != null) file.charId = comboBoxManager.SelectedValue.ToString();

            User manager = comboBoxManager.SelectedItem as User;
            file.manager_id = manager.id;
            file.company = manager.company;

            if (!file.onOff[1]) comboBoxCharId.BackColor = Color.Yellow;
            else comboBoxCharId.BackColor = Color.FromName("Window");
        }
        private void comboBoxCharId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onChenge) comboBoxCharId_Validating(new object(), new CancelEventArgs());
        }

        /// <summary>
        /// Изменили номер заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNumber_Validating(object sender, CancelEventArgs e)
        {
            textBoxNumber.Text = textBoxNumber.Text.Trim(Path.GetInvalidFileNameChars());
            file.number = textBoxNumber.Text;
            textBoxFileName.Text = file.ToString();
            if (!file.onOff[0]) textBoxNumber.BackColor = Color.Yellow;
            else textBoxNumber.BackColor = Color.FromName("Window");
        }

        /// <summary>
        /// Изменили коментарий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxComent_Validating(object sender, CancelEventArgs e)
        {
            textBoxComent.Text = textBoxComent.Text.Trim(Path.GetInvalidFileNameChars());
            file.coment = textBoxComent.Text;
            textBoxFileName.Text = file.ToString();
        }

        /// <summary>
        /// Переключатель: люверсы по большей стороне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxCringleOnM_CheckedChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                onChenge = false;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked)
                {
                    checkBoxCringleON4.Checked = false;
                    checkBoxCringleOnL.Checked = false;
                    checkBoxCringleOnL2.Checked = false;
                    checkBoxCringleOnM2.Checked = false;
                    if (file.sizeM > 600)
                    {
                        textBoxCringle.Text = ((int)(file.sizeM / 300)).ToString();
                        file.cringle = ((int)(file.sizeM / 300));
                    }else
                    {
                        textBoxCringle.Text = "2";
                        file.cringle = 2;
                    }

                    textBoxCringle_Validating(new object(), new CancelEventArgs());
                }
                else
                {
                    textBoxCringle.Text = "0";
                    file.cringle = 0;
                }

                onChenge = true;
                textBoxFileName.Text = file.ToString();
            }
        }

        /// <summary>
        /// Переключатель: люверсы по меньшей стороне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxCringleOnL_CheckedChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                onChenge = false;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked)
                {
                    checkBoxCringleON4.Checked = false;
                    checkBoxCringleOnL2.Checked = false;
                    checkBoxCringleOnM.Checked = false;
                    checkBoxCringleOnM2.Checked = false;
                    if (file.sizeL > 600)
                    {
                        textBoxCringle.Text = ((int)(file.sizeL / 300)).ToString();
                        file.cringle = ((int)(file.sizeL / 300));
                    }
                    else
                    {
                        textBoxCringle.Text = "2";
                        file.cringle = 2;
                    }


                textBoxCringle_Validating(new object(), new CancelEventArgs());
                }
                else
                {
                    textBoxCringle.Text = "0";
                    file.cringle = 0;
                }
                onChenge = true;
                textBoxFileName.Text = file.ToString();
            }
        }

        /// <summary>
        /// Переключатель: люверсы по 2 большим сторонам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxCringleOnM2_CheckedChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                onChenge = false;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked)
                {
                    checkBoxCringleON4.Checked = false;
                    checkBoxCringleOnL.Checked = false;
                    checkBoxCringleOnL2.Checked = false;
                    checkBoxCringleOnM.Checked = false;

                    if (file.sizeM > 600)
                    {
                        textBoxCringle.Text = ((int)(2 * file.sizeM / 300)).ToString();
                        file.cringle = ((int)(2 * file.sizeM / 300));
                    }
                    else
                    {
                        textBoxCringle.Text = "4";
                        file.cringle = 4;
                    }

                    textBoxCringle_Validating(new object(), new CancelEventArgs());
                }
                else
                {
                    textBoxCringle.Text = "0";
                    file.cringle = 0;
                }
                onChenge = true;
                textBoxFileName.Text = file.ToString();
            }
        }

        /// <summary>
        /// Переключатель: люверсы по 2 меньшим сторонам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxCringleOnL2_CheckedChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked)
                {
                    onChenge = false;
                    checkBoxCringleON4.Checked = false;
                    checkBoxCringleOnL.Checked = false;
                    checkBoxCringleOnM.Checked = false;
                    checkBoxCringleOnM2.Checked = false;

                    if (file.sizeL > 600)
                    {
                        textBoxCringle.Text = ((int)(2 * file.sizeL / 300)).ToString();
                        file.cringle = ((int)(2 * file.sizeL / 300));
                    }
                    else
                    {
                        textBoxCringle.Text = "4";
                        file.cringle = 4;
                    }

                    textBoxCringle_Validating(new object(), new CancelEventArgs());
                }
                else
                {
                    textBoxCringle.Text = "0";
                    file.cringle = 0;
                }
                onChenge = true;
                textBoxFileName.Text = file.ToString();
            }
        }

        /// <summary>
        ///  Переключатель: люверсы по всем сторонам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxCringleON4_CheckedChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                onChenge = false;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked)
                {
                    int c = 0;
                    checkBoxCringleOnL.Checked = false;
                    checkBoxCringleOnL2.Checked = false;
                    checkBoxCringleOnM.Checked = false;
                    checkBoxCringleOnM2.Checked = false;


                    if (file.sizeL < 600) c = 4;
                    if (file.sizeM < 600) c += 4;                   


                    textBoxCringle.Text = (((int)(2 * (file.sizeL + file.sizeM) / 300)) + c).ToString();
                    file.cringle = (((int)(2 * (file.sizeL + file.sizeM) / 300)) + c);


                    textBoxCringle_Validating(new object(), new CancelEventArgs());
                }
                else
                {
                    textBoxCringle.Text = "0";
                    file.cringle = 0;
                }
                onChenge = true;
            }
        }

        /// <summary>
        /// Склей по большей стороне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkStikM_CheckedChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                onChenge = false;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked)
                {
                    checkStickL.Checked = false;
                    checkBoxStick4.Checked = false;

                    file.stick = file.sizeM;
                    textBoxStick.Text = file.sizeM.ToString();
                    textBoxStick_Validating(new object(), new CancelEventArgs());

                }
                else
                {
                    checkStickL.Checked = false;
                    checkBoxStick4.Checked = false;
                    file.stick = 0;
                    textBoxStick.Text = "0";
                    textBoxStick_Validating(new object(), new CancelEventArgs());
                }
                onChenge = true;
            }
        }

        /// <summary>
        /// Склей по меньшей стороне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkStickL_CheckedChanged(object sender, EventArgs e)
        {

            if (onChenge)
            {
                onChenge = false;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked)
                {
                    checkStikM.Checked = false;
                    checkBoxStick4.Checked = false;
                    file.stick = file.sizeL;

                    textBoxStick.Text = file.sizeL.ToString();
                    textBoxStick_Validating(new object(), new CancelEventArgs());
                }
                else
                {
                    checkStikM.Checked = false;
                    checkBoxStick4.Checked = false;
                    file.stick = 0;
                    textBoxStick.Text = "0";
                    textBoxStick_Validating(new object(), new CancelEventArgs());
                }
                onChenge = true;
            }
        }

        /// <summary>
        /// Склей по пириметру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxStick4_CheckedChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                onChenge = false;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked)
                {
                    checkStikM.Checked = false;
                    checkStickL.Checked = false;

                    file.stick = ((file.sizeL + file.sizeM) * 2);

                    textBoxStick.Text = ((file.sizeL + file.sizeM) * 2).ToString();
                    textBoxStick_Validating(new object(), new CancelEventArgs());
                }
                else
                {
                    checkStikM.Checked = false;
                    checkStickL.Checked = false;
                    file.stick = 0;
                    textBoxStick.Text = "0";
                    textBoxStick_Validating(new object(), new CancelEventArgs());
                }
                onChenge = true;
            }
        }       

        /// <summary>
        /// Выбрали материал
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                comboBoxMaterial.ForeColor = Color.Black;
                labelMaterial.Text = "Материал";
                Polnocvet.Material result = (Polnocvet.Material)Polnocvet.Material.Parse(typeof(Polnocvet.Material), comboBoxMaterial.SelectedItem.ToString());
                file.мaterial = result;
                //file.bensity = Polnocvet.Вensity.Нет;
                //file.transparency = Polnocvet.Тransparency.Нет;
                //file.texture = Polnocvet.Тexture.Нет;
                onChenge = false;
                if (file.stanok == Polnocvet.Stanok.УФ)
                {                    
                    labelBasis.Text = "Белый цвет";
                    string[] transparency2 = Enum.GetNames(typeof(Polnocvet.Тransparency));
                    comboBoxBasis.DataSource = transparency2;
                    comboBoxBasis.Enabled = true;
                    comboBoxBasis.SelectedItem = file.transparency.ToString();
                    string[] texture2 = Enum.GetNames(typeof(Polnocvet.Тexture));
                    comboBoxТexture.DataSource = texture2;
                    comboBoxТexture.Enabled = true;
                    comboBoxТexture.SelectedItem = file.texture.ToString();
                    onChenge = true;
                    comboBoxBasis_SelectedIndexChanged(new object(), new EventArgs());
                    onChenge = false;
                    if(result == Polnocvet.Material.Нет)
                    {
                        comboBoxMaterial.ForeColor = Color.Red;
                        labelMaterial.Text = "Укажите материал";
                    }
                }else
                switch (result)
                {
                    case Polnocvet.Material.Нет:
                        comboBoxMaterial.ForeColor = Color.Red;
                        labelMaterial.Text = "Укажите материал";
                        labelBasis.Text = "Основание";
                        comboBoxBasis.DataSource = null;
                        comboBoxBasis.Enabled = false;
                        comboBoxТexture.Enabled = false;
                        comboBoxТexture.DataSource = null;                       
                        break;
                    case Polnocvet.Material.Банер:                        
                        labelBasis.Text = "Плотность";
                        string[] bensity = Enum.GetNames(typeof(Polnocvet.Вensity));
                        comboBoxBasis.DataSource = bensity;
                        comboBoxBasis.Enabled = true;
                        comboBoxBasis.SelectedItem = file.bensity.ToString();
                        comboBoxТexture.Enabled = false;
                        comboBoxТexture.DataSource = null;
                        onChenge = true;
                            comboBoxBasis_SelectedIndexChanged(new object(), new EventArgs());
                        onChenge = false;
                        break;
                    case Polnocvet.Material.Пленка:
                        labelBasis.Text = "Прозрачность";
                        string[] transparency = Enum.GetNames(typeof(Polnocvet.Тransparency));
                        comboBoxBasis.DataSource = transparency;
                        comboBoxBasis.Enabled = true;
                        comboBoxBasis.SelectedItem = file.transparency.ToString();
                        string[] texture = Enum.GetNames(typeof(Polnocvet.Тexture));
                        comboBoxТexture.DataSource = texture;
                        comboBoxТexture.Enabled = true;
                        comboBoxТexture.SelectedItem = file.texture.ToString();
                        onChenge = true;
                        comboBoxBasis_SelectedIndexChanged(new object(), new EventArgs());                           
                        onChenge = false;
                        break;                   
                    default:
                        labelBasis.Text = "Основание";
                        comboBoxBasis.DataSource = null;
                        comboBoxBasis.Enabled = false;
                        comboBoxТexture.Enabled = false;
                        comboBoxТexture.DataSource = null;
                        break;
                }
                onChenge = true;
                textBoxFileName.Text = file.ToString();
            }
        }

        /// <summary>
        /// Выбрали основание материала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxBasis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                Polnocvet.Material result = (Polnocvet.Material)Polnocvet.Material.Parse(typeof(Polnocvet.Material), comboBoxMaterial.SelectedItem.ToString());
                if(file.stanok == Polnocvet.Stanok.УФ)
                {
                    file.bensity = Polnocvet.Вensity.Нет;
                    Polnocvet.Тexture tr = file.texture;
                    Polnocvet.Тransparency transparency = (Polnocvet.Тransparency)Polnocvet.Тransparency.Parse(typeof(Polnocvet.Тransparency), comboBoxBasis.SelectedItem.ToString());
                    file.transparency = transparency;
                    file.texture = Polnocvet.Тexture.Нет;
                    file.texture = tr;
                }
                else if (result == Polnocvet.Material.Банер)
                {
                    file.transparency = Polnocvet.Тransparency.Нет;
                    file.texture = Polnocvet.Тexture.Нет;
                    Polnocvet.Вensity bensity = (Polnocvet.Вensity)Polnocvet.Вensity.Parse(typeof(Polnocvet.Вensity), comboBoxBasis.SelectedItem.ToString());
                    file.bensity = bensity;
                }
                else if (result == Polnocvet.Material.Пленка)
                {
                    file.bensity = Polnocvet.Вensity.Нет;
                    Polnocvet.Тexture tr = file.texture;
                    Polnocvet.Тransparency transparency = (Polnocvet.Тransparency)Polnocvet.Тransparency.Parse(typeof(Polnocvet.Тransparency), comboBoxBasis.SelectedItem.ToString());
                    file.transparency = transparency;
                    file.texture = Polnocvet.Тexture.Нет;
                    file.texture = tr;
                }

                if( (file.onOff[6] && file.onOff[8]) ) comboBoxBasis.BackColor = Color.FromName("Window");
                else comboBoxBasis.BackColor = Color.Yellow;

                comboBoxТexture_SelectedIndexChanged(new object(), new EventArgs());
                textBoxFileName.Text = file.ToString();
            }
        }

        /// <summary>
        /// Выбрали текстуру материала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxТexture_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                Polnocvet.Material result = (Polnocvet.Material)Polnocvet.Material.Parse(typeof(Polnocvet.Material), comboBoxMaterial.SelectedItem.ToString());

                if (result == Polnocvet.Material.Пленка || file.stanok == Polnocvet.Stanok.Ламинация || file.stanok == Polnocvet.Stanok.УФ)
                {
                    Polnocvet.Тexture texture = (Polnocvet.Тexture)Polnocvet.Тexture.Parse(typeof(Polnocvet.Тexture), comboBoxТexture.SelectedItem.ToString());
                    file.texture = texture;
                }
                if (file.onOff[7]) comboBoxТexture.BackColor = Color.FromName("Window");
                else comboBoxТexture.BackColor = Color.Yellow;
                textBoxFileName.Text = file.ToString();
            }
        }

        /// <summary>
        /// Указали что нужна ламинация после печати и указали основу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxLaminaciy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {
                Polnocvet.Тransparency result = (Polnocvet.Тransparency)Polnocvet.Тransparency.Parse(typeof(Polnocvet.Тransparency), comboBoxLaminaciy.SelectedItem.ToString());

                if (result != Polnocvet.Тransparency.Нет) comboBoxLam2.Enabled = true;
                else comboBoxLam2.Enabled = false;
                file.laminaciy.transparency = result;
                textBoxFileName.Text = file.ToString();
            }
        }

        /// <summary>
        /// Выбрали текстуру ламинации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxLam2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onChenge)
            {                
                Polnocvet.Тexture texture = (Polnocvet.Тexture)Polnocvet.Тexture.Parse(typeof(Polnocvet.Тexture), comboBoxLam2.SelectedItem.ToString());
                file.laminaciy.texture = texture;
               
                textBoxFileName.Text = file.ToString();
            }
        }
        #endregion      

        /// <summary>
        /// Требуется переименовать исходный файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonRenameOn_CheckedChanged(object sender, EventArgs e)
        {
            global::zeppMailer.Properties.Settings.Default.renemePolnocvet = 0;
            global::zeppMailer.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Требуется создать копию файла с новым именем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonRenameNo_CheckedChanged(object sender, EventArgs e)
        {
            global::zeppMailer.Properties.Settings.Default.renemePolnocvet = 1;
            global::zeppMailer.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Просто отправить файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonRenameEs_CheckedChanged(object sender, EventArgs e)
        {
            global::zeppMailer.Properties.Settings.Default.renemePolnocvet = 3;
            global::zeppMailer.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Отправляем файлы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SentFiles_Click(object sender, EventArgs e)
        {           
            bool b0 = true;
            string msg = "Файлы помеченные: \"!!!\", - содержат ошибки в подготовленном имени файла. \r\n Вам необходимо их исправить!";
            for (int i = 0; i < files.Count; i++)
            {
                if (!files[i].FileOnServer)
                {
                    bool b1 = true;
                    string s = listBoxFile.Items[i].ToString();
                    foreach (bool b2 in files[i].onOff)
                    {
                        // msg += f.onOffn[i] + " - " + f.onOff[i].ToString() + "; ";
                        //listBoxFile.SelectedIndex

                        if (!b2)
                        {
                            this.listBoxFile.SelectedIndexChanged -= new System.EventHandler(this.listBoxFile_SelectedIndexChanged);
                            s = s.TrimStart('!', ' ');
                            listBoxFile.Items[i] = "!!! " + s;
                            this.listBoxFile.SelectedIndexChanged += new System.EventHandler(this.listBoxFile_SelectedIndexChanged);
                            b1 = false;
                            break;
                        }
                    }

                    if (b1)
                    {
                        listBoxFile.Items[i] = s.TrimStart('!', ' ');
                        switch (global::zeppMailer.Properties.Settings.Default.renemePolnocvet)
                        {
                            case 0:
                               /* try
                                {
                                    try { files[i].Startfile.CopyTo(files[i].Startfile.DirectoryName + "\\" + files[i].ToString()); }
                                    catch { MessageBox.Show("Ошибка при копировании файла: " + files[i].Startfile.DirectoryName + "\\" + files[i].ToString()); b0 = false; }
                                    files[i].Sеndfile = new System.IO.FileInfo(files[i].Startfile.DirectoryName + "\\" + files[i].ToString());
                                    try { files[i].Startfile.Delete(); }
                                    catch { MessageBox.Show("Ошибка при удалении файла: " + files[i].Startfile.FullName); }
                                    files[i].Startfile = files[i].Sеndfile;
                                }
                                catch { MessageBox.Show("Ошибка во время переименования файла: " + files[i].Startfile.FullName); b0 = false; }
                                break;*/
                            case 1:
                                try
                                {
                                    if (!File.Exists(files[i].Startfile.DirectoryName + "\\" + files[i].ToString()))
                                    {
                                        try { files[i].Startfile.CopyTo(files[i].Startfile.DirectoryName + "\\" + files[i].ToString()); }
                                        catch { MessageBox.Show("Ошибка при сохранении файла(1): " + files[i].Startfile.DirectoryName + "\\" + files[i].ToString()); b0 = false; }
                                    }
                                       files[i].Sеndfile = new System.IO.FileInfo(files[i].Startfile.DirectoryName + "\\" + files[i].ToString());
                                }
                                catch { MessageBox.Show("Ошибка при сохранени файла(2): " + files[i].Startfile.FullName); b0 = false; }
                                break;
                            case 3:
                                files[i].Sеndfile = files[i].Startfile;
                                break;
                        }

                    }
                    else b0 = b1;
                }

            }

            if (!b0) MessageBox.Show(msg);
            else
            {
                this.listBoxFile.SelectedIndex = 0;
                long len = 0;
                foreach (Polnocvet f in files) len += f.Sеndfile.Length;
                progressBarCopyFile.Maximum = int.Parse((len / 1000000).ToString());
                progressBarCopyFile.Minimum = 0;
                progressBarCopyFile.Value = 0;
                progressBarCopyFile.Visible = true;
                labelCopyFileAll.Visible = true;
                labelCopyFileAll.Text = "Всего: " + int.Parse((len / 1000000).ToString()) + "Mb";
                labelCopyFileAll.Tag = int.Parse((len / 1000000).ToString());
                labelCopyFileSaved.Visible = true;
                labelCopyFileSaved.Text = "Готово: 0";
                labelCopyFileSaved.Tag = 0;
                labelCopyFileSaving.Visible = true;
                labelCopyFileSaving.Text = "Сейчас: " + (files[0].Sеndfile.Length / 1000000).ToString();
                labelCopyFileSaving.Tag = int.Parse((files[0].Sеndfile.Length / 1000000).ToString());
                // Копируем файлы на сервер
                if (backgroundWorker1.IsBusy != true) backgroundWorker1.RunWorkerAsync();

                // Записыаем в базу на сайте  
                // ******** Перенесенено в функцию backgroundWorker1_RunWorkerCompleted, вызываемую по окончании копирования ******         
                //  if (!Helper.SavePolnocvetToMYSQL(files)) { MessageBox.Show("Неудалось создать задиния на сайте"); } ;
            }

        }

        /// <summary>
        /// Фоновое копирование файлов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;
            try
            {
                // Проверить доступнали папка
                if (!destDir.Exists) destDir.Create();
            }
            catch { MessageBox.Show("Ошибка при присоздании директории по адресу: " + destDir.ToString()); return; }

            foreach (Polnocvet f in files)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (worker.WorkerReportsProgress)
                            worker.ReportProgress(int.Parse((f.Startfile.Length / 1000000).ToString()), f);
                    try
                    {   
                        //FileSystem.CopyFile(f.Startfile.FullName, destDir + "\\" + f.ToString(), UIOption.AllDialogs, UICancelOption.ThrowException);

                        if (File.Exists(destDir + "\\" + f.ToString())) { MessageBox.Show("Такой файл уже сервере: " + f.Sеndfile.Name); continue; }
                        try { f.Startfile.CopyTo(destDir + "\\" + f.ToString()); f.FileOnServer = true; }
                        catch { MessageBox.Show("Ошибка при копировании файла на сервер: " + f.Sеndfile.FullName); f.FileOnServer = false; }                        
                    }
                    catch { MessageBox.Show("Ошибка при передаче файла на сервер: " + f.Sеndfile.Name); f.FileOnServer = false; }
                }
            }
        }

        /// <summary>
        /// Сообщение фонового процесса о ходе копирования файлов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarCopyFile.Value +=  e.ProgressPercentage;
            Polnocvet file = e.UserState as Polnocvet;          

            labelCopyFileSaved.Tag = long.Parse(labelCopyFileSaved.Tag.ToString()) + file.Sеndfile.Length / 1000000;
            labelCopyFileSaved.Text = "Готово: " + labelCopyFileSaved.Tag.ToString() + "Mb";

            int i = files.FindIndex(delegate (Polnocvet f) { return f.ToString() == file.ToString(); });
            if (files.Count - 1 >= i + 1)
            {
                labelCopyFileSaving.Tag = files[i].Sеndfile.Length / 1000000;
                labelCopyFileSaving.Text = "Сейчас: " + labelCopyFileSaving.Tag.ToString() + "Mb";            
                this.listBoxFile.SelectedIndex = i + 1;
            }
        }

        /// <summary>
        /// Фоновый процесс копирования завершен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ( abort)
            {
                DelAndStopForm();
                return;
            }
            if (!Helper.SavePolnocvetToMYSQL(files, linkFile)) { MessageBox.Show("Неудалось создать некоторые задания на сайте"); }
            progressBarCopyFile.Visible = false;
            labelCopyFileAll.Visible = false;
            labelCopyFileSaved.Visible = false;
            labelCopyFileSaving.Visible = false;
            for (int i =0; i < files.Count; i++)
            {
                if(files[i].id_file > 0)
                {
                    files.RemoveAt(i);
                    listBoxFile.Items.RemoveAt(i);
                    i--;
                }
            }
            if (files.Count == 0) file = new Polnocvet();
            else
            {
                file = files[files.Count - 1];
                listBoxFile.SelectedIndex = files.Count - 1;
            }
            listBoxFile_SelectedIndexChanged(new object(), new EventArgs());

            /*
            if (e.Cancelled == true)
            {
                resultLabel.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                resultLabel.Text = "Error: " + e.Error.Message;
            }
            else
            {
                resultLabel.Text = "Done!";
            }
            */
        }        

        /// <summary>
        /// Открыть сайт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLinck_Click(object sender, EventArgs e)
        {
            string linkStart = global::zeppMailer.Properties.Settings.Default.http_zepp + "index.php?option=com_zepp_polnocvet&view=main&Itemid=110";
            System.Diagnostics.Process.Start(linkStart);
        }       

        private void FormПолноцвет_FormClosing(object sender, FormClosingEventArgs e)
        {
            string caption = "Закрыть окно";
            string message = "";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            if (Helper.updateMySQL)
            {
                MessageBox.Show("Дождитесь окончание обновления базы");
                e.Cancel = true;
                return;
            }

            else if (backgroundWorker1.IsBusy)
            {
                message = "Идет коприрование файлов, остановит? (да)\r\n Файлы уже скопированные, будут удаленны с сервера";
                result = MessageBox.Show(this, message, caption, buttons);

                if (result == DialogResult.Yes)
                {
                    backgroundWorker1.CancelAsync();
                    this.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;
                    abort = true;
                   /* while (backgroundWorker1.IsBusy) {  }
                   / foreach (Polnocvet f in files)
                    {
                        if (File.Exists(destDir + "\\" + f.ToString()) && f.FileOnServer)
                        {
                            try { File.Delete(destDir + "\\" + f.ToString()); f.FileOnServer = false; }
                            catch { MessageBox.Show("Ошибка при удалении файла с сервера: " + f.Sеndfile.FullName); f.FileOnServer = false; }
                        }
                    }

                    //e.Cancel = false;
                    //this.Close();      */
                } else {
                    e.Cancel = true;  }
            }

            else if (files.Count > 0)
            {
                message = "Изменения в именах файлов не сохранятся, закрыть? (да)";
                result = MessageBox.Show(this, message, caption, buttons);

                if (result == DialogResult.No) e.Cancel = true;
            }
            else if(theUser.comp == null)
            {
                message = "Нужна авторизация";
                result = MessageBox.Show( message);
            }
            else
            {
               /* message = "Выйти? (да)";
                result = MessageBox.Show(this, message, caption, buttons);

                if (result == DialogResult.No) e.Cancel = true;*/
            }
        }

        void DelAndStopForm()
        {
             while (backgroundWorker1.IsBusy) {  }
                      foreach (Polnocvet f in files)
                       {
                           if (File.Exists(destDir + "\\" + f.ToString()) && f.FileOnServer)
                           {
                               try { File.Delete(destDir + "\\" + f.ToString()); f.FileOnServer = false; }
                               catch { MessageBox.Show("Ошибка при удалении файла с сервера: " + f.Sеndfile.FullName); f.FileOnServer = false; }
                           }
                       }

                       //e.Cancel = false;
                       this.Close();      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Polnocvet> tempFiles = new List<Polnocvet>();

            for ( int i = 0; i < files.Count; i++)
            {
                Polnocvet tempFile = new Polnocvet(file.ToString());                
                tempFile.coment = (i+1).ToString() + "_" + tempFile.coment;
                tempFile.Startfile = files[i].Startfile;
                tempFiles.Add(tempFile);               
            }

            files = new List<Polnocvet>(tempFiles);

            if (listBoxFile.SelectedIndex == -1) listBoxFile.SelectedIndex = 0;
            else file = files[listBoxFile.SelectedIndex];

            onChenge = true;
            comboBoxMaterial_SelectedIndexChanged(new object(), new EventArgs());
            textBoxFileName.Text = file.ToString();

        }
       
        private void button2_MouseMove(object sender, EventArgs e)
        {            
            toolTip1.ToolTipTitle = "Для всех файлов";           
            toolTip1.SetToolTip(button2, "Этот шаблон будет применен ко всем открытам файлам, \n все поля будут установленны как у текущего файла,\n поле \"коментарий\" получит порядковый номер");
        }

        private void radioButtonRenameOn_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Переименовать исходные файлы";
            toolTip1.SetToolTip(radioButtonRenameOn, "Исходные файлы получат названия как в созданых шаблонах");

        }

        private void radioButtonRenameNo_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Продублировать файлы";
            toolTip1.SetToolTip(radioButtonRenameNo, "Исходные файлы остануться с прежнми названиями,\n и будет создана копия с новыми именами");
        }

        private void radioButtonRenameEs_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Оставить исходные файлы";
            toolTip1.SetToolTip(radioButtonRenameEs, "Исходные файлы останутся с прежними названиями");

        }

        private void buttonDel_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Убрать из списка";
            toolTip1.SetToolTip(buttonDel, "Выбранный файл будет удален из списка, исходный файл останется на месте");
        }

        private void groupBoxCringle_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Автоматичекий подсчет количества люверсов";
            toolTip1.SetToolTip(groupBoxCringle, "Длина делится на 300 и округляется в меньшую сторону,\n не может быть меньше двух на одну сторону.");
        }

        private void textBoxFileName_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Сформированное название файла";
            toolTip1.SetToolTip(textBoxFileName, "Имя файла формируется из полей ниже, согласно шаблону.\n Вручную данное поле править нельзя, но можно скопировать");

        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Автоматичекий подсчет количества люверсов";
            toolTip1.SetToolTip(label1, "Длина делится на 300 и округляется в меньшую сторону,\n не может быть меньше двух на одну сторону.");

        }
    }
}

