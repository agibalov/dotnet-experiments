namespace WpfWebApiExperiment.ViewModels
{
    public interface ILongOperationListener
    {
        void OnOperationStarted();
        void OnOperationFinished();
    }
}