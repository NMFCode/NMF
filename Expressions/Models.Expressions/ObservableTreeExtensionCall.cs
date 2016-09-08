using System;
using System.Collections.Generic;
using System.Linq;
using NMF.Models;


namespace NMF.Expressions
{
    class ObservableTreeExtensionCall<T1, TResult> : ObservableStaticMethodCall<T1, TResult>
        where T1 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;

        public ICollection<string> Arg1Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties) : base(function, arg1)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, TResult> : ObservableStaticMethodCall<T1, T2, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties) : base(function, arg1, arg2)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, TResult> : ObservableStaticMethodCall<T1, T2, T3, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties) : base(function, arg1, arg2, arg3)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties) : base(function, arg1, arg2, arg3, arg4)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties) : base(function, arg1, arg2, arg3, arg4, arg5)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
        where T8 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;
        private List<BubbledChangeListener> anchor8Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }
        public ICollection<string> Arg8Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties, INotifyExpression<T8> arg8, IEnumerable<Type> anchors8, ICollection<string> arg8Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors8 != null && anchors8.Any())
            {
                anchor8Listener = anchors8.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
            Arg8Properties = arg8Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.Element = Argument8.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties, 
                Argument8.ApplyParameters(parameters), anchor8Listener?.Select(l => l.Type), Arg8Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
        where T8 : IModelElement
        where T9 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;
        private List<BubbledChangeListener> anchor8Listener;
        private List<BubbledChangeListener> anchor9Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }
        public ICollection<string> Arg8Properties { get; set; }
        public ICollection<string> Arg9Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties, INotifyExpression<T8> arg8, IEnumerable<Type> anchors8, ICollection<string> arg8Properties, INotifyExpression<T9> arg9, IEnumerable<Type> anchors9, ICollection<string> arg9Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors8 != null && anchors8.Any())
            {
                anchor8Listener = anchors8.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors9 != null && anchors9.Any())
            {
                anchor9Listener = anchors9.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
            Arg8Properties = arg8Properties;
            Arg9Properties = arg9Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.Element = Argument8.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.Element = Argument9.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties, 
                Argument8.ApplyParameters(parameters), anchor8Listener?.Select(l => l.Type), Arg8Properties, 
                Argument9.ApplyParameters(parameters), anchor9Listener?.Select(l => l.Type), Arg9Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
        where T8 : IModelElement
        where T9 : IModelElement
        where T10 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;
        private List<BubbledChangeListener> anchor8Listener;
        private List<BubbledChangeListener> anchor9Listener;
        private List<BubbledChangeListener> anchor10Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }
        public ICollection<string> Arg8Properties { get; set; }
        public ICollection<string> Arg9Properties { get; set; }
        public ICollection<string> Arg10Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties, INotifyExpression<T8> arg8, IEnumerable<Type> anchors8, ICollection<string> arg8Properties, INotifyExpression<T9> arg9, IEnumerable<Type> anchors9, ICollection<string> arg9Properties, INotifyExpression<T10> arg10, IEnumerable<Type> anchors10, ICollection<string> arg10Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors8 != null && anchors8.Any())
            {
                anchor8Listener = anchors8.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors9 != null && anchors9.Any())
            {
                anchor9Listener = anchors9.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors10 != null && anchors10.Any())
            {
                anchor10Listener = anchors10.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
            Arg8Properties = arg8Properties;
            Arg9Properties = arg9Properties;
            Arg10Properties = arg10Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.Element = Argument8.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.Element = Argument9.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.Element = Argument10.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties, 
                Argument8.ApplyParameters(parameters), anchor8Listener?.Select(l => l.Type), Arg8Properties, 
                Argument9.ApplyParameters(parameters), anchor9Listener?.Select(l => l.Type), Arg9Properties, 
                Argument10.ApplyParameters(parameters), anchor10Listener?.Select(l => l.Type), Arg10Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
        where T8 : IModelElement
        where T9 : IModelElement
        where T10 : IModelElement
        where T11 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;
        private List<BubbledChangeListener> anchor8Listener;
        private List<BubbledChangeListener> anchor9Listener;
        private List<BubbledChangeListener> anchor10Listener;
        private List<BubbledChangeListener> anchor11Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }
        public ICollection<string> Arg8Properties { get; set; }
        public ICollection<string> Arg9Properties { get; set; }
        public ICollection<string> Arg10Properties { get; set; }
        public ICollection<string> Arg11Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties, INotifyExpression<T8> arg8, IEnumerable<Type> anchors8, ICollection<string> arg8Properties, INotifyExpression<T9> arg9, IEnumerable<Type> anchors9, ICollection<string> arg9Properties, INotifyExpression<T10> arg10, IEnumerable<Type> anchors10, ICollection<string> arg10Properties, INotifyExpression<T11> arg11, IEnumerable<Type> anchors11, ICollection<string> arg11Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors8 != null && anchors8.Any())
            {
                anchor8Listener = anchors8.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors9 != null && anchors9.Any())
            {
                anchor9Listener = anchors9.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors10 != null && anchors10.Any())
            {
                anchor10Listener = anchors10.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors11 != null && anchors11.Any())
            {
                anchor11Listener = anchors11.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
            Arg8Properties = arg8Properties;
            Arg9Properties = arg9Properties;
            Arg10Properties = arg10Properties;
            Arg11Properties = arg11Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.Element = Argument8.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.Element = Argument9.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.Element = Argument10.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.Element = Argument11.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties, 
                Argument8.ApplyParameters(parameters), anchor8Listener?.Select(l => l.Type), Arg8Properties, 
                Argument9.ApplyParameters(parameters), anchor9Listener?.Select(l => l.Type), Arg9Properties, 
                Argument10.ApplyParameters(parameters), anchor10Listener?.Select(l => l.Type), Arg10Properties, 
                Argument11.ApplyParameters(parameters), anchor11Listener?.Select(l => l.Type), Arg11Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
        where T8 : IModelElement
        where T9 : IModelElement
        where T10 : IModelElement
        where T11 : IModelElement
        where T12 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;
        private List<BubbledChangeListener> anchor8Listener;
        private List<BubbledChangeListener> anchor9Listener;
        private List<BubbledChangeListener> anchor10Listener;
        private List<BubbledChangeListener> anchor11Listener;
        private List<BubbledChangeListener> anchor12Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }
        public ICollection<string> Arg8Properties { get; set; }
        public ICollection<string> Arg9Properties { get; set; }
        public ICollection<string> Arg10Properties { get; set; }
        public ICollection<string> Arg11Properties { get; set; }
        public ICollection<string> Arg12Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties, INotifyExpression<T8> arg8, IEnumerable<Type> anchors8, ICollection<string> arg8Properties, INotifyExpression<T9> arg9, IEnumerable<Type> anchors9, ICollection<string> arg9Properties, INotifyExpression<T10> arg10, IEnumerable<Type> anchors10, ICollection<string> arg10Properties, INotifyExpression<T11> arg11, IEnumerable<Type> anchors11, ICollection<string> arg11Properties, INotifyExpression<T12> arg12, IEnumerable<Type> anchors12, ICollection<string> arg12Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors8 != null && anchors8.Any())
            {
                anchor8Listener = anchors8.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors9 != null && anchors9.Any())
            {
                anchor9Listener = anchors9.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors10 != null && anchors10.Any())
            {
                anchor10Listener = anchors10.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors11 != null && anchors11.Any())
            {
                anchor11Listener = anchors11.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors12 != null && anchors12.Any())
            {
                anchor12Listener = anchors12.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
            Arg8Properties = arg8Properties;
            Arg9Properties = arg9Properties;
            Arg10Properties = arg10Properties;
            Arg11Properties = arg11Properties;
            Arg12Properties = arg12Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.Element = Argument8.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.Element = Argument9.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.Element = Argument10.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.Element = Argument11.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor12Listener != null)
            {
                foreach (var listener in anchor12Listener)
                {
                    listener.Element = Argument12.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor12Listener != null)
            {
                foreach (var listener in anchor12Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties, 
                Argument8.ApplyParameters(parameters), anchor8Listener?.Select(l => l.Type), Arg8Properties, 
                Argument9.ApplyParameters(parameters), anchor9Listener?.Select(l => l.Type), Arg9Properties, 
                Argument10.ApplyParameters(parameters), anchor10Listener?.Select(l => l.Type), Arg10Properties, 
                Argument11.ApplyParameters(parameters), anchor11Listener?.Select(l => l.Type), Arg11Properties, 
                Argument12.ApplyParameters(parameters), anchor12Listener?.Select(l => l.Type), Arg12Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
        where T8 : IModelElement
        where T9 : IModelElement
        where T10 : IModelElement
        where T11 : IModelElement
        where T12 : IModelElement
        where T13 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;
        private List<BubbledChangeListener> anchor8Listener;
        private List<BubbledChangeListener> anchor9Listener;
        private List<BubbledChangeListener> anchor10Listener;
        private List<BubbledChangeListener> anchor11Listener;
        private List<BubbledChangeListener> anchor12Listener;
        private List<BubbledChangeListener> anchor13Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }
        public ICollection<string> Arg8Properties { get; set; }
        public ICollection<string> Arg9Properties { get; set; }
        public ICollection<string> Arg10Properties { get; set; }
        public ICollection<string> Arg11Properties { get; set; }
        public ICollection<string> Arg12Properties { get; set; }
        public ICollection<string> Arg13Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties, INotifyExpression<T8> arg8, IEnumerable<Type> anchors8, ICollection<string> arg8Properties, INotifyExpression<T9> arg9, IEnumerable<Type> anchors9, ICollection<string> arg9Properties, INotifyExpression<T10> arg10, IEnumerable<Type> anchors10, ICollection<string> arg10Properties, INotifyExpression<T11> arg11, IEnumerable<Type> anchors11, ICollection<string> arg11Properties, INotifyExpression<T12> arg12, IEnumerable<Type> anchors12, ICollection<string> arg12Properties, INotifyExpression<T13> arg13, IEnumerable<Type> anchors13, ICollection<string> arg13Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors8 != null && anchors8.Any())
            {
                anchor8Listener = anchors8.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors9 != null && anchors9.Any())
            {
                anchor9Listener = anchors9.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors10 != null && anchors10.Any())
            {
                anchor10Listener = anchors10.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors11 != null && anchors11.Any())
            {
                anchor11Listener = anchors11.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors12 != null && anchors12.Any())
            {
                anchor12Listener = anchors12.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors13 != null && anchors13.Any())
            {
                anchor13Listener = anchors13.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
            Arg8Properties = arg8Properties;
            Arg9Properties = arg9Properties;
            Arg10Properties = arg10Properties;
            Arg11Properties = arg11Properties;
            Arg12Properties = arg12Properties;
            Arg13Properties = arg13Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.Element = Argument8.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.Element = Argument9.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.Element = Argument10.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.Element = Argument11.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor12Listener != null)
            {
                foreach (var listener in anchor12Listener)
                {
                    listener.Element = Argument12.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor13Listener != null)
            {
                foreach (var listener in anchor13Listener)
                {
                    listener.Element = Argument13.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor12Listener != null)
            {
                foreach (var listener in anchor12Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor13Listener != null)
            {
                foreach (var listener in anchor13Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties, 
                Argument8.ApplyParameters(parameters), anchor8Listener?.Select(l => l.Type), Arg8Properties, 
                Argument9.ApplyParameters(parameters), anchor9Listener?.Select(l => l.Type), Arg9Properties, 
                Argument10.ApplyParameters(parameters), anchor10Listener?.Select(l => l.Type), Arg10Properties, 
                Argument11.ApplyParameters(parameters), anchor11Listener?.Select(l => l.Type), Arg11Properties, 
                Argument12.ApplyParameters(parameters), anchor12Listener?.Select(l => l.Type), Arg12Properties, 
                Argument13.ApplyParameters(parameters), anchor13Listener?.Select(l => l.Type), Arg13Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
        where T8 : IModelElement
        where T9 : IModelElement
        where T10 : IModelElement
        where T11 : IModelElement
        where T12 : IModelElement
        where T13 : IModelElement
        where T14 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;
        private List<BubbledChangeListener> anchor8Listener;
        private List<BubbledChangeListener> anchor9Listener;
        private List<BubbledChangeListener> anchor10Listener;
        private List<BubbledChangeListener> anchor11Listener;
        private List<BubbledChangeListener> anchor12Listener;
        private List<BubbledChangeListener> anchor13Listener;
        private List<BubbledChangeListener> anchor14Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }
        public ICollection<string> Arg8Properties { get; set; }
        public ICollection<string> Arg9Properties { get; set; }
        public ICollection<string> Arg10Properties { get; set; }
        public ICollection<string> Arg11Properties { get; set; }
        public ICollection<string> Arg12Properties { get; set; }
        public ICollection<string> Arg13Properties { get; set; }
        public ICollection<string> Arg14Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties, INotifyExpression<T8> arg8, IEnumerable<Type> anchors8, ICollection<string> arg8Properties, INotifyExpression<T9> arg9, IEnumerable<Type> anchors9, ICollection<string> arg9Properties, INotifyExpression<T10> arg10, IEnumerable<Type> anchors10, ICollection<string> arg10Properties, INotifyExpression<T11> arg11, IEnumerable<Type> anchors11, ICollection<string> arg11Properties, INotifyExpression<T12> arg12, IEnumerable<Type> anchors12, ICollection<string> arg12Properties, INotifyExpression<T13> arg13, IEnumerable<Type> anchors13, ICollection<string> arg13Properties, INotifyExpression<T14> arg14, IEnumerable<Type> anchors14, ICollection<string> arg14Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors8 != null && anchors8.Any())
            {
                anchor8Listener = anchors8.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors9 != null && anchors9.Any())
            {
                anchor9Listener = anchors9.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors10 != null && anchors10.Any())
            {
                anchor10Listener = anchors10.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors11 != null && anchors11.Any())
            {
                anchor11Listener = anchors11.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors12 != null && anchors12.Any())
            {
                anchor12Listener = anchors12.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors13 != null && anchors13.Any())
            {
                anchor13Listener = anchors13.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors14 != null && anchors14.Any())
            {
                anchor14Listener = anchors14.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
            Arg8Properties = arg8Properties;
            Arg9Properties = arg9Properties;
            Arg10Properties = arg10Properties;
            Arg11Properties = arg11Properties;
            Arg12Properties = arg12Properties;
            Arg13Properties = arg13Properties;
            Arg14Properties = arg14Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.Element = Argument8.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.Element = Argument9.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.Element = Argument10.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.Element = Argument11.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor12Listener != null)
            {
                foreach (var listener in anchor12Listener)
                {
                    listener.Element = Argument12.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor13Listener != null)
            {
                foreach (var listener in anchor13Listener)
                {
                    listener.Element = Argument13.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor14Listener != null)
            {
                foreach (var listener in anchor14Listener)
                {
                    listener.Element = Argument14.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor12Listener != null)
            {
                foreach (var listener in anchor12Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor13Listener != null)
            {
                foreach (var listener in anchor13Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor14Listener != null)
            {
                foreach (var listener in anchor14Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties, 
                Argument8.ApplyParameters(parameters), anchor8Listener?.Select(l => l.Type), Arg8Properties, 
                Argument9.ApplyParameters(parameters), anchor9Listener?.Select(l => l.Type), Arg9Properties, 
                Argument10.ApplyParameters(parameters), anchor10Listener?.Select(l => l.Type), Arg10Properties, 
                Argument11.ApplyParameters(parameters), anchor11Listener?.Select(l => l.Type), Arg11Properties, 
                Argument12.ApplyParameters(parameters), anchor12Listener?.Select(l => l.Type), Arg12Properties, 
                Argument13.ApplyParameters(parameters), anchor13Listener?.Select(l => l.Type), Arg13Properties, 
                Argument14.ApplyParameters(parameters), anchor14Listener?.Select(l => l.Type), Arg14Properties);
        }
    }
    class ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : ObservableStaticMethodCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
        where T1 : IModelElement
        where T2 : IModelElement
        where T3 : IModelElement
        where T4 : IModelElement
        where T5 : IModelElement
        where T6 : IModelElement
        where T7 : IModelElement
        where T8 : IModelElement
        where T9 : IModelElement
        where T10 : IModelElement
        where T11 : IModelElement
        where T12 : IModelElement
        where T13 : IModelElement
        where T14 : IModelElement
        where T15 : IModelElement
    {
        private List<BubbledChangeListener> anchor1Listener;
        private List<BubbledChangeListener> anchor2Listener;
        private List<BubbledChangeListener> anchor3Listener;
        private List<BubbledChangeListener> anchor4Listener;
        private List<BubbledChangeListener> anchor5Listener;
        private List<BubbledChangeListener> anchor6Listener;
        private List<BubbledChangeListener> anchor7Listener;
        private List<BubbledChangeListener> anchor8Listener;
        private List<BubbledChangeListener> anchor9Listener;
        private List<BubbledChangeListener> anchor10Listener;
        private List<BubbledChangeListener> anchor11Listener;
        private List<BubbledChangeListener> anchor12Listener;
        private List<BubbledChangeListener> anchor13Listener;
        private List<BubbledChangeListener> anchor14Listener;
        private List<BubbledChangeListener> anchor15Listener;

        public ICollection<string> Arg1Properties { get; set; }
        public ICollection<string> Arg2Properties { get; set; }
        public ICollection<string> Arg3Properties { get; set; }
        public ICollection<string> Arg4Properties { get; set; }
        public ICollection<string> Arg5Properties { get; set; }
        public ICollection<string> Arg6Properties { get; set; }
        public ICollection<string> Arg7Properties { get; set; }
        public ICollection<string> Arg8Properties { get; set; }
        public ICollection<string> Arg9Properties { get; set; }
        public ICollection<string> Arg10Properties { get; set; }
        public ICollection<string> Arg11Properties { get; set; }
        public ICollection<string> Arg12Properties { get; set; }
        public ICollection<string> Arg13Properties { get; set; }
        public ICollection<string> Arg14Properties { get; set; }
        public ICollection<string> Arg15Properties { get; set; }

        public ObservableTreeExtensionCall(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, INotifyExpression<T1> arg1, IEnumerable<Type> anchors1, ICollection<string> arg1Properties, INotifyExpression<T2> arg2, IEnumerable<Type> anchors2, ICollection<string> arg2Properties, INotifyExpression<T3> arg3, IEnumerable<Type> anchors3, ICollection<string> arg3Properties, INotifyExpression<T4> arg4, IEnumerable<Type> anchors4, ICollection<string> arg4Properties, INotifyExpression<T5> arg5, IEnumerable<Type> anchors5, ICollection<string> arg5Properties, INotifyExpression<T6> arg6, IEnumerable<Type> anchors6, ICollection<string> arg6Properties, INotifyExpression<T7> arg7, IEnumerable<Type> anchors7, ICollection<string> arg7Properties, INotifyExpression<T8> arg8, IEnumerable<Type> anchors8, ICollection<string> arg8Properties, INotifyExpression<T9> arg9, IEnumerable<Type> anchors9, ICollection<string> arg9Properties, INotifyExpression<T10> arg10, IEnumerable<Type> anchors10, ICollection<string> arg10Properties, INotifyExpression<T11> arg11, IEnumerable<Type> anchors11, ICollection<string> arg11Properties, INotifyExpression<T12> arg12, IEnumerable<Type> anchors12, ICollection<string> arg12Properties, INotifyExpression<T13> arg13, IEnumerable<Type> anchors13, ICollection<string> arg13Properties, INotifyExpression<T14> arg14, IEnumerable<Type> anchors14, ICollection<string> arg14Properties, INotifyExpression<T15> arg15, IEnumerable<Type> anchors15, ICollection<string> arg15Properties) : base(function, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15)
        {
            if (anchors1 != null && anchors1.Any())
            {
                anchor1Listener = anchors1.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors2 != null && anchors2.Any())
            {
                anchor2Listener = anchors2.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors3 != null && anchors3.Any())
            {
                anchor3Listener = anchors3.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors4 != null && anchors4.Any())
            {
                anchor4Listener = anchors4.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors5 != null && anchors5.Any())
            {
                anchor5Listener = anchors5.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors6 != null && anchors6.Any())
            {
                anchor6Listener = anchors6.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors7 != null && anchors7.Any())
            {
                anchor7Listener = anchors7.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors8 != null && anchors8.Any())
            {
                anchor8Listener = anchors8.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors9 != null && anchors9.Any())
            {
                anchor9Listener = anchors9.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors10 != null && anchors10.Any())
            {
                anchor10Listener = anchors10.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors11 != null && anchors11.Any())
            {
                anchor11Listener = anchors11.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors12 != null && anchors12.Any())
            {
                anchor12Listener = anchors12.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors13 != null && anchors13.Any())
            {
                anchor13Listener = anchors13.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors14 != null && anchors14.Any())
            {
                anchor14Listener = anchors14.Select(t => new BubbledChangeListener(null, t)).ToList();
            }
            if (anchors15 != null && anchors15.Any())
            {
                anchor15Listener = anchors15.Select(t => new BubbledChangeListener(null, t)).ToList();
            }

            Arg1Properties = arg1Properties;
            Arg2Properties = arg2Properties;
            Arg3Properties = arg3Properties;
            Arg4Properties = arg4Properties;
            Arg5Properties = arg5Properties;
            Arg6Properties = arg6Properties;
            Arg7Properties = arg7Properties;
            Arg8Properties = arg8Properties;
            Arg9Properties = arg9Properties;
            Arg10Properties = arg10Properties;
            Arg11Properties = arg11Properties;
            Arg12Properties = arg12Properties;
            Arg13Properties = arg13Properties;
            Arg14Properties = arg14Properties;
            Arg15Properties = arg15Properties;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.Element = Argument1.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.Element = Argument2.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.Element = Argument3.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.Element = Argument4.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.Element = Argument5.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.Element = Argument6.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.Element = Argument7.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.Element = Argument8.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.Element = Argument9.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.Element = Argument10.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.Element = Argument11.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor12Listener != null)
            {
                foreach (var listener in anchor12Listener)
                {
                    listener.Element = Argument12.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor13Listener != null)
            {
                foreach (var listener in anchor13Listener)
                {
                    listener.Element = Argument13.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor14Listener != null)
            {
                foreach (var listener in anchor14Listener)
                {
                    listener.Element = Argument14.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }
            if (anchor15Listener != null)
            {
                foreach (var listener in anchor15Listener)
                {
                    listener.Element = Argument15.Value;
                    listener.BubbledChange += handler;
                    listener.Attach();
                }
            }

        protected override void OnDetach()
        {
            EventHandler<BubbledChangeEventArgs> handler = Listener_BubbledChange;
            if (anchor1Listener != null)
            {
                foreach (var listener in anchor1Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor2Listener != null)
            {
                foreach (var listener in anchor2Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor3Listener != null)
            {
                foreach (var listener in anchor3Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor4Listener != null)
            {
                foreach (var listener in anchor4Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor5Listener != null)
            {
                foreach (var listener in anchor5Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor6Listener != null)
            {
                foreach (var listener in anchor6Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor7Listener != null)
            {
                foreach (var listener in anchor7Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor8Listener != null)
            {
                foreach (var listener in anchor8Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor9Listener != null)
            {
                foreach (var listener in anchor9Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor10Listener != null)
            {
                foreach (var listener in anchor10Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor11Listener != null)
            {
                foreach (var listener in anchor11Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor12Listener != null)
            {
                foreach (var listener in anchor12Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor13Listener != null)
            {
                foreach (var listener in anchor13Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor14Listener != null)
            {
                foreach (var listener in anchor14Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            if (anchor15Listener != null)
            {
                foreach (var listener in anchor15Listener)
                {
                    listener.BubbledChange -= handler;
                    listener.Detach();
                }
            }
            base.OnDetach();
        }

        private void Listener_BubbledChange(object sender, BubbledChangeEventArgs e)
        {
            if (Successors.IsAttached)
            {
                ExecutionEngine.Current.ManualInvalidation(this);
            }
        }

        public override INotifyExpression<TResult> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableTreeExtensionCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Function, 
                Argument1.ApplyParameters(parameters), anchor1Listener?.Select(l => l.Type), Arg1Properties, 
                Argument2.ApplyParameters(parameters), anchor2Listener?.Select(l => l.Type), Arg2Properties, 
                Argument3.ApplyParameters(parameters), anchor3Listener?.Select(l => l.Type), Arg3Properties, 
                Argument4.ApplyParameters(parameters), anchor4Listener?.Select(l => l.Type), Arg4Properties, 
                Argument5.ApplyParameters(parameters), anchor5Listener?.Select(l => l.Type), Arg5Properties, 
                Argument6.ApplyParameters(parameters), anchor6Listener?.Select(l => l.Type), Arg6Properties, 
                Argument7.ApplyParameters(parameters), anchor7Listener?.Select(l => l.Type), Arg7Properties, 
                Argument8.ApplyParameters(parameters), anchor8Listener?.Select(l => l.Type), Arg8Properties, 
                Argument9.ApplyParameters(parameters), anchor9Listener?.Select(l => l.Type), Arg9Properties, 
                Argument10.ApplyParameters(parameters), anchor10Listener?.Select(l => l.Type), Arg10Properties, 
                Argument11.ApplyParameters(parameters), anchor11Listener?.Select(l => l.Type), Arg11Properties, 
                Argument12.ApplyParameters(parameters), anchor12Listener?.Select(l => l.Type), Arg12Properties, 
                Argument13.ApplyParameters(parameters), anchor13Listener?.Select(l => l.Type), Arg13Properties, 
                Argument14.ApplyParameters(parameters), anchor14Listener?.Select(l => l.Type), Arg14Properties, 
                Argument15.ApplyParameters(parameters), anchor15Listener?.Select(l => l.Type), Arg15Properties);
        }
    }
    internal static class ObservableTreeExtensionCallTypes
    {
        public static readonly Type[] Types = { typeof(ObservableTreeExtensionCall<,>), typeof(ObservableTreeExtensionCall<,,>), typeof(ObservableTreeExtensionCall<,,,>), typeof(ObservableTreeExtensionCall<,,,,>), typeof(ObservableTreeExtensionCall<,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,,,,,,,,>), typeof(ObservableTreeExtensionCall<,,,,,,,,,,,,,,,>) };
    }
}
