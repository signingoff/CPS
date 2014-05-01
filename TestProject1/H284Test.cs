using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApplication1;

namespace TestProject1
{
    [TestClass]
    public class H284Test
    {
        [TestMethod]
        public void Empty()
        {
            H284Name name = new H284Name();
            Assert.IsFalse(name.Parser(string.Empty).Succeeded);
        }

        [TestMethod]
        public void _U()
        {
            H284Name name = new H284Name();
            Assert.IsFalse(name.Parser("_U").Succeeded);
        }

        [TestMethod]
        public void TwoU()
        {
            H284Name name = new H284Name();
            Assert.IsFalse(name.Parser("2U").Succeeded);
        }

        [TestMethod]
        public void U()
        {
            var result = new H284Name().Parser("U");
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Recognized, "U");
            Assert.AreEqual(result.Remaining, string.Empty);

            result = new H284Name().Parser("U{");
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Recognized, "U");
            Assert.AreEqual(result.Remaining, "{");

            result = new H284Name().Parser("U2{");
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Recognized, "U2");
            Assert.AreEqual(result.Remaining, "{");

            result = new H284Name().Parser("U_{");
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Recognized, "U_");
            Assert.AreEqual(result.Remaining, "{");

            result = new H284Name().Parser("U123_{");
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Recognized, "U123_");
            Assert.AreEqual(result.Remaining, "{");

            result = new H284Name().Parser("USER001");
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Recognized, "USER001");
            Assert.AreEqual(result.Remaining, string.Empty);

            result = new H284Name().Parser("USER001{");
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Recognized, "USER001");
            Assert.AreEqual(result.Remaining, "{");

            result = new H284Name().Parser("a0123456789012345678901234567890123456789012345678901234567890123456789");
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Recognized,
                "a0123456789012345678901234567890123456789012345678901234567890123");
            Assert.AreEqual(result.Remaining, "456789");
        }
    }
}
