using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Testing;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Tests
{
    internal class LoginTest:PublicTestsBase
    {
        // This fixture structure closes the web browser and opens it again between each test, to ensure that each test starts in a logged-out state regardless of what previous tests have been doing.
        [OneTimeSetUp]
        public void SuiteSetup()
        {
            StartProgramAtSomePort();
            DbContextOptionsBuilder.UseInMemoryDatabase(DatabaseName);
            
        }

        [SetUp]
        public void Setup()
        {
            Driver = StartAnyWebDriver();
            Driver.Manage().Window.Size = new System.Drawing.Size(1824, 900);
        }
        [TearDown]
        public void Teardown()
        {
            Driver.Dispose();
        }

        [OneTimeTearDown]
        public void SuiteTearDown()
        {
            Client?.Dispose();
            Driver?.Dispose();
        }

        [Test]
        public void LoginUserTest()
        {
            string email = FindFreeEmail();
            string password = "Password1!";
            using (ApplicationDbContext dbContext = new ApplicationDbContext(DbContextOptionsBuilder.Options, new FakeLogger<ApplicationDbContext>()))
            {
                User user = new User()
                {
                    Email = email,
                    UserName =email,
                    NormalizedEmail = email.ToUpper(),
                    NormalizedUserName = email.ToUpper(),
                    SecurityStamp = "",
                    EmailConfirmed = true
                };
                string hash = new PasswordHasher<User>().HashPassword(user, password);
                user.PasswordHash = hash;
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
            Driver.Navigate().GoToUrl(HomeUrl);
            Thread.Sleep(200);
            Driver.FindElement(By.LinkText("Login")).Click();
            Thread.Sleep(100);
            Driver.FindElement(By.Name("Input.Email")).SendKeys(email);
            Driver.FindElement(By.Name("Input.Password")).SendKeys(password);
            Driver.FindElement(By.XPath("//form//button")).Click();
            Thread.Sleep(200);
            Assert.That(Driver.FindElement(By.XPath("//main//*[@class='left-top']//h1")).Text, Is.EqualTo("Logged in"));

        }

    }
}
