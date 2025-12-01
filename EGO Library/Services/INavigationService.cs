using EGO_Library.Models;

namespace EGO_Library.Services
{
    public interface INavigationService
    {
        // основная навигация
        void NavigateToGiftList();
        void NavigateToRecipes();
        void NavigateToAbout();
        void NavigateToGiftDetail(EgoGift gift);
        void GoBack();

        // авторизация
        void NavigateToLogin();
        void NavigateToRegister();
    }
}