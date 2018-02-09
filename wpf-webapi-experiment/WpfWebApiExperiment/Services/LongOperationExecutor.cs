using System;
using System.Threading.Tasks;
using Ninject;

namespace WpfWebApiExperiment.Services
{
    public class LongOperationExecutor : ILongOperationExecutor
    {
        private readonly ILongOperationListener _longOperationListener;

        [Inject]
        public LongOperationExecutor(ILongOperationListener longOperationListener)
        {
            _longOperationListener = longOperationListener;
        }

        public async Task<T> Execute<T>(Func<T> func)
        {
            _longOperationListener.OnOperationStarted();
            try
            {
                return await Task.Run(func);
            }
            finally
            {
                _longOperationListener.OnOperationFinished();
            }
        }
    }
}