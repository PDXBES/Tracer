using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using BesAsm.Framework.Tracer;

namespace BesAsm.Tracer.TracerAddIn
{
  public class SelectStartLinks : ESRI.ArcGIS.Desktop.AddIns.Tool
  {
    private TracerExtension _tracerExtension;        

    public SelectStartLinks()
    {
      Enabled = false;
      _tracerExtension = TracerExtension.GetExtension();
    }

    protected override void OnMouseDown(MouseEventArgs arg)
    {
      try
      {       
        _tracerExtension.SelectStartLink(arg.X, arg.Y);                  
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(
          ex.Message, "Error adding start link!",
          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
      finally
      {
        ArcMap.Application.CurrentTool = null;
      }
    }        

    protected override void OnUpdate()
    {
      Enabled = _tracerExtension.IsConfigured;
    }
    
  }
 
}
