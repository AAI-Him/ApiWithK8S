using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public sealed class TestErrorEventArgs : EventArgs
    {
        internal TestErrorEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    internal class Startup
    {
        // event & delegate test
        delegate void Notify(string message);
        event Notify ProcessCompleted;
        void Print1(string message) => Console.WriteLine($"Print1:{message}");
        void Print2(string message) => Console.WriteLine($"Print2:{message}");
        void Print3(string message) => Console.WriteLine($"Print3:{message}");

        public event EventHandler<TestErrorEventArgs> Error;

        public Startup(Action<string>[]? actions) 
        {
            if (actions != null)
            {
                Notify notifyDelegate = Print1;
                notifyDelegate += Print2;
                notifyDelegate += Print3;
                notifyDelegate("Hello world");
            }
        }

        protected virtual void OnError(string message)
        {
            if (Error != null)
            {
                Error.Invoke(this, new TestErrorEventArgs(message));
            }
        }

        protected virtual void OnProcessCompleted(string message)
        {
            if (ProcessCompleted == null)
            {
                ProcessCompleted += Print1;
                ProcessCompleted += Print2;
                ProcessCompleted += Print3;
            }
            ProcessCompleted.Invoke(message);
        }
    }
}
