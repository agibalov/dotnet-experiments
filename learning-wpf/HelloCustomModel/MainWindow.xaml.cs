using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelloCustomModel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TheViewModel
            {
                FancyThings = new ObservableCollection<FancyThing>
                {
                    new FancyThing { FancinessDescription = "qqq1" }, 
                    new FancyThing { FancinessDescription = "qqq2" }, 
                    new FancyThing { FancinessDescription = "qqq3" }
                }
            };

            AddAnItem.Click += AddAnItem_Click;
        }

        void AddAnItem_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<FancyThing> fancyThings = ((TheViewModel)DataContext).FancyThings;
            fancyThings.Add(new FancyThing { FancinessDescription = Guid.NewGuid().ToString() });
            fancyThings.Add(new FancyThing { FancinessDescription = Guid.NewGuid().ToString() });
        }
    }

    public class TheViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FancyThing> FancyThings { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FancyThing : INotifyPropertyChanged
    {
        private string _fancinessDescription;

        public string FancinessDescription
        {
            get { return _fancinessDescription; }
            set
            {
                _fancinessDescription = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
