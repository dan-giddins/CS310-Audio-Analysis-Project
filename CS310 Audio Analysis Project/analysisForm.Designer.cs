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
            this.lblX = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFreq = new System.Windows.Forms.TextBox();
            this.txtExpander = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkCircles = new System.Windows.Forms.CheckBox();
            this.chkIntersections = new System.Windows.Forms.CheckBox();
            this.chkPoints = new System.Windows.Forms.CheckBox();
            this.chkSolo = new System.Windows.Forms.CheckBox();
            this.chkEntities = new System.Windows.Forms.CheckBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.txtSeparation = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAnalysis)).BeginInit();
            this.SuspendLayout();
            // 
            // picAnalysis
            // 
            this.picAnalysis.BackColor = System.Drawing.Color.White;
            this.picAnalysis.Location = new System.Drawing.Point(210, 12);
            this.picAnalysis.Name = "picAnalysis";
            this.picAnalysis.Size = new System.Drawing.Size(971, 543);
            this.picAnalysis.TabIndex = 1;
            this.picAnalysis.TabStop = false;
            this.picAnalysis.Paint += new System.Windows.Forms.PaintEventHandler(this.picAnalysis_Paint);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(118, 15);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(20, 13);
            this.lblX.TabIndex = 2;
            this.lblX.Text = "X: ";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(118, 38);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(17, 13);
            this.lblY.TabIndex = 2;
            this.lblY.Text = "Y:";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(12, 12);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(100, 20);
            this.txtX.TabIndex = 3;
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(12, 35);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(100, 20);
            this.txtY.TabIndex = 3;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(9, 58);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(32, 13);
            this.lblError.TabIndex = 4;
            this.lblError.Text = "Error:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Threshold (int):";
            // 
            // txtThreshold
            // 
            this.txtThreshold.Location = new System.Drawing.Point(92, 150);
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.Size = new System.Drawing.Size(100, 20);
            this.txtThreshold.TabIndex = 3;
            this.txtThreshold.Text = "20";
            this.txtThreshold.TextChanged += new System.EventHandler(this.txtThreshold_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Solo_freq (int):";
            // 
            // txtFreq
            // 
            this.txtFreq.Location = new System.Drawing.Point(92, 177);
            this.txtFreq.Name = "txtFreq";
            this.txtFreq.Size = new System.Drawing.Size(100, 20);
            this.txtFreq.TabIndex = 6;
            this.txtFreq.Text = "30";
            this.txtFreq.TextChanged += new System.EventHandler(this.txtFreq_TextChanged);
            // 
            // txtExpander
            // 
            this.txtExpander.Location = new System.Drawing.Point(92, 203);
            this.txtExpander.Name = "txtExpander";
            this.txtExpander.Size = new System.Drawing.Size(100, 20);
            this.txtExpander.TabIndex = 6;
            this.txtExpander.Text = "2";
            this.txtExpander.TextChanged += new System.EventHandler(this.txtExpander_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Expander (d):";
            // 
            // txtGroup
            // 
            this.txtGroup.Location = new System.Drawing.Point(92, 229);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(100, 20);
            this.txtGroup.TabIndex = 6;
            this.txtGroup.Text = "0.01";
            this.txtGroup.TextChanged += new System.EventHandler(this.txtGroup_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Group (d):";
            // 
            // chkCircles
            // 
            this.chkCircles.AutoSize = true;
            this.chkCircles.Location = new System.Drawing.Point(12, 255);
            this.chkCircles.Name = "chkCircles";
            this.chkCircles.Size = new System.Drawing.Size(85, 17);
            this.chkCircles.TabIndex = 8;
            this.chkCircles.Text = "Draw Circles";
            this.chkCircles.UseVisualStyleBackColor = true;
            this.chkCircles.CheckedChanged += new System.EventHandler(this.chkCircles_CheckedChanged);
            // 
            // chkIntersections
            // 
            this.chkIntersections.AutoSize = true;
            this.chkIntersections.Location = new System.Drawing.Point(12, 278);
            this.chkIntersections.Name = "chkIntersections";
            this.chkIntersections.Size = new System.Drawing.Size(114, 17);
            this.chkIntersections.TabIndex = 8;
            this.chkIntersections.Text = "Draw Intersections";
            this.chkIntersections.UseVisualStyleBackColor = true;
            this.chkIntersections.CheckedChanged += new System.EventHandler(this.chkIntersections_CheckedChanged);
            // 
            // chkPoints
            // 
            this.chkPoints.AutoSize = true;
            this.chkPoints.Location = new System.Drawing.Point(12, 301);
            this.chkPoints.Name = "chkPoints";
            this.chkPoints.Size = new System.Drawing.Size(83, 17);
            this.chkPoints.TabIndex = 8;
            this.chkPoints.Text = "Draw Points";
            this.chkPoints.UseVisualStyleBackColor = true;
            this.chkPoints.CheckedChanged += new System.EventHandler(this.chkPoints_CheckedChanged);
            // 
            // chkSolo
            // 
            this.chkSolo.AutoSize = true;
            this.chkSolo.Location = new System.Drawing.Point(12, 324);
            this.chkSolo.Name = "chkSolo";
            this.chkSolo.Size = new System.Drawing.Size(47, 17);
            this.chkSolo.TabIndex = 8;
            this.chkSolo.Text = "Solo";
            this.chkSolo.UseVisualStyleBackColor = true;
            this.chkSolo.CheckedChanged += new System.EventHandler(this.chkSolo_CheckedChanged);
            // 
            // chkEntities
            // 
            this.chkEntities.AutoSize = true;
            this.chkEntities.Checked = true;
            this.chkEntities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEntities.Location = new System.Drawing.Point(12, 347);
            this.chkEntities.Name = "chkEntities";
            this.chkEntities.Size = new System.Drawing.Size(88, 17);
            this.chkEntities.TabIndex = 8;
            this.chkEntities.Text = "Draw Entities";
            this.chkEntities.UseVisualStyleBackColor = true;
            this.chkEntities.CheckedChanged += new System.EventHandler(this.chkEntities_CheckedChanged);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(12, 370);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(74, 17);
            this.chkAll.TabIndex = 8;
            this.chkAll.Text = "All Entities";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Scale (float):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Size (int):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Separation (d):";
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(92, 71);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(100, 20);
            this.txtScale.TabIndex = 3;
            this.txtScale.Text = "200";
            this.txtScale.TextChanged += new System.EventHandler(this.txtScale_TextChanged);
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(92, 97);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(100, 20);
            this.txtSize.TabIndex = 3;
            this.txtSize.Text = "10";
            this.txtSize.TextChanged += new System.EventHandler(this.txtSize_TextChanged);
            // 
            // txtSeparation
            // 
            this.txtSeparation.Location = new System.Drawing.Point(92, 123);
            this.txtSeparation.Name = "txtSeparation";
            this.txtSeparation.Size = new System.Drawing.Size(100, 20);
            this.txtSeparation.TabIndex = 3;
            this.txtSeparation.Text = "0.6";
            this.txtSeparation.TextChanged += new System.EventHandler(this.txtSeparation_TextChanged);
            // 
            // AnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 567);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.chkEntities);
            this.Controls.Add(this.chkSolo);
            this.Controls.Add(this.chkPoints);
            this.Controls.Add(this.chkIntersections);
            this.Controls.Add(this.chkCircles);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGroup);
            this.Controls.Add(this.txtExpander);
            this.Controls.Add(this.txtFreq);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.txtSeparation);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.txtScale);
            this.Controls.Add(this.txtThreshold);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.lblY);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.picAnalysis);
            this.Name = "AnalysisForm";
            this.Text = "Analysis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnalysisForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picAnalysis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picAnalysis;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFreq;
        private System.Windows.Forms.TextBox txtExpander;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkCircles;
        private System.Windows.Forms.CheckBox chkIntersections;
        private System.Windows.Forms.CheckBox chkPoints;
        private System.Windows.Forms.CheckBox chkSolo;
        private System.Windows.Forms.CheckBox chkEntities;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.TextBox txtSeparation;
    }
}