using System.ComponentModel;
using System.Runtime.CompilerServices;
using HelloValueConverter.Annotations;

namespace HelloValueConverter
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public static readonly bool BoolTrue = true;
        public static readonly bool BoolFalse = false;

        private bool _shouldBeVisible;
        public bool ShouldBeVisible
        {
            get { return _shouldBeVisible; }
            set
            {
                _shouldBeVisible = value;
                OnPropertyChanged();
            }
        }

        private Sex _sex;
        public Sex Sex
        {
            get
            {
                return _sex;
            }

            set
            {
                _sex = value;
                OnPropertyChanged();
            }
        }

        private bool _yesNo;
        public bool YesNo
        {
            get
            {
                return _yesNo;
            }

            set
            {
                _yesNo = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Sex
    {
        NoIdea,
        Male,
        Female
    }
}