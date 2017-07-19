namespace HotelManagement
{
    partial class Services
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
            this.AcceptServicesButton = new System.Windows.Forms.Button();
            this.ResultLabel = new System.Windows.Forms.Label();
            this.ServicesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.SelectServircesLabel = new System.Windows.Forms.Label();
            this.ServiceLenLabel = new System.Windows.Forms.Label();
            this.ServicesIdListBox = new System.Windows.Forms.ListBox();
            this.ResultListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // AcceptServicesButton
            // 
            this.AcceptServicesButton.Location = new System.Drawing.Point(-2, 154);
            this.AcceptServicesButton.Name = "AcceptServicesButton";
            this.AcceptServicesButton.Size = new System.Drawing.Size(75, 23);
            this.AcceptServicesButton.TabIndex = 1;
            this.AcceptServicesButton.Text = "OK";
            this.AcceptServicesButton.UseVisualStyleBackColor = true;
            this.AcceptServicesButton.Click += new System.EventHandler(this.AcceptServicesButton_Click);
            // 
            // ResultLabel
            // 
            this.ResultLabel.AutoSize = true;
            this.ResultLabel.Location = new System.Drawing.Point(32, 210);
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Size = new System.Drawing.Size(55, 13);
            this.ResultLabel.TabIndex = 2;
            this.ResultLabel.Text = "00000000";
            this.ResultLabel.Visible = false;
            // 
            // ServicesCheckedListBox
            // 
            this.ServicesCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            this.ServicesCheckedListBox.CheckOnClick = true;
            this.ServicesCheckedListBox.FormattingEnabled = true;
            this.ServicesCheckedListBox.Location = new System.Drawing.Point(12, 24);
            this.ServicesCheckedListBox.Name = "ServicesCheckedListBox";
            this.ServicesCheckedListBox.Size = new System.Drawing.Size(147, 124);
            this.ServicesCheckedListBox.TabIndex = 3;
            // 
            // SelectServircesLabel
            // 
            this.SelectServircesLabel.AutoSize = true;
            this.SelectServircesLabel.Location = new System.Drawing.Point(12, 8);
            this.SelectServircesLabel.Name = "SelectServircesLabel";
            this.SelectServircesLabel.Size = new System.Drawing.Size(84, 13);
            this.SelectServircesLabel.TabIndex = 4;
            this.SelectServircesLabel.Text = "Select Services:";
            // 
            // ServiceLenLabel
            // 
            this.ServiceLenLabel.AutoSize = true;
            this.ServiceLenLabel.Location = new System.Drawing.Point(12, 151);
            this.ServiceLenLabel.Name = "ServiceLenLabel";
            this.ServiceLenLabel.Size = new System.Drawing.Size(0, 13);
            this.ServiceLenLabel.TabIndex = 5;
            this.ServiceLenLabel.Visible = false;
            // 
            // ServicesIdListBox
            // 
            this.ServicesIdListBox.FormattingEnabled = true;
            this.ServicesIdListBox.Location = new System.Drawing.Point(200, 24);
            this.ServicesIdListBox.Name = "ServicesIdListBox";
            this.ServicesIdListBox.Size = new System.Drawing.Size(120, 95);
            this.ServicesIdListBox.TabIndex = 6;
            this.ServicesIdListBox.Visible = false;
            // 
            // ResultListBox
            // 
            this.ResultListBox.FormattingEnabled = true;
            this.ResultListBox.Location = new System.Drawing.Point(337, 24);
            this.ResultListBox.Name = "ResultListBox";
            this.ResultListBox.Size = new System.Drawing.Size(120, 95);
            this.ResultListBox.TabIndex = 7;
            this.ResultListBox.Visible = false;
            // 
            // Services
            // 
            this.AcceptButton = this.AcceptServicesButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(175, 183);
            this.Controls.Add(this.ResultListBox);
            this.Controls.Add(this.ServicesIdListBox);
            this.Controls.Add(this.ServiceLenLabel);
            this.Controls.Add(this.SelectServircesLabel);
            this.Controls.Add(this.ServicesCheckedListBox);
            this.Controls.Add(this.ResultLabel);
            this.Controls.Add(this.AcceptServicesButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Services";
            this.Text = "Services";
            this.Load += new System.EventHandler(this.Services_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button AcceptServicesButton;
        private System.Windows.Forms.Label ResultLabel;
        private System.Windows.Forms.CheckedListBox ServicesCheckedListBox;
        private System.Windows.Forms.Label SelectServircesLabel;
        private System.Windows.Forms.Label ServiceLenLabel;
        private System.Windows.Forms.ListBox ServicesIdListBox;
        private System.Windows.Forms.ListBox ResultListBox;
    }
}