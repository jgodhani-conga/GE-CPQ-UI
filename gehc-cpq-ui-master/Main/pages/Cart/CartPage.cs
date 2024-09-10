using AutomationProject1.Main.utils;
using cpq_ui_master.Main.UIElements.Cart;
using cpq_ui_master.Main.UIElements.Catalog;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace cpq_ui_master.Main.pages.Cart
{
    public class CartPage : StartupPage
    {
        private CartPageElements cartPageElements;
        private IList<IWebElement> ListofProductName;
        private List<string> ProductName;
        public CartPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
            cartPageElements = new CartPageElements(driver);
        }

        private void WaitForCartPageLoading()
        {
            WaitForElementToLoad(cartPageElements.QuantityColumnField, SelectorType.XPath, maxWaitTime);
        }

        public void ChangeQuantity(string quantity)
        {
            SwitchToIFrame();
            WaitForCartPageLoading();
            QuantityChangeForEachProduct(quantity);
            SwitchToDefaultContent();
        }

        private void QuantityChangeForEachProduct(string quantity)
        {
            WaitForElementToLoad(cartPageElements.QuantityColumnField, SelectorType.XPath, maxWaitTime);
            foreach (var item in cartPageElements.Quantity)
            {
                item.Clear();
                item.SendKeys(quantity);
            }

        }

        public void ClickRepriceBtn()
        {
            WaitForRepriceToLoad();
            cartPageElements.RepriceBtn.Click();
        }

        private void WaitForRepriceToLoad()
        {
            WaitForElementToLoad(cartPageElements.Repricebutton, SelectorType.XPath, maxWaitTime);
        }

        public void waitForCartPageToLoad()
        {
            WaitForProgressBarToComplete();
            WaitForElementToLoad(cartPageElements.ProductNamePath, SelectorType.CssSelector, maxWaitTime);
            IList<IWebElement> ListOfProductNamePath = driver.FindElements(By.CssSelector(cartPageElements.ProductNamePath));
            Console.WriteLine("ListOfProductNamePath", ListOfProductNamePath.Count);
            this.ProductName = new List<string>();
            if (ListOfProductNamePath.Count > 0)
            {
                foreach (var productNameElement in ListOfProductNamePath)
                {
                    this.ProductName.Add(productNameElement.Text);
                }
            }
        }
        public void updateQuanityPerProduct(string productName, int quantity = 1)
        {
            int productIndex = this.ProductName.IndexOf(productName);
            Console.WriteLine("this.ProductName", this.ProductName);
            if (quantity > 1)
            {
                updateQuantity(quantity, productIndex);
            }
        }
      
        public void applyAdjustment(string productName, string valueToSelect = "", double AdjustmentAmount = 0)
        {
            SwitchToIFrame();
            WaitForProgressBarToComplete();
            int productIndex = this.ProductName.IndexOf(productName);
            if (!string.IsNullOrEmpty(valueToSelect))
            {
                selectValueInPicklist(productIndex, valueToSelect);
                addAmountInAdjustmentAmount(AdjustmentAmount, productIndex);
            }
            SwitchToDefaultContent();
        }

        public void applyAdjustmentForGE(string productName, string valueToSelect = "", double AdjustmentAmount = 0)
        {
            WaitForProgressBarToComplete();
            int productIndex = this.ProductName.IndexOf(productName);
            if (!string.IsNullOrEmpty(valueToSelect))
            {
                selectValueInPicklistForGE(productIndex, valueToSelect);
                addAmountInAdjustmentAmount(AdjustmentAmount, productIndex);
            }
        }
        public void selectValueInPicklistForGE(int productIndex, string valueToSelect)
        {
            WaitForElementToLoad(cartPageElements.adjustmentAmountForGE.Replace("{productIndex}", (productIndex + 1).ToString()), SelectorType.XPath, maxWaitTime);
            IWebElement AdjustmentTypeField = driver.FindElement(By.XPath(cartPageElements.adjustmentAmountForGE.Replace("{productIndex}", (productIndex + 1).ToString())));
            AdjustmentTypeField.Click();
            WaitForElementToLoad("md-select-value.md-select-placeholder", SelectorType.CssSelector, maxWaitTime);
            var ClickOnDropDown = driver.FindElement(By.CssSelector("md-select-value.md-select-placeholder"));
            ClickOnDropDown.Click();
            WaitForElementToLoad($"(//md-option[@value='{valueToSelect}'])[{this.ProductName.Count}]", SelectorType.XPath, maxWaitTime);
            IWebElement AdjustmentAmountFieldPickListValue = driver.FindElement(By.XPath($"(//md-option[@value='{valueToSelect}'])[{this.ProductName.Count}]"));
            AdjustmentAmountFieldPickListValue.Click();
        }

        public void updateQuantityandAdjustment(string productName, int quantity = 1, string valueToSelect = "", double AdjustmentAmount = 0)
        {
            SwitchToIFrame();
            WaitForProgressBarToComplete();
            WaitForElementToLoad(cartPageElements.ProductNamePath, SelectorType.CssSelector, maxWaitTime);
            IList<IWebElement> ListOfProductNamePath = driver.FindElements(By.CssSelector(cartPageElements.ProductNamePath));
            this.ProductName = new List<string>();
            if (ListOfProductNamePath.Count > 0)
            {
                foreach (var productNameElement in ListOfProductNamePath)
                {
                    this.ProductName.Add(productNameElement.Text);
                }
                int productIndex = ProductName.IndexOf(productName);
                if (quantity > 1)
                {
                    updateQuantity(quantity, productIndex);
                }
                if (!string.IsNullOrEmpty(valueToSelect))
                {
                    selectValueInPicklist(productIndex, valueToSelect);
                    addAmountInAdjustmentAmount(AdjustmentAmount, productIndex);
                }
            }
            SwitchToDefaultContent();
        }

        public void WaitForProgressBarToComplete()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d =>
            {
                var progressBar = d.FindElement(By.Id("ngProgress"));
                return progressBar.GetCssValue("width").Equals("0px", StringComparison.OrdinalIgnoreCase);
            });
        }
        public void updateQuantity(int number, int productIndex)
        {
            WaitForElementToLoad(cartPageElements.quantityPath.Replace("{productIndex}", (productIndex + 1).ToString()), SelectorType.XPath, maxWaitTime);
            IWebElement quantityField = driver.FindElement(By.XPath(cartPageElements.quantityPath.Replace("{productIndex}", (productIndex + 1).ToString())));
            string inputValue = quantityField.GetAttribute("value");
            quantityField.Clear();
            quantityField.SendKeys(number.ToString());
        }

        public void selectValueInPicklist(int productIndex, string valueToSelect)
        {
            WaitForElementToLoad(cartPageElements.adjustmentTypeFieldPath.Replace("{productIndex}", (productIndex + 1).ToString()), SelectorType.XPath, maxWaitTime);
            IWebElement AdjustmentTypeField = driver.FindElement(By.XPath(cartPageElements.adjustmentTypeFieldPath.Replace("{productIndex}", (productIndex + 1).ToString())));
            AdjustmentTypeField.Click();
            WaitForElementToLoad($"(//md-option[@value='{valueToSelect}'])[{this.ProductName.Count}]", SelectorType.XPath, maxWaitTime);
            IWebElement AdjustmentAmountFieldPickListValue = driver.FindElement(By.XPath($"(//md-option[@value='{valueToSelect}'])[{this.ProductName.Count}]"));
            AdjustmentAmountFieldPickListValue.Click();
        }

        public void addAmountInAdjustmentAmount(double amount, int productIndex)
        {
            WaitForElementToLoad(cartPageElements.adjustmentAmountFieldPath.Replace("{productIndex}", (productIndex + 1).ToString()), SelectorType.XPath, maxWaitTime);
            IWebElement AdjustmentAmountField = driver.FindElement(By.XPath(cartPageElements.adjustmentAmountFieldPath.Replace("{productIndex}", (productIndex + 1).ToString())));
            AdjustmentAmountField.Clear();
            AdjustmentAmountField.SendKeys(amount.ToString());
        }

        public double quanityAfterReprice(string ProductNames)
        {
            /*SwitchToIFrame();*/
            int productIndex = ProductName.IndexOf(ProductNames);
            IWebElement quantityField = driver.FindElement(By.XPath(cartPageElements.quantityPath.Replace("{productIndex}", (productIndex + 1).ToString())));
            string inputValue = quantityField.GetAttribute("value").ToString();
            /*   SwitchToDefaultContent();*/
            /*      Console.Write("method"+int.Parse(inputValue));*/
            return double.Parse(inputValue);

        }

        public void waitForUpdatingCart()
        {
            WaitUntilElementIsInvisibleXpath("//div[@id='progress-bar-container']//p[@id='progress-bar-left' and contains(text(), 'Updating Line Items')]");
        }


        public void clickOnAddMoreProducts()
        {

            WaitUntillElementToBeClickbleForXpath("//button[@buttonid='id_task_left_addmoreproducts']", maxWaitTime);
            WaitForElementToLoad("//button[@buttonid='id_task_left_addmoreproducts']", SelectorType.XPath, maxWaitTime);
            IWebElement addProductsButtons = driver.FindElement(By.XPath("//button[@buttonid='id_task_left_addmoreproducts']"));
            addProductsButtons.Click();
        }

        public void ClickOnCheckBoxA(string ProductName)
        {
            int ProductIndex = this.ProductName.IndexOf(ProductName);

            WaitForElementToLoad(cartPageElements.ClickOnCheckBox.Replace("{ProductIndex}", (ProductIndex + 1).ToString()), SelectorType.XPath, maxWaitTime);
            IWebElement ClickCheckBox = driver.FindElement(By.XPath(cartPageElements.ClickOnCheckBox.Replace("{ProductIndex}", (ProductIndex + 1).ToString())));
            ClickCheckBox.Click();
        }
        public void ClickOnFinalize()
        {
            WaitUntillElementToBeClickbleForXpath("//button[@ng-click='displayAction.doAction(displayAction.primaryAction)']", maxWaitTime);
            WaitForElementToLoad("//button[@ng-click='displayAction.doAction(displayAction.primaryAction)']", SelectorType.XPath, maxWaitTime);
            IWebElement clickFinalizeBtn = driver.FindElement(By.XPath("//button[@ng-click='displayAction.doAction(displayAction.primaryAction)']"));
            clickFinalizeBtn.Click();

        }
        public string checkFieldValueFromResponse(string configJson, string productName, string fieldName)
        {
            if (!string.IsNullOrEmpty(configJson))
            {
                var parsedJson = JObject.Parse(configJson);
                var LineItems = parsedJson["CartResponse"]?["LineItems"];
                Console.WriteLine(LineItems);
                if (LineItems == null)
                {
                    Assert.Fail("LineItems not found in CartResponse.");
                    return null;
                }

                var item = LineItems.OfType<JObject>().FirstOrDefault(x => x["Name"]?.ToString() == productName);
                Console.WriteLine("item=" + item);
                if (item == null)
                {
                    Assert.Fail($"Product with name '{productName}' not found.");
                    return null;
                }
                var fieldValue = item[$"{fieldName}"];
                if (fieldValue == null)
                {
                    Assert.Fail($"Field '{fieldName}' not found for the product '{productName}'.");
                    return null;
                }

                return fieldValue.ToString();
            }
            else
            {
                Assert.Fail("configJson was not received.");
                return null;
            }
        }
        public void ClickOnMassUpdate()
        {
            /*          SwitchToIFrame();*/
            WaitForElementToLoad(cartPageElements.ClickOnMassUpdate, SelectorType.XPath, maxWaitTime);
            cartPageElements.ClickOnMassUpdateBtn.Click();
            /*     SwitchToDefaultContent();*/
        }

        public void UpdateQuantityInMassUpdate()
        {
            WaitForElementToLoad("//input[contains(@class, 'aptQuantity') and @type='mass update']", SelectorType.XPath, maxWaitTime);
            IWebElement updateMassQuantity = driver.FindElement(By.XPath("//input[contains(@class, 'aptQuantity') and @type='mass update']"));

            updateMassQuantity.SendKeys("3");

        }
        public void ClickOnApplyButton()
        {
            WaitForElementToLoad(cartPageElements.ClickOnApply, SelectorType.XPath, maxWaitTime);
            cartPageElements.ClickOnApplyBtn.Click();
        }
        public void applyAdjustmentPerProduct(JToken productData)
        {
            foreach (var item in productData)
            {
                WaitForProgressBarToComplete();
                int productIndex = this.ProductName.IndexOf(item["ProductName"].ToString());
                if (!string.IsNullOrEmpty(item["AdjustMentType"].ToString()))
                {
                    selectValueInPicklist(productIndex, item["AdjustMentType"].ToString());
                    addAmountInAdjustmentAmount(double.Parse(item["AdjustMentAmount"].ToString()), productIndex);
                }
            }
        }
        public void AssertUpdateQuantity(JToken updateQuantityData)
        {
            foreach (var item in updateQuantityData)
            {
                Assert.That(double.Parse(item["Quantity"].ToString()).Equals(quanityAfterReprice(item["ProductName"].ToString())), "Quantity1 is not matching");
            }
        }
        public void ClickOnCheckBox(JToken cloneProductName)
        {
            foreach (var item in cloneProductName)
            {
                int ProductIndex = this.ProductName.IndexOf(item.ToString());
                WaitForElementToLoad(cartPageElements.ClickOnCheckBox.Replace("{ProductIndex}", (ProductIndex + 1).ToString()), SelectorType.XPath, maxWaitTime);
                IWebElement ClickCheckBox = driver.FindElement(By.XPath(cartPageElements.ClickOnCheckBox.Replace("{ProductIndex}", (ProductIndex + 1).ToString())));
                ClickCheckBox.Click();

            }
        }
        public void clickOnCloneBtn()
        {
            WaitForElementToLoad("i.fa.cart-actions.fa-clone", SelectorType.CssSelector, maxWaitTime);
            IWebElement cloneButton = driver.FindElement(By.CssSelector("i.fa.cart-actions.fa-clone"));
            cloneButton.Click();
            WaitForProgressBarToComplete();
            waitForUpdatingCart();

        }
        public void checkAssertionItemCloneOrNot(JToken clonedProduct)
        {

            foreach (var item in clonedProduct)
            {
                Assert.That(itemClonedOrNot(item.ToString()), Is.True, "The item was not cloned.");
            }
        }
        public bool itemClonedOrNot(string itemName)
        {
            WaitForElementToLoad(cartPageElements.productPath.Replace("{productName}", (itemName).ToString()), SelectorType.XPath, maxWaitTime);
            IList<IWebElement> productNameList = driver.FindElements(By.XPath(cartPageElements.productPath.Replace("{productName}", (ProductName).ToString())));
            return productNameList.Count != 1;
        }
        public string checkProductExitInCart(string productName)
        {
            return driver.FindElement(By.XPath(cartPageElements.productNameXpath.Replace("{productName}", productName).ToString())).Text;
        }
        public void expandCaretIcon(JToken productData)
        {
            foreach (var item in productData)
            {
                IWebElement caretIcon = driver.FindElement(By.XPath($"//span[@title='{item["ProductName"]}']/ancestor::span[@class='product-name-space']/preceding-sibling::span[contains(@class, 'toggle-row-icon-wrapper')]//a[contains(@class, 'toggle-row-icon') and contains(@class, 'fa-caret-right')]"));
                caretIcon.Click();
            }
        }
        public void AssertConstraintRuleOptionProducts(JToken productData)
        {
            foreach (var bundle in productData)
            {
                var constraintRuleOptionProducts = bundle["constraintRuleOptionProduct"].ToObject<List<string>>();

                // Loop through each option product and assert it exists in the cart
                foreach (var optionProduct in constraintRuleOptionProducts)
                {
                    Assert.That(checkProductExitInCart(optionProduct).Equals(optionProduct),
                                $"Product/Option Product '{optionProduct}' not present in cart");
                }
            }
        }

    }

}
