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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string gonderilecekVeri;
        public static SqlConnection gonderilecekCon;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        private void SistemeGiris(object sender, EventArgs e)
        {
            string user = kullaniciAdText.Text;
            string pass = sifreText.Text;
            con = new SqlConnection("server =.; Initial Catalog = dbLogin; Integrated Security = SSPI");
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM tblUser where usr='" + kullaniciAdText.Text + "' AND psw='" + sifreText.Text + "'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
               
                MessageBox.Show("Tebrikler! Başarılı bir şekilde giriş yaptınız.");
                con.Close();
                gonderilecekCon = con;
                gonderilecekVeri = kullaniciAdText.Text;
                Form2 form2 = new Form2();
                form2.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adını ve şifrenizi kontrol ediniz.");
                con.Close();
            }
        }

        private void SistemdenCikis(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

