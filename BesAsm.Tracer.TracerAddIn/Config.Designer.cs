//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BesAsm.Tracer.TracerAddIn {
    using ESRI.ArcGIS.Framework;
    using ESRI.ArcGIS.ArcMapUI;
    using System;
    using System.Collections.Generic;
    using ESRI.ArcGIS.Desktop.AddIns;
    
    
    /// <summary>
    /// A class for looking up declarative information in the associated configuration xml file (.esriaddinx).
    /// </summary>
    internal static class ThisAddIn {
        
        internal static string Name {
            get {
                return "BesAsm.Tracer.TracerAddIn";
            }
        }
        
        internal static string AddInID {
            get {
                return "{c79bf049-aded-4948-94dd-369f83ca59f8}";
            }
        }
        
        internal static string Company {
            get {
                return "City of Portland";
            }
        }
        
        internal static string Version {
            get {
                return "1.1";
            }
        }
        
        internal static string Description {
            get {
                return "Stand-alone add-in for performing network traces";
            }
        }
        
        internal static string Author {
            get {
                return "BES ASM";
            }
        }
        
        internal static string Date {
            get {
                return "6/17/2015";
            }
        }
        
        internal static ESRI.ArcGIS.esriSystem.UID ToUID(this System.String id) {
            ESRI.ArcGIS.esriSystem.UID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = id;
            return uid;
        }
        
        /// <summary>
        /// A class for looking up Add-in id strings declared in the associated configuration xml file (.esriaddinx).
        /// </summary>
        internal class IDs {
            
            /// <summary>
            /// Returns 'City_of_Portland_BesAsm.Tracer.TracerAddIn_OpenSettings', the id declared for Add-in Button class 'OpenSettings'
            /// </summary>
            internal static string OpenSettings {
                get {
                    return "City_of_Portland_BesAsm.Tracer.TracerAddIn_OpenSettings";
                }
            }
            
            /// <summary>
            /// Returns 'City_of_Portland_BesAsm.Tracer.TracerAddIn_SelectStartLinks', the id declared for Add-in Tool class 'SelectStartLinks'
            /// </summary>
            internal static string SelectStartLinks {
                get {
                    return "City_of_Portland_BesAsm.Tracer.TracerAddIn_SelectStartLinks";
                }
            }
            
            /// <summary>
            /// Returns 'City_of_Portland_BesAsm.Tracer.TracerAddIn_SelectStopLinks', the id declared for Add-in Tool class 'SelectStopLinks'
            /// </summary>
            internal static string SelectStopLinks {
                get {
                    return "City_of_Portland_BesAsm.Tracer.TracerAddIn_SelectStopLinks";
                }
            }
            
            /// <summary>
            /// Returns 'City_of_Portland_BesAsm.Tracer.TracerAddIn_ClearTrace', the id declared for Add-in Button class 'ClearTrace'
            /// </summary>
            internal static string ClearTrace {
                get {
                    return "City_of_Portland_BesAsm.Tracer.TracerAddIn_ClearTrace";
                }
            }
            
            /// <summary>
            /// Returns 'City_of_Portland_BesAsm.Tracer.TracerAddIn_PerformTrace', the id declared for Add-in Button class 'PerformTrace'
            /// </summary>
            internal static string PerformTrace {
                get {
                    return "City_of_Portland_BesAsm.Tracer.TracerAddIn_PerformTrace";
                }
            }
            
            /// <summary>
            /// Returns 'City_of_Portland_BesAsm.Tracer.TracerAddIn_TracerExtension', the id declared for Add-in Extension class 'TracerExtension'
            /// </summary>
            internal static string TracerExtension {
                get {
                    return "City_of_Portland_BesAsm.Tracer.TracerAddIn_TracerExtension";
                }
            }
        }
    }
    
internal static class ArcMap
{
  private static IApplication s_app = null;
  private static IDocumentEvents_Event s_docEvent;

  public static IApplication Application
  {
    get
    {
      if (s_app == null)
        s_app = Internal.AddInStartupObject.GetHook<IMxApplication>() as IApplication;

      return s_app;
    }
  }

  public static IMxDocument Document
  {
    get
    {
      if (Application != null)
        return Application.Document as IMxDocument;

      return null;
    }
  }
  public static IMxApplication ThisApplication
  {
    get { return Application as IMxApplication; }
  }
  public static IDockableWindowManager DockableWindowManager
  {
    get { return Application as IDockableWindowManager; }
  }
  public static IDocumentEvents_Event Events
  {
    get
    {
      s_docEvent = Document as IDocumentEvents_Event;
      return s_docEvent;
    }
  }
}

namespace Internal
{
  [StartupObjectAttribute()]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public sealed partial class AddInStartupObject : AddInEntryPoint
  {
    private static AddInStartupObject _sAddInHostManager;
    private List<object> m_addinHooks = null;

    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    public AddInStartupObject()
    {
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override bool Initialize(object hook)
    {
      bool createSingleton = _sAddInHostManager == null;
      if (createSingleton)
      {
        _sAddInHostManager = this;
        m_addinHooks = new List<object>();
        m_addinHooks.Add(hook);
      }
      else if (!_sAddInHostManager.m_addinHooks.Contains(hook))
        _sAddInHostManager.m_addinHooks.Add(hook);

      return createSingleton;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    protected override void Shutdown()
    {
      _sAddInHostManager = null;
      m_addinHooks = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
    internal static T GetHook<T>() where T : class
    {
      if (_sAddInHostManager != null)
      {
        foreach (object o in _sAddInHostManager.m_addinHooks)
        {
          if (o is T)
            return o as T;
        }
      }

      return null;
    }

    // Expose this instance of Add-in class externally
    public static AddInStartupObject GetThis()
    {
      return _sAddInHostManager;
    }
  }
}
}
