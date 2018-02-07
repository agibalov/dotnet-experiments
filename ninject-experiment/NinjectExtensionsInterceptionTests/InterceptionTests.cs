using NUnit.Framework;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;

namespace NinjectExtensionsInterceptionTests
{
    public class InterceptionTests
    {
        [Test]
        public void CanInterceptMethodCall()
        {
            var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false }, new LinFuModule());
            kernel.Bind<TracingInterceptor>().ToSelf().InSingletonScope();
            kernel.Bind<CalculatorService>().ToSelf();
            
            var calculator = kernel.Get<CalculatorService>();
            Assert.AreEqual(3, calculator.Add(1, 2));

            var tracingInterceptor = kernel.Get<TracingInterceptor>();
            Assert.AreEqual(1, tracingInterceptor.LastA);
            Assert.AreEqual(2, tracingInterceptor.LastB);
            Assert.AreEqual(3, tracingInterceptor.LastResult);
        }
    }

    public class CalculatorService
    {
        [TraceCalls]
        public virtual int Add(int a, int b)
        {
            return a + b;
        }
    }

    public class TracingInterceptor : IInterceptor
    {
        public int LastA { get; set; }
        public int LastB { get; set; }
        public int LastResult { get; set; }

        public void Intercept(IInvocation invocation)
        {
            LastA = (int)invocation.Request.Arguments[0];
            LastB = (int)invocation.Request.Arguments[1];

            invocation.Proceed();

            LastResult = (int)invocation.ReturnValue;
        }
    }

    public class TraceCallsAttribute : InterceptAttribute
    {
        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return request.Context.Kernel.Get<TracingInterceptor>();
        }
    }
}
