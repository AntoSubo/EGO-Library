using EGO_Library.Models;
using System.Collections.ObjectModel;

namespace EGO_Library.ViewModels
{
    public class GiftDetailViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public ObservableCollection<string> Recipes { get; set; } = new();
        public ObservableCollection<string> Sources { get; set; } = new();

        public GiftDetailViewModel(EgoGift gift)
        {
            Name = gift.Name;
            Status = gift.Status;
            Description = gift.Description;
            ImagePath = gift.Icon; // или путь к изображению
            //Recipes = new ObservableCollection<string>(gift.FusionRecipes);
            //Sources = new ObservableCollection<string>(gift.Sources);
        }
    }
}