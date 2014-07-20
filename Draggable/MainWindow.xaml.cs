using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Draggable
{
    public partial class MainWindow : Window
    {
        private bool _isManipulating;
        private Point _originalMousePosition;
        private Point _originalRectPosition;

        public MainWindow()
        {
            InitializeComponent();

            MyRect.MouseDown += MyRectOnMouseDown;
            MyCanvas.MouseMove += MyRectOnMouseMove;
            MyRect.MouseUp += MyRectOnMouseUp;
        }

        private void MyRectOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_isManipulating)
            {
                throw new Exception("Impossible");
            }

            _isManipulating = true;
            _originalMousePosition = mouseButtonEventArgs.GetPosition(MyCanvas);
            _originalRectPosition = new Point(Canvas.GetLeft(MyRect), Canvas.GetTop(MyRect));

            Title = "Down";
        }

        private void MyRectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_isManipulating)
            {
                var currentPosition = mouseEventArgs.GetPosition(MyCanvas);
                var delta = currentPosition - _originalMousePosition;
                Canvas.SetLeft(MyRect, _originalRectPosition.X + delta.X);
                Canvas.SetTop(MyRect, _originalRectPosition.Y + delta.Y);
            }
        }

        private void MyRectOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!_isManipulating)
            {
                throw new Exception("Impossible");
            }

            _isManipulating = false;

            Title = "Up";
        }
    }
}
