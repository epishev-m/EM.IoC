namespace EM.IoC
{
using Foundation;

public sealed class SignalCommandContainer :
	CommandContainer,
	ISignalCommandContainer
{
	#region ISignalCommandContainer

	public new ISignalCommandBindingLifeTime Bind<T>()
		where T : class, ISignal
	{
		var signal = Container.GetInstance<T>();

		return Bind(signal) as ISignalCommandBindingLifeTime;
	}

	public new bool Unbind<T>()
		where T : class, ISignal
	{
		var signal = Container.GetInstance<T>();
		signal.RemoveListener(ReactTo);

		return Unbind(signal);
	}

	#endregion
	#region CommandContainer

	public override void ReactTo(
		object trigger,
		object data = null)
	{
		base.ReactTo(trigger, data);

		if (GetBinding(trigger) is SignalCommandBinding {IsOneOff: true} binding)
		{
			Unbind(binding);
		}
	}

	protected override IBinding GetRawBinding(
		object key,
		object name)
	{
		return new SignalCommandBinding(this, key, name, BindingResolver);
	}

	protected override void BindingResolver(
		IBinding binding)
	{
		base.BindingResolver(binding);

		var signal = (ISignal) binding.Key;
		signal.AddListener(ReactTo);
	}

	#endregion
	#region SignalCommandContainer

	public SignalCommandContainer(
		IDiContainer container) :
		base(container)
	{
	}

	#endregion
}

}
