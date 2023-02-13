
namespace Calendar
{
    partial class EditCustomRemoteCommandsDialog
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
            this.okbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.customRemoteSaveCommandEditBox = new System.Windows.Forms.TextBox();
            this.customRemoteLoadCommandEditBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okbutton
            // 
            this.okbutton.Location = new System.Drawing.Point(327, 94);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(75, 23);
            this.okbutton.TabIndex = 0;
            this.okbutton.Text = "OK";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Custom remote save command:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Custom remote load command:";
            // 
            // customRemoteSaveCommandEditBox
            // 
            this.customRemoteSaveCommandEditBox.Location = new System.Drawing.Point(176, 23);
            this.customRemoteSaveCommandEditBox.Name = "customRemoteSaveCommandEditBox";
            this.customRemoteSaveCommandEditBox.Size = new System.Drawing.Size(515, 20);
            this.customRemoteSaveCommandEditBox.TabIndex = 3;
            // 
            // customRemoteLoadCommandEditBox
            // 
            this.customRemoteLoadCommandEditBox.Location = new System.Drawing.Point(176, 53);
            this.customRemoteLoadCommandEditBox.Name = "customRemoteLoadCommandEditBox";
            this.customRemoteLoadCommandEditBox.Size = new System.Drawing.Size(515, 20);
            this.customRemoteLoadCommandEditBox.TabIndex = 4;
            // 
            // EditCustomRemoteCommandsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 129);
            this.Controls.Add(this.customRemoteLoadCommandEditBox);
            this.Controls.Add(this.customRemoteSaveCommandEditBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okbutton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditCustomRemoteCommandsDialog";
            this.Text = "Custom Remote Commands";
            this.Load += new System.EventHandler(this.EditCustomRemoteCommandsDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox customRemoteSaveCommandEditBox;
        private System.Windows.Forms.TextBox customRemoteLoadCommandEditBox;
    }
}