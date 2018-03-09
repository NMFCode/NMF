using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions
{
    internal class ModelFuncExtractionParameter<TBase1, T> : INotifyExpression<T>
    {
        

        public INotifyExpression<TBase1> Base1 { get; private set; }
        public Func<TBase1, T> MemberGetter { get; private set; }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Base1;
            }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public ModelFuncExtractionParameter(INotifyExpression<TBase1> base1, Func<TBase1, T> memberGetter)
        {
            Base1 = base1;
            MemberGetter = memberGetter;
        }

        public bool CanBeConstant
        {
            get
            {
                return Base1.CanBeConstant;
            }
        }

        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return Base1.IsParameterFree;
            }
        }

        public T Value
        {
            get
            {
                return MemberGetter(Base1.Value);
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        public void Attach() { }

        public void Detach() { }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose() { }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }
    }
    internal class ModelFuncExtractionParameter<TBase1, TBase2, T> : INotifyExpression<T>
    {
        

        public INotifyExpression<TBase1> Base1 { get; private set; }
        public INotifyExpression<TBase2> Base2 { get; private set; }
        public Func<TBase1, TBase2, T> MemberGetter { get; private set; }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Base1;
                yield return Base2;
            }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public ModelFuncExtractionParameter(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, Func<TBase1, TBase2, T> memberGetter)
        {
            Base1 = base1;
            Base2 = base2;
            MemberGetter = memberGetter;
        }

        public bool CanBeConstant
        {
            get
            {
                return Base1.CanBeConstant;
            }
        }

        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return Base1.IsParameterFree;
            }
        }

        public T Value
        {
            get
            {
                return MemberGetter(Base1.Value, Base2.Value);
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        public void Attach() { }

        public void Detach() { }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose() { }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }
    }
    internal class ModelFuncExtractionParameter<TBase1, TBase2, TBase3, T> : INotifyExpression<T>
    {
        

        public INotifyExpression<TBase1> Base1 { get; private set; }
        public INotifyExpression<TBase2> Base2 { get; private set; }
        public INotifyExpression<TBase3> Base3 { get; private set; }
        public Func<TBase1, TBase2, TBase3, T> MemberGetter { get; private set; }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Base1;
                yield return Base2;
                yield return Base3;
            }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public ModelFuncExtractionParameter(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, Func<TBase1, TBase2, TBase3, T> memberGetter)
        {
            Base1 = base1;
            Base2 = base2;
            Base3 = base3;
            MemberGetter = memberGetter;
        }

        public bool CanBeConstant
        {
            get
            {
                return Base1.CanBeConstant;
            }
        }

        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return Base1.IsParameterFree;
            }
        }

        public T Value
        {
            get
            {
                return MemberGetter(Base1.Value, Base2.Value, Base3.Value);
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        public void Attach() { }

        public void Detach() { }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose() { }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }
    }
    internal class ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, T> : INotifyExpression<T>
    {
        

        public INotifyExpression<TBase1> Base1 { get; private set; }
        public INotifyExpression<TBase2> Base2 { get; private set; }
        public INotifyExpression<TBase3> Base3 { get; private set; }
        public INotifyExpression<TBase4> Base4 { get; private set; }
        public Func<TBase1, TBase2, TBase3, TBase4, T> MemberGetter { get; private set; }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Base1;
                yield return Base2;
                yield return Base3;
                yield return Base4;
            }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public ModelFuncExtractionParameter(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, Func<TBase1, TBase2, TBase3, TBase4, T> memberGetter)
        {
            Base1 = base1;
            Base2 = base2;
            Base3 = base3;
            Base4 = base4;
            MemberGetter = memberGetter;
        }

        public bool CanBeConstant
        {
            get
            {
                return Base1.CanBeConstant;
            }
        }

        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return Base1.IsParameterFree;
            }
        }

        public T Value
        {
            get
            {
                return MemberGetter(Base1.Value, Base2.Value, Base3.Value, Base4.Value);
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        public void Attach() { }

        public void Detach() { }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose() { }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }
    }
    internal class ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, T> : INotifyExpression<T>
    {
        

        public INotifyExpression<TBase1> Base1 { get; private set; }
        public INotifyExpression<TBase2> Base2 { get; private set; }
        public INotifyExpression<TBase3> Base3 { get; private set; }
        public INotifyExpression<TBase4> Base4 { get; private set; }
        public INotifyExpression<TBase5> Base5 { get; private set; }
        public Func<TBase1, TBase2, TBase3, TBase4, TBase5, T> MemberGetter { get; private set; }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Base1;
                yield return Base2;
                yield return Base3;
                yield return Base4;
                yield return Base5;
            }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public ModelFuncExtractionParameter(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, Func<TBase1, TBase2, TBase3, TBase4, TBase5, T> memberGetter)
        {
            Base1 = base1;
            Base2 = base2;
            Base3 = base3;
            Base4 = base4;
            Base5 = base5;
            MemberGetter = memberGetter;
        }

        public bool CanBeConstant
        {
            get
            {
                return Base1.CanBeConstant;
            }
        }

        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return Base1.IsParameterFree;
            }
        }

        public T Value
        {
            get
            {
                return MemberGetter(Base1.Value, Base2.Value, Base3.Value, Base4.Value, Base5.Value);
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        public void Attach() { }

        public void Detach() { }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose() { }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }
    }
    internal class ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, T> : INotifyExpression<T>
    {
        

        public INotifyExpression<TBase1> Base1 { get; private set; }
        public INotifyExpression<TBase2> Base2 { get; private set; }
        public INotifyExpression<TBase3> Base3 { get; private set; }
        public INotifyExpression<TBase4> Base4 { get; private set; }
        public INotifyExpression<TBase5> Base5 { get; private set; }
        public INotifyExpression<TBase6> Base6 { get; private set; }
        public Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, T> MemberGetter { get; private set; }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Base1;
                yield return Base2;
                yield return Base3;
                yield return Base4;
                yield return Base5;
                yield return Base6;
            }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public ModelFuncExtractionParameter(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, T> memberGetter)
        {
            Base1 = base1;
            Base2 = base2;
            Base3 = base3;
            Base4 = base4;
            Base5 = base5;
            Base6 = base6;
            MemberGetter = memberGetter;
        }

        public bool CanBeConstant
        {
            get
            {
                return Base1.CanBeConstant;
            }
        }

        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return Base1.IsParameterFree;
            }
        }

        public T Value
        {
            get
            {
                return MemberGetter(Base1.Value, Base2.Value, Base3.Value, Base4.Value, Base5.Value, Base6.Value);
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        public void Attach() { }

        public void Detach() { }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose() { }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }
    }
    internal class ModelFuncExtractionParameter<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, T> : INotifyExpression<T>
    {
        

        public INotifyExpression<TBase1> Base1 { get; private set; }
        public INotifyExpression<TBase2> Base2 { get; private set; }
        public INotifyExpression<TBase3> Base3 { get; private set; }
        public INotifyExpression<TBase4> Base4 { get; private set; }
        public INotifyExpression<TBase5> Base5 { get; private set; }
        public INotifyExpression<TBase6> Base6 { get; private set; }
        public INotifyExpression<TBase7> Base7 { get; private set; }
        public Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, T> MemberGetter { get; private set; }

        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Base1;
                yield return Base2;
                yield return Base3;
                yield return Base4;
                yield return Base5;
                yield return Base6;
                yield return Base7;
            }
        }

        public ISuccessorList Successors { get; } = NotifySystem.DefaultSystem.CreateSuccessorList();

        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        public ModelFuncExtractionParameter(INotifyExpression<TBase1> base1, INotifyExpression<TBase2> base2, INotifyExpression<TBase3> base3, INotifyExpression<TBase4> base4, INotifyExpression<TBase5> base5, INotifyExpression<TBase6> base6, INotifyExpression<TBase7> base7, Func<TBase1, TBase2, TBase3, TBase4, TBase5, TBase6, TBase7, T> memberGetter)
        {
            Base1 = base1;
            Base2 = base2;
            Base3 = base3;
            Base4 = base4;
            Base5 = base5;
            Base6 = base6;
            Base7 = base7;
            MemberGetter = memberGetter;
        }

        public bool CanBeConstant
        {
            get
            {
                return Base1.CanBeConstant;
            }
        }

        public bool IsConstant
        {
            get
            {
                return false;
            }
        }

        public bool IsParameterFree
        {
            get
            {
                return Base1.IsParameterFree;
            }
        }

        public T Value
        {
            get
            {
                return MemberGetter(Base1.Value, Base2.Value, Base3.Value, Base4.Value, Base5.Value, Base6.Value, Base7.Value);
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        public void Attach() { }

        public void Detach() { }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Dispose() { }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            return new ValueChangedNotificationResult<T>(this, Value, Value);
        }
    }
}
