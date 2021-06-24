namespace EM.IoC
{
using Foundation;

public class CommandBinding :
	Binding,
	ICommandBinding,
	ICommandBindingComposite,
	ICommandBindingLifeTime
{
	private readonly ICommandContainer container;

	private bool? isSequence;

	#region ICommandBindingLifeTime

	public ICommandBindingComposite InGlobal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this, nameof(InGlobal));

		LifeTime = LifeTime.Global;

		return this;
	}

	public ICommandBindingComposite InLocal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this, nameof(InLocal));

		LifeTime = LifeTime.Local;

		return this;
	}

	#endregion
	#region ICommandBindingComposite

	public ICommandBinding InParallel()
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(InParallel));
		Requires.ValidOperation(isSequence == null, this, nameof(InParallel));

		isSequence = false;

		return this;
	}

	public ICommandBinding InSequence()
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(InSequence));
		Requires.ValidOperation(isSequence == null, this, nameof(InSequence));

		isSequence = true;

		return this;
	}

	#endregion
	#region ICommandBinding

	public new ICommandBinding To<T>()
		where T : ICommand
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(To));
		Requires.ValidOperation(isSequence != null, this, nameof(isSequence));

		return base.To<T>() as ICommandBinding;
	}

	public void Execute(object data = null)
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(Execute));
		Requires.ValidOperation(isSequence != null, this, nameof(isSequence));

		container.ReactTo(Key, data);
	}

	#endregion
	#region CommandBinding

	public bool? IsSequence => isSequence;

	public LifeTime LifeTime
	{
		get;
		private set;
	} = LifeTime.External;

	public CommandBinding(
		ICommandContainer container,
		object key,
		object name,
		Resolver resolver) :
		base(
			key,
			name,
			resolver)
	{
		Requires.NotNull(container, nameof(container));

		this.container = container;
	}

	#endregion
}

}
