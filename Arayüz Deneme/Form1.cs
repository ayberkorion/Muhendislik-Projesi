using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arayüz_Deneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = null; 

            try
            {
                baglanti = new SqlConnection(@"Data Source=AYBERK\SQLEXPRESS;Initial Catalog=NakliyeDB;Integrated Security=True");
                baglanti.Open();

                SqlCommand sqlKomut = new SqlCommand("SELECT AdSoyad,KullaniciAdi, Sifre,Rol FROM Kullanicilar WHERE KullaniciAdi=@kullaniciadi AND Sifre=@sifre", baglanti);
                sqlKomut.Parameters.AddWithValue("@kullaniciadi", textBox1.Text);
                sqlKomut.Parameters.AddWithValue("@sifre", textBox2.Text);
                SqlDataReader sqlDR = sqlKomut.ExecuteReader();

                if(sqlDR.Read())
                {
                    string adsoyad = sqlDR["AdSoyad"].ToString();
                    string rol = sqlDR["Rol"].ToString() ;

                        MessageBox.Show("                Giriş Başarılı! \nHoşgeldiniz Sayın " + adsoyad + "");
                        
                        AnaSayfa anaSayfa = new AnaSayfa(adsoyad,rol);
                        anaSayfa.Show();
                        this.Hide();

                }
                else
                    {
                        MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı");
                    }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sql Query Sırasında Hata Oluştu !    " + ex.ToString());
            }
            finally 
            {   
                if (baglanti != null) 
                    baglanti.Close();
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
