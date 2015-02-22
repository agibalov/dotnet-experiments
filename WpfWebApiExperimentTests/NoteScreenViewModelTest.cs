using System;
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
            var apiClient = new Mock<IApiClient>();
            apiClient.Setup(c => c.GetNote("123"))
                .Returns(new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"});

            var navigationService = new Mock<INavigationService>();
            var longOperationExecutor = new Mock<ILongOperationExecutor>();
            longOperationExecutor.Setup(e => e.Execute(It.IsAny<Func<NoteDTO>>()))
                .Returns((Func<NoteDTO> f) => Task.FromResult(f()));

            var noteScreenViewModel = new NoteScreenViewModel(
                apiClient.Object, 
                navigationService.Object,
                longOperationExecutor.Object, 
                "123");

            ((IActivate)noteScreenViewModel).Activate();

            Assert.AreEqual("123", noteScreenViewModel.Id);
            Assert.AreEqual("Hi", noteScreenViewModel.Title);
            Assert.AreEqual("Hello there", noteScreenViewModel.Text);

            apiClient.Verify(c => c.GetNote("123"), Times.Once);
        }

        [Test]
        public void CanNavigateBack()
        {
            var apiClient = new Mock<IApiClient>();
            apiClient.Setup(c => c.GetNote("123"))
                .Returns(new NoteDTO { Id = "123", Title = "Hi", Text = "Hello there" });

            var navigationService = new Mock<INavigationService>();
            var longOperationExecutor = new Mock<ILongOperationExecutor>();
            longOperationExecutor.Setup(e => e.Execute(It.IsAny<Func<NoteDTO>>()))
                .Returns((Func<NoteDTO> f) => Task.FromResult(f()));

            var noteScreenViewModel = new NoteScreenViewModel(
                apiClient.Object,
                navigationService.Object,
                longOperationExecutor.Object,
                "123");

            ((IActivate)noteScreenViewModel).Activate();

            noteScreenViewModel.GoBack();

            navigationService.Verify(s => s.NavigateToNoteList(), Times.Once);
        }
    }
}
