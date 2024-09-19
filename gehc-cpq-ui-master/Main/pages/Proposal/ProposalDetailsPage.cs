using AutomationProject1.Main.utils;
using gehc_cpq_ui_master.Main.UIElements.Proposal;
using gehc_cpq_ui_master.Main.utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gehc_cpq_ui_master.Main.pages.Proposal
{
    public class ProposalDetailsPage : StartupPage
    {
        private string ProposalIdField = "Proposal ID";
        private string ProposalNameField = "Proposal Name";
        private string PriceListField = "Proposal Name";
        private string OpportunityField = "Opportunity";
        private string ConfigureProductRLSField = "Configure Products (Platform)";

        private enum FieldNames {}; 
        private ProposalDetailsPageElements elements;
        public ProposalDetailsPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
            elements = new ProposalDetailsPageElements(driver);
        }
        public ProposalDetailsPage waitTillProposalDetailsPageisLoaded()
        {
            WaitForElementToLoad("//records-entity-label[normalize-space(text())='Quote/Proposal']", SelectorType.XPath, maxWaitTime);
            return this;
        }
        public ProposalDetailsPage waitForPopupToLoad()
        {
            WaitForElementToLoad(elements.popupHeader, SelectorType.XPath, maxWaitTime);
            return this;
        }
        public ProposalDetailsPage deleteCurrentProposal()
        {
            waitTillProposalDetailsPageisLoaded();
            elements.btnDeleteProposal.Click();
            waitForPopupToLoad();
            elements.btnConfirmDelete.Click();
            return this;
        }
        public string getProposalObjectId()
        {
            waitTillProposalDetailsPageisLoaded();
            string[] urlParts= driver.Url.Split('/');
            return urlParts[urlParts.Length - 2];
        }
        public string getProposalName()
        {
            return getTextFieldValue(ProposalNameField);
        }
        public string getProposalId()
        {
            return getTextFieldValue(ProposalIdField);
        }
        public void clickOnConfigureProductsForRLS()
        {
            WaitForElementToLoad("//*[@id='detailTab__item']", SelectorType.XPath, maxWaitTime);
            driver.FindElement(By.XPath("//*[@id='detailTab__item']")).Click();
            WaitUntillElementToBeClickble(elements.txtXPathBtn.Replace("{fieldName}", ConfigureProductRLSField),SelectorType.XPath, maxWaitTime);
            IWebElement element = driver.FindElement(By.XPath(elements.txtXPathBtn.Replace("{fieldName}", ConfigureProductRLSField)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }
        public string getTextFieldValue(string fieldName)
        {
            string textFieldPath = elements.txtXPathProposal.Replace("{fieldName}", fieldName) + elements.txtXPathFieldValue.Replace("{fieldName}", fieldName);
            WaitForElementToLoad(textFieldPath, SelectorType.XPath, minWaitTime);
            return driver.FindElement(By.XPath(textFieldPath)).Text;
        }

        public void clickOnRelatedTab()
        {
            WaitForElementToLoad("//a[text()='Related']", SelectorType.XPath, maxWaitTime);
            driver.FindElement(By.XPath("//a[text()='Related']")).Click();
           
        }
        public void waitForProposalDetailPageToLoad()
        {
            fluentWaitUntilElementIsVisible("//span[@title='Configurations']", SelectorType.XPath,maxWaitTime);
            WaitUntillElementToBeClickble("//span[@title='Configurations']", SelectorType.XPath, maxWaitTime);


        }
        public void clickOnConfigureIcon()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@title='Configurations']")));
                element.Click();
            }
            
            catch (StaleElementReferenceException)
{
                var element = driver.FindElement(By.XPath("//span[@title='Configurations']"));
                element.Click();
            }


        }
        public string CheckFinalize()
        {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='brandBand_2']/div/div/div[2]/div/lst-related-list-desktop/article/lst-related-list-view-manager/lst-common-list-internal/div/div/lst-primary-display-manager/div/lst-primary-display/lst-primary-display-grid/lightning-datatable/div[2]/div/div/table/tbody/tr/td[5]/lightning-primitive-cell-factory/span/div/lightning-primitive-custom-cell/lst-formatted-text/span")));

            return element.Text.ToString();       
        }
        public void switchToNewWindow(string originalWindowHandle)
        {

            foreach (string windowHandle in driver.WindowHandles)
            {
                if (windowHandle != originalWindowHandle)
                {
                    driver.SwitchTo().Window(windowHandle);
                    break;
                }
            }
           
        }

        public void waitUntilClickConfigure()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30)); // Adjust the timeout as needed
                                                                                      // FluentWaitUntilElementVisibleForXpath(elements.ConfigurationOne, maxWaitTime);

            var totalValueText = "";
            try
            {
                wait.Until(driver =>
                {
                    try
                    {
                        // Find the element and get its text
                        var miniCartTotal = driver.FindElement(By.XPath(elements.ConfigurationOne));
                        totalValueText = miniCartTotal.Text.Trim();
                        int number = int.Parse(totalValueText.Trim('(', ')'));
                        return number > 0;
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("Element not found. Waiting for it to appear...");
                        return false; // Continue waiting
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Failed to parse the number from text: " + totalValueText);
                        return false; // Continue waiting
                    }
                });

                clickOnConfigureIcon();
            }
            catch (WebDriverTimeoutException e)
            {
                Console.WriteLine("Timed out waiting for the condition. Error: " + e.Message);
            }
        }
    }
}
