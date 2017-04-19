using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;


/// <summary>
/// Class that has basics driver and browse functions, like initialize driver, open a browser, maximize, etc.
/// </summary>
/// <remarks>Written by Alan Spindler in 18/04/2017</remarks>
namespace SeleniumBasics.GeneralClasses
{
    class Browser
    {
                
        #region public variables

        public RemoteWebDriver driver;        
        //Starter URL    
        public const string BaseUrl = "www.google.com";
        public static string relativePath = System.IO.Directory.GetCurrentDirectory();

        #endregion

        /// <summary>
        /// Initialize the Driver and the Chrome Browser 
        /// <remarks>Written by Alan Spindler in 10/04/2017/remarks>
        private void InitializeChrome()
        {                  
            driver = new ChromeDriver(relativePath);            
        }

        /// <summary>
        /// Initialize the Driver and the FirefoxBrowser 
        /// <remarks>Written by Alan Spindler in 10/04/2017/remarks>
        private void InitializeFirefox()
        {
            driver = new FirefoxDriver();
        }

        /// <summary>
        /// Maximize the Browser
        /// <remarks>Written by Alan Spindler in 10/04/2017/remarks>
        private void MaximizeBrowser()
        {
            driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Close the driver and the Browser
        /// </summary>
        /// <remarks>Written by Alan Spindler in 10/04/2017/remarks>
        public void QuitlDriver()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }         
        }

    }
}
