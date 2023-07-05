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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DisconnectedEnvironment
{
    public partial class Form4 : Form
    {
        private string stringConnection = "data source=getto⛧\\RAYNANDA_AQIYAS;" + "database=DisconnectedEnvironment;User ID=sa;Password=Zidanlagoa13";
        private SqlConnection koneksi;

        private void refreshform()
        {
            cbxNama.Enabled = false;
            cbxSM.Enabled = false;
            cbxTM.Enabled = false;
            cbxNama.SelectedIndex = -1;
            cbxSM.SelectedIndex = -1;
            cbxTM.SelectedIndex = -1;
            txtNIM.Visible = false;
            btnAdd.Enabled = true;
            btnClear.Enabled = false;
            btnSave.Enabled = false;
            
        }


        public Form4()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshform();
        }

        private void dataGridView()
        {
            koneksi.Open();
            string str = "select * from dbo.status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        private void cbNama()
        {
            koneksi.Open();
            string str = "select nama_mahasiswa from dbo*mahasiswa where " + "not EXISTS(select id_status from dbo.status_mahasiswa where"
                + "status_mahasiswa.nim = mahasiswa.nim)";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();

            cbxNama.DisplayMember = "nama_mahasiswa";
            cbxNama.ValueMember = "NIM";
            cbxNama.DataSource = ds.Tables[0];
        }

        private void cbTahunMasuk()
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i = 0; i < type.Length; i++)
            {
                if (i == 0)
                {
                    cbxTM.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + i;
                    cbxTM.Items.Add(l.ToString());
                }
            }
        }

        private void cbxNama_SelectedIndexChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "select NIM from dbo.mahasiswa where nama_mahasiswa = @nm";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", cbxNama.Text));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();

            txtNIM.Text = nim;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbxTM.Enabled = true;
            cbxNama.Enabled = true;
            cbxSM.Enabled = true;
            txtNIM.Visible = true;
            cbTahunMasuk();
            cbNama();
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string statusMahasiswa = cbxSM.Text;
            string TahunMasuk = cbxTM.Text;
            int count = 0;
            string tempKodeStatus = "";
            string KodeStatus = "";
            koneksi.Open();

            string str = "select count (*) from dbo.status_mahasiswa";
            SqlCommand cm = new SqlCommand(str, koneksi);
            count = (int)cm.ExecuteScalar();

            if (count == 0)
            {
                KodeStatus = "1";
            }
            else
            {
                string queryStrings = "select Max(id_status) from dbo.status_mahasiswa";
                SqlCommand cmStatusMahasiswaSum = new SqlCommand(str, koneksi);
                int totalStatusMahasiswa = (int)cmStatusMahasiswaSum.ExecuteScalar();
                int finalKodeStatusInt = totalStatusMahasiswa + 1;
                KodeStatus = Convert.ToString(finalKodeStatusInt);
            }
            string queryString = "insert into dbo.status_mahasiswa (id_status, nim, " + "status_mahasiswa, tahun_masuk)" +
                "values(@ids, @NIM, @sm, @tm)";
            SqlCommand cmd = new SqlCommand(queryString, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("ids", KodeStatus));
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("sm", statusMahasiswa));
            cmd.Parameters.Add(new SqlParameter("tm", TahunMasuk));
            cmd.ExecuteNonQuery();
            koneksi.Close();

            MessageBox.Show("Data berhasil di simpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshform();
            dataGridView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
    }
}
