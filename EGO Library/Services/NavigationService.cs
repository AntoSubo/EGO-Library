//using EGO_Library.Views;
//using System.Windows;

//namespace EGO_Library.Services
//{
//    public class NavigationService
//    {
//        private static MainWindow MainWindow => Application.Current.MainWindow as MainWindow;

//        public static RelayCommand NavigateToGiftListCommand { get; } = new RelayCommand(
//            _ => MainWindow?.NavigateToGiftList());

//        public static RelayCommand NavigateToRecipesCommand { get; } = new RelayCommand(
//            _ => MainWindow?.NavigateToRecipes());

//        public static RelayCommand NavigateToAboutCommand { get; } = new RelayCommand(
//            _ => MainWindow?.NavigateToAbout());

//        public static RelayCommand NavigateToGiftDetailCommand { get; } = new RelayCommand(
//            _ => MainWindow?.NavigateToGiftDetail());

//        // Методы для прямой навигации
//        public static void NavigateToGiftList()
//        {
//            MainWindow?.NavigateToGiftList();
//        }

//        public static void NavigateToRecipes()
//        {
//            MainWindow?.NavigateToRecipes();
//        }

//        public static void NavigateToAbout()
//        {
//            MainWindow?.NavigateToAbout();
//        }

//        //public static void NavigateToGiftDetail(object gift = null)
//        //{
//        //    MainWindow?.NavigateToGiftDetail(gift);
//        //}
//    }
//}