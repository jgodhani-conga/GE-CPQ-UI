using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpq_ui_master.Main.UIElements.Favorite
{
    public class FavoritePageElement
    {
        public FavoritePageElement(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public string ClickOnFavorite = "//i[@class='fa cart-actions fa-star']";

        [FindsBy(How = How.XPath, Using = "//i[@class='fa cart-actions fa-star']")]
        public IWebElement ClickOnFavoriteBtn { get; set; }


        public string ClickOnSave = "//button[@type='button' and contains(@class, 'ands-btn') and contains(@class, 'ands-primary') and @ng-click='saveAsFavorite.validateAndSave()']";

        [FindsBy(How = How.XPath, Using = "//button[@type='button' and contains(@class, 'ands-btn') and contains(@class, 'ands-primary') and @ng-click='saveAsFavorite.validateAndSave()']")]
        public IWebElement ClickOnSaveBtn { get; set; }


        [FindsBy(How = How.XPath, Using = "//input[@type='text' and @ng-model='dynamicField.getSetModel' and @ng-blur='dynamicField.onFocusOut()' and @ng-disabled='dynamicField.IsDisabled || !dynamicField.IsEditable']")]
        public IWebElement inputTxtFavName { get; set; }

    }
}
