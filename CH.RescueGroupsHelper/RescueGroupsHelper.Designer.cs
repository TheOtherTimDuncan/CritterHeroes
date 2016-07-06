namespace CH.RescueGroupsHelper
{
    partial class RescueGroupsHelper
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
            this.btnExecute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.cmbAction = new System.Windows.Forms.ComboBox();
            this.cbPrivate = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnLoadJson = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tree = new System.Windows.Forms.TreeView();
            this.txtHttp = new System.Windows.Forms.TextBox();
            this.clbFields = new System.Windows.Forms.CheckedListBox();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.cmbKeyField = new System.Windows.Forms.ComboBox();
            this.lblKeyField = new System.Windows.Forms.Label();
            this.lblKeyValue = new System.Windows.Forms.Label();
            this.txtKeyValue = new System.Windows.Forms.TextBox();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabExplorer = new System.Windows.Forms.TabPage();
            this.txtExplorerLog = new System.Windows.Forms.TextBox();
            this.tabImporter = new System.Windows.Forms.TabPage();
            this.btnImportFile = new System.Windows.Forms.Button();
            this.btnImportWeb = new System.Windows.Forms.Button();
            this.txtImporterLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabExplorer.SuspendLayout();
            this.tabImporter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(28, 108);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(100, 28);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Object Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Object Action:";
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "animals",
            "animalBreeds",
            "animalSpecies",
            "animalStatuses",
            "businesses",
            "people"});
            this.cmbType.Location = new System.Drawing.Point(119, 15);
            this.cmbType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(160, 24);
            this.cmbType.TabIndex = 5;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // cmbAction
            // 
            this.cmbAction.FormattingEnabled = true;
            this.cmbAction.Items.AddRange(new object[] {
            "define",
            "list",
            "search",
            "get",
            "add/update"});
            this.cmbAction.Location = new System.Drawing.Point(119, 49);
            this.cmbAction.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAction.Name = "cmbAction";
            this.cmbAction.Size = new System.Drawing.Size(160, 24);
            this.cmbAction.TabIndex = 6;
            this.cmbAction.SelectedIndexChanged += new System.EventHandler(this.cmbAction_SelectedIndexChanged);
            // 
            // cbPrivate
            // 
            this.cbPrivate.AutoSize = true;
            this.cbPrivate.Location = new System.Drawing.Point(122, 87);
            this.cbPrivate.Margin = new System.Windows.Forms.Padding(4);
            this.cbPrivate.Name = "cbPrivate";
            this.cbPrivate.Size = new System.Drawing.Size(18, 17);
            this.cbPrivate.TabIndex = 7;
            this.cbPrivate.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 87);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Private:";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(308, 108);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 28);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnLoadJson
            // 
            this.btnLoadJson.Location = new System.Drawing.Point(308, 18);
            this.btnLoadJson.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadJson.Name = "btnLoadJson";
            this.btnLoadJson.Size = new System.Drawing.Size(100, 28);
            this.btnLoadJson.TabIndex = 10;
            this.btnLoadJson.Text = "Load JSON";
            this.btnLoadJson.UseVisualStyleBackColor = true;
            this.btnLoadJson.Click += new System.EventHandler(this.btnLoadJson_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(431, 18);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtHttp);
            this.splitContainer1.Size = new System.Drawing.Size(770, 493);
            this.splitContainer1.SplitterDistance = 244;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 11;
            // 
            // tree
            // 
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Margin = new System.Windows.Forms.Padding(4);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(770, 244);
            this.tree.TabIndex = 4;
            // 
            // txtHttp
            // 
            this.txtHttp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHttp.Location = new System.Drawing.Point(0, 0);
            this.txtHttp.Margin = new System.Windows.Forms.Padding(4);
            this.txtHttp.Multiline = true;
            this.txtHttp.Name = "txtHttp";
            this.txtHttp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHttp.Size = new System.Drawing.Size(770, 244);
            this.txtHttp.TabIndex = 0;
            // 
            // clbFields
            // 
            this.clbFields.Enabled = false;
            this.clbFields.FormattingEnabled = true;
            this.clbFields.Location = new System.Drawing.Point(28, 144);
            this.clbFields.Name = "clbFields";
            this.clbFields.Size = new System.Drawing.Size(379, 140);
            this.clbFields.TabIndex = 12;
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(28, 291);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(107, 23);
            this.btnCheckAll.TabIndex = 13;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(141, 291);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(107, 23);
            this.btnUncheckAll.TabIndex = 14;
            this.btnUncheckAll.Text = "Uncheck All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // cmbKeyField
            // 
            this.cmbKeyField.FormattingEnabled = true;
            this.cmbKeyField.Location = new System.Drawing.Point(103, 335);
            this.cmbKeyField.Margin = new System.Windows.Forms.Padding(4);
            this.cmbKeyField.Name = "cmbKeyField";
            this.cmbKeyField.Size = new System.Drawing.Size(187, 24);
            this.cmbKeyField.TabIndex = 16;
            // 
            // lblKeyField
            // 
            this.lblKeyField.AutoSize = true;
            this.lblKeyField.Location = new System.Drawing.Point(28, 342);
            this.lblKeyField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKeyField.Name = "lblKeyField";
            this.lblKeyField.Size = new System.Drawing.Size(70, 17);
            this.lblKeyField.TabIndex = 15;
            this.lblKeyField.Text = "Key Field:";
            // 
            // lblKeyValue
            // 
            this.lblKeyValue.AutoSize = true;
            this.lblKeyValue.Location = new System.Drawing.Point(28, 372);
            this.lblKeyValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKeyValue.Name = "lblKeyValue";
            this.lblKeyValue.Size = new System.Drawing.Size(76, 17);
            this.lblKeyValue.TabIndex = 17;
            this.lblKeyValue.Text = "Key Value:";
            // 
            // txtKeyValue
            // 
            this.txtKeyValue.Location = new System.Drawing.Point(103, 366);
            this.txtKeyValue.Name = "txtKeyValue";
            this.txtKeyValue.Size = new System.Drawing.Size(187, 22);
            this.txtKeyValue.TabIndex = 18;
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabExplorer);
            this.tabs.Controls.Add(this.tabImporter);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1216, 547);
            this.tabs.TabIndex = 19;
            // 
            // tabExplorer
            // 
            this.tabExplorer.BackColor = System.Drawing.SystemColors.Control;
            this.tabExplorer.Controls.Add(this.txtKeyValue);
            this.tabExplorer.Controls.Add(this.lblKeyValue);
            this.tabExplorer.Controls.Add(this.cmbKeyField);
            this.tabExplorer.Controls.Add(this.lblKeyField);
            this.tabExplorer.Controls.Add(this.btnUncheckAll);
            this.tabExplorer.Controls.Add(this.btnCheckAll);
            this.tabExplorer.Controls.Add(this.clbFields);
            this.tabExplorer.Controls.Add(this.splitContainer1);
            this.tabExplorer.Controls.Add(this.btnLoadJson);
            this.tabExplorer.Controls.Add(this.btnClear);
            this.tabExplorer.Controls.Add(this.label3);
            this.tabExplorer.Controls.Add(this.cbPrivate);
            this.tabExplorer.Controls.Add(this.cmbAction);
            this.tabExplorer.Controls.Add(this.cmbType);
            this.tabExplorer.Controls.Add(this.label2);
            this.tabExplorer.Controls.Add(this.label1);
            this.tabExplorer.Controls.Add(this.btnExecute);
            this.tabExplorer.Controls.Add(this.txtExplorerLog);
            this.tabExplorer.Location = new System.Drawing.Point(4, 25);
            this.tabExplorer.Name = "tabExplorer";
            this.tabExplorer.Padding = new System.Windows.Forms.Padding(3);
            this.tabExplorer.Size = new System.Drawing.Size(1208, 518);
            this.tabExplorer.TabIndex = 0;
            this.tabExplorer.Text = "Explorer";
            // 
            // txtExplorerLog
            // 
            this.txtExplorerLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtExplorerLog.Location = new System.Drawing.Point(27, 403);
            this.txtExplorerLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtExplorerLog.Multiline = true;
            this.txtExplorerLog.Name = "txtExplorerLog";
            this.txtExplorerLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtExplorerLog.Size = new System.Drawing.Size(383, 108);
            this.txtExplorerLog.TabIndex = 0;
            // 
            // tabImporter
            // 
            this.tabImporter.BackColor = System.Drawing.SystemColors.Control;
            this.tabImporter.Controls.Add(this.btnImportFile);
            this.tabImporter.Controls.Add(this.btnImportWeb);
            this.tabImporter.Controls.Add(this.txtImporterLog);
            this.tabImporter.Location = new System.Drawing.Point(4, 25);
            this.tabImporter.Name = "tabImporter";
            this.tabImporter.Padding = new System.Windows.Forms.Padding(3);
            this.tabImporter.Size = new System.Drawing.Size(1208, 518);
            this.tabImporter.TabIndex = 1;
            this.tabImporter.Text = "Importer";
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
            // txtImporterLog
            // 
            this.txtImporterLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImporterLog.Location = new System.Drawing.Point(14, 45);
            this.txtImporterLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtImporterLog.Multiline = true;
            this.txtImporterLog.Name = "txtImporterLog";
            this.txtImporterLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtImporterLog.Size = new System.Drawing.Size(1188, 468);
            this.txtImporterLog.TabIndex = 0;
            // 
            // RescueGroupsHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 571);
            this.Controls.Add(this.tabs);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RescueGroupsHelper";
            this.Text = "Rescue Groups Helper";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabExplorer.ResumeLayout(false);
            this.tabExplorer.PerformLayout();
            this.tabImporter.ResumeLayout(false);
            this.tabImporter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.ComboBox cmbAction;
        private System.Windows.Forms.CheckBox cbPrivate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnLoadJson;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.TextBox txtHttp;
        private System.Windows.Forms.CheckedListBox clbFields;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.ComboBox cmbKeyField;
        private System.Windows.Forms.Label lblKeyField;
        private System.Windows.Forms.Label lblKeyValue;
        private System.Windows.Forms.TextBox txtKeyValue;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabExplorer;
        private System.Windows.Forms.TabPage tabImporter;
        private System.Windows.Forms.TextBox txtImporterLog;
        private System.Windows.Forms.Button btnImportWeb;
        private System.Windows.Forms.Button btnImportFile;
        private System.Windows.Forms.TextBox txtExplorerLog;
    }
}

