using System.Threading.Tasks;
using DemoApp;
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
}