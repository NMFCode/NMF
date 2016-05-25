using System;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Message
{
    [Serializable]
    public class ModelExecuteActionMessage<T> : IStreamMessage
    {
        private readonly Action<T> _action;
        private readonly Action<T, object> _actionWithState;
        private readonly object _state;

        public ModelExecuteActionMessage(Action<T> action)
        {
            _action = action;
        }

        public ModelExecuteActionMessage(Action<T, object> action, object state)
        {
            _actionWithState = action;
            _state = state;
        }

        public void Execute(T model)
        {
            if (_actionWithState != null)
                _actionWithState(model, _state);
            else
                _action(model);
        }
    }
}