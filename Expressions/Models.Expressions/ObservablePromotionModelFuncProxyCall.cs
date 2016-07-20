using System;
using System.Collections.Generic;
using System.ComponentModel;
using NMF.Models;

namespace NMF.Expressions
{
    internal class ObservablePromotionModelFunc1Bases1Parameters<TBase1, TPar1, TResult> : ObservableModelFuncProxyCall1Bases1Parameters<TBase1, TPar1, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases1Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TPar1, TResult> func)
            : base(base1, selector1)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TResult>(func, Parameter1, par1Properties, par1Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases1Parameters<TBase1, TPar1, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases2Parameters<TBase1, TPar1, TPar2, TResult> : ObservableModelFuncProxyCall1Bases2Parameters<TBase1, TPar1, TPar2, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases2Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TPar1, TPar2, TResult> func)
            : base(base1, selector1, selector2)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases2Parameters<TBase1, TPar1, TPar2, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases3Parameters<TBase1, TPar1, TPar2, TPar3, TResult> : ObservableModelFuncProxyCall1Bases3Parameters<TBase1, TPar1, TPar2, TPar3, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases3Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TPar1, TPar2, TPar3, TResult> func)
            : base(base1, selector1, selector2, selector3)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases3Parameters<TBase1, TPar1, TPar2, TPar3, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases4Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TResult> : ObservableModelFuncProxyCall1Bases4Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases4Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TPar1, TPar2, TPar3, TPar4, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases4Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases5Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TResult> : ObservableModelFuncProxyCall1Bases5Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases5Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases5Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases6Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> : ObservableModelFuncProxyCall1Bases6Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases6Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases6Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases7Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> : ObservableModelFuncProxyCall1Bases7Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases7Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases7Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases8Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> : ObservableModelFuncProxyCall1Bases8Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases8Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases8Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases9Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> : ObservableModelFuncProxyCall1Bases9Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases9Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases9Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases10Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> : ObservableModelFuncProxyCall1Bases10Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases10Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases10Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases11Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> : ObservableModelFuncProxyCall1Bases11Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases11Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases11Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases12Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> : ObservableModelFuncProxyCall1Bases12Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases12Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases12Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases13Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> : ObservableModelFuncProxyCall1Bases13Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases13Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases13Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases14Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> : ObservableModelFuncProxyCall1Bases14Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases14Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases14Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc1Bases15Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> : ObservableModelFuncProxyCall1Bases15Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc1Bases15Parameters(INotifyExpression<TBase1> base1, Func<TBase1, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TBase1, TPar15> selector15, ICollection<string> par15Properties, bool par15Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> func)
            : base(base1, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14, selector15)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments, Parameter15, par15Properties, par15Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc1Bases15Parameters<TBase1, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(Parameter1.Base1, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                Parameter15.MemberGetter, PromotionFunc.Arg15Properties, PromotionFunc.Arg15Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases1Parameters<TBase1, TBase2, TPar1, TResult> : ObservableModelFuncProxyCall2Bases1Parameters<TBase1, TBase2, TPar1, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases1Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TPar1, TResult> func)
            : base(base1, base2, selector1)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TResult>(func, Parameter1, par1Properties, par1Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases1Parameters<TBase1, TBase2, TPar1, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases2Parameters<TBase1, TBase2, TPar1, TPar2, TResult> : ObservableModelFuncProxyCall2Bases2Parameters<TBase1, TBase2, TPar1, TPar2, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases2Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TPar1, TPar2, TResult> func)
            : base(base1, base2, selector1, selector2)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases2Parameters<TBase1, TBase2, TPar1, TPar2, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases3Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TResult> : ObservableModelFuncProxyCall2Bases3Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases3Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TPar1, TPar2, TPar3, TResult> func)
            : base(base1, base2, selector1, selector2, selector3)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases3Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases4Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TResult> : ObservableModelFuncProxyCall2Bases4Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases4Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TPar1, TPar2, TPar3, TPar4, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases4Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases5Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TResult> : ObservableModelFuncProxyCall2Bases5Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases5Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases5Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases6Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> : ObservableModelFuncProxyCall2Bases6Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases6Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases6Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases7Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> : ObservableModelFuncProxyCall2Bases7Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases7Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases7Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases8Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> : ObservableModelFuncProxyCall2Bases8Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases8Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases8Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases9Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> : ObservableModelFuncProxyCall2Bases9Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases9Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases9Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases10Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> : ObservableModelFuncProxyCall2Bases10Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases10Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases10Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases11Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> : ObservableModelFuncProxyCall2Bases11Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases11Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases11Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases12Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> : ObservableModelFuncProxyCall2Bases12Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases12Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases12Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases13Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> : ObservableModelFuncProxyCall2Bases13Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases13Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases13Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases14Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> : ObservableModelFuncProxyCall2Bases14Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases14Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases14Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc2Bases15Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> : ObservableModelFuncProxyCall2Bases15Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc2Bases15Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TBase1, TBase2, TPar15> selector15, ICollection<string> par15Properties, bool par15Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> func)
            : base(base1, base2, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14, selector15)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments, Parameter15, par15Properties, par15Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc2Bases15Parameters<TBase1, TBase2, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(Parameter1.Base1, Parameter1.Base2, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                Parameter15.MemberGetter, PromotionFunc.Arg15Properties, PromotionFunc.Arg15Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases1Parameters<TBase1, TBase2, TBase3, TPar1, TResult> : ObservableModelFuncProxyCall3Bases1Parameters<TBase1, TBase2, TBase3, TPar1, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases1Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TPar1, TResult> func)
            : base(base1, base2, base3, selector1)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TResult>(func, Parameter1, par1Properties, par1Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases1Parameters<TBase1, TBase2, TBase3, TPar1, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases2Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TResult> : ObservableModelFuncProxyCall3Bases2Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases2Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TPar1, TPar2, TResult> func)
            : base(base1, base2, base3, selector1, selector2)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases2Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases3Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TResult> : ObservableModelFuncProxyCall3Bases3Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases3Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TPar1, TPar2, TPar3, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases3Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases4Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TResult> : ObservableModelFuncProxyCall3Bases4Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases4Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TPar1, TPar2, TPar3, TPar4, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases4Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases5Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TResult> : ObservableModelFuncProxyCall3Bases5Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases5Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases5Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases6Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> : ObservableModelFuncProxyCall3Bases6Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases6Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases6Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases7Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> : ObservableModelFuncProxyCall3Bases7Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases7Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases7Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases8Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> : ObservableModelFuncProxyCall3Bases8Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases8Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases8Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases9Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> : ObservableModelFuncProxyCall3Bases9Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases9Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases9Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases10Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> : ObservableModelFuncProxyCall3Bases10Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases10Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases10Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases11Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> : ObservableModelFuncProxyCall3Bases11Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases11Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases11Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases12Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> : ObservableModelFuncProxyCall3Bases12Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases12Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases12Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases13Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> : ObservableModelFuncProxyCall3Bases13Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases13Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases13Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases14Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> : ObservableModelFuncProxyCall3Bases14Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases14Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases14Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc3Bases15Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> : ObservableModelFuncProxyCall3Bases15Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc3Bases15Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TBase1, TBase2, TBase3, TPar15> selector15, ICollection<string> par15Properties, bool par15Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> func)
            : base(base1, base2, base3, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14, selector15)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments, Parameter15, par15Properties, par15Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc3Bases15Parameters<TBase1, TBase2, TBase3, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                Parameter15.MemberGetter, PromotionFunc.Arg15Properties, PromotionFunc.Arg15Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TResult> : ObservableModelFuncProxyCall4Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases1Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TPar1, TResult> func)
            : base(base1, base2, base3, base4, selector1)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TResult>(func, Parameter1, par1Properties, par1Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TResult> : ObservableModelFuncProxyCall4Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases2Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TPar1, TPar2, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TResult> : ObservableModelFuncProxyCall4Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases3Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TPar1, TPar2, TPar3, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TResult> : ObservableModelFuncProxyCall4Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases4Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TPar1, TPar2, TPar3, TPar4, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TResult> : ObservableModelFuncProxyCall4Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases5Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> : ObservableModelFuncProxyCall4Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases6Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> : ObservableModelFuncProxyCall4Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases7Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> : ObservableModelFuncProxyCall4Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases8Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> : ObservableModelFuncProxyCall4Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases9Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> : ObservableModelFuncProxyCall4Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases10Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> : ObservableModelFuncProxyCall4Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases11Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> : ObservableModelFuncProxyCall4Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases12Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> : ObservableModelFuncProxyCall4Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases13Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> : ObservableModelFuncProxyCall4Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases14Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc4Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> : ObservableModelFuncProxyCall4Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc4Bases15Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TBase1, TBase2, TBase3, TBase4, TPar15> selector15, ICollection<string> par15Properties, bool par15Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> func)
            : base(base1, base2, base3, base4, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14, selector15)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments, Parameter15, par15Properties, par15Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc4Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                Parameter15.MemberGetter, PromotionFunc.Arg15Properties, PromotionFunc.Arg15Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TResult> : ObservableModelFuncProxyCall5Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases1Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TPar1, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TResult>(func, Parameter1, par1Properties, par1Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TResult> : ObservableModelFuncProxyCall5Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases2Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TPar1, TPar2, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TResult> : ObservableModelFuncProxyCall5Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases3Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TPar1, TPar2, TPar3, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TResult> : ObservableModelFuncProxyCall5Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases4Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TPar1, TPar2, TPar3, TPar4, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TResult> : ObservableModelFuncProxyCall5Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases5Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> : ObservableModelFuncProxyCall5Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases6Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> : ObservableModelFuncProxyCall5Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases7Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> : ObservableModelFuncProxyCall5Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases8Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> : ObservableModelFuncProxyCall5Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases9Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> : ObservableModelFuncProxyCall5Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases10Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> : ObservableModelFuncProxyCall5Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases11Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> : ObservableModelFuncProxyCall5Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases12Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> : ObservableModelFuncProxyCall5Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases13Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> : ObservableModelFuncProxyCall5Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases14Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc5Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> : ObservableModelFuncProxyCall5Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc5Bases15Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar15> selector15, ICollection<string> par15Properties, bool par15Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> func)
            : base(base1, base2, base3, base4, base5, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14, selector15)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments, Parameter15, par15Properties, par15Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc5Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                Parameter15.MemberGetter, PromotionFunc.Arg15Properties, PromotionFunc.Arg15Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TResult> : ObservableModelFuncProxyCall6Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases1Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TPar1, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TResult>(func, Parameter1, par1Properties, par1Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TResult> : ObservableModelFuncProxyCall6Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases2Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TPar1, TPar2, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TResult> : ObservableModelFuncProxyCall6Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases3Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TPar1, TPar2, TPar3, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TResult> : ObservableModelFuncProxyCall6Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases4Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TPar1, TPar2, TPar3, TPar4, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TResult> : ObservableModelFuncProxyCall6Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases5Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> : ObservableModelFuncProxyCall6Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases6Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> : ObservableModelFuncProxyCall6Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases7Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> : ObservableModelFuncProxyCall6Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases8Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> : ObservableModelFuncProxyCall6Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases9Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> : ObservableModelFuncProxyCall6Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases10Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> : ObservableModelFuncProxyCall6Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases11Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> : ObservableModelFuncProxyCall6Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases12Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> : ObservableModelFuncProxyCall6Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases13Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> : ObservableModelFuncProxyCall6Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases14Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc6Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> : ObservableModelFuncProxyCall6Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc6Bases15Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar15> selector15, ICollection<string> par15Properties, bool par15Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14, selector15)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments, Parameter15, par15Properties, par15Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc6Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                Parameter15.MemberGetter, PromotionFunc.Arg15Properties, PromotionFunc.Arg15Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TResult> : ObservableModelFuncProxyCall7Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases1Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TPar1, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TResult>(func, Parameter1, par1Properties, par1Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TResult> : ObservableModelFuncProxyCall7Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases2Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TPar1, TPar2, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TResult> : ObservableModelFuncProxyCall7Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases3Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TPar1, TPar2, TPar3, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TResult> : ObservableModelFuncProxyCall7Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases4Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TPar1, TPar2, TPar3, TPar4, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TResult> : ObservableModelFuncProxyCall7Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases5Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> : ObservableModelFuncProxyCall7Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases6Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> : ObservableModelFuncProxyCall7Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases7Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> : ObservableModelFuncProxyCall7Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases8Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> : ObservableModelFuncProxyCall7Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases9Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> : ObservableModelFuncProxyCall7Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases10Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> : ObservableModelFuncProxyCall7Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases11Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> : ObservableModelFuncProxyCall7Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases12Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> : ObservableModelFuncProxyCall7Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases13Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> : ObservableModelFuncProxyCall7Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases14Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                PromotionFunc.Function);
        }
    }
    internal class ObservablePromotionModelFunc7Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> : ObservableModelFuncProxyCall7Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>
    {
        public ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> PromotionFunc { get; private set; }

        public ObservablePromotionModelFunc7Bases15Parameters(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, ICollection<string> par1Properties, bool par1Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, ICollection<string> par2Properties, bool par2Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, ICollection<string> par3Properties, bool par3Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, ICollection<string> par4Properties, bool par4Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, ICollection<string> par5Properties, bool par5Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, ICollection<string> par6Properties, bool par6Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, ICollection<string> par7Properties, bool par7Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, ICollection<string> par8Properties, bool par8Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, ICollection<string> par9Properties, bool par9Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, ICollection<string> par10Properties, bool par10Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, ICollection<string> par11Properties, bool par11Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> selector12, ICollection<string> par12Properties, bool par12Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> selector13, ICollection<string> par13Properties, bool par13Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar14> selector14, ICollection<string> par14Properties, bool par14Containments, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar15> selector15, ICollection<string> par15Properties, bool par15Containments, Func<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult> func)
            : base(base1, base2, base3, base4, base5, base6, base7, selector1, selector2, selector3, selector4, selector5, selector6, selector7, selector8, selector9, selector10, selector11, selector12, selector13, selector14, selector15)
        {
            PromotionFunc = new ObservablePromotionMethodCall<TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(func, Parameter1, par1Properties, par1Containments, Parameter2, par2Properties, par2Containments, Parameter3, par3Properties, par3Containments, Parameter4, par4Properties, par4Containments, Parameter5, par5Properties, par5Containments, Parameter6, par6Properties, par6Containments, Parameter7, par7Properties, par7Containments, Parameter8, par8Properties, par8Containments, Parameter9, par9Properties, par9Containments, Parameter10, par10Properties, par10Containments, Parameter11, par11Properties, par11Containments, Parameter12, par12Properties, par12Containments, Parameter13, par13Properties, par13Containments, Parameter14, par14Properties, par14Containments, Parameter15, par15Properties, par15Containments);
        }

        public override INotifyExpression<TResult> Func
        {
            get
            {
                return PromotionFunc;
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionModelFunc7Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1, TPar2, TPar3, TPar4, TPar5, TPar6, TPar7, TPar8, TPar9, TPar10, TPar11, TPar12, TPar13, TPar14, TPar15, TResult>(Parameter1.Base1, Parameter1.Base2, Parameter1.Base3, Parameter1.Base4, Parameter1.Base5, Parameter1.Base6, Parameter1.Base7, 
                Parameter1.MemberGetter, PromotionFunc.Arg1Properties, PromotionFunc.Arg1Composition,
                Parameter2.MemberGetter, PromotionFunc.Arg2Properties, PromotionFunc.Arg2Composition,
                Parameter3.MemberGetter, PromotionFunc.Arg3Properties, PromotionFunc.Arg3Composition,
                Parameter4.MemberGetter, PromotionFunc.Arg4Properties, PromotionFunc.Arg4Composition,
                Parameter5.MemberGetter, PromotionFunc.Arg5Properties, PromotionFunc.Arg5Composition,
                Parameter6.MemberGetter, PromotionFunc.Arg6Properties, PromotionFunc.Arg6Composition,
                Parameter7.MemberGetter, PromotionFunc.Arg7Properties, PromotionFunc.Arg7Composition,
                Parameter8.MemberGetter, PromotionFunc.Arg8Properties, PromotionFunc.Arg8Composition,
                Parameter9.MemberGetter, PromotionFunc.Arg9Properties, PromotionFunc.Arg9Composition,
                Parameter10.MemberGetter, PromotionFunc.Arg10Properties, PromotionFunc.Arg10Composition,
                Parameter11.MemberGetter, PromotionFunc.Arg11Properties, PromotionFunc.Arg11Composition,
                Parameter12.MemberGetter, PromotionFunc.Arg12Properties, PromotionFunc.Arg12Composition,
                Parameter13.MemberGetter, PromotionFunc.Arg13Properties, PromotionFunc.Arg13Composition,
                Parameter14.MemberGetter, PromotionFunc.Arg14Properties, PromotionFunc.Arg14Composition,
                Parameter15.MemberGetter, PromotionFunc.Arg15Properties, PromotionFunc.Arg15Composition,
                PromotionFunc.Function);
        }
    }

	internal static class ObservablePromotionModelFuncTypes
	{
		public static readonly Type[,] Types = {
			{ typeof(ObservablePromotionModelFunc1Bases1Parameters<,,>), typeof(ObservablePromotionModelFunc1Bases2Parameters<,,,>), typeof(ObservablePromotionModelFunc1Bases3Parameters<,,,,>), typeof(ObservablePromotionModelFunc1Bases4Parameters<,,,,,>), typeof(ObservablePromotionModelFunc1Bases5Parameters<,,,,,,>), typeof(ObservablePromotionModelFunc1Bases6Parameters<,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases7Parameters<,,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases8Parameters<,,,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases9Parameters<,,,,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases10Parameters<,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases11Parameters<,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases12Parameters<,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases13Parameters<,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases14Parameters<,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc1Bases15Parameters<,,,,,,,,,,,,,,,,>) },
			{ typeof(ObservablePromotionModelFunc2Bases1Parameters<,,,>), typeof(ObservablePromotionModelFunc2Bases2Parameters<,,,,>), typeof(ObservablePromotionModelFunc2Bases3Parameters<,,,,,>), typeof(ObservablePromotionModelFunc2Bases4Parameters<,,,,,,>), typeof(ObservablePromotionModelFunc2Bases5Parameters<,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases6Parameters<,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases7Parameters<,,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases8Parameters<,,,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases9Parameters<,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases10Parameters<,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases11Parameters<,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases12Parameters<,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases13Parameters<,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases14Parameters<,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc2Bases15Parameters<,,,,,,,,,,,,,,,,,>) },
			{ typeof(ObservablePromotionModelFunc3Bases1Parameters<,,,,>), typeof(ObservablePromotionModelFunc3Bases2Parameters<,,,,,>), typeof(ObservablePromotionModelFunc3Bases3Parameters<,,,,,,>), typeof(ObservablePromotionModelFunc3Bases4Parameters<,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases5Parameters<,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases6Parameters<,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases7Parameters<,,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases8Parameters<,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases9Parameters<,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases10Parameters<,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases11Parameters<,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases12Parameters<,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases13Parameters<,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases14Parameters<,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc3Bases15Parameters<,,,,,,,,,,,,,,,,,,>) },
			{ typeof(ObservablePromotionModelFunc4Bases1Parameters<,,,,,>), typeof(ObservablePromotionModelFunc4Bases2Parameters<,,,,,,>), typeof(ObservablePromotionModelFunc4Bases3Parameters<,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases4Parameters<,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases5Parameters<,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases6Parameters<,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases7Parameters<,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases8Parameters<,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases9Parameters<,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases10Parameters<,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases11Parameters<,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases12Parameters<,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases13Parameters<,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases14Parameters<,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc4Bases15Parameters<,,,,,,,,,,,,,,,,,,,>) },
			{ typeof(ObservablePromotionModelFunc5Bases1Parameters<,,,,,,>), typeof(ObservablePromotionModelFunc5Bases2Parameters<,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases3Parameters<,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases4Parameters<,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases5Parameters<,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases6Parameters<,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases7Parameters<,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases8Parameters<,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases9Parameters<,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases10Parameters<,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases11Parameters<,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases12Parameters<,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases13Parameters<,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases14Parameters<,,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc5Bases15Parameters<,,,,,,,,,,,,,,,,,,,,>) },
			{ typeof(ObservablePromotionModelFunc6Bases1Parameters<,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases2Parameters<,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases3Parameters<,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases4Parameters<,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases5Parameters<,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases6Parameters<,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases7Parameters<,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases8Parameters<,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases9Parameters<,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases10Parameters<,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases11Parameters<,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases12Parameters<,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases13Parameters<,,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases14Parameters<,,,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc6Bases15Parameters<,,,,,,,,,,,,,,,,,,,,,>) },
			{ typeof(ObservablePromotionModelFunc7Bases1Parameters<,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases2Parameters<,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases3Parameters<,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases4Parameters<,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases5Parameters<,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases6Parameters<,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases7Parameters<,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases8Parameters<,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases9Parameters<,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases10Parameters<,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases11Parameters<,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases12Parameters<,,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases13Parameters<,,,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases14Parameters<,,,,,,,,,,,,,,,,,,,,,>), typeof(ObservablePromotionModelFunc7Bases15Parameters<,,,,,,,,,,,,,,,,,,,,,,>) }
		};
	}
}
