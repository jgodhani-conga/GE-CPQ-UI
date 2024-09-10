using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gehc_cpq_ui_master.Main.UIElements.Proposal
{
    public class ProposalDetailsPageElements
    {
        public ProposalDetailsPageElements(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.XPath, Using = "//span[text()='Configure Products (Platform)']/ancestor::dt/following-sibling::dd//img")]
        public IWebElement btnConfigureProductRLS { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[text()='Delete']")]
        public IWebElement btnDeleteProposal { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[text()='Delete' and @class='slds-button slds-button_brand']")]
        public IWebElement btnConfirmDelete { get; set; }
        public string popupHeader = "//button[text()='Delete' and @class='slds-button slds-button_brand']";

        public string txtXPathProposal = "//records-record-layout-section//records-record-layout-row//records-record-layout-item[@field-label='{fieldName}']";
        public string txtXPathFieldValue =  "//span/ancestor::dt/following-sibling::dd//span//lightning-formatted-text";
        public string txtXPathBtn = "//span[text()='{fieldName}']/ancestor::dt/following-sibling::dd//img";
        public string ProposalName = "//p[@title='Proposal Name']";

    }
}
