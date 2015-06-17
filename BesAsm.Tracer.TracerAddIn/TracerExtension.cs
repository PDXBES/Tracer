using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Display;
using BesAsm.Framework.Tracer;

namespace BesAsm.Tracer.TracerAddIn
{
  public class TracerExtension : ESRI.ArcGIS.Desktop.AddIns.Extension
  {
    protected override void OnStartup()
    {
    }

    protected override void OnShutdown()
    {      
    }

    private IMap map;

    private TracerSettingsForm _tracerSettingsForm;

    private IFeatureLayer _traceFeatureLayer;
    private string _usField;
    private string _dsField;

    private SimpleNetwork _traceNetwork;
    private IList<SimpleGraphEdge> _startEdges;
    private IList<SimpleGraphEdge> _stopEdges;

    private List<IElement> _graphicElements;

    ESRI.ArcGIS.Display.INewCircleFeedback2 _circleFeedback;

    private static TracerExtension _tracerExtension;

    private bool _isConfigured = false;

    /// <summary>
    /// Constructs a TracerExtension object
    /// </summary>
    public TracerExtension()
    {
      this._startEdges = new List<SimpleGraphEdge>();
      this._stopEdges = new List<SimpleGraphEdge>();
      this._traceFeatureLayer = null;

      _graphicElements = new List<IElement>();

      _tracerExtension = this;
    }

    internal static TracerExtension GetExtension()
    {
      if (_tracerExtension == null)
      {
        UID uid = new UIDClass();
        uid.Value = ThisAddIn.IDs.TracerExtension;
        ArcMap.Application.FindExtensionByCLSID(uid);
      }
      return _tracerExtension;
    }

