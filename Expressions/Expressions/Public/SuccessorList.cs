using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    public class SuccessorList : ShortList<INotifiable>
    {
        public override void Insert(int index, INotifiable item)
        {
            //TODO Ugly hack: Since we cannot always remove the dummy null successor,
            //we automatically delete it here when a proper successor is added.
            if (index == 1 && item != null && this[0] == null)
            {
                RemoveAt(0);
                base.Insert(0, item);
            }
            base.Insert(index, item);
        }
    }
}
