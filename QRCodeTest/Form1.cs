using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyHelper.二维码;

namespace QRCodeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBack_Click(object sender, EventArgs e)
        {
            backColor.ShowDialog();
            btBack.Text = backColor.Color.Name;
        }

        /// <summary>
        /// 设置前景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFore_Click(object sender, EventArgs e)
        {
            foreColor.ShowDialog();
            btFore.Text = foreColor.Color.Name;
        }

        /// <summary>
        /// 获取logo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            var ds = logoFile.ShowDialog();
            if (ds == DialogResult.OK)
            {
                var file = logoFile.FileName;
                pbLogo.Image = Image.FromFile(file);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var msg = tbContent.Text; //正文
            var error = QrCodeHelper.GetErrLevel(cbError.SelectedText); //容错率
            var backColors = this.backColor.Color; //二维码背景色
            var foreColors = this.foreColor.Color; //二维码前景色
            var pixel = int.Parse(tbPixel.Text); //二维码大小像素
            var version = int.Parse(tbVersion.Text); //二维码版本
            var whiteBord = rbBord.Checked; //是否包含白边
            var logoSize = int.Parse(tbLogoSize.Text); //logo大小
            var logoBord = int.Parse(tbLogoBord.Text);//logo框
            var logoPath = logoFile.FileName != "openFileDialog1" ? logoFile.FileName : null;
            pbQRCode.Image = QrCodeHelper.CodeImage(msg, error, pixel, foreColors, backColors, false, version, logoPath,
                logoSize, logoBord, whiteBord);
            tbDec.Text = ThoughtWorksQrCodeHelper.DeQrCode(pbQRCode.Image);
        }
    }
}
