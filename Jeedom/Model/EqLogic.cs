using Jeedom.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Jeedom.Model
{
    [DataContract]
    public class EqLogic : INotifyPropertyChanged
    {
        #region Private Fields

        [DataMember(Name = "cmds")]
        private ObservableCollectionEx<Command> _Cmds;

        private double _DateTime;

        [DataMember(Name = "display")]
        private EqLogicDisplay _Display;

        [DataMember(Name = "eqType_name")]
        private string _EqTypeName;

        private RelayCommand<object> _ExecCommandByLogicalID;

        private RelayCommand<object> _ExecCommandByName;

        private RelayCommand<object> _ExecCommandByType;

        [DataMember(Name = "id")]
        private string _Id;

        [DataMember(Name = "isEnable")]
        [JsonConverter(typeof(JsonConverters.BooleanJsonConverter))]
        private bool _IsEnable;

        [DataMember(Name = "isVisible")]
        [JsonConverter(typeof(JsonConverters.BooleanJsonConverter))]
        private bool _IsVisible = true;

        [DataMember(Name = "logicalId")]
        private string _LogicalId;

        [DataMember(Name = "name")]
        private string _Name;

        [DataMember(Name = "object_id")]
        private string _ObjectId;

        private JdObject _Parent;

        private bool _Updating;

        public EqLogic()
        {
        }

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Liste des commandes de l'équipement
        /// </summary>
        public ObservableCollectionEx<Command> Cmds
        {
            get
            {
                return _Cmds;
            }

            set
            {
                _Cmds = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("VisibleCmds");
            }
        }

        public int ColSpan
        {
            get
            {
                var _visibleCmds = GetVisibleCmds().Count();
                if (_visibleCmds > 3)
                    return 6;
                else
                    return _visibleCmds * 2;
            }
        }

        public EqLogicDisplay Display
        {
            get
            {
                return _Display;
            }
            set
            {
                _Display = value;
                NotifyPropertyChanged();
            }
        }

        public String EqTypeName
        {
            get
            {
                return _EqTypeName;
            }
            set
            {
                _EqTypeName = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "logicalId"
        /// </summary>
        public RelayCommand<object> ExecCommandByLogicalID
        {
            get
            {
                this._ExecCommandByLogicalID = this._ExecCommandByLogicalID ?? new RelayCommand<object>(async parameters =>
                 {
                     var cmd = Cmds.Where(c => c.LogicalId.ToLower() == parameters.ToString().ToLower()).FirstOrDefault();
                     if (cmd != null)
                         await ExecCommand(cmd);
                 });
                return this._ExecCommandByLogicalID;
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "name"
        /// </summary>
        public RelayCommand<object> ExecCommandByName
        {
            get
            {
                this._ExecCommandByName = this._ExecCommandByName ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        var cmd = Cmds.Where(c => c.Name.ToLower() == parameters.ToString().ToLower()).FirstOrDefault();
                        if (cmd != null)
                            await ExecCommand(cmd);
                    }
                    catch (Exception) { }
                });
                return this._ExecCommandByName;
            }
        }

        /// <summary>
        /// Exécute une commande à partir de son "generic_type"
        /// </summary>
        public RelayCommand<object> ExecCommandByType
        {
            get
            {
                this._ExecCommandByType = this._ExecCommandByType ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        var cmd = Cmds.Where(c => c.Display.generic_type == parameters.ToString()).FirstOrDefault();
                        if (cmd != null)
                            await ExecCommand(cmd);
                    }
                    catch (Exception) { }
                });
                return this._ExecCommandByType;
            }
        }

        public string Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }

            set
            {
                _IsVisible = Convert.ToBoolean(value);
            }
        }

        public bool IsEnable
        {
            get
            {
                return _IsEnable;
            }

            set
            {
                _IsEnable = Convert.ToBoolean(value);
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        public int RowSpan
        {
            get
            {
                var _visibleCmds = GetVisibleCmds().Count();
                if (_visibleCmds > 3)
                    return (int)Math.Ceiling((double)_visibleCmds / 3) * 2;
                else
                    return 2;
            }
        }

        public bool Updating
        {
            get
            {
                return _Updating;
            }
            set
            {
                _Updating = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Dernière modification de l'équipement
        /// </summary>
        public double DateTime
        {
            get
            {
                return _DateTime;
            }
            set
            {
                _DateTime = value;
                NotifyPropertyChanged();
            }
        }

        public string ObjectId
        {
            get
            {
                return _ObjectId;
            }
            set
            {
                _ObjectId = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Command> VisibleCmds
        {
            get
            {
                return GetVisibleCmds();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public ObservableCollection<Command> GetActionsCmds()
        {
            if (Cmds != null)
            {
                IEnumerable<Command> results = Cmds.Where(c => c.Type == "action");
                return new ObservableCollection<Command>(results);
            }
            else
                return new ObservableCollection<Command>();
        }

        public ObservableCollection<Command> GetInformationsCmds()
        {
            if (Cmds != null)
            {
                IEnumerable<Command> results = Cmds.Where(c => c.Type == "info");
                return new ObservableCollection<Command>(results);
            }
            else
                return new ObservableCollection<Command>();
        }

        public ObservableCollection<Command> GetVisibleCmds()
        {
            if (Cmds != null)
            {
                IEnumerable<Command> results = Cmds.Where(c => c.IsVisible == true && c.LogicalId != null);
                return new ObservableCollection<Command>(results);
            }
            else
                return new ObservableCollection<Command>();
        }

        #endregion Public Methods

        #region Private Methods

        private async Task ExecCommand(Command cmd)
        {
            if (cmd != null)
            {
                this.Updating = true;
                await RequestViewModel.Instance.ExecuteCommand(cmd);
                //await Task.Delay(TimeSpan.FromSeconds(3));
                await RequestViewModel.Instance.UpdateEqLogic(this);
                NotifyPropertyChanged("Cmds");
                this.Updating = false;
            }
        }

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            //System.Diagnostics.Debug.WriteLine("eqLogic update : " + Name + " " + propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Private Methods
    }
}
