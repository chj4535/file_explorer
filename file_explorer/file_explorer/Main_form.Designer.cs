﻿using System.Drawing;
using System.Windows.Forms;

namespace file_explorer
{
    partial class Main_form
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.nextButton = new System.Windows.Forms.Button();
            this.upperButton = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.user_name_label = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mainFormtreeview = new System.Windows.Forms.TreeView();
            this.mainFormlistview = new System.Windows.Forms.ListView();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.mainFormcombobox = new System.Windows.Forms.ComboBox();
            this.mainFormrecentcombobox = new System.Windows.Forms.ComboBox();
            this.mainFormimagelist = new System.Windows.Forms.ImageList(this.components);
            this.backButton = new System.Windows.Forms.Button();
            this.mainFormpathbutton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.SystemColors.Window;
            this.nextButton.CausesValidation = false;
            this.nextButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nextButton.FlatAppearance.BorderSize = 0;
            this.nextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextButton.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nextButton.ForeColor = System.Drawing.Color.Black;
            this.nextButton.Location = new System.Drawing.Point(27, 44);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(23, 23);
            this.nextButton.TabIndex = 2;
            this.nextButton.TabStop = false;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // upperButton
            // 
            this.upperButton.BackColor = System.Drawing.SystemColors.Window;
            this.upperButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.upperButton.FlatAppearance.BorderSize = 0;
            this.upperButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upperButton.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.upperButton.Location = new System.Drawing.Point(68, 44);
            this.upperButton.Name = "upperButton";
            this.upperButton.Size = new System.Drawing.Size(23, 23);
            this.upperButton.TabIndex = 4;
            this.upperButton.Text = "∧";
            this.upperButton.UseVisualStyleBackColor = true;
            this.upperButton.Click += new System.EventHandler(this.upperButton_Click);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("굴림", 10F);
            this.textBox2.Location = new System.Drawing.Point(678, 45);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(190, 23);
            this.textBox2.TabIndex = 6;
            // 
            // user_name_label
            // 
            this.user_name_label.AutoSize = true;
            this.user_name_label.Font = new System.Drawing.Font("굴림", 12F);
            this.user_name_label.Location = new System.Drawing.Point(655, 10);
            this.user_name_label.Name = "user_name_label";
            this.user_name_label.Size = new System.Drawing.Size(0, 16);
            this.user_name_label.TabIndex = 8;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(4, 70);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mainFormtreeview);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mainFormlistview);
            this.splitContainer1.Size = new System.Drawing.Size(864, 368);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 9;
            // 
            // mainFormtreeview
            // 
            this.mainFormtreeview.BackColor = System.Drawing.SystemColors.Window;
            this.mainFormtreeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainFormtreeview.ForeColor = System.Drawing.SystemColors.WindowText;
            this.mainFormtreeview.Location = new System.Drawing.Point(0, 0);
            this.mainFormtreeview.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.mainFormtreeview.Name = "mainFormtreeview";
            this.mainFormtreeview.Size = new System.Drawing.Size(145, 368);
            this.mainFormtreeview.TabIndex = 0;
            this.mainFormtreeview.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.mainFormtreeview_AfterCollapse);
            this.mainFormtreeview.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.mainFormtreeview_AfterExpand);
            this.mainFormtreeview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mainFormtreeview_AfterSelect);
            // 
            // mainFormlistview
            // 
            this.mainFormlistview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainFormlistview.FullRowSelect = true;
            this.mainFormlistview.Location = new System.Drawing.Point(0, 0);
            this.mainFormlistview.Name = "mainFormlistview";
            this.mainFormlistview.Size = new System.Drawing.Size(716, 368);
            this.mainFormlistview.TabIndex = 0;
            this.mainFormlistview.UseCompatibleStateImageBehavior = false;
            this.mainFormlistview.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.mainFormlistview_ColumnClick);
            this.mainFormlistview.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.mainFormlistview_ItemDrag);
            this.mainFormlistview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewDoubleClick);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(4, 439);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(864, 21);
            this.textBox3.TabIndex = 10;
            // 
            // mainFormcombobox
            // 
            this.mainFormcombobox.BackColor = System.Drawing.SystemColors.Window;
            this.mainFormcombobox.CausesValidation = false;
            this.mainFormcombobox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mainFormcombobox.Font = new System.Drawing.Font("굴림", 11F);
            this.mainFormcombobox.Location = new System.Drawing.Point(95, 45);
            this.mainFormcombobox.Margin = new System.Windows.Forms.Padding(0);
            this.mainFormcombobox.Name = "mainFormcombobox";
            this.mainFormcombobox.Size = new System.Drawing.Size(546, 23);
            this.mainFormcombobox.TabIndex = 12;
            this.mainFormcombobox.TabStop = false;
            this.mainFormcombobox.DropDown += new System.EventHandler(this.mainFormcombobox_DropDown);
            this.mainFormcombobox.SelectedIndexChanged += new System.EventHandler(this.mainFormcombobox_SelectedIndexChanged);
            this.mainFormcombobox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mainFormcombobox_KeyPress);
            this.mainFormcombobox.Leave += new System.EventHandler(this.mainFormcombobox_Leave);
            // 
            // mainFormrecentcombobox
            // 
            this.mainFormrecentcombobox.BackColor = System.Drawing.SystemColors.Window;
            this.mainFormrecentcombobox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mainFormrecentcombobox.Font = new System.Drawing.Font("굴림", 11F);
            this.mainFormrecentcombobox.ForeColor = System.Drawing.Color.Black;
            this.mainFormrecentcombobox.FormattingEnabled = true;
            this.mainFormrecentcombobox.Location = new System.Drawing.Point(4, 44);
            this.mainFormrecentcombobox.MaxDropDownItems = 10;
            this.mainFormrecentcombobox.Name = "mainFormrecentcombobox";
            this.mainFormrecentcombobox.Size = new System.Drawing.Size(66, 23);
            this.mainFormrecentcombobox.TabIndex = 13;
            this.mainFormrecentcombobox.SelectedIndexChanged += new System.EventHandler(this.mainFormrecentcombobox_SelectedIndexChanged);
            // 
            // mainFormimagelist
            // 
            this.mainFormimagelist.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.mainFormimagelist.ImageSize = new System.Drawing.Size(16, 16);
            this.mainFormimagelist.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.backButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.backButton.FlatAppearance.BorderSize = 0;
            this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backButton.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold);
            this.backButton.ForeColor = System.Drawing.Color.Gray;
            this.backButton.Location = new System.Drawing.Point(5, 44);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(23, 23);
            this.backButton.TabIndex = 14;
            this.backButton.TabStop = false;
            this.backButton.Text = "<";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // mainFormpathbutton
            // 
            this.mainFormpathbutton.BackColor = System.Drawing.SystemColors.Window;
            this.mainFormpathbutton.FlatAppearance.BorderSize = 0;
            this.mainFormpathbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mainFormpathbutton.Location = new System.Drawing.Point(95, 45);
            this.mainFormpathbutton.Margin = new System.Windows.Forms.Padding(0);
            this.mainFormpathbutton.Name = "mainFormpathbutton";
            this.mainFormpathbutton.Size = new System.Drawing.Size(530, 23);
            this.mainFormpathbutton.TabIndex = 16;
            this.mainFormpathbutton.UseVisualStyleBackColor = false;
            this.mainFormpathbutton.Click += new System.EventHandler(this.mainFormpathbutton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(640, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Main_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(872, 463);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mainFormpathbutton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.mainFormcombobox);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.user_name_label);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.upperButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.mainFormrecentcombobox);
            this.Name = "Main_form";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button upperButton;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label user_name_label;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView mainFormtreeview;
        private System.Windows.Forms.ListView mainFormlistview;
        private System.Windows.Forms.TextBox textBox3;
        private ComboBox mainFormcombobox;
        private ComboBox mainFormrecentcombobox;
        private ImageList mainFormimagelist;
        private Button backButton;
        private Button mainFormpathbutton;
        private Button button1;
    }
}

