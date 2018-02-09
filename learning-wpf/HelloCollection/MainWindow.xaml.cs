using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HelloCollection
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AppViewModel
            {
                People = new ObservableCollection<PersonViewModel>
                {
                    new PersonViewModel {Name = "Andrey"},
                    new PersonViewModel {Name = "loki2302"}
                }
            };
        }
    }

    public class AppViewModel
    {
        public ObservableCollection<PersonViewModel> People { get; set; }
        public ICommand AddPersonCommand { get; set; }
        public ICommand DeletePersonCommand { get; set; }

        public AppViewModel()
        {
            People = new ObservableCollection<PersonViewModel>();
            AddPersonCommand = new AddPersonCommand(this);
            DeletePersonCommand = new DeletePersonCommand(this);
        }
    }

    public class PersonViewModel
    {
        public string Name { get; set; }
    }

    public class AddPersonCommand : ICommand
    {
        private readonly AppViewModel _appViewModel;

        public AddPersonCommand(AppViewModel appViewModel)
        {
            _appViewModel = appViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _appViewModel.People.Add(new PersonViewModel
            {
                Name = Guid.NewGuid().ToString()
            });
        }

        public event EventHandler CanExecuteChanged;
    }

    public class DeletePersonCommand : ICommand
    {
        private readonly AppViewModel _appViewModel;

        public DeletePersonCommand(AppViewModel appViewModel)
        {
            _appViewModel = appViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _appViewModel.People.Remove((PersonViewModel)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
