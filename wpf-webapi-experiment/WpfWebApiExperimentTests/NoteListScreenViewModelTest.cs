using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.ViewModels;
using WpfWebApiExperiment.ViewModels.NoteListScreen;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperimentTests
{
    public class NoteListScreenViewModelTest
    {
        [Test]
        public void WhenThereIsNoApiErrorNotesAreLoaded()
        {
            var apiExecutor = new Mock<IApiExecutor>(MockBehavior.Strict);
            apiExecutor.Setup(e => e.Execute(It.IsAny<GetNotesApiRequest>())).Returns(Task.FromResult(new List<NoteDTO>
            {
                new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}
            }));

            var navigationService = new Mock<INavigationService>();

            var noteListScreenViewModel = new NoteListScreenViewModel(
                apiExecutor.Object, 
                navigationService.Object);

            ((IActivate)noteListScreenViewModel).Activate();

            var noteListScreenViewModelState = noteListScreenViewModel.State;
            Assert.IsInstanceOf<OkNoteListScreenViewModelState>(noteListScreenViewModelState);
            var okState = (OkNoteListScreenViewModelState) noteListScreenViewModelState;
            Assert.AreEqual(1, okState.Notes.Count);

            apiExecutor.Verify(e => e.Execute(It.IsAny<GetNotesApiRequest>()), Times.Once);
        }

        [Test]
        public void WhenThereIsApiErrorTheMessageIsDisplayed()
        {
            var apiExecutor = new Mock<IApiExecutor>(MockBehavior.Strict);
            apiExecutor.Setup(e => e.Execute(It.IsAny<GetNotesApiRequest>())).Throws(new ApiException("something bad"));

            var navigationService = new Mock<INavigationService>(MockBehavior.Strict);

            var noteListScreenViewModel = new NoteListScreenViewModel(
                apiExecutor.Object,
                navigationService.Object);

            ((IActivate)noteListScreenViewModel).Activate();

            var noteListScreenViewModelState = noteListScreenViewModel.State;
            Assert.IsInstanceOf<ErrorNoteListScreenViewModelState>(noteListScreenViewModelState);
            var errorState = (ErrorNoteListScreenViewModelState) noteListScreenViewModelState;
            Assert.IsNotNullOrEmpty(errorState.ErrorMessage);

            apiExecutor.Verify(e => e.Execute(It.IsAny<GetNotesApiRequest>()), Times.Once);
        }

        [Test]
        public void CanNavigateToNote()
        {
            var apiExecutor = new Mock<IApiExecutor>(MockBehavior.Strict);
            apiExecutor.Setup(e => e.Execute(It.IsAny<GetNotesApiRequest>())).Returns(Task.FromResult(new List<NoteDTO>
            {
                new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}
            }));

            var navigationService = new Mock<INavigationService>(MockBehavior.Strict);
            navigationService.Setup(s => s.NavigateToNote("123"));

            var noteListScreenViewModel = new NoteListScreenViewModel(
                apiExecutor.Object,
                navigationService.Object);

            ((IActivate)noteListScreenViewModel).Activate();

            var state = noteListScreenViewModel.State;
            Assert.IsInstanceOf<OkNoteListScreenViewModelState>(state);
            var okState = (OkNoteListScreenViewModelState)state;
            Assert.AreEqual(1, okState.Notes.Count);

            var targetNote = okState.Notes.First();
            noteListScreenViewModel.ViewNote(targetNote);

            apiExecutor.Verify(e => e.Execute(It.IsAny<GetNotesApiRequest>()), Times.Once);
            navigationService.Verify(s => s.NavigateToNote("123"), Times.Once);
        }
    }
}
