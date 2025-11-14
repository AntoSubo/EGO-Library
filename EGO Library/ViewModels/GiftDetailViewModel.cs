using EGO_Library.Models;
using System.ComponentModel;

namespace EGO_Library.ViewModels
{
    public class GiftDetailViewModel : INotifyPropertyChanged
    {
        private EgoGift _selectedGift;
        public EgoGift SelectedGift
        {
            get => _selectedGift;
            set
            {
                _selectedGift = value;
                OnPropertyChanged(nameof(SelectedGift));
            }
        }

        public GiftDetailViewModel(EgoGift gift)
        {
            SelectedGift = gift;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
