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

       
        public static readonly DependencyProperty SilderProperty =
            DependencyProperty.Register("FLAP_SLIDER", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty DownProperty =
            DependencyProperty.Register("FLAP_DOWN", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StopProperty =
             DependencyProperty.Register("FLAP_STOP", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty UpProperty =
             DependencyProperty.Register("FLAP_UP", typeof(Command), typeof(ThermostatPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("FLAP_STATE", typeof(string), typeof(ThermostatPluginControl), new PropertyMetadata(null));

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

        public Command Position { get { return (Command)GetValue(SilderProperty); } set { SetValue(SilderProperty, value); } }
        public Command Down { get { return (Command)GetValue(DownProperty); } set { SetValue(DownProperty, value); } }
        public Command Stop { get { return (Command)GetValue(StopProperty); } set { SetValue(StopProperty, value); } }
        public Command Up { get { return (Command)GetValue(UpProperty); } set { SetValue(UpProperty, value); } }
        public string State { get { return (string)GetValue(StateProperty); } set { SetValue(StateProperty, value); } }
       
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
                    case "FLAP_STATE":
                        State = cmd.Value;
                        //Position.Value =cmd.Value;
                        break;

                    case "FLAP_SLIDER":
                        Position =cmd;
                        break;

                    case "FLAP_DOWN":
                        Down = cmd;
                        break;

                    case "FLAP_STOP": 
                        Stop = cmd;
                        break;

                    case "FLAP_UP": 
                        Up = cmd;
                        break;
                        
                }
            }

            _DataContextLoaded = true;
        }
        #endregion Private Methods
    }
}
