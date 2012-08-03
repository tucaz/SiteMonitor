using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using SiteMonitor.Core.Runner;

namespace SiteMonitor.Core
{
    public class Compra : BaseRunner
    {
        private string baseURL = "http://www.fnac.com.br";

        public override string Title
        {
            get
            {
                return "Script de compra de um produto com boleto";
            }
        }

        public override string YAxisLegend
        {
            get
            {
                return "Tempo (segundos)";
            }
        }

        protected override bool Run(IWebDriver driver)
        {
            var testSuccessful = false;

            Log("testSuccessful: " + testSuccessful.ToString());

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(30));
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));

            try
            {
                driver.Navigate().GoToUrl(baseURL + "/index.html");
                driver.FindElement(By.CssSelector("dd.foto > a > img")).Click();
                driver.FindElement(By.LinkText("comprar")).Click();

                //Se for um produto que tenha serviço, não seleciona nenhum [serviço] e vai embora
                if (driver.Url.IndexOf("DetalheServico.aspx") > -1)
                {
                    Log("É página de serviço");

                    Log("Checkbox ckbLiConcordoGarantia:" + driver.IsElementPresent(By.Id("ckbLiConcordoGarantia")).ToString());
                    driver.FindElement(By.Id("ckbLiConcordoGarantia")).Click();

                    Log("Checkbox ckbLiConcordoGarantia:" + driver.IsElementPresent(By.ClassName("btAvancar")).ToString());
                    driver.FindElement(By.ClassName("btAvancar")).Click();
                }

                driver.FindElement(By.CssSelector("img.bt_limpar")).Click();
                driver.FindElement(By.Id("txtEmail")).Clear();
                driver.FindElement(By.Id("txtEmail")).SendKeys("fn@c.com");
                driver.FindElement(By.Id("txtSenha")).Clear();
                driver.FindElement(By.Id("txtSenha")).SendKeys("321321");
                driver.FindElement(By.CssSelector("a.btOk.enviaForm > span")).Click();
                driver.FindElement(By.Id("182638")).Click();
                driver.FindElement(By.XPath("//a[contains(@rel,'boletoItau')]")).Click();
                driver.FindElement(By.CssSelector("input.txtcpfcnpjboleto")).Clear();
                driver.FindElement(By.CssSelector("input.txtcpfcnpjboleto")).SendKeys("34238009843");
                driver.FindElement(By.CssSelector("img[alt=\"Clique aqui para Finalizar a compra\"]")).Click();

                testSuccessful = driver.IsElementPresent(By.Id("nrPedido"));

                driver.FindElement(By.CssSelector("#btVoltar > img")).Click();
                driver.FindElement(By.LinkText("sair")).Click();

                Log("testSuccessful: " + testSuccessful.ToString());
                return testSuccessful;
            }
            catch (Exception ex)
            {
                Log("Exception -> " + ex.Message);
                return false;
            }
        }       
    }

    public static class IWebDriverExtensions
    {
        public static bool IsElementPresent(this IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
