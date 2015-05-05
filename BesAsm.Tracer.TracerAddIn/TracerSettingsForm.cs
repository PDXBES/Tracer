using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using BesAsm.Framework.Tracer;

namespace BesAsm.Tracer.TracerAddIn
{
  public partial class TracerSettingsForm : Form
  {
    private IFeatureLayer traceFL;
    public string USField;
    public string DSField;
    private IList<SimpleGraphEdge> startEdges;
    private IList<SimpleGraphEdge> stopEdges;
    private bool hasDirtyTrace;
    private bool hasDirtyNetwork;
    private bool abortClose = false;

    public TracerSettingsForm()
    {
      InitializeComponent();

      this.USField = "";
      this.DSField = "";
      this.hasDirtyTrace = false;
      this.hasDirtyNetwork = false;
      this.startEdges = new List<SimpleGraphEdge>();
      this.stopEdges = new List<SimpleGraphEdge>();
    }

    public void SetupForm(IEnumDataset enumDataset, IList<SimpleGraphEdge> startEdges, IList<SimpleGraphEdge> stopEdges)
    {
      this.startEdges = new List<SimpleGraphEdge>();
      this.stopEdges = new List<SimpleGraphEdge>();

      //When re-entering the settings form, we do not know for sure if the
      //previously selected FeatureClass still exists in the document. However,
      //if it does it is neccesary to remember it.
      IFeatureLayer flHolder = this.traceFL;

      this.traceFL = null;
      this.hasDirtyNetwork = true;

      this.cmbFeatureLayer.SelectedItem = null;
      this.cmbFeatureLayer.Items.Clear();

      //Add all polyline FeatureClasses in the document to the FeatureClass
      //drop-down list.
      IDataset ds;
      ds = enumDataset.Next();
      while (ds != null)
      {
        try
        {
          if (ds.Type == esriDatasetType.esriDTFeatureClass)
          {
            IFeatureLayer fl;
            fl = (IFeatureLayer)ds;

            IFeatureClass fc;
            fc = fl.FeatureClass;

            if (fc.ShapeType == esriGeometryType.esriGeometryPolyline)
            {
              this.cmbFeatureLayer.Items.Add(new FeatureLayerWrapper(fl));
            }
          }
        }
        catch
        {
        }
        finally
        {
          ds = enumDataset.Next();
        }
      }

      //If possible, overwrite the combobox selection with the 
      //FeatureLayer that was selected previously

      foreach (FeatureLayerWrapper featureLayerWrapper in this.cmbFeatureLayer.Items)
      {
        if (featureLayerWrapper.FeatureLayer == flHolder)
        {
          this.cmbFeatureLayer.SelectedItem = featureLayerWrapper;

          foreach (SimpleGraphEdge g in startEdges)
          {
            this.startEdges.Add(g);
            this.lstStartPipes.Items.Add(g.ToString());
          }

          foreach (SimpleGraphEdge g in stopEdges)
          {
            this.stopEdges.Add(g);
            this.lstStopPipes.Items.Add(g.ToString());
          }

          this.traceFL = flHolder;
          this.hasDirtyNetwork = false;
          break;
        }
      }

      //If a Network was built previously, but the FeatureClass that formed that Network
      //has been removed from the document then throw an exception.
      if (this.traceFL == null && this.cmbFeatureLayer.Items.Count > 0)
      {
        this.cmbFeatureLayer.SelectedIndex = 0;
      }

      this.lstStartPipes.Items.Clear();
      foreach (SimpleGraphEdge edge in this.startEdges)
      {
        this.lstStartPipes.Items.Add(edge);
      }
      this.lstStopPipes.Items.Clear();
      foreach (SimpleGraphEdge edge in this.stopEdges)
      {
        this.lstStopPipes.Items.Add(edge);
      }

      //After a new call to setup, the Trace is not dirty until the Start/Stop
      //links are changed.
      this.hasDirtyTrace = false;

      return;
    }

    private void cmbFeatureLayer_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      this.hasDirtyTrace = true;
      this.hasDirtyNetwork = true;

      this.cmbUpstreamNodeField.Items.Clear();
      this.cmbDownstreamNodeField.Items.Clear();

