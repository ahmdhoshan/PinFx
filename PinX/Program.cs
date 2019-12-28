using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;

namespace PinX
{
    static class Program
    {

        static public string PinFileHeader = "Title,Image,Link,Description,Pinned";
        static public string AccountsFileHeader = "Email,Password,UserName,FireFoxProfile,Proxy,Active,Defult Boards";
        static public string RePinFileHeader = "Pin Id,Source URL,Repin By";
        static public string FireFoxProfilesPath = @"C:\\Users\\Ali\\AppData\\Roaming\\Mozilla\\Firefox\\Profiles\\";
        static public string repinfile = "repin.txt";
        static public string accountsfile = "accounts.txt";





        static public FirefoxDriver Make_Driver(string AccountProfile)
        {
            //Bulid a driver
            //Set a custom profile
            FirefoxProfile profile = new FirefoxProfile((FireFoxProfilesPath + AccountProfile));
            FirefoxOptions options = new FirefoxOptions();


            options.Profile = profile;


            FirefoxDriver driver = new FirefoxDriver(options);




            return driver;
        }







        static public IEnumerable<string> ReadRepinFile()
        {
            var lines = File.ReadAllLines(repinfile);
            if (lines.Count() > 0)
            {
                foreach (var cellValues in lines.Skip(1))
                {
                    var cellArray = cellValues
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    yield return cellArray[0];
                }
            }
        }




        static public bool CheckProxy(ref FirefoxDriver driver, string AcountProxy)
        {
            Thread.Sleep(2000);

            driver.Navigate().GoToUrl("https://getfoxyproxy.org/geoip/");

            if(driver.FindElement(By.XPath("/html/body/div[2]/div/div/strong[1]")).Text == AcountProxy)
            {
                return true;
            }
            return false;
        }








        static public void HumanAct(ref FirefoxDriver driver, string defultboards,int delay1, int delay2,int humanAct1,int humanAct2,int pageloadTime,int TinyclickTime,int HowMuchToTry,int IncreaseTime=0, System.IO.StreamWriter file=null,CheckBox CreatDriverCheck = null)
        {






            string[] words = defultboards.Split('*'); //Get Boards Names

            Random rnd = new Random();


            //Generate eandom number to select random Pins Count
            int NumberOfPins = rnd.Next(humanAct1, humanAct2);



            Debug.WriteLine("Number Of Random Pins:" + NumberOfPins);


            Thread.Sleep(pageloadTime);





            for (int i = 0; i < NumberOfPins; i++)
            {
                int HowMuch = 0;


                IList<IWebElement>[] PinsInPage = new IList<IWebElement>[HowMuchToTry+1];


                ReloadPage:



                if (HowMuch > HowMuchToTry) //If We Tried 5 Times to Reload the page then stop!
                {
                    file.WriteLine("**Error (Random Pin Try): We can't get data ater " + HowMuch + "Tries");
                    goto NextPin;
                }

                driver.Navigate().GoToUrl("https://www.pinterest.com/");

                Thread.Sleep(pageloadTime + IncreaseTime*HowMuch);


                //Generate eandom number to select random delay
                int DelayValue = rnd.Next(delay1, delay2);
                //Generate eandom number to select random board
                string BoardName = words[rnd.Next(0, words.Length)];
                int HowMuch2 = 0,flag=0;
                String currentURL;

                try
                {
                    //Get the pins in the page
                    PinsInPage[HowMuch] = driver.FindElements(By.CssSelector("*[class^='MIw QLY Rym ojN p6V zI7 iyn Hsu']"));
                    Debug.WriteLine("Number Of Pins:" + PinsInPage[i+1].Count);
                    PinsInPage[HowMuch][i+1].Click();
                    currentURL = driver.Url; //Get the current page url
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("(Program)-->The i:" + i + " How Much:" + HowMuch + " The Exceprion:" + ex.ToString());
                    Debug.WriteLine("Im Here 01");
                    HowMuch++;
                    goto ReloadPage;
                }


                ReloadPage2:
                if(flag==1) driver.Navigate().GoToUrl(currentURL);

                if (HowMuch2 > HowMuchToTry) //If We Tried 5 Times to Reload the page then stop!
                {
                    file.WriteLine("**Error (Random Pin Try): We can't get data ater " + HowMuch + "Tries");
                    goto NextPin;
                }



                Thread.Sleep(pageloadTime +IncreaseTime * HowMuch);

                try
                {

                //click on the shape to fill board name
                driver.FindElement(By.CssSelector("*[class^='PinBetterSave__DropdownText']")).Click();


                Thread.Sleep(TinyclickTime);

                //fill the board name
                driver.FindElement(By.XPath("//*[@id='pickerSearchField']")).SendKeys(BoardName);

                //Click The board Name
                driver.FindElement(By.CssSelector("*[class^='tBJ dyH iFc SMy yTZ pBj DrD IZT mWe z-6']")).Click();


                Debug.WriteLine("Delay:" + DelayValue);
                Debug.WriteLine("Pins Number:" + NumberOfPins);

                 //Save that the pin was saved in the history.txt
                 file.WriteLine("Random Pin:" + currentURL + "Saved");

                }
                catch
                {
                    Debug.WriteLine("Im Here 02");
                    flag = 1;
                    HowMuch2++;
                    goto ReloadPage2;
                }


                //Clear the pins array
                Array.Clear(PinsInPage, 0, HowMuchToTry);
                Thread.Sleep(DelayValue);

                NextPin:;
            }


        }











