namespace ViewPort.Views
{
    partial class ViewModePassword
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PSW_Input_TB = new System.Windows.Forms.TextBox();
            this.PSW_Check_BT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "ViewMode 로드시에는 비밀번호 입력 부탁드립니다.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "비밀번호 :";
            // 
            // PSW_Input_TB
            // 
            this.PSW_Input_TB.Location = new System.Drawing.Point(94, 68);
            this.PSW_Input_TB.Name = "PSW_Input_TB";
            this.PSW_Input_TB.PasswordChar = '*';
            this.PSW_Input_TB.Size = new System.Drawing.Size(100, 21);
            this.PSW_Input_TB.TabIndex = 2;
            // 
            // PSW_Check_BT
            // 
            this.PSW_Check_BT.Location = new System.Drawing.Point(224, 66);
            this.PSW_Check_BT.Name = "PSW_Check_BT";
            this.PSW_Check_BT.Size = new System.Drawing.Size(75, 23);
            this.PSW_Check_BT.TabIndex = 3;
            this.PSW_Check_BT.Text = "확인";
            this.PSW_Check_BT.UseVisualStyleBackColor = true;
            this.PSW_Check_BT.Click += new System.EventHandler(this.PSW_Check_BT_Click);
            // 
            // ViewModePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 111);
            this.Controls.Add(this.PSW_Check_BT);
            this.Controls.Add(this.PSW_Input_TB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ViewModePassword";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox PSW_Input_TB;
        private System.Windows.Forms.Button PSW_Check_BT;
    }
}