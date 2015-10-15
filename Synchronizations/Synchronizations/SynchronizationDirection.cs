using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public enum SynchronizationDirection
    {
        LeftToRight,
        RightToLeft,
        LeftToRightForced,
        RightToLeftForced,
        LeftWins,
        RightWins
    }
}
