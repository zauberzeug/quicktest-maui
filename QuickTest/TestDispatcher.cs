using System;
using Microsoft.Maui.Dispatching;
using System.Threading;

namespace QuickTest
{
    public class TestDispatcher : IDispatcher
    {
        public bool IsDispatchRequired => false;

        public IDispatcherTimer CreateTimer()
        {
            Console.WriteLine("!!! TestDispatcher CreateTimer !!!");
            throw new NotImplementedException();
        }

        public bool Dispatch(Action action)
        {
            action();
            return true;
        }

        public bool DispatchDelayed(TimeSpan delay, Action action)
        {
            Console.WriteLine($"!!! DispatchDelayed {delay} !!!");
            throw new NotImplementedException();
        }
    }

    public class TestDispatcherProvider : IDispatcherProvider
    {
        IDispatcher dispatcher;

        public IDispatcher GetForCurrentThread()
        {
            if (dispatcher == null)
                dispatcher = new TestDispatcher();

            return dispatcher;
        }
    }
}

