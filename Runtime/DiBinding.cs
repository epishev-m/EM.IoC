namespace EM.IoC
{

using Foundation;
using System.Linq;

public sealed class DiBinding :
	Binding,
	IDiBindingSingleton,
	IDiBinding,
	IDiBindingLifeTime
{
	private readonly IReflector _reflector;

	private readonly IDiContainer _container;

	#region IDiBindingLifeTime

	public LifeTime LifeTime
	{
		get;
		private set;
	} = LifeTime.External;

	public IDiBinding InGlobal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this, nameof(InGlobal));

		LifeTime = LifeTime.Global;

		return this;
	}

	public IDiBinding InLocal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.External, this, nameof(InLocal));

		LifeTime = LifeTime.Local;

		return this;
	}

	#endregion

	#region IDiBinding

	public new IDiBindingSingleton To<T>()
		where T : class
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(To));
		Requires.ValidOperation(Values == null, this, nameof(Values));

		var instanceProvider = new InstanceProviderActivator(typeof(T),
			_reflector,
			_container);

		return base.To(instanceProvider) as IDiBindingSingleton;
	}

	public new void To(object instance)
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(To));
		Requires.ValidOperation(Values == null, this, nameof(Values));
		Requires.NotNull(instance, nameof(instance));
		Requires.ReferenceType(instance.GetType(), nameof(instance));

		var instanceProvider = new InstanceProvider(instance);
		var unused = base.To(instanceProvider);
	}

	public IDiBindingSingleton ToFactory<T>()
		where T : class, IFactory
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(ToFactory));
		Requires.ValidOperation(Values == null, this, nameof(Values));

		var instanceProvider = new InstanceProviderFactory(
			new InstanceProviderActivator(typeof(T), _reflector, _container));

		var unused = base.To(instanceProvider);

		return this;
	}

	public IDiBindingSingleton ToFactory(IFactory factory)
	{
		Requires.ValidOperation(LifeTime != LifeTime.External, this, nameof(ToFactory));
		Requires.ValidOperation(Values == null, this, nameof(Values));
		Requires.NotNull(factory, nameof(factory));

		var instanceProvider = new InstanceProviderFactory(new InstanceProvider(factory));

		var unused = base.To(instanceProvider);

		return this;
	}

	#endregion

	#region IDiBindingSingleton

	public void ToSingleton()
	{
		Requires.ValidOperation(Values != null, this, nameof(Values));

		var value = Values?.First();
		var instanceProvider = value as IInstanceProvider;
		instanceProvider = new InstanceProviderSingleton(instanceProvider);
		RemoveAllValues();
		var unused = base.To(instanceProvider);
	}

	#endregion

	#region DiBinding

	public DiBinding(IReflector reflector,
		IDiContainer container,
		object key,
		object name,
		Resolver resolver)
		:
		base(key,
			name,
			resolver)
	{
		Requires.NotNull(reflector, nameof(reflector));
		Requires.NotNull(container, nameof(container));

		_container = container;
		_reflector = reflector;
	}

	#endregion
}

}
