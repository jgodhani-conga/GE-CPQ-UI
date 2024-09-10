using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gehc_cpq_ui_master.Main.UIElements.Proposal
{
    public class ProposalListPageElements
    {
        public ProposalListPageElements(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//a[@title='New']")]
        public IWebElement btnNewProposal { get; set; }
        public string strXPathbtnNewProposal = "//a[@title='New']";
    }
}
