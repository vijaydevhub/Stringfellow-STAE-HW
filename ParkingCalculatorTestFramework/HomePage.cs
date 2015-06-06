using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingCalculatorTestFramework
{
    public class HomePage
    {
        public static string SiteUrl = "http://adam.goucher.ca/parkcalc/index.php";

        public static void GoTo()
        {
            Browser.GoTo(SiteUrl);
        }

        public static void Submit()
        {
            Browser.Submit();
        }

        public static string Cost()
        {
            return Browser.GetCostOrError();
        }

        public static string Duration()
        {
            return Browser.GetDuration();
        }

        public static string ErrorMessage()
        {
            return Browser.GetCostOrError();
        }

        public static List<string> ValuesOfInputFields(int lotIndex, int amPmIndex)
        {
            //Create a blank list and add the values of each input from the page
            List<string> inputValues = new List<string>();

            inputValues.Add(Browser.GetValueOfLot(lotIndex).ToString());
            inputValues.Add(Browser.GetValueOfEntryPm(amPmIndex).ToString());
            inputValues.Add(Browser.GetEntryTime());
            inputValues.Add(Browser.GetEntryDate());
            inputValues.Add(Browser.GetExitTime());
            inputValues.Add(Browser.GetExitDate());

            return inputValues;
        }
    }
}
