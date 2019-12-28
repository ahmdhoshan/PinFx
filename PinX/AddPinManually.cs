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
    public partial class AddPostManually : Form
    {

        private PinBlogPosts pinblogpost;

        public AddPostManually(PinBlogPosts pinblogpost)
        {
            InitializeComponent();
            this.pinblogpost = pinblogpost;
        }


        public AddPostManually()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddPostManually_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string title = textBox3.Text;
            string image = textBox1.Text;
            string link = textBox2.Text;
            string description = richTextBox1.Text;
            string[] row = { title,image, link, description };
            pinblogpost.dataGridView1.Rows.Add(row);

            //Update The input file after add
            Program.ExportToFile("input.txt", pinblogpost.dataGridView1, Program.PinFileHeader);
        }
    }
}
