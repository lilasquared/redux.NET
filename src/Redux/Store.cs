using System;

namespace Redux
{
    public interface IStoreBase<out TState> {
        Object Dispatch(Object action);
        TState State { get; }
    }

    public interface IStore<TState> : IStoreBase<TState>
    {
        event StateChangeHandler<TState> StateChanged;
    }

    public partial class Store<TState> : IStore<TState>
    {
        private readonly object _syncRoot = new object();
        private readonly Dispatcher _dispatcher;
        private readonly Reducer<TState> _reducer;
        private StateChangeHandler<TState> _stateChangeHandler;

        public Store(Reducer<TState> reducer, TState initialState = default(TState), params Middleware<TState>[] middlewares)
        {
            _reducer = reducer;
            _dispatcher = ApplyMiddlewares(middlewares);

            State = initialState;
        }

        public Object Dispatch(Object action)
        {
            return _dispatcher(action);
        }

        public TState State { get; private set; }
        
        public event StateChangeHandler<TState> StateChanged 
        {
            add 
            {
                _stateChangeHandler += value;
                value.Invoke(State);
            }
            remove {
                _stateChangeHandler -= value;
            }
        }

        private Dispatcher ApplyMiddlewares(params Middleware<TState>[] middlewares)
        {
            Dispatcher dispatcher = InnerDispatch;
            foreach (var middleware in middlewares)
            {
                dispatcher = middleware(this)(dispatcher);
            }
            return dispatcher;
        }

        private Object InnerDispatch(Object action)
        {
            lock (_syncRoot)
            {
                State = _reducer(State, action);
            }
            _stateChangeHandler?.Invoke(State);
            return action;
        }
    }
}
