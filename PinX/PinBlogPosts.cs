using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinX
{
    public partial class PinBlogPosts : Form
    {








        public PinBlogPosts()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddPostManually fr = new AddPostManually(this);
            fr.Show();
        }

        private void PinBlogPosts_Load(object sender, EventArgs e)
        {
            Program.ReadPinsFile("input.txt",this.dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var FD = new System.Windows.Forms.OpenFileDialog();

            FD.Filter = "Text file(*.txt)|*.txt";


            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileToOpen = FD.FileName;
                Program.ReadPinsFile(FD.FileName, this.dataGridView1);

                Debug.WriteLine(FD.FileName);

            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {


            //Detect The File to Export to
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text file(*.txt)|*.txt";
            sfd.FilterIndex = 1;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) { return; }
            string dirLocationString = sfd.FileName;

            //Export To The Selected File
            Program.ExportToFile(dirLocationString, dataGridView1, Program.PinFileHeader);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            //Update The input file after clear
            Program.ExportToFile("input.txt", this.dataGridView1, Program.PinFileHeader);
        }

        private void button3_Click(object sender, EventArgs e)
        {

    
           

            //Set a custom profile
            FirefoxProfile profile = new FirefoxProfile(@"C:\Users\USER\\AppData\Roaming\Mozilla\Firefox\Profiles\ejhxt9wa.Slnum");
            FirefoxOptions options = new FirefoxOptions();
            options.Profile = profile;

            FirefoxDriver driver = new FirefoxDriver(options);


            //Login
            /*
            driver.Navigate().GoToUrl("https://www.pinterest.com");

            //Click on sign in
            driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div/div[3]/div/div[1]/div/div/div[1]/div/div/div[1]/div[4]/div/div[3]/div/div/a")).Click();

            //Fill user and pass
            driver.FindElement(By.XPath("//*[@id='email']")).SendKeys("nqtisimmer@hotmail.com");
            driver.FindElement(By.XPath("//*[@id='password']")).SendKeys("iptfeccofk5");

            //click on login
            driver.FindElement(By.XPath("/ html / body / div[1] / div / div / div / div / div[3] / div / div[1] / div / div / div[1] / div / div / div / div[3] / div[1] / form / div[5] / button / div")).Click();

            Thread.Sleep(3000);

            */

            driver.Navigate().GoToUrl("https://www.pinterest.com/pin-builder/");


            //fill the Title
            driver.FindElement(By.CssSelector("*[class^='TextArea__textArea TextArea__bold TextArea__light TextArea__enabled TextArea__large TextArea__wrap']")).SendKeys("Very Cool !!!!!");

            //fill the description
            driver.FindElement(By.CssSelector("*[class^='TextArea__textArea TextArea__light TextArea__enabled TextArea__medium TextArea__wrap']")).SendKeys("Very Cool");

            //fill the image
            driver.FindElement(By.CssSelector("input[id$='media-upload-input']")).SendKeys("C:\\Users\\USER\\Downloads\\fg.png");

            //click save
            driver.FindElement(By.CssSelector("*[class^='tBJ dyH iFc SMy yTZ erh DrD IZT mWe']")).Click();


            Thread.Sleep(2000);


            //fill the board name
            driver.FindElement(By.XPath("//*[@id='pickerSearchField']")).SendKeys("Clothing");


            //Click The board Name
            driver.FindElement(By.CssSelector("*[class^='tBJ dyH iFc SMy yTZ pBj DrD IZT mWe z-6']")).Click();


            //click save
            driver.FindElement(By.CssSelector("*[class^='tBJ dyH iFc SMy yTZ erh DrD IZT mWe']")).Click();


        }

        private void button9_Click(object sender, EventArgs e)
        {
            Program.DeleteSelectedItems(this.dataGridView1);

            //Update The Pins File After Delete
            Program.ExportToFile("input.txt", this.dataGridView1, Program.PinFileHeader);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 1 || this.dataGridView1.SelectedRows.Count ==0)
            {
                MessageBox.Show("Please Select One Row");
            }
            else
            {
                Pin_Edit fr = new Pin_Edit(this);
                fr.Show();
            }
        }
    }
}
