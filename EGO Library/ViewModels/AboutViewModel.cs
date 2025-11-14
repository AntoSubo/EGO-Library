using System.ComponentModel;

namespace EGO_Library.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        public string AppName { get; set; } = "EGO Library";
        public string Author { get; set; } = "[ФИО]";
        public string Version { get; set; } = "1.0.0";
        public string Technologies { get; set; } = "WPF, C#";

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
