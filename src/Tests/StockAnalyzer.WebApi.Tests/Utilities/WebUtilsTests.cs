using NUnit.Framework;
using System;
using System.Threading;

namespace StockAnalyzer.WebApi.Utilities.Tests
{
    [TestFixture]
    public class WebUtilsTests
    {
        string allowedLocales;
        [SetUp]
        public void Intialize()
        {
            allowedLocales = "pl-PL,pl;q=0.8,en-US;q=0.6,en;q=0.4";
        }
        [Test]
        public void SetUserLocale_AssignAndAssert_PolishLocale()
        {

            WebUtils.SetUserLocale("pl-PL", null, null, true, allowedLocales);
            Assert.IsNotNull(Thread.CurrentThread.CurrentCulture);
            Assert.IsNotNull(Thread.CurrentThread.CurrentUICulture);
            Assert.AreEqual("pl-PL", Thread.CurrentThread.CurrentCulture.Name);
            Assert.AreEqual("pl-PL", Thread.CurrentThread.CurrentUICulture.Name);

        }

        [Test]
        public void SetUserLocale_WithBlankLocale_AssignDefaultUSLocale()
        {

            WebUtils.SetUserLocale("", null, null, true, allowedLocales);
            Assert.IsNotNull(Thread.CurrentThread.CurrentCulture);
            Assert.IsNotNull(Thread.CurrentThread.CurrentUICulture);
            Assert.AreEqual("en-US", Thread.CurrentThread.CurrentCulture.Name);
            Assert.AreEqual("en-US", Thread.CurrentThread.CurrentUICulture.Name);
        }

        [Test]
        public void SetUserLocale_WIthInvalidLocale_AssignDefaultUSLocale()
        {
            WebUtils.SetUserLocale("TY1", null, null, true, allowedLocales);
            Assert.IsNotNull(Thread.CurrentThread.CurrentCulture);
            Assert.IsNotNull(Thread.CurrentThread.CurrentUICulture);
            Assert.AreEqual("en-US", Thread.CurrentThread.CurrentCulture.Name);
            Assert.AreEqual("en-US", Thread.CurrentThread.CurrentUICulture.Name);

        }
    }
}