using Jeedom.Model;
using Jeedom.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace Jeedom
{
    // TODO: convertir le singleton en class threadsafe avec accès sur des objets statiques
    public sealed partial class RequestViewModel : INotifyPropertyChanged
    {
        static public ConfigurationViewModel config = new ConfigurationViewModel();
        public string configByKey = "";
        public string InteractReply;
        public bool Populated = false;
        public CancellationTokenSource tokenSource;
        private static volatile RequestViewModel _instance;
        private static object threadLock = new object();
        private ObservableCollection<Command> _commandList = new ObservableCollection<Command>();
        private double _dateTime;
        private ObservableCollection<EqLogic> _eqLogicList = new ObservableCollection<EqLogic>();
        private ObservableCollection<JdItem> _favoriteList = new ObservableCollection<JdItem>();
        private List<string> _favoriteIdList = new List<string>();
        private ObservableCollection<Interact> _interactList = new ObservableCollection<Interact>();
        private string _loadingMessage;
        private ObservableCollection<Message> _messageList = new ObservableCollection<Message>();
        private ObservableCollection<JdObject> _objectList = new ObservableCollection<JdObject>();
        private int _progress = 0;
        private RelayCommand<object> _RefreshCommand;
        private ObservableCollection<Scene> _sceneList = new ObservableCollection<Scene>();
        private Boolean _updating;
        private string _version;
        private int pass = 0;

        public RequestViewModel()
        {
        }

        public static RequestViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(threadLock)
                    {
                        if (_instance == null)
                            _instance = new RequestViewModel();
                    }
                }

                return _instance;
            }
        }
    }
}