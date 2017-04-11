using Jeedom;
using Jeedom.Model;
using Jeedom.Mvvm;
using JeedomApp.Controls;
using JeedomApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace JeedomApp.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class DashboardPage : Page
    {
        #region Private Fields

        private GridViewItem _eqLogicItemSelected;

        #endregion Private Fields

        #region Public Constructors

        public DashboardPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Permet de retouver le premier parent correspondant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;

            if (parent is T)
                return parent as T;
            else
                return FindParent<T>(parent);
        }

        public void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            // http://stackoverflow.com/questions/36741757/uwp-listview-item-context-menu

            // Cherche si on est bien sur une tuile, sinon on sort
            var item = FindParent<GridViewItem>(((FrameworkElement)e.OriginalSource));
            if (item == null)
                return;

            // Enregistre l'item sélectionné
            _eqLogicItemSelected = item;

            //Cherche la VariableSizedGridView
            var gridView = FindParent<VariableSizedGridView>(((FrameworkElement)e.OriginalSource));

            // Affiche le menu
            //eqLogicsMenuFlyout.ShowAt(gridView, e.GetPosition(gridView));
        }

        public void MenuFlyoutItem_ChangeSizeClick(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            var size = item.Tag as string;

            switch (size)
            {
                case "small":
                    VariableSizedWrapGrid.SetRowSpan(_eqLogicItemSelected, 1);
                    VariableSizedWrapGrid.SetColumnSpan(_eqLogicItemSelected, 1);
                    break;

                case "med":
                    VariableSizedWrapGrid.SetRowSpan(_eqLogicItemSelected, 2);
                    VariableSizedWrapGrid.SetColumnSpan(_eqLogicItemSelected, 2);
                    break;

                case "wide":
                    VariableSizedWrapGrid.SetRowSpan(_eqLogicItemSelected, 2);
                    VariableSizedWrapGrid.SetColumnSpan(_eqLogicItemSelected, 4);
                    break;

                case "extra-wide":
                    VariableSizedWrapGrid.SetRowSpan(_eqLogicItemSelected, 4);
                    VariableSizedWrapGrid.SetColumnSpan(_eqLogicItemSelected, 6);
                    break;

                case "large":
                    VariableSizedWrapGrid.SetRowSpan(_eqLogicItemSelected, 4);
                    VariableSizedWrapGrid.SetColumnSpan(_eqLogicItemSelected, 4);
                    break;

                case "extra-large":
                    VariableSizedWrapGrid.SetRowSpan(_eqLogicItemSelected, 6);
                    VariableSizedWrapGrid.SetColumnSpan(_eqLogicItemSelected, 6);
                    break;

                default:
                    break;
            }
            // Recharge la disposition de la WrapGrid
            VariableSizedWrapGrid vswGrid = VisualTreeHelper.GetParent(_eqLogicItemSelected) as VariableSizedWrapGrid;
            vswGrid.InvalidateMeasure();
        }

        #endregion Public Methods

        #region Private Methods

        public void MenuFlyoutItem_PinToStartClick(object sender, RoutedEventArgs e)
        {
        }

        #endregion Private Methods

        /*
        private async void MenuFlyoutItem_Click_Epingler(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = sender as MenuFlyoutItem;
                var id = item.Tag as string;
                JdObject objs = RequestViewModel.Instance.ObjectList.Where(o => o.id.Equals(id)).First();
                var TileExist = SecondaryTile.Exists(objs.id);
                if (!TileExist)
                {
                    var Tile = new SecondaryTile(objs.id)
                    {
                        DisplayName = objs.name,
                        Arguments = "Object",
                    };
                    var succes = await Tile.RequestCreateAsync();
                }
            }
            catch (Exception) { }
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }*/
    }
}