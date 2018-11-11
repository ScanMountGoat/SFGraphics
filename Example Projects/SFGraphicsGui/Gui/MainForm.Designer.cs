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
            this.renderShapeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawCubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawSphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glControl1 = new GLViewport.GLViewport();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTextureToolStripMenuItem,
            this.renderShapeToolStripMenuItem});
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
            // renderShapeToolStripMenuItem
            // 
            this.renderShapeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawCubeToolStripMenuItem,
            this.drawSphereToolStripMenuItem});
            this.renderShapeToolStripMenuItem.Name = "renderShapeToolStripMenuItem";
            this.renderShapeToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.renderShapeToolStripMenuItem.Text = "Render Shape";
            // 
            // drawCubeToolStripMenuItem
            // 
            this.drawCubeToolStripMenuItem.Name = "drawCubeToolStripMenuItem";
            this.drawCubeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.drawCubeToolStripMenuItem.Text = "Draw Cube";
            this.drawCubeToolStripMenuItem.Click += new System.EventHandler(this.drawCubeToolStripMenuItem_Click);
            // 
            // drawSphereToolStripMenuItem
            // 
            this.drawSphereToolStripMenuItem.Name = "drawSphereToolStripMenuItem";
            this.drawSphereToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.drawSphereToolStripMenuItem.Text = "Draw Sphere";
            this.drawSphereToolStripMenuItem.Click += new System.EventHandler(this.drawSphereToolStripMenuItem_Click);
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 24);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(540, 368);
            this.glControl1.TabIndex = 2;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 392);
            this.Controls.Add(this.glControl1);
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
        private System.Windows.Forms.ToolStripMenuItem renderShapeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawCubeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawSphereToolStripMenuItem;
        private GLViewport.GLViewport glControl1;
    }
}

