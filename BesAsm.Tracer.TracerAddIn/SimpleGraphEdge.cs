using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BesAsm.Framework.Tracer;

namespace BesAsm.Tracer.TracerAddIn
{
  public class SimpleGraphEdge : IGraphEdge<object>
  {
    public int _edgeId;
    public object _sinkNode, _sourceNode;
    public object _base;

    public SimpleGraphEdge(int edgeId, object sourceNode, object sinkNode)
    {
      _edgeId = edgeId;      
      _sourceNode = sourceNode;
      _sinkNode = sinkNode;
    }

    public int EdgeId
    {
      get { return _edgeId; }
    }

    public object SinkNode
    {
      get { return _sinkNode; }
    }

    public object SourceNode
    {
      get { return _sourceNode; }
    }

    public object Base
    {
      get { return _base; }
      set { _base = value; }
    }

    public override string ToString()
    {
      return _sourceNode.ToString() + " -> " + _sinkNode.ToString();
    }
  }
}
