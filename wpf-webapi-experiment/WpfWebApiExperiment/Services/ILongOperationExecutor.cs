using System;
using System.Threading.Tasks;

namespace WpfWebApiExperiment.Services
{
    public interface ILongOperationExecutor
    {
        Task<T> Execute<T>(Func<T> func);
    }
}