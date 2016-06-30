namespace CH.RescueGroupsImporter
{
    partial class RescueGroupsImporter
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnImportWeb = new System.Windows.Forms.Button();
            this.btnImportFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 53);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(1140, 514);
            this.txtLog.TabIndex = 0;
            // 
            // btnImportWeb
            // 
            this.btnImportWeb.Location = new System.Drawing.Point(13, 13);
            this.btnImportWeb.Name = "btnImportWeb";
            this.btnImportWeb.Size = new System.Drawing.Size(148, 34);
            this.btnImportWeb.TabIndex = 1;
            this.btnImportWeb.Text = "Import From Web";
            this.btnImportWeb.UseVisualStyleBackColor = true;
            // 
            // btnImportFile
            // 
            this.btnImportFile.Location = new System.Drawing.Point(167, 12);
            this.btnImportFile.Name = "btnImportFile";
            this.btnImportFile.Size = new System.Drawing.Size(148, 34);
            this.btnImportFile.TabIndex = 2;
            this.btnImportFile.Text = "Import From File";
            this.btnImportFile.UseVisualStyleBackColor = true;
            // 
            // RescueGroupsImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 579);
            this.Controls.Add(this.btnImportFile);
            this.Controls.Add(this.btnImportWeb);
            this.Controls.Add(this.txtLog);
            this.Name = "RescueGroupsImporter";
            this.Text = "Rescue Groups Importer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnImportWeb;
        private System.Windows.Forms.Button btnImportFile;
    }
}

