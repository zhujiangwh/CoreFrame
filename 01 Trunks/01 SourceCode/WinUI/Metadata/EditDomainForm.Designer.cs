namespace Core.Metadata
{
    partial class EditDomainForm
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
            this.editDomainControl1 = new Core.Metadata.EditDomainControl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // editDomainControl1
            // 
            this.editDomainControl1.EditedObject = typeof(Core.Metadata.Domain);
            this.editDomainControl1.Location = new System.Drawing.Point(185, 12);
            this.editDomainControl1.Name = "editDomainControl1";
            this.editDomainControl1.Size = new System.Drawing.Size(511, 423);
            this.editDomainControl1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EditDomainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 437);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.editDomainControl1);
            this.Name = "EditDomainForm";
            this.Text = "EditDomainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private EditDomainControl editDomainControl1;
        private System.Windows.Forms.Button button1;
    }
}