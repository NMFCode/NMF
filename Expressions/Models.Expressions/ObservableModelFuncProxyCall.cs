using System;
using System.Collections.Generic;
using System.ComponentModel;
using NMF.Models;

namespace NMF.Expressions
{
    internal abstract class ObservableModelFuncProxyCall1Bases1Parameters<TBase1,  TPar1, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases1Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases2Parameters<TBase1,  TPar1,  TPar2, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases2Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases3Parameters<TBase1,  TPar1,  TPar2,  TPar3, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases3Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases4Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases4Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases5Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases5Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases6Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases6Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases7Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases7Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases8Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar8> Parameter8 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases8Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7, Func<TBase1, TPar8> selector8)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TPar8>(argument1, selector8);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases9Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar9> Parameter9 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases9Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7, Func<TBase1, TPar8> selector8, Func<TBase1, TPar9> selector9)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TPar8>(argument1, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TPar9>(argument1, selector9);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases10Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar10> Parameter10 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases10Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7, Func<TBase1, TPar8> selector8, Func<TBase1, TPar9> selector9, Func<TBase1, TPar10> selector10)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TPar8>(argument1, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TPar9>(argument1, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TPar10>(argument1, selector10);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases11Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar11> Parameter11 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases11Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7, Func<TBase1, TPar8> selector8, Func<TBase1, TPar9> selector9, Func<TBase1, TPar10> selector10, Func<TBase1, TPar11> selector11)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TPar8>(argument1, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TPar9>(argument1, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TPar10>(argument1, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TPar11>(argument1, selector11);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases12Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar12> Parameter12 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases12Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7, Func<TBase1, TPar8> selector8, Func<TBase1, TPar9> selector9, Func<TBase1, TPar10> selector10, Func<TBase1, TPar11> selector11, Func<TBase1, TPar12> selector12)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TPar8>(argument1, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TPar9>(argument1, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TPar10>(argument1, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TPar11>(argument1, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TPar12>(argument1, selector12);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases13Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar13> Parameter13 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases13Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7, Func<TBase1, TPar8> selector8, Func<TBase1, TPar9> selector9, Func<TBase1, TPar10> selector10, Func<TBase1, TPar11> selector11, Func<TBase1, TPar12> selector12, Func<TBase1, TPar13> selector13)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TPar8>(argument1, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TPar9>(argument1, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TPar10>(argument1, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TPar11>(argument1, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TPar12>(argument1, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TPar13>(argument1, selector13);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases14Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar14> Parameter14 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases14Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7, Func<TBase1, TPar8> selector8, Func<TBase1, TPar9> selector9, Func<TBase1, TPar10> selector10, Func<TBase1, TPar11> selector11, Func<TBase1, TPar12> selector12, Func<TBase1, TPar13> selector13, Func<TBase1, TPar14> selector14)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TPar8>(argument1, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TPar9>(argument1, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TPar10>(argument1, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TPar11>(argument1, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TPar12>(argument1, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TPar13>(argument1, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TPar14>(argument1, selector14);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall1Bases15Parameters<TBase1,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14,  TPar15, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar14> Parameter14 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TPar15> Parameter15 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall1Bases15Parameters(INotifyExpression<TBase1> argument1, Func<TBase1, TPar1> selector1, Func<TBase1, TPar2> selector2, Func<TBase1, TPar3> selector3, Func<TBase1, TPar4> selector4, Func<TBase1, TPar5> selector5, Func<TBase1, TPar6> selector6, Func<TBase1, TPar7> selector7, Func<TBase1, TPar8> selector8, Func<TBase1, TPar9> selector9, Func<TBase1, TPar10> selector10, Func<TBase1, TPar11> selector11, Func<TBase1, TPar12> selector12, Func<TBase1, TPar13> selector13, Func<TBase1, TPar14> selector14, Func<TBase1, TPar15> selector15)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TPar1>(argument1, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TPar2>(argument1, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TPar3>(argument1, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TPar4>(argument1, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TPar5>(argument1, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TPar6>(argument1, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TPar7>(argument1, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TPar8>(argument1, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TPar9>(argument1, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TPar10>(argument1, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TPar11>(argument1, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TPar12>(argument1, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TPar13>(argument1, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TPar14>(argument1, selector14);
            Parameter15 = new ModelFuncExtractionParameter<TBase1, TPar15>(argument1, selector15);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases1Parameters<TBase1, TBase2,  TPar1, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases1Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases2Parameters<TBase1, TBase2,  TPar1,  TPar2, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases2Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases3Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases3Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases4Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases4Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases5Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases5Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases6Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases6Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases7Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases7Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases8Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar8> Parameter8 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases8Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7, Func<TBase1, TBase2, TPar8> selector8)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar8>(argument1, argument2, selector8);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases9Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar9> Parameter9 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases9Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7, Func<TBase1, TBase2, TPar8> selector8, Func<TBase1, TBase2, TPar9> selector9)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar8>(argument1, argument2, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar9>(argument1, argument2, selector9);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases10Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar10> Parameter10 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases10Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7, Func<TBase1, TBase2, TPar8> selector8, Func<TBase1, TBase2, TPar9> selector9, Func<TBase1, TBase2, TPar10> selector10)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar8>(argument1, argument2, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar9>(argument1, argument2, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar10>(argument1, argument2, selector10);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases11Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar11> Parameter11 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases11Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7, Func<TBase1, TBase2, TPar8> selector8, Func<TBase1, TBase2, TPar9> selector9, Func<TBase1, TBase2, TPar10> selector10, Func<TBase1, TBase2, TPar11> selector11)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar8>(argument1, argument2, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar9>(argument1, argument2, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar10>(argument1, argument2, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar11>(argument1, argument2, selector11);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases12Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar12> Parameter12 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases12Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7, Func<TBase1, TBase2, TPar8> selector8, Func<TBase1, TBase2, TPar9> selector9, Func<TBase1, TBase2, TPar10> selector10, Func<TBase1, TBase2, TPar11> selector11, Func<TBase1, TBase2, TPar12> selector12)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar8>(argument1, argument2, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar9>(argument1, argument2, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar10>(argument1, argument2, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar11>(argument1, argument2, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar12>(argument1, argument2, selector12);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases13Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar13> Parameter13 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases13Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7, Func<TBase1, TBase2, TPar8> selector8, Func<TBase1, TBase2, TPar9> selector9, Func<TBase1, TBase2, TPar10> selector10, Func<TBase1, TBase2, TPar11> selector11, Func<TBase1, TBase2, TPar12> selector12, Func<TBase1, TBase2, TPar13> selector13)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar8>(argument1, argument2, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar9>(argument1, argument2, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar10>(argument1, argument2, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar11>(argument1, argument2, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar12>(argument1, argument2, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar13>(argument1, argument2, selector13);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases14Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar14> Parameter14 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases14Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7, Func<TBase1, TBase2, TPar8> selector8, Func<TBase1, TBase2, TPar9> selector9, Func<TBase1, TBase2, TPar10> selector10, Func<TBase1, TBase2, TPar11> selector11, Func<TBase1, TBase2, TPar12> selector12, Func<TBase1, TBase2, TPar13> selector13, Func<TBase1, TBase2, TPar14> selector14)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar8>(argument1, argument2, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar9>(argument1, argument2, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar10>(argument1, argument2, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar11>(argument1, argument2, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar12>(argument1, argument2, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar13>(argument1, argument2, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar14>(argument1, argument2, selector14);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall2Bases15Parameters<TBase1, TBase2,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14,  TPar15, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar14> Parameter14 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TPar15> Parameter15 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall2Bases15Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, Func<TBase1, TBase2, TPar1> selector1, Func<TBase1, TBase2, TPar2> selector2, Func<TBase1, TBase2, TPar3> selector3, Func<TBase1, TBase2, TPar4> selector4, Func<TBase1, TBase2, TPar5> selector5, Func<TBase1, TBase2, TPar6> selector6, Func<TBase1, TBase2, TPar7> selector7, Func<TBase1, TBase2, TPar8> selector8, Func<TBase1, TBase2, TPar9> selector9, Func<TBase1, TBase2, TPar10> selector10, Func<TBase1, TBase2, TPar11> selector11, Func<TBase1, TBase2, TPar12> selector12, Func<TBase1, TBase2, TPar13> selector13, Func<TBase1, TBase2, TPar14> selector14, Func<TBase1, TBase2, TPar15> selector15)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar1>(argument1, argument2, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar2>(argument1, argument2, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar3>(argument1, argument2, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar4>(argument1, argument2, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar5>(argument1, argument2, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar6>(argument1, argument2, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar7>(argument1, argument2, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar8>(argument1, argument2, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar9>(argument1, argument2, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar10>(argument1, argument2, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar11>(argument1, argument2, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar12>(argument1, argument2, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar13>(argument1, argument2, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar14>(argument1, argument2, selector14);
            Parameter15 = new ModelFuncExtractionParameter<TBase1, TBase2, TPar15>(argument1, argument2, selector15);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases1Parameters<TBase1, TBase2, TBase3,  TPar1, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases1Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases2Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases2Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases3Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases3Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases4Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases4Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases5Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases5Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases6Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases6Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases7Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases7Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases8Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8> Parameter8 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases8Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7, Func<TBase1, TBase2, TBase3, TPar8> selector8)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8>(argument1, argument2, argument3, selector8);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases9Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9> Parameter9 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases9Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7, Func<TBase1, TBase2, TBase3, TPar8> selector8, Func<TBase1, TBase2, TBase3, TPar9> selector9)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8>(argument1, argument2, argument3, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9>(argument1, argument2, argument3, selector9);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases10Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10> Parameter10 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases10Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7, Func<TBase1, TBase2, TBase3, TPar8> selector8, Func<TBase1, TBase2, TBase3, TPar9> selector9, Func<TBase1, TBase2, TBase3, TPar10> selector10)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8>(argument1, argument2, argument3, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9>(argument1, argument2, argument3, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10>(argument1, argument2, argument3, selector10);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases11Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11> Parameter11 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases11Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7, Func<TBase1, TBase2, TBase3, TPar8> selector8, Func<TBase1, TBase2, TBase3, TPar9> selector9, Func<TBase1, TBase2, TBase3, TPar10> selector10, Func<TBase1, TBase2, TBase3, TPar11> selector11)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8>(argument1, argument2, argument3, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9>(argument1, argument2, argument3, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10>(argument1, argument2, argument3, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11>(argument1, argument2, argument3, selector11);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases12Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar12> Parameter12 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases12Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7, Func<TBase1, TBase2, TBase3, TPar8> selector8, Func<TBase1, TBase2, TBase3, TPar9> selector9, Func<TBase1, TBase2, TBase3, TPar10> selector10, Func<TBase1, TBase2, TBase3, TPar11> selector11, Func<TBase1, TBase2, TBase3, TPar12> selector12)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8>(argument1, argument2, argument3, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9>(argument1, argument2, argument3, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10>(argument1, argument2, argument3, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11>(argument1, argument2, argument3, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar12>(argument1, argument2, argument3, selector12);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases13Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar13> Parameter13 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases13Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7, Func<TBase1, TBase2, TBase3, TPar8> selector8, Func<TBase1, TBase2, TBase3, TPar9> selector9, Func<TBase1, TBase2, TBase3, TPar10> selector10, Func<TBase1, TBase2, TBase3, TPar11> selector11, Func<TBase1, TBase2, TBase3, TPar12> selector12, Func<TBase1, TBase2, TBase3, TPar13> selector13)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8>(argument1, argument2, argument3, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9>(argument1, argument2, argument3, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10>(argument1, argument2, argument3, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11>(argument1, argument2, argument3, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar12>(argument1, argument2, argument3, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar13>(argument1, argument2, argument3, selector13);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases14Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar14> Parameter14 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases14Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7, Func<TBase1, TBase2, TBase3, TPar8> selector8, Func<TBase1, TBase2, TBase3, TPar9> selector9, Func<TBase1, TBase2, TBase3, TPar10> selector10, Func<TBase1, TBase2, TBase3, TPar11> selector11, Func<TBase1, TBase2, TBase3, TPar12> selector12, Func<TBase1, TBase2, TBase3, TPar13> selector13, Func<TBase1, TBase2, TBase3, TPar14> selector14)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8>(argument1, argument2, argument3, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9>(argument1, argument2, argument3, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10>(argument1, argument2, argument3, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11>(argument1, argument2, argument3, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar12>(argument1, argument2, argument3, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar13>(argument1, argument2, argument3, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar14>(argument1, argument2, argument3, selector14);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall3Bases15Parameters<TBase1, TBase2, TBase3,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14,  TPar15, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar14> Parameter14 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar15> Parameter15 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall3Bases15Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, Func<TBase1, TBase2, TBase3, TPar1> selector1, Func<TBase1, TBase2, TBase3, TPar2> selector2, Func<TBase1, TBase2, TBase3, TPar3> selector3, Func<TBase1, TBase2, TBase3, TPar4> selector4, Func<TBase1, TBase2, TBase3, TPar5> selector5, Func<TBase1, TBase2, TBase3, TPar6> selector6, Func<TBase1, TBase2, TBase3, TPar7> selector7, Func<TBase1, TBase2, TBase3, TPar8> selector8, Func<TBase1, TBase2, TBase3, TPar9> selector9, Func<TBase1, TBase2, TBase3, TPar10> selector10, Func<TBase1, TBase2, TBase3, TPar11> selector11, Func<TBase1, TBase2, TBase3, TPar12> selector12, Func<TBase1, TBase2, TBase3, TPar13> selector13, Func<TBase1, TBase2, TBase3, TPar14> selector14, Func<TBase1, TBase2, TBase3, TPar15> selector15)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar1>(argument1, argument2, argument3, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar2>(argument1, argument2, argument3, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar3>(argument1, argument2, argument3, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar4>(argument1, argument2, argument3, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar5>(argument1, argument2, argument3, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar6>(argument1, argument2, argument3, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar7>(argument1, argument2, argument3, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar8>(argument1, argument2, argument3, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar9>(argument1, argument2, argument3, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar10>(argument1, argument2, argument3, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar11>(argument1, argument2, argument3, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar12>(argument1, argument2, argument3, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar13>(argument1, argument2, argument3, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar14>(argument1, argument2, argument3, selector14);
            Parameter15 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TPar15>(argument1, argument2, argument3, selector15);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases1Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases1Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases2Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases2Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases3Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases3Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases4Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases4Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases5Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases5Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases6Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases6Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases7Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases7Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases8Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8> Parameter8 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases8Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8>(argument1, argument2, argument3, argument4, selector8);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases9Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9> Parameter9 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases9Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8>(argument1, argument2, argument3, argument4, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9>(argument1, argument2, argument3, argument4, selector9);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases10Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10> Parameter10 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases10Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8>(argument1, argument2, argument3, argument4, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9>(argument1, argument2, argument3, argument4, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10>(argument1, argument2, argument3, argument4, selector10);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases11Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11> Parameter11 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases11Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8>(argument1, argument2, argument3, argument4, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9>(argument1, argument2, argument3, argument4, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10>(argument1, argument2, argument3, argument4, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11>(argument1, argument2, argument3, argument4, selector11);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases12Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar12> Parameter12 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases12Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TPar12> selector12)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8>(argument1, argument2, argument3, argument4, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9>(argument1, argument2, argument3, argument4, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10>(argument1, argument2, argument3, argument4, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11>(argument1, argument2, argument3, argument4, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar12>(argument1, argument2, argument3, argument4, selector12);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases13Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar13> Parameter13 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases13Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TPar13> selector13)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8>(argument1, argument2, argument3, argument4, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9>(argument1, argument2, argument3, argument4, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10>(argument1, argument2, argument3, argument4, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11>(argument1, argument2, argument3, argument4, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar12>(argument1, argument2, argument3, argument4, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar13>(argument1, argument2, argument3, argument4, selector13);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases14Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar14> Parameter14 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases14Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TPar13> selector13, Func<TBase1, TBase2, TBase3, TBase4, TPar14> selector14)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8>(argument1, argument2, argument3, argument4, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9>(argument1, argument2, argument3, argument4, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10>(argument1, argument2, argument3, argument4, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11>(argument1, argument2, argument3, argument4, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar12>(argument1, argument2, argument3, argument4, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar13>(argument1, argument2, argument3, argument4, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar14>(argument1, argument2, argument3, argument4, selector14);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall4Bases15Parameters<TBase1, TBase2, TBase3, TBase4,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14,  TPar15, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar14> Parameter14 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar15> Parameter15 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall4Bases15Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, Func<TBase1, TBase2, TBase3, TBase4, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TPar13> selector13, Func<TBase1, TBase2, TBase3, TBase4, TPar14> selector14, Func<TBase1, TBase2, TBase3, TBase4, TPar15> selector15)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar1>(argument1, argument2, argument3, argument4, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar2>(argument1, argument2, argument3, argument4, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar3>(argument1, argument2, argument3, argument4, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar4>(argument1, argument2, argument3, argument4, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar5>(argument1, argument2, argument3, argument4, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar6>(argument1, argument2, argument3, argument4, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar7>(argument1, argument2, argument3, argument4, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar8>(argument1, argument2, argument3, argument4, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar9>(argument1, argument2, argument3, argument4, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar10>(argument1, argument2, argument3, argument4, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar11>(argument1, argument2, argument3, argument4, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar12>(argument1, argument2, argument3, argument4, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar13>(argument1, argument2, argument3, argument4, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar14>(argument1, argument2, argument3, argument4, selector14);
            Parameter15 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TPar15>(argument1, argument2, argument3, argument4, selector15);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases1Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases2Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases3Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases4Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases5Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases6Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases7Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> Parameter8 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases8Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8>(argument1, argument2, argument3, argument4, argument5, selector8);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> Parameter9 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases9Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8>(argument1, argument2, argument3, argument4, argument5, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9>(argument1, argument2, argument3, argument4, argument5, selector9);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> Parameter10 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases10Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8>(argument1, argument2, argument3, argument4, argument5, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9>(argument1, argument2, argument3, argument4, argument5, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10>(argument1, argument2, argument3, argument4, argument5, selector10);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> Parameter11 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases11Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8>(argument1, argument2, argument3, argument4, argument5, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9>(argument1, argument2, argument3, argument4, argument5, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10>(argument1, argument2, argument3, argument4, argument5, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11>(argument1, argument2, argument3, argument4, argument5, selector11);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> Parameter12 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases12Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> selector12)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8>(argument1, argument2, argument3, argument4, argument5, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9>(argument1, argument2, argument3, argument4, argument5, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10>(argument1, argument2, argument3, argument4, argument5, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11>(argument1, argument2, argument3, argument4, argument5, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12>(argument1, argument2, argument3, argument4, argument5, selector12);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> Parameter13 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases13Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> selector13)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8>(argument1, argument2, argument3, argument4, argument5, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9>(argument1, argument2, argument3, argument4, argument5, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10>(argument1, argument2, argument3, argument4, argument5, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11>(argument1, argument2, argument3, argument4, argument5, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12>(argument1, argument2, argument3, argument4, argument5, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13>(argument1, argument2, argument3, argument4, argument5, selector13);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar14> Parameter14 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases14Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> selector13, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar14> selector14)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8>(argument1, argument2, argument3, argument4, argument5, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9>(argument1, argument2, argument3, argument4, argument5, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10>(argument1, argument2, argument3, argument4, argument5, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11>(argument1, argument2, argument3, argument4, argument5, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12>(argument1, argument2, argument3, argument4, argument5, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13>(argument1, argument2, argument3, argument4, argument5, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar14>(argument1, argument2, argument3, argument4, argument5, selector14);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall5Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14,  TPar15, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar14> Parameter14 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar15> Parameter15 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall5Bases15Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13> selector13, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar14> selector14, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TPar15> selector15)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar1>(argument1, argument2, argument3, argument4, argument5, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar2>(argument1, argument2, argument3, argument4, argument5, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar3>(argument1, argument2, argument3, argument4, argument5, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar4>(argument1, argument2, argument3, argument4, argument5, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar5>(argument1, argument2, argument3, argument4, argument5, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar6>(argument1, argument2, argument3, argument4, argument5, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar7>(argument1, argument2, argument3, argument4, argument5, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar8>(argument1, argument2, argument3, argument4, argument5, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar9>(argument1, argument2, argument3, argument4, argument5, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar10>(argument1, argument2, argument3, argument4, argument5, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar11>(argument1, argument2, argument3, argument4, argument5, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar12>(argument1, argument2, argument3, argument4, argument5, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar13>(argument1, argument2, argument3, argument4, argument5, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar14>(argument1, argument2, argument3, argument4, argument5, selector14);
            Parameter15 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TPar15>(argument1, argument2, argument3, argument4, argument5, selector15);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases1Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases2Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases3Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases4Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases5Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases6Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases7Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> Parameter8 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases8Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, selector8);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> Parameter9 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases9Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, selector9);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> Parameter10 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases10Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, selector10);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> Parameter11 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases11Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, selector11);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> Parameter12 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases12Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> selector12)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12>(argument1, argument2, argument3, argument4, argument5, argument6, selector12);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> Parameter13 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases13Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> selector13)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12>(argument1, argument2, argument3, argument4, argument5, argument6, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13>(argument1, argument2, argument3, argument4, argument5, argument6, selector13);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar14> Parameter14 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases14Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> selector13, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar14> selector14)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12>(argument1, argument2, argument3, argument4, argument5, argument6, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13>(argument1, argument2, argument3, argument4, argument5, argument6, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar14>(argument1, argument2, argument3, argument4, argument5, argument6, selector14);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall6Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14,  TPar15, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar14> Parameter14 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar15> Parameter15 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall6Bases15Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13> selector13, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar14> selector14, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar15> selector15)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar12>(argument1, argument2, argument3, argument4, argument5, argument6, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar13>(argument1, argument2, argument3, argument4, argument5, argument6, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar14>(argument1, argument2, argument3, argument4, argument5, argument6, selector14);
            Parameter15 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TPar15>(argument1, argument2, argument3, argument4, argument5, argument6, selector15);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases1Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases1Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases2Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases2Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases3Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases3Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases4Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases4Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases5Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases5Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases6Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases6Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases7Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases7Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases8Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> Parameter8 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases8Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector8);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases9Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> Parameter9 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases9Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector9);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases10Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> Parameter10 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases10Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector10);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases11Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> Parameter11 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases11Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector11);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases12Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> Parameter12 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases12Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> selector12)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector12);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases13Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> Parameter13 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases13Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> selector13)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector13);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases14Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar14> Parameter14 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases14Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> selector13, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar14> selector14)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar14>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector14);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
    internal abstract class ObservableModelFuncProxyCall7Bases15Parameters<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7,  TPar1,  TPar2,  TPar3,  TPar4,  TPar5,  TPar6,  TPar7,  TPar8,  TPar9,  TPar10,  TPar11,  TPar12,  TPar13,  TPar14,  TPar15, TResult> : NotifyExpression<TResult>
    {
        public abstract INotifyExpression<TResult> Func { get; }

        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> Parameter1 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> Parameter2 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> Parameter3 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> Parameter4 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> Parameter5 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> Parameter6 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> Parameter7 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> Parameter8 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> Parameter9 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> Parameter10 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> Parameter11 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> Parameter12 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> Parameter13 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar14> Parameter14 { get; private set; }
        public ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar15> Parameter15 { get; private set; }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public ObservableModelFuncProxyCall7Bases15Parameters(INotifyExpression<TBase1> argument1, INotifyExpression<TBase2> argument2, INotifyExpression<TBase3> argument3, INotifyExpression<TBase4> argument4, INotifyExpression<TBase5> argument5, INotifyExpression<TBase6> argument6, INotifyExpression<TBase7> argument7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1> selector1, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2> selector2, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3> selector3, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4> selector4, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5> selector5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6> selector6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7> selector7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8> selector8, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9> selector9, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10> selector10, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11> selector11, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12> selector12, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13> selector13, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar14> selector14, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar15> selector15)
        {
            Parameter1 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar1>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector1);
            Parameter2 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar2>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector2);
            Parameter3 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar3>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector3);
            Parameter4 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar4>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector4);
            Parameter5 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar5>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector5);
            Parameter6 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar6>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector6);
            Parameter7 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar7>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector7);
            Parameter8 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar8>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector8);
            Parameter9 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar9>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector9);
            Parameter10 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar10>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector10);
            Parameter11 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar11>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector11);
            Parameter12 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar12>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector12);
            Parameter13 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar13>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector13);
            Parameter14 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar14>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector14);
            Parameter15 = new ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, TPar15>(argument1, argument2, argument3, argument4, argument5, argument6, argument7, selector15);
        }

        protected override void AttachCore()
        {
            Parameter1.Base1.Attach();
            Parameter1.Base1.ValueChanged += RefreshFunc;
            Parameter1.Base2.Attach();
            Parameter1.Base2.ValueChanged += RefreshFunc;
            Parameter1.Base3.Attach();
            Parameter1.Base3.ValueChanged += RefreshFunc;
            Parameter1.Base4.Attach();
            Parameter1.Base4.ValueChanged += RefreshFunc;
            Parameter1.Base5.Attach();
            Parameter1.Base5.ValueChanged += RefreshFunc;
            Parameter1.Base6.Attach();
            Parameter1.Base6.ValueChanged += RefreshFunc;
            Parameter1.Base7.Attach();
            Parameter1.Base7.ValueChanged += RefreshFunc;
        }

        private void RefreshFunc(object sender, ValueChangedEventArgs e)
        {
            Func.Refresh();
            Refresh();
        }

        protected override void DetachCore()
        {
            Parameter1.Base1.Detach();
            Parameter1.Base1.ValueChanged -= RefreshFunc;
            Parameter1.Base2.Detach();
            Parameter1.Base2.ValueChanged -= RefreshFunc;
            Parameter1.Base3.Detach();
            Parameter1.Base3.ValueChanged -= RefreshFunc;
            Parameter1.Base4.Detach();
            Parameter1.Base4.ValueChanged -= RefreshFunc;
            Parameter1.Base5.Detach();
            Parameter1.Base5.ValueChanged -= RefreshFunc;
            Parameter1.Base6.Detach();
            Parameter1.Base6.ValueChanged -= RefreshFunc;
            Parameter1.Base7.Detach();
            Parameter1.Base7.ValueChanged -= RefreshFunc;
        }

        protected override TResult GetValue()
        {
            return Func.Value;
        }
    }
}
