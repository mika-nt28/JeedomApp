using Jeedom.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace JeedomApp.Controls
{
    public sealed partial class ThermostatPluginControl : UserControl
    {
        #region Public Fields

       
        public static readonly DependencyProperty TemperatureProperty =
            DependencyProperty.Register("Temperature", typeof(string), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty SetPointProperty =
            DependencyProperty.Register("SetPoint", typeof(string), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty SetSetPointProperty =
             DependencyProperty.Register("SetSetPoint", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ModeProperty =
             DependencyProperty.Register("Mode", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty SetModeProperty =
           DependencyProperty.Register("SetMode", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty UnlockBrushProperty =
            DependencyProperty.Register("UnlockBrush", typeof(Brush), typeof(ThermostatPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF434A54")));

        public static readonly DependencyProperty UnlockIconProperty =
            DependencyProperty.Register("UnlockIcon", typeof(string), typeof(ThermostatPluginControl), new PropertyMetadata("jeedom_lumiere_Unlock"));

        public static readonly DependencyProperty UnlockProperty =
            DependencyProperty.Register("Unlock", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty LockBrushProperty =
            DependencyProperty.Register("LockBrush", typeof(Brush), typeof(ThermostatPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF96C927")));

        public static readonly DependencyProperty LockIconProperty =
                    DependencyProperty.Register("LockIcon", typeof(string), typeof(ThermostatPluginControl), new PropertyMetadata("jeedom_lumiere_on"));

        public static readonly DependencyProperty LockProperty =
            DependencyProperty.Register("Lock", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateBoolProperty =
            DependencyProperty.Register("StateBool", typeof(bool), typeof(ThermostatPluginControl), new PropertyMetadata(false));

        public static readonly DependencyProperty StateBrushProperty =
                    DependencyProperty.Register("StateBrush", typeof(Brush), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateIconProperty =
            DependencyProperty.Register("StateIcon", typeof(string), typeof(ThermostatPluginControl), new PropertyMetadata("jeedom_lumiere_Unlock"));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        #endregion Public Fields

        #region Private Fields

        private bool _DataContextLoaded = false;

        #endregion Private Fields

        #region Public Constructors

        public ThermostatPluginControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += VoletPluginControl_DataContextChanged;
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string Temperature { get { return (string)GetValue(TemperatureProperty); } set { SetValue(TemperatureProperty, value); } }
        public string SetPoint { get { return (string)GetValue(SetPointProperty); } set { SetValue(SetPointProperty, value); } }
        public Command SetSetPoint { get { return (Command)GetValue(SetSetPointProperty); } set { SetValue(SetSetPointProperty, value); } }
        public Command Mode { get { return (Command)GetValue(ModeProperty); } set { SetValue(ModeProperty, value); } }
        public Command SetMode { get { return (Command)GetValue(SetModeProperty); } set { SetValue(SetModeProperty, value); } }
        public Command Unlock { get { return (Command)GetValue(UnlockProperty); } set { SetValue(UnlockProperty, value); } }
        public Brush UnlockBrush { get { return (Brush)GetValue(UnlockBrushProperty); } set { SetValue(UnlockBrushProperty, value); } }
        public string UnlockIcon { get { return (string)GetValue(UnlockIconProperty); } set { SetValue(UnlockIconProperty, value); } }
        public Command Lock { get { return (Command)GetValue(LockProperty); } set { SetValue(LockProperty, value); } }
        public Brush LockBrush { get { return (Brush)GetValue(LockBrushProperty); } set { SetValue(LockBrushProperty, value); } }
        public string LockIcon { get { return (string)GetValue(LockIconProperty); } set { SetValue(LockIconProperty, value); } }
        public Command State { get { return (Command)GetValue(StateProperty); } set { SetValue(StateProperty, value); } }
        public bool StateBool { get { return (bool)GetValue(StateBoolProperty); } set { SetValue(StateBoolProperty, value); } }
        public Brush StateBrush { get { return (Brush)GetValue(StateBrushProperty); } set { SetValue(StateBrushProperty, value); } }
        public string StateIcon { get { return (string)GetValue(StateIconProperty); } set { SetValue(StateIconProperty, value); } }

        #endregion Public Properties

        #region Public Methods

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        #endregion Public Methods

        #region Private Methods


        private void Cmds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Update UI ");
            UpdateUI();
        }

        private void VoletPluginControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            //System.Diagnostics.Debug.WriteLine("DataContexChange");
            // Pas besoin de recharger
            if (_DataContextLoaded)
                return;

            if (this.DataContext == null)
                return;
            if (!(this.DataContext.GetType() == typeof(EqLogic)))
                return;

            var eq = (EqLogic)this.DataContext;

            // On s'abonne aux changements des commandes
            eq.Cmds.CollectionChanged += Cmds_CollectionChanged; ;

            foreach (var cmd in eq.Cmds)
            {
                switch (cmd.Display.generic_type)
                {
                    case "THERMOSTAT_TEMPERATURE":
                        Temperature = cmd.Value;
                        break;
                    case "THERMOSTAT_SETPOINT":
                        SetPoint = cmd.Value;
                        break;
                    case "THERMOSTAT_SET_SETPOINT":
                        SetSetPoint = cmd;
                        break;
                    case "THERMOSTAT_LOCK":
                        State = cmd;
                        break;
                    case "THERMOSTAT_SET_LOCK":
                        Lock = cmd;
                        break;
                    case "THERMOSTAT_SET_UNLOCK":
                        Unlock = cmd;
                        break;
                    case "THERMOSTAT_MODE":
                        Mode = cmd;
                        break;
                    case "THERMOSTAT_SET_MODE":
                        SetMode = cmd;
                        break;
                    case "THERMOSTAT_STATE":
                        break;
                }
            }

            _DataContextLoaded = true;
            UpdateUI();
        } 
        /// <summary>
          /// Mise à jour de l'interface
          /// </summary>
        private void UpdateUI()
        {
            SetSetPoint.Value = SetPoint;
            if (State == null)
                return;

            int _ValueInt;
            int.TryParse(State.Value, out _ValueInt);

            if (State.Value == "1" || State.Value.ToLower() == "on" || _ValueInt > 0)
            {
                StateBool = true;
                StateBrush = LockBrush;
                StateIcon = LockIcon;
            }
            else
            {
                StateBool = false;
                StateBrush = UnlockBrush;
                StateIcon = UnlockIcon;
            }
        }
        #endregion Private Methods
    }
}
