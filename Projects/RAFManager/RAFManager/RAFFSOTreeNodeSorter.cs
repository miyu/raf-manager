using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace RAFManager
{
    public class RAFFSOTreeNodeSorter:IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return ((RAFInMemoryFileSystemObject)x).Text.CompareTo(((RAFInMemoryFileSystemObject)y).Text);
        }
    }
}
