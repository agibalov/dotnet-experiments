using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DummyWpfApplication;
using NUnit.Framework;

namespace NonCodedUiTestExperiment
{
    public class DummyWpfApplicationNonCodedUiTest
    {
        [Test]
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

            Thread.Sleep(3000);

            app.Dispatcher.Invoke(() =>
            {
                var mainWindow = app.MainWindow;

                Assert.AreEqual("MainWindow", mainWindow.Title);

                var myButton = (Button)mainWindow.FindName("MyButton");
                myButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

                Assert.AreEqual("OMG!!!", mainWindow.Title);
            });

            Thread.Sleep(3000);

            app.Dispatcher.Invoke(() => app.Shutdown());
        }
    }
}
