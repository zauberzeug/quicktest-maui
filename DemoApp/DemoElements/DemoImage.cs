﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
namespace DemoApp
{
    public class DemoImage : Image
    {
        public DemoImage(string ressourceName)
        {
            Source = ImageSource.FromResource(ressourceName);
            Source.AutomationId = ressourceName;
            Aspect = Aspect.AspectFit;
        }
    }
}
