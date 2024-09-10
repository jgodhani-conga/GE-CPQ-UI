using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cpq_ui_master.Main.UIElements.Catalog
{
    public class ConfigureProductElements
    {
        public ConfigureProductElements(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public string SearchProduct = "//input[@placeholder='Find Products']";

        [FindsBy(How = How.XPath, Using = "//input[@placeholder='Find Products']")]
        public IWebElement findProductsSearch { get; set; }

        public string AddToCartButton = "span.ng-binding.ng-scope[ng-show='(!catalogProduct.isSelectedInCatalog(catalogProduct.product.productSO.Id) || catalogProduct.pageSettings.DisableAddAnother)']";

        [FindsBy(How = How.CssSelector, Using = "span.ng-binding.ng-scope[ng-show='(!catalogProduct.isSelectedInCatalog(catalogProduct.product.productSO.Id) || catalogProduct.pageSettings.DisableAddAnother)']")]
        public IWebElement AddToCartButtons { get; set; }

        public string ConfigureButton = "//button[contains(text(), 'Configure...')]";

         [FindsBy(How = How.CssSelector, Using = "md-icon[aria-label='shopping_cart']")]
        public IWebElement ShoppingCartIcon { get; set; }

        public string NumberOfItemInCart = "//span[@class='mini-cart__display__total line-item-count ng-binding ng-scope']";

        public string ViewCartbutton = "button.mini-cart__actions__view-cart";

        [FindsBy(How = How.CssSelector, Using = "button.mini-cart__actions__view-cart")]
        public IWebElement ViewCartBtn { get; set; }

        public string shoppingCartProductNamePath = "//h3[@class='mini-cart__items__item__title']/span[@class='ng-binding']";

        [FindsBy(How = How.XPath, Using = "//h3[@class='mini-cart__items__item__title']/span[@class='ng-binding']")]
        public IList<IWebElement> shoppingCartProductNames { get; set; }

        public string selectOptionGroup = "//md-tab-item[contains(text(),'{optionGroupName}')]";

        public string optionProductName = "//a[text()='{optionName}']";

        public string validateButton = "button.ValidateBundle";

        public string validateMessagePath = "div.sidebar--configure-recalculate span.ng-binding";

        public string AbandonCartButton = "//md-icon[text()='exit_to_app']";

        [FindsBy(How = How.XPath, Using = "//md-icon[text()='exit_to_app']")]
        public IWebElement AbandonCartBtn { get; set; }

        public string ClickOk = "//button[@ng-click='customAction.abandonCart()']\r\n";

        [FindsBy(How = How.XPath, Using = "//button[@ng-click='customAction.abandonCart()']\r\n")]
        public IWebElement ClickOkBtn { get; set; }

     

        public string ClickOnDeleteIcon = "//i[@class='fa cart-actions fa-trash']";

        [FindsBy(How = How.XPath, Using = "//i[@class='fa cart-actions fa-trash']")]
        public IWebElement ClickOnDeleteIconBtn { get; set; }

        public string ClickOnSelectAllProduct = "//*[@id='cart-checkbox--header']";

        [FindsBy(How = How.XPath, Using = "//*[@id='cart-checkbox--header']")]
        public IWebElement ClickOnSelectAllProductBtn { get; set; }

        public string ClickOnFavorite = "//i[@class='fa cart-actions fa-star']";

        [FindsBy(How = How.XPath, Using = "//i[@class='fa cart-actions fa-star']")]
        public IWebElement ClickOnFavoriteBtn { get; set; }

    }
}
