using AutomationProject1.Main.pages;
using AutomationProject1.Main.utils;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using cpq_ui_master.Main.Config;
using cpq_ui_master.Main.pages.Cart;
using cpq_ui_master.Main.pages.Catalog;
using gehc_cpq_ui_master.Main.pages.Proposal;
using gehc_cpq_ui_master.Main.utils;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V127.DevToolsSessionDomains;
using cpq_ui_master.Main.pages.Favorite;


namespace AutomationProject1.test
{
    [TestFixture]
    public class Test1
    {
        public LoginPage loginPage;
        public IWebDriver driver;
        private Assert assert;
        private ProposalListPage proposalListPage;
        private CreateNewProposalPopup createNewProposal;
        private string jsonFilePath;
        private ProposalDetailsPage proposalDetailsPage;
        private ConfigureProductsPage configureProductsPage;
        private CartPage cartPage;
        private URLGenerator urlGenerator;
        private string solutionDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private string filePath;
        private string ProposalDetailPath;
        private string ProposalId;
        private string ProposalDetailsPageUrl;
        ExtentReports extent;
        ExtentTest test;
        private object configJson;
        private string originalWindowHandle;
        private StartupPage startupPage;
        private FavoritePage favoritePage;

        [OneTimeSetUp]
        public void setupMethod()
        {
            extent = ReportsGenerationClass.StartReporting();
            WebDriverUtils utils = new WebDriverUtils();
            utils.initializeDriver("Chrome");
            driver = utils.GetDriver();
            urlGenerator = new URLGenerator();
            filePath = Path.Combine(solutionDirectory, "Main", "resources");

            ProposalDetailsPageUrl = "";
            loginPage = new LoginPage(driver, urlGenerator);
            proposalListPage = loginPage.login();
            createNewProposal = new CreateNewProposalPopup(driver);
            proposalDetailsPage = new ProposalDetailsPage(driver);
            configureProductsPage = new ConfigureProductsPage(driver);
            cartPage = new CartPage(driver);
            startupPage = new StartupPage(driver);
            favoritePage = new FavoritePage(driver);

        }
        [SetUp]
        public async Task beforeMethod()
        {

            if (!driver.Url.Contains(urlGenerator.proposalListPageURL))
                proposalListPage.returnToProposalListPage(urlGenerator.proposalListPageURL);
        }

        [Test(Author = "Brijesh Paghdal", Description = "TC_1001 : Create New Propsoal")]
        public void CreateNewProposal()
        {
            test = extent.CreateTest("Create New Proposal");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Create New Proposal");
            try
            {
                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                Assert.That(newProposalName.Equals(proposalDetailsPage.getProposalName()), "Proposal Name are not matching");
                if (!driver.Url.Equals(this.ProposalDetailsPageUrl))
                    driver.Navigate().GoToUrl(this.ProposalDetailsPageUrl);
                ReportsGenerationClass.LogInfo(test, "Test Completed.");
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                ReportsGenerationClass.LogInfo(test, "Test completed.");
            }
        }

