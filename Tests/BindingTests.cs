using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class BindingTests : QuickTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();
            Launch(new App());
            ShouldSee("This is a test for the CI");
            OpenMenu("Binding");
        }

        [Test]
        public void TestBinding()
        {
            ShouldSee("updated bound text");
        }
    }
}
