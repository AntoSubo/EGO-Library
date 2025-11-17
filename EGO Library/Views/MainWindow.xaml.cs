//using EGO_Library.Views.Controls;
//using System.Windows;
//using System.Windows.Controls;

//namespace EGO_Library.Views
//{
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            InitializeComponent();
//            NavigateToGiftList();
//        }

//        public void NavigateToGiftList()
//        {
//            MainContent.Content = new GiftListView();
//        }

//        public void NavigateToRecipes()
//        {
//            MainContent.Content = new RecipeView();
//        }

//        public void NavigateToAbout()
//        {
//            MainContent.Content = new AboutView();
//        }

//        public void NavigateToGiftDetail()
//        {
//            MainContent.Content = new GiftDetailView();
//        }

//        public ContentControl MainContentControl => MainContent;
//    }
//}

using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace EGO_Library.Services
{
    public class DataService
    {

        // EgoGift operations
        public ObservableCollection<EgoGift> GetAllGifts()
        {
            var gifts = _context.EgoGifts
                .Include(g => g.Sources)
                .OrderBy(g => g.Tier)
                .ThenBy(g => g.Name)
                .ToList();
            return new ObservableCollection<EgoGift>(gifts);
        }

        public EgoGift GetGiftById(int id)
        {
            return _context.EgoGifts
                .Include(g => g.Sources)
                .FirstOrDefault(g => g.Id == id);
        }

        public void AddGift(EgoGift gift)
        {
            _context.EgoGifts.Add(gift);
            _context.SaveChanges();
        }

        public void UpdateGift(EgoGift gift)
        {
            _context.EgoGifts.Update(gift);
            _context.SaveChanges();
        }

        public void DeleteGift(int id)
        {
            var gift = _context.EgoGifts.Find(id);
            if (gift != null)
            {
                _context.EgoGifts.Remove(gift);
                _context.SaveChanges();
            }
        }

        //// Recipe operations 
        //public ObservableCollection<Recipe> GetAllRecipes()
        //{
        //    var recipes = _context.Recipes
        //        .OrderBy(r => r.Name)
        //        .ToList();
        //    return new ObservableCollection<Recipe>(recipes);
        //}
    }
}