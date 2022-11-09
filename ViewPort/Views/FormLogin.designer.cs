namespace SDIP.Forms
{
    partial class FormLogin
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
            this.TB_ID = new System.Windows.Forms.TextBox();
            this.TB_PASSWORD = new System.Windows.Forms.TextBox();
            this.BTN_LOGIN = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // TB_ID
            // 
            this.TB_ID.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TB_ID.Location = new System.Drawing.Point(23, 89);
            this.TB_ID.Name = "TB_ID";
            this.TB_ID.Size = new System.Drawing.Size(182, 29);
            this.TB_ID.TabIndex = 0;
            // 
            // TB_PASSWORD
            // 
            this.TB_PASSWORD.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TB_PASSWORD.Location = new System.Drawing.Point(23, 124);
            this.TB_PASSWORD.Name = "TB_PASSWORD";
            this.TB_PASSWORD.Size = new System.Drawing.Size(182, 29);
            this.TB_PASSWORD.TabIndex = 1;
            this.TB_PASSWORD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_PASSWORD_KeyDown);
            // 
            // BTN_LOGIN
            // 
            this.BTN_LOGIN.Location = new System.Drawing.Point(211, 89);
            this.BTN_LOGIN.Name = "BTN_LOGIN";
            this.BTN_LOGIN.Size = new System.Drawing.Size(103, 64);
            this.BTN_LOGIN.TabIndex = 2;
            this.BTN_LOGIN.Text = "LOGIN";
            this.BTN_LOGIN.Click += new System.EventHandler(this.BTN_LOGIN_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 169);
            this.Controls.Add(this.BTN_LOGIN);
            this.Controls.Add(this.TB_PASSWORD);
            this.Controls.Add(this.TB_ID);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Movable = false;
            this.Name = "FormLogin";
            this.Resizable = false;
            this.Text = "Carlo SDIP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TB_ID;
        private System.Windows.Forms.TextBox TB_PASSWORD;
        private MetroFramework.Controls.MetroTile BTN_LOGIN;
    }
}