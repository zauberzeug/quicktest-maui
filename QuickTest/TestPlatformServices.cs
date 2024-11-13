using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace QuickTest
{
#pragma warning disable CS0612 // Type or member is obsolete (MAUI currently still uses ISystemResourcesProvider internally)
    public class TestSystemResourcesProvider : ISystemResourcesProvider
#pragma warning restore CS0612 // Type or member is obsolete
    {
        public IResourceDictionary GetSystemResources()
        {
            return new ResourceDictionary();
        }
    }
}

