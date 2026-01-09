using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Arayüz_Deneme
{
    public partial class AnaSayfa : Form
    {
        public AnaSayfa(string adsoyad, string rol)
        {
            InitializeComponent();
            
            // Logo yükle
            try
            {
                string logoPath = System.IO.Path.Combine(Application.StartupPath, "Resources", "logo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    pictureBoxLogo.Image = Image.FromFile(logoPath);
                }
            }
            catch { }
            
            uckontrolpaneli1.BringToFront();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }
            label1.Text = adsoyad;
            label2.Text =   rol;
        }

        private void label3_Click(object sender, EventArgs e)
        { 
            uckontrolpaneli1.BringToFront();
            uckontrolpaneli1.GuncelleDurum();
        }

        private void gondericlick(object sender, EventArgs e)
        {
            ucgonderiler1.BringToFront();
        }

        private void cikis_Click(object sender, EventArgs e)
        {

            Form1 form1 = new Form1();
            form1.Show();
            this.Close();


        }
        private void label5_Click(object sender, EventArgs e)
        {
            ucgonderiekle1.BringToFront();
        }

        private void AnaSayfa_Load_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            ucstoktakip1.BringToFront();
        }

        private void label9_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
            ucurunekleme1.BringToFront();
                   
        }

        private void label11_Click(object sender, EventArgs e)
        {
            ucdepoekleme1.BringToFront();
        }

        private void ucstoktakip2_Load(object sender, EventArgs e)
        {

        }
    }
}
    