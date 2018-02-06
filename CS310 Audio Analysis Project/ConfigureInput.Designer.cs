﻿namespace CS310_Audio_Analysis_Project
{
    partial class ConfigureInputForm
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
            this.btnReset = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblDevices = new System.Windows.Forms.Label();
            this.picWaveform0 = new System.Windows.Forms.PictureBox();
            this.picWaveform1 = new System.Windows.Forms.PictureBox();
            this.picWaveform2 = new System.Windows.Forms.PictureBox();
            this.picWaveform3 = new System.Windows.Forms.PictureBox();
            this.boxDevice = new System.Windows.Forms.ComboBox();
            this.btnFrequencies = new System.Windows.Forms.Button();
            this.chkReadFile = new System.Windows.Forms.CheckBox();
            this.btnAnalyse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform3)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 489);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(400, 23);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Refresh Input";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(419, 489);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(400, 23);
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
            // picWaveform0
            // 
            this.picWaveform0.BackColor = System.Drawing.Color.White;
            this.picWaveform0.Location = new System.Drawing.Point(12, 40);
            this.picWaveform0.Name = "picWaveform0";
            this.picWaveform0.Size = new System.Drawing.Size(400, 200);
            this.picWaveform0.TabIndex = 5;
            this.picWaveform0.TabStop = false;
            this.picWaveform0.Paint += new System.Windows.Forms.PaintEventHandler(this.picWaveform0_Paint);
            // 
            // picWaveform1
            // 
            this.picWaveform1.BackColor = System.Drawing.Color.White;
            this.picWaveform1.Location = new System.Drawing.Point(419, 40);
            this.picWaveform1.Name = "picWaveform1";
            this.picWaveform1.Size = new System.Drawing.Size(400, 200);
            this.picWaveform1.TabIndex = 5;
            this.picWaveform1.TabStop = false;
            this.picWaveform1.Paint += new System.Windows.Forms.PaintEventHandler(this.picWaveform1_Paint);
            // 
            // picWaveform2
            // 
            this.picWaveform2.BackColor = System.Drawing.Color.White;
            this.picWaveform2.Location = new System.Drawing.Point(12, 282);
            this.picWaveform2.Name = "picWaveform2";
            this.picWaveform2.Size = new System.Drawing.Size(400, 200);
            this.picWaveform2.TabIndex = 5;
            this.picWaveform2.TabStop = false;
            this.picWaveform2.Paint += new System.Windows.Forms.PaintEventHandler(this.picWaveform2_Paint);
            // 
            // picWaveform3
            // 
            this.picWaveform3.BackColor = System.Drawing.Color.White;
            this.picWaveform3.Location = new System.Drawing.Point(419, 282);
            this.picWaveform3.Name = "picWaveform3";
            this.picWaveform3.Size = new System.Drawing.Size(400, 200);
            this.picWaveform3.TabIndex = 5;
            this.picWaveform3.TabStop = false;
            this.picWaveform3.Paint += new System.Windows.Forms.PaintEventHandler(this.picWaveform3_Paint);
            // 
            // boxDevice
            // 
            this.boxDevice.FormattingEnabled = true;
            this.boxDevice.Location = new System.Drawing.Point(89, 13);
            this.boxDevice.Name = "boxDevice";
            this.boxDevice.Size = new System.Drawing.Size(323, 21);
            this.boxDevice.TabIndex = 2;
            this.boxDevice.SelectedIndexChanged += new System.EventHandler(this.boxDevice_SelectedIndexChanged);
            // 
            // btnFrequencies
            // 
            this.btnFrequencies.Location = new System.Drawing.Point(115, 518);
            this.btnFrequencies.Name = "btnFrequencies";
            this.btnFrequencies.Size = new System.Drawing.Size(297, 23);
            this.btnFrequencies.TabIndex = 7;
            this.btnFrequencies.Text = "Test Frequencies";
            this.btnFrequencies.UseVisualStyleBackColor = true;
            this.btnFrequencies.Click += new System.EventHandler(this.btnFrequencies_Click);
            // 
            // chkReadFile
            // 
            this.chkReadFile.AutoSize = true;
            this.chkReadFile.Location = new System.Drawing.Point(12, 524);
            this.chkReadFile.Name = "chkReadFile";
            this.chkReadFile.Size = new System.Drawing.Size(97, 17);
            this.chkReadFile.TabIndex = 8;
            this.chkReadFile.Text = "Read From File";
            this.chkReadFile.UseVisualStyleBackColor = true;
            this.chkReadFile.CheckedChanged += new System.EventHandler(this.chkReadFile_CheckedChanged);
            // 
            // btnAnalyse
            // 
            this.btnAnalyse.Location = new System.Drawing.Point(419, 518);
            this.btnAnalyse.Name = "btnAnalyse";
            this.btnAnalyse.Size = new System.Drawing.Size(400, 23);
            this.btnAnalyse.TabIndex = 9;
            this.btnAnalyse.Text = "Analyse";
            this.btnAnalyse.UseVisualStyleBackColor = true;
            this.btnAnalyse.Click += new System.EventHandler(this.btnAnalysise_Click);
            // 
            // ConfigureInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 553);
            this.Controls.Add(this.btnAnalyse);
            this.Controls.Add(this.chkReadFile);
            this.Controls.Add(this.btnFrequencies);
            this.Controls.Add(this.picWaveform3);
            this.Controls.Add(this.picWaveform2);
            this.Controls.Add(this.picWaveform1);
            this.Controls.Add(this.picWaveform0);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.boxDevice);
            this.Controls.Add(this.btnReset);
            this.DoubleBuffered = true;
            this.Name = "ConfigureInputForm";
            this.Text = "Configure Input";
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaveform3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblDevices;
        private System.Windows.Forms.PictureBox picWaveform0;
        private System.Windows.Forms.PictureBox picWaveform1;
        private System.Windows.Forms.PictureBox picWaveform2;
        private System.Windows.Forms.PictureBox picWaveform3;
        private System.Windows.Forms.ComboBox boxDevice;
        private System.Windows.Forms.Button btnFrequencies;
        private System.Windows.Forms.CheckBox chkReadFile;
        private System.Windows.Forms.Button btnAnalyse;
    }
}

