using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public interface IConverter<TLeft, TRight>
    {
        TRight ConvertLeftToRight(TLeft left, TRight currentRight);

        TLeft ConvertRightToLeft(TRight right, TLeft currentLeft);
    }
}
