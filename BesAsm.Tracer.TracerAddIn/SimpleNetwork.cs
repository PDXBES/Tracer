using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace BesAsm.Tracer.TracerAddIn
{
  public class SimpleNetwork : KeyedCollection<int, SimpleGraphEdge>
  {
    public SimpleNetwork(ICollection<SimpleGraphEdge> edges)
      : base(null, 5)
    {
      foreach (SimpleGraphEdge edge in edges)
        base.Add(edge);      
    }
    
    protected override int GetKeyForItem(SimpleGraphEdge item)
    {
      return item.EdgeId;
    }
  }
}