    /// <summary>
    /// Displays the dialog for setting up the tracer
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments object</param>
    internal void ShowSettings()
    {
      IDocumentDatasets docDatasets;

      try
      {
        docDatasets = (IDocumentDatasets)ArcMap.Document;
        if (docDatasets.Datasets == null)
        {
          throw new Exception("No DataSets loaded.");
        }

        if (this._tracerSettingsForm == null)
          _tracerSettingsForm = new TracerSettingsForm();

        this._tracerSettingsForm.SetupForm(docDatasets.Datasets, this._startEdges, this._stopEdges);

        if (this._tracerSettingsForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        {
          return;
        }

        IMouseCursor mouseCursor;
        mouseCursor = new MouseCursorClass();
        mouseCursor.SetCursor(2);

        if (_tracerSettingsForm.DirtyNetwork || !_isConfigured)
        {
          _isConfigured = true;

          _traceFeatureLayer = this._tracerSettingsForm.TraceFL;
          _usField = this._tracerSettingsForm.USField;
          _dsField = this._tracerSettingsForm.DSField;

          int featureCount = _traceFeatureLayer.FeatureClass.FeatureCount(null);

          int[] edgeIds = new int[featureCount];
          int edgeIdIndex = _traceFeatureLayer.FeatureClass.FindField(_traceFeatureLayer.FeatureClass.OIDFieldName);
          string[] sourceEdges = new string[featureCount];
          int sourceEdgeIndex = _traceFeatureLayer.FeatureClass.FindField(_usField);
          string[] sinkEdges = new string[featureCount];
          int sinkEdgeIndex = _traceFeatureLayer.FeatureClass.FindField(_dsField);

          IStepProgressor progressBar = ArcMap.Application.StatusBar.ProgressBar;
          new FloatingProgressBar(featureCount);

          ArcMap.Application.StatusBar.ProgressAnimation.Show();
          ArcMap.Application.StatusBar.ProgressAnimation.Play(0, -1, -1);
          ArcMap.Application.StatusBar.ProgressAnimation.Message = "Building Network...";
          progressBar.Position = 0;
          progressBar.MinRange = 0;
          progressBar.MaxRange = featureCount;
          progressBar.Message = "Building Network...";
          progressBar.Show();

          try
          {
            IFeatureCursor cursor = _traceFeatureLayer.Search(null, true);
            IFeature edgeFeature = cursor.NextFeature();

            IList<SimpleGraphEdge> graphEdges = new List<SimpleGraphEdge>();

            int badEdgeCount = 0;

            while (edgeFeature != null)
            {
              try
              {
                object dbValue = edgeFeature.get_Value(sourceEdgeIndex);
                if (dbValue == DBNull.Value)
                {
                  badEdgeCount++;
                  continue;
                }

                object sourceNode = dbValue;

                dbValue = edgeFeature.get_Value(sinkEdgeIndex);
                if (dbValue == DBNull.Value)
                {
                  badEdgeCount++;
                  continue;
                }

                object sinkNode = dbValue;

                //DME pipes have a whitespace character appended to the FRM_NODE

                //TODO: DME pipes with unknown up- or downstream nodes have
                //XXXX. A better implemntation would not have this hardcoded.
                if (sourceNode is string)
                {
                  sourceNode = ((string)sourceNode).Trim();
                  if (((string)sourceNode).StartsWith("XXXX"))
                  {
                    badEdgeCount++;
                    continue;
                  }
                }

                if (sinkNode is string)
                {
                  sinkNode = ((string)sinkNode).Trim();
                  if (((string)sinkNode).StartsWith("XXXX"))
                  {
                    badEdgeCount++;
                    continue;
                  }
                }

                SimpleGraphEdge graphEdge;
                graphEdge = new SimpleGraphEdge(edgeFeature.OID, sourceNode, sinkNode);
                graphEdges.Add(graphEdge);
              }
              catch
              {
                badEdgeCount++;
                continue;
              }
              finally
              {
                progressBar.Step();
                edgeFeature = cursor.NextFeature();
              }
            }

            _traceNetwork = new SimpleNetwork(graphEdges);

            string successMessage = "Tracer configured: FeatureClass='"
              + _traceFeatureLayer.Name + "'. US Field='" + _usField + "'. DS Field='" + _dsField + "'.";

            if (badEdgeCount > 0)
              successMessage += badEdgeCount + " links were excluded due to bad node values (null, 'XXXX', etc...).";

            System.Windows.Forms.MessageBox.Show(successMessage);
          }
          catch (Exception ex)
          {
            throw new Exception("Could not create Network. " + ex.ToString());
          }
          finally
          {
            mouseCursor = null;
            ArcMap.Application.StatusBar.PlayProgressAnimation(false); // ProgressAnimation.Stop();
            ArcMap.Application.StatusBar.ProgressAnimation.Hide();
            progressBar.Hide();
          }
        }

        if (_tracerSettingsForm.DirtyTrace)
        {
          _startEdges = _tracerSettingsForm.StartEdges;
          _stopEdges = _tracerSettingsForm.StopEdges;

          RebuildStartStopLinkGraphics();

          this.PerformTrace();
        }
      }

      catch (Exception ex)
      {
        if (this._tracerSettingsForm.Visible)
        {
          this._tracerSettingsForm.Hide();
        }
        System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error Building Network.");
        _isConfigured = false;
      }
    }

    /// <summary>
    /// Performs the tracing routine
    /// </summary>
    public void PerformTrace()
    {
      // Setup and get layer for tracing
      ArcMap.Application.StatusBar.ProgressAnimation.Show();
      ArcMap.Application.StatusBar.ProgressAnimation.Play(0, -1, -1);
      ArcMap.Application.StatusBar.ProgressAnimation.Message = "Tracing...";

      IMouseCursor mouseCursor;
      mouseCursor = new MouseCursorClass();
      mouseCursor.SetCursor(2);

      ESRI.ArcGIS.Carto.IActiveView activeView;
      activeView = (ESRI.ArcGIS.Carto.IActiveView)ArcMap.Document.FocusMap;

      activeView.Refresh();

      ESRI.ArcGIS.Carto.IMap map;
      map = activeView.FocusMap;

      IFeatureLayer featureLayer = null;
      IEnumLayer enumLayer;

      enumLayer = map.get_Layers(null, true);
      ILayer l = (ILayer)enumLayer.Next();
      while (l != null)
      {
        if (l is IFeatureLayer)
        {
          featureLayer = (IFeatureLayer)l;
          if (featureLayer == _traceFeatureLayer) { break; }
        }
        l = (ILayer)enumLayer.Next();
      }
      if (featureLayer == null)
      {
        System.Windows.Forms.MessageBox.Show("Could not find '" + _traceFeatureLayer.Name + "' in TOC.");
        return;
      }

      //this.traceNetwork.ClearSubNetwork();

      // Perform trace and select the traced subnetwork into temp array
      ICollection<SimpleGraphEdge> selectedEdges = _traceNetwork.Trace<SimpleGraphEdge, object>(_startEdges, _stopEdges);

      IFeatureSelection featureSelection;
      featureSelection = (IFeatureSelection)featureLayer;
      featureSelection.Clear();
      featureSelection.SelectionSet.Refresh();

      if (selectedEdges.Count <= 0)
      {
        activeView.Refresh();
        return;
      }

      int[] selectedEdgeIds = selectedEdges.GetIdList<SimpleGraphEdge, object>();
      featureSelection.SelectionSet.AddList(selectedEdges.Count, ref selectedEdgeIds[0]);

      mouseCursor = null;

      // Zoom to traced subnetwork if desired
      if (this._tracerSettingsForm.ZoomToTrace)
      {
        ESRI.ArcGIS.Geometry.IGeometryFactory pGeomFactory;
        pGeomFactory = new ESRI.ArcGIS.Geometry.GeometryEnvironmentClass();

        ESRI.ArcGIS.Geometry.IEnumGeometry pEnumGeom;
        ESRI.ArcGIS.Geodatabase.IEnumGeometryBind pEnumGeomBind;
        pEnumGeom = new ESRI.ArcGIS.Geodatabase.EnumFeatureGeometry();
        pEnumGeomBind = (ESRI.ArcGIS.Geodatabase.IEnumGeometryBind)pEnumGeom;
        pEnumGeomBind.BindGeometrySource(null, featureSelection.SelectionSet);

        ESRI.ArcGIS.Geometry.IGeometry pGeom;
        pGeom = pGeomFactory.CreateGeometryFromEnumerator(pEnumGeom);
        pGeom.Envelope.Expand(1.2, 1.2, true);
        activeView.Extent = pGeom.Envelope;
      }

      ArcMap.Application.StatusBar.ProgressAnimation.Stop();
      ArcMap.Application.StatusBar.ProgressAnimation.Hide();

      activeView.Refresh();

      return;
    }

    /// <summary>
    /// Initializes the tracer parameters
    /// </summary>
    /// <param name="network">The network to trace</param>
    /// <param name="fl">The feature layer representing the network</param>
    /// <param name="usField">The field of the network for the source/upstream side of the edges</param>
    /// <param name="dsField">The field of the network for the sink/downstream side of the edges</param>
    /// <param name="map">The map containing the feature layer</param>
    public void SetupTracer(SimpleNetwork network, IFeatureLayer fl, string usField, string dsField, IMap map)
    {
      this._traceNetwork = network;
      this._traceFeatureLayer = fl;
      this._usField = usField;
      this._dsField = dsField;
      this.map = map;
    }

    public bool IsConfigured
    {
      get { return _isConfigured; }
    }

    ///<summary>Finds all the features in a GeoFeature layer by supplying a point. The point could come from a mouse click on the map.</summary>
    ///  
    ///<param name="searchTolerance">A System.Double that is the number of map units to search. Example: 25</param>
    ///<param name="point">An IPoint interface in map units where the user clicked on the map</param>
    ///<param name="geoFeatureLayer">An ILayer interface to search upon</param>
    ///<param name="activeView">An IActiveView interface</param>
    ///   
    ///<returns>An IFeatureCursor interface is returned containing all the selected features found in the GeoFeatureLayer.</returns>
    ///   
    ///<remarks></remarks>
    private List<SimpleGraphEdge> GetAllFeaturesFromPointSearchInGeoFeatureLayer(int x, int y, int screenTolerance)
    {

      List<SimpleGraphEdge> selectedEdges = new List<SimpleGraphEdge>();

      if (screenTolerance < 0 || _traceFeatureLayer == null || ArcMap.Document.ActiveView == null)
      {
        _circleFeedback.Stop();
        _circleFeedback = null;
        return selectedEdges;
      }

      IActiveView activeView = ArcMap.Document.ActiveView;

      IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y) as IPoint;
      double mapTolerance = Math.Abs(point.X - activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(x + screenTolerance, y).X);

      IMap map = ArcMap.Document.ActiveView.FocusMap;

      // Expand the points envelope to give better search results    
      IEnvelope envelope = point.Envelope;
      envelope.Expand(mapTolerance, mapTolerance, false);

      IFeatureClass featureClass = _traceFeatureLayer.FeatureClass;

      System.String shapeFieldName = featureClass.ShapeFieldName;

      // Create a new spatial filter and use the new envelope as the geometry    
      ISpatialFilter spatialFilter = new SpatialFilterClass();
      spatialFilter.Geometry = envelope;
      spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
      spatialFilter.set_OutputSpatialReference(shapeFieldName, map.SpatialReference);
      spatialFilter.GeometryField = shapeFieldName;

      // Do the searh
      IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);

