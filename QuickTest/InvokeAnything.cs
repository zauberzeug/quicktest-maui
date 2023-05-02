using System;
using System.Linq;
using System.Reflection;

namespace QuickTest
{
    public static class InvokeAnything
    {
        static BindingFlags bindingFlags = BindingFlags.FlattenHierarchy |
                                                       BindingFlags.NonPublic | BindingFlags.Public |
                                                       BindingFlags.Instance | BindingFlags.Static;

        public static void Invoke(this object obj, string methodName, params object[] parameters)
        {
            foreach (var method in obj.GetType().GetMethods(bindingFlags))
                if (method.Name.Split('.').Last() == methodName && method.GetParameters().Length == parameters.Length)
                {
                    method.Invoke(obj, parameters);
                    return;
                }
            throw new InvalidOperationException($"Method with name {methodName} and parameter count {parameters.Length} not found on type {obj.GetType().FullName}");
        }
    }
}
