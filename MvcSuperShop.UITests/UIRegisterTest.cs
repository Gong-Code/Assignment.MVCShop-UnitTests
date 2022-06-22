using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace MvcSuperShop.UITests
{
    [TestClass]
    public class UIRegisterTest
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
        public void When_register_is_completed_should_return_valid_register_Url()
        {
            //ARRANGE
            _driver.Navigate().GoToUrl("https://localhost:7122/");

            Wait(3);

            var allButtons = _driver.FindElements(By.LinkText("Register"));
            
            var btn = allButtons.First();
            
            btn.Click();
            
            Wait(3);

            //ACT           
            var password = "Hejsan123!";

            var emailInput = _driver.FindElement(By.Id("Input_Email"));
            var randomEmail = GenerateRandomEmail();
            emailInput.SendKeys(randomEmail);

            var passwordInput = _driver.FindElement(By.Id("Input_Password"));
            passwordInput.SendKeys(password);

            var confirmPassword = _driver.FindElement(By.Id("Input_ConfirmPassword"));
            confirmPassword.SendKeys(password);

            Wait(3);
    
            var submit = _driver.FindElement(By.Id("registerSubmit"));
            
            submit.Click();

            Wait(3);

            //ASSERT

            Assert.AreEqual($"https://localhost:7122/Identity/Account/RegisterConfirmation?email={randomEmail}&returnUrl=%2F", _driver.Url);
        }


        private void Wait(int secs = 1)
        {
            System.Threading.Thread.Sleep(secs * 1000);
        }

        public string GenerateRandomEmail()
        {
            Random randomGenerator = new Random();
            int randomInt = (int)randomGenerator.NextInt64(maxValue: 1000);
            var randomEmail = "username" + randomInt + "@gmail.com";

            return randomEmail;
        }
    }
}
