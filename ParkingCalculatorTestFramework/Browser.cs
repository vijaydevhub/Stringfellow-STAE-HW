using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace ParkingCalculatorTestFramework
{
    public static class Browser
    {
        private static IWebDriver driver;

        //Page element names. Update if these change in the future
        public static string lotId = "Lot";
        public static string entryTimeId = "EntryTime";
        public static string entryDateId = "EntryDate";
        public static string exitTimeId = "ExitTime";
        public static string exitDateId = "ExitDate";
        public static string entryAMPMName = "EntryTimeAMPM";
        public static string exitAMPMName = "ExitTimeAMPM";
        public static string submitName = "Submit";
        public static string calendarIconCssSelector = "img[alt=\"Pick a date\"]";
        public static string yearToggleCssSelector = "font";
        public static string monthSelectorName = "MonthSelector";

        public static void GoTo(string url)
        {
            //Navigate to a given URL
            driver = new FirefoxDriver();
            driver.Url = url;
        }

        public static void Close()
        {
            //Close the browser
            driver.Close();
        }

        public static void SelectLotByName(string lotName)
        {
            //Select a Lot from the dropdown based on the lotName
            new SelectElement(driver.FindElement(By.Id(lotId))).SelectByText(lotName);
        }

        public static void EnterEntryTime(string entryTime)
        {
            //Clears out the entry time and replaces with a new entryTime
            driver.FindElement(By.Id(entryTimeId)).Clear();
            driver.FindElement(By.Id(entryTimeId)).SendKeys(entryTime);
        }

        public static void EnterEntryDate(string entryDate)
        {
            //Clears out the entry date and replaces with a new entryDate
            driver.FindElement(By.Id(entryDateId)).Clear();
            driver.FindElement(By.Id(entryDateId)).SendKeys(entryDate);
        }

        public static void EnterExitTime(string exitTime)
        {
            //Clears out the exit time and replaces with a new exitTime
            driver.FindElement(By.Id(exitTimeId)).Clear();
            driver.FindElement(By.Id(exitTimeId)).SendKeys(exitTime);
        }

        public static void EnterExitDate(string exitDate)
        {
            //Clears out the exit date and replaces with a new exitDate
            driver.FindElement(By.Id(exitDateId)).Clear();
            driver.FindElement(By.Id(exitDateId)).SendKeys(exitDate);
        }

        public static void SelectAmOrPm(string morningOrAfternoon)
        {
            //If input is Entry change the entry time to PM, if input is Exit change exit time to PM
            if (morningOrAfternoon == "Entry")
            {
                driver.FindElements(By.Name(entryAMPMName))[1].Click();
            }
            else if (morningOrAfternoon == "Exit")
            {
                driver.FindElements(By.Name(exitAMPMName))[1].Click();
            }
        }

        public static void Submit()
        {
            //Submit the page
            driver.FindElement(By.Name(submitName)).Click();
        }

        public static void SelectDateInCalendar(string entryMonth, string entryDay)
        {
            //Find the calendar popup window
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
            driver.FindElement(By.CssSelector(yearToggleCssSelector)).Click();
            new SelectElement(driver.FindElement(By.Name(monthSelectorName))).SelectByText(entryMonth);
            driver.FindElement(By.LinkText(entryDay)).Click();

            //Switch focus back to the main window
            driver.SwitchTo().Window(existingWindowHandle);
        }

        public static void ClickCalendarIcon(int calendarIndex)
        {
            //Click a calendar icon base on the calendarIndex
            driver.FindElements(By.CssSelector(calendarIconCssSelector))[calendarIndex].Click();
        }

        public static string GetCostOrError()
        {
            //Get the information for cost or error message.
            return driver.FindElement(By.CssSelector("b")).Text.Trim();
        }

        public static string GetDuration()
        {
            //Get the information for duration
            return driver.FindElement(By.CssSelector("span.BodyCopy > font > b")).Text.Trim();
        }

        public static void ClearDate(string dateEntryOrExit)
        {
            //Clear out entry or exit date
            if (dateEntryOrExit == "Entry")
            {
                driver.FindElement(By.Id(entryDateId)).Clear();
            }
            else if (dateEntryOrExit == "Exit")
            {
                driver.FindElement(By.Id(exitDateId)).Clear();
            }
        }

        public static void ClearTime(string timeEntryOrExit)
        {
            //Clear out entry or exit time
            if (timeEntryOrExit == "Entry")
            {
                driver.FindElement(By.Id(entryTimeId)).Clear();
            }
            else if (timeEntryOrExit == "Exit")
            {
                driver.FindElement(By.Id(exitTimeId)).Clear();
            }
        }

        public static bool GetValueOfLot(int lotIndex)
        {
            //Check if a certain lot is selected in the drop down
            return driver.FindElement(By.Id(lotId)).FindElements(By.TagName("option"))[lotIndex].Selected;
        }

        public static bool GetValueOfEntryPm(int amPmIndex)
        {
            //Check if the entry time is marked as AM or PM
            return driver.FindElements(By.Name(entryAMPMName))[amPmIndex].Selected;
        }

        public static string GetEntryTime()
        {
            //Get the value of the entry time
            return driver.FindElement(By.Id(entryTimeId)).GetAttribute("value");
        }

        public static string GetEntryDate()
        {
            //Get the value of the entry date
            return driver.FindElement(By.Id(entryDateId)).GetAttribute("value");
        }

        public static string GetExitTime()
        {
            //Get the value of the exit time
            return driver.FindElement(By.Id(exitTimeId)).GetAttribute("value");
        }

        public static string GetExitDate()
        {
            //Get the value of the exit date
            return driver.FindElement(By.Id(exitDateId)).GetAttribute("value");
        }
    }
}