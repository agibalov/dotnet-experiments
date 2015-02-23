using System.Threading.Tasks;
using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.ViewModels;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperimentTests
{
    public class NoteScreenViewModelTest
    {
        [Test]
        public void WhenThereIsNoApiErrorNoteIsLoaded()
        {
            var apiExecutor = new Mock<IApiExecutor>(MockBehavior.Strict);
            apiExecutor.Setup(e => e.Execute(It.Is<GetNoteApiRequest>(r => r.Id == "123")))
                .Returns(Task.FromResult(new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}));

            var navigationService = new Mock<INavigationService>();

            var noteScreenViewModel = new NoteScreenViewModel(
                apiExecutor.Object, 
                navigationService.Object,
                "123");

            ((IActivate)noteScreenViewModel).Activate();

            Assert.AreEqual("123", noteScreenViewModel.Id);
            Assert.AreEqual("Hi", noteScreenViewModel.Title);
            Assert.AreEqual("Hello there", noteScreenViewModel.Text);

            apiExecutor.Verify(c => c.Execute(It.Is<GetNoteApiRequest>(r => r.Id == "123")), Times.Once);
        }

        [Test]
        public void CanNavigateBack()
        {
            var apiExecutor = new Mock<IApiExecutor>(MockBehavior.Strict);
            apiExecutor.Setup(e => e.Execute(It.Is<GetNoteApiRequest>(r => r.Id == "123")))
                .Returns(Task.FromResult(new NoteDTO { Id = "123", Title = "Hi", Text = "Hello there" }));

            var navigationService = new Mock<INavigationService>(MockBehavior.Strict);
            navigationService.Setup(s => s.NavigateToNoteList());

            var noteScreenViewModel = new NoteScreenViewModel(
                apiExecutor.Object,
                navigationService.Object,
                "123");

            ((IActivate)noteScreenViewModel).Activate();

            noteScreenViewModel.GoBack();

            navigationService.Verify(s => s.NavigateToNoteList(), Times.Once);
        }
    }
}
