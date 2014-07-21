using System.Threading;
using System.Threading.Tasks;

namespace HelloCaliburnMicro
{
    public class KindOfRemoteService
    {
        public async Task<string> GetDefaultValue()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "Default Value";
            });
        }

        public async Task<string> MakeHelloMessage(string name)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(2000);
                return string.Format("Hello {0}", name);
            });
        }
    }
}