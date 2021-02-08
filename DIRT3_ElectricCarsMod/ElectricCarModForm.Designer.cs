namespace DIRT3_ElectricCarsMod
{
    partial class ElectricCarModForm
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
            this.rtbECM = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbECM
            // 
            this.rtbECM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbECM.BackColor = System.Drawing.Color.White;
            this.rtbECM.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbECM.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.rtbECM.Location = new System.Drawing.Point(12, 12);
            this.rtbECM.Name = "rtbECM";
            this.rtbECM.ReadOnly = true;
            this.rtbECM.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbECM.Size = new System.Drawing.Size(591, 448);
            this.rtbECM.TabIndex = 0;
            this.rtbECM.TabStop = false;
            this.rtbECM.Text = "";
            // 
            // ElectricCarModForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 472);
            this.Controls.Add(this.rtbECM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ElectricCarModForm";
            this.Opacity = 0.95D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "DiRT 3 - Elektric Cars Mod Info";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbECM;
    }
}