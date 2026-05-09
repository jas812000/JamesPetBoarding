using NUnit.Framework;

namespace JamesPetBoardingTests
{
    [TestFixture]
    public class BaseTests
    {
        [Test]
        public void BaseTest()
        {
            Assert.That(1 == 1, "Pass condition");
        }
    }
}