      if (this.cmbFeatureLayer.SelectedItem == null)
      { return; }

      FeatureLayerWrapper featureLayerWrapper = (FeatureLayerWrapper)this.cmbFeatureLayer.SelectedItem;
      IFeatureClass featureClass = featureLayerWrapper.FeatureClass;

      //The selected FeatureLayer has changed, so the old start/stop links
      //are no longer valid. If the user clicks "Cancel", the previously
      //selected start/stop links are preserved in TracerExtension.			
      this.startEdges = new List<SimpleGraphEdge>();
      this.stopEdges = new List<SimpleGraphEdge>();
      this.lstStartPipes.Items.Clear();
      this.lstStopPipes.Items.Clear();

      //Add all fields of the currently selected FeatureLayer to both
      //up- and downstream field selectors. Up- and downstream node
      //fields are required to be strings.
      IFields fields = featureClass.Fields;
      for (int i = 0; i < fields.FieldCount; i++)
      {
        IField field = fields.get_Field(i);
        if (field.Type == esriFieldType.esriFieldTypeString || field.Type == esriFieldType.esriFieldTypeInteger)
        {
          this.cmbUpstreamNodeField.Items.Add(field.Name);
          this.cmbDownstreamNodeField.Items.Add(field.Name);
        }
      }

      if (this.cmbUpstreamNodeField.Items.Contains(this.USField))
      {
        this.cmbUpstreamNodeField.SelectedItem = this.USField;
      }
      else
      {
        foreach (string fieldName in Properties.Settings.Default.KnownUsNodeFieldNames)
        {
          if (this.cmbUpstreamNodeField.Items.Contains(fieldName))
          {
            this.cmbUpstreamNodeField.SelectedItem = fieldName;
            break;
          }
        }
      }

      if (this.cmbDownstreamNodeField.Items.Contains(this.DSField))
      {
        this.cmbDownstreamNodeField.SelectedItem = this.DSField;
      }
      else
      {
        foreach (string fieldName in Properties.Settings.Default.KnownDsNodeFieldNames)
        {
          if (this.cmbDownstreamNodeField.Items.Contains(fieldName))
          {
            this.cmbDownstreamNodeField.SelectedItem = fieldName;
            break;
          }
        }
      }
    }

