﻿
namespace tcp_client_gui
{
    partial class FormChatServer
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
            this.lblIP = new System.Windows.Forms.Label();
            this.listchat = new System.Windows.Forms.ListBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIP.Location = new System.Drawing.Point(12, 67);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(72, 36);
            this.lblIP.TabIndex = 13;
            this.lblIP.Text = "IP : ";
            // 
            // listchat
            // 
            this.listchat.FormattingEnabled = true;
            this.listchat.ItemHeight = 16;
            this.listchat.Location = new System.Drawing.Point(12, 160);
            this.listchat.Name = "listchat";
            this.listchat.Size = new System.Drawing.Size(745, 244);
            this.listchat.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(108, 36);
            this.lblTitle.TabIndex = 11;
            this.lblTitle.Text = "Server";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(331, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(15, 118);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(310, 22);
            this.txtMsg.TabIndex = 9;
            // 
            // FormChatServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 443);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.listchat);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtMsg);
            this.Name = "FormChatServer";
            this.Text = "FormChatServer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormChatServer_FormClosed);
            this.Load += new System.EventHandler(this.FormChatServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.ListBox listchat;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtMsg;
    }
}