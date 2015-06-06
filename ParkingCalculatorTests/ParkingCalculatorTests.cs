using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingCalculatorTestFramework;

namespace ParkingCalculatorTests
{
    [TestClass]
    public class ParkingCalculatorTests
    {
        [TestInitialize]
        public void SetupTests()
        {
            HomePage.GoTo();
        }

        [TestMethod]
        public void ShortTermparkingOneHourTest()
        {
            Browser.SelectLotByName("Short-Term Parking");
            Browser.EnterEntryTime("10:00");
            Browser.EnterEntryDate("01/01/2014");
            Browser.SelectAmOrPm("Entry");
            Browser.EnterExitTime("11:00");
            Browser.EnterExitDate("01/01/2014");
            Browser.SelectAmOrPm("Exit");
            HomePage.Submit();

            Assert.AreEqual("$ 2.00", HomePage.Cost());
            Assert.AreEqual("(0 Days, 1 Hours, 0 Minutes)", HomePage.Duration());

        }

        [TestMethod]
        public void LongTermSurfaceparkingOneMonthTest()
        {
            Browser.SelectLotByName("Long-Term Surface Parking");
            Browser.ClickCalendarIcon(0);
            Browser.SelectDateInCalendar("January", "1");
            Browser.ClickCalendarIcon(1);
            Browser.SelectDateInCalendar("February", "1");
            HomePage.Submit();

            Assert.AreEqual("$ 270.00", HomePage.Cost());
            Assert.AreEqual("(31 Days, 0 Hours, 0 Minutes)", HomePage.Duration());
        }

        [TestMethod]
        public void ShortTermParkingEntryDateAfterExitDateTest()
        {
            Browser.SelectLotByName("Short-Term Parking");
            Browser.EnterEntryDate("01/02/2014");
            Browser.EnterExitDate("01/01/2014");
            HomePage.Submit();

            Assert.AreEqual("ERROR! Your Exit Date Or Time Is Before Your Entry Date or Time".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void SubmitOnPageLoadTest()
        {
            HomePage.Submit();

            Assert.AreEqual("ERROR! Enter A Correctly Formatted Date".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void ShortTermParkingEntryTimeAfterExitTimeTest()
        {
            Browser.SelectLotByName("Short-Term Parking");
            Browser.EnterEntryTime("11:00");
            Browser.EnterEntryDate("01/01/2014");
            Browser.EnterExitTime("8:00");
            Browser.EnterExitDate("01/01/2014");
            HomePage.Submit();

            Assert.AreEqual("ERROR! Your Exit Date Or Time Is Before Your Entry Date or Time".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void InvalidTimeWith00FollowingTest()
        {
            Browser.SelectLotByName("Short-Term Parking");
            Browser.EnterEntryTime("159357:00");
            Browser.EnterEntryDate("01/01/2014");
            Browser.EnterExitDate("01/01/2014");
            HomePage.Submit();

            Assert.AreEqual("ERROR! Your Entry or Exit Time Is Invalid".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void InvalidTimeWithout00FollowingTest()
        {
            Browser.SelectLotByName("Short-Term Parking");
            Browser.EnterEntryTime("159357");
            Browser.EnterEntryDate("01/01/2014");
            Browser.EnterExitDate("01/01/2014");
            HomePage.Submit();

            Assert.AreEqual("ERROR! Your Entry or Exit Time Is Invalid".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void FieldsMatchAfterSubmitionTest()
        {
            Browser.SelectLotByName("Short-Term Parking");
            Browser.SelectAmOrPm("Entry");
            Browser.EnterEntryTime("10:00");
            Browser.EnterEntryDate("01/01/2014");
            Browser.EnterExitTime("11:00");
            Browser.EnterExitDate("01/02/2014");
            var beforeSubmit = HomePage.ValuesOfInputFields(2,1);
            HomePage.Submit();
            var aftersubmit = HomePage.ValuesOfInputFields(2, 1);

            for (int indexValue = 0; indexValue < 6; indexValue++)
            {
                Assert.AreEqual(beforeSubmit[indexValue], aftersubmit[indexValue]);
            }
        }

        [TestMethod]
        public void EmptyEntryAndExitDateTest()
        {
            Browser.ClearDate("Entry");
            Browser.ClearDate("Exit");
            HomePage.Submit();

            Assert.AreEqual("ERROR! Enter A Correctly Formatted Date".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void EmptyEntryDateTest()
        {
            Browser.ClearDate("Entry");
            Browser.EnterExitDate("01/01/2014");
            HomePage.Submit();

            Assert.AreEqual("ERROR! Enter A Correctly Formatted Date".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void EmptyExitDateTest()
        {
            Browser.EnterEntryDate("01/01/2014");
            Browser.ClearDate("Exit");
            HomePage.Submit();

            Assert.AreEqual("ERROR! Enter A Correctly Formatted Date".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void EmptyEntryAndExitTimeTest()
        {
            Browser.ClearTime("Entry");
            Browser.ClearTime("Exit");
            Browser.EnterEntryDate("01/01/2014");
            Browser.EnterExitDate("01/01/2014");
            HomePage.Submit();

            Assert.AreEqual("ERROR! Enter A Correctly Formatted Time".ToUpper(), HomePage.ErrorMessage());
        }

        [TestMethod]
        public void ShortTermParkingEntryAndExitSameTimeTest()
        {
            Browser.SelectLotByName("Short-Term Parking");
            Browser.EnterEntryTime("10:00");
            Browser.EnterExitTime("10:00");
            Browser.EnterEntryDate("01/01/2015");
            Browser.EnterExitDate("01/01/2015");
            HomePage.Submit();

            Assert.AreEqual("$ 0.00", HomePage.Cost());
            Assert.AreEqual("(0 Days, 0 Hours, 0 Minutes)", HomePage.Duration());
        }

        [TestMethod]
        public void EconomyParkingEntryAndExitTimeSameTest()
        {
            Browser.SelectLotByName("Economy Parking");
            Browser.EnterEntryTime("10:00");
            Browser.EnterExitTime("10:00");
            Browser.EnterEntryDate("01/01/2015");
            Browser.EnterExitDate("01/01/2015");
            HomePage.Submit();

            Assert.AreEqual("$ 0.00", HomePage.Cost());
            Assert.AreEqual("(0 Days, 0 Hours, 0 Minutes)", HomePage.Duration());
        }

        [TestCleanup]
        public void CleanUpTests()
        {
            Browser.Close();
        }
    }
}
