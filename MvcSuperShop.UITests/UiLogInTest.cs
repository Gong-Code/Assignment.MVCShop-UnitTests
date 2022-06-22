using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSuperShop.UITests
{
    [TestClass]
    public class UiLogInTest
    {
        private static IWebDriver _driver;

        [ClassInitialize]
        public static void Initialize(TestContext aContext)
        {
            _driver = new ChromeDriver();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _driver.Close();
            _driver.Dispose();
        }

        [TestMethod]
        public void When_LogIn_return_IndexPage()
        {
            //ARRANGE
            _driver.Navigate().GoToUrl("https://localhost:7122/");

            Wait(3);

            var allButtons = _driver.FindElements(By.LinkText("Login"));

            var btn = allButtons.First();

            btn.Click();

            Wait(3);

            //ACT
            var emailInput = _driver.FindElement(By.Id("Input_Email"));
            emailInput.SendKeys("stefan.holmberg@systementor.se");

            var passwordInput = _driver.FindElement(By.Id("Input_Password"));
            passwordInput.SendKeys("Hejsan123#");

            var submit = _driver.FindElement(By.Id("login-submit"));

            submit.Click();

            Wait(3);

            //ASSERT
            Assert.AreEqual("https://localhost:7122/", _driver.Url);

        }

        private void Wait(int secs = 1)
        {
            System.Threading.Thread.Sleep(secs * 1000);
        }
    }
}
