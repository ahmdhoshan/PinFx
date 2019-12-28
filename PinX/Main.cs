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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PinBlogPosts fr = new PinBlogPosts();
            fr.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Accounts fr = new Accounts();
            fr.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Repin rp = new Repin();


            rp.Show();  

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void GetPins_button_Click(object sender, EventArgs e)
        {
            GetPins fr = new GetPins();
            fr.Show();
        }
    }
}
