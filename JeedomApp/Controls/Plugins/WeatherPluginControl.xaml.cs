using Jeedom.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Template10.Mvvm;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace JeedomApp.Controls
{
    public sealed partial class WeatherPluginControl : UserControl
    {
        public EqLogic TheEqLogic
        {
            get { return (EqLogic)GetValue(TheEqLogicProperty); }
            set
            {
                SetValue(TheEqLogicProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for TheEqLogic. This enables animation,
        // styling, binding, etc...
        public static readonly DependencyProperty TheEqLogicProperty =
            DependencyProperty.Register("TheEqLogic", typeof(EqLogic), typeof(WeatherPluginControl), new PropertyMetadata(null));

        public WeatherPluginControl()
        {
            this.InitializeComponent();
            _Day1Name = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)DateTime.Now.DayOfWeek].Substring(0, 3).ToUpper();
            _Day2Name = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)DateTime.Now.AddDays(1).DayOfWeek].Substring(0, 3).ToUpper();
            _Day3Name = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)DateTime.Now.AddDays(2).DayOfWeek].Substring(0, 3).ToUpper();
            _Day4Name = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)DateTime.Now.AddDays(3).DayOfWeek].Substring(0, 3).ToUpper();
            _Day5Name = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)DateTime.Now.AddDays(4).DayOfWeek].Substring(0, 3).ToUpper();

            this.DataContextChanged += WeatherPluginControl_DataContextChanged;
            //this.DataContext = this;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion INotifyPropertyChanged

        private string _Day1Name = default(string);
        public string Day1Name { get { return _Day1Name; } set { Set(ref _Day1Name, value); } }

        private string _Day2Name = default(string);
        public string Day2Name { get { return _Day2Name; } set { Set(ref _Day2Name, value); } }

        private string _Day3Name = default(string);
        public string Day3Name { get { return _Day3Name; } set { Set(ref _Day3Name, value); } }

        private string _Day4Name = default(string);
        public string Day4Name { get { return _Day4Name; } set { Set(ref _Day4Name, value); } }

        private string _Day5Name = default(string);
        public string Day5Name { get { return _Day5Name; } set { Set(ref _Day5Name, value); } }

        private string _Summary = default(string);
        public string Summary { get { return _Summary; } set { Set(ref _Summary, value); } }

        private double _Temperature = default(double);
        public double Temperature { get { return _Temperature; } set { Set(ref _Temperature, value); } }

        private double _TemperatureMax1 = default(double);
        public double TemperatureMax1 { get { return _TemperatureMax1; } set { Set(ref _TemperatureMax1, value); } }

        private double _TemperatureMax2 = default(double);
        public double TemperatureMax2 { get { return _TemperatureMax2; } set { Set(ref _TemperatureMax2, value); } }

        private double _TemperatureMax3 = default(double);
        public double TemperatureMax3 { get { return _TemperatureMax3; } set { Set(ref _TemperatureMax3, value); } }

        private double _TemperatureMax4 = default(double);
        public double TemperatureMax4 { get { return _TemperatureMax4; } set { Set(ref _TemperatureMax4, value); } }

        private double _TemperatureMax5 = default(double);
        public double TemperatureMax5 { get { return _TemperatureMax5; } set { Set(ref _TemperatureMax5, value); } }

        private double _TemperatureMin1 = default(double);
        public double TemperatureMin1 { get { return _TemperatureMin1; } set { Set(ref _TemperatureMin1, value); } }

        private double _TemperatureMin2 = default(double);
        public double TemperatureMin2 { get { return _TemperatureMin2; } set { Set(ref _TemperatureMin2, value); } }

        private double _TemperatureMin3 = default(double);
        public double TemperatureMin3 { get { return _TemperatureMin3; } set { Set(ref _TemperatureMin3, value); } }

        private double _TemperatureMin4 = default(double);
        public double TemperatureMin4 { get { return _TemperatureMin4; } set { Set(ref _TemperatureMin4, value); } }

        private double _TemperatureMin5 = default(double);
        public double TemperatureMin5 { get { return _TemperatureMin5; } set { Set(ref _TemperatureMin5, value); } }

        private string _Icon = default(string);
        public string Icon { get { return _Icon; } set { Set(ref _Icon, value); } }

        private string _Icon1 = default(string);
        public string Icon1 { get { return _Icon1; } set { Set(ref _Icon1, value); } }

        private string _Icon2 = default(string);
        public string Icon2 { get { return _Icon2; } set { Set(ref _Icon2, value); } }

        private string _Icon3 = default(string);
        public string Icon3 { get { return _Icon3; } set { Set(ref _Icon3, value); } }

        private string _Icon4 = default(string);
        public string Icon4 { get { return _Icon4; } set { Set(ref _Icon4, value); } }

        private string _Icon5 = default(string);
        public string Icon5 { get { return _Icon5; } set { Set(ref _Icon5, value); } }

        private void WeatherPluginControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (TheEqLogic == null)
                return;

            foreach (var cmd in TheEqLogic.Cmds)
            {
                switch (cmd.LogicalId)
                {
                    case "summary":
                        _Summary = cmd.Value;
                        break;

                    case "temperature":
                        Double.TryParse(cmd.Value, out _Temperature);
                        break;

                    case "temperatureMax_1":
                        Double.TryParse(cmd.Value, out _TemperatureMax1);
                        break;

                    case "temperatureMax_2":
                        Double.TryParse(cmd.Value, out _TemperatureMax2);
                        break;

                    case "temperatureMax_3":
                        Double.TryParse(cmd.Value, out _TemperatureMax3);
                        break;

                    case "temperatureMax_4":
                        Double.TryParse(cmd.Value, out _TemperatureMax4);
                        break;

                    case "temperatureMax_5":
                        Double.TryParse(cmd.Value, out _TemperatureMax5);
                        break;

                    case "temperatureMin_1":
                        Double.TryParse(cmd.Value, out _TemperatureMin1);
                        break;

                    case "temperatureMin_2":
                        Double.TryParse(cmd.Value, out _TemperatureMin2);
                        break;

                    case "temperatureMin_3":
                        Double.TryParse(cmd.Value, out _TemperatureMin3);
                        break;

                    case "temperatureMin_4":
                        Double.TryParse(cmd.Value, out _TemperatureMin4);
                        break;

                    case "temperatureMin_5":
                        Double.TryParse(cmd.Value, out _TemperatureMin5);
                        break;

                    case "icon":
                        _Icon = cmd.Value;
                        break;

                    case "icon_1":
                        _Icon1 = cmd.Value;
                        break;

                    case "icon_2":
                        _Icon2 = cmd.Value;
                        break;

                    case "icon_3":
                        _Icon3 = cmd.Value;
                        break;

                    case "icon_4":
                        _Icon4 = cmd.Value;
                        break;

                    case "icon_5":
                        _Icon5 = cmd.Value;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}