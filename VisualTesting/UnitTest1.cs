using System;
using Applitools;
using Applitools.Selenium;
using Applitools.VisualGrid;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Configuration = Applitools.Selenium.Configuration;

namespace VisualTesting
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod2()
        {
            String shouldBreakSiteStr = Environment.GetEnvironmentVariable("INJECT_BUG");

            bool shouldBreakSite = false;

            if (shouldBreakSiteStr != null && shouldBreakSiteStr.Length > 0)
                shouldBreakSite = bool.Parse(shouldBreakSiteStr);

            String testName = "Functional VS Visual";

            VisualGridRunner runner = new VisualGridRunner(10);
            Eyes eyes = new Eyes(runner);
            ChromeDriver driver = new ChromeDriver();

            Configuration sconf = eyes.GetConfiguration();

            sconf.SetAppName(testName);

            sconf.SetTestName(testName);

            eyes.ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_API_KEY");

            var batchName = Environment.GetEnvironmentVariable("APPLITOOLS_BATCH_NAME");
            var batchId = Environment.GetEnvironmentVariable("APPLITOOLS_BATCH_ID");

            BatchInfo batchInfo = new Applitools.BatchInfo(batchName);
            batchInfo.Id = batchId;

            sconf.SetBatch(batchInfo);

            sconf.AddBrowser(1200, 800, BrowserType.CHROME);
            sconf.AddBrowser(1200, 800, BrowserType.FIREFOX);
            sconf.AddBrowser(1200, 800, BrowserType.SAFARI);
            sconf.AddBrowser(1200, 800, BrowserType.IE_11);
            sconf.AddBrowser(1200, 800, BrowserType.EDGE);


            //sconf.AddDeviceEmulation(DeviceName.iPad, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.iPad_Pro, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.iPhone_6_7_8_Plus, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.iPhone_X, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.Galaxy_Note_3, ScreenOrientation.Portrait);
            //sconf.AddDeviceEmulation(DeviceName.Nexus_10, ScreenOrientation.Portrait);

            eyes.SetLogHandler(new FileLogHandler(@"C:\Users\user\Desktop\appli.log", false , true));

            sconf.SetViewportSize(1000, 600);

            eyes.SetConfiguration(sconf);

            Console.WriteLine("Open conection to Eyes");
            eyes.Open(driver);

            driver.Url = "https://github.com/login";

            Console.WriteLine("Visual Assertion #1");
            eyes.Check(Target.Window().Fully().WithName("Login page"));

            Console.WriteLine("Click Login");
            driver.FindElement(By.CssSelector("#login > form > div.auth-form-body.mt-3 > input.btn.btn-primary.btn-block"))
                    .Click();

            if (shouldBreakSite)
            {
                Console.WriteLine("Breaking Site");
                BreakSite(driver);
            }

            Console.WriteLine("Begin Functional Assertions");

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

            Console.WriteLine("Completed Functional Assertions");

            Console.WriteLine("Visual Assertion #2");
            eyes.Check(Target.Window().Fully().WithName("Error Message"));

            Console.WriteLine("Close connection to Eyes");
            eyes.CloseAsync();

            System.Diagnostics.Debug.WriteLine("Waiting for visual test to complete");
            TestResultsSummary results = runner.GetAllTestResults();
            System.Diagnostics.Debug.WriteLine(results);

            driver.Quit();
        }

        private static void BreakSite(ChromeDriver driver)
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
