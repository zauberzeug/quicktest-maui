using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace QuickTest
{
    public class ElementInfo
    {
        public Element Element;
        public Action InvokeTap;

        public static ElementInfo FromElement(Element element)
        {
            return new ElementInfo {
                Element = element,
            };
        }
    }
}
