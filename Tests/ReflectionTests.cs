using System;
using System.Reflection;
using DemoApp;
using NUnit.Framework;
using QuickTest;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Tests
{
    public class ReflectionTests : QuickTest<App>
    {

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            Launch(new App());
        }

        [Test]
        public void ScanForSendMethods()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
            foreach (var type in typeof(Page).Assembly.GetTypes())
                foreach (var method in type.GetMethods(flags))
                    if (method.Name.Contains("Send") || method.Name.Contains("Notify"))
                        Console.WriteLine(type.Name + ": " + method.Name);
        }
    }
}