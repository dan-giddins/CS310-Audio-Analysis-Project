﻿namespace CS310_Audio_Analysis_Project
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
            this.btnRecord = new System.Windows.Forms.Button();
            this.tmrLabel = new System.Windows.Forms.Timer(this.components);
            this.boxDevices = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblDevices = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRecord
            // 
            this.btnRecord.Location = new System.Drawing.Point(12, 40);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(532, 23);
            this.btnRecord.TabIndex = 1;
            this.btnRecord.Text = "Record";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // tmrLabel
            // 
            this.tmrLabel.Interval = 50;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 536);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.boxDevices);
            this.Controls.Add(this.btnRecord);
            this.Name = "MainForm";
            this.Text = "CS310 Audio Analysis Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Timer tmrLabel;
        private System.Windows.Forms.ComboBox boxDevices;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblDevices;
    }
}

