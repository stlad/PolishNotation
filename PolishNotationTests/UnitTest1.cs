using NUnit.Framework;
using PolishNotation;
namespace PolishNotationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("1+2",3)]
        [TestCase("(1+2)^2", 9)]
        [TestCase("3/5+6+(4+5*6+6)", 46.6)]
        [TestCase("sqrt(25)", 5)]
        [TestCase("sqrt(abs(-25))", 5)]
        public void PolishNotationTest(string str, double res)
        {
            var exp = new Expression(str);
            exp.Calculate(false);
            Assert.AreEqual(res, exp.Result);
        }
    }
}