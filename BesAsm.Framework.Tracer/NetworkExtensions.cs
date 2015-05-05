using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BesAsm.Framework.Tracer;

namespace BesAsm.Framework.Tracer
{
  /// <summary>
  /// Extension method for performing trace functions on an IList<IGraphEdge>
  /// </summary>
  public static class NetworkExtensions
  {
    
    /// <summary>
    /// Performs an upstream network traversal of an IList<IGraphEdge> given a collection of start and stop edges
    /// </summary>
    /// <typeparam name="T">An object that implements IGraphEdge</typeparam>
    /// <param name="network">A list of IGraphEdge objects to be traversed</param>
    /// <param name="startEdges">A list of starting edges from which to trace upstream</param>
    /// <param name="stopEdges">A list of edges at which to terminate the trace</param>
    /// <returns>A collection of IGraphEdge objects which were traced</returns>    
    public static IList<ET> Trace<ET,NT>(this IList<ET> network, IList<ET> startEdges, IList<ET> stopEdges) where ET : IGraphEdge<NT>
    {     
      Network<ET,NT> fastNetwork = new Network<ET,NT>(network);
      return fastNetwork.Trace(startEdges, stopEdges).ToList();;      
    }

    /// <summary>
    /// Returns an array of IGraphEdge.EdgeIDs
    /// </summary>
    /// <typeparam name="ET"></typeparam>
    /// <param name="edges"></param>
    /// <returns></returns>
    public static int[] GetIdList<ET,NT>(this ICollection<ET> edges) where ET: IGraphEdge<NT>
    {
      int[] ids = new int[edges.Count];
      int i = 0;
      foreach (ET edge in edges)
      {
        ids[i] = edge.EdgeId;
        i++;
      }
      return ids;
    }    
  }
}
