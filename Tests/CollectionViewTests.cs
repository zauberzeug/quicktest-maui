using System;
using System.Threading.Tasks;
using DemoApp;
using Microsoft.Maui.Controls;
using QuickTest;

namespace Tests;

public class CollectionViewTests : QuickTest<App>
{
    [SetUp]
    protected override void SetUp()
    {
        base.SetUp();

        Launch(new App());
        OpenMenu("CollectionView");
        ShouldSee("CollectionView demos");
    }

    [Test]
    public void TestGroup()
    {
        Tap("DemoCollectionViewGrouped");
        ShouldSee("Mammals");
        ShouldSee("Reptiles");
        ShouldSee("Crocodile");
    }

    [Test]
    public void TestLabel()
    {
        Tap("DemoCollectionViewWithLabel");
        ShouldSee("Item A1");
        ShouldSee("Item B1");
        ShouldSee("Item C1");
    }

    [Test]
    public void TestHeadersAndFooters()
    {
        Tap("DemoCollectionViewWithLabel");
        ShouldSee("plain header");
        ShouldSee("plain footer");
    }

    [Test]
    public void TestDataSelector()
    {
        Tap("DemoCollectionViewWithDataSelector");
        ShouldSee("plain header");
        ShouldSee("plain footer");
        ShouldSee("Edit Me!", 2);
        ShouldSee("Read Me!", 1);
    }

    [Test]
    public void TestGrid()
    {
        Tap("DemoCollectionViewWithGrid");
        ShouldSee("Col 1");
        ShouldSee("Col 2");
    }

    [Test]
    public void TestTap()
    {
        Tap("DemoCollectionViewWithTapAction");
        Tap("Item A1");
        ShouldSee("Page for Item A1");
    }

    [Test]
    public void TestParentIsSetupCorrectly()
    {
        Tap("DemoCollectionViewWithGrid");
        var element = FindFirst("Col 1");
        Assert.That(element, Is.Not.Null);

        // Triggering the garbage collector reproduces an issue which occurs in real life, especially for larger
        // test suites:
        // If the parent for CollectionView ItemViews is not set, only the subviews of the ItemViews actually found
        // by QuickTest are held by a reference. All other parts of ItemViews can be removed by the garbage collector,
        // preventing the FindParent feature of QuickTest to work as intended.
        GC.Collect();
        Assert.That(element.FindParent<Grid>(), Is.Not.Null);

        // We also expect the CollectionView to be a parent of the ItemViews.
        Assert.That(element.FindParent<CollectionView>(), Is.Not.Null);
    }
}