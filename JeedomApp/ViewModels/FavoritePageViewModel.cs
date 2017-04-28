using Jeedom;
using Jeedom.Tools;
using Template10.Mvvm;

namespace JeedomApp.ViewModels
{
    internal class FavoritePageViewModel : ViewModelBase
    {
        public FavoritePageViewModel()
        {
            Instance = this;
        }

        public static FavoritePageViewModel Instance
        {
            get;
            private set;
        }

        public IdFavoriteItemList FavoriteList
        {
            get
            {
                return RequestViewModel.Instance.FavoriteList;
            }
        }
    }
}