using System;

namespace BesAsm.Framework.Tracer
{
  /// <summary>
  /// Describes the minimum functionality of a GraphEdge used by the <see cref="Tracer">Tracer</see>
  /// </summary>
  public interface IGraphEdge<T>
  {

    /// <summary>
    /// Gets the edge id.
    /// </summary>
    int EdgeId
    {
      get;
    }

    /// <summary>
    /// Gets the sink (downstream) node.
    /// </summary>
    T SinkNode
    {
      get;
    }

    /// <summary>
    /// Gets the source (upstream) node.
    /// </summary>
    T SourceNode
    {
      get;
    }

  }

}
