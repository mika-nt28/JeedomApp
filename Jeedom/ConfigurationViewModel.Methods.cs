using System;
using System.Collections.Generic;

namespace Jeedom
{
    public partial class ConfigurationViewModel
    {
        /// <summary>
        /// Supprime tous les paramètres
        /// </summary>
        public void Reset()
        {
            this.ApiKey = "";
            this.ConnexionAuto = true;
            this.GeoFenceActivation = false;
            this.GeoFenceActivationDistance = "";
            this.GeolocActivation = false;
            this.GeolocObjectId = "";
            this.Host = "";
            this.Login = "";
            this.NotificationActivation = false;
            this.NotificationObjectId = "";
            this.Password = "";
            this.TwoFactor = false;
            this.TwoFactorCode = "";
            this.FavoriteList = new List<string>();
        }

        /// <summary>
        /// Active le mode démo
        /// </summary>
        public void SetDemoMode()
        {
            this.ApiKey = "demo";
            this.ConnexionAuto = true;
            this.GeoFenceActivation = false;
            this.GeoFenceActivationDistance = "";
            this.GeolocActivation = false;
            this.GeolocObjectId = "";
            this.Host = "demo";
            this.Login = "demo";
            this.NotificationActivation = false;
            this.NotificationObjectId = "";
            this.Password = "demo";
            this.TwoFactor = false;
            this.TwoFactorCode = "";
        }

        /// <summary>
        /// Vérifie que la configuration est entièrement peuplée (api key et host disponible)
        /// </summary>
        private void TestPopulated()
        {
            if (String.IsNullOrWhiteSpace(Address.Link) || String.IsNullOrWhiteSpace(_apikey))
                Populated = false;
            else
                Populated = true;
        }
    }
}