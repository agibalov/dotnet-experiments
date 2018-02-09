using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelloArchitecture
{
    public class KindOfRemoteService
    {
        public async Task<string> GetMagicValue(string extra)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(3000);
                return Guid.NewGuid() + ", parameter=" + extra;
            });
        }
    }
}