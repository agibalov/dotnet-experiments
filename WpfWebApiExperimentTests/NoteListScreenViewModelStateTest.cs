using System;
using System.Collections.Generic;
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
        public async void DefaultStateCanLoadNotesAndSwitchToOkState()
        {
            var apiClient = new Mock<IApiClient>(MockBehavior.Strict);
            apiClient.Setup(c => c.GetNotes()).Returns(new List<NoteDTO>
            {
                new NoteDTO { Id = "123", Title = "Hi", Text = "Hello there" }
            });

            var longOperationExecutor = new Mock<ILongOperationExecutor>();
            longOperationExecutor.Setup(e => e.Execute(It.IsAny<Func<List<NoteDTO>>>()))
                .Returns((Func<List<NoteDTO>> f) => Task.FromResult(f()));

            var state = new DefaultNoteListScreenViewModelState();
            var newState = await state.HandleScreenActivated(apiClient.Object, longOperationExecutor.Object);
            Assert.IsInstanceOf<OkNoteListScreenViewModelState>(newState);

            var okState = (OkNoteListScreenViewModelState) newState;
            Assert.AreEqual(1, okState.Notes.Count);

            apiClient.Verify(c => c.GetNotes(), Times.Once);
            longOperationExecutor.Verify(e => e.Execute(It.IsAny<Func<List<NoteDTO>>>()), Times.Once);
        }
    }
}