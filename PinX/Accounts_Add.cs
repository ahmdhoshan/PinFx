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
    public partial class Accounts_Add : Form
    {

        private Accounts accounts1;

        public Accounts_Add(Accounts acc)
        {
            InitializeComponent();
            this.accounts1 = acc;
        }




        public Accounts_Add()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Please fill all the fields");
            }

            else
            {

                string email = textBox1.Text;
                string password = textBox2.Text;
                string profile = textBox3.Text;
                string proxy = textBox5.Text;
                string active = textBox4.Text;
                string DefultBoards = textBox6.Text;
                string UserName = textBox7.Text;

                string[] row = { email, password, UserName, profile, proxy, active, DefultBoards };
                accounts1.dataGridView1.Rows.Add(row);

                //Update The Accounts File After Add
                Program.ExportToFile("accounts.txt", accounts1.dataGridView1, Program.AccountsFileHeader);

                this.Close();
            }

        }

        private void Accounts_Add_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
