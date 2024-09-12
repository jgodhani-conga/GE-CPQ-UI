using AutomationProject1.Main.utils;
using cpq_ui_master.Main.UIElements.Catalog;
using cpq_ui_master.Main.UIElements.Favorite;
using gehc_cpq_ui_master.Main.pages.Proposal;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace cpq_ui_master.Main.pages.Favorite
{
    public class FavoritePage : StartupPage
    {
        private FavoritePageElement favElement;

        public FavoritePage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
            favElement = new FavoritePageElement(driver);
        }

        public void clickOnFavorite()
        {
            WaitForElementToLoad(favElement.clickOnFavorite, SelectorType.XPath, maxWaitTime);
            favElement.clickOnFavoriteBtn.Click();
        }

        public FavoritePage enterFavoriteName(String favoriteName)
        {
            favElement.inputTxtFavName.SendKeys(favoriteName);
            return this;
        }

        public void clickOnSaveBtn()
        {
            WaitForElementToLoad(favElement.clickOnSave, SelectorType.XPath, maxWaitTime);
            favElement.clickOnSaveBtn.Click();
        }

        public void addProductFromFavorite(string favoriteName)
        {
            WaitForElementToLoad("//a[text()='Favorites']", SelectorType.XPath, maxWaitTime);
            IWebElement favoriteGroup = driver.FindElement(By.XPath("//a[text()='Favorites']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", favoriteGroup);

            WaitForElementToLoad("input.catalog-search-input", SelectorType.CssSelector, maxWaitTime);
            IWebElement searchFavorite = driver.FindElement(By.CssSelector("input.catalog-search-input"));
            searchFavorite.SendKeys(favoriteName + Keys.Enter);

            WaitUntilElementIsInvisibleXpath("//span[text()='Please wait while products are loading']");
            WaitForElementToLoad($"//a[contains(text(), '{favoriteName}')]", SelectorType.XPath, maxWaitTime);

            WaitForElementToLoad("//span[contains(text(), 'Add to Cart')]", SelectorType.XPath, maxWaitTime);

            FluentWaitUntilElementVisibleForXpath("//span[contains(text(), 'Add to Cart')]", maxWaitTime);
            driver.FindElement(By.XPath("//span[contains(text(), 'Add to Cart')]")).Click();

        }
    }
}
