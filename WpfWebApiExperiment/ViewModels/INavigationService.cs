namespace WpfWebApiExperiment.ViewModels
{
    public interface INavigationService
    {
        void NavigateToNoteList();
        void NavigateToNote(string id);
    }
}