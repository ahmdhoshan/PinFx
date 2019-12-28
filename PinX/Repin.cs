using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
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
    public partial class Repin : Form
    {

        static public bool ActiveAccount = true;



        public Repin()
        {
            InitializeComponent();
        }

        private void Repin_Load(object sender, EventArgs e)
        {
            Program.ReadPinsFile("repin.txt", this.dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.DeleteSelectedItems(this.dataGridView1);

            //Update The Repins File After Delete
            Program.ExportToFile("repin.txt", this.dataGridView1, Program.RePinFileHeader);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 1 || this.dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select One Row");
            }
            else
            {

                Repin_Edit fr = new Repin_Edit(this);
                fr.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int RowNumber;
            foreach (var accountinfo in Program.ReadAccountsFile())
            {
                RowNumber = 0;
                bool trueproxy = true;

                if (accountinfo.Item2 == "Yes" || accountinfo.Item2 == "yes" || accountinfo.Item2 == "YES")
                {

                    //Bulid a driver
                    //Set a custom profile
                    FirefoxProfile profile = new FirefoxProfile((Program.FireFoxProfilesPath + accountinfo.Item1));
                    FirefoxOptions options = new FirefoxOptions();
                    options.Profile = profile;
                    FirefoxDriver driver = new FirefoxDriver(options);


                    if (IPCheck_CheckBox.Checked)
                    {
                        if (!Program.CheckProxy(ref driver, accountinfo.Item5)) trueproxy = false;
                    }

                    if (trueproxy)
                    {

                        foreach (var pin in Program.ReadRepinFile())
                        {
                            if (Program.RepinaPin(pin, ref driver, accountinfo.Item3))
                            {
                                dataGridView1.Rows[RowNumber].Cells[2].Value = dataGridView1.Rows[RowNumber].Cells[2].Value + accountinfo.Item4 + ",";
                            }
                            RowNumber++;



                            if (HumanAct_checkBox.Checked) //Do human Act
                            {
                                Program.HumanAct(ref driver, accountinfo.Item3, Int32.Parse(DelayRange01_textBox.Text), Int32.Parse(DelayRange02_textBox.Text), Int32.Parse(HumanActPinsNum01_textBox.Text), Int32.Parse(HumanActPinsNum02_textBox.Text), Int32.Parse(PageLoadTime_textBox.Text), Int32.Parse(BetweenTasksDelay_textBox.Text), Int32.Parse(HowMuchToTry_textBox.Text));
                            }

                        }

                    }
                    else
                    {
                        //Proxy problem in account!

                    }



                    driver.Close();
                }


            }





        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void RepinFromURL_Click(object sender, EventArgs e)
        {

            //Make history file to save the steps
            System.IO.StreamWriter file = new System.IO.StreamWriter("history.txt");
            file.AutoFlush = true;
            file.WriteLine("The URL TO REPIN From:" + URLToRepinFrom_textBox.Text + System.Environment.NewLine);


            int PageLoadTime = Int32.Parse(PageLoadTime_textBox.Text);
            int HowMuchToTry = Int32.Parse(HowMuchToTry_textBox.Text);
            int BetweenTasksDelay = Int32.Parse(BetweenTasksDelay_textBox.Text);
            int DelayRange01 = Int32.Parse(DelayRange01_textBox.Text);
            int DelayRange02 = Int32.Parse(DelayRange02_textBox.Text);



            foreach (var accountinfo in Program.ReadAccountsFile())
            {


                if (accountinfo.Item2 == "Yes" || accountinfo.Item2 == "yes" || accountinfo.Item2 == "YES")
                {
                    //Add The account username
                    file.WriteLine("Account Username:" + accountinfo.Item4 + "-->");
                    int pinNumber = 0, increaseTime = 0 ;
                    bool trueproxy = true;


                    /*
                    //Bulid a driver
                    //Set a custom profile
                    FirefoxProfile profile = new FirefoxProfile((Program.FireFoxProfilesPath + accountinfo.Item1));
                    FirefoxOptions options = new FirefoxOptions();


                    options.Profile = profile;
                    FirefoxDriver driver = new FirefoxDriver(options);

                    */


                    FirefoxDriver driver = Program.Make_Driver(accountinfo.Item1);


                    driver.Manage().Window.Maximize();
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(180);


                    if (IPCheck_CheckBox.Checked)
                    {
                        if (!Program.CheckProxy(ref driver, accountinfo.Item5)) trueproxy = false;
                    }


                    if (trueproxy)
                    {

                       
                        for (int i = 0; i < Int32.Parse(NumberOfPinsToRepin_textBox.Text); i++)
                        {
                            int HowMuch = 0, HowMuch2 = 0, flag = 0;

                            String currentURL;

                            IList<IWebElement>[] PinsInPage = new IList<IWebElement>[HowMuchToTry+1];

                            ReloadPage:

                            if (HowMuch > HowMuchToTry) //If We Tried 5 Times to Reload the page then stop!
                            {
                                file.WriteLine("**Error: We can't get data ater " + HowMuch + "Tries");
                                goto NextAccount;
                            }

                            try
                            {
                                driver.Navigate().GoToUrl(URLToRepinFrom_textBox.Text);
                            }
                            catch
                            {
                                //Define a new driver if the old one faild
                               if(NewDriverWhenFail_checkBox.Checked)
                                {
                                    driver.Close();
                                    driver = Program.Make_Driver(accountinfo.Item1);
                                }
                            }

                            //Page load time
                            Thread.Sleep(PageLoadTime + increaseTime * HowMuch);

                            try
                            {

                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight/13);");

                                //Tiny click delay
                                Thread.Sleep(BetweenTasksDelay + increaseTime * HowMuch);


                                //Get the pins in the page
                                PinsInPage[HowMuch] = driver.FindElements(By.CssSelector("*[class^='MIw QLY Rym ojN p6V zI7 iyn Hsu']"));
                                //Tiny click delay
                                Thread.Sleep(BetweenTasksDelay + increaseTime* HowMuch);
                                PinsInPage[HowMuch][pinNumber + Int32.Parse(textBox7.Text)].Click();
                                currentURL = driver.Url; //Get the current page url
                            }
                            catch(Exception ex)
                            {
                                Debug.WriteLine("(Repin)-->The i:" + i + " How Much:" + HowMuch + " The Exceprion:" + ex.ToString());
                                HowMuch++;
                                Debug.WriteLine("Im Here 1");
                                if (MultiplyTimeWhenFail_checkBox.Checked) increaseTime = BetweenTasksDelay;
                                goto ReloadPage;
                            }

                            Debug.WriteLine("PIIINS NUUUM:" + PinsInPage[HowMuch].Count);

                            //Retry The New Driver After Bulid it!
                            RetryDriver:

                            PageLoad2:

                            if (flag == 1)
                            {
                                try
                                {
                                    driver.Navigate().GoToUrl(currentURL);
                                }
                                catch
                                {
                                    //Define a new driver if the old one faild
                                    if (NewDriverWhenFail_checkBox.Checked)
                                    {
                                        driver.Close();
                                        driver = Program.Make_Driver(accountinfo.Item1);
                                        goto RetryDriver;
                                    }
                                }
                            }
                                


                            if (HowMuch2 > HowMuchToTry) //If We Tried 5 Times to Reload the page then stop!
                            {
                                file.WriteLine("**Error: We can't get data ater " + HowMuch2 + "Tries");
                                goto NextAccount;
                            }



                            //Page Load Time
                            Thread.Sleep(PageLoadTime + increaseTime);
                            string[] words = accountinfo.Item3.Split('*'); //Get Boards Names
                            Random rnd = new Random();
                            //Generate eandom number to select random board
                            string BoardName = words[rnd.Next(0, words.Length)];


                            try
                            {
                            //click on the shape to fill board name
                            driver.FindElement(By.CssSelector("*[class^='PinBetterSave__DropdownText']")).Click();
                            //Tiny click delay
                            Thread.Sleep(BetweenTasksDelay);
                            //fill the board name
                            driver.FindElement(By.XPath("//*[@id='pickerSearchField']")).SendKeys(BoardName);
                            //Click The board Name
                            driver.FindElement(By.CssSelector("*[class^='tBJ dyH iFc SMy yTZ pBj DrD IZT mWe z-6']")).Click();
                             //Save that the pin was saved in the history.txt
                             file.WriteLine("Original Pin (" + i + "):" + currentURL + "Saved");
                            }
                            catch
                            {
                                HowMuch2++;
                                Debug.WriteLine("Im Here 2");
                                flag = 1;
                                if (MultiplyTimeWhenFail_checkBox.Checked) increaseTime = BetweenTasksDelay;
                                goto PageLoad2;
                            }

                            if (HumanAct_checkBox.Checked) //Do human Act
                            {
                                //Check If the human act fails!
                                Program.HumanAct(ref driver, accountinfo.Item3, DelayRange01, DelayRange02, Int32.Parse(HumanActPinsNum01_textBox.Text), Int32.Parse(HumanActPinsNum02_textBox.Text), PageLoadTime, BetweenTasksDelay, HowMuchToTry, increaseTime,file, NewDriverWhenFail_checkBox);

                                //Check If the account is ok:
                                if (!ActiveAccount) //if the flag is false
                                {

                                }
                            }


                            pinNumber++;

                            //Delay with a random value
                            int DelayValue = rnd.Next(DelayRange01, DelayRange02);
                            Thread.Sleep(DelayValue);

                            //Clear the pins array
                            Array.Clear(PinsInPage, 0, Int32.Parse(HowMuchToTry_textBox.Text));
                        }

                    }
                    else
                    {
                        file.WriteLine("**Error: There is a problem with proxy (Or there is no internet!)");
                    }


                    driver.Close();
                }


                //If there is an error with an account move to this line!
                NextAccount:

                //Add new line to history.txt file
                file.WriteLine(System.Environment.NewLine);
            }

            MessageBox.Show("All done,Please read history.txt file");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
