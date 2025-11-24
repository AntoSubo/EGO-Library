using EGO_Library.Models;
using EGO_Library.Services;
using System.Windows.Input;

namespace EGO_Library.ViewModels
{
    public class GiftDetailViewModel : BaseViewModel
    {
        private readonly EgoGift _gift;
        private readonly INavigationService _navigationService;

        public EgoGift Gift => _gift;
        public ICommand GoBackCommand { get; }

        public GiftDetailViewModel(EgoGift gift, INavigationService navigationService)
        {
            _gift = gift;
            _navigationService = navigationService;
            GoBackCommand = new RelayCommand(_ => _navigationService.GoBack());
        }
    }
}