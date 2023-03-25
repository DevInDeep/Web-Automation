using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System.Threading;
using System;
using System.Collections.Generic;

namespace WebAutomation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EdgeDriver driver = null;
            try
            {
                var deviceDriver = EdgeDriverService.CreateDefaultService();
                deviceDriver.HideCommandPromptWindow = true;
                EdgeOptions options = new EdgeOptions();
                options.AddArguments("--disable-infobars");

                driver = new EdgeDriver(deviceDriver, options);
                driver.Manage().Window.Maximize();

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                driver.Navigate().GoToUrl("https://devindeep.com/");
                var inputBox = driver.FindElement(By.XPath("//*[@id=\"search-2\"]/form/label/input"));
                inputBox.SendKeys("ML.NET");
                var searchButton = driver.FindElement(By.XPath("//*[@id=\"search-2\"]/form/button[1]"));
                searchButton.Click();
                Thread.Sleep(2000);
                var searchArticles = driver.FindElements(By.TagName("article"));
                var urls = new List<string>();
                for(int i=0;i<searchArticles.Count;i++)
                {
                    var header = searchArticles[i].FindElement(By.XPath(".//h2/a"));
                    var hrefAttribute = header.GetAttribute("href");
                    Console.WriteLine($"{i+1}: {header.Text}");
                    urls.Add(hrefAttribute);
                }
                Console.WriteLine($"Pick article from 1 to {urls.Count}");
                var userInput = Console.ReadLine();
                if (Int32.TryParse(userInput, out int urlIndex))
                {
                    driver.Navigate().GoToUrl(urls[urlIndex-1]);
                }

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}