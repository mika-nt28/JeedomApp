using System;
using Windows.Storage;

namespace Jeedom
{
    public class ConfigurationViewModel
    {
        private string _host;
        private string _hostExt;
        private string _login;
        private string _password;
        private bool? _twoFactor;
        private bool? _connexionAuto;
        private string _twoFactorCode;
        private string _apikey;

        public Uri Uri
        {
            get
            {
                if(_host.indexOf("http")>-1)
                    var uri = new UriBuilder("http", _host, 80);
                else
                    uri = new UriBuilder(_host, 80);
                return uri.Uri;
            }
        }        
        public Uri UriExt
        {
            get
            {
                var uri = new UriBuilder(_hostExt);
                return uri.Uri;
            }
        }

        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;
                RoamingSettings.Values[settingLogin] = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }
        public bool? ConnexionAuto
        {
            get
            {
                return _connexionAuto;
            }

            set
            {
                _connexionAuto = value;
                RoamingSettings.Values[settingConnexionAuto] = value;
            }
        }
        public bool? TwoFactor
        {
            get
            {
                return _twoFactor;
            }

            set
            {
                _twoFactor = value;
                RoamingSettings.Values[settingTwoFactor] = value;
            }
        }

        public string TwoFactorCode
        {
            get
            {
                return _twoFactorCode;
            }

            set
            {
                _twoFactorCode = value;
            }
        }

       public string Host
        {
            set
            {
                _host = value;
                RoamingSettings.Values[settingHost] = value;
            }

            get
            {
                return _host;
            }
        }
       public string HostExt
        {
            set
            {
                _hostExt = value;
                RoamingSettings.Values[settingHostExt] = value;
            }

            get
            {
                return _hostExt;
            }
        }

        public string ApiKey
        {
            set
            {
                if (value != null)
                {
                    _apikey = value;
                    RoamingSettings.Values[settingAPIKey] = value;
                }
            }
            get
            {
                return _apikey;
            }
        }

                public bool Populated = false;
        private bool _GeolocActivation;

        public bool GeolocActivation
        {
            set
            {
                _GeolocActivation = value;
                LocalSettings.Values["GeolocActivation"] = value;
            }

            get
            {
                return _GeolocActivation;
            }
        }

        private bool _GeoFenceActivation;

        public bool GeoFenceActivation
        {
            set
            {
                _GeoFenceActivation = value;
                LocalSettings.Values["GeoFenceActivation"] = value;
            }

            get
            {
                return _GeoFenceActivation;
            }
        }

        private string _GeoFenceActivationDistance;

        public string GeoFenceActivationDistance
        {
            set
            {
                _GeoFenceActivationDistance = value;
                LocalSettings.Values["GeoFenceActivationDistance"] = value;
            }

            get
            {
                return _GeoFenceActivationDistance;
            }
        }

        private bool _NotificationActivation;

        public bool NotificationActivation
        {
            set
            {
                _NotificationActivation = value;
                LocalSettings.Values["NotificationActivation"] = value;
            }

            get
            {
                return _NotificationActivation;
            }
        }

        private string _GeolocObjectId;

        public string GeolocObjectId
        {
            set
            {
                _GeolocObjectId = value;
                LocalSettings.Values["GeolocObjectId"] = value;
            }

            get
            {
                return _GeolocObjectId;
            }
        }

        private string _NotificationObjectId;

        public string NotificationObjectId
        {
            set
            {
                _NotificationObjectId = value;
                LocalSettings.Values["NotificationObjectId"] = value;
            }

            get
            {
                return _NotificationObjectId;
            }
        }

        private ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;
        private ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        private const string settingHost = "addressSetting";
        private const string settingHostExt = "addressExtSetting";
        private const string settingLogin = "LoginSetting";
        private const string settingTwoFactor = "TwoFactorSetting";
        private const string settingConnexionAuto = "ConnexionAutoSetting";
        private const string settingAPIKey = "apikeySetting";

        public ConfigurationViewModel()
        {
            Populated = true;

            _host = RoamingSettings.Values[settingHost] as string;
            if (_host == null)
                Populated = false;
            _hostExt = RoamingSettings.Values[settingHostExt] as string;
            if (_hostExt == null)
                Populated = false;
            _login = RoamingSettings.Values[settingLogin] as string;
            if (_login == null)
                Populated = false;
            if (RoamingSettings.Values[settingTwoFactor] != null)
                _twoFactor = Convert.ToBoolean(RoamingSettings.Values[settingTwoFactor]);
            if (RoamingSettings.Values[settingConnexionAuto] != null)
                _twoFactor = Convert.ToBoolean(RoamingSettings.Values[settingConnexionAuto]);

            _apikey = RoamingSettings.Values[settingAPIKey] as string;
            if (_apikey == null)
                Populated = false;

            _GeolocActivation = (LocalSettings.Values["GeolocActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["GeolocActivation"]);
            _GeoFenceActivation = (LocalSettings.Values["GeoFenceActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["GeoFenceActivation"]);
            _NotificationActivation = (LocalSettings.Values["NotificationActivation"] == null) ? false : Convert.ToBoolean(LocalSettings.Values["NotificationActivation"]);

            _GeolocObjectId = (LocalSettings.Values["GeolocObjectId"] == null) ? "" : LocalSettings.Values["GeolocObjectId"].ToString();
            _NotificationObjectId = (LocalSettings.Values["NotificationObjectId"] == null) ? "" : LocalSettings.Values["NotificationObjectId"].ToString();
            _GeoFenceActivationDistance = (LocalSettings.Values["GeoFenceActivationDistance"] == null) ? "" : LocalSettings.Values["GeoFenceActivationDistance"].ToString();
        }
    }
}
