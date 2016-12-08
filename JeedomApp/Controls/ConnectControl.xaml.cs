using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Jeedom;
using System.Threading.Tasks;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace JeedomApp.Controls
{
    public sealed partial class ConnectControl : UserControl
    {
        public ConnectControl()
        {
            this.InitializeComponent();
        }
        private async void tbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            await taskFactory.StartNew(async () =>
            {
                await RequestViewModel.Instance.CheckTwoFactorConnexion();
                if (RequestViewModel.config.TwoFactor == true)
                    tbtwoFactorCode.Visibility = Visibility.Visible;
                else
                    tbtwoFactorCode.Visibility = Visibility.Collapsed;
            });

        }
       
    }
}
