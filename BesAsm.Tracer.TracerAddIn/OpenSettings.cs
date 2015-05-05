using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BesAsm.Tracer.TracerAddIn
{
  public class OpenSettings : ESRI.ArcGIS.Desktop.AddIns.Button
  {
    private TracerExtension _tracerExtension;

    public OpenSettings()
    {
      _tracerExtension = TracerExtension.GetExtension();
    }

    protected override void OnClick()
    {
      //
      //  TODO: Sample code showing how to access button host
      //
      try
      {
        ArcMap.Application.CurrentTool = null;
        _tracerExtension.ShowSettings();
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(
          ex.Message, "Error loading the tracer configuration dialog!",
          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }

    }
    protected override void OnUpdate()
    {
      Enabled = ArcMap.Application != null;
    }
  }

}
