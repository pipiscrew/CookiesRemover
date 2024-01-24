namespace CookiesRemover
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnFirefox = new System.Windows.Forms.Button();
            this.btnChromeDisableSW = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDefaultWebProtocol = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Image = global::CookiesRemover.Properties.Resources.chromium48;
            this.button1.Location = new System.Drawing.Point(24, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 62);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnFirefox
            // 
            this.btnFirefox.Image = global::CookiesRemover.Properties.Resources.firefox48;
            this.btnFirefox.Location = new System.Drawing.Point(24, 110);
            this.btnFirefox.Name = "btnFirefox";
            this.btnFirefox.Size = new System.Drawing.Size(96, 62);
            this.btnFirefox.TabIndex = 0;
            this.btnFirefox.UseVisualStyleBackColor = true;
            this.btnFirefox.Click += new System.EventHandler(this.btnFirefox_Click);
            // 
            // btnChromeDisableSW
            // 
            this.btnChromeDisableSW.AllowDrop = true;
            this.btnChromeDisableSW.Location = new System.Drawing.Point(126, 12);
            this.btnChromeDisableSW.Name = "btnChromeDisableSW";
            this.btnChromeDisableSW.Size = new System.Drawing.Size(133, 31);
            this.btnChromeDisableSW.TabIndex = 2;
            this.btnChromeDisableSW.Text = "disable service worker";
            this.btnChromeDisableSW.UseVisualStyleBackColor = true;
            this.btnChromeDisableSW.Click += new System.EventHandler(this.btnChromeDisableSW_Click);
            this.btnChromeDisableSW.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnChromeDisableSW_DragDrop);
            this.btnChromeDisableSW.DragEnter += new System.Windows.Forms.DragEventHandler(this.btnChromeDisableSW_DragEnter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDefaultWebProtocol);
            this.groupBox1.Location = new System.Drawing.Point(24, 188);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "windows > settings > default protocols";
            // 
            // btnDefaultWebProtocol
            // 
            this.btnDefaultWebProtocol.AllowDrop = true;
            this.btnDefaultWebProtocol.Location = new System.Drawing.Point(6, 39);
            this.btnDefaultWebProtocol.Name = "btnDefaultWebProtocol";
            this.btnDefaultWebProtocol.Size = new System.Drawing.Size(219, 40);
            this.btnDefaultWebProtocol.TabIndex = 0;
            this.btnDefaultWebProtocol.Text = " add browser to default web browsers ";
            this.btnDefaultWebProtocol.UseVisualStyleBackColor = true;
            this.btnDefaultWebProtocol.Click += new System.EventHandler(this.btnDefaultWebProtocol_Click);
            this.btnDefaultWebProtocol.DragDrop += new System.Windows.Forms.DragEventHandler(this.btnDefaultWebProtocol_DragDrop);
            this.btnDefaultWebProtocol.DragEnter += new System.Windows.Forms.DragEventHandler(this.btnDefaultWebProtocol_DragEnter);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 320);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnChromeDisableSW);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnFirefox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cookies Remover";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFirefox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnChromeDisableSW;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDefaultWebProtocol;
    }
}

