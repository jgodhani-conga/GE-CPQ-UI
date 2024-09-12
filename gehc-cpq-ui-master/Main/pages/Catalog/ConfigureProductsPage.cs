using AutomationProject1.Main.utils;
using cpq_ui_master.Main.UIElements.Cart;
using cpq_ui_master.Main.UIElements.Catalog;
using gehc_cpq_ui_master.Main.UIElements.Proposal;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace cpq_ui_master.Main.pages.Catalog
{
    public class ConfigureProductsPage : StartupPage
    {
        private ConfigureProductElements configureProele;
        private string solutionDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private string filePath;
        private List<string> optionGroupName;


        public ConfigureProductsPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
            configureProele = new ConfigureProductElements(driver);
        }


        public void addProductUsingJsonFile()
        {
            WaitForElementToLoad(configureProele.searchProduct, SelectorType.XPath, maxWaitTime);
            filePath = Path.Combine(solutionDirectory, "Main", "resources");
            string jsonFilePath = Path.Combine(filePath, "ProductCodes", "ProductCodes.json");
            var jsonData = JObject.Parse(File.ReadAllText(jsonFilePath))["Verfity_TC_02"];
            foreach (var productCode in jsonData["ProductCodes"])
            {
                configureProele.findProductsSearch.SendKeys(productCode.ToString() + Keys.Enter); 
                WaitForElementToLoad(configureProele.addToCartButton, SelectorType.CssSelector, maxWaitTime);
                configureProele.addToCartButtons.Click();
                configureProele.findProductsSearch.Clear();
            }
        }

        public void addBundleProduct(string productname)
        {
            WaitForElementToLoad(configureProele.searchProduct, SelectorType.XPath, maxWaitTime);
            configureProele.findProductsSearch.SendKeys(productname.ToString() + Keys.Enter);
            WaitForElementToLoad(configureProele.configureButton, SelectorType.XPath, maxWaitTime);
            IWebElement ConfigureBtn = driver.FindElement(By.XPath(configureProele.configureButton));
            ConfigureBtn.Click();
            WaitForElementToLoad("//md-pagination-wrapper//md-tab-item", SelectorType.XPath, maxWaitTime);
            IList<IWebElement> optionGroupNames = driver.FindElements(By.XPath("//md-pagination-wrapper//md-tab-item"));
            this.optionGroupName = new List<string>();
            foreach(var optionGroupName in optionGroupNames)
            {
                this.optionGroupName.Add(optionGroupName.Text);
            }
        }
        public void addBundleProduct(JToken productsData, ConfigureProductsPage configureProductsPage)
        {
            int totalRecord = productsData.Count();
            int count = 1;
            foreach (var productData in productsData)
            {
                // Add each bundle product
                WaitForElementToLoad(configureProele.searchProduct, SelectorType.XPath, maxWaitTime);
                configureProele.findProductsSearch.SendKeys(productData["ProductName"].ToString() + Keys.Enter);
                WaitForElementToLoad(configureProele.configureButton, SelectorType.XPath, maxWaitTime);
                IWebElement ConfigureBtn = driver.FindElement(By.XPath(configureProele.configureButton));
                ConfigureBtn.Click();
                WaitForElementToLoad("//md-pagination-wrapper//md-tab-item", SelectorType.XPath, maxWaitTime);

                IList<IWebElement> optionGroupNames = driver.FindElements(By.XPath("//md-pagination-wrapper//md-tab-item"));
                this.optionGroupName = new List<string>();
                foreach (var optionGroupName in optionGroupNames)
                {
                    this.optionGroupName.Add(optionGroupName.Text);
                }

                // Add option products for each bundle
                addOptionProductsFromJson(productData, configureProductsPage);

                if (count < totalRecord)
                {
                    clickOnAddMoreProducts();
                    waitForCatalogPageToLoad();
                    count++;
                }


            }
        }
        public void clickOnAddMoreProducts()
        {

            WaitUntillElementToBeClickbleForXpath("//button[@buttonid='id_task_left_addmoreproducts']", maxWaitTime);
            WaitForElementToLoad("//button[@buttonid='id_task_left_addmoreproducts']", SelectorType.XPath, maxWaitTime);
            IWebElement addProductsButtons = driver.FindElement(By.XPath("//button[@buttonid='id_task_left_addmoreproducts']"));
            addProductsButtons.Click();
        }
       
       
        private void addOptionProductsFromJson(JToken productData, ConfigureProductsPage configureProductsPage)
        {
            foreach (var optionGroup in productData["OptionGroups"])
            {
                string optionGroupName = optionGroup["OptionGroup"].ToString();
                foreach (var optionProduct in optionGroup["OptionProducts"])
                {
                    string optionProductName = optionProduct.ToString();
                    configureProductsPage.addOptionProductForBundle(optionGroupName, optionProductName);
                }
            }

        }
        public void addOptionProductForBundle(string optionGroupName, string optionProductName)
        {

            string optionGroupXpath = configureProele.selectOptionGroup.Replace("{optionGroupName}", optionGroupName);
            WaitForElementToLoad(optionGroupXpath, SelectorType.XPath, maxWaitTime);
            IWebElement optionGroupTab = driver.FindElement(By.XPath(optionGroupXpath));
            optionGroupTab.Click();
            string ariaControlsValue = optionGroupTab.GetAttribute("aria-controls");
            IWebElement targetElement = driver.FindElement(By.CssSelector($"#{ariaControlsValue} .main-configure-product__product-option.ng-scope"));

            if (!targetElement.GetAttribute("class").Contains("is--open"))
            {
                targetElement.Click();
            }
            var subGroupElements = driver.FindElements(By.CssSelector($"#{ariaControlsValue} .sub-group.ng-scope"));
            if (subGroupElements.Count > 0)
            {
                foreach (var subGroupElement in subGroupElements)
                {
                    var nestedElement = subGroupElement.FindElement(By.CssSelector(".main-configure-product__product-option.ng-scope"));
                    bool isOpen = nestedElement.GetAttribute("class").Contains("is--open");

                    if (isOpen)
                    {
                        Console.WriteLine("Nested element is already expanded.");
                    }
                    else
                    {
                        Console.WriteLine("Nested element is not expanded. Clicking to expand...");
                        subGroupElement.Click();
                    }
                }
            }
            else
            {
                Console.WriteLine("No sub-group elements found within the specified section.");
            }

            // Locate the checkbox for the option product and select it if necessary
            int optionGroupIndex = this.optionGroupName.FindIndex(name => name.Contains(optionGroupName));
            IWebElement checkbox = driver.FindElement(By.XPath($"(//a[text()='{optionProductName}']/ancestor::div[contains(@class, 'main-configure-product__product-option-container')]//div[contains(@class, 'checkbox-override') or contains(@class, 'radio-override')]//input)[{optionGroupIndex + 1}]"));

            if (!checkbox.Selected)
            {
                // If not selected, click the checkbox or radio button
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
            }
        }
        public void clickOnGoTopricing()
        {
            WaitForElementToLoad(configureProele.validateButton, SelectorType.CssSelector, maxWaitTime);
            IWebElement validateBtn = driver.FindElement(By.CssSelector(configureProele.validateButton));
            validateBtn.Click();

            WaitUntilElementIsInvisible(configureProele.validateMessagePath);
            WaitUntillElementToBeClickble("button.GoToPricing", maxWaitTime);
            WaitForElementToLoad("button.GoToPricing", SelectorType.CssSelector, maxWaitTime);
            IWebElement goToPricing = driver.FindElement(By.CssSelector("button.GoToPricing"));
            goToPricing.Click();
        }


        public void addProductUsingCode(string productname) {
            WaitForElementToLoad(configureProele.searchProduct, SelectorType.XPath, maxWaitTime);
            configureProele.findProductsSearch.SendKeys(productname.ToString() + Keys.Enter);
            WaitForElementToLoad(configureProele.addToCartButton, SelectorType.CssSelector, maxWaitTime);
            configureProele.addToCartButtons.Click();
            configureProele.findProductsSearch.Clear();
        }

        public void waitForCatalogPageToLoad()
        {
            FluentWaitUntilElementVisibleForXpath(configureProele.searchProduct, maxWaitTime);
        }

        public void clickOnShoppingCart()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30)); // Adjust the timeout as needed

            // Wait until the totalValue is greater than zero
            wait.Until(driver =>
            {
                // Find the element and get its text
                var miniCartTotal = driver.FindElement(By.XPath(configureProele.numberOfItemInCart));
                string totalValueText = miniCartTotal.Text.Trim();
                int totalValue = int.Parse(totalValueText);
                return totalValue > 0;
            });
            configureProele.shoppingCartIcon.Click();
        }

        public void clickOnViewCartButton()
        {
            WaitForElementToLoad(configureProele.viewCartbutton, SelectorType.CssSelector, maxWaitTime);
            configureProele.viewCartBtn.Click();
        }

        public int checkNumberOfProductInCart()
        {
            var miniCartTotal = driver.FindElement(By.XPath(configureProele.numberOfItemInCart));
            string totalValueText = miniCartTotal.Text.Trim();
            int totalValue = int.Parse(totalValueText);
            return totalValue;
        }

        public void clickOnAbandonCart()
        {
            WaitForElementToLoad(configureProele.abandonCartButton, SelectorType.XPath, maxWaitTime);
            configureProele.abandonCartBtn.Click();
        }
        public void clickOkButton()
        {
            WaitForElementToLoad(configureProele.clickOk, SelectorType.XPath, maxWaitTime);
            configureProele.clickOkBtn.Click();
        }
       
        public void clickOnDeleteIcon()
        {
            WaitForElementToLoad(configureProele.ClickOnDeleteIcon, SelectorType.XPath, maxWaitTime);
            configureProele.ClickOnDeleteIconBtn.Click();
        }
        public void clickOnSelectAllProduct()
        {
            WaitForElementToLoad(configureProele.ClickOnSelectAllProduct, SelectorType.XPath, maxWaitTime);
            configureProele.ClickOnSelectAllProductBtn.Click();
        }
        public void addProductsToCart(JToken productNames)
        {
            foreach (var productCode in productNames)
            {
                configureProele.findProductsSearch.SendKeys(productCode.ToString() + Keys.Enter);
                WaitForElementToLoad(configureProele.addToCartButton, SelectorType.CssSelector, maxWaitTime);
                configureProele.addToCartButtons.Click();
                configureProele.findProductsSearch.Clear();
            }
        }

    }
}
