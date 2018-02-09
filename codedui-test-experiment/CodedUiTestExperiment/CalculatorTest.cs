using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodedUiTestExperiment
{
    [CodedUITest]
    public class CalculatorTest
    {
        [TestMethod]
        public void OneAndTwoShouldBeThree()
        {
            var calculatorApplication = ApplicationUnderTest.Launch(@"c:\windows\System32\calc.exe");
            var calculatorWindow = MakeWindowWithClassName("CalcFrame");

            var button1 = MakeButtonWithParentAndName(calculatorWindow, "1");
            var button2 = MakeButtonWithParentAndName(calculatorWindow, "2");
            var addButton = MakeButtonWithParentAndName(calculatorWindow, "Add");
            var equalsButton = MakeButtonWithParentAndName(calculatorWindow, "Equals");
            var resultText = MakeLabelWithParentAndName(calculatorWindow, "Result");

            Mouse.Click(button1);
            Mouse.Click(addButton);
            Mouse.Click(button2);
            Mouse.Click(equalsButton);
            Assert.AreEqual("3", resultText.DisplayText);
        }

        private static WinWindow MakeWindowWithClassName(string className)
        {
            var winWindow = new WinWindow();
            winWindow.SearchProperties[UITestControl.PropertyNames.ClassName] = className;
            return winWindow;
        }

        private static WinButton MakeButtonWithParentAndName(UITestControl parentWindow, string name)
        {
            var winButton = new WinButton(parentWindow);
            winButton.SearchProperties[UITestControl.PropertyNames.Name] = name;
            return winButton;
        }

        private static WinText MakeLabelWithParentAndName(UITestControl parentWindow, string name)
        {
            var winText = new WinText(parentWindow);
            winText.SearchProperties[UITestControl.PropertyNames.Name] = name;
            return winText;
        }
    }
}
