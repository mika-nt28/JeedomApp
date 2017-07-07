﻿using Jeedom.Api.Http;
using Jeedom.Api.Json;
using Jeedom.Api.Json.Event;
using Jeedom.Api.Json.Response;
using Jeedom.Model;
using Jeedom.Mvvm;
using Jeedom.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Networking.PushNotifications;

namespace Jeedom
{
    public partial class RequestViewModel
    {
        public void AddToFavorite(EqLogic eq)
        {
            if (!FavoriteList.Contains(eq))
            {
                FavoriteList.Add(eq);
                _favoriteIdList.Add("eq_" + eq.Id);
                config.FavoriteList = _favoriteIdList;
            }
        }

        public void AddToFavorite(Scene sc)
        {
            if (!FavoriteList.Contains(sc))
            {
                FavoriteList.Add(sc);
                _favoriteIdList.Add("sc_" + sc.Id);
                config.FavoriteList = _favoriteIdList;
            }
        }

        public async Task<Error> CheckTwoFactorConnexion()
        {
            Parameters parameters = new Parameters();
            parameters.login = config.Login;
            var jsonrpc = new JsonRpcClient(parameters);

            if (await jsonrpc.SendRequest("user::useTwoFactorAuthentification"))
            {
                var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                if (reponse.result == "1")
                    config.TwoFactor = true;
                else
                    config.TwoFactor = false;
            }

            return jsonrpc.Error;
        }

        public async Task<Error> ConnectJeedomByLogin()
        {
            Parameters parameters = new Parameters();
            parameters.login = config.Login;
            parameters.password = config.Password;
            //if (config.TwoFactor == true)
            //    parameters.twoFactorCode = config.TwoFactorCode;
            var jsonrpc = new JsonRpcClient(parameters);

            //LoadingMessage = "Tentative de connexion en https";
            config.Address.ProtocolType = Address.Protocol.Https;
            if (await jsonrpc.SendRequest("user::getHash"))
            {
                var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                config.ApiKey = reponse.result;
                return jsonrpc.Error;
            }

            //LoadingMessage = "Tentative de connexion en https en ignorant les erreurs de certificat";
            config.Address.ProtocolType = Address.Protocol.SelfSigned;
            if (await jsonrpc.SendRequest("user::getHash"))
            {
                var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                config.ApiKey = reponse.result;
                return jsonrpc.Error;
            }

            //LoadingMessage = "Tentative de connexion en http";
            config.Address.ProtocolType = Address.Protocol.Http;
            if (await jsonrpc.SendRequest("user::getHash"))
            {
                var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                config.ApiKey = reponse.result;
            }
            // Par sécurité on retourne sur https
            config.Address.ProtocolType = Address.Protocol.Https;
            return jsonrpc.Error;
        }

        public async Task<Error> CreateEqLogicMobile()
        {
            Parameters parameters = new Parameters();
            parameters.plugin = "mobile";
            parameters.platform = "windows";
            var jsonrpc = new JsonRpcClient(parameters);
            if (await jsonrpc.SendRequest("Iq"))
            {
                // Récupère la liste de tous les eqLogics
                var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                if (reponse != null)
                {
                    config.IdMobile = reponse.result;
                }
            }

            return jsonrpc.Error;
        }

        public async Task<Error> DownloadInteraction()
        {
            var jsonrpc = new JsonRpcClient();
            //Ajouter le téléchargemnent et la mise a jours des interaction Jeedom
            try
            {
                var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///JeedomAppVoiceCommandes.xml"));
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);

                VoiceCommandDefinition commandDefinitions;

                string countryCode = CultureInfo.CurrentCulture.Name.ToLower();
                if (countryCode.Length == 0)
                {
                    countryCode = "fr-fr";
                }

                if (VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue("JeedomAppCommandSet_" + countryCode, out commandDefinitions))
                {
                    List<string> InteractsList = new List<string>();
                    if (await jsonrpc.SendRequest("interactQuery::all"))
                    {
                        InteractList.Clear();
                        var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<Interact>>>();
                        if (response != null)
                            InteractList = response.result;
                    }

                    foreach (var Iteract in InteractList)
                    {
                        InteractsList.Add(Iteract.query);
                    }
                    await commandDefinitions.SetPhraseListAsync("InteractList", InteractsList);
                }
            }
            catch (Exception ex)
            {
                // System.Diagnostics.Debug.WriteLine("Updating Phrase list for VCDs: " + ex.ToString());
            }

            return jsonrpc.Error;
        }

