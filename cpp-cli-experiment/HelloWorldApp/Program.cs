using System;
using HelloWorldLib;

namespace HelloWorldApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Result is {0}", new Calculator().InstanceAddNumbers(1, 2));
            Console.WriteLine("Result is {0}", Calculator.StaticAddNumbers(1, 2));
        }
    }
}
