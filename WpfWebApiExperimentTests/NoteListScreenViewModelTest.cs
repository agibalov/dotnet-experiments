using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using WpfWebApiExperiment.ViewModels;
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
            apiClient.Setup(c => c.GetNotes()).ReturnsAsync(new List<NoteDTO>
            {
                new NoteDTO { Id = "123", Title = "Hi", Text = "Hello there" }
            });

            var navigationService = new Mock<INavigationService>();

            var longOperationExecutor = new Mock<ILongOperationExecutor>();
            longOperationExecutor.Setup(e => e.Execute(It.IsAny<Func<Task<List<NoteDTO>>>>()))
                .Returns((Func<Task<List<NoteDTO>>> f) => f());

            var noteListScreenViewModel = new NoteListScreenViewModel(
                apiClient.Object, 
                navigationService.Object, 
                longOperationExecutor.Object);
            ((IActivate)noteListScreenViewModel).Activate();

            Assert.AreEqual(1, noteListScreenViewModel.Notes.Count);
        }
    }
}
