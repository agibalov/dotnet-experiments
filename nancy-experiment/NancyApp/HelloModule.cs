using Nancy;

namespace NancyApp
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = parameters => "Hello World";

            Get["/add/{a}/{b}"] = parameters =>
                {
                    var a = (int)parameters.a;
                    var b = (int)parameters.b;
                    var result = a + b;
                    return string.Format("{0}", result);
                };
        }
    }
}