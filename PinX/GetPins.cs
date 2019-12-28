using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinX
{
    public partial class GetPins : Form
    {
        public GetPins()
        {
            InitializeComponent();
        }

        private void GetPins_Load(object sender, EventArgs e)
        {

        }

        private void GoButton_Click(object sender, EventArgs e)
        {




            foreach (var accountinfo in Program.ReadAccountsFile())
            {


                if (accountinfo.Item2 == "Test" || accountinfo.Item2 == "test" || accountinfo.Item2 == "TEST")
                {
                   // System.IO.StreamWriter file = new System.IO.StreamWriter(textBox5.Text);

                    FirefoxDriver driver1 = Program.Make_Driver(accountinfo.Item1);


                    driver1.Manage().Window.Maximize();
                    driver1.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(180);

                    string path = @textBox5.Text;


                    string SourceURL = textBox2.Text;


                    int PageLong = System.Convert.ToInt32(textBox3.Text);

                    int PinPagewait = System.Convert.ToInt32(textBox4.Text);


                    File.AppendAllText(path, "URL:" + SourceURL + Environment.NewLine, Encoding.UTF8);



                    driver1.Navigate().GoToUrl(SourceURL);


                    Thread.Sleep(3000);


                    for (int i = 0; i < PageLong; i++) //Scroll In The Page
                    {
                        ((IJavaScriptExecutor)driver1).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
                        Thread.Sleep(2500);
                    }

                    IList<IWebElement> PinsInPage = driver1.FindElements(By.CssSelector("*[class^='XiG zI7 iyn Hsu']"));


                    //Bulid an List To Putt All The Pins URLS in (To Solve DOM Problem)
                    List<string> list = new List<string>();


                    //  List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();



                    bool flag = false;


                    Debug.WriteLine("Pins in page:" + PinsInPage.Count);

                    //Put The Articles URLs in The Array
                    for (int i = 0; i < PinsInPage.Count; i++)
                    {
                        string theurl = "";

                        try
                        {
                            theurl = PinsInPage[i].FindElement(By.CssSelector("a")).GetAttribute("href");
                            // Debug.WriteLine("URL:"+theurl);
                            flag = true;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        finally
                        {
                            if (flag)
                            {
                                //  list2.Add(new KeyValuePair<string, string>(theurl, Title));

                                list.Add(theurl);
                            }
                            flag = false;
                        }
                    }


                    //Remove the Repeated items from the list
                    list = list.Distinct().ToList();


                    list.RemoveRange(0, 4); //Remove First Wrong Urls


                    int counter = 0;


                    foreach (string s in list) //Go to all the Pins!
                    {

                        counter++;


                        //wait 3 minuts
                        if (counter%15 == 0) Thread.Sleep(180000);


                        //wait 7 minuts
                        if (counter % 50 == 0) Thread.Sleep(420000);


                        string Saves, FinalSaves;

                        Start:

                        //  driver1.Url = s;

                        try
                        {


                            driver1.Navigate().GoToUrl(s);

                            Thread.Sleep(PinPagewait); //Wait to reload the page



                            Saves = driver1.FindElement(By.CssSelector("*[class^='Eqh l7T zI7 iyn Hsu']")).Text;

                            FinalSaves = Regex.Replace(Saves, "[^0-9.]", ""); //remove "pin" wor

                            string checkK = Regex.Replace(Saves, "[^k.]", "");



                            if (checkK == "k")
                            {
                                File.AppendAllText(path, s + "*" + FinalSaves + "000" + Environment.NewLine, Encoding.UTF8);
                            }
                            else if (System.Convert.ToInt32(FinalSaves) >= System.Convert.ToInt32(textBox1.Text))
                            {
                                File.AppendAllText(path, s + "*" + FinalSaves + Environment.NewLine, Encoding.UTF8);
                            }


                        }
                        catch (Exception)
                        {
                            goto Start;
                        }




                    }


                    driver1.Quit();


                }
            }



           

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
