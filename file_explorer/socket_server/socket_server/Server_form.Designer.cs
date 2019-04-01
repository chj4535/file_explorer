namespace socket_server
{
    partial class Server_form
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
            this.label1 = new System.Windows.Forms.Label();
            this.port_textbox = new System.Windows.Forms.TextBox();
            this.Start_server = new System.Windows.Forms.Button();
            this.server_log_richtextbox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F);
            this.label1.Location = new System.Drawing.Point(34, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "포트번호";
            // 
            // port_textbox
            // 
            this.port_textbox.Location = new System.Drawing.Point(112, 17);
            this.port_textbox.Name = "port_textbox";
            this.port_textbox.Size = new System.Drawing.Size(100, 21);
            this.port_textbox.TabIndex = 1;
            // 
            // Start_server
            // 
            this.Start_server.Location = new System.Drawing.Point(233, 15);
            this.Start_server.Name = "Start_server";
            this.Start_server.Size = new System.Drawing.Size(175, 26);
            this.Start_server.TabIndex = 2;
            this.Start_server.Text = "시작";
            this.Start_server.UseVisualStyleBackColor = true;
            this.Start_server.Click += new System.EventHandler(this.Start_server_Click);
            // 
            // server_log_richtextbox
            // 
            this.server_log_richtextbox.Location = new System.Drawing.Point(37, 44);
            this.server_log_richtextbox.Name = "server_log_richtextbox";
            this.server_log_richtextbox.Size = new System.Drawing.Size(758, 386);
            this.server_log_richtextbox.TabIndex = 3;
            this.server_log_richtextbox.Text = "";
            // 
            // Server_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 455);
            this.Controls.Add(this.server_log_richtextbox);
            this.Controls.Add(this.Start_server);
            this.Controls.Add(this.port_textbox);
            this.Controls.Add(this.label1);
            this.Name = "Server_form";
            this.Text = "Server_form";
            this.Load += new System.EventHandler(this.Server_form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox port_textbox;
        private System.Windows.Forms.Button Start_server;
        private System.Windows.Forms.RichTextBox server_log_richtextbox;
    }
}