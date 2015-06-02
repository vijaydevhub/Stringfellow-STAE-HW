using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;



namespace MBSoftwareTestAutomationEngineerHW
{
    [TestClass]
    class ParkingCalculatorTests
    {
        static void Main(string[] args)
        {
            ShortTermParkingOneHourTest();

            LongTermSurfaceParkingOneMonthTest();

            ShortTermParkingEntryDateAfterExitDateTest();

            SubmitOnPageLoadTest();

            ShortTermparkingEntryTimeAfterExtiTimeTest();

            FieldsMatchAfterSubmitionTest();

            InvalidTimeWith00FollowingTest();

            InvalidTimeWithOut00FollowingTest();

            EmptyEntryAndExitDateTest();

            EmptyEntryAndExitTimesTest();

            EmptyEntryDateTest();

            EmptyExitDateTest();

            ShortTermParkingEntryAndExitSameTimeTest();

            EconomyParkingEntryAndExitSameTimeTest();

            Console.WriteLine("The tests have finished running. Failed results are above.");
        }

        [TestMethod]
        public static void ShortTermParkingOneHourTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Short-Term Parkin for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Short-Term Parking");

            //Clear out Entry Time and replace with 10:00
            driver.FindElement(By.Id("EntryTime")).Clear();
            driver.FindElement(By.Id("EntryTime")).SendKeys("10:00");

            //Clear out Entry Date and replace with 01/01/2014
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2014");

            //Select PM for the Entry Time
            driver.FindElements(By.Name("EntryTimeAMPM"))[1].Click();

            //Clear out Exit Time and replace with 11:00
            driver.FindElement(By.Id("ExitTime")).Clear();
            driver.FindElement(By.Id("ExitTime")).SendKeys("11:00");

            //Clear out Exit Date and replace with 01/01/2014
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Select PM for the Exit Time
            driver.FindElements(By.Name("ExitTimeAMPM"))[1].Click();

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that the cost after submition is $2.00 and the duration is 1 hour
            try
            {
                Assert.AreEqual("$ 2.00", driver.FindElement(By.CssSelector("b")).Text);
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The price of one hour of short term parking was not calculated correctly.");
            }

