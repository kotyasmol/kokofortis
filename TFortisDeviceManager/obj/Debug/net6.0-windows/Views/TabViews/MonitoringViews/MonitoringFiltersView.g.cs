﻿#pragma checksum "..\..\..\..\..\..\Views\TabViews\MonitoringViews\MonitoringFiltersView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B5C12EEFF6344966CEB41456C3D43289D249944E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Expression.Media;
using HandyControl.Expression.Shapes;
using HandyControl.Interactivity;
using HandyControl.Media.Animation;
using HandyControl.Media.Effects;
using HandyControl.Properties.Langs;
using HandyControl.Themes;
using HandyControl.Tools;
using HandyControl.Tools.Converter;
using HandyControl.Tools.Extension;
using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
using Stylet.Xaml;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TFortisDeviceManager.Converters;
using TFortisDeviceManager.Properties;
using TFortisDeviceManager.ViewModels;


namespace TFortisDeviceManager.Views {
    
    
    /// <summary>
    /// MonitoringFiltersView
    /// </summary>
    public partial class MonitoringFiltersView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\..\..\Views\TabViews\MonitoringViews\MonitoringFiltersView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal HandyControl.Controls.CheckComboBox ModelFilter;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\..\..\Views\TabViews\MonitoringViews\MonitoringFiltersView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal HandyControl.Controls.CheckComboBox IpFilter;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\..\Views\TabViews\MonitoringViews\MonitoringFiltersView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal HandyControl.Controls.CheckComboBox ParameterFilter;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\..\..\Views\TabViews\MonitoringViews\MonitoringFiltersView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal HandyControl.Controls.CheckComboBox StateFilter;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TFortisDeviceManager;V1.0.0.0;component/views/tabviews/monitoringviews/monitorin" +
                    "gfiltersview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\TabViews\MonitoringViews\MonitoringFiltersView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ModelFilter = ((HandyControl.Controls.CheckComboBox)(target));
            return;
            case 2:
            this.IpFilter = ((HandyControl.Controls.CheckComboBox)(target));
            return;
            case 3:
            this.ParameterFilter = ((HandyControl.Controls.CheckComboBox)(target));
            return;
            case 4:
            this.StateFilter = ((HandyControl.Controls.CheckComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

