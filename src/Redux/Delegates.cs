using System;

namespace Redux {
    /// <summary>
    /// Represents a method that can be used as a handler for <see cref="IStore{TState}.StateChanged" />
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    public delegate void StateChangeHandler<in TState>(TState state);

    /// <summary>
    /// Represents a method that dispatches an action.
    /// </summary>
    /// <param name="action">The action to dispatch.</param>
    /// <returns>Something</returns>
    public delegate Object Dispatcher(Object action);

    /// <summary>
    /// Represents a method that is used to transform one state to the next.
    /// </summary>
    /// <typeparam name="TState">The state tree type.</typeparam>
    /// <param name="previousState">The previous state tree.</param>
    /// <param name="action">The action to be applied to the state tree.</param>
    /// <returns>The updated state tree.</returns>
    public delegate TState Reducer<TState>(TState previousState, Object action);

    /// <summary>Represents a method that is used as middleware</summary>
    /// <typeparam name="TState">The state tree type.</typeparam>
    /// <param name="store">The <see cref="T:Store{TState}" /> this middleware is to be used by.</param>
    /// <returns>
    ///     A function that, when called with a <see cref="Dispatcher" />, returns a new
    ///     <see cref="Dispatcher" /> that wraps the first one.
    /// </returns>
    public delegate Func<Dispatcher, Dispatcher> Middleware<in TState>(IStoreBase<TState> store);
}