        static public bool RepinaPin(string pinid, ref FirefoxDriver driver,string defultboards)
        {
            try
            {
                //Bulid a pin URL
                string pinurl = "https://www.pinterest.com/pin/" + pinid + "/";

                driver.Navigate().GoToUrl(pinurl);

                string[] words = defultboards.Split('*'); //Get Boards Names

                //Generate eandom number to select random board
                Random rnd = new Random();

                string BoardName = words[rnd.Next(0, words.Length)];

                Thread.Sleep(2000);

                //click save
                driver.FindElement(By.CssSelector("*[class^='PinBetterSave__DropdownText']")).Click();




                Thread.Sleep(2000);


                //fill the board name
                driver.FindElement(By.XPath("//*[@id='pickerSearchField']")).SendKeys(BoardName);

                //Click The board Name
                driver.FindElement(By.CssSelector("*[class^='tBJ dyH iFc SMy yTZ pBj DrD IZT mWe z-6']")).Click();



                Thread.Sleep(2000);

                return true;
            }
            catch
            {
                return false;
            }

        }



        static public IEnumerable<Tuple<string, string, string, string, string>> ReadAccountsFile()
        {
            Tuple<string, string, string, string, string> t; //Make a Pair
            var lines = File.ReadAllLines(accountsfile);

            if (lines.Count() > 0)
            {
                foreach (var cellValues in lines.Skip(1))
                {
                    var cellArray = cellValues
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    t = new Tuple<string, string, string, string, string>(cellArray[3], cellArray[5], cellArray[6], cellArray[2], cellArray[4]);
                    yield return t;
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }


        static public void DeleteSelectedItems(DataGridView dataGridView1)
        {
            //Delete Selected Items
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(row.Index);
            }
            //Save To File ater Delete
        }



        public static void ExportToFile(string filePAth, DataGridView dataGridView1,string fileHeader)
        {
            //This line of code creates a text file for the data export.
            System.IO.StreamWriter file = new System.IO.StreamWriter(filePAth);
            try
            {
                
                file.WriteLine(fileHeader);

                string sLine = "";

                //This for loop loops through each row in the table
                for (int r = 0; r <= dataGridView1.Rows.Count - 2; r++)
                {
                    //This for loop loops through each column, and the row number
                    //is passed from the for loop above.
                    for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                    {
                        sLine = sLine + dataGridView1.Rows[r].Cells[c].Value;
                        if (c != dataGridView1.Columns.Count - 1)
                        {
                            //A comma is added as a text delimiter in order
                            //to separate each field in the text file.
                            //You can choose another character as a delimiter.
                            sLine = sLine + ",";
                        }
                    }
                    //The exported text is written to the text file, one line at a time.
                    file.WriteLine(sLine);
                    sLine = "";
                }

                file.Close();
                System.Windows.Forms.MessageBox.Show("Export Complete.", "Program Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                file.Close();
            }


        }





        public static void ReadPinsFile(string filename, DataGridView dataGridView1)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var lines = File.ReadAllLines(filename);
            if (lines.Count() > 0)
            {
                foreach (var columnName in lines.FirstOrDefault()
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dataGridView1.Columns.Add(columnName, columnName);
                }
                foreach (var cellValues in lines.Skip(1))
                {
                    var cellArray = cellValues
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (cellArray.Length == dataGridView1.Columns.Count || cellArray.Length == dataGridView1.Columns.Count-1)
                        dataGridView1.Rows.Add(cellArray);
                }
            }
        }






    }
}
