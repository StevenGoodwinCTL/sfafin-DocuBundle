﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Salesforce.Common;
using Salesforce.Force;
using System.Dynamic;
using System.Web.Script.Serialization;

namespace DocuBundle
{

    public partial class DocuBundle : Form
    {
        Progress alert;
        private string clientID = "3MVG9fMtCkV6eLhc9gvyskOl9HlyCtxnqmhP3PInmh7zkExpWkPwyJym66bFJUoS6NLZPVhScifwof4GTQQ6Y";
        private string clientSecret = "1865994625115300186";
        private string redirectURL = "DocuBundle:callback";
        private string code = "";
        private TokenResponse token;

        private Dictionary<string, sfdcStaffNote> staffNotesMap = new Dictionary<string, sfdcStaffNote>();


        public DocuBundle()
        {
            InitializeComponent();
        }

        private void btnDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default["lastDirectory"] = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Save(); // Saves settings in application configuration file
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            wbLogin.Visible = true;
            wbLogin.Width = 614;
            wbLogin.Height = 482;
            var authURI = new StringBuilder();
            authURI.Append("https://login.salesforce.com/services/oauth2/authorize?");
            authURI.Append("response_type=code");
            authURI.Append("&client_id=" + clientID);
            authURI.Append("&redirect_uri=" + redirectURL);
            wbLogin.Navigate(authURI.ToString());

        }

