using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arayüz_Deneme
{
    public partial class ucurunekleme : UserControl
    {
        public ucurunekleme()
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
                baglanti.Open();
                SqlTransaction tran = baglanti.BeginTransaction();
                try
                {
                    SqlCommand cmdyukler = new SqlCommand("INSERT INTO Yukler (YukAdi,YukTuru,Aciklama,DepoAdi) VALUES (@ad,@tur,@aciklama,@depoadi);",baglanti , tran);
                    cmdyukler.Parameters.AddWithValue("@ad",textBox1.Text);
                    cmdyukler.Parameters.AddWithValue("@tur", textBox2.Text);
                    cmdyukler.Parameters.AddWithValue("@depoadi", comboBox1.SelectedValue);
                    cmdyukler.Parameters.AddWithValue("@aciklama", textBox4.Text);

                    cmdyukler.ExecuteNonQuery();
                    tran.Commit();
                    MessageBox.Show("Veriler Eklendi");
                    ClearTextBoxes(this);
                    textBox1.Focus();

                }
                catch(Exception ex) 
                {

                    tran.Rollback();
                    MessageBox.Show("Hata Oluştu İşlem İptal Edildi." + ex.Message);
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

                if (baglanti != null)
                    baglanti.Close();
            }
        }
        void YukleriYenile()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT DISTINCT YukId, YukAdi FROM Yukler", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            comboBox2.DataSource = dt2;
            comboBox2.DisplayMember = "YukAdi";
            comboBox2.ValueMember = "YukId";
        }
        private void ucurunekleme_Load(object sender, EventArgs e)
        {
            YukleriYenile();
            baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Ad FROM Depolar", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Ad";
            comboBox1.ValueMember = "Ad";

            SqlDataAdapter da2 = new SqlDataAdapter("SELECT DISTINCT YukId,YukAdi FROM Yukler", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            
            comboBox2.DataSource = dt2;
            comboBox2.DisplayMember = "YukAdi";
            comboBox2.ValueMember = "YukId";


            baglanti.Close();   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if(comboBox2.SelectedIndex == -1)
                    {

                    MessageBox.Show("Lütfen Silmek İçin Bir Ürün Seçiniz!!!");
                    return;
                }


                baglanti.Open();
                DialogResult result = MessageBox.Show("Seçili Satırı Silmek İstediğinize Emin Misiniz", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(comboBox2.SelectedValue);
                    SqlCommand silme = new SqlCommand("DELETE FROM Yukler WHERE YukId=@id", baglanti);
                    silme.Parameters.AddWithValue("@id", id);
                    silme.ExecuteNonQuery();

                    YukleriYenile();
                    comboBox1.SelectedIndex = -1;
                }
                
            }
            catch(Exception ex)  
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

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string query = "SELECT YukAdi,YukTuru,DepoAdi,Aciklama FROM Yukler ";
                

            try

            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter ad = new SqlDataAdapter(query, baglanti);
                ad.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns["YukAdi"].HeaderText = "Yük Adı";
                dataGridView1.Columns["YukTuru"].HeaderText = "Yük Türü";
                dataGridView1.Columns["DepoAdi"].HeaderText = "Depo Adı";
                dataGridView1.Columns["Aciklama"].HeaderText = "Açıklama";
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

        private void button4_Click(object sender, EventArgs e)
        {
            YukleriYenile();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }
    }
}
