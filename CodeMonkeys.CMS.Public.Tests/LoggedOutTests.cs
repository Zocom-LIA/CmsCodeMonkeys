using CodeMonkeys.CMS.Public;
using CodeMonkeys.CMS.Public.Components.Account.Pages.Manage;
using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Testing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V126.Input;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Withywoods.WebTesting;

namespace CodeMonkeys.CMS.Public.Tests
{
    public class LoggedOutTests : PublicTestsBase
    {
        // This fixture structure is for tests that start in a logged-out state and stay in that state.
        [OneTimeSetUp]
        public void Setup()
        {
            StartProgramAtSomePort();
            DbContextOptionsBuilder.UseInMemoryDatabase(DatabaseName);
            Driver = StartAnyWebDriver();
            Driver.Manage().Window.Size = new System.Drawing.Size(1824, 900);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Client?.Dispose();
            Driver?.Dispose();
        }


        [Test]
        public void EntryPageTest()
        {
            Driver.Navigate().GoToUrl(HomeUrl);
            Thread.Sleep(100);
            Assert.That(Driver.FindElement(By.XPath("//header//a")).Text, Is.EqualTo("CODE MONKEYS"));
            Assert.That(Driver.FindElement(By.XPath("//main//h1")).Text, Is.EqualTo("PUBLIC"));
        }

        [Test]
        public void RegisterTest()
        {
            string email = FindFreeEmail();
            string password = "Password1!";
            Driver.Navigate().GoToUrl(HomeUrl);
            Thread.Sleep(100); // The wait command below only rarely achieves a state where the click succeeds.
            //new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Register")));
            Driver.FindElement(By.LinkText("Register")).Click();
            Thread.Sleep(100); // Empirically determined value. Presumably system-specific. Presumably, a better way exists.
            Driver.FindElement(By.Name("Input.Email")).SendKeys(email);
            Driver.FindElement(By.Name("Input.Password")).SendKeys(password);
            Driver.FindElement(By.Name("Input.ConfirmPassword")).SendKeys(password);
            Driver.FindElement(By.XPath("//form/button")).Click();
            Thread.Sleep(100);
            Driver.FindElement(By.LinkText("Click here to confirm your account")).Click();
            Thread.Sleep(100);
            using (ApplicationDbContext dbContext = new ApplicationDbContext(DbContextOptionsBuilder.Options, new FakeLogger<ApplicationDbContext>()))
                Assert.That(dbContext.Users.FirstOrDefault(user => user.Email == email), Is.InstanceOf(typeof(User)));
        }
    }
}