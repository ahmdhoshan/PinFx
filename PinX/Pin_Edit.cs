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
    public partial class Pin_Edit : Form
    {


        private PinBlogPosts f2;

        public Pin_Edit(PinBlogPosts ff)
        {
            InitializeComponent();
            this.f2 = ff;
        }



        public Pin_Edit()
        {
            InitializeComponent();
        }

        private void Pin_Edit_Load(object sender, EventArgs e)
        {
            textBox3.Text = f2.dataGridView1.SelectedCells[0].Value.ToString();
            textBox1.Text = f2.dataGridView1.SelectedCells[1].Value.ToString();
            textBox2.Text = f2.dataGridView1.SelectedCells[2].Value.ToString();
            richTextBox1.Text = f2.dataGridView1.SelectedCells[3].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f2.dataGridView1.SelectedCells[0].Value = textBox3.Text;
            f2.dataGridView1.SelectedCells[1].Value = textBox1.Text;
            f2.dataGridView1.SelectedCells[2].Value = textBox2.Text;
            f2.dataGridView1.SelectedCells[3].Value= richTextBox1.Text;
            //Update The input file after Edit
            Program.ExportToFile("input.txt", f2.dataGridView1, Program.PinFileHeader);
        }
    }
}
