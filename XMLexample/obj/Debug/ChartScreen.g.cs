﻿

#pragma checksum "C:\Users\Daniel\Desktop\Platformy mobilne\PM_WindowsStore-Aplication\XMLexample\ChartScreen.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E55B677BE013C9196190EB6200CDE0B1"
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
    partial class ChartScreen : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 20 "..\..\ChartScreen.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ClearCanvas_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 21 "..\..\ChartScreen.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ExitButtonChart_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 61 "..\..\ChartScreen.xaml"
                ((global::Windows.UI.Xaml.Controls.DatePicker)(target)).DateChanged += this.dateStart_DateChanged;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 62 "..\..\ChartScreen.xaml"
                ((global::Windows.UI.Xaml.Controls.DatePicker)(target)).DateChanged += this.DatePicker_DateChanged;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 64 "..\..\ChartScreen.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.LoadHistory_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 65 "..\..\ChartScreen.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SaveChart_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 66 "..\..\ChartScreen.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.WriteHistory_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 69 "..\..\ChartScreen.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.errorConsole_TextChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


