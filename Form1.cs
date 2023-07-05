using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisconnectedEnvironment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataProdiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
            this.Hide();
        }

        private void dataMahasiswaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.Show();
            this.Hide();
        }

        private void dataStatusMahasiswaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.Show();
            this.Hide();
        }
    }
}
