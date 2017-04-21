namespace zeppMailer
{
    partial class FormПолноцвет
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormПолноцвет));
            this.comboBoxManager = new System.Windows.Forms.ComboBox();
            this.labelManager = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.listBoxFile = new System.Windows.Forms.ListBox();
            this.buttonFile = new System.Windows.Forms.Button();
            this.comboBoxStanok = new System.Windows.Forms.ComboBox();
            this.labelStanok = new System.Windows.Forms.Label();
            this.labelMaterial = new System.Windows.Forms.Label();
            this.comboBoxMaterial = new System.Windows.Forms.ComboBox();
            this.labelStick = new System.Windows.Forms.Label();
            this.labelCringle = new System.Windows.Forms.Label();
            this.labelSiseL = new System.Windows.Forms.Label();
            this.labelSiseM = new System.Windows.Forms.Label();
            this.labelCharId = new System.Windows.Forms.Label();
            this.comboBoxCharId = new System.Windows.Forms.ComboBox();
            this.labelLaminaciy = new System.Windows.Forms.Label();
            this.comboBoxLaminaciy = new System.Windows.Forms.ComboBox();
            this.labelCopyes = new System.Windows.Forms.Label();
            this.labelComent = new System.Windows.Forms.Label();
            this.labelNumber = new System.Windows.Forms.Label();
            this.groupBoxCringle = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxCringleON4 = new System.Windows.Forms.CheckBox();
            this.checkBoxCringleOnL2 = new System.Windows.Forms.CheckBox();
            this.checkBoxCringleOnM2 = new System.Windows.Forms.CheckBox();
            this.checkBoxCringleOnL = new System.Windows.Forms.CheckBox();
            this.checkBoxCringleOnM = new System.Windows.Forms.CheckBox();
            this.groupBoxStick = new System.Windows.Forms.GroupBox();
            this.checkBoxStick4 = new System.Windows.Forms.CheckBox();
            this.checkStickL = new System.Windows.Forms.CheckBox();
            this.checkStikM = new System.Windows.Forms.CheckBox();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.textBoxStick = new System.Windows.Forms.TextBox();
            this.textBoxCringle = new System.Windows.Forms.TextBox();
            this.textBoxComent = new System.Windows.Forms.TextBox();
            this.textBoxSiseM = new System.Windows.Forms.TextBox();
            this.textBoxSiseL = new System.Windows.Forms.TextBox();
            this.textBoxCopyes = new System.Windows.Forms.TextBox();
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelBasis = new System.Windows.Forms.Label();
            this.comboBoxBasis = new System.Windows.Forms.ComboBox();
            this.labelPadding = new System.Windows.Forms.Label();
            this.textBoxPadding = new System.Windows.Forms.TextBox();
            this.comboBoxТexture = new System.Windows.Forms.ComboBox();
            this.labelТexture = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxLam2 = new System.Windows.Forms.ComboBox();
            this.buttonDel = new System.Windows.Forms.Button();
            this.groupBoxRename = new System.Windows.Forms.GroupBox();
            this.radioButtonRenameEs = new System.Windows.Forms.RadioButton();
            this.radioButtonRenameNo = new System.Windows.Forms.RadioButton();
            this.radioButtonRenameOn = new System.Windows.Forms.RadioButton();
            this.progressBarCopyFile = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.labelCopyFileAll = new System.Windows.Forms.Label();
            this.labelCopyFileSaved = new System.Windows.Forms.Label();
            this.labelCopyFileSaving = new System.Windows.Forms.Label();
            this.buttonLinck = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxCringle.SuspendLayout();
            this.groupBoxStick.SuspendLayout();
            this.groupBoxRename.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxManager
            // 
            this.comboBoxManager.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxManager.Enabled = false;
            this.comboBoxManager.FormattingEnabled = true;
            this.comboBoxManager.Location = new System.Drawing.Point(391, 62);
            this.comboBoxManager.Name = "comboBoxManager";
            this.comboBoxManager.Size = new System.Drawing.Size(124, 21);
            this.comboBoxManager.TabIndex = 0;
            this.comboBoxManager.SelectedIndexChanged += new System.EventHandler(this.comboBoxManager_SelectedIndexChanged);
            this.comboBoxManager.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxManager_Validating);
            // 
            // labelManager
            // 
            this.labelManager.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelManager.AutoSize = true;
            this.labelManager.Location = new System.Drawing.Point(391, 45);
            this.labelManager.Name = "labelManager";
            this.labelManager.Size = new System.Drawing.Size(118, 13);
            this.labelManager.TabIndex = 1;
            this.labelManager.Text = "Выберите менеджера";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Выберите файлы";
            // 
            // listBoxFile
            // 
            this.listBoxFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxFile.FormattingEnabled = true;
            this.listBoxFile.Location = new System.Drawing.Point(12, 64);
            this.listBoxFile.MinimumSize = new System.Drawing.Size(365, 23);
            this.listBoxFile.Name = "listBoxFile";
            this.listBoxFile.Size = new System.Drawing.Size(365, 433);
            this.listBoxFile.TabIndex = 2;
            this.listBoxFile.SelectedIndexChanged += new System.EventHandler(this.listBoxFile_SelectedIndexChanged);
            this.listBoxFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxFile_KeyDown);
            // 
            // buttonFile
            // 
            this.buttonFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFile.Location = new System.Drawing.Point(12, 9);
            this.buttonFile.MinimumSize = new System.Drawing.Size(301, 49);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(306, 49);
            this.buttonFile.TabIndex = 3;
            this.buttonFile.Text = "Выбрать файлы";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // comboBoxStanok
            // 
            this.comboBoxStanok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxStanok.Enabled = false;
            this.comboBoxStanok.FormattingEnabled = true;
            this.comboBoxStanok.Location = new System.Drawing.Point(393, 103);
            this.comboBoxStanok.Name = "comboBoxStanok";
            this.comboBoxStanok.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStanok.TabIndex = 4;
            this.comboBoxStanok.SelectedIndexChanged += new System.EventHandler(this.comboBoxStanok_SelectedIndexChanged);
            // 
            // labelStanok
            // 
            this.labelStanok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStanok.AutoSize = true;
            this.labelStanok.Location = new System.Drawing.Point(390, 87);
            this.labelStanok.Name = "labelStanok";
            this.labelStanok.Size = new System.Drawing.Size(43, 13);
            this.labelStanok.TabIndex = 5;
            this.labelStanok.Text = "Станок";
            // 
            // labelMaterial
            // 
            this.labelMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMaterial.AutoSize = true;
            this.labelMaterial.Location = new System.Drawing.Point(390, 131);
            this.labelMaterial.Name = "labelMaterial";
            this.labelMaterial.Size = new System.Drawing.Size(57, 13);
            this.labelMaterial.TabIndex = 7;
            this.labelMaterial.Text = "Материал";
            // 
            // comboBoxMaterial
            // 
            this.comboBoxMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMaterial.Enabled = false;
            this.comboBoxMaterial.FormattingEnabled = true;
            this.comboBoxMaterial.Location = new System.Drawing.Point(393, 147);
            this.comboBoxMaterial.Name = "comboBoxMaterial";
            this.comboBoxMaterial.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMaterial.TabIndex = 6;
            this.comboBoxMaterial.SelectedIndexChanged += new System.EventHandler(this.comboBoxMaterial_SelectedIndexChanged);
            // 
            // labelStick
            // 
            this.labelStick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStick.AutoSize = true;
            this.labelStick.Location = new System.Drawing.Point(392, 299);
            this.labelStick.Name = "labelStick";
            this.labelStick.Size = new System.Drawing.Size(38, 13);
            this.labelStick.TabIndex = 11;
            this.labelStick.Text = "Склей";
            // 
            // labelCringle
            // 
            this.labelCringle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCringle.AutoSize = true;
            this.labelCringle.Location = new System.Drawing.Point(392, 342);
            this.labelCringle.Name = "labelCringle";
            this.labelCringle.Size = new System.Drawing.Size(55, 13);
            this.labelCringle.TabIndex = 13;
            this.labelCringle.Text = "Люверсы";
            // 
            // labelSiseL
            // 
            this.labelSiseL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSiseL.AutoSize = true;
            this.labelSiseL.Location = new System.Drawing.Point(545, 87);
            this.labelSiseL.Name = "labelSiseL";
            this.labelSiseL.Size = new System.Drawing.Size(98, 13);
            this.labelSiseL.TabIndex = 17;
            this.labelSiseL.Text = "Меньшая сторона";
            // 
            // labelSiseM
            // 
            this.labelSiseM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSiseM.AutoSize = true;
            this.labelSiseM.Location = new System.Drawing.Point(545, 46);
            this.labelSiseM.Name = "labelSiseM";
            this.labelSiseM.Size = new System.Drawing.Size(96, 13);
            this.labelSiseM.TabIndex = 15;
            this.labelSiseM.Text = "Большая сторона";
            // 
            // labelCharId
            // 
            this.labelCharId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCharId.AutoSize = true;
            this.labelCharId.Location = new System.Drawing.Point(545, 258);
            this.labelCharId.Name = "labelCharId";
            this.labelCharId.Size = new System.Drawing.Size(120, 13);
            this.labelCharId.TabIndex = 23;
            this.labelCharId.Text = "Инициалы менеджера";
            // 
            // comboBoxCharId
            // 
            this.comboBoxCharId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCharId.Enabled = false;
            this.comboBoxCharId.FormattingEnabled = true;
            this.comboBoxCharId.Location = new System.Drawing.Point(548, 274);
            this.comboBoxCharId.Name = "comboBoxCharId";
            this.comboBoxCharId.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCharId.TabIndex = 22;
            this.comboBoxCharId.SelectedIndexChanged += new System.EventHandler(this.comboBoxCharId_SelectedIndexChanged);
            this.comboBoxCharId.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxCharId_Validating);
            // 
            // labelLaminaciy
            // 
            this.labelLaminaciy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLaminaciy.AutoSize = true;
            this.labelLaminaciy.Location = new System.Drawing.Point(545, 176);
            this.labelLaminaciy.Name = "labelLaminaciy";
            this.labelLaminaciy.Size = new System.Drawing.Size(116, 13);
            this.labelLaminaciy.TabIndex = 21;
            this.labelLaminaciy.Text = "Материал ламинации";
            // 
            // comboBoxLaminaciy
            // 
            this.comboBoxLaminaciy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLaminaciy.Enabled = false;
            this.comboBoxLaminaciy.FormattingEnabled = true;
            this.comboBoxLaminaciy.Location = new System.Drawing.Point(548, 192);
            this.comboBoxLaminaciy.Name = "comboBoxLaminaciy";
            this.comboBoxLaminaciy.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLaminaciy.TabIndex = 20;
            this.comboBoxLaminaciy.SelectedIndexChanged += new System.EventHandler(this.comboBoxLaminaciy_SelectedIndexChanged);
            // 
            // labelCopyes
            // 
            this.labelCopyes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopyes.AutoSize = true;
            this.labelCopyes.Location = new System.Drawing.Point(545, 132);
            this.labelCopyes.Name = "labelCopyes";
            this.labelCopyes.Size = new System.Drawing.Size(99, 13);
            this.labelCopyes.TabIndex = 19;
            this.labelCopyes.Text = "Количество копий";
            // 
            // labelComent
            // 
            this.labelComent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelComent.AutoSize = true;
            this.labelComent.Location = new System.Drawing.Point(542, 342);
            this.labelComent.Name = "labelComent";
            this.labelComent.Size = new System.Drawing.Size(69, 13);
            this.labelComent.TabIndex = 27;
            this.labelComent.Text = "Коментарий";
            // 
            // labelNumber
            // 
            this.labelNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNumber.AutoSize = true;
            this.labelNumber.Location = new System.Drawing.Point(545, 299);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(80, 13);
            this.labelNumber.TabIndex = 25;
            this.labelNumber.Text = "Номер заказа";
            // 
            // groupBoxCringle
            // 
            this.groupBoxCringle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCringle.Controls.Add(this.label1);
            this.groupBoxCringle.Controls.Add(this.checkBoxCringleON4);
            this.groupBoxCringle.Controls.Add(this.checkBoxCringleOnL2);
            this.groupBoxCringle.Controls.Add(this.checkBoxCringleOnM2);
            this.groupBoxCringle.Controls.Add(this.checkBoxCringleOnL);
            this.groupBoxCringle.Controls.Add(this.checkBoxCringleOnM);
            this.groupBoxCringle.Location = new System.Drawing.Point(675, 46);
            this.groupBoxCringle.Name = "groupBoxCringle";
            this.groupBoxCringle.Size = new System.Drawing.Size(160, 196);
            this.groupBoxCringle.TabIndex = 36;
            this.groupBoxCringle.TabStop = false;
            this.groupBoxCringle.Text = "Люверсы";
            this.groupBoxCringle.MouseHover += new System.EventHandler(this.groupBoxCringle_MouseHover);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 48);
            this.label1.TabIndex = 5;
            this.label1.Text = "Аккуратно!                Не учитывает многих факторов! ";
            this.label1.MouseHover += new System.EventHandler(this.label1_MouseHover);
            // 
            // checkBoxCringleON4
            // 
            this.checkBoxCringleON4.AutoSize = true;
            this.checkBoxCringleON4.Enabled = false;
            this.checkBoxCringleON4.Location = new System.Drawing.Point(6, 164);
            this.checkBoxCringleON4.Name = "checkBoxCringleON4";
            this.checkBoxCringleON4.Size = new System.Drawing.Size(91, 17);
            this.checkBoxCringleON4.TabIndex = 4;
            this.checkBoxCringleON4.Text = "Все стороны";
            this.checkBoxCringleON4.UseVisualStyleBackColor = true;
            this.checkBoxCringleON4.CheckedChanged += new System.EventHandler(this.checkBoxCringleON4_CheckedChanged);
            this.checkBoxCringleON4.MouseHover += new System.EventHandler(this.groupBoxCringle_MouseHover);
            // 
            // checkBoxCringleOnL2
            // 
            this.checkBoxCringleOnL2.AutoSize = true;
            this.checkBoxCringleOnL2.Enabled = false;
            this.checkBoxCringleOnL2.Location = new System.Drawing.Point(6, 142);
            this.checkBoxCringleOnL2.Name = "checkBoxCringleOnL2";
            this.checkBoxCringleOnL2.Size = new System.Drawing.Size(140, 17);
            this.checkBoxCringleOnL2.TabIndex = 3;
            this.checkBoxCringleOnL2.Text = "Обе меньших стороны";
            this.checkBoxCringleOnL2.UseVisualStyleBackColor = true;
            this.checkBoxCringleOnL2.CheckedChanged += new System.EventHandler(this.checkBoxCringleOnL2_CheckedChanged);
            this.checkBoxCringleOnL2.MouseHover += new System.EventHandler(this.groupBoxCringle_MouseHover);
            // 
            // checkBoxCringleOnM2
            // 
            this.checkBoxCringleOnM2.AutoSize = true;
            this.checkBoxCringleOnM2.Enabled = false;
            this.checkBoxCringleOnM2.Location = new System.Drawing.Point(6, 123);
            this.checkBoxCringleOnM2.Name = "checkBoxCringleOnM2";
            this.checkBoxCringleOnM2.Size = new System.Drawing.Size(138, 17);
            this.checkBoxCringleOnM2.TabIndex = 2;
            this.checkBoxCringleOnM2.Text = "Обе больших стороны";
            this.checkBoxCringleOnM2.UseVisualStyleBackColor = true;
            this.checkBoxCringleOnM2.CheckedChanged += new System.EventHandler(this.checkBoxCringleOnM2_CheckedChanged);
            this.checkBoxCringleOnM2.MouseHover += new System.EventHandler(this.groupBoxCringle_MouseHover);
            // 
            // checkBoxCringleOnL
            // 
            this.checkBoxCringleOnL.AutoSize = true;
            this.checkBoxCringleOnL.Enabled = false;
            this.checkBoxCringleOnL.Location = new System.Drawing.Point(6, 103);
            this.checkBoxCringleOnL.Name = "checkBoxCringleOnL";
            this.checkBoxCringleOnL.Size = new System.Drawing.Size(117, 17);
            this.checkBoxCringleOnL.TabIndex = 1;
            this.checkBoxCringleOnL.Text = "Меньшая сторона";
            this.checkBoxCringleOnL.UseVisualStyleBackColor = true;
            this.checkBoxCringleOnL.CheckedChanged += new System.EventHandler(this.checkBoxCringleOnL_CheckedChanged);
            this.checkBoxCringleOnL.MouseHover += new System.EventHandler(this.groupBoxCringle_MouseHover);
            // 
            // checkBoxCringleOnM
            // 
            this.checkBoxCringleOnM.AutoSize = true;
            this.checkBoxCringleOnM.Enabled = false;
            this.checkBoxCringleOnM.Location = new System.Drawing.Point(6, 81);
            this.checkBoxCringleOnM.Name = "checkBoxCringleOnM";
            this.checkBoxCringleOnM.Size = new System.Drawing.Size(115, 17);
            this.checkBoxCringleOnM.TabIndex = 0;
            this.checkBoxCringleOnM.Text = "Большая сторона";
            this.checkBoxCringleOnM.UseVisualStyleBackColor = true;
            this.checkBoxCringleOnM.CheckedChanged += new System.EventHandler(this.checkBoxCringleOnM_CheckedChanged);
            this.checkBoxCringleOnM.MouseHover += new System.EventHandler(this.groupBoxCringle_MouseHover);
            // 
            // groupBoxStick
            // 
            this.groupBoxStick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxStick.Controls.Add(this.checkBoxStick4);
            this.groupBoxStick.Controls.Add(this.checkStickL);
            this.groupBoxStick.Controls.Add(this.checkStikM);
            this.groupBoxStick.Location = new System.Drawing.Point(675, 248);
            this.groupBoxStick.Name = "groupBoxStick";
            this.groupBoxStick.Size = new System.Drawing.Size(160, 91);
            this.groupBoxStick.TabIndex = 37;
            this.groupBoxStick.TabStop = false;
            this.groupBoxStick.Text = "Склей";
            // 
            // checkBoxStick4
            // 
            this.checkBoxStick4.AutoSize = true;
            this.checkBoxStick4.Enabled = false;
            this.checkBoxStick4.Location = new System.Drawing.Point(7, 66);
            this.checkBoxStick4.Name = "checkBoxStick4";
            this.checkBoxStick4.Size = new System.Drawing.Size(97, 17);
            this.checkBoxStick4.TabIndex = 2;
            this.checkBoxStick4.Text = "По пириметру";
            this.checkBoxStick4.UseVisualStyleBackColor = true;
            this.checkBoxStick4.CheckedChanged += new System.EventHandler(this.checkBoxStick4_CheckedChanged);
            // 
            // checkStickL
            // 
            this.checkStickL.AutoSize = true;
            this.checkStickL.Enabled = false;
            this.checkStickL.Location = new System.Drawing.Point(7, 42);
            this.checkStickL.Name = "checkStickL";
            this.checkStickL.Size = new System.Drawing.Size(133, 17);
            this.checkStickL.TabIndex = 1;
            this.checkStickL.Text = "По меньшей стороне";
            this.checkStickL.UseVisualStyleBackColor = true;
            this.checkStickL.CheckedChanged += new System.EventHandler(this.checkStickL_CheckedChanged);
            // 
            // checkStikM
            // 
            this.checkStikM.AutoSize = true;
            this.checkStikM.Enabled = false;
            this.checkStikM.Location = new System.Drawing.Point(7, 20);
            this.checkStikM.Name = "checkStikM";
            this.checkStikM.Size = new System.Drawing.Size(131, 17);
            this.checkStikM.TabIndex = 0;
            this.checkStikM.Text = "По большей стороне";
            this.checkStikM.UseVisualStyleBackColor = true;
            this.checkStikM.CheckedChanged += new System.EventHandler(this.checkStikM_CheckedChanged);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileName.Enabled = false;
            this.textBoxFileName.Location = new System.Drawing.Point(472, 13);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ReadOnly = true;
            this.textBoxFileName.Size = new System.Drawing.Size(363, 20);
            this.textBoxFileName.TabIndex = 28;
            this.textBoxFileName.MouseHover += new System.EventHandler(this.textBoxFileName_MouseHover);
            // 
            // textBoxStick
            // 
            this.textBoxStick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStick.Enabled = false;
            this.textBoxStick.Location = new System.Drawing.Point(395, 319);
            this.textBoxStick.Name = "textBoxStick";
            this.textBoxStick.Size = new System.Drawing.Size(121, 20);
            this.textBoxStick.TabIndex = 39;
            this.textBoxStick.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxStick_Validating);
            // 
            // textBoxCringle
            // 
            this.textBoxCringle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCringle.Enabled = false;
            this.textBoxCringle.Location = new System.Drawing.Point(393, 358);
            this.textBoxCringle.Name = "textBoxCringle";
            this.textBoxCringle.Size = new System.Drawing.Size(121, 20);
            this.textBoxCringle.TabIndex = 40;
            this.textBoxCringle.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxCringle_Validating);
            // 
            // textBoxComent
            // 
            this.textBoxComent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComent.Enabled = false;
            this.textBoxComent.Location = new System.Drawing.Point(545, 358);
            this.textBoxComent.Name = "textBoxComent";
            this.textBoxComent.Size = new System.Drawing.Size(290, 20);
            this.textBoxComent.TabIndex = 41;
            this.textBoxComent.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxComent_Validating);
            // 
            // textBoxSiseM
            // 
            this.textBoxSiseM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSiseM.Enabled = false;
            this.textBoxSiseM.Location = new System.Drawing.Point(548, 62);
            this.textBoxSiseM.Name = "textBoxSiseM";
            this.textBoxSiseM.Size = new System.Drawing.Size(121, 20);
            this.textBoxSiseM.TabIndex = 42;
            this.textBoxSiseM.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSiseM_Validating);
            // 
            // textBoxSiseL
            // 
            this.textBoxSiseL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSiseL.Enabled = false;
            this.textBoxSiseL.Location = new System.Drawing.Point(548, 101);
            this.textBoxSiseL.Name = "textBoxSiseL";
            this.textBoxSiseL.Size = new System.Drawing.Size(121, 20);
            this.textBoxSiseL.TabIndex = 43;
            this.textBoxSiseL.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSiseL_Validating);
            // 
            // textBoxCopyes
            // 
            this.textBoxCopyes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCopyes.Enabled = false;
            this.textBoxCopyes.Location = new System.Drawing.Point(548, 147);
            this.textBoxCopyes.Name = "textBoxCopyes";
            this.textBoxCopyes.Size = new System.Drawing.Size(121, 20);
            this.textBoxCopyes.TabIndex = 44;
            this.textBoxCopyes.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxCopyes_Validating);
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNumber.Enabled = false;
            this.textBoxNumber.Location = new System.Drawing.Point(548, 315);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(121, 20);
            this.textBoxNumber.TabIndex = 45;
            this.textBoxNumber.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxNumber_Validating);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(395, 430);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(440, 66);
            this.button1.TabIndex = 46;
            this.button1.Text = "Отправить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SentFiles_Click);
            // 
            // labelBasis
            // 
            this.labelBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBasis.AutoSize = true;
            this.labelBasis.Location = new System.Drawing.Point(392, 176);
            this.labelBasis.Name = "labelBasis";
            this.labelBasis.Size = new System.Drawing.Size(63, 13);
            this.labelBasis.TabIndex = 48;
            this.labelBasis.Text = "Основание";
            // 
            // comboBoxBasis
            // 
            this.comboBoxBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBasis.Enabled = false;
            this.comboBoxBasis.FormattingEnabled = true;
            this.comboBoxBasis.Location = new System.Drawing.Point(395, 192);
            this.comboBoxBasis.Name = "comboBoxBasis";
            this.comboBoxBasis.Size = new System.Drawing.Size(121, 21);
            this.comboBoxBasis.TabIndex = 47;
            this.comboBoxBasis.SelectedIndexChanged += new System.EventHandler(this.comboBoxBasis_SelectedIndexChanged);
            // 
            // labelPadding
            // 
            this.labelPadding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPadding.AutoSize = true;
            this.labelPadding.Location = new System.Drawing.Point(392, 257);
            this.labelPadding.Name = "labelPadding";
            this.labelPadding.Size = new System.Drawing.Size(33, 13);
            this.labelPadding.TabIndex = 9;
            this.labelPadding.Text = "Поля";
            // 
            // textBoxPadding
            // 
            this.textBoxPadding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPadding.Enabled = false;
            this.textBoxPadding.Location = new System.Drawing.Point(395, 274);
            this.textBoxPadding.Name = "textBoxPadding";
            this.textBoxPadding.Size = new System.Drawing.Size(121, 20);
            this.textBoxPadding.TabIndex = 38;
            this.textBoxPadding.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxPadding_Validating);
            // 
            // comboBoxТexture
            // 
            this.comboBoxТexture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxТexture.Enabled = false;
            this.comboBoxТexture.FormattingEnabled = true;
            this.comboBoxТexture.Location = new System.Drawing.Point(394, 232);
            this.comboBoxТexture.Name = "comboBoxТexture";
            this.comboBoxТexture.Size = new System.Drawing.Size(121, 21);
            this.comboBoxТexture.TabIndex = 49;
            this.comboBoxТexture.SelectedIndexChanged += new System.EventHandler(this.comboBoxТexture_SelectedIndexChanged);
            // 
            // labelТexture
            // 
            this.labelТexture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelТexture.AutoSize = true;
            this.labelТexture.Location = new System.Drawing.Point(391, 216);
            this.labelТexture.Name = "labelТexture";
            this.labelТexture.Size = new System.Drawing.Size(54, 13);
            this.labelТexture.TabIndex = 50;
            this.labelТexture.Text = "Текстура";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(545, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 52;
            this.label2.Text = "Текстура ламинации";
            // 
            // comboBoxLam2
            // 
            this.comboBoxLam2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLam2.Enabled = false;
            this.comboBoxLam2.FormattingEnabled = true;
            this.comboBoxLam2.Location = new System.Drawing.Point(548, 232);
            this.comboBoxLam2.Name = "comboBoxLam2";
            this.comboBoxLam2.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLam2.TabIndex = 51;
            this.comboBoxLam2.SelectedIndexChanged += new System.EventHandler(this.comboBoxLam2_SelectedIndexChanged);
            // 
            // buttonDel
            // 
            this.buttonDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDel.Location = new System.Drawing.Point(320, 9);
            this.buttonDel.MaximumSize = new System.Drawing.Size(57, 49);
            this.buttonDel.MinimumSize = new System.Drawing.Size(57, 49);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(57, 49);
            this.buttonDel.TabIndex = 53;
            this.buttonDel.Text = "Убрать";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            this.buttonDel.MouseHover += new System.EventHandler(this.buttonDel_MouseHover);
            // 
            // groupBoxRename
            // 
            this.groupBoxRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRename.Controls.Add(this.radioButtonRenameEs);
            this.groupBoxRename.Controls.Add(this.radioButtonRenameNo);
            this.groupBoxRename.Controls.Add(this.radioButtonRenameOn);
            this.groupBoxRename.Location = new System.Drawing.Point(395, 384);
            this.groupBoxRename.Name = "groupBoxRename";
            this.groupBoxRename.Size = new System.Drawing.Size(440, 40);
            this.groupBoxRename.TabIndex = 54;
            this.groupBoxRename.TabStop = false;
            // 
            // radioButtonRenameEs
            // 
            this.radioButtonRenameEs.AutoSize = true;
            this.radioButtonRenameEs.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonRenameEs.Location = new System.Drawing.Point(327, 4);
            this.radioButtonRenameEs.Name = "radioButtonRenameEs";
            this.radioButtonRenameEs.Size = new System.Drawing.Size(107, 30);
            this.radioButtonRenameEs.TabIndex = 2;
            this.radioButtonRenameEs.Text = "Не оставлять себе";
            this.radioButtonRenameEs.UseVisualStyleBackColor = true;
            this.radioButtonRenameEs.CheckedChanged += new System.EventHandler(this.radioButtonRenameEs_CheckedChanged);
            this.radioButtonRenameEs.MouseHover += new System.EventHandler(this.radioButtonRenameEs_MouseHover);
            // 
            // radioButtonRenameNo
            // 
            this.radioButtonRenameNo.AutoSize = true;
            this.radioButtonRenameNo.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonRenameNo.Location = new System.Drawing.Point(143, 4);
            this.radioButtonRenameNo.Name = "radioButtonRenameNo";
            this.radioButtonRenameNo.Size = new System.Drawing.Size(147, 30);
            this.radioButtonRenameNo.TabIndex = 1;
            this.radioButtonRenameNo.Text = "Сохранить себе как копию";
            this.radioButtonRenameNo.UseVisualStyleBackColor = true;
            this.radioButtonRenameNo.CheckedChanged += new System.EventHandler(this.radioButtonRenameNo_CheckedChanged);
            this.radioButtonRenameNo.MouseHover += new System.EventHandler(this.radioButtonRenameNo_MouseHover);
            // 
            // radioButtonRenameOn
            // 
            this.radioButtonRenameOn.AutoSize = true;
            this.radioButtonRenameOn.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonRenameOn.Checked = true;
            this.radioButtonRenameOn.Enabled = false;
            this.radioButtonRenameOn.Location = new System.Drawing.Point(0, 4);
            this.radioButtonRenameOn.Name = "radioButtonRenameOn";
            this.radioButtonRenameOn.Size = new System.Drawing.Size(127, 30);
            this.radioButtonRenameOn.TabIndex = 0;
            this.radioButtonRenameOn.TabStop = true;
            this.radioButtonRenameOn.Text = "Переименовать у себя";
            this.radioButtonRenameOn.UseVisualStyleBackColor = true;
            this.radioButtonRenameOn.CheckedChanged += new System.EventHandler(this.radioButtonRenameOn_CheckedChanged);
            this.radioButtonRenameOn.MouseHover += new System.EventHandler(this.radioButtonRenameOn_MouseHover);
            // 
            // progressBarCopyFile
            // 
            this.progressBarCopyFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarCopyFile.ForeColor = System.Drawing.Color.Lime;
            this.progressBarCopyFile.Location = new System.Drawing.Point(395, 431);
            this.progressBarCopyFile.Maximum = 0;
            this.progressBarCopyFile.Name = "progressBarCopyFile";
            this.progressBarCopyFile.Size = new System.Drawing.Size(440, 45);
            this.progressBarCopyFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCopyFile.TabIndex = 55;
            this.progressBarCopyFile.UseWaitCursor = true;
            this.progressBarCopyFile.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // labelCopyFileAll
            // 
            this.labelCopyFileAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopyFileAll.AutoSize = true;
            this.labelCopyFileAll.Location = new System.Drawing.Point(392, 486);
            this.labelCopyFileAll.Name = "labelCopyFileAll";
            this.labelCopyFileAll.Size = new System.Drawing.Size(80, 13);
            this.labelCopyFileAll.TabIndex = 56;
            this.labelCopyFileAll.Text = "labelCopyFileAll";
            this.labelCopyFileAll.Visible = false;
            // 
            // labelCopyFileSaved
            // 
            this.labelCopyFileSaved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopyFileSaved.AutoSize = true;
            this.labelCopyFileSaved.Location = new System.Drawing.Point(545, 486);
            this.labelCopyFileSaved.Name = "labelCopyFileSaved";
            this.labelCopyFileSaved.Size = new System.Drawing.Size(100, 13);
            this.labelCopyFileSaved.TabIndex = 57;
            this.labelCopyFileSaved.Tag = "0";
            this.labelCopyFileSaved.Text = "labelCopyFileSaved";
            this.labelCopyFileSaved.Visible = false;
            // 
            // labelCopyFileSaving
            // 
            this.labelCopyFileSaving.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopyFileSaving.AutoSize = true;
            this.labelCopyFileSaving.Location = new System.Drawing.Point(722, 486);
            this.labelCopyFileSaving.Name = "labelCopyFileSaving";
            this.labelCopyFileSaving.Size = new System.Drawing.Size(102, 13);
            this.labelCopyFileSaving.TabIndex = 58;
            this.labelCopyFileSaving.Text = "labelCopyFileSaving";
            this.labelCopyFileSaving.Visible = false;
            // 
            // buttonLinck
            // 
            this.buttonLinck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLinck.Enabled = false;
            this.buttonLinck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLinck.Location = new System.Drawing.Point(395, 431);
            this.buttonLinck.Name = "buttonLinck";
            this.buttonLinck.Size = new System.Drawing.Size(440, 65);
            this.buttonLinck.TabIndex = 59;
            this.buttonLinck.Text = "Открыть сайт";
            this.buttonLinck.UseVisualStyleBackColor = true;
            this.buttonLinck.Visible = false;
            this.buttonLinck.Click += new System.EventHandler(this.buttonLinck_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(391, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 60;
            this.button2.Text = "Для всех";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseHover += new System.EventHandler(this.button2_MouseMove);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // FormПолноцвет
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 508);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonLinck);
            this.Controls.Add(this.labelCopyFileSaving);
            this.Controls.Add(this.labelCopyFileSaved);
            this.Controls.Add(this.labelCopyFileAll);
            this.Controls.Add(this.progressBarCopyFile);
            this.Controls.Add(this.groupBoxRename);
            this.Controls.Add(this.buttonDel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxLam2);
            this.Controls.Add(this.labelТexture);
            this.Controls.Add(this.comboBoxТexture);
            this.Controls.Add(this.labelBasis);
            this.Controls.Add(this.comboBoxBasis);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxNumber);
            this.Controls.Add(this.textBoxCopyes);
            this.Controls.Add(this.textBoxSiseL);
            this.Controls.Add(this.textBoxSiseM);
            this.Controls.Add(this.textBoxComent);
            this.Controls.Add(this.textBoxCringle);
            this.Controls.Add(this.textBoxStick);
            this.Controls.Add(this.textBoxPadding);
            this.Controls.Add(this.groupBoxStick);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.groupBoxCringle);
            this.Controls.Add(this.labelComent);
            this.Controls.Add(this.labelNumber);
            this.Controls.Add(this.labelCharId);
            this.Controls.Add(this.comboBoxCharId);
            this.Controls.Add(this.labelLaminaciy);
            this.Controls.Add(this.comboBoxLaminaciy);
            this.Controls.Add(this.labelCopyes);
            this.Controls.Add(this.labelSiseL);
            this.Controls.Add(this.labelSiseM);
            this.Controls.Add(this.labelCringle);
            this.Controls.Add(this.labelStick);
            this.Controls.Add(this.labelPadding);
            this.Controls.Add(this.labelMaterial);
            this.Controls.Add(this.comboBoxMaterial);
            this.Controls.Add(this.labelStanok);
            this.Controls.Add(this.comboBoxStanok);
            this.Controls.Add(this.buttonFile);
            this.Controls.Add(this.listBoxFile);
            this.Controls.Add(this.labelManager);
            this.Controls.Add(this.comboBoxManager);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(863, 397);
            this.Name = "FormПолноцвет";
            this.Text = "Файлы на печать в полнцвет";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormПолноцвет_FormClosing);
            this.Load += new System.EventHandler(this.FormПолнцвет_Load);
            this.groupBoxCringle.ResumeLayout(false);
            this.groupBoxCringle.PerformLayout();
            this.groupBoxStick.ResumeLayout(false);
            this.groupBoxStick.PerformLayout();
            this.groupBoxRename.ResumeLayout(false);
            this.groupBoxRename.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.ComboBox comboBoxManager;
        private System.Windows.Forms.Label labelManager;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox listBoxFile;
        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.ComboBox comboBoxStanok;
        private System.Windows.Forms.Label labelStanok;
        private System.Windows.Forms.Label labelMaterial;
        private System.Windows.Forms.ComboBox comboBoxMaterial;
        private System.Windows.Forms.Label labelStick;
        private System.Windows.Forms.Label labelCringle;
        private System.Windows.Forms.Label labelSiseL;
        private System.Windows.Forms.Label labelSiseM;
        private System.Windows.Forms.Label labelCharId;
        private System.Windows.Forms.ComboBox comboBoxCharId;
        private System.Windows.Forms.Label labelLaminaciy;
        private System.Windows.Forms.ComboBox comboBoxLaminaciy;
        private System.Windows.Forms.Label labelCopyes;
        private System.Windows.Forms.Label labelComent;
        private System.Windows.Forms.Label labelNumber;

        private System.Windows.Forms.GroupBox groupBoxCringle;
        private System.Windows.Forms.CheckBox checkBoxCringleON4;
        private System.Windows.Forms.CheckBox checkBoxCringleOnL2;
        private System.Windows.Forms.CheckBox checkBoxCringleOnM2;
        private System.Windows.Forms.CheckBox checkBoxCringleOnL;
        private System.Windows.Forms.CheckBox checkBoxCringleOnM;
        private System.Windows.Forms.GroupBox groupBoxStick;
        private System.Windows.Forms.CheckBox checkBoxStick4;
        private System.Windows.Forms.CheckBox checkStickL;
        private System.Windows.Forms.CheckBox checkStikM;

        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.TextBox textBoxStick;
        private System.Windows.Forms.TextBox textBoxCringle;
        private System.Windows.Forms.TextBox textBoxComent;
        private System.Windows.Forms.TextBox textBoxSiseM;
        private System.Windows.Forms.TextBox textBoxSiseL;
        private System.Windows.Forms.TextBox textBoxCopyes;
        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelBasis;
        private System.Windows.Forms.ComboBox comboBoxBasis;
        private System.Windows.Forms.Label labelPadding;
        private System.Windows.Forms.TextBox textBoxPadding;
        private System.Windows.Forms.ComboBox comboBoxТexture;
        private System.Windows.Forms.Label labelТexture;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxLam2;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.GroupBox groupBoxRename;
        private System.Windows.Forms.RadioButton radioButtonRenameEs;
        private System.Windows.Forms.RadioButton radioButtonRenameNo;
        private System.Windows.Forms.RadioButton radioButtonRenameOn;
        private System.Windows.Forms.ProgressBar progressBarCopyFile;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelCopyFileAll;
        private System.Windows.Forms.Label labelCopyFileSaved;
        private System.Windows.Forms.Label labelCopyFileSaving;
        private System.Windows.Forms.Button buttonLinck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}