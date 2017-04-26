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
    public sealed partial class AlarmPluginControl : UserControl
    {
        #region Public Fields

        public static readonly DependencyProperty ArmedBrushProperty =
            DependencyProperty.Register("ArmedBrush", typeof(Brush), typeof(AlarmPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF434A54")));
        public static readonly DependencyProperty ArmedProperty =
            DependencyProperty.Register("Armed", typeof(Command), typeof(AlarmPluginControl), new PropertyMetadata(null));
        public static readonly DependencyProperty EnableBrushProperty =
            DependencyProperty.Register("EnableBrush", typeof(Brush), typeof(AlarmPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF96C927")));
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.Register("Enable", typeof(Command), typeof(AlarmPluginControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ModeBrushProperty =
            DependencyProperty.Register("ModeBrush", typeof(Brush), typeof(AlarmPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF434A54")));
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(Command), typeof(AlarmPluginControl), new PropertyMetadata(null));
        public static readonly DependencyProperty SetModeBrushProperty =
            DependencyProperty.Register("SetModeBrush", typeof(Brush), typeof(AlarmPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF96C927")));
        public static readonly DependencyProperty SetModeProperty =
            DependencyProperty.Register("SetMode", typeof(Command), typeof(AlarmPluginControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ReleasedBrushProperty =
            DependencyProperty.Register("ReleasedBrush", typeof(Brush), typeof(AlarmPluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF96C927")));
        public static readonly DependencyProperty ReleasedProperty =
            DependencyProperty.Register("Released", typeof(Command), typeof(AlarmPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateBoolProperty =
            DependencyProperty.Register("StateBool", typeof(bool), typeof(AlarmPluginControl), new PropertyMetadata(false));

        public static readonly DependencyProperty StateBrushProperty =
                    DependencyProperty.Register("StateBrush", typeof(Brush), typeof(AlarmPluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateIconProperty =
            DependencyProperty.Register("StateIcon", typeof(string), typeof(AlarmPluginControl), new PropertyMetadata("jeedom_lumiere_off"));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(Command), typeof(AlarmPluginControl), new PropertyMetadata(null));

        #endregion Public Fields

        #region Private Fields

        private bool _DataContextLoaded = false;

        #endregion Private Fields

        #region Public Constructors

        public AlarmPluginControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += AlarmPluginControl_DataContextChanged;
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public Command State { get { return (Command)GetValue(StateProperty); } set { SetValue(StateProperty, value); } }
        public bool StateBool { get { return (bool)GetValue(StateBoolProperty); } set { SetValue(StateBoolProperty, value); } }
        public Brush StateBrush { get { return (Brush)GetValue(StateBrushProperty); } set { SetValue(StateBrushProperty, value); } }
        public string StateIcon { get { return (string)GetValue(StateIconProperty); } set { SetValue(StateIconProperty, value); } }
        public Command Armed { get { return (Command)GetValue(ArmedProperty); } set { SetValue(ArmedProperty, value); } }
        public Brush ArmedBrush { get { return (Brush)GetValue(ArmedBrushProperty); } set { SetValue(ArmedBrushProperty, value); } }
        public Command Enable { get { return (Command)GetValue(EnableProperty); } set { SetValue(EnableProperty, value); } }
        public Brush EnableBrush { get { return (Brush)GetValue(EnableBrushProperty); } set { SetValue(ArmedBrushProperty, value); } }
        public Command Mode { get { return (Command)GetValue(ModeProperty); } set { SetValue(ModeProperty, value); } }
        public Brush ModeBrush { get { return (Brush)GetValue(ModeBrushProperty); } set { SetValue(ModeBrushProperty, value); } }
        public Command SetMode { get { return (Command)GetValue(SetModeProperty); } set { SetValue(SetModeProperty, value); } }
        public Brush SetModeBrush { get { return (Brush)GetValue(SetModeBrushProperty); } set { SetValue(ArmedBrushProperty, value); } }
        public Command Released { get { return (Command)GetValue(ReleasedProperty); } set { SetValue(ReleasedProperty, value); } }
        public Brush ReleasedBrush { get { return (Brush)GetValue(ReleasedBrushProperty); } set { SetValue(ReleasedBrushProperty, value); } }

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
    
        }

        private void AlarmPluginControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
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
                    case "ALARM_STATE":
                        State = cmd;
                        break;

                    case "ALARM_ARMED":
                        Armed = cmd;
                        break;

                    case "ALARM_ENABLE_STATE":
                        Enable = cmd;
                        break;

                    case "ALARM_MODE":
                        Mode = cmd;
                        break;

                    case "ALARM_SET_MODE":
                        SetMode = cmd;
                        break;

                    case "ALARM_RELEASED":
                        Released = cmd;
                        break;

                    default:

                        break;
                }
            }

            _DataContextLoaded = true;
        }

        /// <summary>
        /// Mise à jour de l'interface
        /// </summary>

        #endregion Private Methods
    }
}
