# Quicktest for .NET MAUI

Quicktest provides infrastructure to write acceptance and integration tests with NUnit for .NET Maui apps. With a little care (mocking HTTP, etc) you can achieve a very fast executing set of tests to ensure high level requirements are working as expected.

```csharp

public class ToolingTests : QuickTest<App>
{
    [SetUp]
    protected override void SetUp()
    {
        base.SetUp();

        Launch(new App());
    }

    [Test]
    public void LoginShouldGreetUser()
    {
        Input("Username", "test user");
        Input("Password", "mysecret");
        Tap("Login");
        ShouldSee("Welcome test user");
    }
}
```

We created this project for internal purposes but wanted to share it with the community as soon as possible. There is no documentation right now, but we think that looking at our tests provided with the source code you should be able to figure things out.

# Current state of this library

Due to the unreliable state of .NET MAUI in mid 2023, we postponed the migration of our Xamarin.Forms apps. This library represents our initial port of Quicktest from [Xamarin.Forms](https://github.com/zauberzeug/quicktest) to MAUI. It has not been maintained since. Some details:

- The library worked well with the .NET 7.0 MAUI release available in May 2023.
- It provides the same API and features as the Xamarin.Forms versions. Previously missing controls and new controls have not been added.
- We ported the lib to .NET 7, so it can't be used with .NET 6.

# NuGet

The library can be installed via nuget.org (https://www.nuget.org/packages/Zauberzeug.QuickTest.Maui).