            try
            {
                Assert.AreEqual("(0 Days, 1 Hours, 0 Minutes)",
                    driver.FindElement(By.CssSelector("span.BodyCopy > font > b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The duration for one hour of short term parking was not calculated correctly.");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void LongTermSurfaceParkingOneMonthTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Long-Term Surface Parking for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Long-Term Surface Parking");

            //Click on the Entry Date calendar
            driver.FindElement(By.CssSelector("img[alt=\"Pick a date\"]")).Click();

            //Find the popup window and keep track of the main window
            string popupHandle = string.Empty;
            ReadOnlyCollection<string> windowHandles = driver.WindowHandles;

            string existingWindowHandle = driver.CurrentWindowHandle;

            foreach (string handle in windowHandles)
            {
                if (handle != existingWindowHandle)
                {
                    popupHandle = handle;
                    break;
                }
            }

            //Switch focus to the popup window
            driver.SwitchTo().Window(popupHandle);

            //Select January 1st, 2014 in the calendar
            driver.FindElement(By.CssSelector("font")).Click();
            new SelectElement(driver.FindElement(By.Name("MonthSelector"))).SelectByText("January");
            driver.FindElement(By.LinkText("1")).Click();

            //Switch focus back to the main window
            driver.SwitchTo().Window(existingWindowHandle);

            //Repeat steps from Entry Date for Exit Date
            driver.FindElement(By.XPath("(//img[@alt='Pick a date'])[2]")).Click();

            string popupHandle2 = string.Empty;
            ReadOnlyCollection<string> windowHandles2 = driver.WindowHandles;

            string existingWindowHandle2 = driver.CurrentWindowHandle;

            foreach (string handle in windowHandles2)
            {
                if (handle != existingWindowHandle2)
                {
                    popupHandle2 = handle;
                    break;
                }
            }

            driver.SwitchTo().Window(popupHandle2);

            //Select February 1st, 2014 from the calendar
            driver.FindElement(By.CssSelector("font")).Click();
            new SelectElement(driver.FindElement(By.Name("MonthSelector"))).SelectByText("February");
            driver.FindElement(By.LinkText("1")).Click();

            driver.SwitchTo().Window(existingWindowHandle);

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that the cost is $270.00 and that the duration was 31 Days
            try
            {
                Assert.AreEqual("$ 270.00", driver.FindElement(By.CssSelector("b")).Text);
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The price of one hour of short term parking was not calculated correctly.");
            }

            try
            {
                Assert.AreEqual("(31 Days, 0 Hours, 0 Minutes)",
                    driver.FindElement(By.CssSelector("span.BodyCopy > font > b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The duration for one hour of short term parking was not calculated correctly.");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void ShortTermParkingEntryDateAfterExitDateTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Short-Term Parking for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Short-Term Parking");

            //Clear the Entry Date and replace with 01/02/2014
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/02/2014");

            //Clear the Exit Date and replace with 01/01/2014
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get an error letting you know that your Exit Date is before your Entry Date
            try
            {
                Assert.AreEqual("ERROR! Your Exit Date Or Time Is Before Your Entry Date or Time".ToUpper(), driver.FindElement(By.CssSelector("b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The error for the Exit Date before the Entry Date is not working properly!");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void SubmitOnPageLoadTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get an Error telling you that you have an invalid date format
            try
            {
                Assert.AreEqual("ERROR! Enter A Correctly Formatted Date".ToUpper(), driver.FindElement(By.CssSelector("b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("Submitting the page upon page load did not work correctly!");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void ShortTermparkingEntryTimeAfterExtiTimeTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Short-Term Parking for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Short-Term Parking");

            //Clear out Entry Time and replace with 12:00
            driver.FindElement(By.Id("EntryTime")).Clear();
            driver.FindElement(By.Id("EntryTime")).SendKeys("11:00");

            //Clear out Entry Date and replace with 01/01/2014
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2014");

            //Clear out Exit Time and replace with 11:00
            driver.FindElement(By.Id("ExitTime")).Clear();
            driver.FindElement(By.Id("ExitTime")).SendKeys("8:00");

            //Clear out Exit Date and replace with 01/01/2014
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get and error telling you that your Exit Time is before your Entry Time
            try
            {
                Assert.AreEqual("ERROR! Your Exit Date Or Time Is Before Your Entry Date or Time".ToUpper(), driver.FindElement(By.CssSelector("b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The error for the Exit Time before the Entry Time is not working properly!");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void InvalidTimeWith00FollowingTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Short-Term Parking for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Short-Term Parking");

            //Clear out Entry Time and replace with 159357:00
            driver.FindElement(By.Id("EntryTime")).Clear();
            driver.FindElement(By.Id("EntryTime")).SendKeys("159357:00");

            //Clear out Entry Date and replace with 01/01/2014
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2014");

            //Clear out Exit Time and replace with 11:00
            driver.FindElement(By.Id("ExitTime")).Clear();
            driver.FindElement(By.Id("ExitTime")).SendKeys("11:00");

            //Clear out Exit Date and replace with 01/01/2014
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get an error for your invalid time
            try
            {
                Assert.AreEqual("ERROR! Your Entry or Exit Time Is Invalid".ToUpper(), driver.FindElement(By.CssSelector("b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("Invalid times are being accepted!");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void InvalidTimeWithOut00FollowingTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Short-Term Parking for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Short-Term Parking");

            //Clear out Entry Time and replace with 159357
            driver.FindElement(By.Id("EntryTime")).Clear();
            driver.FindElement(By.Id("EntryTime")).SendKeys("159357");

            //Clear out Entry Date and replace with 01/01/2014
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2014");

            //Clear out Exit Time and replace with 11:00
            driver.FindElement(By.Id("ExitTime")).Clear();
            driver.FindElement(By.Id("ExitTime")).SendKeys("11:00");

            //Clear out Exit Date and replace with 01/01/2014
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get an error for invalid time
            try
            {
                Assert.AreEqual("ERROR! Your Entry or Exit Time Is Invalid".ToUpper(), driver.FindElement(By.CssSelector("b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("Invalid times are being accepted!");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void FieldsMatchAfterSubmitionTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Long-Term Surface Parking for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Long-Term Surface Parking");

            //Select PM for the Entry Time
            driver.FindElements(By.Name("EntryTimeAMPM"))[1].Click();

            //Clear out Entry Time and replace with 10:00
            driver.FindElement(By.Id("EntryTime")).Clear();
            driver.FindElement(By.Id("EntryTime")).SendKeys("10:00");

            //Clear out Entry Date and replace with 01/01/2014
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2014");

            //Clear out Exit Time and replace with 11:00
            driver.FindElement(By.Id("ExitTime")).Clear();
            driver.FindElement(By.Id("ExitTime")).SendKeys("11:00");

            //Clear out Exit Date and replace with 01/01/2014
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/02/2014");

            //Get all the current page values before submit and save them to variable to be checked against after page submition
            var selectedLot = driver.FindElement(By.Id("Lot")).FindElements(By.TagName("option"))[2].Selected;
            var radioButton = driver.FindElements(By.Name("EntryTimeAMPM"))[1].Selected;
            var entryTime = driver.FindElement(By.Id("EntryTime")).GetAttribute("value");
            var entryDate = driver.FindElement(By.Id("EntryDate")).GetAttribute("value");
            var exitTime = driver.FindElement(By.Id("ExitTime")).GetAttribute("value");
            var exitDate = driver.FindElement(By.Id("ExitDate")).GetAttribute("value");
            
            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that all of the page values before page submition are the same after page submition
            try
            {
                Assert.AreEqual(entryTime, driver.FindElement(By.Id("EntryTime")).GetAttribute("value"));
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The Entry Time was not carried over after submit");
            }

            try
            {
                Assert.AreEqual(entryDate, driver.FindElement(By.Id("EntryDate")).GetAttribute("value"));
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The Entry Date was not carried over after submit");
            }

            try
            {
                Assert.AreEqual(exitTime, driver.FindElement(By.Id("ExitTime")).GetAttribute("value"));
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The Exit Time was not carried over after submit");
            }

            try
            {
                Assert.AreEqual(exitDate, driver.FindElement(By.Id("ExitDate")).GetAttribute("value"));
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The Exit Date was not carried over after submit");
            }

            try
            {
                Assert.AreEqual(radioButton, driver.FindElements(By.Name("EntryTimeAMPM"))[1].Selected);
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The AM/PM radio button was not carried over after submit");
            }

            try
            {
                Assert.AreEqual(selectedLot, driver.FindElement(By.Name("Lot")).FindElements(By.TagName("option"))[2].Selected);
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The selected lot was not carried over after submit");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void EmptyEntryAndExitDateTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Clear the Entry Date
            driver.FindElement(By.Id("EntryDate")).Clear();

            //Clear the Exit Date
            driver.FindElement(By.Id("ExitDate")).Clear();

            //Submit the page
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get an error for invalid date(s)
            try
            {
                Assert.AreEqual("ERROR! Enter A Correctly Formatted Date".ToUpper(), driver.FindElement(By.Name("EntryTimeAMPM")));
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("NULL Entry and Exit Dates are accepted together!");
            }

            //Close the browser
            driver.Close();

        }

        [TestMethod]
        public static void EmptyEntryDateTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Clear the Entry Date
            driver.FindElement(By.Id("EntryDate")).Clear();

            //Clear the Exit Date and replace with 01/01/2014
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get an error for an invalid Entry Date
            try
            {
                Assert.AreEqual("ERROR! Enter A Correctly Formatted Date".ToUpper(), driver.FindElement(By.CssSelector("b")));
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("NULL Entry Date is accepted!");
            }

            //Close the browser
            driver.Close();

        }

        [TestMethod]
        public static void EmptyExitDateTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Clear the Entry Date and replace with 01/01/2014
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2014");

            //Clear the Exit Date
            driver.FindElement(By.Id("ExitDate")).Clear();

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get an error for an invalid Exit Date
            try
            {
                Assert.AreEqual("ERROR! Enter A Correctly Formatted Date".ToUpper(), driver.FindElement(By.CssSelector("b")));
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("NULL Exit Date is accepted!");
            }

            //Close the browser
            driver.Close();

        }

        [TestMethod]
        public static void EmptyEntryAndExitTimesTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Clear the Entry Time
            driver.FindElement(By.Id("EntryTime")).Clear();

            //Clear the Exit Time
            driver.FindElement(By.Id("ExitTime")).Clear();

            //Clear the Entry and Exit Dates and replace with 01/01/2014
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2014");

            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2014");

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that you get an error for invalid time(s)
            try
            {
                Assert.AreEqual("ERROR! Enter A Correctly Formatted Time".ToUpper(),
                    driver.FindElement(By.CssSelector("b")));
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("Blank times are being accepted!");
            }
             
            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void ShortTermParkingEntryAndExitSameTimeTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Short-Term Parking for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Short-Term Parking");

            //Clear the Entry Time and replace with 10:00
            driver.FindElement(By.Id("EntryTime")).Clear();
            driver.FindElement(By.Id("EntryTime")).SendKeys("10:00");

            //Clear the Exit Time and replace with 10:00
            driver.FindElement(By.Id("ExitTime")).Clear();
            driver.FindElement(By.Id("ExitTime")).SendKeys("10:00");

            //Clear the Entry Date and replace with 01/01/2015
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2015");

            //Clear the Exit Date and replace with 01/01/2015
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2015");

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that the cost for 0 hours of Short-Term Parking is $0.00
            try
            {
                Assert.AreEqual("$ 0.00", driver.FindElement(By.CssSelector("b")).Text);
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The cost for zero hours of Short-Term Parking is incorrect!");
            }

            //Assert that the duration is 0 hours
            try
            {
                Assert.AreEqual("(0 Days, 0 Hours, 0 Minutes)",
                    driver.FindElement(By.CssSelector("span.BodyCopy > font > b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The duration for zero hours of short term parking was not calculated correctly.");
            }

            //Close the browser
            driver.Close();
        }

        [TestMethod]
        public static void EconomyParkingEntryAndExitSameTimeTest()
        {
            //Open up the browser and navigate to the Parking Calculator
            IWebDriver driver = new ChromeDriver(@"C:\Users\jacob.stringfellow\Desktop\STAE HW\Drivers");
            driver.Url = "http://adam.goucher.ca/parkcalc/index.php";

            //Select Economy Parking for Lot
            new SelectElement(driver.FindElement(By.Id("Lot"))).SelectByText("Economy Parking");

            //Clear the Entry Time and replace with 10:00
            driver.FindElement(By.Id("EntryTime")).Clear();
            driver.FindElement(By.Id("EntryTime")).SendKeys("10:00");

            //Clear the Exit Time and replace with 10:00
            driver.FindElement(By.Id("ExitTime")).Clear();
            driver.FindElement(By.Id("ExitTime")).SendKeys("10:00");

            //Clear the Entry Date and replace with 01/01/2015
            driver.FindElement(By.Id("EntryDate")).Clear();
            driver.FindElement(By.Id("EntryDate")).SendKeys("01/01/2015");

            //Clear the Exit Date and replace with 01/01/2015
            driver.FindElement(By.Id("ExitDate")).Clear();
            driver.FindElement(By.Id("ExitDate")).SendKeys("01/01/2015");

            //Submit the form
            driver.FindElement(By.Name("Submit")).Click();

            //Assert that the cost for 0 hours of Economy Parking is $0.00
            try
            {
                Assert.AreEqual("$ 0.00", driver.FindElement(By.CssSelector("b")).Text);
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The cost for zero hours of Economy Parking is incorrect!");
            }

            //Assert that the duration is 0 hours
            try
            {
                Assert.AreEqual("(0 Days, 0 Hours, 0 Minutes)",
                    driver.FindElement(By.CssSelector("span.BodyCopy > font > b")).Text.Trim());
            }
            catch (UnitTestAssertException)
            {
                Console.WriteLine("The duration for zero hours of economy parking was not calculated correctly.");
            }

            //Close the browser
            driver.Close();
        }
    }
}
