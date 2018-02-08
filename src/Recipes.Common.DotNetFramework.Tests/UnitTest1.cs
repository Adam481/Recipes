using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Recipes.Common.DotNetFramework.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var test = "Tsst";
            var config = MySection.GetConfig();
        }
    }
}
