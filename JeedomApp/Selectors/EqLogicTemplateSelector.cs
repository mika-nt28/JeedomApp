using Jeedom.Model;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JeedomApp.Selectors
{
    internal class EqLogicTemplateSelector : DataTemplateSelector
    {
        #region Public Properties

        public DataTemplate EqLogicTemplate { get; set; }
        public DataTemplate OnOffEqLogicTemplate { get; set; }
        public DataTemplate SonosEqLogicTemplate { get; set; }
        public DataTemplate ForecastIoEqLogicTemplate { get; set; }
        public DataTemplate ZWaveEqLogicTemplate { get; set; }
        public DataTemplate CameraEqLogicTemplate { get; set; }

        #endregion Public Properties

        #region Protected Methods

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var eq = item as EqLogic;
            //Jeedom.RequestViewModel.Instance.UpdateEqLogic(eq);
            //var element = container as FrameworkElement;

            // Cherche si on a spécifié un Template dans les customParameters de l'équipement
            /*if (eq.display != null)
                if (eq.display.customParameters != null)
                    if (eq.display.customParameters.JeedomAppTemplate != null)
                        switch (eq.display.customParameters.JeedomAppTemplate.ToLower())
                        {
                            case "sonos":
                                return SonosEqLogicTemplate;

                            case "onoff":
                                return OnOffEqLogicTemplate;
                        }*/

            // Cherche par rapport aux commandes de l'équipement
            //TODO : Voir "generic_type" : https://www.jeedom.com/forum/viewtopic.php?f=112&t=15155#p278226

            // Lumière OnOff
            if (ContainCmd(eq, new[] { "LIGHT_STATE", "LIGHT_ON", "LIGHT_OFF" }))
            {
                container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 1);
                container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 1);
                return OnOffEqLogicTemplate;
            }

            // Cherche par rapport au plugin
            System.Diagnostics.Debug.WriteLine(eq.eqType_name);
            switch (eq.eqType_name)
            {
                case "sonos3":
                    container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 3);
                    container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 3);
                    return SonosEqLogicTemplate;

                case "forecastio":
                    container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                    container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 2);
                    return ForecastIoEqLogicTemplate;

                case "camera":
                    container.SetValue(VariableSizedWrapGrid.RowSpanProperty, 1);
                    container.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 2);
                    return CameraEqLogicTemplate;
            }
            return EqLogicTemplate;
        }

        /// <summary>
        /// Renvoie vrai si l'équipement a les commandes recherchées
        /// </summary>
        /// <param name="eq">L'équipement Jeedom</param>
        /// <param name="types">Les generic_type recherchés</param>
        /// <returns></returns>
        private static bool ContainCmd(EqLogic eq, string[] types)
        {
            int _find = 0;
            foreach (var type in types)
            {
                if (eq.cmds != null)
                {
                    var search = eq.cmds.Where(c => c.display.generic_type == type);
                    if (search.Count() > 0)
                        _find += 1;
                }
            }
            return _find == types.Count();
        }

        #endregion Protected Methods
    }
}