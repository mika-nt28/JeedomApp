using Jeedom.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeedom
{
    public partial class ConfigurationViewModel
    {
        public string ApiKey
        {
            set
            {
                if (value != null)
                {
                    _apikey = value;
                    TestPopulated();
                    RoamingSettings.Values[settingAPIKey] = value;
                }
            }
            get
            {
                return _apikey;
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
                LocalSettings.Values[settingConnexionAuto] = value;
            }
        }

        public List<string> FavoriteList
        {
            get { return _favoriteList; }
            set
            {
                _favoriteList = value;
                LocalSettings.Values[settingFavoriteList] = JsonConvert.SerializeObject(value);
            }
        }

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

        public string Host
        {
            set
            {
                Address.Link = value;

                TestPopulated();
                RoamingSettings.Values[settingHost] = Address.Link;
            }

            get
            {
                return Address.Link;
            }
        }

        public string IdMobile
        {
            set
            {
                if (value != null)
                {
                    _idMobile = value;
                    LocalSettings.Values[settingIdMobile] = value;
                }
            }
            get
            {
                return _idMobile;
            }
        }

        public string IdPush
        {
            set
            {
                if (value != null)
                {
                    _idPush = value;
                    LocalSettings.Values[settingIdPush] = value;
                }
            }
            get
            {
                return _idPush;
            }
        }

        public bool IsDemoEnabled
        {
            get
            {
                if (Login == "demo" && Password == "demo" && ApiKey == "demo")
                    return true;
                else
                    return false;
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

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                byte[] bytes = Encoding.GetEncoding(0).GetBytes(value);
                _password = Encoding.UTF8.GetString(bytes);
                RoamingSettings.Values[settingPassword] = _password;
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

        /// <summary>
        /// URI d'accès au serveur JEEDOM
        /// </summary>
        public Uri Uri
        {
            get
            {
                return Address.Uri;
            }
        }

        public bool UseExtHost
        {
            get
            {
                return _useExtHost;
            }

            set
            {
                _useExtHost = value;
            }
        }

        internal Address Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }
    }
}