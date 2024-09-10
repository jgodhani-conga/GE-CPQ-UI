using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationProject1.Main.UIElements
{
    public class LoginPageElements
    {
        public LoginPageElements(IWebDriver driver)
        {
            PageFactory.InitElements(driver,this);
        }
        public String cssIdpOption = "span[title='{args0}']";
        
        [FindsBy(How = How.CssSelector, Using = "div.sign-in-wrapper")]
        public IWebElement tagDivMain { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "cc-button")]
        public IWebElement tagCcButton  { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div.sign-in-wrapper > cc-sign-in")]
        public IWebElement tagCcSignIn  { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "cc-select")]
        public IWebElement tagCcSelect  { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "div[id='select-button']")]
        public IWebElement tagSelectIdp  { get; set; }

        [FindsBy(How = How.CssSelector, Using = "button[name='Sign In']")]
        public IWebElement btnSignIn { get; set; }

        [FindsBy(How = How.Id, Using = "username")]
        public IWebElement txtUserName { get; set; }

        [FindsBy(How = How.Id, Using = "password")]
        public IWebElement txtPassword { get; set; }

        [FindsBy(How = How.Id, Using = "Login")]
        public IWebElement btnLogin { get; set; }

        [FindsBy(How = How.Id, Using = "header")]
        public IWebElement lblOauthError { get; set; }

    }
}
