﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls.Internals;

namespace QuickTest
{
    public class ActionSheetPopup : Popup
    {
        readonly ActionSheetArguments arguments;

        public ActionSheetPopup(ActionSheetArguments arguments)
        {
            this.arguments = arguments;
        }

        public override bool Contains(string text)
        {
            return arguments.Title == text
                || arguments.Cancel == text
                || arguments.Destruction == text
                || arguments.Buttons.Contains(text);
        }

        public override int Count(string text)
        {
            return arguments.Buttons.Concat(new string[] { arguments.Title, arguments.Cancel, arguments.Destruction }).Count(t => t == text);
        }

        public override bool Tap(string text)
        {
            if (arguments.Cancel == text || arguments.Destruction == text || arguments.Buttons.Contains(text)) {
                arguments.SetResult(text);
                return true;
            } else
                return false;
        }

        public override string Render()
        {
            var parts = new List<string> { arguments.Title };
            if (arguments.Destruction != null)
                parts.Add($"[{arguments.Destruction}]");
            foreach (var button in arguments.Buttons)
                parts.Add($"[{button}]");
            if (arguments.Cancel != null)
                parts.Add($"[{arguments.Cancel}]");
            return string.Join("\n", parts);
        }

        public override string ToString()
        {
            return $"Action sheet \"{arguments.Title}\"";
        }
    }
}
