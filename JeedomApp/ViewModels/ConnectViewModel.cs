using JeedomApp.Services.SettingsServices;
using Jeedom;
using System;
using Template10.Mvvm;

namespace JeedomApp.ViewModels
{
    internal class ConnectViewModel : ViewModelBase
    {
        public static ConnectViewModel Instance { get; private set; }
        

        public ConnectViewModel()
        {
            Instance = this;
        }

        public string Host
        {
            get { return RequestViewModel.config.Host; }
            set { RequestViewModel.config.Host = value; }
        }
        public string Login
        {
            get { return RequestViewModel.config.Login; }
            set { RequestViewModel.config.Login = value; }
        }
        public string Password
        {
            get { return RequestViewModel.config.Password; }
            set { RequestViewModel.config.Password = value; }
        }
        public bool? TwoFactor
        {
            get { return RequestViewModel.config.TwoFactor; }
            set { RequestViewModel.config.TwoFactor = value; }
        }
        public bool? ConnexionAuto
        {
            get { return RequestViewModel.config.ConnexionAuto; }
            set { RequestViewModel.config.ConnexionAuto = value; }
        }
        public string TwoFactorCode
        {
            get { return RequestViewModel.config.TwoFactorCode; }
            set { RequestViewModel.config.TwoFactorCode = value; }
        }
    }
}