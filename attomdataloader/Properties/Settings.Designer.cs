﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace attomdataloader.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public string BatchSize {
            get {
                return ((string)(this["BatchSize"]));
            }
            set {
                this["BatchSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10000")]
        public string NotifyAfter {
            get {
                return ((string)(this["NotifyAfter"]));
            }
            set {
                this["NotifyAfter"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\RealtyTrac\\Incoming\\")]
        public string RootFolderExt {
            get {
                return ((string)(this["RootFolderExt"]));
            }
            set {
                this["RootFolderExt"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("TaxAssessor_")]
        public string TaxAssessorPrefix {
            get {
                return ((string)(this["TaxAssessorPrefix"]));
            }
            set {
                this["TaxAssessorPrefix"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Foreclosure_")]
        public string ForeclosurePrefix {
            get {
                return ((string)(this["ForeclosurePrefix"]));
            }
            set {
                this["ForeclosurePrefix"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Recorder_")]
        public string RecorderPrefix {
            get {
                return ((string)(this["RecorderPrefix"]));
            }
            set {
                this["RecorderPrefix"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("realtytrac.dbo.TaxAssessorImport")]
        public string TaxAssessorTable {
            get {
                return ((string)(this["TaxAssessorTable"]));
            }
            set {
                this["TaxAssessorTable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("realtytrac.dbo.ForeclosureImport")]
        public string ForeclosureTable {
            get {
                return ((string)(this["ForeclosureTable"]));
            }
            set {
                this["ForeclosureTable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("realtytrac.dbo.RecorderImport")]
        public string RecorderTable {
            get {
                return ((string)(this["RecorderTable"]));
            }
            set {
                this["RecorderTable"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("sftp1.realtytrac.com")]
        public string FTPAddress {
            get {
                return ((string)(this["FTPAddress"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("22")]
        public string FTPPort {
            get {
                return ((string)(this["FTPPort"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Reipro")]
        public string FTPUserName {
            get {
                return ((string)(this["FTPUserName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("GPazYk7!")]
        public string FTPPassword {
            get {
                return ((string)(this["FTPPassword"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("outgoing")]
        public string FTPDownloadDir {
            get {
                return ((string)(this["FTPDownloadDir"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("realtytrac.dbo.AVMEquityImport")]
        public string AVMTable {
            get {
                return ((string)(this["AVMTable"]));
            }
            set {
                this["AVMTable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("AVM_")]
        public string AVMPrefix {
            get {
                return ((string)(this["AVMPrefix"]));
            }
            set {
                this["AVMPrefix"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("key-897c027bf628e5c669c10d9f64ae7908")]
        public string SMTPAPIKey {
            get {
                return ((string)(this["SMTPAPIKey"]));
            }
            set {
                this["SMTPAPIKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ssh-rsa 2048 01:8e:c9:85:ba:37:df:29:60:66:eb:af:be:b2:b0:fd")]
        public string SshHostKeyFingerprint {
            get {
                return ((string)(this["SshHostKeyFingerprint"]));
            }
            set {
                this["SshHostKeyFingerprint"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\RealtyTrac\\Incoming\\GlobalLogs")]
        public string GlobalLogFile {
            get {
                return ((string)(this["GlobalLogFile"]));
            }
            set {
                this["GlobalLogFile"] = value;
            }
        }
    }
}