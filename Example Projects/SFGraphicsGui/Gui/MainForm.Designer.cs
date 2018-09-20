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
            this.glControl1 = new OpenTK.GLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uVTestPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magentaBlackStripesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderShapeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawCubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 42);
            this.glControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(1081, 711);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTextureToolStripMenuItem,
            this.renderShapeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1081, 42);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadTextureToolStripMenuItem
            // 
            this.loadTextureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uVTestPatternToolStripMenuItem,
            this.magentaBlackStripesToolStripMenuItem});
            this.loadTextureToolStripMenuItem.Name = "loadTextureToolStripMenuItem";
            this.loadTextureToolStripMenuItem.Size = new System.Drawing.Size(187, 38);
            this.loadTextureToolStripMenuItem.Text = "Render Texture";
            // 
            // uVTestPatternToolStripMenuItem
            // 
            this.uVTestPatternToolStripMenuItem.Name = "uVTestPatternToolStripMenuItem";
            this.uVTestPatternToolStripMenuItem.Size = new System.Drawing.Size(351, 38);
            this.uVTestPatternToolStripMenuItem.Text = "UV Test Pattern";
            this.uVTestPatternToolStripMenuItem.Click += new System.EventHandler(this.uVTestPatternToolStripMenuItem_Click);
            // 
            // magentaBlackStripesToolStripMenuItem
            // 
            this.magentaBlackStripesToolStripMenuItem.Name = "magentaBlackStripesToolStripMenuItem";
            this.magentaBlackStripesToolStripMenuItem.Size = new System.Drawing.Size(351, 38);
            this.magentaBlackStripesToolStripMenuItem.Text = "Magenta/Black Stripes";
            this.magentaBlackStripesToolStripMenuItem.Click += new System.EventHandler(this.magentaBlackStripesToolStripMenuItem_Click);
            // 
            // renderShapeToolStripMenuItem
            // 
            this.renderShapeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawCubeToolStripMenuItem});
            this.renderShapeToolStripMenuItem.Name = "renderShapeToolStripMenuItem";
            this.renderShapeToolStripMenuItem.Size = new System.Drawing.Size(175, 38);
            this.renderShapeToolStripMenuItem.Text = "Render Shape";
            // 
            // drawCubeToolStripMenuItem
            // 
            this.drawCubeToolStripMenuItem.Name = "drawCubeToolStripMenuItem";
            this.drawCubeToolStripMenuItem.Size = new System.Drawing.Size(324, 38);
            this.drawCubeToolStripMenuItem.Text = "Draw Cube";
            this.drawCubeToolStripMenuItem.Click += new System.EventHandler(this.drawCubeToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 753);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "SFGraphics Example";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadTextureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uVTestPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem magentaBlackStripesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderShapeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawCubeToolStripMenuItem;
    }
}

