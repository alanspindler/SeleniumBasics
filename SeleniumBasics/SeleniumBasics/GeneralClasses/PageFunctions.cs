using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace SeleniumBasics.GeneralClasses
{
    class PageFunctions
    {
        public RemoteWebDriver driver;
        public PageFunctions(RemoteWebDriver driver)
        {
            this.driver = driver;
        }

        //Highlight the element with a specific color
        public static object HighlightElement(IWebDriver driver, By element)
        {
            string WeHighlightedColour = "arguments[0].style.border='5px solid red'";
            var myLocator = driver.FindElement(element);
            var js = (IJavaScriptExecutor)driver;
            return js.ExecuteScript(WeHighlightedColour, myLocator);
        }

        /// <summary>
        /// Wait for the element to be present
        /// </summary>
        /// <remarks>Written by Alan Spindler at 23/11/2015</remarks>
        public void WaitForElement(By element)
        {
           
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(12));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(element));
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Click on a element in the page, like buttons, fields, combobox, lists
        /// </summary>
        /// <remarks>Written by Alan Spindler at 22/01/2016</remarks>
        public void ClickElement(By element)
        {
            WaitForElement(element);
            HighlightElement(driver, element);
            driver.FindElement(element).Click();
        }

        /// <summary>
        /// DoubleClick in a element in the page, like buttons, fields, combobox, lists
        /// </summary>
        /// <remarks>>Written by Alan Spindler at 19/10/2016</remarks>
        public void DoubleClickElement(By element)
        {
            WaitForLoadingOverlay();
            WaitForElement(element);
            Actions action = new Actions(driver);
            action.DoubleClick(driver.FindElement(element)).DoubleClick();
            action.Build().Perform();
        }

        /// <summary>
        /// Fill a text field with some text. In case is called with a empty text, cleans the current text from the field.        
        /// </summary>
        /// <remarks>Written by Alan Spindler at 23/11/2015</remarks>
        public void FillField(By element, string Text)
        {
            HighlightElement(driver, element);
            if (Text == "")
            {
                InsertText(element, Keys.Control + "a");
                InsertText(element, Keys.Delete);
                InsertText(element, Keys.Tab);                
            }
            else
            {
                driver.FindElement(element).Clear();
                InsertText(element, Text);
            }
        }

        /// <summary>
        /// Insert text on a field
        /// </summary>
        /// <remarks>Written by Alan Spindler at 22/01/2016</remarks>
        public void InsertText(By element, string key)
        {
            WaitForElement(element);
            driver.FindElement(element).SendKeys(key);
        }

        /// <summary>
        /// Wait for a Loading Overlay to go away before resuming an action
        /// </summary>
        /// <remarks>>Written by Alan Spindler at 18/04/2016</remarks>
        public void WaitForLoadingOverlay()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(500));
            By LoadingOverlay = By.Id("loading");
            //Checks if the overlay is visible
            var IsOverlayVisible = false;
            try
            {               
               wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(LoadingOverlay));
               IsOverlayVisible = true;
            }
            catch
            {
            }

            //While the overlay is visible, keeps on loop until it goes away.
            while (IsOverlayVisible)
            {
                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(300));
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(LoadingOverlay));
                }
                catch
                {
                    IsOverlayVisible = false;
                }
            }
            Thread.Sleep(500);
        }


        /// Assert the value of a text field
        /// </summary>
        /// <remarks>Written by Alan Spindler at 23/11/2015</remarks>
        public void AssertFieldValue(string text, By field)
        {
            WaitForLoadingOverlay();
            var value = driver.FindElement(field).GetAttribute("value");
            Assert.AreEqual(text, value);
        }

        //Wait for a combobox to load a specific value
        public void WaitValueCombobox(By element, string text)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(25000));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElementLocated(element, text));
        }


        /// <summary>
        /// Assert the text from a element
        /// </summary>
        /// <remarks>Written by Alan Spindler at 04/03/2016</remarks>
        public void AssertText(string desiredText, By field)
            {
                WaitForElement(field);
                WaitForLoadingOverlay();
                var elementText = driver.FindElement(field).Text;
                Assert.AreEqual(desiredText, elementText);
            }


        /// <summary>
        /// Check or uncheck checkbox
        /// </summary>
        /// <remarks>Written by Alan Spindler at 24/06/2016</remarks>
        public void MarcarCheckbox(By elementToValidate, By elementToClick, bool value)
        {
            WaitForElement(elementToValidate);
            HighlightElement(driver, elementToClick);
            var elementChecked= driver.FindElement(elementToValidate).GetAttribute("checked");
            //If it needs to CHECK the checkbox, see if it is UNCHECKED and clicks. Otherwise, doesn't do anything.            
            if (value)
            {
                if (elementChecked != "true")
                {
                    driver.FindElement(elementToClick).Click();
                }
            }
            //If it needs to UNCHECK the checkbox, see if it is CHECKED and clicks. Otherwise, doesn't do anything.                        
            else
            {
                if (elementChecked == "true")
                {
                    driver.FindElement(elementToClick).Click();
                }
            }
        }

        /// <summary>
        /// Select a item in a combobox
        /// </summary>
        /// <remarks>Written by Alan Spindler at 27/06/2016</remarks>
        public void SelectComboItem(By element, string text)
        {
            WaitForElement(element);
            WaitValueCombobox(element, text);
            HighlightElement(driver, element);
            var selectedElement = driver.FindElement(element);
            var selectElement = new SelectElement(selectedElement);
            selectElement.SelectByText(text);
        }

        /// <summary>
        /// Assert if Checkbox or Option is selected or not
        /// </summary>
        /// <remarks>Written by Alan Spindler at 03/03/2016</remarks>
        public void AssertCheckboxorOption(By element, bool isChecked)
        {
            WaitForElement(element);
            var checkbox = driver.FindElement(element).Selected;

            if (isChecked)
            {
                Assert.AreEqual(true, checkbox);
            }
        }

        /// <summary>
        /// Este método verifica se o elemento da página está visível
        /// </summary>
        /// <remarks>Written by Alan Spindler at 25/01/2016</remarks>
        public static bool IsElementDisplayed(IWebDriver driver, By element)
        {
            if (driver.FindElements(element).Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Assert element displayed
        /// </summary>
        /// <remarks>Escrita por Alan Spindler em 03/03/2016</remarks>
        public void AssertElementDisplayed(By element)
        {
            Assert.AreEqual(true, IsElementDisplayed(driver, element));
        }
    }
    }

