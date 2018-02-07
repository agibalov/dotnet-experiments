using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using LinFu.Proxy;

namespace LinFuExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = new LinFu.DynamicObject();

            IMagic m = new Magic().CreateDuck<IMagic>();
            m.DoSomething();
        }
    }

    public interface IMagic
    {
        void DoSomething();
    }

    public class Magic : DynamicObject
    {
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Console.WriteLine("something happened - {0}", binder.Name);
            result = null;
            return true;
        }
    }
}
