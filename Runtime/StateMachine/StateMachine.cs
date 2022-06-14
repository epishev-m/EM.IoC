namespace EM.IoC
{

using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public abstract class StateMachine<TState> :
	IStateMachine<TState>
{
	private readonly IStateFactory<TState> stateFactory;

	private Type currentStateType;

	private TState currentState;

	#region IStateMachine

	public bool IsActive<T>()
		where T : class, TState
	{
		return currentStateType == typeof(T);
	}

	public async UniTask EnterAsync<T>(CancellationToken ct = default)
		where T : class, TState, IEnterState
	{
		await UniTask.DelayFrame(1, PlayerLoopTiming.Update, ct);
		await EnterInternalAsync<T>(CallEnterAsync, ct);
	}

	public async UniTask EnterAsync<T, TPayload>(TPayload payload,
		CancellationToken ct = default)
		where T : class, TState, IPayloadEnterState<TPayload>
	{
		await UniTask.DelayFrame(1, PlayerLoopTiming.Update, ct);

		UniTask ExecuteAsync(TState state,
			CancellationToken ctt) =>
			CallEnterAsync(state, typeof(TPayload), payload, ctt);

		await EnterInternalAsync<T>(ExecuteAsync, ct);
	}

	#endregion

	#region StateMachine

	protected StateMachine(IStateFactory<TState> stateFactory)
	{
		this.stateFactory = stateFactory;
	}

	private async UniTask EnterInternalAsync<T>(Func<TState, CancellationToken, UniTask> callEnter,
		CancellationToken ct)
		where T : class, TState
	{
		if (IsActive<T>())
		{
			return;
		}

		var oldState = GetStateInstance(currentStateType);
		var newState = GetStateInstance(typeof(T));

		if (oldState is IExitState exitState)
		{
			await exitState.OnExitAsync(ct);
		}

		currentState = newState;
		currentStateType = newState.GetType();
		await callEnter(currentState, ct);
	}

	private static async UniTask CallEnterAsync(TState state,
		CancellationToken ct)
	{
		if (state is IEnterState enterState)
		{
			await enterState.OnEnterAsync(ct);
		}
	}

	private static async UniTask CallEnterAsync(TState state,
		Type payloadType,
		object payload,
		CancellationToken ct)
	{
		var stateGenericType = typeof(IPayloadEnterState<>).MakeGenericType(payloadType);

		if (!stateGenericType.IsInstanceOfType(state))
		{
			throw new StateException(
				$"State {state.GetType().FullName} mut be instance of IPayloadedState<{payloadType.Name}>!");
		}

		var enterMethod = stateGenericType.GetMethod("OnEnterAsync");

		if (enterMethod == null)
		{
			throw new StateException($"State {state.GetType().FullName}: cannot find OnEnter method!");
		}

		var task = (UniTask) enterMethod.Invoke(state, new[] {payload, ct});
		await task;
	}

	private TState GetStateInstance(Type stateType)
	{
		if (stateType == null)
		{
			return default;
		}

		var result = stateFactory.Create(stateType);

		if (result == null)
		{
			throw new StateException($"Cannot find game state {stateType.FullName}!");
		}

		return result;
	}

	#endregion
}

}