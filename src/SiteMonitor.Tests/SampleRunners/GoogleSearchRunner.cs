using System.Threading;
using OpenQA.Selenium;
using SiteMonitor.Runner;

namespace SiteMonitor.Tests.SampleRunners
{
    public class GoogleSearchRunner : BaseRunner
    {
        protected override bool Run(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://www.google.com");

            var searchBox = driver.FindElement(By.Name("q"));
            searchBox.Clear();
            searchBox.SendKeys("google");

            var title = driver.Title;

            return title == "google - Pesquisa Google";
        }
    }
}
