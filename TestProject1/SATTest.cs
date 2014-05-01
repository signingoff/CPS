using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApplication1;

namespace TestProject1
{
    [TestClass]
    public class SATTest
    {
        [TestMethod]
        public void IsDigit()
        {
            IsDigitSAT sat = new IsDigitSAT(new Item());
            var result = sat.Parser("12121");
            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void OR()
        {
            IsDigitSAT isDigit = new IsDigitSAT(new Item());
            IsAlphaSAT isAlpha = new IsAlphaSAT(new Item());

            OR or = new OR(isDigit, isAlpha);

            Assert.IsTrue(or.Parser("1").Succeeded);
            Assert.IsTrue(or.Parser("a").Succeeded);
            Assert.IsFalse(or.Parser("_").Succeeded);
            Assert.AreEqual(or.Parser("1").Recognized, "1");
            Assert.AreEqual(or.Parser("1").Remaining, string.Empty);
        }

        [TestMethod]
        public void SEQ()
        {
            SEQ seq = new SEQ(new IsDigitSAT(new Item()), new IsAlphaSAT(new Item()));
            Assert.IsFalse(seq.Parser("12").Succeeded);
            Assert.IsTrue(seq.Parser("1a2").Succeeded);
            Assert.AreEqual(seq.Parser("1a2").Recognized, "1a");
            Assert.AreEqual(seq.Parser("1a2").Remaining, "2");
        }

        [TestMethod]
        public void OneOrMany()
        {
            OneOrMany oneOrMany = new OneOrMany(new IsDigitSAT(new Item()), 4);
            Assert.IsFalse(oneOrMany.Parser("aa").Succeeded);
            Assert.IsTrue(oneOrMany.Parser("1234").Succeeded);
            Assert.AreEqual(oneOrMany.Parser("1234a").Recognized, "1234");
            Assert.AreEqual(oneOrMany.Parser("1234a").Remaining, "a");

            oneOrMany = new OneOrMany(new IsDigitSAT(new Item()), 2);
            Assert.IsTrue(oneOrMany.Parser("1aaa").Succeeded);
            Assert.AreEqual(oneOrMany.Parser("1aaa").Recognized, "1");
            Assert.AreEqual(oneOrMany.Parser("1aaa").Remaining, "aaa");
        }
    }
}
