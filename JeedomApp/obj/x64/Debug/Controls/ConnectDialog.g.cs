﻿#pragma checksum "C:\Users\micha_000\Source\Repos\JeedomApp\JeedomApp\Controls\ConnectDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E292A7DD2CBCD1A96B80BFE612FCA10B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JeedomApp.Controls
{
    partial class ConnectDialog : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element1 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 57 "..\..\..\Controls\ConnectDialog.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element1).Click += this.bConnect_Click;
                    #line default
                }
                break;
            case 2:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element2 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 60 "..\..\..\Controls\ConnectDialog.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element2).Click += this.QrCodeInfo_Click;
                    #line default
                }
                break;
            case 3:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element3 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 63 "..\..\..\Controls\ConnectDialog.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element3).Click += this.Demo_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.ErrorMsg = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.tbTitle = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6:
                {
                    this.tbSubtitle = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.image = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
