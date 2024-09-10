using AutomationProject1.Main.utils;
using gehc_cpq_ui_master.Main.UIElements.Proposal;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace gehc_cpq_ui_master.Main.pages.Proposal
{
    public class CreateNewProposalPopup : StartupPage
    {
        private CreateNewProposalPopupElements element;
        public CreateNewProposalPopup(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
            element = new CreateNewProposalPopupElements(driver);
        }
        public CreateNewProposalPopup clickOnMenuItemFromRecordTypeDropDown(String option)
        {
          IList<IWebElement> elements = element.selectRecordType;
            foreach (IWebElement element in elements){
                if (element.Text == option)
                {
                    element.Click();
                    break;
                }
            }
            return this;
        }
        public CreateNewProposalPopup waitTillCreateProposalPopupisLoaded()
        {
            WaitForElementToLoad(element.txtXPathProposalPopupHeader, SelectorType.XPath,minWaitTime);
            return this;
        }
        public CreateNewProposalPopup waitTillSelectRecordTypePopupisLoaded()
        {
            WaitForElementToLoad(element.txtCssRecordTypepopup, SelectorType.CssSelector, minWaitTime);
            return this;
        }
        public CreateNewProposalPopup clickOnNextButton()
        {
            element.btnNext.Click();
            return this;
        }
        public CreateNewProposalPopup enterProposalName(String proposalName)
        {
            element.inputTxtProposalName.SendKeys(proposalName);
            return this;
        }
        public CreateNewProposalPopup selectPriceList(String priceListName)
        {
            selectLookupItemFromDropDown(element.inputTxtPriceListName[0], element.strXpathSearchDropDownList.Replace("{labelName}", "Price List"), priceListName);
            return this;
        }
        public CreateNewProposalPopup selectOpportunity(String opportunityName)
        {
            selectLookupItemFromDropDown(element.inputTxtOpportunity, element.strXpathSearchDropDownList.Replace("{labelName}","Opportunity"), opportunityName);

            return this;
        }

        public CreateNewProposalPopup clickOnSaveButton()
        {
            element.btnSaveEdit.Click();
            return this;
        }
        public CreateNewProposalPopup selectAccount(string account)
        {
            selectLookupItemFromDropDown(element.inputTxtAccount,element.strXpathSearchDropDownList.Replace("{labelName}", "Account"), account);
            return this;
        }
        private void selectLookupItemFromDropDown(IWebElement inputBox, string searchListElementsString, string searchText)
        {
            inputBox.SendKeys(searchText);
            inputBox.Click();
            WaitForElementToLoad(searchListElementsString, SelectorType.XPath, minWaitTime);
            IList<IWebElement> searchListElements = driver.FindElements(By.XPath(searchListElementsString));
            foreach (IWebElement item in searchListElements)
            {
                if (item.GetAttribute("Title").ToString().Equals(searchText))
                {
                    item.Click();
                    break;
                }
            }
        }
    }
}
