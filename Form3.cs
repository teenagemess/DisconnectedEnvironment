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

namespace DisconnectedEnvironment
{
    public partial class Form3 : Form
    {
        private string stringConnection = "data source=getto⛧\\RAYNANDA_AQIYAS;" + "database=DisconnectedEnvironment;User ID=sa;Password=Zidanlagoa13";
        private SqlConnection koneksi;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        BindingSource customersBindingSource = new BindingSource();

        public Form3()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            this.bnMahasiswa.BindingSource = this.customersBindingSource;
            refreshform();
        }

        private void FormDataMahasiswa_load()
        {
            koneksi.Open();
            SqlDataAdapter dataAdapter1 = new SqlDataAdapter(new SqlCommand("Select m.nim, m.nama_mahasiswa, "
                + "m.alamat, m.jenis_kelamin, m.tgl_lahir, p.nama_prodi From dbo.mahasiswa m "
                + "join dbo.prodi p on m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            dataAdapter1.Fill(ds);

            this.customersBindingSource.DataSource = ds.Tables[0];
            this.txtNIM.DataBindings.Add(new Binding("Text", this.customersBindingSource, "NIM", true));
            this.txtNama.DataBindings.Add(new Binding("Text", this.customersBindingSource, "nama_mahasiswa", true));
            this.txtAlamat.DataBindings.Add(new Binding("Text", this.customersBindingSource, "alamat", true));
            this.cbxJK.DataBindings.Add(new Binding("Text", this.customersBindingSource, "jenis_kelamin", true));
            this.dtTL.DataBindings.Add(new Binding("Text", this.customersBindingSource, "tgl_lahir", true));
            this.cbxProdi.DataBindings.Add(new Binding("Text", this.customersBindingSource, "nama_prodi", true));
            koneksi.Close();
        }

        private void clearBinding()
        {
            this.txtNIM.DataBindings.Clear();
            this.txtNama.DataBindings.Clear();
            this.txtAlamat.DataBindings.Clear();
            this.cbxJK.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();
            this.dtTL.DataBindings.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = txtNIM.Text;
            nama = txtNama.Text;
            jk = cbxJK.Text;
            alamat = txtAlamat.Text;
            tgl = dtTL.Value;
            prodi = cbxProdi.Text;
            int hs = 0;
            koneksi.Open();
            string strs = "select id_prodi from dbo.prodi where nama_prodi = @dd";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@dd", prodi));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                hs = int.Parse(dr["id_prodi"].ToString());
            }
            dr.Close();
            string str = "insert into dbo.mahasiswa (nim, nama_mahasiswa, jenis_kelamin, alamat, tgl_lahir, id_prodi)"
               + "values(@NIM, @Nm, @Jk, @Al, @Tgll, @Idp)";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("Nm", nama));
            cmd.Parameters.Add(new SqlParameter("Jk", jk));
            cmd.Parameters.Add(new SqlParameter("Al", alamat));
            cmd.Parameters.Add(new SqlParameter("Tgll", tgl));
            cmd.Parameters.Add(new SqlParameter("Idp", hs));
            cmd.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            refreshform();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void refreshform()
        {
            txtNIM.Enabled = false;
            txtNama.Enabled = false;
            txtAlamat.Enabled = false;
            cbxJK.Enabled = false;
            cbxProdi.Enabled = false;
            dtTL.Enabled = false;
            btnAdd.Enabled = true;
            btnClear.Enabled = false;
            btnSave.Enabled = false;
            clearBinding();
            FormDataMahasiswa_load();
        }

        private void Prodicbx()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.prodi";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();
            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = ds.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtNIM.Text = "";
            txtNama.Text = "";
            txtAlamat.Text = "";
            dtTL.Value = DateTime.Today;
            txtNIM.Enabled = true;
            txtNama.Enabled = true;
            cbxJK.Enabled = true;
            txtAlamat.Enabled = true;
            cbxProdi.Enabled = true;
            Prodicbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = false;  
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
    }
}
