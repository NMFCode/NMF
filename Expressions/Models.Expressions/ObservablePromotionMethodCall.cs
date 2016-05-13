using System;
using System.Collections.Generic;
using System.ComponentModel;
using NMF.Models;

namespace NMF.Expressions
{
    internal class ObservablePromotionMethodCall<T1, TResult> : ObservableStaticMethodCall<T1, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition)
            : base(func, argument1)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, TResult> : ObservableStaticMethodCall<T1, T2, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition)
            : base(func, argument1, argument2)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, TResult> : ObservableStaticMethodCall<T1, T2, T3, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition)
            : base(func, argument1, argument2, argument3)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition)
            : base(func, argument1, argument2, argument3, argument4)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }
        public ICollection<string> Arg8Properties { get; private set; }
        public bool Arg8Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition, INotifyExpression<T8> argument8, ICollection<string> arg8Properties, bool arg8Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
            Arg8Properties = arg8Properties;
			Arg8Composition = arg8Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Attach();
            Argument8.ValueChanged += Argument8Changed;
            if (Arg8Properties != null)
            {
                RegisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange += Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged += Arg8PropertyChanged;
                }
            }
        }

        private void UnregisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange -= Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged -= Arg8PropertyChanged;
                }
            }
        }

        private void Arg8BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg8PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument8Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg8Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg8BubbledChange(oldValue);
                Refresh();
                RegisterArg8BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Detach();
            Argument8.ValueChanged -= Argument8Changed;
            if (Arg8Properties != null)
            {
                UnregisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition, Argument8.ApplyParameters(parameters), Arg8Properties, Arg8Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }
        public ICollection<string> Arg8Properties { get; private set; }
        public bool Arg8Composition { get; private set; }
        public ICollection<string> Arg9Properties { get; private set; }
        public bool Arg9Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition, INotifyExpression<T8> argument8, ICollection<string> arg8Properties, bool arg8Composition, INotifyExpression<T9> argument9, ICollection<string> arg9Properties, bool arg9Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
            Arg8Properties = arg8Properties;
			Arg8Composition = arg8Composition;
            Arg9Properties = arg9Properties;
			Arg9Composition = arg9Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Attach();
            Argument8.ValueChanged += Argument8Changed;
            if (Arg8Properties != null)
            {
                RegisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Attach();
            Argument9.ValueChanged += Argument9Changed;
            if (Arg9Properties != null)
            {
                RegisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange += Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged += Arg8PropertyChanged;
                }
            }
        }

        private void UnregisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange -= Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged -= Arg8PropertyChanged;
                }
            }
        }

        private void Arg8BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg8PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument8Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg8Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg8BubbledChange(oldValue);
                Refresh();
                RegisterArg8BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange += Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged += Arg9PropertyChanged;
                }
            }
        }

        private void UnregisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange -= Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged -= Arg9PropertyChanged;
                }
            }
        }

        private void Arg9BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg9PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument9Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg9Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg9BubbledChange(oldValue);
                Refresh();
                RegisterArg9BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Detach();
            Argument8.ValueChanged -= Argument8Changed;
            if (Arg8Properties != null)
            {
                UnregisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Detach();
            Argument9.ValueChanged -= Argument9Changed;
            if (Arg9Properties != null)
            {
                UnregisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition, Argument8.ApplyParameters(parameters), Arg8Properties, Arg8Composition, Argument9.ApplyParameters(parameters), Arg9Properties, Arg9Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }
        public ICollection<string> Arg8Properties { get; private set; }
        public bool Arg8Composition { get; private set; }
        public ICollection<string> Arg9Properties { get; private set; }
        public bool Arg9Composition { get; private set; }
        public ICollection<string> Arg10Properties { get; private set; }
        public bool Arg10Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition, INotifyExpression<T8> argument8, ICollection<string> arg8Properties, bool arg8Composition, INotifyExpression<T9> argument9, ICollection<string> arg9Properties, bool arg9Composition, INotifyExpression<T10> argument10, ICollection<string> arg10Properties, bool arg10Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
            Arg8Properties = arg8Properties;
			Arg8Composition = arg8Composition;
            Arg9Properties = arg9Properties;
			Arg9Composition = arg9Composition;
            Arg10Properties = arg10Properties;
			Arg10Composition = arg10Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Attach();
            Argument8.ValueChanged += Argument8Changed;
            if (Arg8Properties != null)
            {
                RegisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Attach();
            Argument9.ValueChanged += Argument9Changed;
            if (Arg9Properties != null)
            {
                RegisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Attach();
            Argument10.ValueChanged += Argument10Changed;
            if (Arg10Properties != null)
            {
                RegisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange += Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged += Arg8PropertyChanged;
                }
            }
        }

        private void UnregisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange -= Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged -= Arg8PropertyChanged;
                }
            }
        }

        private void Arg8BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg8PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument8Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg8Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg8BubbledChange(oldValue);
                Refresh();
                RegisterArg8BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange += Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged += Arg9PropertyChanged;
                }
            }
        }

        private void UnregisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange -= Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged -= Arg9PropertyChanged;
                }
            }
        }

        private void Arg9BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg9PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument9Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg9Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg9BubbledChange(oldValue);
                Refresh();
                RegisterArg9BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange += Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged += Arg10PropertyChanged;
                }
            }
        }

        private void UnregisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange -= Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged -= Arg10PropertyChanged;
                }
            }
        }

        private void Arg10BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg10PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument10Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg10Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg10BubbledChange(oldValue);
                Refresh();
                RegisterArg10BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Detach();
            Argument8.ValueChanged -= Argument8Changed;
            if (Arg8Properties != null)
            {
                UnregisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Detach();
            Argument9.ValueChanged -= Argument9Changed;
            if (Arg9Properties != null)
            {
                UnregisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Detach();
            Argument10.ValueChanged -= Argument10Changed;
            if (Arg10Properties != null)
            {
                UnregisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition, Argument8.ApplyParameters(parameters), Arg8Properties, Arg8Composition, Argument9.ApplyParameters(parameters), Arg9Properties, Arg9Composition, Argument10.ApplyParameters(parameters), Arg10Properties, Arg10Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }
        public ICollection<string> Arg8Properties { get; private set; }
        public bool Arg8Composition { get; private set; }
        public ICollection<string> Arg9Properties { get; private set; }
        public bool Arg9Composition { get; private set; }
        public ICollection<string> Arg10Properties { get; private set; }
        public bool Arg10Composition { get; private set; }
        public ICollection<string> Arg11Properties { get; private set; }
        public bool Arg11Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition, INotifyExpression<T8> argument8, ICollection<string> arg8Properties, bool arg8Composition, INotifyExpression<T9> argument9, ICollection<string> arg9Properties, bool arg9Composition, INotifyExpression<T10> argument10, ICollection<string> arg10Properties, bool arg10Composition, INotifyExpression<T11> argument11, ICollection<string> arg11Properties, bool arg11Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
            Arg8Properties = arg8Properties;
			Arg8Composition = arg8Composition;
            Arg9Properties = arg9Properties;
			Arg9Composition = arg9Composition;
            Arg10Properties = arg10Properties;
			Arg10Composition = arg10Composition;
            Arg11Properties = arg11Properties;
			Arg11Composition = arg11Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Attach();
            Argument8.ValueChanged += Argument8Changed;
            if (Arg8Properties != null)
            {
                RegisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Attach();
            Argument9.ValueChanged += Argument9Changed;
            if (Arg9Properties != null)
            {
                RegisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Attach();
            Argument10.ValueChanged += Argument10Changed;
            if (Arg10Properties != null)
            {
                RegisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Attach();
            Argument11.ValueChanged += Argument11Changed;
            if (Arg11Properties != null)
            {
                RegisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange += Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged += Arg8PropertyChanged;
                }
            }
        }

        private void UnregisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange -= Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged -= Arg8PropertyChanged;
                }
            }
        }

        private void Arg8BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg8PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument8Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg8Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg8BubbledChange(oldValue);
                Refresh();
                RegisterArg8BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange += Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged += Arg9PropertyChanged;
                }
            }
        }

        private void UnregisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange -= Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged -= Arg9PropertyChanged;
                }
            }
        }

        private void Arg9BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg9PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument9Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg9Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg9BubbledChange(oldValue);
                Refresh();
                RegisterArg9BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange += Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged += Arg10PropertyChanged;
                }
            }
        }

        private void UnregisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange -= Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged -= Arg10PropertyChanged;
                }
            }
        }

        private void Arg10BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg10PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument10Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg10Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg10BubbledChange(oldValue);
                Refresh();
                RegisterArg10BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange += Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged += Arg11PropertyChanged;
                }
            }
        }

        private void UnregisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange -= Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged -= Arg11PropertyChanged;
                }
            }
        }

        private void Arg11BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg11PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument11Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg11Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg11BubbledChange(oldValue);
                Refresh();
                RegisterArg11BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Detach();
            Argument8.ValueChanged -= Argument8Changed;
            if (Arg8Properties != null)
            {
                UnregisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Detach();
            Argument9.ValueChanged -= Argument9Changed;
            if (Arg9Properties != null)
            {
                UnregisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Detach();
            Argument10.ValueChanged -= Argument10Changed;
            if (Arg10Properties != null)
            {
                UnregisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Detach();
            Argument11.ValueChanged -= Argument11Changed;
            if (Arg11Properties != null)
            {
                UnregisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition, Argument8.ApplyParameters(parameters), Arg8Properties, Arg8Composition, Argument9.ApplyParameters(parameters), Arg9Properties, Arg9Composition, Argument10.ApplyParameters(parameters), Arg10Properties, Arg10Composition, Argument11.ApplyParameters(parameters), Arg11Properties, Arg11Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }
        public ICollection<string> Arg8Properties { get; private set; }
        public bool Arg8Composition { get; private set; }
        public ICollection<string> Arg9Properties { get; private set; }
        public bool Arg9Composition { get; private set; }
        public ICollection<string> Arg10Properties { get; private set; }
        public bool Arg10Composition { get; private set; }
        public ICollection<string> Arg11Properties { get; private set; }
        public bool Arg11Composition { get; private set; }
        public ICollection<string> Arg12Properties { get; private set; }
        public bool Arg12Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition, INotifyExpression<T8> argument8, ICollection<string> arg8Properties, bool arg8Composition, INotifyExpression<T9> argument9, ICollection<string> arg9Properties, bool arg9Composition, INotifyExpression<T10> argument10, ICollection<string> arg10Properties, bool arg10Composition, INotifyExpression<T11> argument11, ICollection<string> arg11Properties, bool arg11Composition, INotifyExpression<T12> argument12, ICollection<string> arg12Properties, bool arg12Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
            Arg8Properties = arg8Properties;
			Arg8Composition = arg8Composition;
            Arg9Properties = arg9Properties;
			Arg9Composition = arg9Composition;
            Arg10Properties = arg10Properties;
			Arg10Composition = arg10Composition;
            Arg11Properties = arg11Properties;
			Arg11Composition = arg11Composition;
            Arg12Properties = arg12Properties;
			Arg12Composition = arg12Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Attach();
            Argument8.ValueChanged += Argument8Changed;
            if (Arg8Properties != null)
            {
                RegisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Attach();
            Argument9.ValueChanged += Argument9Changed;
            if (Arg9Properties != null)
            {
                RegisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Attach();
            Argument10.ValueChanged += Argument10Changed;
            if (Arg10Properties != null)
            {
                RegisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Attach();
            Argument11.ValueChanged += Argument11Changed;
            if (Arg11Properties != null)
            {
                RegisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
            Argument12.Attach();
            Argument12.ValueChanged += Argument12Changed;
            if (Arg12Properties != null)
            {
                RegisterArg12BubbledChange((IModelElement)Argument12.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange += Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged += Arg8PropertyChanged;
                }
            }
        }

        private void UnregisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange -= Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged -= Arg8PropertyChanged;
                }
            }
        }

        private void Arg8BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg8PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument8Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg8Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg8BubbledChange(oldValue);
                Refresh();
                RegisterArg8BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange += Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged += Arg9PropertyChanged;
                }
            }
        }

        private void UnregisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange -= Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged -= Arg9PropertyChanged;
                }
            }
        }

        private void Arg9BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg9PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument9Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg9Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg9BubbledChange(oldValue);
                Refresh();
                RegisterArg9BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange += Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged += Arg10PropertyChanged;
                }
            }
        }

        private void UnregisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange -= Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged -= Arg10PropertyChanged;
                }
            }
        }

        private void Arg10BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg10PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument10Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg10Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg10BubbledChange(oldValue);
                Refresh();
                RegisterArg10BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange += Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged += Arg11PropertyChanged;
                }
            }
        }

        private void UnregisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange -= Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged -= Arg11PropertyChanged;
                }
            }
        }

        private void Arg11BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg11PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument11Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg11Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg11BubbledChange(oldValue);
                Refresh();
                RegisterArg11BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg12BubbledChange(IModelElement arg12)
        {
            if (arg12 != null)
            {
                if (Arg12Composition)
                {
                    arg12.BubbledChange += Arg12BubbledChange;
                }
                else
                {
                    arg12.PropertyChanged += Arg12PropertyChanged;
                }
            }
        }

        private void UnregisterArg12BubbledChange(IModelElement arg12)
        {
            if (arg12 != null)
            {
                if (Arg12Composition)
                {
                    arg12.BubbledChange -= Arg12BubbledChange;
                }
                else
                {
                    arg12.PropertyChanged -= Arg12PropertyChanged;
                }
            }
        }

        private void Arg12BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg12Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg12PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg12Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument12Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg12Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg12BubbledChange(oldValue);
                Refresh();
                RegisterArg12BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Detach();
            Argument8.ValueChanged -= Argument8Changed;
            if (Arg8Properties != null)
            {
                UnregisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Detach();
            Argument9.ValueChanged -= Argument9Changed;
            if (Arg9Properties != null)
            {
                UnregisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Detach();
            Argument10.ValueChanged -= Argument10Changed;
            if (Arg10Properties != null)
            {
                UnregisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Detach();
            Argument11.ValueChanged -= Argument11Changed;
            if (Arg11Properties != null)
            {
                UnregisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
            Argument12.Detach();
            Argument12.ValueChanged -= Argument12Changed;
            if (Arg12Properties != null)
            {
                UnregisterArg12BubbledChange((IModelElement)Argument12.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition, Argument8.ApplyParameters(parameters), Arg8Properties, Arg8Composition, Argument9.ApplyParameters(parameters), Arg9Properties, Arg9Composition, Argument10.ApplyParameters(parameters), Arg10Properties, Arg10Composition, Argument11.ApplyParameters(parameters), Arg11Properties, Arg11Composition, Argument12.ApplyParameters(parameters), Arg12Properties, Arg12Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }
        public ICollection<string> Arg8Properties { get; private set; }
        public bool Arg8Composition { get; private set; }
        public ICollection<string> Arg9Properties { get; private set; }
        public bool Arg9Composition { get; private set; }
        public ICollection<string> Arg10Properties { get; private set; }
        public bool Arg10Composition { get; private set; }
        public ICollection<string> Arg11Properties { get; private set; }
        public bool Arg11Composition { get; private set; }
        public ICollection<string> Arg12Properties { get; private set; }
        public bool Arg12Composition { get; private set; }
        public ICollection<string> Arg13Properties { get; private set; }
        public bool Arg13Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition, INotifyExpression<T8> argument8, ICollection<string> arg8Properties, bool arg8Composition, INotifyExpression<T9> argument9, ICollection<string> arg9Properties, bool arg9Composition, INotifyExpression<T10> argument10, ICollection<string> arg10Properties, bool arg10Composition, INotifyExpression<T11> argument11, ICollection<string> arg11Properties, bool arg11Composition, INotifyExpression<T12> argument12, ICollection<string> arg12Properties, bool arg12Composition, INotifyExpression<T13> argument13, ICollection<string> arg13Properties, bool arg13Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
            Arg8Properties = arg8Properties;
			Arg8Composition = arg8Composition;
            Arg9Properties = arg9Properties;
			Arg9Composition = arg9Composition;
            Arg10Properties = arg10Properties;
			Arg10Composition = arg10Composition;
            Arg11Properties = arg11Properties;
			Arg11Composition = arg11Composition;
            Arg12Properties = arg12Properties;
			Arg12Composition = arg12Composition;
            Arg13Properties = arg13Properties;
			Arg13Composition = arg13Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Attach();
            Argument8.ValueChanged += Argument8Changed;
            if (Arg8Properties != null)
            {
                RegisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Attach();
            Argument9.ValueChanged += Argument9Changed;
            if (Arg9Properties != null)
            {
                RegisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Attach();
            Argument10.ValueChanged += Argument10Changed;
            if (Arg10Properties != null)
            {
                RegisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Attach();
            Argument11.ValueChanged += Argument11Changed;
            if (Arg11Properties != null)
            {
                RegisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
            Argument12.Attach();
            Argument12.ValueChanged += Argument12Changed;
            if (Arg12Properties != null)
            {
                RegisterArg12BubbledChange((IModelElement)Argument12.Value);
            }
            Argument13.Attach();
            Argument13.ValueChanged += Argument13Changed;
            if (Arg13Properties != null)
            {
                RegisterArg13BubbledChange((IModelElement)Argument13.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange += Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged += Arg8PropertyChanged;
                }
            }
        }

        private void UnregisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange -= Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged -= Arg8PropertyChanged;
                }
            }
        }

        private void Arg8BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg8PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument8Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg8Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg8BubbledChange(oldValue);
                Refresh();
                RegisterArg8BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange += Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged += Arg9PropertyChanged;
                }
            }
        }

        private void UnregisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange -= Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged -= Arg9PropertyChanged;
                }
            }
        }

        private void Arg9BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg9PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument9Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg9Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg9BubbledChange(oldValue);
                Refresh();
                RegisterArg9BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange += Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged += Arg10PropertyChanged;
                }
            }
        }

        private void UnregisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange -= Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged -= Arg10PropertyChanged;
                }
            }
        }

        private void Arg10BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg10PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument10Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg10Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg10BubbledChange(oldValue);
                Refresh();
                RegisterArg10BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange += Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged += Arg11PropertyChanged;
                }
            }
        }

        private void UnregisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange -= Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged -= Arg11PropertyChanged;
                }
            }
        }

        private void Arg11BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg11PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument11Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg11Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg11BubbledChange(oldValue);
                Refresh();
                RegisterArg11BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg12BubbledChange(IModelElement arg12)
        {
            if (arg12 != null)
            {
                if (Arg12Composition)
                {
                    arg12.BubbledChange += Arg12BubbledChange;
                }
                else
                {
                    arg12.PropertyChanged += Arg12PropertyChanged;
                }
            }
        }

        private void UnregisterArg12BubbledChange(IModelElement arg12)
        {
            if (arg12 != null)
            {
                if (Arg12Composition)
                {
                    arg12.BubbledChange -= Arg12BubbledChange;
                }
                else
                {
                    arg12.PropertyChanged -= Arg12PropertyChanged;
                }
            }
        }

        private void Arg12BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg12Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg12PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg12Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument12Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg12Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg12BubbledChange(oldValue);
                Refresh();
                RegisterArg12BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg13BubbledChange(IModelElement arg13)
        {
            if (arg13 != null)
            {
                if (Arg13Composition)
                {
                    arg13.BubbledChange += Arg13BubbledChange;
                }
                else
                {
                    arg13.PropertyChanged += Arg13PropertyChanged;
                }
            }
        }

        private void UnregisterArg13BubbledChange(IModelElement arg13)
        {
            if (arg13 != null)
            {
                if (Arg13Composition)
                {
                    arg13.BubbledChange -= Arg13BubbledChange;
                }
                else
                {
                    arg13.PropertyChanged -= Arg13PropertyChanged;
                }
            }
        }

        private void Arg13BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg13Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg13PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg13Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument13Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg13Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg13BubbledChange(oldValue);
                Refresh();
                RegisterArg13BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Detach();
            Argument8.ValueChanged -= Argument8Changed;
            if (Arg8Properties != null)
            {
                UnregisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Detach();
            Argument9.ValueChanged -= Argument9Changed;
            if (Arg9Properties != null)
            {
                UnregisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Detach();
            Argument10.ValueChanged -= Argument10Changed;
            if (Arg10Properties != null)
            {
                UnregisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Detach();
            Argument11.ValueChanged -= Argument11Changed;
            if (Arg11Properties != null)
            {
                UnregisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
            Argument12.Detach();
            Argument12.ValueChanged -= Argument12Changed;
            if (Arg12Properties != null)
            {
                UnregisterArg12BubbledChange((IModelElement)Argument12.Value);
            }
            Argument13.Detach();
            Argument13.ValueChanged -= Argument13Changed;
            if (Arg13Properties != null)
            {
                UnregisterArg13BubbledChange((IModelElement)Argument13.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition, Argument8.ApplyParameters(parameters), Arg8Properties, Arg8Composition, Argument9.ApplyParameters(parameters), Arg9Properties, Arg9Composition, Argument10.ApplyParameters(parameters), Arg10Properties, Arg10Composition, Argument11.ApplyParameters(parameters), Arg11Properties, Arg11Composition, Argument12.ApplyParameters(parameters), Arg12Properties, Arg12Composition, Argument13.ApplyParameters(parameters), Arg13Properties, Arg13Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }
        public ICollection<string> Arg8Properties { get; private set; }
        public bool Arg8Composition { get; private set; }
        public ICollection<string> Arg9Properties { get; private set; }
        public bool Arg9Composition { get; private set; }
        public ICollection<string> Arg10Properties { get; private set; }
        public bool Arg10Composition { get; private set; }
        public ICollection<string> Arg11Properties { get; private set; }
        public bool Arg11Composition { get; private set; }
        public ICollection<string> Arg12Properties { get; private set; }
        public bool Arg12Composition { get; private set; }
        public ICollection<string> Arg13Properties { get; private set; }
        public bool Arg13Composition { get; private set; }
        public ICollection<string> Arg14Properties { get; private set; }
        public bool Arg14Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition, INotifyExpression<T8> argument8, ICollection<string> arg8Properties, bool arg8Composition, INotifyExpression<T9> argument9, ICollection<string> arg9Properties, bool arg9Composition, INotifyExpression<T10> argument10, ICollection<string> arg10Properties, bool arg10Composition, INotifyExpression<T11> argument11, ICollection<string> arg11Properties, bool arg11Composition, INotifyExpression<T12> argument12, ICollection<string> arg12Properties, bool arg12Composition, INotifyExpression<T13> argument13, ICollection<string> arg13Properties, bool arg13Composition, INotifyExpression<T14> argument14, ICollection<string> arg14Properties, bool arg14Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
            Arg8Properties = arg8Properties;
			Arg8Composition = arg8Composition;
            Arg9Properties = arg9Properties;
			Arg9Composition = arg9Composition;
            Arg10Properties = arg10Properties;
			Arg10Composition = arg10Composition;
            Arg11Properties = arg11Properties;
			Arg11Composition = arg11Composition;
            Arg12Properties = arg12Properties;
			Arg12Composition = arg12Composition;
            Arg13Properties = arg13Properties;
			Arg13Composition = arg13Composition;
            Arg14Properties = arg14Properties;
			Arg14Composition = arg14Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Attach();
            Argument8.ValueChanged += Argument8Changed;
            if (Arg8Properties != null)
            {
                RegisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Attach();
            Argument9.ValueChanged += Argument9Changed;
            if (Arg9Properties != null)
            {
                RegisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Attach();
            Argument10.ValueChanged += Argument10Changed;
            if (Arg10Properties != null)
            {
                RegisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Attach();
            Argument11.ValueChanged += Argument11Changed;
            if (Arg11Properties != null)
            {
                RegisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
            Argument12.Attach();
            Argument12.ValueChanged += Argument12Changed;
            if (Arg12Properties != null)
            {
                RegisterArg12BubbledChange((IModelElement)Argument12.Value);
            }
            Argument13.Attach();
            Argument13.ValueChanged += Argument13Changed;
            if (Arg13Properties != null)
            {
                RegisterArg13BubbledChange((IModelElement)Argument13.Value);
            }
            Argument14.Attach();
            Argument14.ValueChanged += Argument14Changed;
            if (Arg14Properties != null)
            {
                RegisterArg14BubbledChange((IModelElement)Argument14.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange += Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged += Arg8PropertyChanged;
                }
            }
        }

        private void UnregisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange -= Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged -= Arg8PropertyChanged;
                }
            }
        }

        private void Arg8BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg8PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument8Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg8Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg8BubbledChange(oldValue);
                Refresh();
                RegisterArg8BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange += Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged += Arg9PropertyChanged;
                }
            }
        }

        private void UnregisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange -= Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged -= Arg9PropertyChanged;
                }
            }
        }

        private void Arg9BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg9PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument9Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg9Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg9BubbledChange(oldValue);
                Refresh();
                RegisterArg9BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange += Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged += Arg10PropertyChanged;
                }
            }
        }

        private void UnregisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange -= Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged -= Arg10PropertyChanged;
                }
            }
        }

        private void Arg10BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg10PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument10Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg10Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg10BubbledChange(oldValue);
                Refresh();
                RegisterArg10BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange += Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged += Arg11PropertyChanged;
                }
            }
        }

        private void UnregisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange -= Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged -= Arg11PropertyChanged;
                }
            }
        }

        private void Arg11BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg11PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument11Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg11Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg11BubbledChange(oldValue);
                Refresh();
                RegisterArg11BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg12BubbledChange(IModelElement arg12)
        {
            if (arg12 != null)
            {
                if (Arg12Composition)
                {
                    arg12.BubbledChange += Arg12BubbledChange;
                }
                else
                {
                    arg12.PropertyChanged += Arg12PropertyChanged;
                }
            }
        }

        private void UnregisterArg12BubbledChange(IModelElement arg12)
        {
            if (arg12 != null)
            {
                if (Arg12Composition)
                {
                    arg12.BubbledChange -= Arg12BubbledChange;
                }
                else
                {
                    arg12.PropertyChanged -= Arg12PropertyChanged;
                }
            }
        }

        private void Arg12BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg12Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg12PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg12Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument12Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg12Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg12BubbledChange(oldValue);
                Refresh();
                RegisterArg12BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg13BubbledChange(IModelElement arg13)
        {
            if (arg13 != null)
            {
                if (Arg13Composition)
                {
                    arg13.BubbledChange += Arg13BubbledChange;
                }
                else
                {
                    arg13.PropertyChanged += Arg13PropertyChanged;
                }
            }
        }

        private void UnregisterArg13BubbledChange(IModelElement arg13)
        {
            if (arg13 != null)
            {
                if (Arg13Composition)
                {
                    arg13.BubbledChange -= Arg13BubbledChange;
                }
                else
                {
                    arg13.PropertyChanged -= Arg13PropertyChanged;
                }
            }
        }

        private void Arg13BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg13Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg13PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg13Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument13Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg13Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg13BubbledChange(oldValue);
                Refresh();
                RegisterArg13BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg14BubbledChange(IModelElement arg14)
        {
            if (arg14 != null)
            {
                if (Arg14Composition)
                {
                    arg14.BubbledChange += Arg14BubbledChange;
                }
                else
                {
                    arg14.PropertyChanged += Arg14PropertyChanged;
                }
            }
        }

        private void UnregisterArg14BubbledChange(IModelElement arg14)
        {
            if (arg14 != null)
            {
                if (Arg14Composition)
                {
                    arg14.BubbledChange -= Arg14BubbledChange;
                }
                else
                {
                    arg14.PropertyChanged -= Arg14PropertyChanged;
                }
            }
        }

        private void Arg14BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg14Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg14PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg14Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument14Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg14Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg14BubbledChange(oldValue);
                Refresh();
                RegisterArg14BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Detach();
            Argument8.ValueChanged -= Argument8Changed;
            if (Arg8Properties != null)
            {
                UnregisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Detach();
            Argument9.ValueChanged -= Argument9Changed;
            if (Arg9Properties != null)
            {
                UnregisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Detach();
            Argument10.ValueChanged -= Argument10Changed;
            if (Arg10Properties != null)
            {
                UnregisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Detach();
            Argument11.ValueChanged -= Argument11Changed;
            if (Arg11Properties != null)
            {
                UnregisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
            Argument12.Detach();
            Argument12.ValueChanged -= Argument12Changed;
            if (Arg12Properties != null)
            {
                UnregisterArg12BubbledChange((IModelElement)Argument12.Value);
            }
            Argument13.Detach();
            Argument13.ValueChanged -= Argument13Changed;
            if (Arg13Properties != null)
            {
                UnregisterArg13BubbledChange((IModelElement)Argument13.Value);
            }
            Argument14.Detach();
            Argument14.ValueChanged -= Argument14Changed;
            if (Arg14Properties != null)
            {
                UnregisterArg14BubbledChange((IModelElement)Argument14.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition, Argument8.ApplyParameters(parameters), Arg8Properties, Arg8Composition, Argument9.ApplyParameters(parameters), Arg9Properties, Arg9Composition, Argument10.ApplyParameters(parameters), Arg10Properties, Arg10Composition, Argument11.ApplyParameters(parameters), Arg11Properties, Arg11Composition, Argument12.ApplyParameters(parameters), Arg12Properties, Arg12Composition, Argument13.ApplyParameters(parameters), Arg13Properties, Arg13Composition, Argument14.ApplyParameters(parameters), Arg14Properties, Arg14Composition);
        }
    }
    internal class ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
    {
        public ICollection<string> Arg1Properties { get; private set; }
        public bool Arg1Composition { get; private set; }
        public ICollection<string> Arg2Properties { get; private set; }
        public bool Arg2Composition { get; private set; }
        public ICollection<string> Arg3Properties { get; private set; }
        public bool Arg3Composition { get; private set; }
        public ICollection<string> Arg4Properties { get; private set; }
        public bool Arg4Composition { get; private set; }
        public ICollection<string> Arg5Properties { get; private set; }
        public bool Arg5Composition { get; private set; }
        public ICollection<string> Arg6Properties { get; private set; }
        public bool Arg6Composition { get; private set; }
        public ICollection<string> Arg7Properties { get; private set; }
        public bool Arg7Composition { get; private set; }
        public ICollection<string> Arg8Properties { get; private set; }
        public bool Arg8Composition { get; private set; }
        public ICollection<string> Arg9Properties { get; private set; }
        public bool Arg9Composition { get; private set; }
        public ICollection<string> Arg10Properties { get; private set; }
        public bool Arg10Composition { get; private set; }
        public ICollection<string> Arg11Properties { get; private set; }
        public bool Arg11Composition { get; private set; }
        public ICollection<string> Arg12Properties { get; private set; }
        public bool Arg12Composition { get; private set; }
        public ICollection<string> Arg13Properties { get; private set; }
        public bool Arg13Composition { get; private set; }
        public ICollection<string> Arg14Properties { get; private set; }
        public bool Arg14Composition { get; private set; }
        public ICollection<string> Arg15Properties { get; private set; }
        public bool Arg15Composition { get; private set; }

        public ObservablePromotionMethodCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, INotifyExpression<T1> argument1, ICollection<string> arg1Properties, bool arg1Composition, INotifyExpression<T2> argument2, ICollection<string> arg2Properties, bool arg2Composition, INotifyExpression<T3> argument3, ICollection<string> arg3Properties, bool arg3Composition, INotifyExpression<T4> argument4, ICollection<string> arg4Properties, bool arg4Composition, INotifyExpression<T5> argument5, ICollection<string> arg5Properties, bool arg5Composition, INotifyExpression<T6> argument6, ICollection<string> arg6Properties, bool arg6Composition, INotifyExpression<T7> argument7, ICollection<string> arg7Properties, bool arg7Composition, INotifyExpression<T8> argument8, ICollection<string> arg8Properties, bool arg8Composition, INotifyExpression<T9> argument9, ICollection<string> arg9Properties, bool arg9Composition, INotifyExpression<T10> argument10, ICollection<string> arg10Properties, bool arg10Composition, INotifyExpression<T11> argument11, ICollection<string> arg11Properties, bool arg11Composition, INotifyExpression<T12> argument12, ICollection<string> arg12Properties, bool arg12Composition, INotifyExpression<T13> argument13, ICollection<string> arg13Properties, bool arg13Composition, INotifyExpression<T14> argument14, ICollection<string> arg14Properties, bool arg14Composition, INotifyExpression<T15> argument15, ICollection<string> arg15Properties, bool arg15Composition)
            : base(func, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15)
        {
            Arg1Properties = arg1Properties;
			Arg1Composition = arg1Composition;
            Arg2Properties = arg2Properties;
			Arg2Composition = arg2Composition;
            Arg3Properties = arg3Properties;
			Arg3Composition = arg3Composition;
            Arg4Properties = arg4Properties;
			Arg4Composition = arg4Composition;
            Arg5Properties = arg5Properties;
			Arg5Composition = arg5Composition;
            Arg6Properties = arg6Properties;
			Arg6Composition = arg6Composition;
            Arg7Properties = arg7Properties;
			Arg7Composition = arg7Composition;
            Arg8Properties = arg8Properties;
			Arg8Composition = arg8Composition;
            Arg9Properties = arg9Properties;
			Arg9Composition = arg9Composition;
            Arg10Properties = arg10Properties;
			Arg10Composition = arg10Composition;
            Arg11Properties = arg11Properties;
			Arg11Composition = arg11Composition;
            Arg12Properties = arg12Properties;
			Arg12Composition = arg12Composition;
            Arg13Properties = arg13Properties;
			Arg13Composition = arg13Composition;
            Arg14Properties = arg14Properties;
			Arg14Composition = arg14Composition;
            Arg15Properties = arg15Properties;
			Arg15Composition = arg15Composition;
        }

        protected override void AttachCore()
        {
            Argument1.Attach();
            Argument1.ValueChanged += Argument1Changed;
            if (Arg1Properties != null)
            {
                RegisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Attach();
            Argument2.ValueChanged += Argument2Changed;
            if (Arg2Properties != null)
            {
                RegisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Attach();
            Argument3.ValueChanged += Argument3Changed;
            if (Arg3Properties != null)
            {
                RegisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Attach();
            Argument4.ValueChanged += Argument4Changed;
            if (Arg4Properties != null)
            {
                RegisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Attach();
            Argument5.ValueChanged += Argument5Changed;
            if (Arg5Properties != null)
            {
                RegisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Attach();
            Argument6.ValueChanged += Argument6Changed;
            if (Arg6Properties != null)
            {
                RegisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Attach();
            Argument7.ValueChanged += Argument7Changed;
            if (Arg7Properties != null)
            {
                RegisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Attach();
            Argument8.ValueChanged += Argument8Changed;
            if (Arg8Properties != null)
            {
                RegisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Attach();
            Argument9.ValueChanged += Argument9Changed;
            if (Arg9Properties != null)
            {
                RegisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Attach();
            Argument10.ValueChanged += Argument10Changed;
            if (Arg10Properties != null)
            {
                RegisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Attach();
            Argument11.ValueChanged += Argument11Changed;
            if (Arg11Properties != null)
            {
                RegisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
            Argument12.Attach();
            Argument12.ValueChanged += Argument12Changed;
            if (Arg12Properties != null)
            {
                RegisterArg12BubbledChange((IModelElement)Argument12.Value);
            }
            Argument13.Attach();
            Argument13.ValueChanged += Argument13Changed;
            if (Arg13Properties != null)
            {
                RegisterArg13BubbledChange((IModelElement)Argument13.Value);
            }
            Argument14.Attach();
            Argument14.ValueChanged += Argument14Changed;
            if (Arg14Properties != null)
            {
                RegisterArg14BubbledChange((IModelElement)Argument14.Value);
            }
            Argument15.Attach();
            Argument15.ValueChanged += Argument15Changed;
            if (Arg15Properties != null)
            {
                RegisterArg15BubbledChange((IModelElement)Argument15.Value);
            }
        }
		
        private void RegisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange += Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged += Arg1PropertyChanged;
                }
            }
        }

        private void UnregisterArg1BubbledChange(IModelElement arg1)
        {
            if (arg1 != null)
            {
                if (Arg1Composition)
                {
                    arg1.BubbledChange -= Arg1BubbledChange;
                }
                else
                {
                    arg1.PropertyChanged -= Arg1PropertyChanged;
                }
            }
        }

        private void Arg1BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg1PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg1Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument1Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg1Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg1BubbledChange(oldValue);
                Refresh();
                RegisterArg1BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange += Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged += Arg2PropertyChanged;
                }
            }
        }

        private void UnregisterArg2BubbledChange(IModelElement arg2)
        {
            if (arg2 != null)
            {
                if (Arg2Composition)
                {
                    arg2.BubbledChange -= Arg2BubbledChange;
                }
                else
                {
                    arg2.PropertyChanged -= Arg2PropertyChanged;
                }
            }
        }

        private void Arg2BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg2PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg2Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument2Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg2Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg2BubbledChange(oldValue);
                Refresh();
                RegisterArg2BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange += Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged += Arg3PropertyChanged;
                }
            }
        }

        private void UnregisterArg3BubbledChange(IModelElement arg3)
        {
            if (arg3 != null)
            {
                if (Arg3Composition)
                {
                    arg3.BubbledChange -= Arg3BubbledChange;
                }
                else
                {
                    arg3.PropertyChanged -= Arg3PropertyChanged;
                }
            }
        }

        private void Arg3BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg3PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg3Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument3Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg3Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg3BubbledChange(oldValue);
                Refresh();
                RegisterArg3BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange += Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged += Arg4PropertyChanged;
                }
            }
        }

        private void UnregisterArg4BubbledChange(IModelElement arg4)
        {
            if (arg4 != null)
            {
                if (Arg4Composition)
                {
                    arg4.BubbledChange -= Arg4BubbledChange;
                }
                else
                {
                    arg4.PropertyChanged -= Arg4PropertyChanged;
                }
            }
        }

        private void Arg4BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg4PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg4Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument4Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg4Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg4BubbledChange(oldValue);
                Refresh();
                RegisterArg4BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange += Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged += Arg5PropertyChanged;
                }
            }
        }

        private void UnregisterArg5BubbledChange(IModelElement arg5)
        {
            if (arg5 != null)
            {
                if (Arg5Composition)
                {
                    arg5.BubbledChange -= Arg5BubbledChange;
                }
                else
                {
                    arg5.PropertyChanged -= Arg5PropertyChanged;
                }
            }
        }

        private void Arg5BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg5PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg5Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument5Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg5Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg5BubbledChange(oldValue);
                Refresh();
                RegisterArg5BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange += Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged += Arg6PropertyChanged;
                }
            }
        }

        private void UnregisterArg6BubbledChange(IModelElement arg6)
        {
            if (arg6 != null)
            {
                if (Arg6Composition)
                {
                    arg6.BubbledChange -= Arg6BubbledChange;
                }
                else
                {
                    arg6.PropertyChanged -= Arg6PropertyChanged;
                }
            }
        }

        private void Arg6BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg6PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg6Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument6Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg6Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg6BubbledChange(oldValue);
                Refresh();
                RegisterArg6BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange += Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged += Arg7PropertyChanged;
                }
            }
        }

        private void UnregisterArg7BubbledChange(IModelElement arg7)
        {
            if (arg7 != null)
            {
                if (Arg7Composition)
                {
                    arg7.BubbledChange -= Arg7BubbledChange;
                }
                else
                {
                    arg7.PropertyChanged -= Arg7PropertyChanged;
                }
            }
        }

        private void Arg7BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg7PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg7Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument7Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg7Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg7BubbledChange(oldValue);
                Refresh();
                RegisterArg7BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange += Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged += Arg8PropertyChanged;
                }
            }
        }

        private void UnregisterArg8BubbledChange(IModelElement arg8)
        {
            if (arg8 != null)
            {
                if (Arg8Composition)
                {
                    arg8.BubbledChange -= Arg8BubbledChange;
                }
                else
                {
                    arg8.PropertyChanged -= Arg8PropertyChanged;
                }
            }
        }

        private void Arg8BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg8PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg8Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument8Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg8Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg8BubbledChange(oldValue);
                Refresh();
                RegisterArg8BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange += Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged += Arg9PropertyChanged;
                }
            }
        }

        private void UnregisterArg9BubbledChange(IModelElement arg9)
        {
            if (arg9 != null)
            {
                if (Arg9Composition)
                {
                    arg9.BubbledChange -= Arg9BubbledChange;
                }
                else
                {
                    arg9.PropertyChanged -= Arg9PropertyChanged;
                }
            }
        }

        private void Arg9BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg9PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg9Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument9Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg9Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg9BubbledChange(oldValue);
                Refresh();
                RegisterArg9BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange += Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged += Arg10PropertyChanged;
                }
            }
        }

        private void UnregisterArg10BubbledChange(IModelElement arg10)
        {
            if (arg10 != null)
            {
                if (Arg10Composition)
                {
                    arg10.BubbledChange -= Arg10BubbledChange;
                }
                else
                {
                    arg10.PropertyChanged -= Arg10PropertyChanged;
                }
            }
        }

        private void Arg10BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg10PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg10Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument10Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg10Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg10BubbledChange(oldValue);
                Refresh();
                RegisterArg10BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange += Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged += Arg11PropertyChanged;
                }
            }
        }

        private void UnregisterArg11BubbledChange(IModelElement arg11)
        {
            if (arg11 != null)
            {
                if (Arg11Composition)
                {
                    arg11.BubbledChange -= Arg11BubbledChange;
                }
                else
                {
                    arg11.PropertyChanged -= Arg11PropertyChanged;
                }
            }
        }

        private void Arg11BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg11PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg11Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument11Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg11Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg11BubbledChange(oldValue);
                Refresh();
                RegisterArg11BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg12BubbledChange(IModelElement arg12)
        {
            if (arg12 != null)
            {
                if (Arg12Composition)
                {
                    arg12.BubbledChange += Arg12BubbledChange;
                }
                else
                {
                    arg12.PropertyChanged += Arg12PropertyChanged;
                }
            }
        }

        private void UnregisterArg12BubbledChange(IModelElement arg12)
        {
            if (arg12 != null)
            {
                if (Arg12Composition)
                {
                    arg12.BubbledChange -= Arg12BubbledChange;
                }
                else
                {
                    arg12.PropertyChanged -= Arg12PropertyChanged;
                }
            }
        }

        private void Arg12BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg12Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg12PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg12Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument12Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg12Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg12BubbledChange(oldValue);
                Refresh();
                RegisterArg12BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg13BubbledChange(IModelElement arg13)
        {
            if (arg13 != null)
            {
                if (Arg13Composition)
                {
                    arg13.BubbledChange += Arg13BubbledChange;
                }
                else
                {
                    arg13.PropertyChanged += Arg13PropertyChanged;
                }
            }
        }

        private void UnregisterArg13BubbledChange(IModelElement arg13)
        {
            if (arg13 != null)
            {
                if (Arg13Composition)
                {
                    arg13.BubbledChange -= Arg13BubbledChange;
                }
                else
                {
                    arg13.PropertyChanged -= Arg13PropertyChanged;
                }
            }
        }

        private void Arg13BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg13Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg13PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg13Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument13Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg13Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg13BubbledChange(oldValue);
                Refresh();
                RegisterArg13BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg14BubbledChange(IModelElement arg14)
        {
            if (arg14 != null)
            {
                if (Arg14Composition)
                {
                    arg14.BubbledChange += Arg14BubbledChange;
                }
                else
                {
                    arg14.PropertyChanged += Arg14PropertyChanged;
                }
            }
        }

        private void UnregisterArg14BubbledChange(IModelElement arg14)
        {
            if (arg14 != null)
            {
                if (Arg14Composition)
                {
                    arg14.BubbledChange -= Arg14BubbledChange;
                }
                else
                {
                    arg14.PropertyChanged -= Arg14PropertyChanged;
                }
            }
        }

        private void Arg14BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg14Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg14PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg14Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument14Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg14Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg14BubbledChange(oldValue);
                Refresh();
                RegisterArg14BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }
        private void RegisterArg15BubbledChange(IModelElement arg15)
        {
            if (arg15 != null)
            {
                if (Arg15Composition)
                {
                    arg15.BubbledChange += Arg15BubbledChange;
                }
                else
                {
                    arg15.PropertyChanged += Arg15PropertyChanged;
                }
            }
        }

        private void UnregisterArg15BubbledChange(IModelElement arg15)
        {
            if (arg15 != null)
            {
                if (Arg15Composition)
                {
                    arg15.BubbledChange -= Arg15BubbledChange;
                }
                else
                {
                    arg15.PropertyChanged -= Arg15PropertyChanged;
                }
            }
        }

        private void Arg15BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (IsAttached && Arg15Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Arg15PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttached && Arg15Properties.Contains(e.PropertyName))
            {
                Refresh();
            }
        }

        private void Argument15Changed(object sender, ValueChangedEventArgs e)
        {
            if (Arg15Properties != null && IsAttached)
            {
                var oldValue = e.OldValue as IModelElement;
                var newValue = e.NewValue as IModelElement;
                UnregisterArg15BubbledChange(oldValue);
                Refresh();
                RegisterArg15BubbledChange(newValue);
            }
            else
            {
                Refresh();
            }
        }

        protected override void DetachCore()
        {
            Argument1.Detach();
            Argument1.ValueChanged -= Argument1Changed;
            if (Arg1Properties != null)
            {
                UnregisterArg1BubbledChange((IModelElement)Argument1.Value);
            }
            Argument2.Detach();
            Argument2.ValueChanged -= Argument2Changed;
            if (Arg2Properties != null)
            {
                UnregisterArg2BubbledChange((IModelElement)Argument2.Value);
            }
            Argument3.Detach();
            Argument3.ValueChanged -= Argument3Changed;
            if (Arg3Properties != null)
            {
                UnregisterArg3BubbledChange((IModelElement)Argument3.Value);
            }
            Argument4.Detach();
            Argument4.ValueChanged -= Argument4Changed;
            if (Arg4Properties != null)
            {
                UnregisterArg4BubbledChange((IModelElement)Argument4.Value);
            }
            Argument5.Detach();
            Argument5.ValueChanged -= Argument5Changed;
            if (Arg5Properties != null)
            {
                UnregisterArg5BubbledChange((IModelElement)Argument5.Value);
            }
            Argument6.Detach();
            Argument6.ValueChanged -= Argument6Changed;
            if (Arg6Properties != null)
            {
                UnregisterArg6BubbledChange((IModelElement)Argument6.Value);
            }
            Argument7.Detach();
            Argument7.ValueChanged -= Argument7Changed;
            if (Arg7Properties != null)
            {
                UnregisterArg7BubbledChange((IModelElement)Argument7.Value);
            }
            Argument8.Detach();
            Argument8.ValueChanged -= Argument8Changed;
            if (Arg8Properties != null)
            {
                UnregisterArg8BubbledChange((IModelElement)Argument8.Value);
            }
            Argument9.Detach();
            Argument9.ValueChanged -= Argument9Changed;
            if (Arg9Properties != null)
            {
                UnregisterArg9BubbledChange((IModelElement)Argument9.Value);
            }
            Argument10.Detach();
            Argument10.ValueChanged -= Argument10Changed;
            if (Arg10Properties != null)
            {
                UnregisterArg10BubbledChange((IModelElement)Argument10.Value);
            }
            Argument11.Detach();
            Argument11.ValueChanged -= Argument11Changed;
            if (Arg11Properties != null)
            {
                UnregisterArg11BubbledChange((IModelElement)Argument11.Value);
            }
            Argument12.Detach();
            Argument12.ValueChanged -= Argument12Changed;
            if (Arg12Properties != null)
            {
                UnregisterArg12BubbledChange((IModelElement)Argument12.Value);
            }
            Argument13.Detach();
            Argument13.ValueChanged -= Argument13Changed;
            if (Arg13Properties != null)
            {
                UnregisterArg13BubbledChange((IModelElement)Argument13.Value);
            }
            Argument14.Detach();
            Argument14.ValueChanged -= Argument14Changed;
            if (Arg14Properties != null)
            {
                UnregisterArg14BubbledChange((IModelElement)Argument14.Value);
            }
            Argument15.Detach();
            Argument15.ValueChanged -= Argument15Changed;
            if (Arg15Properties != null)
            {
                UnregisterArg15BubbledChange((IModelElement)Argument15.Value);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservablePromotionMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Function, Argument1.ApplyParameters(parameters), Arg1Properties, Arg1Composition, Argument2.ApplyParameters(parameters), Arg2Properties, Arg2Composition, Argument3.ApplyParameters(parameters), Arg3Properties, Arg3Composition, Argument4.ApplyParameters(parameters), Arg4Properties, Arg4Composition, Argument5.ApplyParameters(parameters), Arg5Properties, Arg5Composition, Argument6.ApplyParameters(parameters), Arg6Properties, Arg6Composition, Argument7.ApplyParameters(parameters), Arg7Properties, Arg7Composition, Argument8.ApplyParameters(parameters), Arg8Properties, Arg8Composition, Argument9.ApplyParameters(parameters), Arg9Properties, Arg9Composition, Argument10.ApplyParameters(parameters), Arg10Properties, Arg10Composition, Argument11.ApplyParameters(parameters), Arg11Properties, Arg11Composition, Argument12.ApplyParameters(parameters), Arg12Properties, Arg12Composition, Argument13.ApplyParameters(parameters), Arg13Properties, Arg13Composition, Argument14.ApplyParameters(parameters), Arg14Properties, Arg14Composition, Argument15.ApplyParameters(parameters), Arg15Properties, Arg15Composition);
        }
    }
}
