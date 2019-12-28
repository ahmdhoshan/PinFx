using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinX
{
    public partial class Repin_Edit : Form
    {

        private Repin f2;

        public Repin_Edit(Repin ff)
        {
            InitializeComponent();
            this.f2 = ff;
        }

        public Repin_Edit()
        {
            InitializeComponent();
        }

        private void Repin_Edit_Load(object sender, EventArgs e)
        {
            textBox3.Text = f2.dataGridView1.SelectedCells[0].Value.ToString();
            textBox1.Text = f2.dataGridView1.SelectedCells[1].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f2.dataGridView1.SelectedCells[0].Value = textBox3.Text;
            f2.dataGridView1.SelectedCells[1].Value = textBox1.Text;
            //Update The Repin file after Edit
            Program.ExportToFile("repin.txt", f2.dataGridView1, Program.RePinFileHeader);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
