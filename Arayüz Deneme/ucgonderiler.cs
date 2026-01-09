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
    public partial class ucgonderiler : UserControl
    {
        public ucgonderiler()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = null;
        ///Durum güncelleme butonu: Hazırlanıyor... → Yolda → Teslim Edildi
        private void button3_Click(object sender, EventArgs e)
        {
            // Seçili satır var mı kontrol et
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen durumunu güncellemek istediğiniz gönderiyi seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                baglanti = new SqlConnection(@"Data Source=AYBERK\SQLEXPRESS;Initial Catalog=NakliyeDB;Integrated Security=True");
                baglanti.Open();

                int sevkiyatId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["SevkiyatId"].Value);
                string mevcutDurum = dataGridView1.SelectedRows[0].Cells["Durum"].Value.ToString();

                // Durum geçişini belirle
                string yeniDurum = "";
                switch (mevcutDurum)
                {
                    case "Hazırlanıyor...":
                        yeniDurum = "Yolda";
                        break;
                    case "Yolda":
                        yeniDurum = "Teslim Edildi";
                        break;
                    case "Teslim Edildi":
                        MessageBox.Show("Bu gönderi zaten teslim edilmiş!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    default:
                        MessageBox.Show("Bilinmeyen durum!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                // Kullanıcıya onay sor
                DialogResult result = MessageBox.Show(
                    $"Durum güncellenecek:\n\n'{mevcutDurum}' → '{yeniDurum}'\n\nDevam etmek istiyor musunuz?",
                    "Durum Güncelleme",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Durumu güncelle
                    string updateQuery = "UPDATE Sevkiyatlar SET Durum = @yeniDurum WHERE SevkiyatId = @id";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, baglanti))
                    {
                        cmd.Parameters.AddWithValue("@yeniDurum", yeniDurum);
                        cmd.Parameters.AddWithValue("@id", sevkiyatId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Durum başarıyla güncellendi!\n\n{mevcutDurum} → {yeniDurum}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // DataGridView'i yenile
                            dataGridView1.SelectedRows[0].Cells["Durum"].Value = yeniDurum;
                        }
                        else
                        {
                            MessageBox.Show("Durum güncellenemedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti != null && baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=AYBERK\SQLEXPRESS;Initial Catalog=NakliyeDB;Integrated Security=True");
            baglanti.Open();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int sevkiyatid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["SevkiyatId"].Value);

                DialogResult result = MessageBox.Show("Seçili Satırı Silmek İstediğinize Emin Misiniz","Onay",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) 
                    {
                    string query = "DELETE FROM Sevkiyatlar WHERE SevkiyatId = @id";
                    using (SqlCommand cmd = new SqlCommand(query, baglanti)) 
                    {   
                        cmd.Parameters.AddWithValue("@id",sevkiyatid);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0) 
                        {
                            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                            MessageBox.Show("Başarıyla Silindi");
                        }
                        else 
                        {
                            MessageBox.Show("Silme işlemi başarısız oldu.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen Silmek için Satır Seçin.");
                }
            }
        }

        private void ucgonderiler_Load(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=AYBERK\SQLEXPRESS;Initial Catalog=NakliyeDB;Integrated Security=True");
            baglanti.Open();
            string query = @"
                                SELECT 
                                    Sev.SevkiyatId,
                                    Y.YukAdi AS YukAdi,
                                    S.AdSoyad AS SoforAdi,
                                    S.TcNo AS SoforTCNO,
                                    A.Plaka AS AracPlakasi,
                                    A.DorsePlaka AS DorsePlakasi,
                                    Sev.CikisYeri,
                                    Sev.VarisYeri,
                                    Sev.TahminiTeslim,
                                    Sev.Durum,
                                    Sev.BaslangicTarihi
                                FROM Sevkiyatlar Sev
                                INNER JOIN Soforler S ON Sev.SoforId = S.SoforId
                                INNER JOIN Araclar A ON Sev.AracId = A.PlakaId
                                INNER JOIN Yukler Y ON Sev.YukId = Y.YukId
                                ";

            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter ad = new SqlDataAdapter(query, baglanti);
                ad.Fill(dt);
                dataGridView1.DataSource = dt;

                dataGridView1.Columns["SoforAdi"].HeaderText = "Şoför Adı";
                dataGridView1.Columns["SoforTCNO"].HeaderText = "Şoför Tc No";
                dataGridView1.Columns["AracPLakasi"].HeaderText = "Araç Plakası";
                dataGridView1.Columns["DorsePlakasi"].HeaderText = "Dorse Plakası";
                dataGridView1.Columns["YukAdi"].HeaderText = "Yük Adı";

            }
            catch (Exception ex)
            {
                string detay = ex.Message;
                if (ex.InnerException != null) detay += "\nInner: " + ex.InnerException.Message;
                MessageBox.Show("Hata oluştu:\n" + detay, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = @"
                                SELECT 
                                    Sev.SevkiyatId,
                                    Y.YukAdi AS YukAdi,
                                    S.AdSoyad AS SoforAdi,
                                    S.TcNo AS SoforTCNO,
                                    A.Plaka AS AracPlakasi,
                                    A.DorsePlaka AS DorsePlakasi,
                                    Sev.CikisYeri,
                                    Sev.VarisYeri,
                                    Sev.TahminiTeslim,
                                    Sev.Durum,
                                    Sev.BaslangicTarihi
                                FROM Sevkiyatlar Sev
                                INNER JOIN Soforler S ON Sev.SoforId = S.SoforId
                                INNER JOIN Araclar A ON Sev.AracId = A.PlakaId
                                INNER JOIN Yukler Y ON Sev.YukId = Y.YukId
                                ";

            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter ad = new SqlDataAdapter(query, baglanti);
                ad.Fill(dt);
                dataGridView1.DataSource = dt;

                dataGridView1.Columns["SoforAdi"].HeaderText = "Şoför Adı";
                dataGridView1.Columns["SoforTCNO"].HeaderText = "Şoför Tc No";
                dataGridView1.Columns["AracPLakasi"].HeaderText = "Araç Plakası";
                dataGridView1.Columns["DorsePlakasi"].HeaderText = "Dorse Plakası";
                dataGridView1.Columns["YukAdi"].HeaderText = "Yük Adı";

            }
            catch (Exception ex)
            {
                string detay = ex.Message;
                if (ex.InnerException != null) detay += "\nInner: " + ex.InnerException.Message;
                MessageBox.Show("Hata oluştu:\n" + detay, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }
        }
    }
}
