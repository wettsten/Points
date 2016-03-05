using NUnit.Framework;
using Shouldly;
using Subject = Points.Common.Extensions.Extensions;

namespace Points.Api.UnitTests.Common.Extensions
{
    [TestFixture]
    public class SpacifyExtensionTests
    {
        [Test]
        public void TestNullAndEmpty([Values("","   ")]string input)
        {
            string result = Subject.Spacify(input);
            result.ShouldBe(string.Empty);
        }

        [Test]
        public void FirstLetterIsCapitalDoesNotGetSpace()
        {
            string test = "Test";
            string result = Subject.Spacify(test);
            result.ShouldBe("Test");
        }

        [Test]
        public void TwoWords()
        {
            string test = "TestTest";
            string result = Subject.Spacify(test);
            result.ShouldBe("Test Test");
        }

        [Test]
        public void TwoWordsWithPreExistingSpace()
        {
            string test = "Test Test";
            string result = Subject.Spacify(test);
            result.ShouldBe("Test Test");
        }
    }
}