using System.Drawing;
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
            this.mainFormcombobox = new System.Windows.Forms.ComboBox();
            this.mainFormrecentcombobox = new System.Windows.Forms.ComboBox();
            this.mainFormimagelist = new System.Windows.Forms.ImageList(this.components);
            this.backButton = new System.Windows.Forms.Button();
            this.mainFormpathbutton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.mainFormlistitemcount = new System.Windows.Forms.Label();
            this.mainFormselectedinfo = new System.Windows.Forms.Label();
            this.mainFormlistviewcontextmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.열기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.복사ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.이름바꾸기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainFormlistviewcopycontextmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.복사ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mainFormlistviewcontextmenu.SuspendLayout();
            this.mainFormlistviewcopycontextmenu.SuspendLayout();
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
            this.mainFormtreeview.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mainFormtreeview_NodeMouseClick);
            // 
            // mainFormlistview
            // 
            this.mainFormlistview.AllowDrop = true;
            this.mainFormlistview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainFormlistview.Font = new System.Drawing.Font("굴림", 12F);
            this.mainFormlistview.FullRowSelect = true;
            this.mainFormlistview.Location = new System.Drawing.Point(0, 0);
            this.mainFormlistview.Name = "mainFormlistview";
            this.mainFormlistview.Size = new System.Drawing.Size(716, 368);
            this.mainFormlistview.TabIndex = 0;
            this.mainFormlistview.UseCompatibleStateImageBehavior = false;
            this.mainFormlistview.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.mainFormlistview_ColumnClick);
            this.mainFormlistview.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.mainFormlistview_ItemDrag);
            this.mainFormlistview.DragDrop += new System.Windows.Forms.DragEventHandler(this.mainFormlistview_DragDrop);
            this.mainFormlistview.DragOver += new System.Windows.Forms.DragEventHandler(this.mainFormlistview_DragOver);
            this.mainFormlistview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainFormlistview_KeyDown);
            this.mainFormlistview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mainFormlistview_MouseClick);
            this.mainFormlistview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewDoubleClick);
            this.mainFormlistview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainFormlistview_MouseDown);
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
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mainFormlistitemcount
            // 
            this.mainFormlistitemcount.AutoSize = true;
            this.mainFormlistitemcount.Font = new System.Drawing.Font("굴림", 10F);
            this.mainFormlistitemcount.Location = new System.Drawing.Point(14, 443);
            this.mainFormlistitemcount.Name = "mainFormlistitemcount";
            this.mainFormlistitemcount.Size = new System.Drawing.Size(0, 14);
            this.mainFormlistitemcount.TabIndex = 18;
            // 
            // mainFormselectedinfo
            // 
            this.mainFormselectedinfo.AutoSize = true;
            this.mainFormselectedinfo.Font = new System.Drawing.Font("굴림", 10F);
            this.mainFormselectedinfo.Location = new System.Drawing.Point(112, 445);
            this.mainFormselectedinfo.Name = "mainFormselectedinfo";
            this.mainFormselectedinfo.Size = new System.Drawing.Size(0, 14);
            this.mainFormselectedinfo.TabIndex = 19;
            // 
            // mainFormlistviewcontextmenu
            // 
            this.mainFormlistviewcontextmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.열기ToolStripMenuItem,
            this.복사ToolStripMenuItem,
            this.삭제ToolStripMenuItem,
            this.이름바꾸기ToolStripMenuItem});
            this.mainFormlistviewcontextmenu.Name = "contextMenuStrip1";
            this.mainFormlistviewcontextmenu.Size = new System.Drawing.Size(139, 92);
            this.mainFormlistviewcontextmenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mainFormlistviewcontextmenu_ItemClicked);
            // 
            // 열기ToolStripMenuItem
            // 
            this.열기ToolStripMenuItem.Name = "열기ToolStripMenuItem";
            this.열기ToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.열기ToolStripMenuItem.Text = "열기";
            // 
            // 복사ToolStripMenuItem
            // 
            this.복사ToolStripMenuItem.Name = "복사ToolStripMenuItem";
            this.복사ToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.복사ToolStripMenuItem.Text = "복사";
            // 
            // 삭제ToolStripMenuItem
            // 
            this.삭제ToolStripMenuItem.Name = "삭제ToolStripMenuItem";
            this.삭제ToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.삭제ToolStripMenuItem.Text = "삭제";
            // 
            // 이름바꾸기ToolStripMenuItem
            // 
            this.이름바꾸기ToolStripMenuItem.Name = "이름바꾸기ToolStripMenuItem";
            this.이름바꾸기ToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.이름바꾸기ToolStripMenuItem.Text = "이름 바꾸기";
            // 
            // mainFormlistviewcopycontextmenu
            // 
            this.mainFormlistviewcopycontextmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.복사ToolStripMenuItem1});
            this.mainFormlistviewcopycontextmenu.Name = "mainFormlistviewcopycontextmenu";
            this.mainFormlistviewcopycontextmenu.Size = new System.Drawing.Size(181, 48);
            // 
            // 복사ToolStripMenuItem1
            // 
            this.복사ToolStripMenuItem1.Name = "복사ToolStripMenuItem1";
            this.복사ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.복사ToolStripMenuItem1.Text = "붙여넣기";
            this.복사ToolStripMenuItem1.Click += new System.EventHandler(this.복사ToolStripMenuItem1_Click);
            // 
            // Main_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(872, 461);
            this.Controls.Add(this.mainFormselectedinfo);
            this.Controls.Add(this.mainFormlistitemcount);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mainFormpathbutton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.mainFormcombobox);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.user_name_label);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.upperButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.mainFormrecentcombobox);
            this.Font = new System.Drawing.Font("굴림", 9F);
            this.Name = "Main_form";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Main_form_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mainFormlistviewcontextmenu.ResumeLayout(false);
            this.mainFormlistviewcopycontextmenu.ResumeLayout(false);
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
        private ComboBox mainFormcombobox;
        private ComboBox mainFormrecentcombobox;
        private ImageList mainFormimagelist;
        private Button backButton;
        private Button mainFormpathbutton;
        private Button button1;
        private Label mainFormlistitemcount;
        private Label mainFormselectedinfo;
        private ContextMenuStrip mainFormlistviewcontextmenu;
        private ToolStripMenuItem 열기ToolStripMenuItem;
        private ToolStripMenuItem 복사ToolStripMenuItem;
        private ToolStripMenuItem 삭제ToolStripMenuItem;
        private ToolStripMenuItem 이름바꾸기ToolStripMenuItem;
        private ContextMenuStrip mainFormlistviewcopycontextmenu;
        private ToolStripMenuItem 복사ToolStripMenuItem1;
    }
}

