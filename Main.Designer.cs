namespace ChatBot
{
    partial class Main
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
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ExitNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Log = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyMenu.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.NotifyMenu;
            this.NotifyIcon.Text = "Skype Bot";
            this.NotifyIcon.Visible = true;
            // 
            // NotifyMenu
            // 
            this.NotifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowNotifyMenuItem,
            this.NotifyMenuSeparator,
            this.ExitNotifyMenuItem});
            this.NotifyMenu.Name = "NotifyMenu";
            this.NotifyMenu.Size = new System.Drawing.Size(104, 54);
            // 
            // ShowNotifyMenuItem
            // 
            this.ShowNotifyMenuItem.Name = "ShowNotifyMenuItem";
            this.ShowNotifyMenuItem.Size = new System.Drawing.Size(103, 22);
            this.ShowNotifyMenuItem.Text = "&Show";
            this.ShowNotifyMenuItem.Click += new System.EventHandler(this.ShowNotifyMenuItem_Click);
            // 
            // NotifyMenuSeparator
            // 
            this.NotifyMenuSeparator.Name = "NotifyMenuSeparator";
            this.NotifyMenuSeparator.Size = new System.Drawing.Size(100, 6);
            // 
            // ExitNotifyMenuItem
            // 
            this.ExitNotifyMenuItem.Name = "ExitNotifyMenuItem";
            this.ExitNotifyMenuItem.Size = new System.Drawing.Size(103, 22);
            this.ExitNotifyMenuItem.Text = "E&xit";
            this.ExitNotifyMenuItem.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Log
            // 
            this.Log.AcceptsTab = true;
            this.Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Log.BackColor = System.Drawing.Color.White;
            this.Log.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Log.Location = new System.Drawing.Point(12, 27);
            this.Log.Name = "Log";
            this.Log.ReadOnly = true;
            this.Log.Size = new System.Drawing.Size(657, 304);
            this.Log.TabIndex = 1;
            this.Log.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(681, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.ExitMenuItem});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(89, 6);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(92, 22);
            this.ExitMenuItem.Text = "E&xit";
            this.ExitMenuItem.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 343);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.Log);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Skype Bot";
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.NotifyMenu.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.ContextMenuStrip NotifyMenu;
        private System.Windows.Forms.ToolStripMenuItem ShowNotifyMenuItem;
        private System.Windows.Forms.ToolStripSeparator NotifyMenuSeparator;
        private System.Windows.Forms.ToolStripMenuItem ExitNotifyMenuItem;
        private System.Windows.Forms.RichTextBox Log;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
    }
}

