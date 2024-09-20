using CodeMonkeys.CMS.Public;
using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
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
    public class Tests
    {

        IWebDriver Driver { get; set; }
        HttpClient Client { get; set; }
        string HomeUrl { get; set; }
        string DatabaseName { get; } = "InMemoryDatabase";
        DbContextOptionsBuilder<ApplicationDbContext> DbContextOptionsBuilder { get; } = new DbContextOptionsBuilder<ApplicationDbContext>();

        [OneTimeSetUp]
        public void Setup()
        {
            StartProgramAtSomePort();
            Driver = StartAnyWebDriver();
            DbContextOptionsBuilder.UseInMemoryDatabase(DatabaseName);
            Driver.Manage().Window.Size = new System.Drawing.Size(1824, 900);
        }

        void StartProgramAtSomePort()
        {
            WebApplicationFactoryFixture<Program> Waff = new WebApplicationFactoryFixture<Program>();
            int port = 5000;
            while (port < 6000) // Have to stop somewhere, but the main exit from the loop takes the form of a return.
            {
                Waff.HostUrl = $"https://localhost:{port}";
                try
                {
                    Client = Waff.WithWebHostBuilder(builder => builder.UseSetting("database", "inMemory").UseSetting("database_name", DatabaseName)).CreateClient();
                    HomeUrl = Waff.HostUrl;
                    return;
                }
                catch (IOException ex)
                {
                    // Can the wording that contains "Failed to bind" be trusted to be stable?
                    if (!ex.Message.Contains("Failed to bind"))
                        throw new Exception("Creation of host failed due to something other than the chosen port being occupied", ex);
                }
                port++;
            }
            throw new Exception("Creation of host failed 1000 times");
        }

        IWebDriver StartAnyWebDriver()
        {
            List<Func<IWebDriver>> funcs = new List<Func<IWebDriver>>()
            {
                () => new ChromeDriver(),
                () => new FirefoxDriver(),
                () => new SafariDriver(),
                () => new EdgeDriver(),
                () => new InternetExplorerDriver()
            };
            List<Exception> exceptions = new List<Exception>();
            foreach (Func<IWebDriver> func in funcs)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            throw new Exception("Creation of web driver failed");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Driver?.Dispose();
            Client?.Dispose();
        }


        [Test]
        public void EntryPageTest()
        {
            Driver.Navigate().GoToUrl(HomeUrl);

            Assert.That(Driver.FindElement(By.XPath("//header//h1")).Text, Is.EqualTo("CODE MONKEYS"));
            Assert.That(Driver.FindElement(By.XPath("//main//h1")).Text, Is.EqualTo("Public"));
        }

        [Test]
        public void RegisterTest()
        {
            int emailNumber = 0;
            string email;
            bool done = false;
            using (ApplicationDbContext dbContext = new ApplicationDbContext(DbContextOptionsBuilder.Options, new FakeLogger<ApplicationDbContext>()))
                do
                {
                    email = $"name{emailNumber}@example.com";
                    emailNumber++;
                    if (dbContext.Users.FirstOrDefault(user => user.Email == email) == null)
                        done = true;
                } while (!done);
            Driver.Navigate().GoToUrl(HomeUrl);
            Thread.Sleep(10); // The wait command below only rarely achieves a state where the click succeeds.
            //new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Register")));
            Driver.FindElement(By.LinkText("Register")).Click();
            Thread.Sleep(10);
            Driver.FindElement(By.Name("Input.Email")).SendKeys(email);
            Driver.FindElement(By.Name("Input.Password")).SendKeys("Password1!");
            Driver.FindElement(By.Name("Input.ConfirmPassword")).SendKeys("Password1!");
            Driver.FindElement(By.XPath("//form/button")).Click();
            Thread.Sleep(10);
            Driver.FindElement(By.LinkText("Click here to confirm your account")).Click();
            Thread.Sleep(10);
            using (ApplicationDbContext dbContext = new ApplicationDbContext(DbContextOptionsBuilder.Options, new FakeLogger<ApplicationDbContext>()))
                Assert.That(dbContext.Users.FirstOrDefault(user => user.Email == email), Is.InstanceOf(typeof(User)));
        }
    }
}