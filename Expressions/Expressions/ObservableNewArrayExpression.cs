using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal sealed class ObservableNewArray1Expression<T> : NotifyExpression<T[]>
    {
        public INotifyExpression<int> Bounds1 { get; private set; }

        public ObservableNewArray1Expression(INotifyExpression<int> bounds1)
        {
            if (bounds1 == null) throw new ArgumentNullException("bounds1");

            Bounds1 = bounds1;
        }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.NewArrayBounds; }
        }

        public override bool IsParameterFree
        {
            get { return Bounds1.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get { yield return Bounds1; }
        }

        protected override T[] GetValue()
        {
            return (T[])Activator.CreateInstance(typeof(T[]), Bounds1.Value);
        }
        
        protected override INotifyExpression<T[]> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableNewArray1Expression<T>(Bounds1.ApplyParameters(parameters, trace));
        }
    }
    
    internal sealed class ObservableNewArray2Expression<T> : NotifyExpression<T[,]>
    {
        public INotifyExpression<int> Bounds1 { get; private set; }
        public INotifyExpression<int> Bounds2 { get; private set; }

        public ObservableNewArray2Expression(INotifyExpression<int> bounds1, INotifyExpression<int> bounds2)
        {
            if (bounds1 == null) throw new ArgumentNullException("bounds1");
            if (bounds2 == null) throw new ArgumentNullException("bounds2");

            Bounds1 = bounds1;
            Bounds2 = bounds2;
        }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.NewArrayBounds; }
        }

        public override bool IsParameterFree
        {
            get { return Bounds1.IsParameterFree && Bounds2.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Bounds1;
                yield return Bounds2;
            }
        }

        protected override T[,] GetValue()
        {
            return (T[,])Activator.CreateInstance(typeof(T[,]), Bounds1.Value, Bounds2.Value);
        }

        protected override INotifyExpression<T[,]> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableNewArray2Expression<T>(Bounds1.ApplyParameters(parameters, trace), Bounds2.ApplyParameters(parameters, trace));
        }
    }

    internal sealed class ObservableNewArray3Expression<T> : NotifyExpression<T[,,]>
    {
        public INotifyExpression<int> Bounds1 { get; private set; }
        public INotifyExpression<int> Bounds2 { get; private set; }
        public INotifyExpression<int> Bounds3 { get; private set; }

        public ObservableNewArray3Expression(INotifyExpression<int> bounds1, INotifyExpression<int> bounds2, INotifyExpression<int> bounds3)
        {
            if (bounds1 == null) throw new ArgumentNullException("bounds1");
            if (bounds2 == null) throw new ArgumentNullException("bounds2");
            if (bounds3 == null) throw new ArgumentNullException("bounds3");

            Bounds1 = bounds1;
            Bounds2 = bounds2;
            Bounds3 = bounds3;
        }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.NewArrayBounds; }
        }

        public override bool IsParameterFree
        {
            get { return Bounds1.IsParameterFree && Bounds2.IsParameterFree && Bounds3.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Bounds1;
                yield return Bounds2;
                yield return Bounds3;
            }
        }

        protected override T[,,] GetValue()
        {
            return (T[,,])Activator.CreateInstance(typeof(T[,,]), Bounds1.Value, Bounds2.Value, Bounds3.Value);
        }

        protected override INotifyExpression<T[,,]> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableNewArray3Expression<T>(Bounds1.ApplyParameters(parameters, trace), Bounds2.ApplyParameters(parameters, trace), Bounds3.ApplyParameters(parameters, trace));
        }
    }
}
