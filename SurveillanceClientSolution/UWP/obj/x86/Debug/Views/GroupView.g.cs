﻿#pragma checksum "C:\VS\BuildingSurveillance\SurveillanceClientSolution\UWP\Views\GroupView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B6004169A23FB212482AC691B0A5C10D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UWP.Views
{
    partial class GroupView : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // Views\GroupView.xaml line 18
                {
                    this.btnEditGroup = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.btnEditGroup).Click += this.btnEditGroup_Click;
                }
                break;
            case 2: // Views\GroupView.xaml line 20
                {
                    this.btnAddDevice = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.btnAddDevice).Click += this.btnAddDevice_Click;
                }
                break;
            case 3: // Views\GroupView.xaml line 21
                {
                    this.btnDelete = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.btnDelete).Click += this.btnDelete_Click;
                }
                break;
            case 4: // Views\GroupView.xaml line 65
                {
                    this.lvDevices = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.lvDevices).ItemClick += this.lvDevices_ItemClick;
                }
                break;
            case 5: // Views\GroupView.xaml line 46
                {
                    this.lvPeople = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
