using AutomationProject1.Main.pages;
using AutomationProject1.Main.resources;
using AutomationProject1.Main.utils;
using gehc_cpq_ui_master.Main.UIElements.Proposal;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace gehc_cpq_ui_master.Main.pages.Proposal
{
    public class ProposalListPage : StartupPage
    {
        private ProposalListPageElements proposalListEle;
        public ProposalListPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
            proposalListEle = new ProposalListPageElements(driver);
        }
        public ProposalListPage returnToProposalListPage(String baseURL)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(baseURL);
            return this;
        }
        public void waitForProposalListPageToLoad()
        {
            WaitForElementToLoad(proposalListEle.strXPathbtnNewProposal,SelectorType.XPath, minWaitTime);
        }
        public ProposalListPage clickOnNewButton()
        {
            waitForProposalListPageToLoad();
            proposalListEle.btnNewProposal.Click();
            return this;
        }
        protected ProposalListPage deleteQuote(string quoteId)
        {
            returnToProposalListPage(Config.baseURL);
            return this;
        }
    }
}
