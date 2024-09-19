using CodeMonkeys.CMS.Public;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using Withywoods.WebTesting;

namespace CodeMonkeys.CMS.Public.Tests
{
    public class Tests
    {

        IWebDriver Driver { get; set; }
        HttpClient Client { get; set; }
        string HomeUrl { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            StartProgramAtSomePort();
            Driver = StartAnyWebDriver();
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
                    Client = Waff.WithWebHostBuilder(builder => builder.UseSetting("database", "inMemory")).CreateClient();
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
            Assert.That(Driver.FindElement(By.XPath("//main//h1")).Text, Is.EqualTo("PUBLIC"));
        }
    }
}