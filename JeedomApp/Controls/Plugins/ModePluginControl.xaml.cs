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
    public sealed partial class ModePluginControl : UserControl
    {
        #region Public Fields

        public static readonly DependencyProperty ModeBrushProperty =
            DependencyProperty.Register("ModeBrush", typeof(Brush), typeof(ModePluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF434A54")));
        public static readonly DependencyProperty ModeSelectBrushProperty =
                   DependencyProperty.Register("ModeSelectBrush", typeof(Brush), typeof(ModePluginControl), new PropertyMetadata(XamlBindingHelper.ConvertValue(typeof(Brush), "#FF96C927")));

        public static readonly DependencyProperty ModeIconProperty =
                    DependencyProperty.Register("ModeIcon", typeof(string), typeof(ModePluginControl), new PropertyMetadata("jeedom_lumiere_on"));

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(Command), typeof(ModePluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateBrushProperty =
                    DependencyProperty.Register("StateBrush", typeof(Brush), typeof(ModePluginControl), new PropertyMetadata(null));

        public static readonly DependencyProperty StateIconProperty =
            DependencyProperty.Register("StateIcon", typeof(string), typeof(ModePluginControl), new PropertyMetadata("jeedom_lumiere_off"));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(Command), typeof(ModePluginControl), new PropertyMetadata(null));

        #endregion Public Fields

        #region Private Fields

        private bool _DataContextLoaded = false;

        #endregion Private Fields

        #region Public Constructors

        public ModePluginControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += ModePluginControl_DataContextChanged;
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public Command Mode { get { return (Command)GetValue(ModeProperty); } set { SetValue(ModeProperty, value); } }
        public Brush ModeBrush { get { return (Brush)GetValue(ModeBrushProperty); } set { SetValue(ModeBrushProperty, value); } }
        public Brush ModeSelectBrush { get { return (Brush)GetValue(ModeSelectBrushProperty); } set { SetValue(ModeSelectBrushProperty, value); } }
        public string ModeIcon { get { return (string)GetValue(ModeIconProperty); } set { SetValue(ModeIconProperty, value); } }
        public Command State { get { return (Command)GetValue(StateProperty); } set { SetValue(StateProperty, value); } }
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

        private void ModePluginControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
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
                    case "MODE_STATE":
                        State = cmd;
                        break;

                    case "MODE_SET_STATE":
                        Mode = cmd;
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
            if (State == null)
                return;

            int _ValueInt;
            int.TryParse(State.Value, out _ValueInt);

            if (State.Value == "1" || State.Value.ToLower() == "on" || _ValueInt > 0)
            {
                StateBrush = ModeSelectBrush;
            }
            else
            {
                StateBrush = ModeBrush;
            }
        }

        #endregion Private Methods
    }
}
