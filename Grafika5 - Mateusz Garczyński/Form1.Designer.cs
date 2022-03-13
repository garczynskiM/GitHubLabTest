
namespace Grafika5___Mateusz_Garczyński
{
    partial class Form1
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
            this.drawingPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Shading = new System.Windows.Forms.GroupBox();
            this.noShadingRadioButton = new System.Windows.Forms.RadioButton();
            this.phongShadingRadioBUtton = new System.Windows.Forms.RadioButton();
            this.gouardShadingRadioButton = new System.Windows.Forms.RadioButton();
            this.constantShadingRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ballViewRadioButton = new System.Windows.Forms.RadioButton();
            this.sideViewRadioButton = new System.Windows.Forms.RadioButton();
            this.topDownViewRadioButton = new System.Windows.Forms.RadioButton();
            this.FOVTrackBarLabel = new System.Windows.Forms.Label();
            this.FOVTrackBar = new System.Windows.Forms.TrackBar();
            this.angleTrackBar = new System.Windows.Forms.TrackBar();
            this.angleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.Shading.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FOVTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.angleTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // drawingPanel
            // 
            this.drawingPanel.BackColor = System.Drawing.Color.White;
            this.drawingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingPanel.Location = new System.Drawing.Point(0, 0);
            this.drawingPanel.Name = "drawingPanel";
            this.drawingPanel.Size = new System.Drawing.Size(606, 450);
            this.drawingPanel.TabIndex = 0;
            this.drawingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.drawingPanel_Paint);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.drawingPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.angleLabel);
            this.splitContainer1.Panel2.Controls.Add(this.angleTrackBar);
            this.splitContainer1.Panel2.Controls.Add(this.Shading);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.FOVTrackBarLabel);
            this.splitContainer1.Panel2.Controls.Add(this.FOVTrackBar);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 606;
            this.splitContainer1.TabIndex = 1;
            // 
            // Shading
            // 
            this.Shading.Controls.Add(this.noShadingRadioButton);
            this.Shading.Controls.Add(this.phongShadingRadioBUtton);
            this.Shading.Controls.Add(this.gouardShadingRadioButton);
            this.Shading.Controls.Add(this.constantShadingRadioButton);
            this.Shading.Location = new System.Drawing.Point(3, 286);
            this.Shading.Name = "Shading";
            this.Shading.Size = new System.Drawing.Size(184, 120);
            this.Shading.TabIndex = 4;
            this.Shading.TabStop = false;
            this.Shading.Text = "Shading";
            // 
            // noShadingRadioButton
            // 
            this.noShadingRadioButton.AutoSize = true;
            this.noShadingRadioButton.Location = new System.Drawing.Point(0, 93);
            this.noShadingRadioButton.Name = "noShadingRadioButton";
            this.noShadingRadioButton.Size = new System.Drawing.Size(63, 21);
            this.noShadingRadioButton.TabIndex = 3;
            this.noShadingRadioButton.TabStop = true;
            this.noShadingRadioButton.Text = "None";
            this.noShadingRadioButton.UseVisualStyleBackColor = true;
            this.noShadingRadioButton.CheckedChanged += new System.EventHandler(this.noShadingRadioButton_CheckedChanged);
            // 
            // phongShadingRadioBUtton
            // 
            this.phongShadingRadioBUtton.AutoSize = true;
            this.phongShadingRadioBUtton.Location = new System.Drawing.Point(0, 73);
            this.phongShadingRadioBUtton.Name = "phongShadingRadioBUtton";
            this.phongShadingRadioBUtton.Size = new System.Drawing.Size(70, 21);
            this.phongShadingRadioBUtton.TabIndex = 2;
            this.phongShadingRadioBUtton.TabStop = true;
            this.phongShadingRadioBUtton.Text = "Phong";
            this.phongShadingRadioBUtton.UseVisualStyleBackColor = true;
            this.phongShadingRadioBUtton.CheckedChanged += new System.EventHandler(this.phongShadingRadioBUtton_CheckedChanged);
            // 
            // gouardShadingRadioButton
            // 
            this.gouardShadingRadioButton.AutoSize = true;
            this.gouardShadingRadioButton.Location = new System.Drawing.Point(0, 46);
            this.gouardShadingRadioButton.Name = "gouardShadingRadioButton";
            this.gouardShadingRadioButton.Size = new System.Drawing.Size(77, 21);
            this.gouardShadingRadioButton.TabIndex = 1;
            this.gouardShadingRadioButton.TabStop = true;
            this.gouardShadingRadioButton.Text = "Gouard";
            this.gouardShadingRadioButton.UseVisualStyleBackColor = true;
            this.gouardShadingRadioButton.CheckedChanged += new System.EventHandler(this.gouardShadingRadioButton_CheckedChanged);
            // 
            // constantShadingRadioButton
            // 
            this.constantShadingRadioButton.AutoSize = true;
            this.constantShadingRadioButton.Checked = true;
            this.constantShadingRadioButton.Location = new System.Drawing.Point(0, 22);
            this.constantShadingRadioButton.Name = "constantShadingRadioButton";
            this.constantShadingRadioButton.Size = new System.Drawing.Size(85, 21);
            this.constantShadingRadioButton.TabIndex = 0;
            this.constantShadingRadioButton.TabStop = true;
            this.constantShadingRadioButton.Text = "Constant";
            this.constantShadingRadioButton.UseVisualStyleBackColor = true;
            this.constantShadingRadioButton.CheckedChanged += new System.EventHandler(this.constantShadingRadioButton_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ballViewRadioButton);
            this.groupBox1.Controls.Add(this.sideViewRadioButton);
            this.groupBox1.Controls.Add(this.topDownViewRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(4, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera";
            // 
            // ballViewRadioButton
            // 
            this.ballViewRadioButton.AutoSize = true;
            this.ballViewRadioButton.Location = new System.Drawing.Point(7, 73);
            this.ballViewRadioButton.Name = "ballViewRadioButton";
            this.ballViewRadioButton.Size = new System.Drawing.Size(180, 21);
            this.ballViewRadioButton.TabIndex = 2;
            this.ballViewRadioButton.Text = "View from top of the ball";
            this.ballViewRadioButton.UseVisualStyleBackColor = true;
            this.ballViewRadioButton.CheckedChanged += new System.EventHandler(this.ballViewRadioButton_CheckedChanged);
            // 
            // sideViewRadioButton
            // 
            this.sideViewRadioButton.AutoSize = true;
            this.sideViewRadioButton.Location = new System.Drawing.Point(7, 47);
            this.sideViewRadioButton.Name = "sideViewRadioButton";
            this.sideViewRadioButton.Size = new System.Drawing.Size(88, 21);
            this.sideViewRadioButton.TabIndex = 1;
            this.sideViewRadioButton.Text = "Side view";
            this.sideViewRadioButton.UseVisualStyleBackColor = true;
            this.sideViewRadioButton.CheckedChanged += new System.EventHandler(this.sideViewRadioButton_CheckedChanged);
            // 
            // topDownViewRadioButton
            // 
            this.topDownViewRadioButton.AutoSize = true;
            this.topDownViewRadioButton.Checked = true;
            this.topDownViewRadioButton.Location = new System.Drawing.Point(7, 19);
            this.topDownViewRadioButton.Name = "topDownViewRadioButton";
            this.topDownViewRadioButton.Size = new System.Drawing.Size(122, 21);
            this.topDownViewRadioButton.TabIndex = 0;
            this.topDownViewRadioButton.TabStop = true;
            this.topDownViewRadioButton.Text = "Top down view";
            this.topDownViewRadioButton.UseVisualStyleBackColor = true;
            this.topDownViewRadioButton.CheckedChanged += new System.EventHandler(this.topDownViewRadioButton_CheckedChanged);
            // 
            // FOVTrackBarLabel
            // 
            this.FOVTrackBarLabel.AutoSize = true;
            this.FOVTrackBarLabel.Location = new System.Drawing.Point(14, 14);
            this.FOVTrackBarLabel.Name = "FOVTrackBarLabel";
            this.FOVTrackBarLabel.Size = new System.Drawing.Size(68, 17);
            this.FOVTrackBarLabel.TabIndex = 1;
            this.FOVTrackBarLabel.Text = "FOV = 60";
            // 
            // FOVTrackBar
            // 
            this.FOVTrackBar.Location = new System.Drawing.Point(3, 34);
            this.FOVTrackBar.Maximum = 120;
            this.FOVTrackBar.Minimum = 30;
            this.FOVTrackBar.Name = "FOVTrackBar";
            this.FOVTrackBar.Size = new System.Drawing.Size(187, 56);
            this.FOVTrackBar.TabIndex = 0;
            this.FOVTrackBar.TickFrequency = 5;
            this.FOVTrackBar.Value = 60;
            this.FOVTrackBar.Scroll += new System.EventHandler(this.FOVTrackBar_Scroll);
            // 
            // angleTrackBar
            // 
            this.angleTrackBar.Location = new System.Drawing.Point(11, 214);
            this.angleTrackBar.Maximum = 100;
            this.angleTrackBar.Name = "angleTrackBar";
            this.angleTrackBar.Size = new System.Drawing.Size(167, 56);
            this.angleTrackBar.TabIndex = 5;
            this.angleTrackBar.TickFrequency = 10;
            this.angleTrackBar.Value = 50;
            this.angleTrackBar.Scroll += new System.EventHandler(this.angleTrackBar_Scroll);
            // 
            // angleLabel
            // 
            this.angleLabel.AutoSize = true;
            this.angleLabel.Location = new System.Drawing.Point(17, 191);
            this.angleLabel.Name = "angleLabel";
            this.angleLabel.Size = new System.Drawing.Size(68, 17);
            this.angleLabel.TabIndex = 6;
            this.angleLabel.Text = "Angle = 0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.Shading.ResumeLayout(false);
            this.Shading.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FOVTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.angleTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel drawingPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label FOVTrackBarLabel;
        private System.Windows.Forms.TrackBar FOVTrackBar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton ballViewRadioButton;
        private System.Windows.Forms.RadioButton sideViewRadioButton;
        private System.Windows.Forms.RadioButton topDownViewRadioButton;
        private System.Windows.Forms.GroupBox Shading;
        private System.Windows.Forms.RadioButton phongShadingRadioBUtton;
        private System.Windows.Forms.RadioButton gouardShadingRadioButton;
        private System.Windows.Forms.RadioButton constantShadingRadioButton;
        private System.Windows.Forms.RadioButton noShadingRadioButton;
        private System.Windows.Forms.TrackBar angleTrackBar;
        private System.Windows.Forms.Label angleLabel;
    }
}

