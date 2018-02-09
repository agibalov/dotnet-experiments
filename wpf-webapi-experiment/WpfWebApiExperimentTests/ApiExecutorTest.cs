using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WpfWebApiExperiment.Services;
using WpfWebApiExperiment.WebApi;
using WpfWebApiExperiment.WebApiClient;

namespace WpfWebApiExperimentTests
{
    public class ApiExecutorTest
    {
        [Test]
        public async Task ApiExecutorReturnsTheResultWhenThereIsNoError()
        {
            var longOperationExecutor = new Mock<ILongOperationExecutor>(MockBehavior.Strict);
            longOperationExecutor.Setup(e => e.Execute(It.IsAny<Func<List<NoteDTO>>>()))
                .Returns((Func<List<NoteDTO>> f) => Task.FromResult(f()));
            
            var apiClient = new Mock<IApiClient>(MockBehavior.Strict);
            apiClient.Setup(c => c.Execute(It.IsAny<GetNotesApiRequest>())).Returns(new List<NoteDTO>
            {
                new NoteDTO {Id = "123", Title = "Hi", Text = "Hello there"}
            });
            
            var apiExecutor = new ApiExecutor(longOperationExecutor.Object, apiClient.Object);
            var notes = await apiExecutor.Execute(new GetNotesApiRequest());

            Assert.AreEqual(1, notes.Count);

            longOperationExecutor.Verify(e => e.Execute(It.IsAny<Func<List<NoteDTO>>>()), Times.Once);
            apiClient.Verify(c => c.Execute(It.IsAny<GetNotesApiRequest>()), Times.Once);
        }
    }
}
