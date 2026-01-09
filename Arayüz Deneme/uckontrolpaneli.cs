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
    public partial class uckontrolpaneli : UserControl
    {
        public uckontrolpaneli()
        {
            InitializeComponent();
        }

        public void GuncelleDurum()
        {
            int toplamveri = 0;
            label6.Text = "0";
            label4.Text = "0";
            label3.Text = "0";
            string durumbildirisi = @"SELECT Durum,Count(*) AS Say FROM Sevkiyatlar GROUP BY Durum";
            using (SqlCommand komutlabel = new SqlCommand(durumbildirisi, baglanti))
            {
                using (SqlDataReader reader = komutlabel.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string durum = reader["Durum"].ToString();
                        int sayi = Convert.ToInt32(reader["Say"]);
                        

                        toplamveri += sayi;

                        switch (durum)
                        {
                            case "Hazırlanıyor...":
                                label6.Text = sayi.ToString();
                                break;
                            case "Teslim Edildi":
                                label4.Text = sayi.ToString();
                                break;


                        }
                    }

                }
            }

            label3.Text = toplamveri.ToString();

            // Yaklaşan gönderileri de yenile
            YaklasanGonderileriYukle();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=AYBERK\SQLEXPRESS;Initial Catalog=NakliyeDB;Integrated Security=True;TrustServerCertificate=True;");
        
        private void YaklasanGonderileriYukle()
        {
            try
            {
                // Yolda olan gönderileri tahmini teslim tarihine göre getir
                string query = @"
                    SELECT 
                        SevkiyatId,
                        VarisYeri,
                        TahminiTeslim,
                        Durum,
                        DATEDIFF(day, GETDATE(), TahminiTeslim) AS KalanGun
                    FROM Sevkiyatlar
                    WHERE Durum = 'Yolda'
                    ORDER BY TahminiTeslim ASC";

                SqlCommand cmd = new SqlCommand(query, baglanti);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // DataGridView'i temizle ve doldur
                dataGridView2.Rows.Clear();
                
                foreach (DataRow row in dt.Rows)
                {
                    string takipNo = "TK-" + row["SevkiyatId"].ToString();
                    string varisYeri = row["VarisYeri"].ToString();
                    DateTime tahminiTeslim = Convert.ToDateTime(row["TahminiTeslim"]);
                    string durum = row["Durum"].ToString(); // Durum bilgisini al
                    int kalanGun = Convert.ToInt32(row["KalanGun"]);
                    
                    // Durum + Kalan gün bilgisi
                    string durumMesaji;
                    if (kalanGun < 0)
                        durumMesaji = durum + " (GECİKMİŞ!)";
                    else if (kalanGun == 0)
                        durumMesaji = durum + " (Bugün)";
                    else if (kalanGun == 1)
                        durumMesaji = durum + " (Yarın)";
                    else
                        durumMesaji = durum + $" ({kalanGun} gün)";

                    dataGridView2.Rows.Add(
                        takipNo,
                        varisYeri,
                        tahminiTeslim.ToString("dd.MM.yyyy"),
                        durumMesaji
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yaklaşan gönderiler yüklenirken hata: " + ex.Message);
            }
        }

        private void uckontrolpaneli_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            int toplamveri = 0;

            label6.Text = "0";
            label4.Text = "0";
            label3.Text = "0";
            string durumbildirisi = @"SELECT Durum,Count(*) AS Say FROM Sevkiyatlar GROUP BY Durum";
            using (SqlCommand komutlabel = new SqlCommand(durumbildirisi, baglanti))
            {
                using(SqlDataReader reader = komutlabel.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string durum = reader["Durum"].ToString();
                        int sayi = Convert.ToInt32(reader["Say"]);
                       
                        toplamveri += sayi;

                        switch (durum)
                        {
                            case "Hazırlanıyor...":
                                label6.Text=sayi.ToString();
                                break;
                            case "Teslim Edildi":
                                label4.Text=sayi.ToString();
                                break;
                        }

                        label3.Text = toplamveri.ToString();
                    }
                }
            }

            label3.Text = toplamveri.ToString();

            // Yaklaşan gönderileri yükle
            YaklasanGonderileriYukle();
        }
    }
}