        [Test(Author = "Alay Patel", Description = "TC_1002 : Add Single Standalone Product")]
        public void AddProductToCart()
        {
            test = extent.CreateTest("Add Product To cart");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Add Product To cart");

            try
            {
                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");
                var testData1 = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["TestData1"];
                var testData2 = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["TestData2"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();

                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding Products To Cart");
                configureProductsPage.addProductUsingJsonFile();
                ReportsGenerationClass.LogInfo(test, "Added Products to Cart Successfully");
                configureProductsPage.clickOnShoppingCart();
                Assert.That(configureProductsPage.checkNumberOfProductInCart(), Is.GreaterThan(0), "Product is not added to cart");
                configureProductsPage.clickOnViewCartButton();
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                Assert.Fail($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Test completed.");
        }
    }

        [Test(Author = "Alay Patel", Description = "TC_1003 : Add Bundle Product")]
        public void AddBudleProductToCart()
        {
            test = extent.CreateTest("Add Bundle Product To cart");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Add Product To cart");

            try
            {
                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");

                var productData = JObject.Parse(File.ReadAllText(TestDatafilePath))["Products"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();

                //Switch to the new tab
                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);
                configureProductsPage.waitForCatalogPageToLoad();
                configureProductsPage.addBundleProduct(productData, configureProductsPage);
                ReportsGenerationClass.LogInfo(test, "Added Bundle Products To Cart Successfully");
                configureProductsPage.clickOnGoTopricing();
                cartPage.waitForCartPageToLoad();
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Test completed.");
            }
           
        }

        [Test(Author = "Alay Patel", Description = "TC_1004 : Update Quantity and Apply Adjustment")]
        public void UpdateQuantityandApplyAdjustment()
        {
            test = extent.CreateTest("Update Quantity and Apply Adjustment");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Update Quantity and Apply Adjustment");

            try
            {
                string configJson = null;
                string statusRequestId = null;

                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];

                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");
                var testData1 = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["TestData1"];
                var testData2 = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["TestData2"];

                var updateQuantityAndApplyAdjustmentTestData = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["updateQuantityAndApplyAdjustmentTestData"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();

                // Switch to the new tab
                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding Products To Cart");
                configureProductsPage.addProductsToCart(updateQuantityAndApplyAdjustmentTestData["Products"]);
                ReportsGenerationClass.LogInfo(test, "Added Products To Cart Successfully");
                configureProductsPage.clickOnShoppingCart();
                configureProductsPage.clickOnViewCartButton();

                cartPage.waitForCartPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Cart page is loaded successfully");
                IDevTools devTools = driver as IDevTools;
                var session = devTools.GetDevToolsSession();
                var domain = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
                var tcs = new TaskCompletionSource<bool>();
                domain.Network.Enable(new OpenQA.Selenium.DevTools.V127.Network.EnableCommandSettings());

                domain.Network.RequestWillBeSent += (sender, e) =>
                {
                    if (e.Request.Url.Contains("/status?includeConfigResponse=true&includes=price-breakups&includes=usage-tiers&includes=summary-groups&includes=adjustments&includes=line-items&includes=line-items.pricelist&includes=line-items.product&includes=line-items.pricelistitem&primaryLineNumbers") && e.Request.Method.Equals("GET"))
                    {
                        statusRequestId = e.RequestId;
                    }
                };
                // Capture response details
                domain.Network.ResponseReceived += (sender, e) =>
                {
                    // Ensure that the request method is DELETE
                    if (e.Response.Url.Contains("/status?includeConfigResponse=true&includes=price-breakups&includes=usage-tiers&includes=summary-groups&includes=adjustments&includes=line-items&includes=line-items.pricelist&includes=line-items.product&includes=line-items.pricelistitem") && e.Response.Status.Equals(200))
                    {
                        Console.WriteLine($"Response URL: {e.Response.Url}");
                        Console.WriteLine($"Response Status: {e.Response.Status}");
                        // Fetch the response body asynchronously
                        var bodyResponse = domain.Network.GetResponseBody(new OpenQA.Selenium.DevTools.V127.Network.GetResponseBodyCommandSettings
                        {
                            RequestId = statusRequestId
                        }).Result;
                        configJson = bodyResponse.Body;
                    }
                };
                ReportsGenerationClass.LogInfo(test, "Updating the Quantity");
                cartPage.updateQuanityPerProduct(updateQuantityAndApplyAdjustmentTestData["updateQantityandAdjustMent"]);
                ReportsGenerationClass.LogInfo(test, "Updated the Quantity Successfully");
                ReportsGenerationClass.LogInfo(test, "Appling an Adjustment Type and Value");
                cartPage.applyAdjustmentPerProduct(updateQuantityAndApplyAdjustmentTestData["updateQantityandAdjustMent"]);
                ReportsGenerationClass.LogInfo(test, "Applied an Adjustment Type and Value Successfully");
                cartPage.clickRepriceBtn();
                cartPage.waitForUpdatingCart();
                ReportsGenerationClass.LogInfo(test, "Updated Cart Succesfully");
                cartPage.AssertUpdateQuantity(updateQuantityAndApplyAdjustmentTestData["updateQantityandAdjustMent"]);

                // Wait for the specific request to finish
               /* if (!string.IsNullOrEmpty(configJson))
                {
                    var parsedJson = JObject.Parse(configJson);
                    Console.WriteLine("parsedJson: " + parsedJson);
                }
                else
                {
                    Assert.Fail("configJson was not received.");
                }*/
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed.");
            }
        }

        [Test(Author = "Jay Godhani", Description = "TC_1005 : Abandon The Cart")]
        public void AbandonCart()
        {
            test = extent.CreateTest("Abandon the cart");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Abandon the cart");
            try
            {
                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");
                var abandonCartTestData = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["AbandonCartTestData"];
                ReportsGenerationClass.LogInfo(test, "Clicking On New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();
                originalWindowHandle = driver.CurrentWindowHandle;
                // Switch to the new tab
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding Products To Cart");
                configureProductsPage.addProductsToCart(abandonCartTestData["Products"]);
                ReportsGenerationClass.LogInfo(test, "Added Products To Cart Successfully");
                configureProductsPage.clickOnShoppingCart();
                Assert.That(configureProductsPage.checkNumberOfProductInCart(), Is.GreaterThan(0), "Product is not added to cart");
                configureProductsPage.clickOnViewCartButton();
                cartPage.waitForCartPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Cart Page is Loaded Successfully");
                ReportsGenerationClass.LogInfo(test, "Clicking on Abandon Cart Button");
                configureProductsPage.clickOnAbandonCart();
                configureProductsPage.clickOkButton();
                ReportsGenerationClass.LogInfo(test, "Cart Abandoned Successfully");
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed");
            }

        }

        [Test(Author = "Jay Godhani", Description = "TC_1006 : Delete a Product in Cart")]
        public void DeleteProduct()
        {
            test = extent.CreateTest("Delete a Product in Cart");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Delete a Product in Cart");

            try
            {
                string configJson = null;
                string requestUrl = null;
                string requestMethod = null;
                string requestId = null;

                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");
                var deleteProductTestData = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["DeleteProductTestData"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();
                originalWindowHandle = driver.CurrentWindowHandle;

                // Switch to the new tab
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                IDevTools devTools = driver as IDevTools;
                var session = devTools.GetDevToolsSession();
                var domain = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
                domain.Network.Enable(new OpenQA.Selenium.DevTools.V127.Network.EnableCommandSettings());

                domain.Network.RequestWillBeSent += (sender, e) =>
                {
                    if (e.Request.Url.Contains("/api/cart/v1/carts"))
                    {
                        requestId = e.RequestId;
                        Console.WriteLine($"Request URL: {requestUrl}");
                        Console.WriteLine($"Request Method: {requestMethod}");
                    }

                };

                domain.Network.ResponseReceived += (sender, e) =>
                {
                    if (e.Response.Url.Contains("/api/cart/v1/carts") && e.Response.Status.Equals(202))
                    {

                        // Fetch the response body asynchronously
                        var bodyResponse = domain.Network.GetResponseBody(new OpenQA.Selenium.DevTools.V127.Network.GetResponseBodyCommandSettings
                        {
                            RequestId = requestId
                        }).Result;
                        configJson = bodyResponse.Body;
                    }
                };

                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding Products To Cart");
                configureProductsPage.addProductsToCart(deleteProductTestData["Products"]);
                ReportsGenerationClass.LogInfo(test, "Added Products to Cart Successfully");
                configureProductsPage.clickOnShoppingCart();
                Assert.That(configureProductsPage.checkNumberOfProductInCart(), Is.GreaterThan(0), "Product is not added to cart");
                configureProductsPage.clickOnViewCartButton();
                cartPage.waitForCartPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Cart Page is Loaded Successfully");
                ReportsGenerationClass.LogInfo(test, "Clicking on the Checkbox To Delete Product From Cart");
                cartPage.clickOnCheckBox(deleteProductTestData["DeleteProducts"]);
                configureProductsPage.clickOnDeleteIcon();
                ReportsGenerationClass.LogInfo(test, "Product Deleted Successfully From Cart");
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed.");
            }

        }

        [Test(Author = "Jay Godhani", Description = "TC_1007 : Finalize cart")]
        public void FinalizeCart()
        {
            test = extent.CreateTest("Finalizing Cart");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Finalizing cart");

            try
            {
                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");
                var finalizeCartTestData = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["FinalizeCartTestData"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.GenerateRandom();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();
                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                IDevTools devTools = driver as IDevTools;
                var session = devTools.GetDevToolsSession();
                var domain = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
                domain.Network.Enable(new OpenQA.Selenium.DevTools.V127.Network.EnableCommandSettings());

                domain.Network.ResponseReceived += (sender, e) =>
                {
                    if (e.Response.Url.Contains("/status?includeConfigResponse=true"))
                    {
                        Console.WriteLine($"Response URL: {e.Response.Url}");
                        Console.WriteLine($"Response Status: {e.Response.Status}");
                        var bodyResponse = domain.Network.GetResponseBody(new OpenQA.Selenium.DevTools.V127.Network.GetResponseBodyCommandSettings
                        {
                            RequestId = e.RequestId
                        }).Result;
                        configJson = bodyResponse.Body;
                    }
                };
                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding Products To Cart");
                configureProductsPage.addProductsToCart(finalizeCartTestData["Products"]);
                ReportsGenerationClass.LogInfo(test, "Added Products To Cart Successfully");
                configureProductsPage.clickOnShoppingCart();
                Assert.That(configureProductsPage.checkNumberOfProductInCart(), Is.GreaterThan(0), "Product is not Added to Cart");
                configureProductsPage.clickOnViewCartButton();
                cartPage.waitForCartPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Cart page is loaded successfully");
                ReportsGenerationClass.LogInfo(test, "Clicking on finalize cart button");
                cartPage.clickOnFinalize();
                driver.Navigate().GoToUrl(ProposalDetailsPageUrl);
                proposalDetailsPage.clickOnRelatedTab();
                proposalDetailsPage.waitUntilClickConfigure();


                //proposalDetailsPage.clickOnConfigureIcon();
                ReportsGenerationClass.LogInfo(test, "Cart Finalized Successfully");
                Assert.That("Finalized".Equals(proposalDetailsPage.CheckFinalize()), "Cart Is Not Finalized");
             }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed.");
             }

        }

        [Test(Author = "Jay Godhani", Description = "TC_1008 : Create a Favorite in cart")]
        public async Task CreateFavoriteProduct()
        {
            test = extent.CreateTest("Create a Favorite in cart");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Create a Favorite Product in Cart");

            try
            {
                string configJson = null;
                var tcs = new TaskCompletionSource<string>();

                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");
                var createFavoriteTestData = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["CreateFavoriteTestData"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();
                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                IDevTools devTools = driver as IDevTools;
                var session = devTools.GetDevToolsSession();
                var domain = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
                domain.Network.Enable(new OpenQA.Selenium.DevTools.V127.Network.EnableCommandSettings());

                domain.Network.ResponseReceived += async (sender, e) =>
                {
                    if (e.Response.Url.Contains("/catalog/v1/favorites") && e.Response.Status.Equals(201))
                    {
                        Console.WriteLine($"Response URL: {e.Response.Url}");
                        Console.WriteLine($"Response Status: {e.Response.Status}");
                        var bodyResponse = await domain.Network.GetResponseBody(new OpenQA.Selenium.DevTools.V127.Network.GetResponseBodyCommandSettings
                        {
                            RequestId = e.RequestId
                        });
                        configJson = bodyResponse.Body;
                        tcs.SetResult(configJson);
                    }
                };
                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding Products To Cart");
                configureProductsPage.addProductsToCart(createFavoriteTestData["Products"]);
                ReportsGenerationClass.LogInfo(test, "Added products to cart successfully");
                configureProductsPage.clickOnShoppingCart();
                Assert.That(configureProductsPage.checkNumberOfProductInCart(), Is.GreaterThan(0), "Product is not added to cart");
                configureProductsPage.clickOnViewCartButton();
                cartPage.waitForCartPageToLoad();
                cartPage.waitForCartPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Cart page is loaded successfully");
                ReportsGenerationClass.LogInfo(test, "Selecting all Products and Creating a Favorite");
                configureProductsPage.clickOnSelectAllProduct();
                favoritePage.clickOnFavorite();

                string baseString = createFavoriteTestData["favoriteName"].ToString();
                string currentDateTime = DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss");
                string resultString = $"{baseString}_{currentDateTime}";

                favoritePage.enterFavoriteName(resultString);
                favoritePage.clickOnSaveBtn();
                ReportsGenerationClass.LogInfo(test, "Favorite created successfully with name: " + resultString);

                string responseJson = await tcs.Task;

                if (!string.IsNullOrEmpty(responseJson))
                {
                    var parsedJson = JObject.Parse(responseJson);
                    Assert.That(resultString.Equals(parsedJson["Name"].ToString()), "Favorite Name is NOt Matching");
                }
                else
                {
                    Assert.Fail("configJson was not received.");
                }
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed.");
            }

        }

        }

        [Test(Author = "Jay Godhani", Description = "TC_1007 : Mass Update in cart")]
        public void MassUpdate()
        {
            test = extent.CreateTest("Mass Update in cart");
            ReportsGenerationClass.LogInfo(test, "Mass Update in cart");

            try
            {
                string configJson = null;
                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");
                var massUpdateTestData = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["MassUpdateTestData"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to create a new proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal created successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();

                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding products to cart");
                configureProductsPage.addProductsToCart(massUpdateTestData["Products"]);
                ReportsGenerationClass.LogInfo(test, "Added products to cart successfully");
                configureProductsPage.clickOnShoppingCart();
                Assert.That(configureProductsPage.checkNumberOfProductInCart(), Is.GreaterThan(0), "Product is not added to cart");
                configureProductsPage.clickOnViewCartButton();
                cartPage.waitForCartPageToLoad();
                IDevTools devTools = driver as IDevTools;
                var session = devTools.GetDevToolsSession();
                var domain = session.GetVersionSpecificDomains<DevToolsSessionDomains>();
                domain.Network.Enable(new OpenQA.Selenium.DevTools.V127.Network.EnableCommandSettings());

                domain.Network.ResponseReceived += (sender, e) =>
                {
                    if (e.Response.Url.Contains("/status?includeConfigResponse=true&includes=price-breakups&includes=usage-tiers&includes=summary-groups&includes=adjustments&includes=line-items&includes=line-items.pricelist&includes=line-items.product&includes=line-items.pricelistitem&includes=line-items.option&primaryLineNumbers=1,2") && e.Response.Status.Equals(200))
                    {
                        Console.WriteLine($"Response URL: {e.Response.Url}");
                        Console.WriteLine($"Response Status: {e.Response.Status}");
                        var bodyResponse = domain.Network.GetResponseBody(new OpenQA.Selenium.DevTools.V127.Network.GetResponseBodyCommandSettings
                        {
                            RequestId = e.RequestId
                        }).Result;
                        configJson = bodyResponse.Body;
                    }
                };
                ReportsGenerationClass.LogInfo(test, "Cart page is loaded successfully");
                ReportsGenerationClass.LogInfo(test, "Selecting all products to mass update");
                configureProductsPage.clickOnSelectAllProduct();
                cartPage.clickOnMassUpdate();
                cartPage.updateQuantityInMassUpdate();
                cartPage.clickOnApplyButton();
                cartPage.waitForProgressBarToComplete();
                cartPage.waitForUpdatingCart();
                cartPage.waitForProgressBarToComplete();

                cartPage.waitForUpdatingCart();
                ReportsGenerationClass.LogInfo(test, "Mass Updated is Completed Successfully");
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed.");
            }
 
        }

        [Test(Author = "Alay Patel", Description = "TC_1002 : Add Product to Cart From Favorite")]
        public void AddProductToCartFromFavorite()
        {
            test = extent.CreateTest("Add Product To cart");
            ReportsGenerationClass.LogInfo(test, "Starting Test - Add Product To Cart from Favorite");

            try
            {
                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");

                var addProductFromFavoriteTestData = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["addProductFromFavoriteTestData"];

                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();

                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                configureProductsPage.waitForCatalogPageToLoad();
                favoritePage.addProductFromFavorite(addProductFromFavoriteTestData["favoriteName"].ToString());
                configureProductsPage.clickOnShoppingCart();
                Assert.That(configureProductsPage.checkNumberOfProductInCart(), Is.GreaterThan(0), "Product is not added to cart");
                configureProductsPage.clickOnViewCartButton();
                cartPage.waitForCartPageToLoad();
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed.");
            }

        }

        }

        [Test(Author = "Alay Patel", Description = "TC_1002 : Copy Product From Cart")]
        public void cloneProductFromCart()
        {
            try
            {
                test = extent.CreateTest("Add Product To cart");
                ReportsGenerationClass.LogInfo(test, "Starting Test - Add Product To cart");

                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");
                var cloneProductTestData = (JObject.Parse(File.ReadAllText(TestDatafilePath)))["cloneProductTestData"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to Create a New Proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal Created Successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();

                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding Products To Cart");
                configureProductsPage.addProductsToCart(cloneProductTestData["Products"]);
                ReportsGenerationClass.LogInfo(test, "Added Products To Cart Successfully");
                configureProductsPage.clickOnShoppingCart();
                Assert.That(configureProductsPage.checkNumberOfProductInCart(), Is.GreaterThan(0), "Product is not Added To Cart");
                configureProductsPage.clickOnViewCartButton();
                cartPage.waitForCartPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Cart Page is Loaded Successfully");
                ReportsGenerationClass.LogInfo(test, "Selecting Products To Clone operation");
                cartPage.clickOnCheckBox(cloneProductTestData["cloneProduct"]);
                cartPage.clickOnCloneBtn();
                ReportsGenerationClass.LogInfo(test, "Product Cloned Successfully");
                cartPage.checkAssertionItemCloneOrNot(cloneProductTestData["cloneProduct"]);
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed.");
            }

        }
        [Test(Author = "Alay Patel", Description = "TC_1002 : Add Contrait rule Bundle Product")]
        public void AddConstraitBudleProductToCart()
        {
            try
            {
                test = extent.CreateTest("Add Bundle Product To cart");
                ReportsGenerationClass.LogInfo(test, "Starting Test - Add Product To cart");

                string configJson = null;

                jsonFilePath = Path.Combine(filePath, "Proposal", "Proposal.json");
                var testData = (JObject.Parse(File.ReadAllText(jsonFilePath)))["Verify_TC_1001"];
                string TestDatafilePath = Path.Combine(solutionDirectory, "Main", "TestData/TestData.json");

                var productData = JObject.Parse(File.ReadAllText(TestDatafilePath))["bundleProductsConstraintRule"];
                ReportsGenerationClass.LogInfo(test, "Clicking on New Button to create a new proposal");
                proposalListPage.clickOnNewButton();
                string newProposalName = testData["ProposalName"].ToString() + createNewProposal.currentDateAndTime();
                createNewProposal
                    .waitTillSelectRecordTypePopupisLoaded()
                    .clickOnMenuItemFromRecordTypeDropDown(testData["RecordType"].ToString())
                    .clickOnNextButton()
                    .waitTillCreateProposalPopupisLoaded()
                    .enterProposalName(newProposalName)
                    .selectOpportunity(testData["Opportunity"].ToString())
                    .selectAccount(testData["Account"].ToString())
                    .selectPriceList(testData["PriceList"].ToString())
                    .clickOnSaveButton();
                ReportsGenerationClass.LogInfo(test, "Proposal created successfully");
                proposalDetailsPage.waitTillProposalDetailsPageisLoaded();
                this.ProposalId = proposalDetailsPage.getProposalObjectId();
                this.ProposalDetailsPageUrl = urlGenerator.GenerateProposalDetailsPageURL(proposalDetailsPage.getProposalObjectId());
                proposalDetailsPage.clickOnConfigureProductsForRLS();

                // Switch to the new tab
                originalWindowHandle = driver.CurrentWindowHandle;
                proposalDetailsPage.switchToNewWindow(originalWindowHandle);

                configureProductsPage.waitForCatalogPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Adding Bundle Products To Cart");
                configureProductsPage.addBundleProduct(productData, configureProductsPage);
                configureProductsPage.clickOnGoTopricing();
                ReportsGenerationClass.LogInfo(test, "Added Bundle Products To Cart Successfully");
                cartPage.waitForCartPageToLoad();
                ReportsGenerationClass.LogInfo(test, "Cart Page is Loaded Successfully");
                ReportsGenerationClass.LogInfo(test, "Checking Option Products Added or Not");
                cartPage.expandCaretIcon(productData);
                cartPage.AssertConstraintRuleOptionProducts(productData);
            }
            catch (Exception ex)
            {
                ReportsGenerationClass.LogFail(test, $"Test failed with exception: {ex.Message}");
                throw new Exception($"Test failed with exception: {ex.Message}");
            }
            finally
            {
                driver.Close();
                driver.SwitchTo().Window(originalWindowHandle);
                ReportsGenerationClass.LogInfo(test, "Task Completed.");
            }

        }
        [TearDown]
        public void tearDown()
        {
            ReportsGenerationClass.EndTest(test);
            if (!this.ProposalDetailsPageUrl.Contains("{proposalId}"))
            {
                if (driver.Url.Equals(this.ProposalDetailsPageUrl))
                    proposalDetailsPage.deleteCurrentProposal();
                else
                {
                    driver.Navigate().GoToUrl(this.ProposalDetailsPageUrl);
                    proposalDetailsPage.deleteCurrentProposal();
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
            ReportsGenerationClass.EndReporting(extent);
        }
    }
}

