using System;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace QuickTest
{
    /// <summary>
    /// Custom alert subscription that intercepts all alert/action sheet/prompt requests
    /// for testing purposes in .NET MAUI 10+
    /// </summary>
    internal class TestAlertSubscription : DispatchProxy
    {
        private User _user;
        private object _platformSubscription;

        public static object Create(User user, object platformSubscription)
        {
            // Get the internal IAlertManagerSubscription interface
            var assembly = typeof(Microsoft.Maui.Controls.Window).Assembly;
            var subscriptionInterface =
                assembly.GetType("Microsoft.Maui.Controls.Platform.AlertManager+IAlertManagerSubscription");

            if (subscriptionInterface == null) {
                throw new InvalidOperationException("Could not find IAlertManagerSubscription interface");
            }

            // Create a dynamic proxy that implements the interface
            var proxy = DispatchProxy.Create(subscriptionInterface, typeof(TestAlertSubscription));

            // Initialize our custom handler
            var testSubscription = (TestAlertSubscription)(object)proxy;
            testSubscription._user = user;
            testSubscription._platformSubscription = platformSubscription;

            return proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            // Intercept the alert/action sheet/prompt methods
            if (targetMethod.Name == "OnAlertRequested" && args.Length == 2) {
                var page = args[0] as Page;
                var arguments = args[1] as AlertArguments;

                _user.RegisterPopup(new AlertPopup(arguments));

                // Call platform implementation if it exists
                if (_platformSubscription != null) {
                    return targetMethod.Invoke(_platformSubscription, args);
                }
                // Note: Don't call SetResult here - let the test tap buttons which will call SetResult
            } else if (targetMethod.Name == "OnActionSheetRequested" && args.Length == 2) {
                var page = args[0] as Page;
                var arguments = args[1] as ActionSheetArguments;
                _user.RegisterPopup(new ActionSheetPopup(arguments));

                if (_platformSubscription != null) {
                    return targetMethod.Invoke(_platformSubscription, args);
                }
                // Note: Don't call SetResult here - let the test tap buttons which will call SetResult
            } else if (targetMethod.Name == "OnPromptRequested" && args.Length == 2) {
                var page = args[0] as Page;
                var arguments = args[1] as PromptArguments;

                _user.RegisterPopup(new PromptPopup(arguments));

                if (_platformSubscription != null) {
                    return targetMethod.Invoke(_platformSubscription, args);
                }
                // Note: Don't call SetResult here - let the test enter text and tap which will call SetResult
            } else if (targetMethod.Name == "OnPageBusy") {
                // Forward to platform if it exists
                if (_platformSubscription != null) {
                    return targetMethod.Invoke(_platformSubscription, args);
                }
            }

            return null;
        }
    }
}
