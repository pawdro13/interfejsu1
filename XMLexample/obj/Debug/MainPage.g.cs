﻿

#pragma checksum "C:\Users\Daniel\Desktop\Platformy mobilne\PM_WindowsStore-Aplication\XMLexample\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "94DC83214AA155348CECAB685463DAFD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XMLexample
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 11 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.pageRoot_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 19 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.UpdateDataButton_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 20 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.CloseApplicationButton1_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 52 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.listBox_daty_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 54 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.listBox_waluty_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 50 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBlock)(target)).SelectionChanged += this.pageTitle_SelectionChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


