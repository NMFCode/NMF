using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution.Minimizing
{
    public class MultiplePropertyChanges : IMinimizingStrategy
    {
        private static readonly Type genericPropertyChangeType = typeof(PropertyChange<>);

        private Dictionary<string, int> propertyChangeIndices = new Dictionary<string, int>();
        
        public List<IModelChange> Execute(List<IModelChange> changes)
        {
            var result = new List<IModelChange>(changes.Count);
            for (int i = 0; i < changes.Count; i++)
            {
                if (changes[i].GetType().Name.StartsWith("PropertyChange"))
                    HandlePropertyChange(result, changes[i], i);
                else
                    result.Add(changes[i]);
            }
            return result;
        }

        private void HandlePropertyChange(List<IModelChange> result, IModelChange propertyChange, int index)
        {
            string key = PropertyChangeToKey(propertyChange);
            int existingIndex;
            if (propertyChangeIndices.TryGetValue(key, out existingIndex))
            {
                var existingChange = result[existingIndex];
                var newValueProperty = existingChange.GetType().GetProperty("NewValue");
                newValueProperty.SetValue(existingChange, newValueProperty.GetValue(propertyChange, null), null);
            }
            else
            {
                propertyChangeIndices.Add(key, index);
                result.Add(propertyChange);
            }
        }

        private string PropertyChangeToKey(IModelChange propertyChange)
        {
            var type = propertyChange.GetType();
            var absoluteUri = (Uri)type.GetProperty("AbsoluteUri").GetValue(propertyChange, null);
            var propertyName = (string)type.GetProperty("PropertyName").GetValue(propertyChange, null);
            return absoluteUri + "." + propertyName;
        }
    }
}
