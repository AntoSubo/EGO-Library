using System.Windows;
using System.Windows.Controls;

namespace EGO_Library.Services
{
    public class NavigationService
    {
        private Frame _mainFrame;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        // Добавь этот конструктор для MainViewModel
        public NavigationService()
        {
            // Для использования без Frame
        }

        public void NavigateToPage(Page page)
        {
            _mainFrame?.Navigate(page);
        }

        public void Navigate(object viewModel)
        {
            // Заглушка - в реальности здесь должна быть логика навигации
        }

        public void GoBack()
        {
            if (_mainFrame?.CanGoBack == true)
                _mainFrame.GoBack();
        }
    }
}