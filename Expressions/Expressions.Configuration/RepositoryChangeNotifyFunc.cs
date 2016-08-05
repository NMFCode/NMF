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

        public RepositoryAffectedNotifyFunc(IModelRepository repository, Expression body, IEnumerable<ParameterExpression> parameters)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (body == null) throw new ArgumentNullException("body");
            if (parameters == null) throw new ArgumentNullException("parameters");

            Repository = repository;
            Body = body;
            Parameters = new List<ParameterExpression>(parameters);
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged
        {
            add { }
            remove { }
        }

        public virtual INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");

            var applyParameters = new ApplyParametersVisitor(parameters);
            var newBody = applyParameters.Visit(Body);

            if (parameters.Count == Parameters.Count)
            {
                var lambda = Expression.Lambda<Func<T>>(newBody);
                return new RepositoryAffectedNotifyValue<T>(Repository, lambda.Compile());
            }
            else
            {
                return new RepositoryAffectedNotifyFunc<T>(Repository, newBody, Parameters.Where(p => !parameters.ContainsKey(p.Name)));
            }
        }

        public INotifyExpression<T> Reduce()
        {
            return this;
        }

        public void Detach()
        {
            throw new InvalidOperationException();
        }

        public void Attach()
        {
            throw new InvalidOperationException();
        }

        public void Refresh()
        {
        }
    }

    internal class RepositoryAffectedReversableNotifyFunc<T> : RepositoryAffectedNotifyFunc<T>, INotifyReversableExpression<T>
    {
        public RepositoryAffectedReversableNotifyFunc(IModelRepository repository, Expression body, IEnumerable<ParameterExpression> parameters)
            : base(repository, body, parameters)
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

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");

            var applyParameters = new ApplyParametersVisitor(parameters);
            var newBody = applyParameters.Visit(Body);

            if (parameters.Count == Parameters.Count)
            {
                var lambda = Expression.Lambda<Func<T>>(newBody);
                var setter = SetExpressionRewriter.CreateSetter(lambda);
                if (setter == null) throw new InvalidOperationException(string.Format("The lambda expression {0} could not be reversed.", newBody));
                return new RepositoryAffectedReversableNotifyValue<T>(Repository, lambda.Compile(), setter.Compile());
            }
            else
            {
                return new RepositoryAffectedReversableNotifyFunc<T>(Repository, newBody, Parameters.Where(p => !parameters.ContainsKey(p.Name)));
            }
        }
    }
}
