using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace QuickTest
{
    public class TestSystemResourcesProvider : ISystemResourcesProvider
    {
        public IResourceDictionary GetSystemResources()
        {
            return new ResourceDictionary();
        }
    }
}

