using Jeedom;
using Jeedom.Model;
using System.Collections.ObjectModel;
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

        public ObservableCollection<JdItem> FavoriteList
        {
            get
            {
                return RequestViewModel.Instance.FavoriteList;
            }
        }
    }
}