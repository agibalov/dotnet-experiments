using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.ViewModels.NoteListScreen;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperimentTests
{
    public class NoteListScreenViewModelStateTest
    {
        [Test]
        public async Task DefaultStateCanLoadNotesAndSwitchToOkState()
        {
            var apiExecutor = new Mock<IApiExecutor>(MockBehavior.Strict);
            apiExecutor.Setup(e => e.Execute(It.IsAny<GetNotesApiRequest>())).Returns(Task.FromResult(new List<NoteDTO>
            {
                new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}
            }));

            var state = new DefaultNoteListScreenViewModelState();
            var newState = await state.HandleScreenActivated(apiExecutor.Object);
            Assert.IsInstanceOf<OkNoteListScreenViewModelState>(newState);

            var okState = (OkNoteListScreenViewModelState) newState;
            Assert.AreEqual(1, okState.Notes.Count);

            apiExecutor.Verify(e => e.Execute(It.IsAny<GetNotesApiRequest>()), Times.Once);
        }

        [Test]
        public async Task DefaultStateSwitchesToErrorStateWhenItCantLoadNotes()
        {
            var apiExecutor = new Mock<IApiExecutor>(MockBehavior.Strict);
            apiExecutor.Setup(e => e.Execute(It.IsAny<GetNotesApiRequest>())).Throws(new ApiException("something bad"));

            var state = new DefaultNoteListScreenViewModelState();
            var newState = await state.HandleScreenActivated(apiExecutor.Object);
            Assert.IsInstanceOf<ErrorNoteListScreenViewModelState>(newState);

            var errorState = (ErrorNoteListScreenViewModelState)newState;
            Assert.IsNotNullOrEmpty(errorState.ErrorMessage);

            apiExecutor.Verify(e => e.Execute(It.IsAny<GetNotesApiRequest>()), Times.Once);
        }

        [Test]
        public void OkStateAllowsForNavigatingToASingleNote()
        {
            var state = new OkNoteListScreenViewModelState(new List<NoteDTO>
            {
                new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}
            });

            var navigationService = new Mock<INavigationService>(MockBehavior.Strict);
            navigationService.Setup(s => s.NavigateToNote("123"));

            state.HandleViewNote(state.Notes.First(), navigationService.Object);

            navigationService.Verify(s => s.NavigateToNote("123"), Times.Once);
        }

        [Test]
        public void ErrorStateHasAnErrorMessage()
        {
            var state = new ErrorNoteListScreenViewModelState("something bad");
            Assert.AreEqual("something bad", state.ErrorMessage);
        }
    }
}