      if (_circleFeedback == null)
      {
        _circleFeedback = new ESRI.ArcGIS.Display.NewCircleFeedback();

        _circleFeedback.Display = ArcMap.Document.ActiveView.ScreenDisplay;
        _circleFeedback.Start(point);
      }

      IPoint newPoint = point;
      newPoint.X += mapTolerance;
      _circleFeedback.MoveTo(newPoint);

      IFeature feature = featureCursor.NextFeature();
      int featureCount = 0;
      int lastFeatureOid = 0;

      if (feature == null)
      {
        System.Threading.Thread.Sleep(Properties.Settings.Default.LinkSelectDelay);
        _circleFeedback.Stop();
        _circleFeedback = null;

        return selectedEdges;
      }

      int oidFieldIndex = feature.Fields.FindField(featureClass.OIDFieldName);

      while (feature != null)
      {
        lastFeatureOid = (int)feature.get_Value(oidFieldIndex);

        feature = featureCursor.NextFeature();

        if (!_traceNetwork.Contains(lastFeatureOid))
          continue;

        featureCount++;
        selectedEdges.Add(_traceNetwork[lastFeatureOid]);
      }

      if (featureCount > 1 && screenTolerance > 1)
      {
        return GetAllFeaturesFromPointSearchInGeoFeatureLayer(x, y, screenTolerance - 1);
      }

