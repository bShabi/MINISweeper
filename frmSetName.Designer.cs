namespace MINISwepper2
{
    partial class frmSetName
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
            this.btnSubmitName = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.lblEnerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSubmitName
            // 
            this.btnSubmitName.Location = new System.Drawing.Point(237, 256);
            this.btnSubmitName.Name = "btnSubmitName";
            this.btnSubmitName.Size = new System.Drawing.Size(193, 81);
            this.btnSubmitName.TabIndex = 0;
            this.btnSubmitName.Text = "Submit";
            this.btnSubmitName.UseVisualStyleBackColor = true;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(237, 137);
            this.textBoxName.Multiline = true;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(193, 85);
            this.textBoxName.TabIndex = 1;
            // 
            // lblEnerName
            // 
            this.lblEnerName.AutoSize = true;
            this.lblEnerName.Font = new System.Drawing.Font("MS Gothic", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnerName.Location = new System.Drawing.Point(175, 61);
            this.lblEnerName.Name = "lblEnerName";
            this.lblEnerName.Size = new System.Drawing.Size(364, 43);
            this.lblEnerName.TabIndex = 2;
            this.lblEnerName.Text = "Enter your name";
            //
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 450);
            this.Controls.Add(this.lblEnerName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.btnSubmitName);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmitName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label lblEnerName;
    }
}