using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution.Minimizing
{
    public class MultiplePropertyChanges : IMinimizingStrategy
    {
        private Dictionary<string, int> propertyChangeIndices = new Dictionary<string, int>();

        public List<IModelChange> Execute(List<IModelChange> changes)
        {
            var result = new List<IModelChange>(changes.Count);
            for (int i = 0; i < changes.Count; i++)
            {
                var propertyChange = changes[i] as PropertyChange;
                if (propertyChange == null)
                    result.Add(changes[i]);
                else
                    HandlePropertyChange(result, propertyChange, i);
            }
            return result;
        }

        private void HandlePropertyChange(List<IModelChange> result, PropertyChange propertyChange, int index)
        {
            string key = PropertyChangeToKey(propertyChange);
            int existingIndex;
            if (propertyChangeIndices.TryGetValue(key, out existingIndex))
            {
                ((PropertyChange)result[existingIndex]).NewValue = propertyChange.NewValue;
            }
            else
            {
                propertyChangeIndices.Add(key, index);
                result.Add(propertyChange);
            }
        }

        private string PropertyChangeToKey(PropertyChange change)
        {
            return change.AbsoluteUri + "." + change.PropertyName;
        }
    }
}
