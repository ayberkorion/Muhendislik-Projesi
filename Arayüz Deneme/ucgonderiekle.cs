using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace Arayüz_Deneme
{
    public partial class ucgonderiekle : UserControl
    {
        public ucgonderiekle()
        {
            InitializeComponent();
        }



        private void ClearTextBoxes(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox) 
                { 
                    c.Text = string.Empty;
                }
                else 
                {
                    ClearTextBoxes(c);
                } 
                    
            }
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=AYBERK\SQLEXPRESS;Initial Catalog=NakliyeDB;Integrated Security=True;TrustServerCertificate=True;");
        
        
        
        private void button1_Click(object sender, EventArgs e)
        {   
            try
            {
                // Sevk miktarı kontrolü - en başta yap
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("Lütfen sevk miktarını giriniz!");
                    textBox5.Focus();
                    return;
                }

                if (!int.TryParse(textBox5.Text, out int sevkMiktari) || sevkMiktari <= 0)
                {
                    MessageBox.Show("Sevk miktarı geçerli bir sayı olmalıdır!");
                    textBox5.Focus();
                    return;
                }

                // YukId'yi al
                if (comboBox2.SelectedValue == null)
                {
                    MessageBox.Show("Lütfen yük seçiniz!");
                    return;
                }
                int yukId = Convert.ToInt32(comboBox2.SelectedValue);

                baglanti.Open();
                SqlTransaction tran = baglanti.BeginTransaction();

                try
                {
                    // 1. Önce stok kontrolü yap (transaction içinde)
                    SqlCommand stokgetir = new SqlCommand("SELECT MevcutStok FROM StokHareketleri WHERE YukId=@id", baglanti, tran);
                    stokgetir.Parameters.AddWithValue("@id", yukId);

                    object stokResult = stokgetir.ExecuteScalar();
                    
                    if (stokResult == null)
                    {
                        throw new Exception("Bu ürün için stok kaydı bulunamadı!");
                    }

                    int mevcutStok = Convert.ToInt32(stokResult);
                    
                    if (mevcutStok < sevkMiktari)
                    {
                        throw new Exception($"Stok Yetersiz! Mevcut Stok: {mevcutStok}, İstenen: {sevkMiktari}");
                    }

                    // 2. Şoför ekle
                    SqlCommand cmdsofor = new SqlCommand("INSERT INTO Soforler (AdSoyad, Telefon,TcNo) VALUES (@ad, @telefon,@tcno); SELECT SCOPE_IDENTITY();", baglanti, tran);
                    cmdsofor.Parameters.AddWithValue("@ad", textBox8.Text);
                    cmdsofor.Parameters.AddWithValue("@telefon", textBox9.Text);
                    cmdsofor.Parameters.AddWithValue("@tcno", textBox10.Text);
                    int soforId = Convert.ToInt32(cmdsofor.ExecuteScalar());

                    // 3. Araç ekle
                    SqlCommand cmdarac = new SqlCommand("INSERT INTO Araclar (Plaka,DorsePlaka) VALUES (@plaka,@dorse); SELECT SCOPE_IDENTITY();", baglanti, tran);
                    cmdarac.Parameters.AddWithValue("@dorse", textBox2.Text);
                    cmdarac.Parameters.AddWithValue("@plaka", textBox1.Text);
                    int aracId = Convert.ToInt32(cmdarac.ExecuteScalar());

                    // 4. Sevkiyat ekle
                    SqlCommand cmdsevkiyat = new SqlCommand("INSERT INTO Sevkiyatlar (YukId,SoforId,AracId,CikisYeri,VarisYeri,Durum,BaslangicTarihi,TahminiTeslim) VALUES (@yuk,@sofor,@arac,@cikis,@varis,@durum,@baslangic,@teslim)",baglanti, tran);
                    cmdsevkiyat.Parameters.AddWithValue("@yuk", yukId);
                    cmdsevkiyat.Parameters.AddWithValue("@sofor", soforId);
                    cmdsevkiyat.Parameters.AddWithValue("@arac", aracId);
                    cmdsevkiyat.Parameters.AddWithValue("@cikis", textBox6.Text);
                    cmdsevkiyat.Parameters.AddWithValue("@varis", textBox7.Text);
                    cmdsevkiyat.Parameters.AddWithValue("@durum", "Hazırlanıyor...");
                    cmdsevkiyat.Parameters.AddWithValue("@baslangic", SqlDbType.DateTime).Value = DateTime.Now.Date;
                    cmdsevkiyat.Parameters.AddWithValue("@teslim", SqlDbType.DateTime).Value = DateTime.Now.AddDays(3).Date;
                    cmdsevkiyat.ExecuteNonQuery();

                    // 5. Stoktan düş
                    SqlCommand stokdus = new SqlCommand("UPDATE StokHareketleri SET MevcutStok = MevcutStok - @miktar WHERE YukId=@id", baglanti, tran);
                    stokdus.Parameters.AddWithValue("@miktar", sevkMiktari);
                    stokdus.Parameters.AddWithValue("@id", yukId);
                    stokdus.ExecuteNonQuery();

                    // Tüm işlemler başarılı, commit et
                    tran.Commit();
                    MessageBox.Show("Sevkiyat başarıyla oluşturuldu ve stoktan düşüldü!"); 
                    ClearTextBoxes(this);
                    textBox1.Focus();
                }
                catch(Exception ex) 
                {
                    tran.Rollback();
                    MessageBox.Show("Hata Oluştu, İşlem İptal Edildi.\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Oluştu:\n" + ex.Message);
            }
            finally
            {
                if (baglanti != null && baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        bool yukleryuklendi=false;

        private void yukyenile ()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT YukTuru FROM Yukler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "YukTuru";
            comboBox1.ValueMember = "YukTuru";

            yukleryuklendi = true;
            baglanti.Close();
        } 
        private void ucgonderiekle_Load_1(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT YukTuru FROM Yukler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
           
            comboBox1.DataSource=dt;
            comboBox1.DisplayMember = "YukTuru";
            comboBox1.ValueMember = "YukTuru";

            baglanti.Close();
            yukleryuklendi = true;
        }
        
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        { 
            if (!yukleryuklendi) return;
            if (comboBox1.SelectedIndex == -1)
                return;
            string secilentur = comboBox1.SelectedValue.ToString();
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter("SELECT YukId, YukAdi FROM Yukler WHERE YukTuru = @tur ", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@tur", secilentur);

            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "YukAdi";
            comboBox2.ValueMember = "YukId";

            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen Yük Türü Ve Adı Seçiniz");
                return;
            }
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            yukyenile();
        }
    }
}
