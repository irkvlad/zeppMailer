namespace zeppMailer
{
    partial class smsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(smsForm));
            this.labelSms = new System.Windows.Forms.Label();
            this.buttonSmsOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelSms
            // 
            this.labelSms.AutoEllipsis = true;
            this.labelSms.CausesValidation = false;
            this.labelSms.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSms.Location = new System.Drawing.Point(12, 20);
            this.labelSms.Name = "labelSms";
            this.labelSms.Size = new System.Drawing.Size(331, 62);
            this.labelSms.TabIndex = 0;
            this.labelSms.Text = "Новых сообщений нет";
            this.labelSms.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSmsOK
            // 
            this.buttonSmsOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSmsOK.Location = new System.Drawing.Point(135, 85);
            this.buttonSmsOK.Name = "buttonSmsOK";
            this.buttonSmsOK.Size = new System.Drawing.Size(75, 23);
            this.buttonSmsOK.TabIndex = 1;
            this.buttonSmsOK.Text = "Закрыть";
            this.buttonSmsOK.UseVisualStyleBackColor = true;
            this.buttonSmsOK.Click += new System.EventHandler(this.buttonSmsOK_Click);
            // 
            // smsForm
            // 
            this.AcceptButton = this.buttonSmsOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(355, 132);
            this.Controls.Add(this.buttonSmsOK);
            this.Controls.Add(this.labelSms);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(363, 159);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(363, 159);
            this.Name = "smsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "   Новые сообщения";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSms;
        private System.Windows.Forms.Button buttonSmsOK;
    }
}