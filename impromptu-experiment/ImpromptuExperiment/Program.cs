using System;
using System.Dynamic;
using ImpromptuInterface;

namespace ImpromptuExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            IDummyService dummyService = new DynamicDummyServiceImpl()
                .ActLike<IDummyService>();

            dummyService.PrintSomething();
            dummyService.AddNumbers(1, 2);
        }
    }

    public interface IDummyService
    {
        void PrintSomething();
        void AddNumbers(int i, int j);
    }

    public class DynamicDummyServiceImpl : DynamicObject
    {
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Console.WriteLine("Executing {0} with args [{1}]", binder.Name, string.Join(",", args));
            result = null;
            return true;
        }
    }
}
