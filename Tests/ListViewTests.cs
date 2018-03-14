﻿using System.Linq;
using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class ListViewTests : IntegrationTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            OpenMenu("ListViews");
            ShouldSee("ListView demo");
        }

        [Test]
        public void TestTextCell()
        {
            Assert.That(Find("Item A1").First(), Is.Not.Null);
            Tap("Item A1");
            ShouldSee("Item A1 tapped");
        }

        [Test]
        public void TestStringViewCell()
        {
            Assert.That(Find("Item A2").First(), Is.Not.Null);
            Tap("Item A2");
            ShouldSee("Item A2 tapped");
        }

        [Test]
        public void TestItemViewCell()
        {
            Assert.That(Find("Item A3").First(), Is.Not.Null);
            Tap("Item A3");
            ShouldSee("Item A3 tapped");
        }

        [Test]
        public void TestGroups()
        {
            Tap("A4");
            ShouldSee("A4 tapped");

            Tap("Ok");
            Tap("A5");
            ShouldSee("A5 tapped");
        }

        protected override void TearDown()
        {
            Tap("Ok");
            ShouldSee("ListView demo");

            base.TearDown();
        }
    }
}
