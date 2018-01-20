namespace CS310_Audio_Analysis_Project
{
    partial class AnalysisForm
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
            this.picAnalysis = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAnalysis)).BeginInit();
            this.SuspendLayout();
            // 
            // picAnalysis
            // 
            this.picAnalysis.BackColor = System.Drawing.Color.White;
            this.picAnalysis.Location = new System.Drawing.Point(12, 12);
            this.picAnalysis.Name = "picAnalysis";
            this.picAnalysis.Size = new System.Drawing.Size(500, 500);
            this.picAnalysis.TabIndex = 1;
            this.picAnalysis.TabStop = false;
            this.picAnalysis.Paint += new System.Windows.Forms.PaintEventHandler(this.picAnalysis_Paint);
            // 
            // AnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 528);
            this.Controls.Add(this.picAnalysis);
            this.Name = "AnalysisForm";
            this.Text = "Analysis";
            ((System.ComponentModel.ISupportInitialize)(this.picAnalysis)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox picAnalysis;
    }
}