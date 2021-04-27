using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes an abstract converter between a LHS type and a RHS type
    /// </summary>
    /// <typeparam name="TLeft">The LHS type</typeparam>
    /// <typeparam name="TRight">The RHS type</typeparam>
    public interface IConverter<TLeft, TRight>
    {
        /// <summary>
        /// Converts the provided LHS value to a RHS value
        /// </summary>
        /// <param name="left">The LHS value</param>
        /// <param name="currentRight">The current RHS value</param>
        /// <returns>The new RHS value</returns>
        TRight ConvertLeftToRight(TLeft left, TRight currentRight);

        /// <summary>
        /// Converts the provided RHS value to a LHS value
        /// </summary>
        /// <param name="right">The RHS value</param>
        /// <param name="currentLeft">The current LHS value</param>
        /// <returns>The new LHS value</returns>
        TLeft ConvertRightToLeft(TRight right, TLeft currentLeft);
    }
}
