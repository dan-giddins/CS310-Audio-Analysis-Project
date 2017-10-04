namespace CS310_Audio_Analysis_Project
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
            this.components = new System.ComponentModel.Container();
            this.btnReset = new System.Windows.Forms.Button();
            this.tmrLabel = new System.Windows.Forms.Timer(this.components);
            this.boxDevices = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblDevices = new System.Windows.Forms.Label();
            this.picWaveform = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 517);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(786, 23);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Refresh Input";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tmrLabel
            // 
            this.tmrLabel.Interval = 1;
            this.tmrLabel.Tick += new System.EventHandler(this.tmrLabel_Tick);
            // 
            // boxDevices
            // 
            this.boxDevices.FormattingEnabled = true;
            this.boxDevices.Location = new System.Drawing.Point(89, 13);
            this.boxDevices.Name = "boxDevices";
            this.boxDevices.Size = new System.Drawing.Size(300, 21);
            this.boxDevices.TabIndex = 2;
            this.boxDevices.SelectedIndexChanged += new System.EventHandler(this.boxDevices_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(394, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh Device List";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblDevices
            // 
            this.lblDevices.AutoSize = true;
            this.lblDevices.Location = new System.Drawing.Point(12, 17);
            this.lblDevices.Name = "lblDevices";
            this.lblDevices.Size = new System.Drawing.Size(71, 13);
            this.lblDevices.TabIndex = 4;
            this.lblDevices.Text = "Input Device:";
            // 
            // picWaveform
            // 
            this.picWaveform.Location = new System.Drawing.Point(15, 41);
            this.picWaveform.Name = "picWaveform";
            this.picWaveform.Size = new System.Drawing.Size(783, 470);
            this.picWaveform.TabIndex = 5;
            this.picWaveform.TabStop = false;
            this.picWaveform.Paint += new System.Windows.Forms.PaintEventHandler(this.picWaveform_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 552);
            this.Controls.Add(this.picWaveform);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.boxDevices);
            this.Controls.Add(this.btnReset);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "CS310 Audio Analysis Project";
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Timer tmrLabel;
        private System.Windows.Forms.ComboBox boxDevices;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblDevices;
        private System.Windows.Forms.PictureBox picWaveform;
    }
}

