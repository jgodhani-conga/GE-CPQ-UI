using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
namespace AutomationProject1.Main.utils
{
    public class WebDriverUtils
    {
        public static IWebDriver driver;

        public void initializeDriver(String driverName)
        {
            switch (driverName.ToUpper())
            {
                case "CHROME":
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("no-sandbox");

                    driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
                    driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(StartupPage.maxAvgWaitTime));
                break;
            }
        }
        public IWebDriver GetDriver()
        {
            return driver;
        }
    }
}
