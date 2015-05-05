using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BesAsm.Framework.Tracer
{
  /// <summary>
  /// A Network of Edges
  /// </summary> 
  internal class Network<ET, NT> : KeyedCollection<int, ET> where ET : IGraphEdge<NT>
  {
    private Dictionary<NT, List<ET>> sinkNodeList;
    private ICollection<ET> subNetwork;

    private bool dirty;
    private bool recursing;

    /// <summary>
    /// Initializes a new instance of the <see cref="Network&lt;GE&gt;"/> class.
    /// </summary>
    /// <param name="edges">The edges to trace.</param>
    internal Network(IList<ET> edges)
      : base(null, 5)
    {
      foreach (ET edge in edges)
        base.Add(edge);

      dirty = true;
      recursing = false;
      subNetwork = new HashSet<ET>();
    }

    private void ResetInternalLists()
    {
      BuildSinkNodeList();
      subNetwork = new HashSet<ET>();
      dirty = false;
    }

    private void BuildSinkNodeList()
    {
      sinkNodeList = new Dictionary<NT, List<ET>>();
      foreach (ET edge in this)
      {
        if (!sinkNodeList.ContainsKey(edge.SinkNode))
          sinkNodeList.Add(edge.SinkNode, new List<ET>());

        sinkNodeList[edge.SinkNode].Add(edge);
      }
    }

    #region KeyedCollection<T> overrides

    protected override void ClearItems()
    {
      dirty = true;
      base.ClearItems();
    }

    protected override void InsertItem(int index, ET item)
    {
      dirty = true;
      base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
      dirty = true;
      base.RemoveItem(index);
    }

    protected override void SetItem(int index, ET item)
    {
      dirty = true;
      base.SetItem(index, item);
    }

    protected override int GetKeyForItem(ET item)
    {
      return item.EdgeId;
    }

    #endregion


    /// <summary>
    /// Performs a network traversal (Trace) of this collection using the specified start and stop edges.
    /// </summary>
    /// <param name="startEdges">The start edges.</param>
    /// <param name="stopEdges">The stop edges.</param>
    /// <returns>A collection of the traced edges</returns>
    internal ICollection<ET> Trace(ICollection<ET> startEdges, ICollection<ET> stopEdges)
    {
      if (dirty && !recursing)
        ResetInternalLists();

      else if (dirty && recursing)
        throw new TraceDataChangedException();

      List<int> stopEdgeKeys = new List<int>();

      if (stopEdges != null)
      {
        foreach (ET stopEdge in stopEdges)
          stopEdgeKeys.Add(stopEdge.EdgeId);
      }

      foreach (ET startEdge in startEdges)
      {
        if (subNetwork.Contains(startEdge))
          continue;

        subNetwork.Add(startEdge);

        if (stopEdges != null && stopEdgeKeys.Contains(startEdge.EdgeId))
          continue;

        IList<ET> upstreamEdges = this.GetUpstreamEdges(startEdge);
        if (upstreamEdges.Count != 0)
        {
          recursing = true;
          this.Trace(upstreamEdges, stopEdges);
        }
      }
      recursing = false;
      return subNetwork;
    }

    /// <summary>
    /// Returns a list of edges which are connected to the upstream node of this edge
    /// </summary>
    /// <typeparam name="T">An object of type IGraphEdge</typeparam>
    /// <param name="network">A collection of IGraphEdge objects from which to search for upstream objects</param>
    /// <param name="edge">The IGraphEdge object whose upstream IGraphEdge objects will be returned</param>
    /// <returns>A collection of IGraphEdge objects upstream of the provided edge</returns>
    private IList<ET> GetUpstreamEdges(ET edge)
    {      
      IList<ET> upstreamEdges = new List<ET>();

      if (sinkNodeList.ContainsKey(edge.SourceNode))
        upstreamEdges = sinkNodeList[edge.SourceNode];

      return upstreamEdges;
    }

  }

  /// <summary>
  /// Indicates an exception thrown during the Trace
  /// </summary>
  public class TraceDataChangedException : Exception
  {
    public TraceDataChangedException()
      : base()
    {
    }
  }
}
