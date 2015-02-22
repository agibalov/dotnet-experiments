namespace WpfWebApiExperiment.Services
{
    public interface ILongOperationListener
    {
        void OnOperationStarted();
        void OnOperationFinished();
    }
}