using System;
using Applitools;
using Applitools.Selenium;
using Applitools.Utils.Geometry;
using Applitools.VisualGrid;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using Configuration = Applitools.Selenium.Configuration;
using ScreenOrientation = Applitools.VisualGrid.ScreenOrientation;

namespace VisualTesting
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod2()
        {
            Eyes eyes;
            InternetExplorerDriver driver = new InternetExplorerDriver();
            
            VisualGridRunner runner = null;

            String shouldBreakSiteStr = Environment.GetEnvironmentVariable("INJECT_BUG");

            bool shouldBreakSite = false;

            if(shouldBreakSiteStr != null && shouldBreakSiteStr.Length > 0)
                shouldBreakSite = bool.Parse(shouldBreakSiteStr);

            String testName = "Functional VS Visual";

            runner = new VisualGridRunner(10);

            eyes = new Eyes(runner);

            Configuration sconf = eyes.GetConfiguration();

            sconf.SetAppName(testName);

            sconf.SetTestName(testName);
            
            eyes.ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_API_KEY");

            sconf.SetBatch(new BatchInfo(testName));

            sconf.AddBrowser(1200, 800, BrowserType.CHROME);
            sconf.AddBrowser(1200, 800, BrowserType.FIREFOX);
            sconf.AddBrowser(1200, 800, BrowserType.IE_11);
            sconf.AddBrowser(1200, 800, BrowserType.EDGE);
            //sconf.AddBrowser(1200, 800, BrowserType.SAFARI);

            //sconf.AddDeviceEmulation(DeviceName.iPad, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.iPad_Pro, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.iPhone_6_7_8_Plus, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.iPhone_X, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.Galaxy_Note_3, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.Nexus_10, ScreenOrientation.Portrait);

            eyes.SetLogHandler(new StdoutLogHandler());
            eyes.SetConfiguration(sconf);

            eyes.Open(driver);
  
            driver.Url = "https://github.com/login";

            eyes.CheckWindow("Login Page");

            driver.FindElement(By.CssSelector("#login > form > div.auth-form-body.mt-3 > input.btn.btn-primary.btn-block"))
                    .Click();

            if (shouldBreakSite)
                BreakSite(driver);

            // validate sign in button
            String buttonText = driver
                    .FindElement(
                            By.CssSelector("#login > form > div.auth-form-body.mt-3 > input.btn.btn-primary.btn-block"))
                    .GetAttribute("value");

            Assert.IsTrue(buttonText.CompareTo("Sign in") == 0, "wrong button");

            // validate error message
            String errorMessage = driver.FindElement(By.CssSelector("#js-flash-container > div > div")).Text;

            Assert.IsTrue(errorMessage.Contains("Incorrect username or password."), "wrong label");

            String usernameTextbox = driver
                    .FindElement(By.CssSelector("#login > form > div.auth-form-body.mt-3 > label:nth-child(1)")).Text;

            Assert.IsTrue(usernameTextbox.Contains("Username or email address"), "wrong label");

            String passwordTextbox = driver
                    .FindElement(By.CssSelector("#login > form > div.auth-form-body.mt-3 > label:nth-child(3)")).Text;

            Assert.IsTrue(passwordTextbox.Contains("Password"), "wrong label");

            eyes.CheckWindow("Error Message");

            try
            {

                eyes.CloseAsync();

                Console.WriteLine(runner.GetAllTestResults(false));

            }
            finally
            {
                driver.Quit();
            }
        }

        private static void BreakSite(InternetExplorerDriver driver)
        {
            
            driver.ExecuteScript("arguments[0].setAttribute('style','color: red')",
                    driver.FindElement(By.CssSelector("#login > form > div.auth-form-body.mt-3 > label:nth-child(1)")));
            driver.ExecuteScript("arguments[0].setAttribute('style','color: pink')",
                    driver.FindElement(By.CssSelector("#login > form > div.auth-form-body.mt-3 > label:nth-child(3)")));
            driver.ExecuteScript("arguments[0].setAttribute('style','fill: green')",
                    driver.FindElement(By.CssSelector(
                            "body > div.position-relative.js-header-wrapper > div.header.header-logged-out.width-full.pt-5.pb-4 > div > a > svg")));
            driver.ExecuteScript("arguments[0].setAttribute('style','font-size: 8px')",
                    driver.FindElement(By.CssSelector("#login > p")));

        }

    }
}
