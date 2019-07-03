namespace SFGraphicsGui
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uVTestPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magentaBlackStripesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glViewport = new SFGraphics.Controls.GLViewport();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.loadTextureToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(540, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadTextureToolStripMenuItem
            // 
            this.loadTextureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uVTestPatternToolStripMenuItem,
            this.magentaBlackStripesToolStripMenuItem});
            this.loadTextureToolStripMenuItem.Name = "loadTextureToolStripMenuItem";
            this.loadTextureToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.loadTextureToolStripMenuItem.Text = "Render Texture";
            // 
            // uVTestPatternToolStripMenuItem
            // 
            this.uVTestPatternToolStripMenuItem.Name = "uVTestPatternToolStripMenuItem";
            this.uVTestPatternToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.uVTestPatternToolStripMenuItem.Text = "UV Test Pattern";
            this.uVTestPatternToolStripMenuItem.Click += new System.EventHandler(this.uvTestPatternToolStripMenuItem_Click);
            // 
            // magentaBlackStripesToolStripMenuItem
            // 
            this.magentaBlackStripesToolStripMenuItem.Name = "magentaBlackStripesToolStripMenuItem";
            this.magentaBlackStripesToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.magentaBlackStripesToolStripMenuItem.Text = "Magenta/Black Stripes";
            this.magentaBlackStripesToolStripMenuItem.Click += new System.EventHandler(this.magentaBlackStripesToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // glControl1
            // 
            this.glViewport.BackColor = System.Drawing.Color.Black;
            this.glViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glViewport.Location = new System.Drawing.Point(0, 24);
            this.glViewport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.glViewport.Name = "glControl1";
            this.glViewport.Size = new System.Drawing.Size(540, 368);
            this.glViewport.TabIndex = 2;
            this.glViewport.VSync = false;
            this.glViewport.Load += new System.EventHandler(this.glControl1_Load);
            this.glViewport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.glControl1_KeyPress);
            this.glViewport.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 392);
            this.Controls.Add(this.glViewport);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "SFGraphics Example";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadTextureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uVTestPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem magentaBlackStripesToolStripMenuItem;
        private SFGraphics.Controls.GLViewport glViewport;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
    }
}

