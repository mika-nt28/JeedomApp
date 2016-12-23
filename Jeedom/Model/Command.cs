using Jeedom.Mvvm;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Jeedom.Model
{
    [DataContract]
    public class Command : INotifyPropertyChanged
    {
        #region Private Fields

        private double _DateTime;

        [DataMember(Name = "display")]
        private CommandDisplay _Display;

        [DataMember(Name = "eqLogic_id")]
        private string _EqLogic_id;

        private RelayCommand<object> _ExecCommand;

        [DataMember(Name = "id")]
        private string _Id;

        [DataMember(Name = "isVisible")]
        [JsonConverter(typeof(JsonConverters.BooleanJsonConverter))]
        private bool _IsVisible;

        [DataMember(Name = "logicalId")]
        private string _LogicalId;

        [DataMember(Name = "name")]
        private string _Name;

        private EqLogic _Parent;

        [DataMember(Name = "subType")]
        private string _SubType;

        [DataMember(Name = "type")]
        private string _Type;

        [DataMember(Name = "unite")]
        private string _Unite;

        private bool _Updating = false;

        [DataMember(Name = "currentValue")]
        private string _Value = "";

        private ParametersOption _WidgetValue = new ParametersOption();

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public double DateTime { get { return _DateTime; } set { _DateTime = value; NotifyPropertyChanged(); } }

        public CommandDisplay Display { get { if (_Display == null) return new CommandDisplay(); else return _Display; } set { _Display = value; NotifyPropertyChanged(); } }

        public string EqLogic_id { get { return _EqLogic_id; } set { _EqLogic_id = value; NotifyPropertyChanged(); } }

        public RelayCommand<object> ExecCommand
        {
            get
            {
                this._ExecCommand = this._ExecCommand ?? new RelayCommand<object>(async parameters =>
                {
                    try
                    {
                        this.Updating = true;
                        Parameters CmdParameters = new Parameters();
                        CmdParameters.id = this.Id;
                        CmdParameters.name = this.Name;
                        CmdParameters.options = this.WidgetValue;
                        await RequestViewModel.Instance.ExecuteCommand(this, CmdParameters);
                        await RequestViewModel.Instance.UpdateTask();

                        this.Updating = false;
                    }
                    catch (Exception) { }
                });
                return this._ExecCommand;
            }
        }

        public string Id { get { return _Id; } set { _Id = value; NotifyPropertyChanged(); } }

        public string LogicalId { get { return _LogicalId; } set { _LogicalId = value; NotifyPropertyChanged(); } }

        public bool IsVisible { get { return _IsVisible; } set { _IsVisible = Convert.ToBoolean(value); NotifyPropertyChanged(); } }

        public String Name { get { return _Name; } set { _Name = value; NotifyPropertyChanged(); } }

        public string Type { get { return _Type; } set { _Type = value; NotifyPropertyChanged(); } }

        public string SubType { get { return _SubType; } set { _SubType = value; NotifyPropertyChanged(); } }

        public string Unite { get { return _Unite; } set { _Unite = value; NotifyPropertyChanged(); } }

        public bool Updating { get { return _Updating; } set { _Updating = value; NotifyPropertyChanged(); } }

        public string Value
        {
            get
            {
                if (_Value != "" && _Value != null)
                {
                    switch (_SubType)
                    {
                        case "numeric":
                            return _Value.Replace('.', ',');
                    }
                }
                return _Value;
            }

            set
            {
                _Value = value;
                if (_Value != "" && _Value != null)
                {
                    //TODO : @mika-nt28 j'ai pas tout compris sur cette notion de WidgetValue ???
                    switch (_SubType)
                    {
                        case "slider":
                            //this.WidgetValue.slider = Convert.ToDouble(RequestViewModel.Instance.CommandList.Where(cmd => cmd.id.Equals(_value.Replace('#', ' ').Trim())).FirstOrDefault().Value);
                            break;

                        case "message":
                            //this.WidgetValue.message = RequestViewModel.Instance.CommandList.Where(cmd => cmd.id.Equals(_value.Replace('#', ' ').Trim())).FirstOrDefault().Value;
                            break;

                        case "color":
                            //var search = RequestViewModel.Instance.CommandList.Where(cmd => cmd.id.Equals(_value.Replace('#', ' ').Trim())).FirstOrDefault();
                            //if (search != null)
                            //    this.WidgetValue.color = new SolidColorBrush(Color.FromArgb(
                            //        255,
                            //        System.Convert.ToByte(search.Value.Substring(1, 2), 16),
                            //        System.Convert.ToByte(search.Value.Substring(3, 2), 16),
                            //        System.Convert.ToByte(search.Value.Substring(5, 2), 16)));
                            break;
                    }
                }

                NotifyPropertyChanged();

                if (_Parent != null)
                {
                    switch (_Name)
                    {
                        case "Consommation":
                            _Parent.Consommation = value + " ";// + unite;
                            break;

                        case "Puissance":
                            _Parent.Puissance = value + " ";// + unite;
                            break;

                        case "Etat":
                            _Parent.State = value;
                            break;
                    }
                }
            }
        }

        public ParametersOption WidgetValue
        {
            get
            {
                return _WidgetValue;
            }

            set
            {
                _WidgetValue = value;
                NotifyPropertyChanged();
            }
        }

        #endregion Public Properties

        #region Private Methods

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Private Methods
    }
}