using EGO_Library.Models;
using System.Collections.ObjectModel;

namespace EGO_Library.ViewModels
{
    public class GiftListViewModel
    {
        public ObservableCollection<EgoGift> Gifts { get; set; }

        public GiftListViewModel()
        {
            Gifts = new ObservableCollection<EgoGift>
            {
                new EgoGift { Name = "Wealth", Tier = 4, Status = "Charge", Icon = "💰" },
                new EgoGift { Name = "Inferno", Tier = 3, Status = "Burn", Icon = "🔥" },
                new EgoGift { Name = "Torrent", Tier = 2, Status = "Bleed", Icon = "💧" }
            };
        }
    }
}