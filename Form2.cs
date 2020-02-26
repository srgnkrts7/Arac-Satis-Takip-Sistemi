using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        
        SqlConnection con;
        SqlConnection sqlCon2 = new SqlConnection("server =.; Initial Catalog = dbAraba; Integrated Security = SSPI");
        SqlDataAdapter da;
        DataSet ds;
        string alanKisi;
        List<Araba> SatinAlinanArabalar = new List<Araba>();

        private void AracAl(object sender, EventArgs e)
        {
            
            Araba alinanAraba = new Araba();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string id="";
                string temp;
                string saseNo = row.Cells[0].Value.ToString();
                alinanAraba.ArabaSaseNo = Convert.ToInt32(saseNo);
                alinanAraba.Fiyat = row.Cells[1].Value.ToString();
                alinanAraba.Marka= row.Cells[2].Value.ToString();
                alinanAraba.Model = row.Cells[3].Value.ToString();
                alinanAraba.Kilometre = row.Cells[4].Value.ToString();
                temp= row.Cells[5].Value.ToString();
                if (temp.Equals(""))
                {//ALAN KİŞİNİN IDsini BULMA
                    con = new SqlConnection("server =.; Initial Catalog = dbLogin; Integrated Security = SSPI");
                    SqlCommand sorgu = new SqlCommand();
                    con.Open();
                    sorgu.Connection = con;
                    sorgu.CommandText = "SELECT id FROM tblUser WHERE usr='" + alanKisi + "'";
                    sorgu.ExecuteNonQuery();
                    SqlDataReader dr = sorgu.ExecuteReader();
                    if (dr.Read())
                    {
                        id = dr["id"].ToString();
                    }
                    alinanAraba.SatinAlanID = id;
                    con.Dispose();
                    con.Close();
                    SqlCommand sorgu2 = new SqlCommand();
                    sqlCon2.Open();
                    sorgu2.Connection = sqlCon2;
                    sorgu2.CommandText = "UPDATE ARABA SET SatinAlanId=" + alinanAraba.SatinAlanID + "WHERE ArabaSaseNo='" + alinanAraba.ArabaSaseNo + "'";
                    sorgu2.ExecuteNonQuery();
                    sqlCon2.Close();
                    SatinAlinanArabalar.Add(alinanAraba);
                    MessageBox.Show("            SAYIN" + "-- " + alanKisi + "--" + "\n  ARACINIZI BAŞARIYLA ALDINIZ !!!");
                }
                else
                {
                    MessageBox.Show("ARAÇ DAHA ÖNCEDEN BAŞKA BİR MÜŞTERİYE SATILMIŞTIR !!!");
                }
            }
        }
        private void SatilanlarıGoster(object sender, EventArgs e)
        {
            satilanlar.Items.Clear();
            foreach (Araba a in SatinAlinanArabalar)
            {
                if (a == null)
                {
                    break;
                }
                else
                {
                    string saseNo = Convert.ToString(a.ArabaSaseNo);
                    string[] dizi = { saseNo, a.Fiyat, a.Marka, a.Model, a.Kilometre, a.SatinAlanID };
                    var satir = new ListViewItem(dizi);
                    satilanlar.Items.Add(satir);
                }
            }
        }
        private void ArabalariGoster(object sender, EventArgs e)
        {
            try {

                dataGridView1.DataSource = null; 
                string sqlCmd = "select * from ARABA";
                da = new SqlDataAdapter(sqlCmd, sqlCon2);
                ds = new DataSet();
                da.Fill(ds, "Araba");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    KacKayitLabel.Text = "Kayıt bulunamadı";
                    return;
                }
                else
                {
                    KacKayitLabel.Text = ds.Tables[0].Rows.Count + " adet kayıt getirildi";
                    dataGridView1.DefaultCellStyle.BackColor = Color.Aqua;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Azure;
                    dataGridView1.DataSource = ds.Tables["Araba"];
                }
                      }catch (SqlException ex)
            {
                MessageBox.Show("Hata : "+ex); 
            }
            finally 
            {
                sqlCon2.Close(); 
                da.Dispose(); 
            }       
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            con = Form1.gonderilecekCon;
            alanKisi = Form1.gonderilecekVeri;
            

        }
    }
}
