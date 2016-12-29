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

        #region Public Properties

        public double DateTime { get { return _DateTime; } set { Set(ref _DateTime, value); } }

        public CommandDisplay Display { get { if (_Display == null) return new CommandDisplay(); else return _Display; } set { Set(ref _Display, value); } }

        public string EqLogic_id { get { return _EqLogic_id; } set { Set(ref _EqLogic_id, value); } }

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

        public string Id { get { return _Id; } set { Set(ref _Id, value); } }

        public string LogicalId { get { return _LogicalId; } set { Set(ref _LogicalId, value); } }

        public bool IsVisible { get { return _IsVisible; } set { Set(ref _IsVisible, Convert.ToBoolean(value)); } }

        public String Name { get { return _Name; } set { Set(ref _Name, value); } }

        public string Type { get { return _Type; } set { Set(ref _Type, value); } }

        public string SubType { get { return _SubType; } set { Set(ref _SubType, value); } }

        public string Unite { get { return _Unite; } set { Set(ref _Unite, value); } }

        public bool Updating { get { return _Updating; } set { Set(ref _Updating, value); } }

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
                Set(ref _Value, value);

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
                Set(ref _WidgetValue, value);
            }
        }

        #endregion Public Properties

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                System.Diagnostics.Debug.WriteLine("cmd update : " + Name + " " + propertyName);
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion INotifyPropertyChanged
    }
}