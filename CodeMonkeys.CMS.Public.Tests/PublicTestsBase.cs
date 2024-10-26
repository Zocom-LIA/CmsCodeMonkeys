using CodeMonkeys.CMS.Public.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Testing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using Withywoods.WebTesting;

namespace CodeMonkeys.CMS.Public.Tests
{
    [TestFixture]
    public class PublicTestsBase
    {
        protected HttpClient Client { get; set; }
        protected string DatabaseName { get; } = "InMemoryDatabase";
        protected DbContextOptionsBuilder<ApplicationDbContext> DbContextOptionsBuilder { get; } = new DbContextOptionsBuilder<ApplicationDbContext>();

        protected IWebDriver Driver { get; set; }
        protected string HomeUrl { get; set; }

        protected IWebDriver StartAnyWebDriver()
        {
            //Comment out the AddArgument commands for examining results onscreen.
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");

            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AddArgument("--headless");
            firefoxOptions.AddArgument("--disable-gpu");

            EdgeOptions edgeOptions = new EdgeOptions();
            edgeOptions.AddArgument("--headless");
            edgeOptions.AddArgument("--disable-gpu");
            List<Func<IWebDriver>> funcs = new List<Func<IWebDriver>>()
            {
                () => new ChromeDriver(chromeOptions),
                () => new FirefoxDriver(firefoxOptions),
                () => new SafariDriver(),
                () => new EdgeDriver(edgeOptions),
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

        protected void StartProgramAtSomePort()
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

        protected string FindFreeEmail()
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
            return email;
        }
    }
}