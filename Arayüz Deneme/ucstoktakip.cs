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
    public partial class ucstoktakip : UserControl
    {
        public ucstoktakip()
        {
            InitializeComponent();


        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=AYBERK\SQLEXPRESS;Initial Catalog=NakliyeDB;Integrated Security=True");
        private void ucstoktakip_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            string query = @"     SELECT 
                                        S.StokId,
                                        Y.YukAdi AS YukAdi,
                                        D.Ad AS DepoAdi,       
                                        D.Kapasite AS Kapasite,
                                        S.MevcutStok
                                    FROM StokHareketleri S
                                    INNER JOIN Yukler Y ON S.YukId = Y.YukId
                                    INNER JOIN Depolar D ON S.DepoId = D.DepoId
                                     ";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();

                SqlDataAdapter ad = new SqlDataAdapter(query, baglanti);

                ad.Fill(dt1);

                dataGridView1.DataSource = dt1;


                dataGridView1.Columns["StokId"].HeaderText = "Stok ID";
                dataGridView1.Columns["YukAdi"].HeaderText = "Yük Adı";
                dataGridView1.Columns["DepoAdi"].HeaderText = "Depo Adı";
                dataGridView1.Columns["MevcutStok"].HeaderText = "Mevcut Stok";
                dataGridView1.Columns["Kapasite"].HeaderText = "Kapasite";


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

            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Ad,DepoId FROM Depolar", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.ValueMember = "DepoId";
            comboBox1.DisplayMember = "Ad";

            SqlDataAdapter da2 = new SqlDataAdapter("SELECT DISTINCT YukId, YukAdi FROM Yukler", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            comboBox2.DataSource = dt2;
            comboBox2.ValueMember = "YukId";
            comboBox2.DisplayMember = "YukAdi";

        }
        private void YukleriYenile()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT DISTINCT YukId, YukAdi FROM Yukler", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            comboBox2.DataSource = dt2;
            comboBox2.ValueMember = "YukId";
            comboBox2.DisplayMember = "YukAdi";

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
        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string query = @"     SELECT 
                                        S.StokId,
                                        Y.YukAdi AS YukAdi,
                                        D.Ad AS DepoAdi,       
                                        D.Kapasite AS Kapasite,
                                        S.MevcutStok
                                    FROM StokHareketleri S
                                    INNER JOIN Yukler Y ON S.YukId = Y.YukId
                                    INNER JOIN Depolar D ON S.DepoId = D.DepoId
                                     ";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter ad = new SqlDataAdapter(query, baglanti);

                ad.Fill(dt);

                dataGridView1.DataSource = dt;


                dataGridView1.Columns["StokId"].HeaderText = "Stok ID";
                dataGridView1.Columns["YukAdi"].HeaderText = "Yük Adı";
                dataGridView1.Columns["DepoAdi"].HeaderText = "Depo Adı";
                dataGridView1.Columns["MevcutStok"].HeaderText = "Mevcut Stok";
                dataGridView1.Columns["Kapasite"].HeaderText = "Kapasite";


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

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                // Validasyon: Stok miktarı kontrolü
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Lütfen stok miktarını giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Focus();
                    return;
                }

                if (!int.TryParse(textBox1.Text, out int yeniStok) || yeniStok <= 0)
                {
                    MessageBox.Show("Stok miktarı geçerli bir sayı olmalıdır!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Focus();
                    return;
                }

                int depoId = Convert.ToInt32(comboBox1.SelectedValue);

                baglanti.Open();
                SqlTransaction tran = baglanti.BeginTransaction();

                try
                {
                    // 1. Depo kapasitesini al
                    SqlCommand kapasiteCmd = new SqlCommand("SELECT Kapasite FROM Depolar WHERE DepoId = @depoId", baglanti, tran);
                    kapasiteCmd.Parameters.AddWithValue("@depoId", depoId);
                    int depoKapasitesi = Convert.ToInt32(kapasiteCmd.ExecuteScalar());

                    // 2. Depodaki mevcut toplam stoğu al
                    SqlCommand mevcutStokCmd = new SqlCommand(
                        "SELECT ISNULL(SUM(MevcutStok), 0) FROM StokHareketleri WHERE DepoId = @depoId", 
                        baglanti, tran);
                    mevcutStokCmd.Parameters.AddWithValue("@depoId", depoId);
                    int mevcutToplamStok = Convert.ToInt32(mevcutStokCmd.ExecuteScalar());

                    // 3. Kapasite kontrolü
                    int yeniToplamStok = mevcutToplamStok + yeniStok;
                    if (yeniToplamStok > depoKapasitesi)
                    {
                        tran.Rollback();
                        MessageBox.Show(
                            $"Depo kapasitesi yetersiz!\n\n" +
                            $"Depo Kapasitesi: {depoKapasitesi}\n" +
                            $"Mevcut Toplam Stok: {mevcutToplamStok}\n" +
                            $"Eklenecek Stok: {yeniStok}\n" +
                            $"Yeni Toplam: {yeniToplamStok}\n\n" +
                            $"Maksimum ekleyebileceğiniz: {depoKapasitesi - mevcutToplamStok}",
                            "Kapasite Aşımı",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    // 4. Stok ekle
                    SqlCommand stokekleme = new SqlCommand(
                        "INSERT INTO StokHareketleri (YukId,DepoId,MevcutStok) VALUES (@yuk,@depo,@mevcutstok)", 
                        baglanti, tran);

                    stokekleme.Parameters.AddWithValue("@yuk", comboBox2.SelectedValue);
                    stokekleme.Parameters.AddWithValue("@depo", depoId);
                    stokekleme.Parameters.AddWithValue("@mevcutstok", yeniStok);

                    stokekleme.ExecuteNonQuery();
                    tran.Commit();
                    
                    MessageBox.Show(
                        $"Stok başarıyla eklendi!\n\n" +
                        $"Depodaki Yeni Toplam Stok: {yeniToplamStok} / {depoKapasitesi}",
                        "Başarılı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    
                    ClearTextBoxes(this);
                    textBox1.Focus();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                string detay = ex.Message;
                if (ex.InnerException != null) detay += "\nInner: " + ex.InnerException.Message;
                MessageBox.Show("Hata oluştu:\n" + detay, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti != null && baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0 )
                {

                    MessageBox.Show("Lütfen Silmek İçin Bir Satır Seçiniz!!!");
                    return;
                }


                baglanti.Open();
                    int stokid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StokId"].Value);

                DialogResult result = MessageBox.Show("Seçili Satırı Silmek İstediğinize Emin Misiniz", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM StokHareketleri WHERE StokId=@id";
                    using (SqlCommand cmd = new SqlCommand(query, baglanti))
                    {
                        cmd.Parameters.AddWithValue("@id", stokid);
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

                    MessageBox.Show("Lütfen Silmek İçin Bir Satır Seçiniz!!!");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata Oluştu \n +" +
                                        ex.Message);
            }
            finally
            {
                if (baglanti != null)
                {
                    baglanti.Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Seçili satır kontrolü
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz stok kaydını seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int stokId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StokId"].Value);
                string yukAdi = dataGridView1.SelectedRows[0].Cells["YukAdi"].Value.ToString();
                int mevcutStok = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MevcutStok"].Value);

                // Basit prompt dialog
                Form promptForm = new Form()
                {
                    Width = 400,
                    Height = 180,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = "Stok Güncelle",
                    StartPosition = FormStartPosition.CenterScreen,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                Label textLabel = new Label() { Left = 20, Top = 20, Width = 350, Height = 40, Text = $"Ürün: {yukAdi}\nMevcut Stok: {mevcutStok}\n\nYeni stok miktarını girin:" };
                TextBox textBox = new TextBox() { Left = 20, Top = 70, Width = 340, Text = mevcutStok.ToString() };
                Button confirmation = new Button() { Text = "Güncelle", Left = 200, Width = 80, Top = 100, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "İptal", Left = 290, Width = 70, Top = 100, DialogResult = DialogResult.Cancel };

                confirmation.Click += (sender2, e2) => { promptForm.Close(); };
                cancel.Click += (sender2, e2) => { promptForm.Close(); };

                promptForm.Controls.Add(textLabel);
                promptForm.Controls.Add(textBox);
                promptForm.Controls.Add(confirmation);
                promptForm.Controls.Add(cancel);
                promptForm.AcceptButton = confirmation;
                promptForm.CancelButton = cancel;

                if (promptForm.ShowDialog() != DialogResult.OK)
                {
                    return; // İptal edildi
                }

                string input = textBox.Text;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return;
                }

                if (!int.TryParse(input, out int yeniStok) || yeniStok < 0)
                {
                    MessageBox.Show("Geçerli bir sayı giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                baglanti.Open();
                SqlTransaction tran = baglanti.BeginTransaction();

                try
                {
                    // Depo bilgilerini al
                    SqlCommand depoCmd = new SqlCommand(
                        "SELECT DepoId FROM StokHareketleri WHERE StokId = @stokId",
                        baglanti, tran);
                    depoCmd.Parameters.AddWithValue("@stokId", stokId);
                    int depoId = Convert.ToInt32(depoCmd.ExecuteScalar());

                    // Depo kapasitesini al
                    SqlCommand kapasiteCmd = new SqlCommand(
                        "SELECT Kapasite FROM Depolar WHERE DepoId = @depoId",
                        baglanti, tran);
                    kapasiteCmd.Parameters.AddWithValue("@depoId", depoId);
                    int depoKapasitesi = Convert.ToInt32(kapasiteCmd.ExecuteScalar());

                    // Depodaki diğer ürünlerin toplam stoğunu al (bu ürün hariç)
                    SqlCommand digerStokCmd = new SqlCommand(
                        "SELECT ISNULL(SUM(MevcutStok), 0) FROM StokHareketleri WHERE DepoId = @depoId AND StokId != @stokId",
                        baglanti, tran);
                    digerStokCmd.Parameters.AddWithValue("@depoId", depoId);
                    digerStokCmd.Parameters.AddWithValue("@stokId", stokId);
                    int digerUrunlerToplamStok = Convert.ToInt32(digerStokCmd.ExecuteScalar());

                    // Kapasite kontrolü
                    int yeniToplamStok = digerUrunlerToplamStok + yeniStok;
                    if (yeniToplamStok > depoKapasitesi)
                    {
                        tran.Rollback();
                        MessageBox.Show(
                            $"Depo kapasitesi yetersiz!\n\n" +
                            $"Depo Kapasitesi: {depoKapasitesi}\n" +
                            $"Diğer Ürünlerin Stoğu: {digerUrunlerToplamStok}\n" +
                            $"Yeni Stok: {yeniStok}\n" +
                            $"Toplam: {yeniToplamStok}\n\n" +
                            $"Maksimum girebileceğiniz: {depoKapasitesi - digerUrunlerToplamStok}",
                            "Kapasite Aşımı",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    // Stoğu güncelle
                    SqlCommand updateCmd = new SqlCommand(
                        "UPDATE StokHareketleri SET MevcutStok = @yeniStok WHERE StokId = @stokId",
                        baglanti, tran);
                    updateCmd.Parameters.AddWithValue("@yeniStok", yeniStok);
                    updateCmd.Parameters.AddWithValue("@stokId", stokId);
                    updateCmd.ExecuteNonQuery();

                    tran.Commit();

                    MessageBox.Show(
                        $"Stok başarıyla güncellendi!\n\n" +
                        $"Eski Stok: {mevcutStok}\n" +
                        $"Yeni Stok: {yeniStok}",
                        "Başarılı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // DataGridView'i güncelle
                    dataGridView1.SelectedRows[0].Cells["MevcutStok"].Value = yeniStok;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
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
    }
}
