using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JeedomApp.Controls
{
    internal class VariableSizedGridView : GridView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            try
            {
                dynamic localItem = item;
                if (item.GetType() == typeof(Jeedom.Model.EqLogic))
                {
                    element.SetValue(VariableSizedWrapGrid.RowSpanProperty, localItem.RowSpan);
                    element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, localItem.ColSpan);
                }
                else
                {
                    element.SetValue(VariableSizedWrapGrid.RowSpanProperty, 1);
                    element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 1);
                }
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e)
            {
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, 1);
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 1);
            }

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}