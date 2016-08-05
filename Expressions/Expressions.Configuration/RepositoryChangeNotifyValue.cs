using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Expressions
{
    internal class RepositoryAffectedNotifyValue<T> : NotifyExpression<T>
    {
        public IModelRepository Repository { get; private set; }
        public Func<T> Getter { get; private set; }

        public RepositoryAffectedNotifyValue(IModelRepository repository, Func<T> getter)
        {
            Repository = repository;
            Getter = getter;
        }

        public override bool IsParameterFree
        {
            get
            {
                return true;
            }
        }

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return this;
        }

        protected override void AttachCore()
        {
            Repository.BubbledChange += Repository_BubbledChange;
        }

        private void Repository_BubbledChange(object sender, Models.BubbledChangeEventArgs e)
        {
            Refresh();
        }

        protected override void DetachCore()
        {
            Repository.BubbledChange -= Repository_BubbledChange;
        }

        protected override T GetValue()
        {
            return Getter();
        }
    }

    internal class RepositoryAffectedReversableNotifyValue<T> : RepositoryAffectedNotifyValue<T>, INotifyReversableExpression<T>
    {
        public Action<T> Setter { get; private set; }

        public RepositoryAffectedReversableNotifyValue(IModelRepository repository, Func<T> getter, Action<T> setter)
            : base(repository, getter)
        {
            if (setter == null) throw new ArgumentNullException("setter");

            Setter = setter;
        }

        public bool IsReversable
        {
            get
            {
                return true;
            }
        }

        T INotifyReversableValue<T>.Value
        {
            get
            {
                return Value;
            }

            set
            {
                Setter(value);
            }
        }
    }
}
