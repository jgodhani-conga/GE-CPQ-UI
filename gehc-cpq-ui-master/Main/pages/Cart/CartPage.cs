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
        private List<string> productName;
        public CartPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
            cartPageElements = new CartPageElements(driver);
        }

        private void WaitForCartPageLoading()
        {
            WaitForElementToLoad(cartPageElements.quantityColumnField, SelectorType.XPath, maxWaitTime);
        }

        public void ChangeQuantity(string quantity)
        {
            SwitchToIFrame();
            WaitForCartPageLoading();
            quantityChangeForEachProduct(quantity);
            SwitchToDefaultContent();
        }

        private void quantityChangeForEachProduct(string quantity)
        {
            WaitForElementToLoad(cartPageElements.quantityColumnField, SelectorType.XPath, maxWaitTime);
            foreach (var item in cartPageElements.Quantity)
            {
                item.Clear();
                item.SendKeys(quantity);
            }

        }

        public void clickRepriceBtn()
        {
            waitForRepriceToLoad();
            cartPageElements.repriceBtn.Click();
        }

        private void waitForRepriceToLoad()
        {
            WaitForElementToLoad(cartPageElements.repricebutton, SelectorType.XPath, maxWaitTime);
        }

        public void waitForCartPageToLoad()
        {
            waitForProgressBarToComplete();
            WaitForElementToLoad(cartPageElements.productNamePath, SelectorType.CssSelector, maxWaitTime);
            IList<IWebElement> ListOfProductNamePath = driver.FindElements(By.CssSelector(cartPageElements.productNamePath));
            Console.WriteLine("ListOfProductNamePath", ListOfProductNamePath.Count);
            this.productName = new List<string>();
            if (ListOfProductNamePath.Count > 0)
            {
                foreach (var productNameElement in ListOfProductNamePath)
                {
                    this.productName.Add(productNameElement.Text);
                }
            }
        }
        public void updateQuanityPerProduct(JToken productData)
        {
            foreach (var item in productData)
            {
                int productIndex = this.productName.IndexOf(item["ProductName"].ToString());
                Console.WriteLine(item["ProductName"].ToString() + productIndex);
                if (int.Parse(item["Quantity"].ToString()) > 1)
                {
                    updateQuantity(int.Parse(item["Quantity"].ToString()), productIndex);
                }
            }

        }

        public void applyAdjustment(string productName, string valueToSelect = "", double AdjustmentAmount = 0)
        {
            SwitchToIFrame();
            waitForProgressBarToComplete();
            int productIndex = this.productName.IndexOf(productName);
            if (!string.IsNullOrEmpty(valueToSelect))
            {
                selectValueInPicklist(productIndex, valueToSelect);
                addAmountInAdjustmentAmount(AdjustmentAmount, productIndex);
            }
            SwitchToDefaultContent();
        }

        public void applyAdjustmentForGE(string productName, string valueToSelect = "", double AdjustmentAmount = 0)
        {
            waitForProgressBarToComplete();
            int productIndex = this.productName.IndexOf(productName);
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
            WaitForElementToLoad($"(//md-option[@value='{valueToSelect}'])[{this.productName.Count}]", SelectorType.XPath, maxWaitTime);
            IWebElement AdjustmentAmountFieldPickListValue = driver.FindElement(By.XPath($"(//md-option[@value='{valueToSelect}'])[{this.productName.Count}]"));
            AdjustmentAmountFieldPickListValue.Click();
        }

        public void updateQuantityandAdjustment(string productName, int quantity = 1, string valueToSelect = "", double AdjustmentAmount = 0)
        {
            SwitchToIFrame();
            waitForProgressBarToComplete();
            WaitForElementToLoad(cartPageElements.productNamePath, SelectorType.CssSelector, maxWaitTime);
            IList<IWebElement> ListOfProductNamePath = driver.FindElements(By.CssSelector(cartPageElements.productNamePath));
            this.productName = new List<string>();
            if (ListOfProductNamePath.Count > 0)
            {
                foreach (var productNameElement in ListOfProductNamePath)
                {
                    this.productName.Add(productNameElement.Text);
                }
                int productIndex = this.productName.IndexOf(productName);
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

        public void waitForProgressBarToComplete()
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
            WaitForElementToLoad(cartPageElements.adjustmentAmountForGE.Replace("{productIndex}", (productIndex + 1).ToString()), SelectorType.XPath, maxWaitTime);
            IWebElement adjustmentTypeField = driver.FindElement(By.XPath(cartPageElements.adjustmentAmountForGE.Replace("{productIndex}", (productIndex + 1).ToString())));
            adjustmentTypeField.Click();
            WaitForElementToLoad(cartPageElements.adjustmentTypePicklist.Replace("{productLength}", (this.productName.Count).ToString()), SelectorType.XPath, maxWaitTime);
            IWebElement clickOnPicklist = driver.FindElement(By.XPath(cartPageElements.adjustmentTypePicklist.Replace("{productLength}", (this.productName.Count).ToString())));
            clickOnPicklist.Click();
            WaitForElementToLoad($"(//md-option[@value='{valueToSelect}'])[{this.productName.Count}]", SelectorType.XPath, maxWaitTime);
            IWebElement adjustmentAmountFieldPickListValue = driver.FindElement(By.XPath($"(//md-option[@value='{valueToSelect}'])[{this.productName.Count}]"));
            adjustmentAmountFieldPickListValue.Click();
        }

        public void addAmountInAdjustmentAmount(double amount, int productIndex)
        {
            WaitForElementToLoad(cartPageElements.adjustmentAmountFieldPath.Replace("{productIndex}", (this.productName.Count).ToString()), SelectorType.XPath, maxWaitTime);
            IWebElement AdjustmentAmountField = driver.FindElement(By.XPath(cartPageElements.adjustmentAmountFieldPath.Replace("{productIndex}", (this.productName.Count).ToString())));
            AdjustmentAmountField.Clear();
            AdjustmentAmountField.SendKeys(amount.ToString());
            WaitForElementToLoad($"(//button[text()='Apply'])[{this.productName.Count}]", SelectorType.XPath, maxWaitTime);
            IWebElement clickOnApplyBtn = driver.FindElement(By.XPath($"(//button[text()='Apply'])[{this.productName.Count}]"));
            clickOnApplyBtn.Click();

        }


        public double quanityAfterReprice(string productNames)
        {
            int productIndex = this.productName.IndexOf(productNames);
            IWebElement quantityField = driver.FindElement(By.XPath(cartPageElements.quantityPath.Replace("{productIndex}", (productIndex + 1).ToString())));
            string inputValue = quantityField.GetAttribute("value").ToString();
            return double.Parse(inputValue);

        }

        public void waitForUpdatingCart()
        {
            WaitUntilElementIsInvisible("//div[@id='progress-bar-container']//p[@id='progress-bar-left' and contains(text(), 'Updating Line Items')]", SelectorType.XPath, maxWaitTime);
        }


        public void clickOnAddMoreProducts()
        {

            WaitUntillElementToBeClickble("//button[@buttonid='id_task_left_addmoreproducts']", SelectorType.XPath, maxWaitTime);
            WaitForElementToLoad("//button[@buttonid='id_task_left_addmoreproducts']", SelectorType.XPath, maxWaitTime);
            IWebElement addProductsButtons = driver.FindElement(By.XPath("//button[@buttonid='id_task_left_addmoreproducts']"));
            addProductsButtons.Click();
        }
        public void clickOnFinalize()
        {
            WaitUntillElementToBeClickble("//button[@ng-click='displayAction.doAction(displayAction.primaryAction)']", SelectorType.XPath, maxWaitTime);
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
        public void clickOnMassUpdate()
        {
            WaitForElementToLoad(cartPageElements.clickOnMassUpdate, SelectorType.XPath, maxWaitTime);
            cartPageElements.clickOnMassUpdateBtn.Click();
        }

        public void updateQuantityInMassUpdate()
        {
            WaitForElementToLoad("//input[contains(@class, 'aptQuantity') and @type='mass update']", SelectorType.XPath, maxWaitTime);
            IWebElement updateMassQuantity = driver.FindElement(By.XPath("//input[contains(@class, 'aptQuantity') and @type='mass update']"));

            updateMassQuantity.SendKeys("3");

        }
        public void clickOnApplyButton()
        {
            WaitForElementToLoad(cartPageElements.clickOnApply, SelectorType.XPath, maxWaitTime);
            cartPageElements.clickOnApplyBtn.Click();
        }
        public void applyAdjustmentPerProduct(JToken productData)
        {
            foreach (var item in productData)
            {
                waitForProgressBarToComplete();
                int productIndex = this.productName.IndexOf(item["ProductName"].ToString());
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
        public void clickOnCheckBox(JToken cloneProductName)
        {
            foreach (var item in cloneProductName)
            {
                int ProductIndex = this.productName.IndexOf(item.ToString());
                WaitForElementToLoad(cartPageElements.clickOnCheckBox.Replace("{ProductIndex}", (ProductIndex + 1).ToString()), SelectorType.XPath, maxWaitTime);
                IWebElement ClickCheckBox = driver.FindElement(By.XPath(cartPageElements.clickOnCheckBox.Replace("{ProductIndex}", (ProductIndex + 1).ToString())));
                ClickCheckBox.Click();

            }
        }
        public void clickOnCloneBtn()
        {
            WaitForElementToLoad("i.fa.cart-actions.fa-clone", SelectorType.CssSelector, maxWaitTime);
            IWebElement cloneButton = driver.FindElement(By.CssSelector("i.fa.cart-actions.fa-clone"));
            cloneButton.Click();
            waitForProgressBarToComplete();
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
            IList<IWebElement> productNameList = driver.FindElements(By.XPath(cartPageElements.productPath.Replace("{productName}", (this.productName).ToString())));
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
