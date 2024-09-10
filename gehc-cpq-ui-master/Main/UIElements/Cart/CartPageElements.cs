using AventStack.ExtentReports.Gherkin.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpq_ui_master.Main.UIElements.Cart
{
    public class CartPageElements
    {
        public CartPageElements(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//dynamic-field[@field-type='QUANTITY']//input[@type='cart line item']")]
        public IList<IWebElement> Quantity { get; set; }

        public string QuantityColumnField = "//span[@title='Quantity']";

        public string Repricebutton = "//button[normalize-space()='Reprice']";


        [FindsBy(How = How.XPath, Using = "//button[normalize-space()='Reprice']")]
        public IWebElement RepriceBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class, 'aptAdjustmentType')]//md-option")]
        public IList<IWebElement> AdjustmentTypeListValue{ get; set; }

        public String AdjustmentTypeListPath = "//div[contains(@class, 'aptAdjustmentType')]//md-option[@value='Discount Amount']";

        [FindsBy(How = How.XPath, Using = "//div[contains(@class, 'aptAdjustmentType')]//md-select")]
        public IList<IWebElement> AdjustmnetTypeDropDown{ get; set; }

        public string ProductNamePath = "span.ng-binding.ng-scope.ng-isolate-scope";

        public string quantityPath = "(//dynamic-field[@field-type='QUANTITY']//input[@type='cart line item'])[{productIndex}]";

        public string adjustmentTypeFieldPath = "(//div[contains(@class, 'aptAdjustmentType')]//md-select)[{productIndex}]";


        public string adjustmentAmountForGE = "(//button[@class='md-icon-button grid-cart-adjustment-dropdown md-button md-ink-ripple' and @aria-label='Adjustments'])[{productIndex}]";

        public string adjustmentAmountFieldPath = "(//div[contains(@class, 'adjustmentamount-input')]//input)[{productIndex}]";


        public string ClickOnCheckBox = "(//div[@ng-if='!row.entity.isSummary'])[{ProductIndex}]";

        [FindsBy(How = How.XPath, Using = "(//div[@ng-if='!row.entity.isSummary'])[{ProductIndex}]")]
        public IWebElement ClickCheckBoxBtn { get; set; }

        public string ClickOnMassUpdate = "//button[@aria-label='Mass Update']//i[@class='fa cart-actions fa-pencil-square-o']";

        [FindsBy(How = How.XPath, Using = "//button[@aria-label='Mass Update']//i[@class='fa cart-actions fa-pencil-square-o']")]
        public IWebElement ClickOnMassUpdateBtn { get; set; }

        public string ClickOnApply = "//button[contains(@class, 'ands-btn') and @ng-click='massLineUpdate.applyMassUpdate()']";

        [FindsBy(How = How.XPath, Using = "//button[contains(@class, 'ands-btn') and @ng-click='massLineUpdate.applyMassUpdate()']")]
        public IWebElement ClickOnApplyBtn { get; set; }

        public string adjustmentTypePicklist = "(//div[contains(@class, 'adjustment-menu-content') and contains(@class, 'fieldtype-wrapper--PICKLIST')])[{productLength}]";

        public string productPath = "//span[@title='{productName}']";
        public string productNameXpath = "//span[text()='{productName}']";
    }
}
