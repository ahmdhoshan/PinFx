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
    public partial class Accounts : Form
    {
        public Accounts()
        {
            InitializeComponent();
        }

        private void Accounts_Load(object sender, EventArgs e)
        {
            Program.ReadPinsFile("accounts.txt",dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Accounts_Add fr = new Accounts_Add(this);
            fr.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.DeleteSelectedItems(this.dataGridView1);

            //Update The Accounts File After Delete
            Program.ExportToFile("accounts.txt", this.dataGridView1, Program.AccountsFileHeader);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(this.dataGridView1.SelectedRows.Count > 1 || this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select One Row");
            }
            else
            {
             Account_Edit fr = new Account_Edit(this);
             fr.Show();
            }
        }
    }
}
