namespace Redux.DevTools
{
    public class TimeMachineStore<TState> : Store<TimeMachineState>
    {
        public TimeMachineStore(Reducer<TState> reducer, TState initialState = default(TState))
            : base(new TimeMachineReducer((state, action) => reducer((TState)state, action)).Execute, new TimeMachineState(initialState))
        {
        }

        public new TState State => ((IStore<TState>)this).State;
    }
}