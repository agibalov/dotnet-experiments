using System.Threading;
using DummyWpfApplication;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodedUiTestExperiment
{
    [CodedUITest]
    public class DummyWpfApplicationTest
    {
        [TestMethod]
        public void ItShouldDoAtLeastSomething()
        {
            App app = null;
            var t = new Thread(() =>
            {
                app = new App();
                app.InitializeComponent();
                app.Run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            var window = new WpfWindow();
            window.SearchProperties[UITestControl.PropertyNames.Name] = "MainWindow";

            var button = new WpfButton(window);
            button.SearchProperties[WpfControl.PropertyNames.AutomationId] = "MyButton";

            var title = new WpfTitleBar(window);
            Assert.AreEqual("MainWindow", title.DisplayText);

            // Thread.Sleep(1000);

            Mouse.Click(button);
            Assert.AreEqual("OMG!!!", title.DisplayText);

            // Thread.Sleep(1000);

            app.Dispatcher.Invoke(() => app.Shutdown());
        }
    }
}
