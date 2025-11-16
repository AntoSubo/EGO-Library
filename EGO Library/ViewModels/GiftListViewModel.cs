using EGO_Library.Models;
using EGO_Library.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EGO_Library.ViewModels
{
    public class GiftListViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private ObservableCollection<EgoGift> _gifts;

        public ObservableCollection<EgoGift> Gifts
        {
            get => _gifts;
            set { _gifts = value; OnPropertyChanged(); }
        }

        public GiftListViewModel()
        {
            _dataService = new DataService();
            _ = LoadGiftsAsync(); // Запускаем асинхронно без await
        }

        private async Task LoadGiftsAsync()
        {
            // Инициализируем базу при первом запуске
            await DatabaseInitializer.InitializeAsync();

            var gifts = await _dataService.GetAllGiftsAsync();
            Gifts = new ObservableCollection<EgoGift>(gifts);
        }
    }
}