
using AutomationProject1.Main.resources;
using AutomationProject1.Main.UIElements;
using AutomationProject1.Main.utils;
using gehc_cpq_ui_master.Main.pages.Proposal;
using gehc_cpq_ui_master.Main.utils;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;

namespace AutomationProject1.Main.pages
{
    public class LoginPage : StartupPage
    {
        LoginPageElements loginPageElements;
        URLGenerator URLGenerator;
        public LoginPage(IWebDriver driver, URLGenerator URLGenerator) : base(driver)
        {
            //adminHelper = new AdminHelper(driver);
            PageFactory.InitElements(driver, this);
            this.URLGenerator = URLGenerator;
        }

        private void loginToSalesforce(String username, String password)
        {
            loginPageElements.txtUserName.SendKeys(username);
            loginPageElements.txtPassword.SendKeys(password);
            loginPageElements.btnLogin.Click();
        }
        public ProposalListPage login()
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(URLGenerator.proposalListPageURL);
            // selectIDPAndSignIn(Config.idpName);
            switch (Config.idpName)
            {
                case "Salesforce":
                case "Salesforce Sandbox":
                    loginPageElements = new LoginPageElements(driver);
                    loginToSalesforce(Config.loginUser, Config.loginPassword);
                    break;
                case "Microsoft":
                    //loginToMicrosoft();
                    break;
                case "Google":
                    // loginToGoogle();
                    break;
                case "Dropbox":
                    //loginToDropbox();
                    break;
                case "Box":
                    // loginToBox();
                    break;
                case "Conga Idp":
                    //loginToCongaIdp();
                    break;
            }
            return new ProposalListPage(driver);
        }
    }
}
