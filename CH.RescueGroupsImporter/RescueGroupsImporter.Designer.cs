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
            this.txtLog.Location = new System.Drawing.Point(11, 42);
            this.txtLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(1014, 412);
            this.txtLog.TabIndex = 0;
            // 
            // btnImportWeb
            // 
            this.btnImportWeb.Location = new System.Drawing.Point(12, 10);
            this.btnImportWeb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImportWeb.Name = "btnImportWeb";
            this.btnImportWeb.Size = new System.Drawing.Size(132, 27);
            this.btnImportWeb.TabIndex = 1;
            this.btnImportWeb.Text = "Import From Web";
            this.btnImportWeb.UseVisualStyleBackColor = true;
            this.btnImportWeb.Click += new System.EventHandler(this.btnImportWeb_Click);
            // 
            // btnImportFile
            // 
            this.btnImportFile.Location = new System.Drawing.Point(148, 10);
            this.btnImportFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImportFile.Name = "btnImportFile";
            this.btnImportFile.Size = new System.Drawing.Size(132, 27);
            this.btnImportFile.TabIndex = 2;
            this.btnImportFile.Text = "Import From File";
            this.btnImportFile.UseVisualStyleBackColor = true;
            this.btnImportFile.Click += new System.EventHandler(this.btnImportFile_Click);
            // 
            // RescueGroupsImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 463);
            this.Controls.Add(this.btnImportFile);
            this.Controls.Add(this.btnImportWeb);
            this.Controls.Add(this.txtLog);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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

