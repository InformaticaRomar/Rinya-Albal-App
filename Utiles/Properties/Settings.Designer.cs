﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Utiles.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(DESCRIPTION =    (ADDRESS_LIST =      (ADDRESS = (PROTOCOL = TCP)(HO" +
            "ST = 172.16.0.14)(PORT = 1521))    )    (CONNECT_DATA = (SID = dbgrinya))  );Use" +
            "r Id=grinya_expert;Password=datadec;")]
        public string Conex_Expert {
            get {
                return ((string)(this["Conex_Expert"]));
            }
            set {
                this["Conex_Expert"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;Use" +
            "r ID=dso;Password=dsodsodso")]
        public string Conex_Quality {
            get {
                return ((string)(this["Conex_Quality"]));
            }
            set {
                this["Conex_Quality"] = value;
            }
        }
    }
}
