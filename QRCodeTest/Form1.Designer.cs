namespace QRCodeTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pbQRCode = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDec = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbContent = new System.Windows.Forms.TextBox();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPixel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbLogoSize = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbLogoBord = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbError = new System.Windows.Forms.ComboBox();
            this.rbBord = new System.Windows.Forms.RadioButton();
            this.rbNotBord = new System.Windows.Forms.RadioButton();
            this.backColor = new System.Windows.Forms.ColorDialog();
            this.foreColor = new System.Windows.Forms.ColorDialog();
            this.btBack = new System.Windows.Forms.Button();
            this.btFore = new System.Windows.Forms.Button();
            this.logoFile = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbQRCode
            // 
            this.pbQRCode.Location = new System.Drawing.Point(12, 12);
            this.pbQRCode.Name = "pbQRCode";
            this.pbQRCode.Size = new System.Drawing.Size(306, 312);
            this.pbQRCode.TabIndex = 0;
            this.pbQRCode.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 353);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "解析后的文本：";
            // 
            // tbDec
            // 
            this.tbDec.Enabled = false;
            this.tbDec.Location = new System.Drawing.Point(131, 353);
            this.tbDec.Name = "tbDec";
            this.tbDec.Size = new System.Drawing.Size(187, 21);
            this.tbDec.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 404);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "待生成正文：";
            // 
            // tbContent
            // 
            this.tbContent.Location = new System.Drawing.Point(131, 404);
            this.tbContent.Name = "tbContent";
            this.tbContent.Size = new System.Drawing.Size(187, 21);
            this.tbContent.TabIndex = 2;
            this.tbContent.Text = "https://www.baidu.com/";
            // 
            // pbLogo
            // 
            this.pbLogo.Location = new System.Drawing.Point(380, 12);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(198, 174);
            this.pbLogo.TabIndex = 3;
            this.pbLogo.TabStop = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(619, 163);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.Text = "上传logo";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(407, 312);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "版本：";
            // 
            // tbPixel
            // 
            this.tbPixel.Location = new System.Drawing.Point(515, 268);
            this.tbPixel.Name = "tbPixel";
            this.tbPixel.Size = new System.Drawing.Size(63, 21);
            this.tbPixel.TabIndex = 7;
            this.tbPixel.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(407, 277);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "二维码像素大小：";
            // 
            // tbVersion
            // 
            this.tbVersion.Location = new System.Drawing.Point(515, 303);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.Size = new System.Drawing.Size(63, 21);
            this.tbVersion.TabIndex = 7;
            this.tbVersion.Text = "5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(584, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "logo大小：";
            // 
            // tbLogoSize
            // 
            this.tbLogoSize.Location = new System.Drawing.Point(674, 78);
            this.tbLogoSize.Name = "tbLogoSize";
            this.tbLogoSize.Size = new System.Drawing.Size(100, 21);
            this.tbLogoSize.TabIndex = 7;
            this.tbLogoSize.Text = "15";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(584, 125);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "logo边框大小：";
            // 
            // tbLogoBord
            // 
            this.tbLogoBord.Location = new System.Drawing.Point(674, 122);
            this.tbLogoBord.Name = "tbLogoBord";
            this.tbLogoBord.Size = new System.Drawing.Size(100, 21);
            this.tbLogoBord.TabIndex = 7;
            this.tbLogoBord.Text = "5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(407, 231);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "容错率：";
            // 
            // cbError
            // 
            this.cbError.FormattingEnabled = true;
            this.cbError.Items.AddRange(new object[] {
            "L",
            "M",
            "Q",
            "H"});
            this.cbError.Location = new System.Drawing.Point(515, 231);
            this.cbError.Name = "cbError";
            this.cbError.Size = new System.Drawing.Size(63, 20);
            this.cbError.TabIndex = 9;
            this.cbError.Text = "L";
            // 
            // rbBord
            // 
            this.rbBord.AutoSize = true;
            this.rbBord.Location = new System.Drawing.Point(418, 353);
            this.rbBord.Name = "rbBord";
            this.rbBord.Size = new System.Drawing.Size(59, 16);
            this.rbBord.TabIndex = 10;
            this.rbBord.Text = "有白边";
            this.rbBord.UseVisualStyleBackColor = true;
            // 
            // rbNotBord
            // 
            this.rbNotBord.AutoSize = true;
            this.rbNotBord.Checked = true;
            this.rbNotBord.Location = new System.Drawing.Point(544, 353);
            this.rbNotBord.Name = "rbNotBord";
            this.rbNotBord.Size = new System.Drawing.Size(59, 16);
            this.rbNotBord.TabIndex = 11;
            this.rbNotBord.TabStop = true;
            this.rbNotBord.Text = "无白边";
            this.rbNotBord.UseVisualStyleBackColor = true;
            // 
            // backColor
            // 
            this.backColor.Color = System.Drawing.Color.White;
            // 
            // btBack
            // 
            this.btBack.Location = new System.Drawing.Point(418, 392);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(75, 23);
            this.btBack.TabIndex = 12;
            this.btBack.Text = "背景色";
            this.btBack.UseVisualStyleBackColor = true;
            this.btBack.Click += new System.EventHandler(this.btBack_Click);
            // 
            // btFore
            // 
            this.btFore.Location = new System.Drawing.Point(544, 391);
            this.btFore.Name = "btFore";
            this.btFore.Size = new System.Drawing.Size(75, 23);
            this.btFore.TabIndex = 13;
            this.btFore.Text = "前景色";
            this.btFore.UseVisualStyleBackColor = true;
            this.btFore.Click += new System.EventHandler(this.btFore_Click);
            // 
            // logoFile
            // 
            this.logoFile.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(338, 424);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "生成二维码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btFore);
            this.Controls.Add(this.btBack);
            this.Controls.Add(this.rbNotBord);
            this.Controls.Add(this.rbBord);
            this.Controls.Add(this.cbError);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbLogoBord);
            this.Controls.Add(this.tbLogoSize);
            this.Controls.Add(this.tbVersion);
            this.Controls.Add(this.tbPixel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.tbContent);
            this.Controls.Add(this.tbDec);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbQRCode);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbQRCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDec;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbContent;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPixel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbLogoSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbLogoBord;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbError;
        private System.Windows.Forms.RadioButton rbBord;
        private System.Windows.Forms.RadioButton rbNotBord;
        private System.Windows.Forms.ColorDialog backColor;
        private System.Windows.Forms.ColorDialog foreColor;
        private System.Windows.Forms.Button btBack;
        private System.Windows.Forms.Button btFore;
        private System.Windows.Forms.OpenFileDialog logoFile;
        private System.Windows.Forms.Button button1;
    }
}

