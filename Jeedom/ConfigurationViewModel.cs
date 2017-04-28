using Jeedom.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.Storage;

namespace Jeedom
{
    public partial class ConfigurationViewModel
    {
        public bool Populated = false;
        private const string settingAPIKey = "apikeySetting";
        private const string settingConnexionAuto = "ConnexionAutoSetting";
        private const string settingFavoriteList = "FavoriteListSetting";
        private const string settingHost = "addressSetting";
        private const string settingHostExt = "addressExtSetting";
        private const string settingIdMobile = "IdMobileSetting";
        private const string settingIdPush = "IdPushSetting";
        private const string settingLogin = "LoginSetting";
        private const string settingPassword = "PasswordSetting";
        private const string settingTwoFactor = "TwoFactorSetting";
        private Address _address = new Address();
        private string _apikey;
        private bool? _connexionAuto;
        private List<string> _favoriteList;
        private bool _GeoFenceActivation;
        private string _GeoFenceActivationDistance;
        private bool _GeolocActivation;
        private string _GeolocObjectId;
        private string _idMobile;
        private string _idPush;
        private string _login;
        private bool _NotificationActivation;
        private string _NotificationObjectId;
        private string _password;
        private bool? _twoFactor;
        private string _twoFactorCode;
        private bool _useExtHost = false;
        private ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        private ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;

        public ConfigurationViewModel()
        {
            Address.Link = RoamingSettings.Values[settingHost] as string;
            _login = RoamingSettings.Values[settingLogin] as string;
            _password = RoamingSettings.Values[settingPassword] as string;

            if (LocalSettings.Values[settingConnexionAuto] != null)
                _connexionAuto = Convert.ToBoolean(LocalSettings.Values[settingConnexionAuto]);
            else
            {
                ConnexionAuto = false;
            }

            _apikey = RoamingSettings.Values[settingAPIKey] as string;

            //Populated si API Key et host disponible
            TestPopulated();

            _idMobile = LocalSettings.Values[settingIdMobile] as string;
            _idPush = LocalSettings.Values[settingIdPush] as string;

            _GeolocActivation = (LocalSettings.Values["GeolocActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["GeolocActivation"]);
            _GeoFenceActivation = (LocalSettings.Values["GeoFenceActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["GeoFenceActivation"]);
            _NotificationActivation = (LocalSettings.Values["NotificationActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["NotificationActivation"]);

            _GeolocObjectId = (LocalSettings.Values["GeolocObjectId"] == null) ? "" : LocalSettings.Values["GeolocObjectId"].ToString();
            _NotificationObjectId = (LocalSettings.Values["NotificationObjectId"] == null) ? "" : LocalSettings.Values["NotificationObjectId"].ToString();
            _GeoFenceActivationDistance = (LocalSettings.Values["GeoFenceActivationDistance"] == null) ? "" : LocalSettings.Values["GeoFenceActivationDistance"].ToString();

            var fav = LocalSettings.Values[settingFavoriteList] as string;
            if (!String.IsNullOrWhiteSpace(fav))
                _favoriteList = JsonConvert.DeserializeObject<List<string>>(fav);
            else
                FavoriteList = new List<string>();
        }
    }
}