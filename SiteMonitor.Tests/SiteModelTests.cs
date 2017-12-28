using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteMonitor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMonitor.Tests
{
    [TestClass]
    public class SiteModelTests
    {
        [TestMethod]
        public void Should_ReturnTheSameAbsoluteURI_WhenUriPropertyIsCalculated()
        {

            var absoluteUriString = "http://google.com/";
            var site = new Site() { UriString = absoluteUriString };

            Assert.AreEqual(absoluteUriString, site.Uri.AbsoluteUri);
        }
    }
}
