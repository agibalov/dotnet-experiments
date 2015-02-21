using System;
using System.Threading.Tasks;

namespace WpfWebApiExperiment.ViewModels
{
    public interface ILongOperationExecutor
    {
        Task<T> Execute<T>(Func<T> func);
    }
}