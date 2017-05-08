using Jeedom;
using Jeedom.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace JeedomApp.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class ScenePage : Page
    {
        public ScenePage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private async void gridview_ItemClick(object sender, ItemClickEventArgs e)
        {
            Scene scene = e.ClickedItem as Scene;
            scene.Updating = true;
            await RequestViewModel.Instance.RunScene(scene);
            scene.Updating = false;
        }

        private void Scene_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var panel = sender as Grid;
            var flyout = panel.Resources["SceneFlyout"] as MenuFlyout;
            flyout.ShowAt(panel, e.GetPosition(panel));
        }

        private void AddToFavorite_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            string id = item.Tag as string;
            var lst = from sc in RequestViewModel.Instance.SceneList where sc.Id == id select sc;
            if (lst.Count() != 0)
            {
                var sc = lst.First();
                RequestViewModel.Instance.AddToFavorite(sc);
            }
        }
    }
}