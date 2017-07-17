namespace DocuBundle
{
    partial class DocuBundle
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoginStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboRecordType = new System.Windows.Forms.ComboBox();
            this.cboNoteType = new System.Windows.Forms.ComboBox();
            this.btnDirectory = new System.Windows.Forms.Button();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.lstNoteList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnGetNoteList = new System.Windows.Forms.Button();
            this.btnExportSelected = new System.Windows.Forms.Button();
            this.txtNoteTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimeBegin = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.wbLogin = new System.Windows.Forms.WebBrowser();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(10, 12);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(131, 24);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login...";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Note Type:";
            // 
            // txtLoginStatus
            // 
            this.txtLoginStatus.Location = new System.Drawing.Point(145, 15);
            this.txtLoginStatus.Name = "txtLoginStatus";
            this.txtLoginStatus.ReadOnly = true;
            this.txtLoginStatus.Size = new System.Drawing.Size(475, 20);
            this.txtLoginStatus.TabIndex = 8;
            this.txtLoginStatus.Text = "Click the login button to sign into salesforce.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Record Type:";
            // 
            // cboRecordType
            // 
            this.cboRecordType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboRecordType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRecordType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecordType.FormattingEnabled = true;
            this.cboRecordType.Location = new System.Drawing.Point(91, 105);
            this.cboRecordType.Name = "cboRecordType";
            this.cboRecordType.Size = new System.Drawing.Size(380, 21);
            this.cboRecordType.TabIndex = 10;
            this.cboRecordType.SelectedIndexChanged += new System.EventHandler(this.cboRecordType_SelectedIndexChanged);
            // 
            // cboNoteType
            // 
            this.cboNoteType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboNoteType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNoteType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNoteType.FormattingEnabled = true;
            this.cboNoteType.Location = new System.Drawing.Point(91, 132);
            this.cboNoteType.Name = "cboNoteType";
            this.cboNoteType.Size = new System.Drawing.Size(380, 21);
            this.cboNoteType.TabIndex = 11;
            // 
            // btnDirectory
            // 
            this.btnDirectory.Location = new System.Drawing.Point(10, 65);
            this.btnDirectory.Name = "btnDirectory";
            this.btnDirectory.Size = new System.Drawing.Size(103, 24);
            this.btnDirectory.TabIndex = 12;
            this.btnDirectory.Text = "Directory...";
            this.btnDirectory.UseVisualStyleBackColor = true;
            this.btnDirectory.Click += new System.EventHandler(this.btnDirectory_Click);
            // 
            // txtDirectory
            // 
            this.txtDirectory.Location = new System.Drawing.Point(119, 68);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.Size = new System.Drawing.Size(501, 20);
            this.txtDirectory.TabIndex = 13;
            this.txtDirectory.TextChanged += new System.EventHandler(this.txtDirectory_TextChanged);
            // 
            // lstNoteList
            // 
            this.lstNoteList.CheckBoxes = true;
            this.lstNoteList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2});
            this.lstNoteList.GridLines = true;
            this.lstNoteList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstNoteList.Location = new System.Drawing.Point(12, 213);
            this.lstNoteList.Name = "lstNoteList";
            this.lstNoteList.Size = new System.Drawing.Size(656, 361);
            this.lstNoteList.TabIndex = 14;
            this.lstNoteList.UseCompatibleStateImageBehavior = false;
            this.lstNoteList.View = System.Windows.Forms.View.Details;
            this.lstNoteList.Visible = false;
            this.lstNoteList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstNoteList_ItemCheck);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Staff Notes Title";
            this.columnHeader1.Width = 300;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Note Type";
            this.columnHeader3.Width = 170;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Date/Time";
            this.columnHeader2.Width = 150;
            // 
            // btnGetNoteList
            // 
            this.btnGetNoteList.Enabled = false;
            this.btnGetNoteList.Location = new System.Drawing.Point(491, 104);
            this.btnGetNoteList.Name = "btnGetNoteList";
            this.btnGetNoteList.Size = new System.Drawing.Size(129, 21);
            this.btnGetNoteList.TabIndex = 15;
            this.btnGetNoteList.Text = "Get Note List";
            this.btnGetNoteList.UseVisualStyleBackColor = true;
            this.btnGetNoteList.Click += new System.EventHandler(this.btnGetNoteList_Click);
            // 
            // btnExportSelected
            // 
            this.btnExportSelected.Enabled = false;
            this.btnExportSelected.Location = new System.Drawing.Point(491, 130);
            this.btnExportSelected.Name = "btnExportSelected";
            this.btnExportSelected.Size = new System.Drawing.Size(129, 23);
            this.btnExportSelected.TabIndex = 16;
            this.btnExportSelected.Text = "Export";
            this.btnExportSelected.UseVisualStyleBackColor = true;
            this.btnExportSelected.Click += new System.EventHandler(this.btnExportSelected_Click);
            // 
            // txtNoteTitle
            // 
            this.txtNoteTitle.Location = new System.Drawing.Point(91, 160);
            this.txtNoteTitle.Name = "txtNoteTitle";
            this.txtNoteTitle.Size = new System.Drawing.Size(380, 20);
            this.txtNoteTitle.TabIndex = 17;
            this.txtNoteTitle.TextChanged += new System.EventHandler(this.txtNoteTitle_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Note Title:";
            // 
            // dateTimeBegin
            // 
            this.dateTimeBegin.Location = new System.Drawing.Point(145, 187);
            this.dateTimeBegin.Name = "dateTimeBegin";
            this.dateTimeBegin.Size = new System.Drawing.Size(200, 20);
            this.dateTimeBegin.TabIndex = 19;
            this.dateTimeBegin.Value = new System.DateTime(2014, 1, 1, 0, 0, 0, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(76, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Begin Date:";
            // 
            // dateTimeEnd
            // 
            this.dateTimeEnd.Location = new System.Drawing.Point(420, 187);
            this.dateTimeEnd.Name = "dateTimeEnd";
            this.dateTimeEnd.Size = new System.Drawing.Size(200, 20);
            this.dateTimeEnd.TabIndex = 21;
            this.dateTimeEnd.Value = new System.DateTime(2200, 1, 1, 0, 0, 0, 0);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(359, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "End Date:";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(16, 193);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(37, 17);
            this.chkSelectAll.TabIndex = 23;
            this.chkSelectAll.Text = "All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // wbLogin
            // 
            this.wbLogin.Location = new System.Drawing.Point(9, 42);
            this.wbLogin.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbLogin.Name = "wbLogin";
            this.wbLogin.ScrollBarsEnabled = false;
            this.wbLogin.Size = new System.Drawing.Size(659, 20);
            this.wbLogin.TabIndex = 24;
            this.wbLogin.Visible = false;
            this.wbLogin.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbLogin_DocumentCompleted);
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(9, 580);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResponse.Size = new System.Drawing.Size(656, 10);
            this.txtResponse.TabIndex = 25;
            this.txtResponse.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // DocuBundle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(680, 592);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.wbLogin);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimeEnd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimeBegin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNoteTitle);
            this.Controls.Add(this.btnExportSelected);
            this.Controls.Add(this.btnGetNoteList);
            this.Controls.Add(this.lstNoteList);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.btnDirectory);
            this.Controls.Add(this.cboNoteType);
            this.Controls.Add(this.cboRecordType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLoginStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogin);
            this.MaximizeBox = false;
            this.Name = "DocuBundle";
            this.Text = "DocuBundle - Export a bundle of Documents for Selected Notes";
            this.Load += new System.EventHandler(this.DocuBundle_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoginStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboRecordType;
        private System.Windows.Forms.ComboBox cboNoteType;
        private System.Windows.Forms.Button btnDirectory;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.ListView lstNoteList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnGetNoteList;
        private System.Windows.Forms.Button btnExportSelected;
        private System.Windows.Forms.TextBox txtNoteTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimeBegin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimeEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.WebBrowser wbLogin;
        private System.Windows.Forms.TextBox txtResponse;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

