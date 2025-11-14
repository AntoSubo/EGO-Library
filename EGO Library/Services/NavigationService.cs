using System;
using EGO_Library.ViewModels;

namespace EGO_Library.Services
{
    public class NavigationService
    {
        private readonly Action<BaseViewModel> _navigate;

        public NavigationService(Action<BaseViewModel> navigate)
        {
            _navigate = navigate;
        }

        public void Navigate(BaseViewModel viewModel)
        {
            _navigate(viewModel);
        }
    }
}
