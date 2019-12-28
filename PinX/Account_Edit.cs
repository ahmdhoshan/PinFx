using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinX
{
    public partial class Account_Edit : Form
    {

        private Accounts accounts1;

        public Account_Edit(Accounts acc)
        {
            InitializeComponent();
            this.accounts1 = acc;
        }



        public Account_Edit()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            accounts1.dataGridView1.SelectedCells[0].Value = textBox1.Text;
            accounts1.dataGridView1.SelectedCells[1].Value = textBox2.Text;
            accounts1.dataGridView1.SelectedCells[2].Value = textBox7.Text;
            accounts1.dataGridView1.SelectedCells[3].Value = textBox3.Text;
            accounts1.dataGridView1.SelectedCells[5].Value = textBox4.Text;
            accounts1.dataGridView1.SelectedCells[4].Value = textBox5.Text;
            accounts1.dataGridView1.SelectedCells[6].Value = textBox6.Text;



            Program.ExportToFile("accounts.txt", accounts1.dataGridView1, Program.AccountsFileHeader);
            this.Close();
        }

        private void Account_Edit_Load(object sender, EventArgs e)
        {
            //Set The Selected Items
            textBox1.Text = accounts1.dataGridView1.SelectedCells[0].Value.ToString();
            textBox2.Text = accounts1.dataGridView1.SelectedCells[1].Value.ToString();
            textBox7.Text = accounts1.dataGridView1.SelectedCells[2].Value.ToString();
            textBox3.Text = accounts1.dataGridView1.SelectedCells[3].Value.ToString();
            textBox5.Text = accounts1.dataGridView1.SelectedCells[4].Value.ToString();
            textBox4.Text = accounts1.dataGridView1.SelectedCells[5].Value.ToString();
            textBox6.Text = accounts1.dataGridView1.SelectedCells[6].Value.ToString();

        }
    }
}
