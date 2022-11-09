namespace ViewPort.Views
{
    partial class EngrModeForm
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
            this.EngStateBT = new DevExpress.XtraEditors.SimpleButton();
            this.EngPW = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.EngState = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EngStateBT
            // 
            this.EngStateBT.Location = new System.Drawing.Point(19, 13);
            this.EngStateBT.Name = "EngStateBT";
            this.EngStateBT.Size = new System.Drawing.Size(213, 42);
            this.EngStateBT.TabIndex = 0;
            this.EngStateBT.Text = "Eng\'r Mode ON/OFF";
            this.EngStateBT.Click += new System.EventHandler(this.EngStateBT_Click);
            // 
            // EngPW
            // 
            this.EngPW.Location = new System.Drawing.Point(12, 33);
            this.EngPW.Name = "EngPW";
            this.EngPW.Size = new System.Drawing.Size(254, 21);
            this.EngPW.TabIndex = 1;
            this.EngPW.Text = "Password";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.EngStateBT);
            this.panel1.Location = new System.Drawing.Point(12, 112);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 69);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "State";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "현재상태 :";
            // 
            // EngState
            // 
            this.EngState.AutoSize = true;
            this.EngState.Location = new System.Drawing.Point(80, 71);
            this.EngState.Name = "EngState";
            this.EngState.Size = new System.Drawing.Size(0, 12);
            this.EngState.TabIndex = 5;
            // 
            // EngrModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 193);
            this.Controls.Add(this.EngState);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EngPW);
            this.Controls.Add(this.panel1);
            this.Name = "EngrModeForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton EngStateBT;
        private System.Windows.Forms.TextBox EngPW;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label EngState;
    }
}