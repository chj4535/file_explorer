namespace file_explorer
{
    partial class Login_form
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
            this.user_id_textbox = new System.Windows.Forms.TextBox();
            this.user_pw_textbox = new System.Windows.Forms.TextBox();
            this.user_id_label = new System.Windows.Forms.Label();
            this.user_pw_label = new System.Windows.Forms.Label();
            this.check_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // user_id_textbox
            // 
            this.user_id_textbox.Font = new System.Drawing.Font("굴림", 12F);
            this.user_id_textbox.Location = new System.Drawing.Point(134, 36);
            this.user_id_textbox.Name = "user_id_textbox";
            this.user_id_textbox.Size = new System.Drawing.Size(268, 26);
            this.user_id_textbox.TabIndex = 0;
            // 
            // user_pw_textbox
            // 
            this.user_pw_textbox.Font = new System.Drawing.Font("굴림", 12F);
            this.user_pw_textbox.Location = new System.Drawing.Point(134, 84);
            this.user_pw_textbox.Name = "user_pw_textbox";
            this.user_pw_textbox.Size = new System.Drawing.Size(268, 26);
            this.user_pw_textbox.TabIndex = 1;
            // 
            // user_id_label
            // 
            this.user_id_label.AutoSize = true;
            this.user_id_label.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.user_id_label.Location = new System.Drawing.Point(62, 39);
            this.user_id_label.Name = "user_id_label";
            this.user_id_label.Size = new System.Drawing.Size(66, 16);
            this.user_id_label.TabIndex = 3;
            this.user_id_label.Text = "아이디 :";
            // 
            // user_pw_label
            // 
            this.user_pw_label.AutoSize = true;
            this.user_pw_label.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.user_pw_label.Location = new System.Drawing.Point(46, 87);
            this.user_pw_label.Name = "user_pw_label";
            this.user_pw_label.Size = new System.Drawing.Size(82, 16);
            this.user_pw_label.TabIndex = 4;
            this.user_pw_label.Text = "비밀번호 :";
            // 
            // check_button
            // 
            this.check_button.Font = new System.Drawing.Font("굴림", 12F);
            this.check_button.Location = new System.Drawing.Point(192, 134);
            this.check_button.Name = "check_button";
            this.check_button.Size = new System.Drawing.Size(102, 31);
            this.check_button.TabIndex = 5;
            this.check_button.Text = "확인";
            this.check_button.UseVisualStyleBackColor = true;
            this.check_button.Click += new System.EventHandler(this.check_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Font = new System.Drawing.Font("굴림", 12F);
            this.cancel_button.Location = new System.Drawing.Point(300, 134);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(102, 31);
            this.cancel_button.TabIndex = 6;
            this.cancel_button.Text = "취소";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // Login_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 195);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.check_button);
            this.Controls.Add(this.user_pw_label);
            this.Controls.Add(this.user_id_label);
            this.Controls.Add(this.user_pw_textbox);
            this.Controls.Add(this.user_id_textbox);
            this.Name = "Login_form";
            this.Text = "login_form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox user_id_textbox;
        private System.Windows.Forms.TextBox user_pw_textbox;
        private System.Windows.Forms.Label user_id_label;
        private System.Windows.Forms.Label user_pw_label;
        private System.Windows.Forms.Button check_button;
        private System.Windows.Forms.Button cancel_button;
    }
}