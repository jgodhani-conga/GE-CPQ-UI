using AutomationProject1.Main.resources;
using gehc_cpq_ui_master.Main.utils;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace AutomationProject1.Main.utils
{
    public class StartupPage:Config
    {
        public IWebDriver driver;
        public static int unitTime = 100;
        public static int pollingInterval = 300;
        public static int sleepWait = 300;
        public static int minWaitTime = 15;
        public static int maxWaitTime = 60;
        public static int maxAvgWaitTime = 120;
        public static int highWaitTime = 180000;
        public DefaultWait<IWebDriver> fluentWait;
        public enum SelectorType { ClassName, CssSelector, Id, Name, TagName, XPath };
        public StartupPage(IWebDriver driver)
        {
            this.driver = driver;
            fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(minWaitTime);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(pollingInterval);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.Message = "Element to be searched not found";
        }
        public string GenerateRandom()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
        
        protected void WaitForElementToLoad(IWebElement element, int waitTime)
        {
            if (minWaitTime > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
                wait.Until(ExpectedConditions.ElementToBeClickable(element));
            }
        }
        public void SwitchToIFrame()
        {
            WaitForElementToLoad("//iframe[contains(@title, 'Test Proposal')]", SelectorType.XPath, maxWaitTime);
            IWebElement iframe = driver.FindElement(By.XPath("//iframe[contains(@title, 'Test Proposal')]"));
            driver.SwitchTo().Frame(iframe);
        }

        public void SwitchToDefaultContent()
        {
            driver.SwitchTo().DefaultContent();
        }
        public void WaitUntilElementIsInvisible(string element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(maxWaitTime));
            By elementXPath = By.CssSelector(element);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(elementXPath));
        }

        public void WaitUntilElementIsInvisibleXpath(string element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(maxWaitTime));
            By elementXPath = By.XPath(element);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(elementXPath));
        }
        public void WaitUntillElementToBeClickble(string selector, int timeInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSeconds));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(selector)));

        }
        public void WaitUntillElementToBeClickbleForXpath(string selector, int timeInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSeconds));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(selector)));

        }
        
        public void FluentWaitUntilElementVisibleForCss(string selector, int timeInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(selector)));
        }
        public void FluentWaitUntilElementVisibleForXpath(string selector, int timeInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(selector)));
        }

        public void WaitForNetworkResponses(IWebDriver driver, int timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            wait.Until(drv =>
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)drv;
                var networkStatus = js.ExecuteScript("return window.performance.getEntriesByType('resource').length == 0;");
                return (bool)networkStatus;
            });
        }
        protected void WaitForElementToLoad(string Selector, SelectorType selectorType,int waitTime)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
            By? by = null;
            switch (selectorType)
            {
                case SelectorType.CssSelector:
                    by = By.CssSelector(Selector);
                    break;
                case SelectorType.XPath:
                    by = By.XPath(Selector);
                    break;
                case SelectorType.Id:
                    by = By.Id(Selector);
                    break;
                case SelectorType.Name:
                    by = By.Name(Selector);
                    break;
                case SelectorType.ClassName:
                    by = By.ClassName(Selector);
                    break;
                case SelectorType.TagName:
                    by = By.TagName(Selector);
                    break;
            }

            wait.Until(driver =>
            {
                bool isPageLoaded = ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");
                bool isElementVisible = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime)).Until(ExpectedConditions.ElementIsVisible(by)) != null;
                return isPageLoaded && isElementVisible;
            });
        }

        public void checkFiedValue(string configJson, string ProductName, string actualValue, string FieldName)
        {
            if (!string.IsNullOrEmpty(configJson))
            {
                var parsedJson = JObject.Parse(configJson);

                var LineItems = parsedJson["CartResponse"]["LineItems"];
                Console.WriteLine(LineItems);

                var item = LineItems
               .OfType<JObject>()
               .FirstOrDefault(x => x["Name"]?.ToString() == ProductName);
                Console.WriteLine("item="+item);
                Assert.That(actualValue.ToString().Equals(item[FieldName].ToString()), "Quantity1 is not matching");
            }
            else
            {
                Assert.Fail("configJson was not received.");
            }
        }
    }
}
