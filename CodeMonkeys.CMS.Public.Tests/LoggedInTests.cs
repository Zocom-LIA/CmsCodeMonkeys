using CodeMonkeys.CMS.Public.Components.Account.Pages.Manage;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Tests
{
    internal class LoggedInTests:PublicTestsBase
    {
        // This fixture structure is for tests that start in a logged-in state and stay in that state.

        string userName;

        [OneTimeSetUp]
        public void Setup()
        {
            StartProgramAtSomePort();
            DbContextOptionsBuilder.UseInMemoryDatabase(DatabaseName);
            Driver = StartAnyWebDriver();
            Driver.Manage().Window.Size = new System.Drawing.Size(1824, 900);
            userName = FindFreeEmail();
            string password = "Password1!";
            SetUpUser(userName, password);
            Driver.Navigate().GoToUrl(HomeUrl);
            Thread.Sleep(10);
            Driver.FindElement(By.LinkText("Login")).Click();
            Thread.Sleep(70);
            Driver.FindElement(By.Name("Input.Email")).SendKeys(userName);
            Driver.FindElement(By.Name("Input.Password")).SendKeys(password);
            Driver.FindElement(By.XPath("//form//button")).Click();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Client?.Dispose();
            Driver?.Dispose();
        }

        [Test]
        public void LoggedInLandingTest()
        {
            Driver.Navigate().GoToUrl(HomeUrl);
            Assert.That(Driver.FindElement(By.XPath("//main//*[@class='left-top']//h1")).Text, Is.EqualTo("Logged in"));
        }

    }
}
