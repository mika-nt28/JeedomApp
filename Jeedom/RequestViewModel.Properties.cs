using Jeedom.Model;
using Jeedom.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace Jeedom
{
    public partial class RequestViewModel
    {
        public ObservableCollection<Command> CommandList
        {
            get { return _commandList; }
            set
            {
                _commandList = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<EqLogic> EqLogicList
        {
            get { return _eqLogicList; }
            set
            {
                _eqLogicList = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<JdItem> FavoriteList
        {
            get { return _favoriteList; }
            set
            {
                _favoriteList = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Interact> InteractList
        {
            get { return _interactList; }
            set
            {
                _interactList = value;
                NotifyPropertyChanged();
            }
        }

        public string LoadingMessage
        {
            get
            {
                return _loadingMessage;
            }

            private set
            {
                _loadingMessage = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Message> MessageList
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<JdObject> ObjectList
        {
            get { return _objectList; }
            set
            {
                _objectList = value;
                NotifyPropertyChanged();
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand<object> RefreshCommand
        {
            get
            {
                this._RefreshCommand = this._RefreshCommand ?? new RelayCommand<object>(async parameters =>
                {
                    await UpdateTask();
                });
                return this._RefreshCommand;
            }
        }

        public ObservableCollection<Scene> SceneList
        {
            get { return _sceneList; }
            set
            {
                _sceneList = value;
                NotifyPropertyChanged();
            }
        }

        public Boolean Updating
        {
            get
            {
                return _updating;
            }
            set
            {
                _updating = value;
                NotifyPropertyChanged();
            }
        }

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                NotifyPropertyChanged();
            }
        }
    }
}