        public async Task<Error> DownloadMessages()
        {
            var jsonrpc = new JsonRpcClient();

            if (await jsonrpc.SendRequest("message::all"))
            {
                MessageList.Clear();
                var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<Message>>>();
                if (response != null)
                    MessageList = response.result;
            }

            return jsonrpc.Error;
        }

        public async Task<Error> DownloadObjects()
        {
            var jsonrpc = new JsonRpcClient();

            if (await jsonrpc.SendRequest("object::all"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<JdObject>>>();
                foreach (JdObject obj in response.result)
                {
                    var lst = from o in ObjectList where o.Id == obj.Id select o;
                    if (lst.Count() != 0)
                    {
                        var ob = lst.FirstOrDefault();
                        ob = obj;
                    }
                    else
                        ObjectList.Add(obj);
                }
            }
            return jsonrpc.Error;
        }

        public async Task<Error> DownloadScenes()
        {
            var jsonrpc = new JsonRpcClient();

            if (await jsonrpc.SendRequest("scenario::all"))
            {
                SceneList.Clear();
                var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<Scene>>>();
                if (response != null)
                    SceneList = response.result;
            }

            return jsonrpc.Error;
        }

        public async Task<Error> DownloadVersion()
        {
            var jsonrpc = new JsonRpcClient();
            if (await jsonrpc.SendRequest("version"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                Version = response.result;
            }

            return jsonrpc.Error;
        }

        public async Task ExecuteCommand(Command cmd, Parameters parameters = null)
        {
            if (config.IsDemoEnabled)
                return;

            cmd.Updating = true;
            if (parameters == null)
            {
                parameters = new Parameters();
                parameters.id = cmd.Id;
                parameters.name = cmd.Name;
            }
            var jsonrpc = new JsonRpcClient(parameters);

            if (await jsonrpc.SendRequest("cmd::execCmd"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<CommandResult>>();
                cmd.Value = response.result.value;
            }
            else
            {
                cmd.Value = "N/A";
            }
            cmd.Updating = false;
        }

        public async Task FirstLaunch()
        {
            Updating = true;

            int pg = 100 / 8;

            LoadingMessage = "Inscription au notification";
            if (config.NotificationActivation == true)
                UpdateNotificationChannel();

            LoadingMessage = "Chargement de la Version";
            var error = await DownloadVersion();
            Progress += pg;

            LoadingMessage = "Chargement des Objets";
            error = await DownloadObjects();
            Progress += pg;

            LoadingMessage = "Chargement des Equipements";
            error = await SynchMobilePlugin();
            Progress += pg;

            LoadingMessage = "Chargement des Scénarios";
            error = await DownloadScenes();
            Progress += pg;

            LoadingMessage = "Chargement des Messages";
            error = await DownloadMessages();
            Progress += pg;

            //LoadingMessage = "Chargement des informations des Commandes";
            //error = await GetEventChanges();
            //Progress += pg;

            LoadingMessage = "Chargement des Interactions";
            await DownloadInteraction();
            Progress += pg;

            LoadingMessage = "Prêt";
            Updating = false;
        }

        public async Task<Error> GetEventChanges()
        {
            var parameters = new Parameters();
            parameters.datetime = _dateTime;
            var jsonrpc = new JsonRpcClient(parameters);

            if (await jsonrpc.SendRequest("event::changes"))
            {
                var response = jsonrpc.GetEvents();
                foreach (JdEvent e in response.Result)
                {
                    switch (e.Name)
                    {
                        case "cmd::update":
                            var ev = e as Event<EventOptionCmd>;
                            var cmd = (from c in CommandList where c.Id == ev.Option.CmdId select c).FirstOrDefault();
                            if (cmd != null)
                            {
                                if (cmd.DateTime < ev.DateTime)
                                {
                                    cmd.Value = ev.Option.Value;
                                    cmd.DateTime = ev.DateTime;
                                }
                            }
                            break;

                        case "eqLogic::update":
                            var eveq = e as Event<EventOptionEqLogic>;
                            var eq = (from c in EqLogicList where c.Id == eveq.Option.EqLogicId select c).FirstOrDefault();

                            if (eq != null)
                            {
                                if (eq.DateTime < eveq.DateTime)
                                {
                                    await UpdateEqLogic(eq);
                                    eq.DateTime = eveq.DateTime;
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
                _dateTime = response.DateTime;
            }

            return jsonrpc.Error;
        }

        public async Task<Error> interactTryToReply(string query)
        {
            InteractReply = "";
            var jsonrpc = new JsonRpcClient();
            Parameters parameters = new Parameters();
            parameters.query = query;
            jsonrpc.SetParameters(parameters);
            if (await jsonrpc.SendRequest("interact::tryToReply"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                if (response != null)
                    InteractReply = response.result;
            }

            return jsonrpc.Error;
        }

        public void LaunchDemo()
        {
            Updating = true;
            // Active le mode démo
            config.SetDemoMode();

            // Charger les données json
            Version = "Démo";
            LoadingMessage = "Chargement des Objets";
            var assembly = GetType().GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("Jeedom.Demo.object.json");
            var sr = new StreamReader(stream);
            ObjectList = JsonConvert.DeserializeObject<ObservableCollection<JdObject>>(sr.ReadToEnd());

            // Populate EqlogicList and CmdList
            EqLogicList.Clear();
            CommandList.Clear();
            foreach (var obj in ObjectList)
            {
                foreach (var eq in obj.EqLogics)
                {
                    EqLogicList.Add(eq);
                    foreach (var cmd in eq.Cmds)
                    {
                        CommandList.Add(cmd);
                    }
                }
            }

            LoadingMessage = "Chargement des Scénarios";
            stream = assembly.GetManifestResourceStream("Jeedom.Demo.scenario.json");
            sr = new StreamReader(stream);
            SceneList = JsonConvert.DeserializeObject<ObservableCollection<Scene>>(sr.ReadToEnd());

            LoadingMessage = "Chargement des Messages";
            stream = assembly.GetManifestResourceStream("Jeedom.Demo.message.json");
            sr = new StreamReader(stream);
            MessageList = JsonConvert.DeserializeObject<ObservableCollection<Message>>(sr.ReadToEnd());

            // Mise à jour des favoris
            PopulateFavoriteList();

            LoadingMessage = "Prêt";
            Updating = false;
        }

        public async Task<Error> PingJeedom()
        {
            Updating = true;
            LoadingMessage = "Contacte Jeedom";
            var jsonrpc = new JsonRpcClient();
            //LoadingMessage = "Tentative de connexion en https";
            config.Address.ProtocolType = Address.Protocol.Https;
            if (await jsonrpc.SendRequest("ping"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                if (response.result == "pong")
                {
                    Updating = false;
                    return null;
                }
            }

            //LoadingMessage = "Tentative de connexion en https en ignorant les erreurs de certificat";
            config.Address.ProtocolType = Address.Protocol.SelfSigned;
            if (await jsonrpc.SendRequest("ping"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                if (response.result == "pong")
                {
                    Updating = false;
                    return null;
                }
            }

            //LoadingMessage = "Tentative de connexion en http";
            config.Address.ProtocolType = Address.Protocol.Http;
            if (await jsonrpc.SendRequest("ping"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                if (response.result == "pong")
                {
                    Updating = false;
                    return null;
                }
            }
            // Par sécurité on retourne sur https
            config.Address.ProtocolType = Address.Protocol.Https;
            Updating = false;
            return jsonrpc.Error;
        }

        public void PopulateFavoriteList()
        {
            FavoriteList.Clear();
            _favoriteIdList = config.FavoriteList;
            foreach (string id in _favoriteIdList)
            {
                var idx = id.Substring(3);
                if (id.StartsWith("eq_"))
                {
                    var lst = from e in EqLogicList where e.Id == idx select e;
                    if (lst.Count() != 0)
                    {
                        var eq = lst.First();
                        FavoriteList.Add(eq);
                    }
                    else
                        _favoriteIdList.Remove(id);
                }
                else if (id.StartsWith("sc_"))
                {
                    var lst = from s in SceneList where s.Id == idx select s;
                    if (lst.Count() != 0)
                    {
                        var sc = lst.First();
                        FavoriteList.Add(sc);
                    }
                    else
                        _favoriteIdList.Remove(id);
                }
                else
                {
                    _favoriteIdList.Remove(id);
                }
            }
            config.FavoriteList = _favoriteIdList;
        }

        public async Task<bool> Reboot()
        {
            if (config.IsDemoEnabled)
                return true;

            var jsonrpc = new JsonRpcClient();

            await jsonrpc.SendRequest("jeeNetwork::reboot");

            if (jsonrpc.Error == null)
                return true;
            else
                return false;
        }

        public void RemoveFromFavorite(EqLogic eq)
        {
            FavoriteList.Remove(eq);
            _favoriteIdList.Remove("eq_" + eq.Id);
            config.FavoriteList = _favoriteIdList;
        }

        public void RemoveFromFavorite(Scene sc)
        {
            FavoriteList.Remove(sc);
            _favoriteIdList.Remove("sc_" + sc.Id);
            config.FavoriteList = _favoriteIdList;
        }

        public async Task RunScene(Scene scene)
        {
            if (config.IsDemoEnabled)
                return;

            var parameters = new Parameters();
            parameters.id = scene.Id;
            parameters.state = "run";
            var jsonrpc = new JsonRpcClient(parameters);

            if (await jsonrpc.SendRequest("scenario::changeState"))
            {
                await UpdateScene(scene);
            }
        }

        public async Task<Error> SearchConfigByKey(string key, string plugin)
        {
            Parameters parameters = new Parameters();
            parameters.key = key;
            parameters.plugin = plugin;
            var jsonrpc = new JsonRpcClient(parameters);

            if (await jsonrpc.SendRequest("config::byKey"))
            {
                configByKey = jsonrpc.GetRequestResponseDeserialized<Response<string>>().result;
            }

            return jsonrpc.Error;
        }

        public async Task<Error> SendNotificationUri(string uri)
        {
            Parameters parameters = new Parameters();
            parameters.plugin = "pushNotification";
            parameters.platform = "windows";
            parameters.query = uri;
            if (config.IdPush != null)
                parameters.id = config.IdPush;
            var jsonrpc = new JsonRpcClient(parameters);
            if (await jsonrpc.SendRequest("Iq"))
            {
                // Récupère la liste de tous les eqLogics
                var reponse = jsonrpc.GetRequestResponseDeserialized<Response<string>>();
                if (reponse != null)
                {
                    config.IdPush = reponse.result;
                }
            }

            return jsonrpc.Error;
        }

        public async Task<bool> SendPosition(string position)
        {
            var httpRpcClient = new HttpRpcClient("/core/api/jeeApi.php?api=" + config.ApiKey + "&type=geoloc&id=" + config.GeolocObjectId + "&value=" + position);

            return await httpRpcClient.SendRequest();
        }

        public async Task<bool> Shutdown()
        {
            if (config.IsDemoEnabled)
                return true;

            var jsonrpc = new JsonRpcClient();

            await jsonrpc.SendRequest("jeeNetwork::halt");

            if (jsonrpc.Error == null)
                return true;
            else
                return false;
        }

        public async Task<Error> SynchMobilePlugin()
        {
            var jsonrpc = new JsonRpcClient();
            Parameters parameters = new Parameters();
            parameters.plugin = "mobile";
            jsonrpc.SetParameters(parameters);
            EqLogicList.Clear();
            CommandList.Clear();

            if (await jsonrpc.SendRequest("sync"))
            {
                // Récupère la liste de tous les eqLogics
                var EqLogics = jsonrpc.GetRequestResponseDeserialized<Response<JdObject>>();
                if (EqLogics != null)
                {
                    foreach (EqLogic eq in EqLogics.result.EqLogics)
                    {
                        EqLogicList.Add(eq);
                    }
                }

                // Récupère la liste de toutes les cmds
                var Cmds = jsonrpc.GetRequestResponseDeserialized<Response<EqLogic>>();
                if (Cmds.result.Cmds != null)
                {
                    foreach (Command cmd in Cmds.result.Cmds)
                    {
                        // AJoute la cmd à son eqLogic
                        if (EqLogicList.Where(o => o.Id.Equals(cmd.EqLogic_id)).FirstOrDefault().Cmds == null)
                            EqLogicList.Where(o => o.Id.Equals(cmd.EqLogic_id)).FirstOrDefault().Cmds = new ObservableCollectionEx<Command>();
                        EqLogicList.Where(o => o.Id.Equals(cmd.EqLogic_id)).FirstOrDefault().Cmds.Add(cmd);

                        // Ajoute la commande à la liste globale des cmds
                        CommandList.Add(cmd);
                    }
                }

                // Suppression des eqLogics sans cmds
                for (int i = EqLogicList.Count - 1; i >= 0; i--)
                {
                    if (EqLogicList[i].Cmds == null)
                        EqLogicList.RemoveAt(i);
                }

                // Affecte les eqLogics à leurs objects correspondants
                foreach (EqLogic eq in EqLogicList)
                {
                    if (ObjectList.Where(o => o.Id.Equals(eq.ObjectId)).FirstOrDefault().EqLogics == null)
                        ObjectList.Where(o => o.Id.Equals(eq.ObjectId)).FirstOrDefault().EqLogics = new ObservableCollectionEx<EqLogic>();
                    ObjectList.Where(o => o.Id.Equals(eq.ObjectId)).FirstOrDefault().EqLogics.Add(eq);
                }

                // Suppression des objects sans eqLogics
                for (int i = ObjectList.Count - 1; i >= 0; i--)
                {
                    if (ObjectList[i].EqLogics == null)
                        ObjectList.RemoveAt(i);
                }

                // Mise à jour des favoris
                PopulateFavoriteList();
            }

            return jsonrpc.Error;
        }

        public async Task UpdateEqLogic(EqLogic eq)
        {
            if (eq.Cmds == null)
                return;

            var infoCmds = (from cmd in eq.Cmds where cmd.Type == "info" select cmd).DefaultIfEmpty();
            if (infoCmds.Count() > 0)
            {
                foreach (Command cmd in infoCmds)
                {
                    if (cmd != null)
                    {
                        if (!cmd.Updating)
                        {
                            await ExecuteCommand(cmd);
                            cmd.DateTime = _dateTime;
                        }
                    }
                }
            }

            eq.DateTime = _dateTime;
        }

        public async void UpdateNotificationChannel()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            await SendNotificationUri(channel.Uri.ToString());
        }

        /*public async Task UpdateObject(JdObject obj)
        {
            var parameters = new Parameters();
            parameters.object_id = obj.Id;
            var jsonrpc = new JsonRpcClient(parameters);

            if (await jsonrpc.SendRequest("eqLogic::byObjectId"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<EqLogic>>>();
                if (response.result != null)
                {
                    foreach (EqLogic eqnew in response.result)
                    {
                        var lst = from e in EqLogicList where e.Id == eqnew.Id select e;
                        if (lst.Count() != 0)
                        {
                            var eqold = lst.FirstOrDefault();
                            eqnew.Cmds = eqold.Cmds;
                            eqold = eqnew;
                        }
                        else
                        {
                            EqLogicList.Add(eqnew);
                            obj.EqLogics.Add(eqnew);
                        }
                        await UpdateEqLogic(eqnew);
                    }
                }
            }
        }*/

        public async Task UpdateObjectList()
        {
            var jsonrpc = new JsonRpcClient();

            if (await jsonrpc.SendRequest("object::all"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<ObservableCollection<JdObject>>>();
                foreach (JdObject obj in response.result)
                {
                    var lst = from o in ObjectList where o.Id == obj.Id select o;
                    if (lst.Count() != 0)
                    {
                        var ob = lst.FirstOrDefault();
                        ob = obj;
                    }
                    else
                        ObjectList.Add(obj);
                }
            }
        }

        public async Task UpdateTask()
        {
            // Vérifie qu'on ne se trouve pas en mode demo
            if (config.IsDemoEnabled)
                return;

            //Vérifier que l'API Key est dispo avant de lancer l'update
            if (!config.Populated)
                return;

            // SI on est déjà en mise à jour on sort
            if (Updating)
                return;

            Updating = true;

            if (ObjectList.Count == 0)
            {
                LoadingMessage = "Contacte Jeedom";
                await DownloadDateTime();
                LoadingMessage = "Chargement des Objets";
                await DownloadObjects();
            }
            else
            {
                LoadingMessage = "Chargements des evènements";
                await GetEventChanges();
            }

            if (pass % 15 == 14)
            {
                LoadingMessage = "Chargement des Messages";
                await DownloadMessages();
            }

            // Met à jour les favoris
            PopulateFavoriteList();

            LoadingMessage = "Prêt";
            Updating = false;
        }

        public async Task<bool> Upgrade()
        {
            if (config.IsDemoEnabled)
                return true;

            var jsonrpc = new JsonRpcClient();

            await jsonrpc.SendRequest("update::update");

            if (jsonrpc.Error == null)
                return true;
            else
                return false;
        }

        private async Task<Error> DownloadDateTime()
        {
            var jsonrpc = new JsonRpcClient();

            if (await jsonrpc.SendRequest("datetime"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<double>>();
                _dateTime = response.result;
            }

            return jsonrpc.Error;
        }

        /*public async Task<bool> SendNotificationUri(string uri)
        {
            var httpRpcClient = new HttpRpcClient("/plugins/pushNotification/php/updatUri.php?api=" + config.ApiKey + "&id=" + config.NotificationObjectId + "&uri=" + uri);

            return await httpRpcClient.SendRequest();
        }*/

        private async Task UpdateScene(Scene scene)
        {
            var parameters = new Parameters();
            parameters.id = scene.Id;
            var jsonrpc = new JsonRpcClient(parameters);

            if (await jsonrpc.SendRequest("scenario::byId"))
            {
                var response = jsonrpc.GetRequestResponseDeserialized<Response<Scene>>();
                scene.lastLaunch = response.result.lastLaunch;
            }
        }
    }
}