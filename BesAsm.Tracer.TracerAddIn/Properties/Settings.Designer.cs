﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BesAsm.Tracer.TracerAddIn.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int DefaultClickTolerance {
            get {
                return ((int)(this["DefaultClickTolerance"]));
            }
            set {
                this["DefaultClickTolerance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>us_node_id</string>
  <string>US_Node</string>
  <string>USNode</string>
  <string>us_node_name_deprecated</string>
  <string>FRM_NODE</string>
  <string>From_Node</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection KnownUsNodeFieldNames {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["KnownUsNodeFieldNames"]));
            }
            set {
                this["KnownUsNodeFieldNames"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>ds_node_id</string>
  <string>DS_Node</string>
  <string>DSNode</string>
  <string>ds_node_name_deprecated</string>
  <string>TO_NODE</string>
  <string>To_Node</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection KnownDsNodeFieldNames {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["KnownDsNodeFieldNames"]));
            }
            set {
                this["KnownDsNodeFieldNames"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Green")]
        public global::System.Drawing.Color StartLinkColor {
            get {
                return ((global::System.Drawing.Color)(this["StartLinkColor"]));
            }
            set {
                this["StartLinkColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Red")]
        public global::System.Drawing.Color StopLinkColor {
            get {
                return ((global::System.Drawing.Color)(this["StopLinkColor"]));
            }
            set {
                this["StopLinkColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50")]
        public int LinkSelectDelay {
            get {
                return ((int)(this["LinkSelectDelay"]));
            }
            set {
                this["LinkSelectDelay"] = value;
            }
        }
    }
}