    private void btnSaveChanges_Click(object sender, System.EventArgs e)
    {
      try
      {
        this.abortClose = false;

        if(!_CheckFieldTypes())
        {
          this.abortClose = true;
          this.hasDirtyNetwork = false;
          this.hasDirtyTrace = false;
          this.DialogResult = DialogResult.Abort;
          return;
        }

        FeatureLayerWrapper featureLayerWrapper = (FeatureLayerWrapper)this.cmbFeatureLayer.SelectedItem;
        this.traceFL = featureLayerWrapper.FeatureLayer;
        this.USField = (string)this.cmbUpstreamNodeField.SelectedItem;
        this.DSField = (string)this.cmbDownstreamNodeField.SelectedItem;
        this.DialogResult = DialogResult.OK;        
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, "Could not save settings: " + ex.ToString(), "Error saving settings");
        this.abortClose = true;
        this.hasDirtyNetwork = false;
        this.hasDirtyTrace = false;
        this.DialogResult = DialogResult.Abort;
      }
    }

    private bool _CheckFieldTypes()
    {
      FeatureLayerWrapper featureLayerWrapper = (FeatureLayerWrapper)this.cmbFeatureLayer.SelectedItem;
      string usFieldName = (string)this.cmbUpstreamNodeField.SelectedItem;
      string dsFieldName = (string)this.cmbDownstreamNodeField.SelectedItem;

      int usFieldIndex = featureLayerWrapper.FeatureClass.FindField(usFieldName);
      int dsFieldIndex = featureLayerWrapper.FeatureClass.FindField(dsFieldName);
      IField usField = featureLayerWrapper.FeatureClass.Fields.get_Field(usFieldIndex);
      IField dsField = featureLayerWrapper.FeatureClass.Fields.get_Field(dsFieldIndex);

      if (usField.Type != dsField.Type)
      {
        MessageBox.Show(this, "The field types for the Upsteam and Downstream Node are not the same.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
      }

      return true;
    }

    private void btnCancel_Click(object sender, System.EventArgs e)
    {
      this.abortClose = false;
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnRemoveStart_Click(object sender, System.EventArgs e)
    {
      //Remove all items selected in start pipe ListBox from the startEdges collection
      foreach (SimpleGraphEdge g in this.lstStartPipes.SelectedItems)
      {
        this.startEdges.Remove(g);
        this.hasDirtyTrace = true;
      }

      //Repopulate the start pipe ListBox from the new startEdges collection
      this.lstStartPipes.Items.Clear();
      foreach (SimpleGraphEdge g in this.startEdges)
      {
        this.lstStartPipes.Items.Add(g);
      }
    }

    private void btnRemoveStop_Click(object sender, System.EventArgs e)
    {
      //Remove all items selected in stop pipe ListBox from the stopEdges collection
      foreach (SimpleGraphEdge g in this.lstStopPipes.SelectedItems)
      {
        this.stopEdges.Remove(g);
        this.hasDirtyTrace = true;
      }

      //Repopulate the stop pipe ListBox from the new stopEdges collection
      this.lstStopPipes.Items.Clear();
      foreach (SimpleGraphEdge g in this.stopEdges)
      {
        this.lstStopPipes.Items.Add(g);
      }
    }

    private void cmbUpstreamNodeField_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      this.lstStartPipes.Items.Clear();
      this.lstStopPipes.Items.Clear();

      foreach (SimpleGraphEdge g in this.startEdges)
      {
        this.startEdges.Remove(g);
      }

      foreach (SimpleGraphEdge g in this.stopEdges)
      {
        this.stopEdges.Remove(g);
      }

      this.hasDirtyTrace = true;
      this.hasDirtyNetwork = true;
    }

    private void cmbDownstreamNodeField_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      this.lstStartPipes.Items.Clear();
      this.lstStopPipes.Items.Clear();

      foreach (SimpleGraphEdge g in this.startEdges)
      {
        this.startEdges.Remove(g);
      }

      foreach (SimpleGraphEdge g in this.stopEdges)
      {
        this.stopEdges.Remove(g);
      }

      this.hasDirtyTrace = true;
    }

    public bool DirtyTrace
    {
      get { return this.hasDirtyTrace; }
    }

    public bool DirtyNetwork
    {
      get { return this.hasDirtyNetwork; }
    }

    public IList<SimpleGraphEdge> StartEdges
    {
      get { return this.startEdges; }
    }

    public IList<SimpleGraphEdge> StopEdges
    {
      get { return this.stopEdges; }
    }

    public IFeatureLayer TraceFL
    {
      get { return this.traceFL; }
    }

    public bool ZoomToTrace
    {
      get { return this.checkBox1.Checked; }
    }

    /// <summary>
    /// Provides a wrapper around IFeatureClass that allows us to add
    /// IFeatureClass to a ListBox and retrieve a meaningful ToString(),
    /// since ESRI's ToString implementation is shit (ie returns 
    /// "COM__OBJECT" for all IFeatureClass objects.)
    /// </summary>
    protected class FeatureLayerWrapper
    {
      private IFeatureClass featureClass;
      private IFeatureLayer featureLayer;
      private string description;

      public FeatureLayerWrapper(IFeatureLayer fl)
      {
        this.featureLayer = fl;
        this.featureClass = fl.FeatureClass;
        string name = fl.Name;
        int dotLocation = name.LastIndexOf(".");
        if (dotLocation == -1)
        {
          this.description = fl.Name;
        }
        else
        {
          this.description = name.Substring(dotLocation + 1);
        }
      }

      public override string ToString()
      {
        return this.description;
      }

      public IFeatureClass FeatureClass
      {
        get { return this.featureClass; }
      }

      public IFeatureLayer FeatureLayer
      {
        get { return this.featureLayer; }
      }
    }

    private void TracerSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.abortClose)
      {
        e.Cancel = true;
        this.abortClose = false;
      }
    }
  }
}
