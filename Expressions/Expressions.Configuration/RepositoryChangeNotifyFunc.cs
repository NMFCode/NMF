using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Expressions
{
    internal class RepositoryAffectedNotifyFunc<T> : INotifyExpression<T>
    {
        public Expression Body { get; private set; }
        public List<ParameterExpression> Parameters { get; private set; }
        public IModelRepository Repository { get; private set; }

        public List<IChangeInfo> ChangeInfos { get; private set; }

        public IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public ISuccessorList Successors { get { throw new InvalidOperationException(); } }

        public T Value
        {
            get
            {
                return default(T);
            }
        }

        public bool IsAttached
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public bool CanBeConstant
        {
            get
            {
                return false;
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
                return false;
            }
        }

        public object ValueObject
        {
            get
            {
                return Value;
            }
        }

        public ExecutionMetaData ExecutionMetaData
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public RepositoryAffectedNotifyFunc(IModelRepository repository, Expression body, IEnumerable<ParameterExpression> parameters) : this(repository, body, parameters, null) { }

        protected RepositoryAffectedNotifyFunc(IModelRepository repository, Expression body, IEnumerable<ParameterExpression> parameters, List<IChangeInfo> changeInfos)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (body == null) throw new ArgumentNullException("body");
            if (parameters == null) throw new ArgumentNullException("parameters");

            Repository = repository;
            Body = body;
            Parameters = new List<ParameterExpression>(parameters);
            ChangeInfos = changeInfos;
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public virtual INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");

            var applyParameters = new RepositoryChangeApplyParametersVisitor(parameters);
            var newBody = applyParameters.Visit(Body);

            var changeInfos = ChangeInfos;
            if (applyParameters.Recorders != null)
            {
                if (changeInfos == null)
                {
                    changeInfos = applyParameters.Recorders;
                }
                else
                {
                    changeInfos = new List<IChangeInfo>(changeInfos);
                    changeInfos.AddRange(applyParameters.Recorders);
                }
            }

            if (parameters.Count == Parameters.Count)
            {
                var lambda = Expression.Lambda<Func<T>>(newBody);
                if (changeInfos == null)
                {
                    return new RepositoryAffectedNotifyValue<T>(Repository, lambda.Compile());
                }
                else
                {
                    return new RepositoryAffectedDependentNotifyValue<T>(Repository, lambda.Compile(), changeInfos);
                }
            }
            else
            {
                return new RepositoryAffectedNotifyFunc<T>(Repository, newBody, Parameters.Where(p => !parameters.ContainsKey(p.Name)), changeInfos);
            }
        }

        INotifyExpression INotifyExpression.ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return ApplyParameters(parameters, trace);
        }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            throw new InvalidOperationException();
        }

        public void Dispose()
        {
        }
    }

    internal class RepositoryAffectedReversableNotifyFunc<T> : RepositoryAffectedNotifyFunc<T>, INotifyReversableExpression<T>
    {
        public RepositoryAffectedReversableNotifyFunc(IModelRepository repository, Expression body, IEnumerable<ParameterExpression> parameters)
            : base(repository, body, parameters)
        { }
        private RepositoryAffectedReversableNotifyFunc(IModelRepository repository, Expression body, IEnumerable<ParameterExpression> parameters, List<IChangeInfo> changeInfos)
            : base(repository, body, parameters, changeInfos)
        { }

        public bool IsReversable
        {
            get
            {
                return false;
            }
        }

        T INotifyReversableValue<T>.Value
        {
            get
            {
                throw new InvalidOperationException();
            }

            set
            {
                throw new InvalidOperationException();
            }
        }

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");

            var applyParameters = new RepositoryChangeApplyParametersVisitor(parameters);
            var newBody = applyParameters.Visit(Body);

            var changeInfos = ChangeInfos;
            if (applyParameters.Recorders != null)
            {
                if (changeInfos == null)
                {
                    changeInfos = applyParameters.Recorders;
                }
                else
                {
                    changeInfos = new List<IChangeInfo>(changeInfos);
                    changeInfos.AddRange(applyParameters.Recorders);
                }
            }

            if (parameters.Count == Parameters.Count)
            {
                var lambda = Expression.Lambda<Func<T>>(newBody);
                var setter = SetExpressionRewriter.CreateSetter(lambda);
                if (setter == null) throw new InvalidOperationException(string.Format("The lambda expression {0} could not be reversed.", newBody));
                if (changeInfos == null)
                {
                    return new RepositoryAffectedReversableNotifyValue<T>(Repository, lambda.Compile(), setter.Compile());
                }
                else
                {
                    return new RepositoryAffectedDependentReversableNotifyValue<T>(Repository, lambda.Compile(), setter.Compile(), changeInfos);
                }
            }
            else
            {
                return new RepositoryAffectedReversableNotifyFunc<T>(Repository, newBody, Parameters.Where(p => !parameters.ContainsKey(p.Name)), changeInfos);
            }
        }
    }
}
