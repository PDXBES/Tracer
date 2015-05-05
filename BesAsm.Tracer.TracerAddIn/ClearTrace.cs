using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace BesAsm.Tracer.TracerAddIn
{
  public class ClearTrace : ESRI.ArcGIS.Desktop.AddIns.Button
  {
    private TracerExtension _tracerExtension;

    public ClearTrace()
    {
      Enabled = false;
      _tracerExtension = TracerExtension.GetExtension();
    }

    protected override void OnClick()
    {
      try
      {
        _tracerExtension.ClearStartStopLinks();
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(
          ex.Message, "Error clearing start/stop links!",
          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
    }

    protected override void OnUpdate()
    {
      Enabled = _tracerExtension.StartLinkCount > 0 || _tracerExtension.StopLinkCount > 0;
    }

  }
}
