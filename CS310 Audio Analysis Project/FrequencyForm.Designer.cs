namespace CS310_Audio_Analysis_Project
{
    partial class FrequencyForm
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
            this.picFrequency3 = new System.Windows.Forms.PictureBox();
            this.picFrequency2 = new System.Windows.Forms.PictureBox();
            this.picFrequency1 = new System.Windows.Forms.PictureBox();
            this.picFrequency0 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picFrequency3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrequency2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrequency1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrequency0)).BeginInit();
            this.SuspendLayout();
            // 
            // picFrequency3
            // 
            this.picFrequency3.Location = new System.Drawing.Point(419, 218);
            this.picFrequency3.Name = "picFrequency3";
            this.picFrequency3.Size = new System.Drawing.Size(400, 200);
            this.picFrequency3.TabIndex = 6;
            this.picFrequency3.TabStop = false;
            this.picFrequency3.Paint += new System.Windows.Forms.PaintEventHandler(this.picFrequency3_Paint);
            // 
            // picFrequency2
            // 
            this.picFrequency2.Location = new System.Drawing.Point(12, 218);
            this.picFrequency2.Name = "picFrequency2";
            this.picFrequency2.Size = new System.Drawing.Size(400, 200);
            this.picFrequency2.TabIndex = 7;
            this.picFrequency2.TabStop = false;
            this.picFrequency2.Paint += new System.Windows.Forms.PaintEventHandler(this.picFrequency2_Paint);
            // 
            // picFrequency1
            // 
            this.picFrequency1.Location = new System.Drawing.Point(419, 12);
            this.picFrequency1.Name = "picFrequency1";
            this.picFrequency1.Size = new System.Drawing.Size(400, 200);
            this.picFrequency1.TabIndex = 8;
            this.picFrequency1.TabStop = false;
            this.picFrequency1.Paint += new System.Windows.Forms.PaintEventHandler(this.picFrequency1_Paint);
            // 
            // picFrequency0
            // 
            this.picFrequency0.Location = new System.Drawing.Point(12, 12);
            this.picFrequency0.Name = "picFrequency0";
            this.picFrequency0.Size = new System.Drawing.Size(400, 200);
            this.picFrequency0.TabIndex = 9;
            this.picFrequency0.TabStop = false;
            this.picFrequency0.Paint += new System.Windows.Forms.PaintEventHandler(this.picFrequency0_Paint);
            // 
            // FrequencyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 473);
            this.Controls.Add(this.picFrequency3);
            this.Controls.Add(this.picFrequency2);
            this.Controls.Add(this.picFrequency1);
            this.Controls.Add(this.picFrequency0);
            this.Name = "FrequencyForm";
            this.Text = "Frequency Test";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrequencyForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picFrequency3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrequency2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrequency1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrequency0)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picFrequency3;
        private System.Windows.Forms.PictureBox picFrequency2;
        private System.Windows.Forms.PictureBox picFrequency1;
        private System.Windows.Forms.PictureBox picFrequency0;
    }
}