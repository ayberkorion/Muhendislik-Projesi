using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arayüz_Deneme
{
    public partial class ucdepoekleme : UserControl
    {
        public ucdepoekleme()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=AYBERK\SQLEXPRESS;Initial Catalog=NakliyeDB;Integrated Security=True;TrustServerCertificate=True;");

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
            try
            {
                    baglanti.Open();
                    SqlTransaction tran = baglanti.BeginTransaction();
                try
                {
                    SqlCommand cmddepolar = new SqlCommand("INSERT INTO Depolar (Ad,Konum,SorumluKisi,Kapasite) VALUES (@ad,@konum,@sorumlu,@kapasite);", baglanti, tran);

                    cmddepolar.Parameters.AddWithValue("@ad", textBox1.Text);
                    cmddepolar.Parameters.AddWithValue("@konum", textBox2.Text);
                    cmddepolar.Parameters.AddWithValue("@sorumlu", textBox3.Text);
                    cmddepolar.Parameters.AddWithValue("@kapasite", textBox4.Text);

                    cmddepolar.ExecuteNonQuery();
                    tran.Commit();
                    ClearTextBoxes(this);
                    textBox1.Focus();
                }
                catch (Exception ex)
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

        private void button2_Click(object sender, EventArgs e)
        {
                baglanti.Open();
                string query = "SELECT Ad,Konum,SorumluKisi,Kapasite FROM Depolar";

            try

            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter ad = new SqlDataAdapter( query, baglanti);
                ad.Fill(dt);
                
                dataGridView1.DataSource = dt;

                dataGridView1.Columns["Ad"].HeaderText = "Depo Adı";
                dataGridView1.Columns["Konum"].HeaderText = "Konum";
                dataGridView1.Columns["SorumluKisi"].HeaderText = "Sorumlu";
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1)
                {

                    MessageBox.Show("Lütfen Silmek İçin Bir Ürün Seçiniz!!!");
                    return;
                }


                baglanti.Open();
                DialogResult result = MessageBox.Show("Seçili Satırı Silmek İstediğinize Emin Misiniz", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(comboBox1.SelectedValue);
                    SqlCommand silme = new SqlCommand("DELETE FROM Depolar WHERE DepoId=@id", baglanti);
                    silme.Parameters.AddWithValue("@id", id);
                    silme.ExecuteNonQuery();

                    DepolariYenile();
                    comboBox1.SelectedIndex = -1;
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

        void DepolariYenile()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT DISTINCT DepoId, Ad FROM Depolar", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            comboBox1.DataSource = dt2;
            comboBox1.DisplayMember = "Ad";
            comboBox1.ValueMember = "DepoId";
        }
        private void ucdepoekleme_Load(object sender, EventArgs e)
        {

            DepolariYenile();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DepolariYenile();
        }
    }
}
