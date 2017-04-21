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
using System.Reflection;
using System.Xml;

namespace zeppMailer
{
    public partial class FormPolnocvet : Form
    {
        //public string[] lam = new string[Properties.Settings.Default.Laminaciy.Count];// =   { "000г", "000м", "010г", "000м" };
        public FormPolnocvet()
        {
            InitializeComponent();
        }

        #region Поля
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
        BackgroundWorker worker;
        bool abort = false;
        /// <summary>
        /// для формирования паки на сервере (разделить файлы по датам)
        /// </summary>
        static string dataPatch;
        /// <summary>
        /// Папка установленная в настройках , куда нужно помещать файлы.
        /// </summary>
        DirectoryInfo destDir;
        /// <summary>
        /// Ссылка для сайта
        /// </summary>
        string linkFile;
        Material material = new Material();
        Stanok stanok = new Stanok();
        List<PrintFile> printFiles = new List<PrintFile>();
        List<Stanok> listStanok = Stanok.getListStanok();
        int selectIndex = 0;
        bool stickValidating = false;
        #endregion

        #region Элементы формы
        /// <summary>
        /// Форма загрузилась
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPolnocvet_Load(object sender, EventArgs e) {
            theUser = new User();
            managers = Helper.GetManagers();
            files = new List<Polnocvet>();
            dataPatch = DateTime.Today.Year.ToString() + "\\" + DateTime.Today.Month.ToString() + "\\" + DateTime.Today.Day.ToString();
            destDir = new DirectoryInfo(Properties.Settings.Default.PutchPolnocvet + "\\" + dataPatch);
            linkFile = Properties.Settings.Default.linkPolnocvet + DateTime.Today.Year.ToString().Trim(' ') + "/" + DateTime.Today.Month.ToString().Trim(' ') + "/" + DateTime.Today.Day.ToString().Trim(' ');
            PrintFile.lam =  new string[Properties.Settings.Default.laminaciy.Count + 1];
                global::zeppMailer.Properties.Settings.Default.laminaciy.CopyTo(PrintFile.lam, 1);
            
           
            if (theUser.comp == null) {
                this.Close(); return;
            }

            comboBoxManager.DataSource = managers;
            comboBoxManager.DisplayMember = "name";
            comboBoxManager.ValueMember = "project_user_id";
            comboBoxManager.SelectedValue = theUser.project_user_id;
            comboBoxCharId.DataSource = Helper.GetCharID(managers);
            comboBoxManager_SelectedIndexChanged(new object(), new EventArgs());

            //Stanok.getStanok();

            comboBoxStanok.DataSource = listStanok;
            comboBoxStanok.DisplayMember = "name";
            comboBoxStanok.ValueMember = "key";
            comboBoxStanok.SelectedIndex = -1;

            comboBoxMaterial.DataSource = Osnova.osnovs.Keys.ToArray();
            comboBoxMaterial.SelectedIndex = -1;

            comboBoxBasis.DataSource = Plotnost.plotnostes.Keys.ToArray();
            comboBoxBasis.SelectedIndex = -1;

            comboBoxColor.DataSource = ZColor.colors.Keys.ToArray();
            comboBoxColor.SelectedIndex = -1;

            comboBoxТexture.DataSource = Texture.textures.Values.ToArray();
            comboBoxТexture.DisplayMember = "name";
            comboBoxТexture.ValueMember = "simvol";
            comboBoxТexture.SelectedIndex = -1;


            PrintFile.lam[0] = "";
            comboBoxLaminaciy.DataSource = PrintFile.lam;
            comboBoxLaminaciy.SelectedIndex = -1;


            switch (global::zeppMailer.Properties.Settings.Default.renemePolnocvet) {
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
        /// Открыли файлы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFile_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;

            // получаем выбранный файл
            string[] filename = openFileDialog1.FileNames;
            //StringBuilder str = new StringBuilder();

            foreach (string s in filename) {
                int i = s.LastIndexOfAny("\\".ToCharArray());
                PrintFile printFile = new PrintFile();
                string n = s;
                if (i > 15) n = s.Substring(0, 5) + "..." + s.Substring(i - 8);
                listBoxFile.Items.Add(s);
                printFile.ParsingName(s); //n
                if (comboBoxCharId.SelectedIndex >= 0) printFile.charId = comboBoxCharId.SelectedValue.ToString();
                printFiles.Add(printFile);
            }

            if (printFiles.Count > 0) {
                buttonFile.BackColor = SystemColors.Control;
                listBoxFile.Enabled = true;
                buttonDel.Enabled = true;
            }

            if (printFiles.Count > 1) buttonForAll.Enabled = true;

            if (listBoxFile.SelectedIndex == -1) listBoxFile.SelectedIndex = 0;

            setCombos(selectIndex);
            textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        /// <summary>
        /// Выбрали файл из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxFile_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBoxFile.Items.Count > 0 && listBoxFile.Items.Count == printFiles.Count) {
                if (listBoxFile.SelectedIndex == -1) listBoxFile.SelectedIndex = 0;
                if (listBoxFile.SelectedIndex > listBoxFile.Items.Count - 1) listBoxFile.SelectedIndex = listBoxFile.Items.Count - 1;
                selectIndex = listBoxFile.SelectedIndex;
                //toolTip1.SetToolTip(listBoxFile, "Путь к выделенному элементу: \n" + printFiles[selectIndex].startfile.DirectoryName);
                setCombos(selectIndex);
                textBoxFileName.Text = printFiles[selectIndex].ToString();
            } else {
                listBoxFile.SelectedIndex = selectIndex = -1;
                textBoxFileName.Text = "";
                setCombos(selectIndex);
                if (listBoxFile.Items.Count != printFiles.Count) {
                    listBoxFile.Items.Clear();
                    printFiles.Clear();
                    MessageBox.Show("Ошибка списка файлов (listBoxFile_SelectedIndexChanged)");
                }
            }


            /*
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count) {
                selectIndex = listBoxFile.SelectedIndex;
                toolTip1.SetToolTip(listBoxFile, "Путь к выделенному элементу: \n" + printFiles[selectIndex].startfile.DirectoryName);
            }
            setCombos(selectIndex);
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
            else textBoxFileName.Text = "";
            */
        }
        /// <summary>
        /// Удалили файл из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDel_Click(object sender, EventArgs e) {

