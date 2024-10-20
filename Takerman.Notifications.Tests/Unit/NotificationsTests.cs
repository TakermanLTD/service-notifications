using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Unit
{
    public class NotificationsTests(ITestOutputHelper testOutputHelper, TestFixture fixture) : TestBed<TestFixture>(testOutputHelper, fixture)
    {
        [Fact]
        public void Should_ReturnTrue_When_TheTestIsCalled()
        {
            Assert.True(true);
        }
    }
}