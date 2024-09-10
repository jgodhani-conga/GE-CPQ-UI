using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace gehc_cpq_ui_master.Main.UIElements
{
    public class StartupPageElements
    {
        public StartupPageElements(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }
    }
}