        private void wbLogin_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            if (e.Url.AbsoluteUri.StartsWith("docubundle:callback"))
            {
                wbLogin.Visible = false;
                wbLogin.Navigate("about:blank");
                code = e.Url.Query;
                code = code.Substring(6);
                GetToken();
                btnGetNoteList.Enabled = true;
                lstNoteList.Visible = true;
            }

        }
        private void GetToken()
        {
            string URI = "https://login.salesforce.com/services/oauth2/token";
            StringBuilder body = new StringBuilder();
            body.Append("code=" + code + "&");
            body.Append("grant_type=authorization_code&");
            body.Append("client_id=" + clientID + "&");
            body.Append("client_secret=" + clientSecret + "&");
            body.Append("redirect_uri=" + redirectURL);

            string result = HttpPost(URI, body.ToString());
            
            // Convert the JSON response into a token object
            JavaScriptSerializer ser = new JavaScriptSerializer();
            token = ser.Deserialize<TokenResponse>(result);

            // Read the REST resources
            //string s = HttpGet(token.instance_url + @"/services/data/v34.0/", "");
            //UIResults.Text = s;
            //txtResponse.Text = s;
            //txtResponse.Visible = true;
            txtLoginStatus.Text = "Logged in to Production.";

            // set the SOQL as a query param
            //string[] param = new string[1];

            //param[0] = "q";
            //param[1] = "SELECT DeveloperName,Name FROM RecordType WHERE SObjectType = 'Staff_Notes__c'";
            //var requestUri = string.Format("{0}/services/data/v26.0/query/?q={1}, instance_name, soqlQuery)

            // Load Record Types
            string json = HttpGet(token.instance_url + @"/services/data/v34.0/query/?q=SELECT+Id,Name+FROM+RecordType+WHERE+SObjectType+=+'Staff_Notes__c'", "",true);
            txtResponse.Text += Environment.NewLine + Environment.NewLine + "Record Types:" + json;
            sfdcRecordTypeCollection recordTypes = ser.Deserialize<sfdcRecordTypeCollection>(json);
            List<sfdcRecordType> recordTypeList = new List<sfdcRecordType>();
            recordTypeList.Add(new sfdcRecordType("All", "All", null));
            for (var i= 0; i < recordTypes.Records.Count;i++) {
                recordTypeList.Add(new sfdcRecordType(recordTypes.Records[i].Id, recordTypes.Records[i].Name, recordTypes.Records[i].attributes));
            }
            cboRecordType.DataSource = recordTypeList;
            cboRecordType.DisplayMember = "Name";
            cboRecordType.ValueMember = "Id";



        }
        public string HttpGet(string URI, string Parameters, Boolean detectEncoding)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.Method = "GET";
            req.Headers.Add("Authorization: OAuth " + token.access_token);
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), detectEncoding);
            return sr.ReadToEnd().Trim();
        }
        public string HttpPost(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";

            // Add parameters to post
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = data.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(data, 0, data.Length);
            os.Close();

            // Do the post and get the response.
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        private void cboRecordType_SelectedIndexChanged(object sender, EventArgs e)
        {

            setNoteType(cboRecordType.SelectedValue.ToString());

        }
        private void setNoteType(string RecordTypeId)
        {

            // The Api request will be,
            // /services/data/ v23.0/sobjects/ObjectName/describe
            // From the JSON response, i can get the picklist values.

            cboNoteType.Items.Clear();
            if (RecordTypeId == "012j0000000pTNNAA2")
            {
                // Staff Notes Investment Fund
                cboNoteType.Items.Add("All");
                cboNoteType.Items.Add("Performance Review Meeting");
                cboNoteType.Items.Add("Compliance Update");
                cboNoteType.Items.Add("General Meeting");
                cboNoteType.Items.Add("General Notes/Updates");

            }
            else
            {
                cboNoteType.Items.Add("All");
                cboNoteType.Items.Add("Staff Meeting");
                cboNoteType.Items.Add("Director's Meeting");
                cboNoteType.Items.Add("Investment Committee Meeting");
                cboNoteType.Items.Add("Performance Review Meeting");
                cboNoteType.Items.Add("Risk Meeting");
                cboNoteType.Items.Add("Compliance Meeting");
                cboNoteType.Items.Add("Compliance Update");
                cboNoteType.Items.Add("Board of Director's Meeting");
                cboNoteType.Items.Add("Corporate Meeting");
                cboNoteType.Items.Add("Conference");
                cboNoteType.Items.Add("General Meeting");
                cboNoteType.Items.Add("General Notes/Updates");
                cboNoteType.Items.Add("Investment Sector Comments");
                cboNoteType.Items.Add("Project Update");
            }
        }

        private void btnGetNoteList_Click(object sender, EventArgs e)
        {
            // Get a list of notes that meet the criteria selected
            txtLoginStatus.Text = "Getting Notes...";
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string whereClause = "+WHERE+";
            // Record Type Filter
            if (cboRecordType.SelectedValue != null)
            {
                if (cboRecordType.SelectedValue.ToString().Length == 18)
                {
                    if (whereClause != "+WHERE+") whereClause += "+AND+";
                    whereClause += "RecordTypeId+=+'" + cboRecordType.SelectedValue.ToString() + "'";
                }
            }
            // Note Type Filter
            if (cboNoteType.Text != null) { 
                if (cboNoteType.Text.Length > 3)
                {
                    if (whereClause != "+WHERE+") whereClause += "+AND+";
                    whereClause += "Note_Type__c+=+'" + cboNoteType.Text + "'";
                }
            }
            // Note title Fitlter
            if (txtNoteTitle.Text.Length > 0)
            {
                if (whereClause != "+WHERE+") whereClause += "+AND+";
                if (txtNoteTitle.Text.Contains('%')) {
                    whereClause += "Name+Like+'" + txtNoteTitle.Text + "'";
                }
                else
                {
                    whereClause += "Name+Like+'" + txtNoteTitle.Text + "%'";
                }
            }
            // Begin and end Dates
            DateTime dateDefault = new DateTime(2014, 1, 1, 0, 0, 0);
            if (dateTimeBegin.Value > dateDefault)
            {
                string dateTime = dateTimeBegin.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzzzz");
                if (whereClause != "+WHERE+") whereClause += "+AND+";
                whereClause += "Date_Time__c>=" + dateTime + "";

            }
            dateDefault = new DateTime(2200, 1, 1, 0, 0, 0);
            if (dateTimeEnd.Value < dateDefault)
            {
                string dateTime = dateTimeEnd.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzzzz");
                if (whereClause != "+WHERE+") whereClause += "+AND+";
                whereClause += "Date_Time__c<=" + dateTime + "";

            }
            // TODO: Sort Order on column head click
            // TODO: Add Limit and/or use Query More
            // TODO: Query More?
            if (whereClause == "+WHERE+") whereClause = "";
            //TODO: When retrieving selected notes, change the field list to retrieve using either a custom setting or using Metadata API
            string json = HttpGet(token.instance_url + @"/services/data/v34.0/query/?q=SELECT+Id,Name,Note_Type__c,Date_Time__c,Account__r.Name,Additional_Attendees__c,CIM_Attendees__c,Location__c,Location_Other__c,Meeting_Type__c,Notes__c,Notes_I_Main_Points_Summary__c,Notes_II_Organization__c,Notes_II_Other__c,Notes_III_Investment_Process_Strategy__c,Notes_IV_Performance_Market_Conditions__c,Notes_V_Other__c,Project__r.Name,Purpose__c,Team__r.Name,Trust__r.Name+FROM+Staff_Notes__c" +  whereClause, "", true);
            txtResponse.Text += Environment.NewLine + Environment.NewLine + "Staff Notes:" + json;
            // TODO: Load into the list of notes.
            lstNoteList.Items.Clear();
            staffNotesMap.Clear();
            sfdcStaffNoteCollection staffNotes = new sfdcStaffNoteCollection();
            try
            {
                staffNotes = ser.Deserialize<sfdcStaffNoteCollection>(json);
            }
            catch (Exception ex)
            {

            }
            if (staffNotes.Records != null)
            {

                for (var i = 0; i < staffNotes.Records.Count; i++)
                {
                    // Add to List on Form
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = staffNotes.Records[i].Id;
                    lvi.Text = staffNotes.Records[i].Name;
                    ListViewItem.ListViewSubItem lvsiType = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem lvsiDate = new ListViewItem.ListViewSubItem();
                    lvsiType.Text = staffNotes.Records[i].Note_Type__c;
                    lvsiDate.Text = staffNotes.Records[i].Date_Time__c;
                    lvi.SubItems.Add(lvsiType);
                    lvi.SubItems.Add(lvsiDate);
                    lstNoteList.Items.Add(lvi);
                    if (chkSelectAll.Checked)
                    {
                        lvi.Checked = true;
                        btnExportSelected.Enabled = true;
                    }
                    else
                    {
                        lvi.Checked = false;
                        btnExportSelected.Enabled = false;
                    }
                    // Add to Map
                    staffNotesMap.Add(staffNotes.Records[i].Id, staffNotes.Records[i]);
                }
            }

            txtLoginStatus.Text = "DONE - Getting Notes.";

        }

        private void btnExportSelected_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                Dictionary<string, string> lstNoteListMap = new Dictionary<string, string>();
                string directoryName;
                foreach (ListViewItem item in lstNoteList.Items)
                {
                    if (item.Checked)
                    {
                        // Create a sub directory using the name of the note
                        try
                        {
                            directoryName = item.Text.Replace(":", " ");
                            if (directoryName.Length > 248) directoryName = directoryName.Remove(247);
                            System.IO.Directory.CreateDirectory(txtDirectory.Text + "\\" + directoryName);
                        }
                        catch (Exception ex)
                        {
                            //TODO: Report the problem with the selected note
                            directoryName = "";
                        }

                        // map the Id for the note to the directory that the note's attachments will be saved in
                        lstNoteListMap.Add(item.Tag.ToString(), txtDirectory.Text + "\\" + directoryName + "\\");
                    }
                }
                if (lstNoteListMap.Count > 0) {
                    // create a new instance of the alert form
                    alert = new Progress();
                    // event handler for the Cancel button in AlertForm
                    alert.Canceled += new EventHandler<EventArgs>(buttonCancel_Click);
                    alert.TopMost = true;
                    alert.Show();
                    // Start the asynchronous operation.
                    List<object> arguments = new List<object>();
                    arguments.Add(lstNoteListMap);
                    backgroundWorker1.RunWorkerAsync(arguments);
                }
                else
                {
                    txtLoginStatus.Text = "ERROR - Please select at least one Staff Note.";
                }
            }


        }
        // This event handler cancels the backgroundworker, fired from Cancel button in AlertForm.
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
                // Close the AlertForm
                alert.Close();
            }
        }
        private string cleanComma(string noteIn)
        {
            string noteOut = "";
            if (noteIn != null) noteOut = noteIn.Replace("&amp;", "&").Replace("<li>", "-").Replace("</li>", "").Replace("<ul>", "").Replace("</ul>", "").Replace("\"", "'").Replace(",","");
            return noteOut;
        }
        private string cleanNotes(string noteIn)
        {
            string noteOut = "";
            if (noteIn != null) noteOut = noteIn.Replace("&amp;", "&").Replace("<li>","-").Replace("</li>","").Replace("<ul>", "").Replace("</ul>", "").Replace("\"", "'");
            return noteOut;
        }
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            // Loop thru and select/unselect all
            foreach (ListViewItem item in lstNoteList.Items)
            {
                if (chkSelectAll.Checked)
                {
                    item.Checked = true;
                    btnExportSelected.Enabled = true;
                }
                else
                {
                    item.Checked = false;
                    btnExportSelected.Enabled = false;
                }
            }
        }
        private void lstNoteList_SelectedIndexChanged(object sender, ItemCheckEventArgs e)
        {
            btnExportSelected.Enabled = false;
            foreach (ListViewItem item in lstNoteList.Items)
            {
                if (item.Checked)
                {
                    btnExportSelected.Enabled = true;
                    break;
                }
            }

        }

        private void lstNoteList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (lstNoteList.CheckedItems.Count == 1 && e.NewValue == CheckState.Unchecked)
                btnExportSelected.Enabled = false;
            else
                btnExportSelected.Enabled = true;
        }

        private void DocuBundle_Load(object sender, EventArgs e)
        {
            txtDirectory.Text = Properties.Settings.Default["lastDirectory"].ToString();
            txtNoteTitle.Text = Properties.Settings.Default["lastTitle"].ToString(); 
        }

        private void txtNoteTitle_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["lastTitle"] = txtNoteTitle.Text;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> genericlist = e.Argument as List<object>;
            Dictionary<string, string> selectedStaffNote = (Dictionary<string,string>)genericlist[0];

            BackgroundWorker worker = sender as BackgroundWorker;

            // Export selected notes to the selected directory.
            JavaScriptSerializer ser = new JavaScriptSerializer();
            //btnExportSelected.Enabled = false;
            string newLine;
            //TODO: Create a StaffNotes.csv in the txtDirectory.Text directory
            StringBuilder staffNotescsv = new StringBuilder();
            newLine = string.Format("{0},{1},{2},", "Id", "Staff Notes Title", "Directory Name");
            newLine += string.Format("{0},{1},{2},", "Note Type", "Date Time", "Account Name");
            newLine += string.Format("{0},{1},{2},", "Additional Attendees", "CIM Attendees", "Location");
            newLine += string.Format("{0},{1},{2},", "Location Other", "Meeting Type", "Notes");
            newLine += string.Format("{0},{1},{2},", "Notes I Main Points Summary", "Notes II Organization", "Notes II Other");
            newLine += string.Format("{0},{1},{2},", "Notes III Investment Process Strategy", "Notes IV Performance Market Conditions", "Notes V Other");
            newLine += string.Format("{0},{1},{2},", "Project Name", "Purpose", "Team Name");
            newLine += string.Format("{0},{1},{2}{3}", "Trust Name", "Feed Viewer", "Chatter Feed Elements", Environment.NewLine);
            staffNotescsv.Append(newLine);

            //TODO: Create a StaffNotesAttachments.csv in the txtDirectory.Text directory
            StringBuilder staffNotesAttachmentscsv = new StringBuilder();

            newLine = string.Format("{0},{1},{2},{3},{4},{5},", "Note/Attachment", "Id", "Staff Note Id", "Staff Notes Title", "Note Title", "Note Body"); // No Title for Attachments
            newLine += string.Format("{0},{1},{2},{3},{4}", "Attachment Name", "Attachment Directory", "Attachment Full Path", "Attachment Description", "Attachment Content Type"); // These 4 are only for Attachments
            newLine += string.Format("{0}", Environment.NewLine);
            staffNotesAttachmentscsv.Append(newLine);

            string txtLoginStatusText = "EXPORT - Getting Staff Notes...";
            foreach (KeyValuePair<string, string> item in selectedStaffNote)
            {
                // Get Item details from the map
                sfdcStaffNote sn;
                staffNotesMap.TryGetValue(item.Key, out sn);
                // map the Id for the note to the directory that the note's attachments will be saved in
                // Add a line in StaffNotes.csv 
                // TODO: needs to be dynamic using field names on the object
                newLine = string.Format("{0},{1},{2},", item.Key, sn.Name, item.Value);

                newLine += string.Format("{0},{1},{2},", "\"" + sn.Note_Type__c + "\"", sn.Date_Time__c, "\"" + ((sn.Account__r == null) ? "":sn.Account__r.Name) + "\"");
                newLine += string.Format("{0},{1},{2},", "\"" + sn.Additional_Attendees__c + "\"", "\"" + sn.CIM_Attendees__c + "\"", "\"" + sn.Location__c + "\"");
                newLine += string.Format("{0},{1},{2},", "\"" + sn.Location_Other__c + "\"", "\"" + sn.Meeting_Type__c + "\"", "\"" + cleanNotes(sn.Notes__c) + "\"");
                newLine += string.Format("{0},{1},{2},", "\"" + cleanNotes(sn.Notes_I_Main_Points_Summary__c) + "\"", "\"" + cleanNotes(sn.Notes_II_Organization__c) + "\"", "\"" + cleanNotes(sn.Notes_II_Other__c) + "\"");
                newLine += string.Format("{0},{1},{2},", "\"" + cleanNotes(sn.Notes_III_Investment_Process_Strategy__c) + "\"", "\"" + cleanNotes(sn.Notes_IV_Performance_Market_Conditions__c) + "\"", "\"" + cleanNotes(sn.Notes_V_Other__c) + "\"");
                newLine += string.Format("{0},{1},{2},", "\"" + ((sn.Project__r == null) ? "" : sn.Project__r.Name )+ "\"", "\"" + sn.Purpose__c + "\"", "\"" + ((sn.Team__r == null) ? "" : sn.Team__r.Name ) + "\"");

                // Get Chatter feed elements for the selected staff Note
                string feedElements = HttpGet(token.instance_url + @"/services/data/v35.0/chatter/feeds/record/" + item.Key + "/feed-elements", "", true);
                //txtResponse.Text += Environment.NewLine + Environment.NewLine + "Feed Elements for " + sn.Name + ":" + feedElements;

                newLine += string.Format("{0},{1},{2}", "\"" + ((sn.Trust__r == null) ? "" : sn.Trust__r.Name) + "\"", "Open in http://jsonlint.com/:", "\"" + feedElements.Replace("\"", "'") + "\"");
                newLine += string.Format("{0}", Environment.NewLine);

                staffNotescsv.Append(newLine);
            }

            txtLoginStatusText = "EXPORT - Getting Notes...";

            // Get the Notes and Attachments for the selected staff notes
            string whereClause = "+WHERE+ParentId+IN(";
            foreach (KeyValuePair<string, string> kvp in selectedStaffNote)
            {
                if (whereClause != "+WHERE+ParentId+IN(") whereClause += ",";
                whereClause += "'" + kvp.Key + "'";
            }
            whereClause += ")";
            // Get Notes
            string json = HttpGet(token.instance_url + @"/services/data/v34.0/query/?q=SELECT+Id,ParentId,Title,Body+FROM+Note" + whereClause, "", true);
            //txtResponse.Text += Environment.NewLine + Environment.NewLine + "Notes:" + json;
            sfdcNoteCollection notes = new sfdcNoteCollection();
            try
            {
                notes = ser.Deserialize<sfdcNoteCollection>(json);
            }
            catch (Exception ex)
            {
            }
            if (notes.Records != null)
            {
                for (var i = 0; i < notes.Records.Count; i++)
                {
                    // Get Item details from the map
                    sfdcStaffNote sn;
                    staffNotesMap.TryGetValue(notes.Records[i].ParentId, out sn);
                    // Add to csv output
                    txtLoginStatusText = "EXPORT - Getting Notes..." + sn.Name;
                    newLine = string.Format("{0},{1},{2},{3},{4},{5},", "Note", notes.Records[i].Id, notes.Records[i].ParentId, "\"" + cleanComma(sn.Name) + "\"", "\"" + cleanComma(notes.Records[i].Title) + "\"", "\"" + cleanComma(notes.Records[i].Body) + "\"");
                    newLine += string.Format("{0},{1},{2},{3},{4}", "", "", "", "", "");
                    newLine += string.Format("{0}", Environment.NewLine);
                    staffNotesAttachmentscsv.Append(newLine);
                }
            }
            txtLoginStatusText = "EXPORT - Getting Attachments...";
            // Get Attachments
            json = HttpGet(token.instance_url + @"/services/data/v34.0/query/?q=SELECT+Id,ParentId,Name,Body,Description,ContentType+FROM+Attachment" + whereClause, "", true);
            //txtResponse.Text += Environment.NewLine + Environment.NewLine + " Attachments:" + json;
            sfdcAttachmentCollection attachments = new sfdcAttachmentCollection();
            try
            {
                attachments = ser.Deserialize<sfdcAttachmentCollection>(json);
            }
            catch (Exception ex)
            {
            }
            int totalRecords = 0;
            int currentRecord = 0;
            if (attachments.Records != null)
            {
                totalRecords = attachments.Records.Count;
            }
            // Get Chatter Content attachments
            txtLoginStatusText = "EXPORT - Getting Attached Contents...";
            whereClause = whereClause.Replace("ParentId", "LinkedEntityId");
            json = HttpGet(token.instance_url + @"/services/data/v34.0/query/?q=SELECT+Id,LinkedEntityId,ContentDocumentId,ContentDocument.Title,ContentDocument.FileExtension,ContentDocument.LatestPublishedVersionId,ContentDocument.Description+FROM+ContentDocumentLink" + whereClause, "", true);
            //txtResponse.Text += Environment.NewLine + Environment.NewLine + " Content/Chatter:" + json;
            sfdcContentCollection contentAttachments = new sfdcContentCollection();
            try
            {
                contentAttachments = ser.Deserialize<sfdcContentCollection>(json);
            }
            catch (Exception ex)
            {
            }
            if (contentAttachments.Records != null)
            {
                totalRecords += contentAttachments.Records.Count;
            }
            if (attachments.Records != null)
            {
                for (var i = 0; i < attachments.Records.Count; i++)
                {
                    currentRecord += 1;
                    // Get Item details from the map
                    sfdcStaffNote sn;
                    staffNotesMap.TryGetValue(attachments.Records[i].ParentId, out sn);
                    // Add to csv output
                    string stafNoteDirectory;
                    selectedStaffNote.TryGetValue(attachments.Records[i].ParentId, out stafNoteDirectory);
                    txtLoginStatusText = "EXPORT - Getting Attachments..." + attachments.Records[i].Name;
                    newLine = string.Format("{0},{1},{2},{3},{4},{5},", "Attachment", attachments.Records[i].Id, attachments.Records[i].ParentId, "\"" + sn.Name + "\"", "", "");
                    newLine += string.Format("{0},{1},{2},{3},{4}", cleanComma(attachments.Records[i].Name), stafNoteDirectory, stafNoteDirectory + cleanComma(attachments.Records[i].Name), cleanComma(attachments.Records[i].Description), attachments.Records[i].ContentType);
                    newLine += string.Format("{0}", Environment.NewLine);
                    staffNotesAttachmentscsv.Append(newLine);
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        int progress = (int)((double)currentRecord * 100 / (double)totalRecords);
                        worker.ReportProgress(progress);
                    }   
                    // Get the Attachment Body
                    // Save it to the Staff Note Directory
                    string attachedDocument = HttpGet(token.instance_url + attachments.Records[i].Body, "", false);
                    System.IO.File.WriteAllText(stafNoteDirectory + cleanComma(attachments.Records[i].Name), attachedDocument);

                }
            }

            if (contentAttachments.Records != null)
            {
                for (var i = 0; i < contentAttachments.Records.Count; i++)
                {
                    currentRecord += 1;
                    // Get Item details from the map
                    sfdcStaffNote sn;
                    staffNotesMap.TryGetValue(contentAttachments.Records[i].LinkedEntityId, out sn);
                    // Add to csv output
                    string stafNoteDirectory;
                    selectedStaffNote.TryGetValue(contentAttachments.Records[i].LinkedEntityId, out stafNoteDirectory);
                    // Get Document Details 
                    //sfdcContentDocument contentDocument = ser.Deserialize<sfdcContentDocument>(contentAttachments.Records[i].ContentDocument);
                    txtLoginStatusText = "EXPORT - Getting Attached Contents..." + contentAttachments.Records[i].ContentDocument.Title;
                    newLine = string.Format("{0},{1},{2},{3},{4},{5},", "Chatter File/Content", contentAttachments.Records[i].Id, contentAttachments.Records[i].LinkedEntityId, "\"" + sn.Name + "\"", "", "");
                    newLine += string.Format("{0},{1},{2},{3},{4}", cleanComma(contentAttachments.Records[i].ContentDocument.Title), stafNoteDirectory, stafNoteDirectory + cleanNotes(contentAttachments.Records[i].ContentDocument.Title) + "." + contentAttachments.Records[i].ContentDocument.FileExtension, cleanComma(contentAttachments.Records[i].ContentDocument.Description), contentAttachments.Records[i].ContentDocument.FileExtension);
                    newLine += string.Format("{0}", Environment.NewLine);
                    staffNotesAttachmentscsv.Append(newLine);
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        int progress = (int)((double)currentRecord * 100 / (double)totalRecords);
                        //worker.ReportProgress(50);
                        worker.ReportProgress(progress);
                    }
                    // Get the Attachment  
                    string contentDocument = HttpGet(token.instance_url + @"/services/data/v34.0/sobjects/ContentVersion/" + contentAttachments.Records[i].ContentDocument.LatestPublishedVersionId + "/VersionData", "", false);
                    // Save it to the Staff Note Directory
                    System.IO.File.WriteAllText(stafNoteDirectory + cleanComma(contentAttachments.Records[i].ContentDocument.Title) + "." + contentAttachments.Records[i].ContentDocument.FileExtension, contentDocument);

                }
            }

            //Get a Content Document
            //string contentDocument = HttpGet(token.instance_url + @"/chatter/files/" + @"069j0000000pg3eAAA/body", "");
            //string contentDocument = HttpGet(token.instance_url + @"/services/data/v20.0/sobjects/Document/" + @"069j0000000pg3eAAA/body", "");

            //string contentDocument = HttpGet(token.instance_url + @"/services/data/v34.0/sobjects/ContentDocumentLink/06Aj0000000R0lVEAS", "");
            // Below line retrieves the document using the latest version ID retrieved from the Get Content Attachments 
            // THIS WORKS BUT IN PPTX format too long - string contentDocumentBig = HttpGet(token.instance_url + @"/services/data/v34.0/sobjects/ContentVersion/068j0000000ps8FAAQ/VersionData", "", false);
            // THIS WORKS FOR A SMALL TEXT DOCUMENT string contentDocument = HttpGet(token.instance_url + @"/services/data/v34.0/sobjects/ContentVersion/068j0000001OlzeAAC/VersionData", "", false);

            // THIS WORKS FOR A SMALL TEXT DOCUMENT System.IO.File.WriteAllText(txtDirectory.Text + "\\SteveTest.txt", contentDocument);
            //SELECT FileExtension  FROM ContentVersion
            // this only gets a link to the document  string contentDocumentSOQL = HttpGet(token.instance_url + @"/services/data/v34.0/query/?q=SELECT+VersionData+FROM+ContentVersion+where+Id='068j0000001OlzeAAC'", "",true);
            //string contentDocument = HttpGet(token.instance_url + @"/services/data/v34.0/query/?q=SELECT+VersionData+FROM+ContentVersion+where+ContentDocumentId='069j0000000pg6J'", "");
            //var response = contentDocument
            //    .Replace(@"-", "+").Replace(@"_", "/");
            //byte[] contentDocumentArray = Convert.FromBase64String(response);

            // BELOW WORKS PERFECTLY - Get the below document using a force.com explorer SOQL query to get VersionData from ContentVerstion and paste into the text string
            //byte[] contentDocumentArray = Convert.FromBase64String(contentDocument);
            //System.IO.File.WriteAllBytes(txtDirectory.Text + "\\StaffNotesxxx.pptx", contentDocumentArray);
            //txtResponse.Text += " contentDocument:" + contentDocument;

            txtLoginStatusText = "EXPORT - Creating csv files...";

            // Create the Staff Notes CSV file
            System.IO.File.WriteAllText(txtDirectory.Text + "\\StaffNotes.csv", staffNotescsv.ToString());
            // Create the Staff Notes Attachments CSV file
            System.IO.File.WriteAllText(txtDirectory.Text + "\\StaffNotes - Notes and Attachments.csv", staffNotesAttachmentscsv.ToString());
            btnExportSelected.Enabled = true;
            txtLoginStatusText = "EXPORT - DONE!";

        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Show the progress in main form (GUI)
            txtLoginStatus.Text = (e.ProgressPercentage.ToString() + "%");
            // Pass the progress to AlertForm label and progressbar
            alert.Message = "In progress, please wait... " + e.ProgressPercentage.ToString() + "%";
            alert.ProgressValue = e.ProgressPercentage;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                txtLoginStatus.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                txtLoginStatus.Text = "Error: " + e.Error.Message;
            }
            else
            {
                txtLoginStatus.Text = "Done!";
            }
            // Close the AlertForm
            alert.Close();
        }

        private void txtDirectory_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["lastDirectory"] = txtDirectory.Text;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file

        }
    }
    public class TokenResponse {
        public string id { get; set; }
        public string issued_at { get; set; }
        public string refresh_token { get; set; }
        public string instance_url { get; set; }
        public string signature { get; set; }
        public string access_token { get; set; }

    }
    // Classes used to retrieve Staff Note Information
    public class sfdcStaffNoteCollection
    {
        public bool Done { get; set; }
        public int TotalSize { get; set; }
        public string nextRecordsUrl { get; set; }
        public List<sfdcStaffNote> Records { get; set; }

    }
    public class sfdcStaffNote
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Note_Type__c { get; set; }
        public string Date_Time__c { get; set; }
        public sfdcAccount Account__r { get; set; }
        public string Additional_Attendees__c { get; set; }
        public string CIM_Attendees__c { get; set; }
        public string Location__c { get; set; }
        public string Location_Other__c { get; set; }
        public string Meeting_Type__c { get; set; }
        public string Notes__c { get; set; }
        public string Notes_I_Main_Points_Summary__c { get; set; }
        public string Notes_II_Organization__c { get; set; }
        public string Notes_II_Other__c { get; set; }
        public string Notes_III_Investment_Process_Strategy__c { get; set; }
        public string Notes_IV_Performance_Market_Conditions__c { get; set; }
        public string Notes_V_Other__c { get; set; }
        public sfdcProject Project__r { get; set; }
        public string Purpose__c { get; set; }
        public sfdcTeam Team__r { get; set; }
        public sfdcTrust Trust__r { get; set; }
        public sfdcStaffNote()
        { 
        }
        public sfdcStaffNote(string i,string n, string nt, string dt, sfdcAttributes a)
        {
            Id = i;
            Name = n;
            if (nt != null) Note_Type__c = nt;
            if (dt != null) Date_Time__c = dt;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }
    }
    public class sfdcAccount
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public sfdcAccount()
        {
        }
        public sfdcAccount(string i, sfdcAttributes a)
        {
            Id = i;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }
    }
    public class sfdcProject
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public sfdcProject()
        {
        }
        public sfdcProject(string i, sfdcAttributes a)
        {
            Id = i;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }
    }
    public class sfdcTeam
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public sfdcTeam()
        {
        }
        public sfdcTeam(string i, sfdcAttributes a)
        {
            Id = i;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }

    }
    public class sfdcTrust
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public sfdcTrust()
        {
        }
        public sfdcTrust(string i, sfdcAttributes a)
        {
            Id = i;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }

    }
    // Classes used to retrieve Note Information
    public class sfdcNoteCollection
    {
        public bool Done { get; set; }
        public int TotalSize { get; set; }
        public string nextRecordsUrl { get; set; }
        public List<sfdcNote> Records { get; set; }

    }
    public class sfdcNote
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public sfdcNote()
        {
        }
        public sfdcNote(string i, sfdcAttributes a)
        {
            Id = i;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }
    }
    // Classes used to retrieve Content Information (Added in Chatter Feed)
    public class sfdcContentCollection
    {
        public bool Done { get; set; }
        public int TotalSize { get; set; }
        public string nextRecordsUrl { get; set; }
        public List<sfdcContent> Records { get; set; }

    }
    public class sfdcContent
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string LinkedEntityId { get; set; }
        public sfdcContentDocument ContentDocument { get; set; }
        public sfdcContent()
        {
        }
        public sfdcContent(string i, sfdcAttributes a)
        {
            Id = i;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }
    }
    public class sfdcContentDocument
    {
        public sfdcAttributes attributes { get; set; }

        public string Id { get; set; }
        public string Title { get; set; }
        public string FileExtension { get; set; }
        public string LatestPublishedVersionId { get; set; }
        public string Description { get; set; }
        public sfdcContentDocument()
        {
        }
        public sfdcContentDocument(string i, sfdcAttributes a)
        {
            Id = i;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }

    }
    // Classes used to retrieve Attachment Information
    public class sfdcAttachmentCollection
    {
        public bool Done { get; set; }
        public int TotalSize { get; set; }
        public string nextRecordsUrl { get; set; }
        public List<sfdcAttachment> Records { get; set; }
    }
    public class sfdcAttachment
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public sfdcAttachment()
        {
        }
        public sfdcAttachment(string i, sfdcAttributes a)
        {
            Id = i;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }
    }
    public class sfdcRecordTypeCollection{
        public bool Done { get; set; }
        public int TotalSize { get; set; }
        public string nextRecordsUrl { get; set; }
        public List<sfdcRecordType> Records { get; set; }

    }
    // Classes used to retrieve Record Type Information
    public class sfdcRecordType
    {
        public sfdcAttributes attributes { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public sfdcRecordType()
        {

        }

        public sfdcRecordType(string i, string n, sfdcAttributes a)
        {
            Id = i;
            Name = n;
            if (a != null)
            {
                attributes = new sfdcAttributes();
                attributes.Type = a.Type;
                attributes.Url = a.Url;
            }
        }
    }
    // Generic Classes used to retrieve records from salesforce
    public class sfdcAttributes
    {
        public string Type { get; set; }
        public string Url { get; set; }
    }
}