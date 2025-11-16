using EGO_Library.Models;
using System.Collections.ObjectModel;

namespace EGO_Library.ViewModels
{
    public class GiftDetailViewModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ObservableCollection<string> Recipes { get; set; }
        public ObservableCollection<string> Sources { get; set; }

        public GiftDetailViewModel(EgoGift gift)
        {
            Name = gift.Name;
            Status = gift.Status;
            Description = gift.Description;
            ImagePath = gift.Icon; // или путь к изображению
            Recipes = new ObservableCollection<string>(gift.FusionRecipes);
            Sources = new ObservableCollection<string>(gift.Sources);
        }
    }
}