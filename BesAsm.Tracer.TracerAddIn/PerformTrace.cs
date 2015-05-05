using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace BesAsm.Tracer.TracerAddIn
{
  public class PerformTrace : ESRI.ArcGIS.Desktop.AddIns.Button
  {
    private TracerExtension _tracerExtension;

    public PerformTrace()
    {
      Enabled = false;
      _tracerExtension = TracerExtension.GetExtension();
    }

    protected override void OnClick()
    {
      try
      {
        _tracerExtension.PerformTrace();
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(
          ex.Message, "Error performing the trace!",
          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
      }
    }

    protected override void OnUpdate()
    {
      Enabled = _tracerExtension.StartLinkCount > 0 || _tracerExtension.StopLinkCount > 0;
    }
  }
}
