using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gehc_cpq_ui_master.Main.UIElements.Proposal
{
    public class CreateNewProposalPopupElements
    {
        public CreateNewProposalPopupElements(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.XPath, Using = "(//span[@class='slds-radio--faux'])/parent::div//following-sibling::div//span")]
        public IList<IWebElement> selectRecordType { get; set; }
        [FindsBy(How = How.CssSelector, Using = ".slds-button.slds-button_neutral.slds-button.slds-button_brand.uiButton")]
        public IWebElement btnNext { get; set; }
        [FindsBy(How = How.XPath, Using = "//label[text()='Proposal Name']/following-sibling::div//input")]
        public IWebElement inputTxtProposalName { get; set; }
        [FindsBy(How = How.XPath, Using = "//label[text()='Account']/following-sibling::div//input")]
        public IWebElement inputTxtAccount { get; set; }
        [FindsBy(How = How.XPath, Using = "//label[text()='Price List']/following-sibling::div//input")]
        public IList<IWebElement> inputTxtPriceListName { get; set; }
        [FindsBy(How = How.XPath, Using = "//label[text()='Opportunity']/following-sibling::div//input")]
        public IWebElement inputTxtOpportunity { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[text()='Save']")]
        public IWebElement btnSaveEdit { get; set; }
        public string strXpathSearchDropDownList = "//label[text()='{labelName}']/following-sibling::div//ul//li//span/lightning-base-combobox-formatted-text";
        public string txtXPathProposalPopupHeader = "//div[@class='form-legend-desktop']";
        public string txtCssRecordTypepopup = "div > div > div.actionBody > div";

    }
}