      _circleFeedback.Stop();
      _circleFeedback = null;

      return selectedEdges;

    }

    public void SelectStartLink(int x, int y)
    {
      int screenTolerance = Properties.Settings.Default.DefaultClickTolerance;

      List<SimpleGraphEdge> selectedEdges = GetAllFeaturesFromPointSearchInGeoFeatureLayer(x, y, screenTolerance);

      if (selectedEdges.Count == 0)
      {
        MessageBox.Show("No Link Was Selected", "Unable to Add Start Link.",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
      else if (selectedEdges.Count > 1)
      {
        MessageBox.Show("More than one start link was selected. Please zoom in and refine your selection", "Unable to Add Start Link.",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      SimpleGraphEdge edge = selectedEdges[0];

      if (_startEdges.Contains(edge))
      {
        MessageBox.Show("The Selected Link is Already a Start Link", "Unable to Add Start Link.",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      _startEdges.Add(edge);

      IGeometry line;
      line = _traceFeatureLayer.FeatureClass.GetFeature(edge.EdgeId).Shape;

      AddGraphicToMap(line, Properties.Settings.Default.StartLinkColor);

      MessageBox.Show("Added Start Link '" + edge.ToString() + "'", "Succesfully Added Start Link.",
        MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void SelectStopLink(int x, int y)
    {
      int screenTolerance = Properties.Settings.Default.DefaultClickTolerance;

      List<SimpleGraphEdge> selectedEdges = GetAllFeaturesFromPointSearchInGeoFeatureLayer(x, y, screenTolerance);

      if (selectedEdges.Count == 0)
      {
        MessageBox.Show("No Link Was Selected", "Unable to Add Stop Link.",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }
      else if (selectedEdges.Count > 1)
      {
        MessageBox.Show("More than one stop link was selected. Please zoom in and refine your selection", "Unable to Add Stop Link.",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      SimpleGraphEdge edge = selectedEdges[0];

      if (_stopEdges.Contains(edge))
      {
        MessageBox.Show("The Selected Link is Already a Stop Link", "Unable to Add Stop Link.",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      _stopEdges.Add(edge);

      IGeometry line;
      line = _traceFeatureLayer.FeatureClass.GetFeature(edge.EdgeId).Shape;

      AddGraphicToMap(line, Properties.Settings.Default.StopLinkColor);

      MessageBox.Show("Added Stop Link '" + edge.ToString() + "'", "Succesfully Added Stop Link.",
        MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    ///<summary>Draw a specified graphic on the map using the supplied colors.</summary>
    ///      
    ///<param name="map">An IMap interface.</param>
    ///<param name="geometry">An IGeometry interface. It can be of the geometry type: esriGeometryPoint, esriGeometryPolyline, or esriGeometryPolygon.</param>
    ///<param name="rgbColor">An IRgbColor interface. The color to draw the geometry.</param>
    ///<param name="outlineRgbColor">An IRgbColor interface. For those geometry's with an outline it will be this color.</param>
    ///      
    ///<remarks>Calling this function will not automatically make the graphics appear in the map area. Refresh the map area after after calling this function with Methods like IActiveView.Refresh or IActiveView.PartialRefresh.</remarks>
    public void AddGraphicToMap(ESRI.ArcGIS.Geometry.IGeometry geometry, System.Drawing.Color color)
    {
      IRgbColor rgbColor = new RgbColorClass();
      IRgbColor outlineRgbColor = new RgbColorClass();

      rgbColor.Red = color.R;
      rgbColor.Green = color.G;
      rgbColor.Blue = color.B;

      outlineRgbColor.Red = color.R;
      outlineRgbColor.Green = color.G;
      outlineRgbColor.Blue = color.B;

      ESRI.ArcGIS.Carto.IMap map = ArcMap.Document.FocusMap;
      ESRI.ArcGIS.Carto.IGraphicsContainer graphicsContainer = (ESRI.ArcGIS.Carto.IGraphicsContainer)map; // Explicit Cast
      ESRI.ArcGIS.Carto.IElement element = null;
      if ((geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
      {
        // Marker symbols
        ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
        simpleMarkerSymbol.Color = rgbColor;
        simpleMarkerSymbol.Outline = true;
        simpleMarkerSymbol.OutlineColor = outlineRgbColor;
        simpleMarkerSymbol.Size = 15;
        simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;

        ESRI.ArcGIS.Carto.IMarkerElement markerElement = new ESRI.ArcGIS.Carto.MarkerElementClass();
        markerElement.Symbol = simpleMarkerSymbol;
        element = (ESRI.ArcGIS.Carto.IElement)markerElement; // Explicit Cast
      }
      else if ((geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
      {
        //  Line elements
        ESRI.ArcGIS.Display.ISimpleLineSymbol simpleLineSymbol = new ESRI.ArcGIS.Display.SimpleLineSymbolClass();
        simpleLineSymbol.Color = rgbColor;
        simpleLineSymbol.Style = ESRI.ArcGIS.Display.esriSimpleLineStyle.esriSLSSolid;
        simpleLineSymbol.Width = 5;

        ESRI.ArcGIS.Carto.ILineElement lineElement = new ESRI.ArcGIS.Carto.LineElementClass();
        lineElement.Symbol = simpleLineSymbol;
        element = (ESRI.ArcGIS.Carto.IElement)lineElement; // Explicit Cast
      }
      else if ((geometry.GeometryType) == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
      {
        // Polygon elements
        ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
        simpleFillSymbol.Color = rgbColor;
        simpleFillSymbol.Style = ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal;
        ESRI.ArcGIS.Carto.IFillShapeElement fillShapeElement = new ESRI.ArcGIS.Carto.PolygonElementClass();
        fillShapeElement.Symbol = simpleFillSymbol;
        element = (ESRI.ArcGIS.Carto.IElement)fillShapeElement; // Explicit Cast
      }
      if (!(element == null))
      {
        element.Geometry = geometry;
        graphicsContainer.AddElement(element, 0);

        _graphicElements.Add(element);
      }

      ArcMap.Document.ActiveView.Refresh();
    }

    public void ClearStartStopLinks()
    {
      _startEdges.Clear();
      _stopEdges.Clear();

      RebuildStartStopLinkGraphics();
    }

    public void RebuildStartStopLinkGraphics()
    {
      ESRI.ArcGIS.Carto.IMap map = ArcMap.Document.FocusMap;
      ESRI.ArcGIS.Carto.IGraphicsContainer graphicsContainer = (ESRI.ArcGIS.Carto.IGraphicsContainer)map; // Explicit Cast

      foreach (IElement element in _graphicElements)
      {
        graphicsContainer.DeleteElement(element);
      }
      _graphicElements.Clear();

      foreach (SimpleGraphEdge edge in _startEdges)
      {
        IGeometry line;
        line = _traceFeatureLayer.FeatureClass.GetFeature(edge.EdgeId).Shape;

        AddGraphicToMap(line, Properties.Settings.Default.StartLinkColor);
      }

      foreach (SimpleGraphEdge edge in _stopEdges)
      {
        IGeometry line;
        line = _traceFeatureLayer.FeatureClass.GetFeature(edge.EdgeId).Shape;

        AddGraphicToMap(line, Properties.Settings.Default.StopLinkColor);
      }

      ArcMap.Document.ActiveView.Refresh();
    }

    public int StartLinkCount
    {
      get { return _startEdges.Count; }
    }

    public int StopLinkCount
    {
      get { return _stopEdges.Count; }
    }

  }

}
