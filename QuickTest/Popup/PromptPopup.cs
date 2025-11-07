using System.Linq;
using Microsoft.Maui.Controls.Internals;

namespace QuickTest
{
    public class PromptPopup : Popup
    {
        readonly PromptArguments arguments;
        string inputValue;

        public PromptPopup(PromptArguments arguments)
        {
            this.arguments = arguments;
            this.inputValue = arguments.InitialValue;
        }

        public override bool Contains(string text)
        {
            return arguments.Title == text
                   || arguments.Message == text
                   || arguments.Placeholder == text
                   || arguments.InitialValue == text
                   || arguments.Accept == text
                   || arguments.Cancel == text;
        }

        public override int Count(string text)
        {
            return new string[] {
                arguments.Title,
                arguments.Message,
                arguments.Placeholder,
                arguments.InitialValue,
                arguments.Accept,
                arguments.Cancel
            }.Count(t => t == text);
        }

        public override bool Tap(string text)
        {
            if (arguments.Accept == text) {
                // Use inputValue if set, otherwise use initial value
                var result = inputValue ?? arguments.InitialValue ?? "";
                arguments.SetResult(result);
                return true;
            } else if (arguments.Cancel == text) {
                arguments.SetResult(null);
                return true;
            } else
                return false;
        }

        public void Input(string text)
        {
            inputValue = text;
        }

        public override string Render()
        {
            var placeholder = arguments.Placeholder ?? "";
            var currentValue = inputValue ?? "";
            return
                $"{arguments.Title}\n{arguments.Message}\n\n[{currentValue}{placeholder}]\n\n{RenderButtons(arguments.Accept, arguments.Cancel)}";
        }

        public override string ToString()
        {
            return $"Prompt \"{arguments.Title}\"";
        }

        string RenderButtons(params string[] titles)
        {
            return string.Join(" ", titles.Where(t => t != null).Select(t => $"[{t}]"));
        }
    }
}