            if (selectIndex > -1) {
                int index = selectIndex;
                printFiles.RemoveAt(selectIndex);
                listBoxFile.Items.RemoveAt(selectIndex);

                if (listBoxFile.Items.Count > 0) {
                    if (index <= (listBoxFile.Items.Count - 1)) {
                        listBoxFile.SelectedIndex = index;
                    } else {
                        listBoxFile.SelectedIndex = listBoxFile.Items.Count - 1;
                    }
                }
                setCombos(selectIndex);
            }
        }
        private void comboBoxManager_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBoxManager.SelectedValue == null) {
                comboBoxManager.BackColor = Color.Yellow;
                comboBoxCharId.BackColor = Color.Yellow;
                comboBoxCharId.SelectedItem = theUser.project_user_id;
            } else {
                comboBoxCharId.SelectedItem = comboBoxManager.SelectedValue;
                comboBoxCharId.BackColor = Color.FromName("Window");
                comboBoxManager.BackColor = Color.FromName("Window");
                PrintFile.selectManager = (User)comboBoxManager.SelectedItem;
                if (printFiles.Count > 0) {
                    printFiles[selectIndex].charId = comboBoxCharId.SelectedValue.ToString();
                } else buttonFile.BackColor = Color.Yellow;

            }

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        /// <summary>
        /// Выбрали станок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxStanok_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && comboBoxStanok.SelectedValue != null)
                if (printFiles.Count > 0) {
                    if (printFiles[selectIndex].stanok == null) printFiles[selectIndex].stanok = new Stanok();
                    printFiles[selectIndex].stanok.SetStanok(comboBoxStanok.SelectedValue.ToString());
                }

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
            setCombos(selectIndex);
        }

        #region Материал
        /// <summary>
        /// Выбрать материал
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxMaterial_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && comboBoxMaterial.SelectedValue != null)
                if (printFiles.Count > 0) {
                    if (printFiles[selectIndex].material == null) printFiles[selectIndex].material = new Material();
                    printFiles[selectIndex].material.setName(comboBoxMaterial.SelectedValue.ToString());
                }

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
            setCombos(selectIndex);
        }
        /// <summary>
        /// Выбрали плотность
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxBasis_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && comboBoxBasis.SelectedValue != null)
                printFiles[selectIndex].material.setPlotnost(comboBoxBasis.SelectedValue.ToString());

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
            setCombos(selectIndex);
        }
        /// <summary>
        /// Выбрали цвет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxColor_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && comboBoxColor.SelectedValue != null)
                printFiles[selectIndex].material.setColor(int.Parse(comboBoxColor.SelectedValue.ToString()));

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
            setCombos(selectIndex);
        }
        /// <summary>
        /// Выбрали текстуру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxТexture_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && comboBoxТexture.SelectedValue != null)
                printFiles[selectIndex].material.setTexture(comboBoxТexture.SelectedValue.ToString()[0]);

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
            setCombos(selectIndex);
        }
        #endregion

        #region Отступы
        private void textBoxPadding_Validating(object sender, CancelEventArgs e) {
            int p = 0;
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && textBoxPadding.TextLength > 0) {
                if (int.TryParse(textBoxPadding.Text.ToString(), out p)) {
                    int os = p % 10;
                    if (os != 0) {
                        textBoxPadding.ForeColor = Color.Red;
                        labelPadding.Text = "Должно быть кратно 10";
                        // textBoxPadding.BackColor = Color.Yellow;
                        printFiles[selectIndex].padding = p;
                    } else {
                        printFiles[selectIndex].padding = p;
                        textBoxPadding.ForeColor = Color.Black;
                        labelPadding.Text = "Поля";
                    }

                } else {
                    textBoxPadding.ForeColor = Color.Red;
                    labelPadding.Text = "Должно быть число";
                }
            }
            if (textBoxPadding.TextLength <= 0) printFiles[selectIndex].padding = 0;
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void textBoxPadding_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) textBoxPadding_Validating(new object(), new CancelEventArgs());
        }
        #endregion

        #region Склей
        private void textBoxStick_Validating(object sender, CancelEventArgs e) {
            if (!stickValidating) {
                stickValidating = true;
                if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && textBoxStick.TextLength > 0) {
                    int p;
                    if (
                    int.TryParse(textBoxStick.Text.ToString(), out p)
                    && printFiles[selectIndex].sizeM != 0
                    && printFiles[selectIndex].sizeL != 0
                    ) {
                        checkStikM.Checked = (p == printFiles[selectIndex].sizeM);
                        checkStickL.Checked = (p == printFiles[selectIndex].sizeL);
                        checkBoxStick4.Checked = (p == (printFiles[selectIndex].sizeM + printFiles[selectIndex].sizeL) * 2);

                        if (!checkStikM.Checked && !checkStickL.Checked && !checkBoxStick4.Checked && p != 0) {
                            textBoxStick.ForeColor = Color.Red;
                            labelStick.Text = "Не кратно сторонам";
                            textBoxStick.Text = p.ToString();
                            printFiles[selectIndex].stick = p;
                        } else {
                            printFiles[selectIndex].stick = p;
                            textBoxStick.ForeColor = Color.Black;
                            labelStick.Text = "Склей";
                        }
                    } else {
                        textBoxStick.ForeColor = Color.Red;
                        labelStick.Text = "Должно быть число";
                    }
                }
                if (textBoxStick.TextLength <= 0) {
                    printFiles[selectIndex].stick = 0;
                    checkStikM.Checked = false;
                    checkStickL.Checked = false;
                    checkBoxStick4.Checked = false;
                }
                if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
                stickValidating = false;
            }
        }
        private void checkStikM_CheckedChanged(object sender, EventArgs e) {
            if (!stickValidating) {
                stickValidating = true;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked) {
                    checkStickL.Checked = false;
                    checkBoxStick4.Checked = false;

                    printFiles[selectIndex].stick = printFiles[selectIndex].sizeM;
                    textBoxStick.Text = printFiles[selectIndex].stick.ToString();
                    // textBoxStick_Validating(new object(), new CancelEventArgs());

                } else {
                    checkStickL.Checked = false;
                    checkBoxStick4.Checked = false;
                    printFiles[selectIndex].stick = 0;
                    textBoxStick.Text = "0";
                    //textBoxStick_Validating(new object(), new CancelEventArgs());
                }
                textBoxStick_Validating(new object(), new CancelEventArgs());
                //setCombos(selectIndex);
                stickValidating = false;
            }
        }
        private void checkStickL_CheckedChanged(object sender, EventArgs e) {
            if (!stickValidating) {
                stickValidating = true;

                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked) {
                    checkStikM.Checked = false;
                    checkBoxStick4.Checked = false;
                    printFiles[selectIndex].stick = printFiles[selectIndex].sizeL;

                    textBoxStick.Text = printFiles[selectIndex].sizeL.ToString();
                    //textBoxStick_Validating(new object(), new CancelEventArgs());
                } else {
                    checkStikM.Checked = false;
                    checkBoxStick4.Checked = false;
                    printFiles[selectIndex].stick = 0;
                    textBoxStick.Text = "0";
                    // textBoxStick_Validating(new object(), new CancelEventArgs());
                }
                textBoxStick_Validating(new object(), new CancelEventArgs());
                stickValidating = false;
            }
        }
        private void checkBoxStick4_CheckedChanged(object sender, EventArgs e) {
            if (!stickValidating) {
                stickValidating = true;
                CheckBox ch = (CheckBox)sender;
                if (ch.CheckState == CheckState.Checked) {
                    checkStikM.Checked = false;
                    checkStickL.Checked = false;

                    printFiles[selectIndex].stick = ((printFiles[selectIndex].sizeL + printFiles[selectIndex].sizeM) * 2);

                    textBoxStick.Text = ((printFiles[selectIndex].sizeL + printFiles[selectIndex].sizeM) * 2).ToString();
                    //textBoxStick_Validating(new object(), new CancelEventArgs());
                } else {
                    checkStikM.Checked = false;
                    checkStickL.Checked = false;
                    printFiles[selectIndex].stick = 0;
                    textBoxStick.Text = "0";
                    // textBoxStick_Validating(new object(), new CancelEventArgs());
                }
                textBoxStick_Validating(new object(), new CancelEventArgs());
                stickValidating = false;
            }
        }
        private void textBoxStick_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) textBoxStick_Validating(new object(), new CancelEventArgs());
        }
        #endregion

        #region Размеры
        private void textBoxSiseM_Validating(object sender, CancelEventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && textBoxSiseM.TextLength > 0) {
                selectIndex = listBoxFile.SelectedIndex;
                int p = 0;
                if (int.TryParse(textBoxSiseM.Text, out p) && p > 0) {
                    printFiles[selectIndex].sizeM = p;
                } else {
                    printFiles[selectIndex].sizeM = 0;
                }
            }
            setCombos(selectIndex);
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void textBoxSiseM_TextChanged(object sender, EventArgs e) {
            int i = 0;
            TextBox t = (TextBox)sender;
            if (int.TryParse(t.Text, out i)) {
                textBoxSiseM.BackColor = Color.FromName("Window");
                if (i > 0 && textBoxSiseL.TextLength > 0) {
                    SetOnTrue();
                }
            }
        }
        private void textBoxSiseM_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) textBoxSiseM_Validating(new object(), new CancelEventArgs());
        }
        private void textBoxSiseL_Validating(object sender, CancelEventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && textBoxSiseL.TextLength > 0) {
                selectIndex = listBoxFile.SelectedIndex;
                int p = 0;
                if (int.TryParse(textBoxSiseL.Text, out p) && p > 0) {
                    printFiles[selectIndex].sizeL = p;
                } else {
                    printFiles[selectIndex].sizeL = 0;
                }
            }
            setCombos(selectIndex);
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void textBoxSiseL_TextChanged(object sender, EventArgs e) {
            int i = 0;
            TextBox t = (TextBox)sender;
            if (int.TryParse(t.Text, out i)) {
                textBoxSiseL.BackColor = Color.FromName("Window");
                if (i > 0 && textBoxSiseM.TextLength > 0) {
                    SetOnTrue();
                }
            }
        }
        private void textBoxSiseL_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) textBoxSiseL_Validating(new object(), new CancelEventArgs());
        }
        #endregion

        #region Люверсы
        private void textBoxCringle_Validating(object sender, CancelEventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && textBoxCringle.TextLength > 0) {
                int p;
                checkBoxCringleOnL.Checked = false;
                checkBoxCringleOnL2.Checked = false;
                checkBoxCringleOnM.Checked = false;
                checkBoxCringleOnM2.Checked = false;
                checkBoxCringleON4.Checked = false;

                if (int.TryParse(textBoxCringle.Text.ToString(), out p)) {
                    printFiles[selectIndex].cringle = p;
                    textBoxCringle.ForeColor = Color.Black;
                    labelCringle.Text = "Люверсы";
                    textBoxFileName.Text = printFiles[selectIndex].ToString();
                    textBoxCringle.Text = p.ToString();


                } else {
                    textBoxCringle.ForeColor = Color.Red;
                    labelCringle.Text = "Должно быть число";

                }
            }
            if (textBoxCringle.TextLength <= 0) {
                printFiles[selectIndex].cringle = 0;
                checkBoxCringleOnL.Checked = false;
                checkBoxCringleOnL2.Checked = false;
                checkBoxCringleOnM.Checked = false;
                checkBoxCringleOnM2.Checked = false;
                checkBoxCringleON4.Checked = false;
            }
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void textBoxCringle_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) textBoxCringle_Validating(new object(), new CancelEventArgs());
        }
        private void checkBoxCringleOnM_CheckedChanged(object sender, EventArgs e) {

            CheckBox ch = (CheckBox)sender;
            if (ch.CheckState == CheckState.Checked) {
                checkBoxCringleON4.Checked = false;
                checkBoxCringleOnL.Checked = false;
                checkBoxCringleOnL2.Checked = false;
                checkBoxCringleOnM2.Checked = false;
                if (printFiles[selectIndex].sizeM > 600) {
                    textBoxCringle.Text = ((int)(printFiles[selectIndex].sizeM / 300)).ToString();
                    printFiles[selectIndex].cringle = ((int)(printFiles[selectIndex].sizeM / 300));
                } else {
                    textBoxCringle.Text = "2";
                    printFiles[selectIndex].cringle = 2;
                }

                //textBoxCringle_Validating(new object(), new CancelEventArgs());
            } else {
                // textBoxCringle.Text = "0";
                //printFiles[selectIndex].cringle = 0;
            }

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();

        }
        private void checkBoxCringleOnL_CheckedChanged(object sender, EventArgs e) {
            CheckBox ch = (CheckBox)sender;
            if (ch.CheckState == CheckState.Checked) {
                checkBoxCringleON4.Checked = false;
                checkBoxCringleOnL2.Checked = false;
                checkBoxCringleOnM.Checked = false;
                checkBoxCringleOnM2.Checked = false;
                if (printFiles[selectIndex].sizeL > 600) {
                    textBoxCringle.Text = ((int)(printFiles[selectIndex].sizeL / 300)).ToString();
                    printFiles[selectIndex].cringle = ((int)(printFiles[selectIndex].sizeL / 300));
                } else {
                    textBoxCringle.Text = "2";
                    printFiles[selectIndex].cringle = 2;
                }
                //textBoxCringle_Validating(new object(), new CancelEventArgs());
            } else {
                //   textBoxCringle.Text = "0";
                //  printFiles[selectIndex].cringle = 0;
            }
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void checkBoxCringleOnM2_CheckedChanged(object sender, EventArgs e) {
            CheckBox ch = (CheckBox)sender;
            if (ch.CheckState == CheckState.Checked) {
                checkBoxCringleON4.Checked = false;
                checkBoxCringleOnL.Checked = false;
                checkBoxCringleOnL2.Checked = false;
                checkBoxCringleOnM.Checked = false;

                if (printFiles[selectIndex].sizeM > 600) {
                    textBoxCringle.Text = ((int)(2 * printFiles[selectIndex].sizeM / 300)).ToString();
                    printFiles[selectIndex].cringle = ((int)(2 * printFiles[selectIndex].sizeM / 300));
                } else {
                    textBoxCringle.Text = "4";
                    printFiles[selectIndex].cringle = 4;
                }
                //textBoxCringle_Validating(new object(), new CancelEventArgs());
            } else {
                //  textBoxCringle.Text = "0";
                // printFiles[selectIndex].cringle = 0;
            }
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void checkBoxCringleOnL2_CheckedChanged(object sender, EventArgs e) {
            CheckBox ch = (CheckBox)sender;
            if (ch.CheckState == CheckState.Checked) {
                checkBoxCringleON4.Checked = false;
                checkBoxCringleOnL.Checked = false;
                checkBoxCringleOnM.Checked = false;
                checkBoxCringleOnM2.Checked = false;

                if (printFiles[selectIndex].sizeL > 600) {
                    textBoxCringle.Text = ((int)(2 * printFiles[selectIndex].sizeL / 300)).ToString();
                    printFiles[selectIndex].cringle = ((int)(2 * printFiles[selectIndex].sizeL / 300));
                } else {
                    textBoxCringle.Text = "4";
                    printFiles[selectIndex].cringle = 4;
                }
                //textBoxCringle_Validating(new object(), new CancelEventArgs());
            } else {
                //  textBoxCringle.Text = "0";
                // printFiles[selectIndex].cringle = 0;
            }
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void checkBoxCringleON4_CheckedChanged(object sender, EventArgs e) {
            CheckBox ch = (CheckBox)sender;
            if (ch.CheckState == CheckState.Checked) {
                int c = 0;
                checkBoxCringleOnL.Checked = false;
                checkBoxCringleOnL2.Checked = false;
                checkBoxCringleOnM.Checked = false;
                checkBoxCringleOnM2.Checked = false;

                if (printFiles[selectIndex].sizeL < 600) c = 4;
                if (printFiles[selectIndex].sizeM < 600) c += 4;

                textBoxCringle.Text = (((int)(2 * (printFiles[selectIndex].sizeL + printFiles[selectIndex].sizeM) / 300)) + c).ToString();
                printFiles[selectIndex].cringle = (((int)(2 * (printFiles[selectIndex].sizeL + printFiles[selectIndex].sizeM) / 300)) + c);

                // textBoxCringle_Validating(new object(), new CancelEventArgs());
            } else {
                // textBoxCringle.Text = "0";
                //printFiles[selectIndex].cringle = 0;
            }
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        #endregion

        #region Копии
        private void textBoxCopyes_Validating(object sender, CancelEventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && textBoxCopyes.TextLength > 0) {
                int p = 0;
                if (int.TryParse(textBoxCopyes.Text.ToString(), out p)) {
                    printFiles[selectIndex].copyes = p;
                    textBoxCopyes.ForeColor = Color.Black;
                    labelCopyes.Text = "Количество копий";

                } else {
                    textBoxCopyes.ForeColor = Color.Red;
                    labelCopyes.Text = "Должно быть число";
                }
            }

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void textBoxCopyes_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) textBoxCopyes_Validating(new object(), new CancelEventArgs());
        }
        #endregion

        /// <summary>
        /// Выбрали материал ламинации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxLaminaciy_SelectionChangeCommitted(object sender, EventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && comboBoxLaminaciy.SelectedValue != null)
                if (printFiles.Count > 0) {
                    // if (printFiles[selectIndex].laminaciy == null) printFiles[selectIndex].laminaciy = new ;
                    printFiles[selectIndex].laminaciy = comboBoxLaminaciy.SelectedValue.ToString();
                }

            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
            setCombos(selectIndex);
        }

        #region Номер
        private void textBoxNumber_Validating(object sender, CancelEventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && textBoxNumber.TextLength > 0) {
                textBoxNumber.BackColor = Color.FromName("Window");
                printFiles[selectIndex].number = textBoxNumber.Text;
            } else {
                textBoxNumber.BackColor = Color.Yellow;
                printFiles[selectIndex].number = textBoxNumber.Text;
            }
            setCombos(selectIndex);
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void textBoxNumber_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) textBoxNumber_Validating(new object(), new CancelEventArgs());
        }
        #endregion

        #region Коментарий
        private void textBoxComent_Validating(object sender, CancelEventArgs e) {
            if (listBoxFile.SelectedIndex >= 0 && listBoxFile.SelectedIndex <= printFiles.Count && textBoxComent.TextLength > 0)
                printFiles[selectIndex].coment = textBoxComent.Text;
            else printFiles[selectIndex].coment = "";
            if (printFiles.Count > 0) textBoxFileName.Text = printFiles[selectIndex].ToString();
        }
        private void textBoxComent_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) textBoxComent_Validating(new object(), new CancelEventArgs());
        }
        #endregion

        private void listBoxFile_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue == 46) buttonDel_Click(sender, e);
        }
        private void buttonForAll_Click(object sender, EventArgs e) {
            //  selectIndex
            // printFiles
            string comment = printFiles[selectIndex].coment;

            for (int i = 0; i < printFiles.Count; i++) {
                if (i != selectIndex) {
                    printFiles[i].copyes = printFiles[selectIndex].copyes;
                    printFiles[i].cringle = printFiles[selectIndex].cringle;
                    printFiles[i].laminaciy = printFiles[selectIndex].laminaciy;
                    printFiles[i].number = printFiles[selectIndex].number;
                    printFiles[i].padding = printFiles[selectIndex].padding;
                    printFiles[i].sizeL = printFiles[selectIndex].sizeL;
                    printFiles[i].sizeM = printFiles[selectIndex].sizeM;

                    if (printFiles[selectIndex].stanok != null) {
                        printFiles[i].stanok = new Stanok();
                        printFiles[i].stanok.SetStanok(printFiles[selectIndex].stanok.key);
                    }

                    printFiles[i].stick = printFiles[selectIndex].stick;
                    printFiles[i].coment = (i + 1).ToString() + " " + comment;

                    if (printFiles[selectIndex].material != null) {
                        printFiles[i].material = new Material();
                        printFiles[i].material.setName(printFiles[selectIndex].material.name);
                        if (printFiles[selectIndex].material.isPlotnost && printFiles[selectIndex].material.plotnost != null) printFiles[i].material.setPlotnost(printFiles[selectIndex].material.plotnost);
                        if (printFiles[selectIndex].material.isTexture && printFiles[selectIndex].material.texture != null) printFiles[i].material.setTexture(printFiles[selectIndex].material.texture);
                        if (printFiles[selectIndex].material.isColor && printFiles[selectIndex].material.color != null) printFiles[i].material.setColor(printFiles[selectIndex].material.color);
                    }

                } else {
                    printFiles[selectIndex].coment = "1 " + comment;
                }
            }
            setCombos(selectIndex);
        }
        #region Радио кнопки перезаписи
        /// <summary>
        /// Требуется переименовать исходный файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonRenameOn_CheckedChanged(object sender, EventArgs e) {
            global::zeppMailer.Properties.Settings.Default.renemePolnocvet = 0;
            global::zeppMailer.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Требуется создать копию файла с новым именем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonRenameNo_CheckedChanged(object sender, EventArgs e) {
            global::zeppMailer.Properties.Settings.Default.renemePolnocvet = 1;
            global::zeppMailer.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Просто отправить файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonRenameEs_CheckedChanged(object sender, EventArgs e) {
            global::zeppMailer.Properties.Settings.Default.renemePolnocvet = 3;
            global::zeppMailer.Properties.Settings.Default.Save();
        }
        #endregion
        /// <summary>
        /// открыть сайт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLinck_Click_1(object sender, EventArgs e) {
            string linkStart = global::zeppMailer.Properties.Settings.Default.http_zepp + "index.php?option=com_zepp_polnocvet&view=main&Itemid=110";
            System.Diagnostics.Process.Start(linkStart);
        }
        #endregion

        #region Поля формы - отображение, проверка значений
        /// <summary>
        /// Установка значений в полях
        /// </summary>
        /// <param name="index"></param>
        private void setCombos(int index) {
            if (printFiles.Count > 0 && index <= printFiles.Count - 1) {
                #region Станок
                if (printFiles[index].stanok != null && printFiles[index].stanok.key != null) {
                    comboBoxStanok.SelectedValue = printFiles[index].stanok.key;
                    comboBoxStanok.BackColor = Color.FromName("Window");
                } else {
                    comboBoxStanok.BackColor = Color.Yellow;
                    comboBoxStanok.SelectedValue = -1;
                }
                labelStanok.Enabled = comboBoxStanok.Enabled = true;
                #endregion

                #region Материал
                if (printFiles[index].material != null &&
                    printFiles[index].material.name != null &&
                    printFiles[index].material.name.Length > 0) {
                    #region Иницилизация полей формы материала установленным значением для выбранного файла и включение полей размера
                    comboBoxMaterial.BackColor = Color.FromName("Window");
                    comboBoxMaterial.SelectedItem = printFiles[index].material.name.ToUpper();
                    labelMaterial.Enabled = comboBoxMaterial.Enabled = true;

                    #region плотность                    
                    if (printFiles[index].material.isPlotnost) {
                        if (printFiles[index].material.plotnost != null) {
                            comboBoxBasis.BackColor = Color.FromName("Window");
                            comboBoxBasis.SelectedItem = printFiles[index].material.plotnost;
                        } else {
                            comboBoxBasis.BackColor = Color.Yellow;
                            comboBoxBasis.SelectedIndex = -1;
                        }
                        labelBasis.Enabled = comboBoxBasis.Enabled = true;
                    } else {
                        comboBoxBasis.SelectedIndex = -1;
                        labelBasis.Enabled = comboBoxBasis.Enabled = false;
                        comboBoxBasis.BackColor = Color.FromName("Window");
                    }
                    #endregion

                    #region Текстура                   
                    if (printFiles[index].material.isTexture) {
                        if (printFiles[index].material.texture != null) {
                            comboBoxТexture.BackColor = Color.FromName("Window");
                            comboBoxТexture.SelectedValue = printFiles[index].material.texture;
                        } else {
                            comboBoxТexture.BackColor = Color.Yellow;
                            comboBoxТexture.SelectedIndex = -1;
                        }

                        labelТexture.Enabled = comboBoxТexture.Enabled = true;
                    } else {
                        comboBoxТexture.SelectedIndex = -1;
                        labelТexture.Enabled = comboBoxТexture.Enabled = false;
                        comboBoxТexture.BackColor = Color.FromName("Window");
                    }
                    #endregion

                    #region Цвет                   
                    if (printFiles[index].material.isColor) {
                        if (printFiles[index].material.color != null) {
                            comboBoxColor.BackColor = Color.FromName("Window");
                            comboBoxColor.SelectedItem = printFiles[index].material.color;
                        } else {
                            comboBoxColor.BackColor = Color.Yellow;
                            comboBoxColor.SelectedIndex = -1;
                        }
                        labelColor.Enabled = comboBoxColor.Enabled = true;
                    } else {
                        comboBoxColor.SelectedIndex = -1;
                        labelColor.Enabled = comboBoxColor.Enabled = false;
                        comboBoxColor.BackColor = Color.FromName("Window");
                    }
                    #endregion

                    labelSiseM.Enabled = textBoxSiseM.Enabled = true;
                    labelSiseL.Enabled = textBoxSiseL.Enabled = true;
                    #endregion
                } else {
                    #region Обнуление полей материала размеров
                    comboBoxMaterial.SelectedIndex = -1;
                    labelMaterial.Enabled = comboBoxMaterial.Enabled = true;
                    comboBoxMaterial.BackColor = Color.Yellow;

                    comboBoxBasis.SelectedIndex = -1;
                    labelBasis.Enabled = comboBoxBasis.Enabled = false;
                    comboBoxBasis.BackColor = Color.FromName("Window");

                    comboBoxТexture.SelectedIndex = -1;
                    labelТexture.Enabled = comboBoxТexture.Enabled = false;
                    comboBoxТexture.BackColor = Color.FromName("Window");

                    comboBoxColor.SelectedIndex = -1;
                    labelColor.Enabled = comboBoxColor.Enabled = false;
                    comboBoxColor.BackColor = Color.FromName("Window");

                    labelSiseM.Enabled = textBoxSiseM.Enabled = false;
                    textBoxSiseM.BackColor = Color.FromName("Window");
                    labelSiseL.Enabled = textBoxSiseL.Enabled = false;
                    textBoxSiseL.BackColor = Color.FromName("Window");
                    #endregion
                }
                #endregion

                #region Размеры
                bool setSise = true;
                #region Больший
                if (printFiles[index] != null && printFiles[index].sizeM > 0) {
                    #region Иницилизация поля формы установленным значением для выбранного файла
                    textBoxSiseM.Enabled = true;
                    textBoxSiseM.BackColor = Color.FromName("Window");
                    textBoxSiseM.Text = printFiles[index].sizeM.ToString();
                    #endregion
                } else {
                    if (textBoxSiseM.Enabled) textBoxSiseM.BackColor = Color.Yellow;
                    textBoxSiseM.Text = "";
                    setSise = false;
                }
                #endregion

                #region Меньший
                if (printFiles[index] != null && printFiles[index].sizeL > 0) {
                    #region Иницилизация поля формы установленным значением для выбранного файла
                    textBoxSiseL.Enabled = true;
                    textBoxSiseL.BackColor = Color.FromName("Window");
                    textBoxSiseL.Text = printFiles[index].sizeL.ToString();
                    #endregion
                } else {
                    if (textBoxSiseL.Enabled) textBoxSiseL.BackColor = Color.Yellow;
                    textBoxSiseL.Text = "";
                    setSise = false;
                }



                if (printFiles[index].sizeL > printFiles[index].sizeM && setSise) {
                    int s = printFiles[index].sizeL;
                    printFiles[index].sizeL = printFiles[index].sizeM;
                    printFiles[index].sizeM = s;
                    textBoxSiseL.Text = printFiles[index].sizeL.ToString();
                    textBoxSiseM.Text = printFiles[index].sizeM.ToString();
                }
                #endregion

                #endregion

                #region Числовые поля
                if (setSise && textBoxSiseM.Enabled) {
                    SetOnTrue();
                    UnsetVolumeText();
                    SetVolumeText();
                } else {
                    SetOnFalse();
                    UnsetVolumeText();
                }
                #endregion

                #region Номер
                if (printFiles[index].number != null) {
                    textBoxNumber.Text = printFiles[index].number;
                    textBoxNumber.BackColor = Color.FromName("Window");
                } else {
                    textBoxNumber.BackColor = Color.Yellow;
                    textBoxNumber.Text = "";
                }
                #endregion

                #region Кнопки отправки файла
                if (isSetPrintFile()) {
                    groupBoxRename.Enabled = true;
                    buttonSendFile.Enabled = true;
                    buttonSendFile.Visible = true;
                    buttonLinck.Enabled = false;
                    buttonLinck.Visible = false;
                } else {
                    groupBoxRename.Enabled = false;
                    buttonSendFile.Enabled = false;
                    buttonSendFile.Visible = false;
                    buttonLinck.Enabled = true;
                    buttonLinck.Visible = true;
                }
                #endregion

                // вызываю перерисовку окна файлов для выдиления ошибочных файлов
                listBoxFile.Invalidate();

            } else {
                labelStanok.Enabled = comboBoxStanok.Enabled = false; comboBoxStanok.SelectedIndex = -1;
                labelMaterial.Enabled = comboBoxMaterial.Enabled = false; comboBoxMaterial.SelectedIndex = -1;
                labelBasis.Enabled = comboBoxBasis.Enabled = false; comboBoxBasis.SelectedIndex = -1;
                labelТexture.Enabled = comboBoxТexture.Enabled = false; comboBoxТexture.SelectedIndex = -1;
                labelColor.Enabled = comboBoxColor.Enabled = false; comboBoxColor.SelectedIndex = -1;
                labelSiseM.Enabled = textBoxSiseM.Enabled = false; textBoxSiseM.Text = "";
                labelSiseL.Enabled = textBoxSiseL.Enabled = false; textBoxSiseL.Text = "";
                groupBoxRename.Enabled = false;
                buttonSendFile.Enabled = false;
                buttonSendFile.Visible = false;
                buttonLinck.Enabled = true;
                buttonLinck.Visible = true;
                textBoxNumber.Text = "";
                buttonDel.Enabled = false;
                SetOnFalse();
                UnsetVolumeText();
            }

        }
        private void SetOnFalse() {
            labelPadding.Enabled = textBoxPadding.Enabled = false;
            groupBoxStick.Enabled = false;
            labelStick.Enabled = textBoxStick.Enabled = false;
            labelCringle.Enabled = textBoxCringle.Enabled = false;
            groupBoxCringle.Enabled = false;
            labelCopyes.Enabled = textBoxCopyes.Enabled = false;
            labelLaminaciy.Enabled = comboBoxLaminaciy.Enabled = false;
            labelNumber.Enabled = textBoxNumber.Enabled = false;
            labelComent.Enabled = textBoxComent.Enabled = false;
        }
        private void UnsetVolumeText() {
            textBoxPadding.Text = "";
            textBoxStick.Text = "";
            textBoxCringle.Text = "";
            textBoxCopyes.Text = "";
            comboBoxLaminaciy.SelectedIndex = -1;
            // textBoxNumber.Text = "";
            textBoxComent.Text = "";
        }
        private void SetOnTrue() {
            labelPadding.Enabled = textBoxPadding.Enabled = true;
            groupBoxStick.Enabled = true;
            labelStick.Enabled = textBoxStick.Enabled = true;
            labelCringle.Enabled = textBoxCringle.Enabled = true;
            groupBoxCringle.Enabled = true;
            labelCopyes.Enabled = textBoxCopyes.Enabled = true;
            labelLaminaciy.Enabled = comboBoxLaminaciy.Enabled = true;
            labelNumber.Enabled = textBoxNumber.Enabled = true;
            labelComent.Enabled = textBoxComent.Enabled = true;
        }
        private void SetVolumeText() {
            if (printFiles[selectIndex].padding > 0) textBoxPadding.Text = printFiles[selectIndex].padding.ToString();
            if (printFiles[selectIndex].stick > 0) textBoxStick.Text = printFiles[selectIndex].stick.ToString();
            if (printFiles[selectIndex].cringle > 0) textBoxCringle.Text = printFiles[selectIndex].cringle.ToString();
            if (printFiles[selectIndex].copyes > 0) textBoxCopyes.Text = printFiles[selectIndex].copyes.ToString();
            comboBoxLaminaciy.SelectedItem = printFiles[selectIndex].laminaciy;
            if (printFiles[selectIndex].coment.Length > 0) textBoxComent.Text = printFiles[selectIndex].coment.ToString();
        }
        /// <summary>
        /// Возвращаем ложь если параметры файлов не верны
        /// </summary>
        /// <returns></returns>
        bool isSetPrintFile() {
            bool rert = true;
            foreach (PrintFile f in printFiles) if (!f.IsSetPropertys()) rert = false;
            return rert;
        }
        #endregion

        #region Прорисовка окна со списком файлов
        /// <summary>
        /// Изминяем цвет строк в окне с файлами если параметры файлов не верны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxFile_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index >= 0) {
                string st = listBoxFile.Items[e.Index].ToString();
                StringBuilder n = new StringBuilder();
                Size len = TextRenderer.MeasureText(st, listBoxFile.Font);
                Size len2 = TextRenderer.MeasureText(st, listBoxFile.Font);
                n.Append(st);
                if (e.Bounds.Width < (len.Width - 10)) {
                    double pr = (len.Width * 1.0) / e.Bounds.Width; // восколько окно больше строк в пикселях
                    double r = (st.Length * 1.0) / pr; // Сколько пикселов рисовать      
                    n.Clear();
                    n.Append(st.Substring(0, 5) + "..." + st.Substring(st.Length - (int)r + 15));
                }

                e.DrawBackground();
                Brush myBrush = Brushes.Black;
                Brush myFonBrush = new SolidBrush(Color.FromName("Window")); //Brushes.Yellow;
                                                                             // Если ошибки в параметрах файла
                if (!printFiles[e.Index].IsSetPropertys()) myFonBrush = Brushes.Yellow;// Brushes.Red;
                                                                                       // Если ошибки в при пересохранения файла файла
                if (!printFiles[e.Index].resaveFile) myBrush = Brushes.Red;
                // Если текущая строка
                if (e.State.HasFlag(DrawItemState.Selected)) {
                    if (myFonBrush == Brushes.Yellow) myBrush = Brushes.Red;
                    myFonBrush = Brushes.PowderBlue;
                }
                e.Graphics.FillRectangle(myFonBrush, e.Bounds);
                e.Graphics.DrawString(n.ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        private void FormPolnocvet_Resize(object sender, EventArgs e) {
            listBoxFile.Invalidate();
        }
        #endregion

        #region Отправка файлов на сервер и смежные элементы формы
        private void buttonSendFile_Click(object sender, EventArgs e) {
            // Сохранение файла согласно выбраным условиям перед отправкой
            StringBuilder msg = new StringBuilder("Ошибки при сохранении файлов на диск:\n");
            ReSaveFile(msg);

            if (PrintFile.errSave) {// Если есть ошибки отправки нет
                msg.AppendLine("Продолжить отправку файлов?\n(OK - продолжить, ОТМЕНА - остановить");
                DialogResult result = MessageBox.Show(msg.ToString(), "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK) PrintFile.errSave = false;
            }

            if (!PrintFile.errSave) {
                #region Копируем файлы на сервер
                this.listBoxFile.SelectedIndex = 0;
                long len = 0;
                foreach (PrintFile f in printFiles) len += f.sеndfile.Length;
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
                labelCopyFileSaving.Text = "Сейчас: " + (printFiles[0].sеndfile.Length / 1000000).ToString();
                labelCopyFileSaving.Tag = int.Parse((printFiles[0].sеndfile.Length / 1000000).ToString());
                // Копируем файлы на сервер
                if (backgroundWorker1.IsBusy != true) backgroundWorker1.RunWorkerAsync();

                // Записыаем в базу на сайте  
                // ******** Перенесенено в функцию backgroundWorker1_RunWorkerCompleted, вызываемую по окончании копирования ******         
                //  if (!Helper.SavePolnocvetToMYSQL(files)) { MessageBox.Show("Неудалось создать задиния на сайте"); } ; 
                #endregion
            }
        }
        /// <summary>
        /// Пересохранение файлов
        /// </summary>
        /// <param name="msg"></param>
        private void ReSaveFile(StringBuilder msg) {

            switch (global::zeppMailer.Properties.Settings.Default.renemePolnocvet) {
                case 0:
                MessageBox.Show("Функция перезаписи файла отключенна");
                /* try
                 {
                     try { files[i].Startfile.CopyTo(files[i].Startfile.DirectoryName + "\\" + files[i].ToString()); }
                     catch { MessageBox.Show("Ошибка при копировании файла: " + files[i].Startfile.DirectoryName + "\\" + files[i].ToString()); b0 = false; }
                     files[i].Sеndfile = new System.IO.FileInfo(files[i].Startfile.DirectoryName + "\\" + files[i].ToString());
                     try { files[i].Startfile.Delete(); }
                     catch { MessageBox.Show("Ошибка при удалении файла: " + files[i].Startfile.FullName); }
                     files[i].Startfile = files[i].Sеndfile;
                 }
                 catch { MessageBox.Show("Ошибка во время переименования файла: " + files[i].Startfile.FullName); b0 = false; }*/
                break;
                case 1: // Сохранить себе как копию
                foreach (PrintFile s in printFiles) {
                    try {
                        if (!File.Exists(s.startfile.DirectoryName + "\\" + s.ToString())) {
                            try { s.startfile.CopyTo(s.startfile.DirectoryName + "\\" + s.ToString()); }
                            catch { MessageBox.Show("Ошибка(1) при сохранении файла: " + s.startfile.DirectoryName + "\\" + s.ToString() + "\n Отмеченно красным"); s.resaveFile = false; PrintFile.errSave = true; msg.AppendLine(s.startfile.Name); listBoxFile.Invalidate(); }
                        }
                        s.sеndfile = new System.IO.FileInfo(s.startfile.DirectoryName + "\\" + s.ToString());
                    }
                    catch { MessageBox.Show("Ошибка(2) при сохранени файла: " + s.startfile.FullName + "\n Отмеченно красным"); s.resaveFile = false; PrintFile.errSave = true; msg.AppendLine(s.startfile.Name); listBoxFile.Invalidate(); }
                }
                break;
                case 3:// Не оставлять себе
                foreach (PrintFile s in printFiles) {
                    s.sеndfile = s.startfile;
                    listBoxFile.Invalidate();
                }
                break;
            }

            //MessageBox.Show("Ошибка(2) при сохранени файла: " + printFiles[1].startfile.FullName + "\n Отмеченно красным"); printFiles[1].resaveFile = false; msg.AppendLine(printFiles[1].startfile.Name);
            //MessageBox.Show("Ошибка(2) при сохранени файла: " + printFiles[3].startfile.FullName + "\n Отмеченно красным"); printFiles[3].resaveFile = false; msg.AppendLine(printFiles[3].startfile.Name);
            //MessageBox.Show("Ошибка(2) при сохранени файла: " + printFiles[4].startfile.FullName + "\n Отмеченно красным"); printFiles[4].resaveFile = false; msg.AppendLine(printFiles[4].startfile.Name);
            //PrintFile.errSave = true;
            //listBoxFile.Invalidate();
        }
        /// <summary>
        /// Фоновое копирование файлов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            worker = sender as BackgroundWorker;
            try {
                // Проверить доступнали папка
                if (!destDir.Exists) destDir.Create();
            }
            catch { MessageBox.Show("Ошибка при создании директории по адресу: " + destDir.ToString()); return; }

            foreach (PrintFile f in printFiles) {
                if (worker.CancellationPending == true) {
                    e.Cancel = true;
                    break;
                } else {
                    if (worker.WorkerReportsProgress)
                        worker.ReportProgress(int.Parse((f.startfile.Length / 1000000).ToString()), f);
                    try {
                        if (File.Exists(destDir + "\\" + f.ToString())) { MessageBox.Show("Такой файл уже сервере: " + f.sеndfile.Name); continue; }
                        try { f.startfile.CopyTo(destDir + "\\" + f.ToString()); f.fileOnServer = true; }
                        catch { MessageBox.Show("Ошибка при копировании файла на сервер: " + f.sеndfile.FullName); f.fileOnServer = false; }
                    }
                    catch { MessageBox.Show("Ошибка при передаче файла на сервер: " + f.sеndfile.Name); f.fileOnServer = false; }
                }
            }
        }

        /// <summary>
        /// Сообщение фонового процесса о ходе копирования файлов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            progressBarCopyFile.Value += e.ProgressPercentage;
            PrintFile file = e.UserState as PrintFile;

            labelCopyFileSaved.Tag = long.Parse(labelCopyFileSaved.Tag.ToString()) + file.sеndfile.Length / 1000000;
            labelCopyFileSaved.Text = "Готово: " + labelCopyFileSaved.Tag.ToString() + "Mb";

            int i = printFiles.FindIndex(delegate (PrintFile f) { return f.ToString() == file.ToString(); });
            if (printFiles.Count - 1 >= i + 1) {
                labelCopyFileSaving.Tag = printFiles[i].sеndfile.Length / 1000000;
                labelCopyFileSaving.Text = "Сейчас: " + labelCopyFileSaving.Tag.ToString() + "Mb";
                this.listBoxFile.SelectedIndex = i + 1;
            }
        }

        /// <summary>
        /// Фоновый процесс копирования завершен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (abort) {
                DelAndStopForm();
                return;
            }
            if (!Helper.SavePolnocvetToMYSQL(printFiles, linkFile)) { MessageBox.Show("Не удалось создать некоторые задания на сайте"); }
            progressBarCopyFile.Visible = false;
            labelCopyFileAll.Visible = false;
            labelCopyFileSaved.Visible = false;
            labelCopyFileSaving.Visible = false;
            for (int i = 0; i < printFiles.Count; i++) {
                if (printFiles[i].id_file > 0) {
                    printFiles.RemoveAt(i);
                    listBoxFile.Items.RemoveAt(i);
                    i--;
                }
            }
            selectIndex = printFiles.Count - 1;
            listBoxFile_SelectedIndexChanged(new object(), new EventArgs());
        }

        /// <summary>
        /// Открыть сайт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLinck_Click(object sender, EventArgs e) {
            string linkStart = global::zeppMailer.Properties.Settings.Default.http_zepp + "index.php?option=com_zepp_polnocvet&view=main&Itemid=110";
            System.Diagnostics.Process.Start(linkStart);
        }

        private void FormПолноцвет_FormClosing(object sender, FormClosingEventArgs e) {
            string caption = "Закрыть окно";
            string message = "";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            if (Helper.updateMySQL) {
                MessageBox.Show("Дождитесь окончание обновления базы");
                e.Cancel = true;
                return;
            } else if (backgroundWorker1.IsBusy) {
                message = "Идет копирование файлов, остановить? (да)\r\n Файлы уже скопированные, будут удалены с сервера";
                result = MessageBox.Show(this, message, caption, buttons);

                if (result == DialogResult.Yes) {
                    backgroundWorker1.CancelAsync();
                    this.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;
                    abort = true;

                } else {
                    e.Cancel = true;
                }
            } else if (printFiles.Count > 0) {
                message = "Изменения в именах файлов не сохранятся, закрыть? (да)";
                result = MessageBox.Show(this, message, caption, buttons);

                if (result == DialogResult.No) e.Cancel = true;
            } else if (theUser.comp == null) {
                message = "Нужна авторизация";
                result = MessageBox.Show(message);
            } else {
                /* message = "Выйти? (да)";
                 result = MessageBox.Show(this, message, caption, buttons);

                 if (result == DialogResult.No) e.Cancel = true;*/
            }
        }

        void DelAndStopForm() {
            while (backgroundWorker1.IsBusy) { }
            foreach (PrintFile f in printFiles) {
                if (File.Exists(destDir + "\\" + f.ToString()) && f.fileOnServer) {
                    try { File.Delete(destDir + "\\" + f.ToString()); f.fileOnServer = false; }
                    catch { MessageBox.Show("Ошибка при удалении файла с сервера: " + f.sеndfile.FullName); f.fileOnServer = false; }
                }
            }

            //e.Cancel = false;
            this.Close();
        }

        #endregion

    }
}
