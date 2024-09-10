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
            WaitForElementToLoad(elements.popupHeader, SelectorType.XPath, minWaitTime);
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
            WaitUntillElementToBeClickbleForXpath(elements.txtXPathBtn.Replace("{fieldName}", ConfigureProductRLSField), maxWaitTime);
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
            FluentWaitUntilElementVisibleForXpath("//span[@title='Configurations']", maxWaitTime);
            WaitUntillElementToBeClickbleForXpath("//span[@title='Configurations']", maxWaitTime);


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
    }
}
