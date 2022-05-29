namespace EM.IoC
{

using Foundation;

public sealed class SignalCommandBinding :
	CommandBinding,
	ISignalCommandBinding,
	ISignalCommandBindingOnce,
	ISignalCommandBindingLifeTime
{
	#region ISignalCommandBindingLifeTime

	public new ISignalCommandBindingComposite InGlobal()
	{
		return base.InGlobal() as ISignalCommandBindingComposite;
	}

	public new ISignalCommandBindingComposite InLocal()
	{
		return base.InLocal() as ISignalCommandBindingComposite;
	}

	#endregion

	#region ISignalCommandBindingOnce

	public bool IsOneOff
	{
		get;
		private set;
	}

	public ISignalCommandBindingComposite Once()
	{
		IsOneOff = true;

		return this;
	}

	public new ISignalCommandBinding InParallel()
	{
		return base.InParallel() as ISignalCommandBinding;
	}

	public new ISignalCommandBinding InSequence()
	{
		return base.InSequence() as ISignalCommandBinding;
	}

	#endregion

	#region ISignalCommandBinding

	public new ISignalCommandBinding To<T>()
		where T : ICommand
	{
		return base.To<T>() as ISignalCommandBinding;
	}

	#endregion

	#region SignalCommandBinding

	public SignalCommandBinding(
		ICommandContainer container,
		object key,
		object name,
		Resolver resolver)
		:
		base(container,
			key,
			name,
			resolver)
	{
	}

	#endregion
}

}