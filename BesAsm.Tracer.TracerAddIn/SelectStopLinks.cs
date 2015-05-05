using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BesAsm.Tracer.TracerAddIn
{
  public class SelectStopLinks : ESRI.ArcGIS.Desktop.AddIns.Tool
  {
   private TracerExtension _tracerExtension;        

    public SelectStopLinks()
    {
      Enabled = false;
      _tracerExtension = TracerExtension.GetExtension();
    }


    protected override void OnMouseDown(MouseEventArgs arg)
    {
      try
      {
        _tracerExtension.SelectStopLink(arg.X, arg.Y);
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(
          ex.Message, "Error adding stop link!",
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
