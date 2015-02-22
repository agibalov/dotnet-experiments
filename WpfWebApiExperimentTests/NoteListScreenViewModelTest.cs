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
            var apiClient = new Mock<IApiClient>();
            apiClient.Setup(c => c.GetNotes()).Returns(new List<NoteDTO>
            {
                new NoteDTO { Id = "123", Title = "Hi", Text = "Hello there" }
            });

            var navigationService = new Mock<INavigationService>();

            var longOperationExecutor = new Mock<ILongOperationExecutor>();
            longOperationExecutor.Setup(e => e.Execute(It.IsAny<Func<List<NoteDTO>>>()))
                .Returns((Func<List<NoteDTO>> f) => Task.FromResult(f()));

            var noteListScreenViewModel = new NoteListScreenViewModel(
                apiClient.Object, 
                navigationService.Object, 
                longOperationExecutor.Object);

            ((IActivate)noteListScreenViewModel).Activate();

            var noteListScreenViewModelState = noteListScreenViewModel.State;
            Assert.IsInstanceOf<OkNoteListScreenViewModelState>(noteListScreenViewModelState);
            var okState = (OkNoteListScreenViewModelState) noteListScreenViewModelState;
            Assert.AreEqual(1, okState.Notes.Count);

            apiClient.Verify(c => c.GetNotes(), Times.Once);
        }

        [Test]
        public void WhenThereIsApiErrorTheMessageIsDisplayed()
        {
            var apiClient = new Mock<IApiClient>(MockBehavior.Strict);
            apiClient.Setup(c => c.GetNotes()).Throws(new ApiException("something bad"));

            var navigationService = new Mock<INavigationService>(MockBehavior.Strict);
            
            var longOperationExecutor = new Mock<ILongOperationExecutor>(MockBehavior.Strict);
            longOperationExecutor.Setup(e => e.Execute(It.IsAny<Func<List<NoteDTO>>>()))
                .Returns((Func<List<NoteDTO>> f) => Task.FromResult(f()));

            var noteListScreenViewModel = new NoteListScreenViewModel(
                apiClient.Object,
                navigationService.Object,
                longOperationExecutor.Object);

            ((IActivate)noteListScreenViewModel).Activate();

            var noteListScreenViewModelState = noteListScreenViewModel.State;
            Assert.IsInstanceOf<ErrorNoteListScreenViewModelState>(noteListScreenViewModelState);
            var errorState = (ErrorNoteListScreenViewModelState) noteListScreenViewModelState;
            Assert.IsNotNullOrEmpty(errorState.ErrorMessage);
        }

        [Test]
        public void CanNavigateToNote()
        {
            var apiClient = new Mock<IApiClient>();
            apiClient.Setup(c => c.GetNotes()).Returns(new List<NoteDTO>
            {
                new NoteDTO { Id = "123", Title = "Hi", Text = "Hello there" }
            });

            var navigationService = new Mock<INavigationService>();

            var longOperationExecutor = new Mock<ILongOperationExecutor>();
            longOperationExecutor.Setup(e => e.Execute(It.IsAny<Func<List<NoteDTO>>>()))
                .Returns((Func<List<NoteDTO>> f) => Task.FromResult(f()));

            var noteListScreenViewModel = new NoteListScreenViewModel(
                apiClient.Object,
                navigationService.Object,
                longOperationExecutor.Object);

            ((IActivate)noteListScreenViewModel).Activate();

            var noteListScreenViewModelState = noteListScreenViewModel.State;
            Assert.IsInstanceOf<OkNoteListScreenViewModelState>(noteListScreenViewModelState);
            var okState = (OkNoteListScreenViewModelState)noteListScreenViewModelState;
            Assert.AreEqual(1, okState.Notes.Count);

            var targetNote = okState.Notes.First();
            noteListScreenViewModel.ViewNote(targetNote);

            navigationService.Verify(s => s.NavigateToNote(targetNote.Id), Times.Once);
        }
    }
}
