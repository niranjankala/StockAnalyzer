﻿using StockAnalyzer.Services;
// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace StockAnalyzer.Services.Tests
{
    [TestFixture]
    public class StockServiceTests
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            var answer = 42;
            Assert.That(answer, Is.EqualTo(42), "Some useful error message");
        }

        [Test()]
        public void StockServiceTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetStockTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetStockTickerSymbolsTest()
        {
            Assert.Fail();
        }
    }
}
