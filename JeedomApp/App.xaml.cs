using Jeedom;
using JeedomApp.Controls;
using JeedomApp.Services.SettingsServices;
using JeedomApp.Views;
using System;
using System.Threading.Tasks;
using Template10.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;


namespace JeedomApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
            Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
            Microsoft.ApplicationInsights.WindowsCollectors.Session);
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            RequestedTheme = SettingsService.Instance.AppTheme;
            CacheMaxDuration = TimeSpan.FromDays(1);
            ShowShellBackButton = SettingsService.Instance.UseShellBackButton;

            #endregion App settings
        }

        // runs even if restored from state
        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // content may already be shell when resuming
            if ((Window.Current.Content as ModalDialog) == null)
            {
                // setup hamburger shell inside a modal dialog
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Shell(nav),
                    ModalContent = new Busy(),
                };
            }
            await Task.CompletedTask;
        }

        // runs only when not restored from state
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
           
            SettingsService.Instance.UseShellBackButton = true;

            // Ne rien mettre au dessus de ce code sinon Template10 fonctionne mal.
            NavigationService.Navigate(typeof(DashboardPage));

            if (RequestViewModel.config.Populated)
            {
                var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

                // Si pas d'APIKEY on lance le dialogue de connexion
                if (RequestViewModel.config.ApiKey == "")
                {
                    ConnectDialog.ShowConnectDialog();
                    return;
                }

                // Tentative de connexion à Jeedom
                if (await RequestViewModel.Instance.PingJeedom() == null)
                {
                    if (RequestViewModel.config.HostExt != "")
                        RequestViewModel.config.UseExtHost = true;
                }
                else
                {
                    ConnectDialog.ShowConnectDialog();
                    return;
                }

                await taskFactory.StartNew(async () =>
                {
                    await RequestViewModel.Instance.FirstLaunch();
                });

                //Lancer le dispatchertimer
                var _dispatcher = new DispatcherTimer();
                _dispatcher.Interval = TimeSpan.FromSeconds(5);
                _dispatcher.Tick += _dispatcher_Tick;
                _dispatcher.Start();
            }
            else
            {
                ConnectDialog.ShowConnectDialog();
            }

            await Task.CompletedTask;
        }

        private async void _dispatcher_Tick(object sender, object e)
        {
            //Shell.SetBusy(true, "Mise à jour");
            await RequestViewModel.Instance.UpdateTask();
            //await RequestViewModel.Instance.SynchMobilePlugin();
            //Shell.SetBusy(false);
        }
    }
}