using NMF.Models.Evolution.Minimizing;
using NMF.Models.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Evolution
{
    /// <summary>
    /// Represents a root collection of model changes.
    /// </summary>
    public class ModelChangeCollection
    {
        /// <summary>
        /// Gets the list of model changes.
        /// </summary>
        public List<IModelChange> Changes { get; set; }

        public ModelChangeCollection() : this(new List<IModelChange>()) { }

        internal ModelChangeCollection(List<IModelChange> changes)
        {
            Changes = changes;
        }

        /// <summary>
        /// Minimizes the list of model changes by removing redundancies.
        /// </summary>
        public void Minimize()
        {
            Minimize(new MultiplePropertyChanges());
        }

        /// <summary>
        /// Minimizes the list of model changes by removing redundancies
        /// using the specified strategies.
        /// </summary>
        /// <param name="strategies"></param>
        public void Minimize(params IMinimizingStrategy[] strategies)
        {
            var localList = Changes.ToList();
            foreach (var strat in strategies)
                localList = strat.Execute(localList);

            Changes = localList;
        }

        public Task MinimizeAsync()
        {
            return Task.Factory.StartNew(Minimize);
        }

        public Task MinimizeAsync(params IMinimizingStrategy[] strategies)
        {
            return Task.Factory.StartNew(() => Minimize(strategies));
        }

        /// <summary>
        /// Applies all changes in this collection to the given repository.
        /// </summary>
        /// <param name="repository"></param>
        public void Apply(IModelRepository repository)
        {
            foreach (var change in Changes)
                change.Apply(repository);
        }
    }
}
