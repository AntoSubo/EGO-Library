using EGO_Library.Models;

namespace EGO_Library.Services
{
    public interface INavigationService
    {
        // Основная навигация
        void NavigateToGiftList();
        void NavigateToRecipes();
        void NavigateToAbout();
        void NavigateToGiftDetail(EgoGift gift);
        void GoBack();

        // НОВЫЕ МЕТОДЫ ДЛЯ АВТОРИЗАЦИИ
        void NavigateToLogin();
        void NavigateToRegister();
    }
}