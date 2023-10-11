namespace EM.IoC
{

using Foundation;
using System.Linq;

public sealed class DiBinding :
	Binding,
	IDiBinding
{
	private readonly IReflector _reflector;

	private readonly IDiContainer _container;

	#region IDiBinding

	public LifeTime LifeTime
	{
		get;
		private set;
	} = LifeTime.None;

	public IDiBinding InGlobal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.None, this);

		LifeTime = LifeTime.Global;

		return this;
	}

	public IDiBinding InLocal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.None, this);

		LifeTime = LifeTime.Local;

		return this;
	}

	public IDiBinding SetLifeTime(LifeTime lifeTime)
	{
		Requires.ValidOperation(LifeTime == LifeTime.None, this);

		LifeTime = lifeTime;

		return this;
	}

	public new IDiBinding To<T>()
		where T : class
	{
		Requires.ValidOperation(LifeTime != LifeTime.None, this);

		ValidInstanceProviderSingleton();

		var instanceProvider = new InstanceProviderActivator(typeof(T),
			_reflector,
			_container);

		return base.To(instanceProvider) as IDiBinding;
	}

	public new IDiBinding To(object instance)
	{
		Requires.ValidOperation(LifeTime != LifeTime.None, this);
		Requires.NotNullParam(instance, nameof(instance));
		Requires.ReferenceType(instance.GetType(), nameof(instance));

		ValidInstanceProviderSingleton();
		
		var instanceProvider = new InstanceProvider(instance);

		return base.To(instanceProvider) as IDiBinding;
	}

	public IDiBinding ToFactory<T>()
		where T : class, IFactory
	{
		Requires.ValidOperation(LifeTime != LifeTime.None, this);

		ValidInstanceProviderSingleton();
		
		var instanceProvider = new InstanceProviderFactory(
			new InstanceProviderActivator(typeof(T), _reflector, _container));

		return base.To(instanceProvider) as IDiBinding;
	}

	public IDiBinding ToFactory(IFactory factory)
	{
		Requires.ValidOperation(LifeTime != LifeTime.None, this);
		Requires.NotNullParam(factory, nameof(factory));

		ValidInstanceProviderSingleton();
		
		var instanceProvider = new InstanceProviderFactory(
			new InstanceProvider(factory));

		return base.To(instanceProvider) as IDiBinding;
	}

	public void AsSingle()
	{
		Requires.ValidOperation(Values != null, this);
		Requires.ValidOperation(Values != null && Values.Count() == 1, this);

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
		: base(key,
			name,
			resolver)
	{
		Requires.NotNullParam(reflector, nameof(reflector));
		Requires.NotNullParam(container, nameof(container));

		_container = container;
		_reflector = reflector;
	}

	private void ValidInstanceProviderSingleton()
	{
		var value = Values?.First();

		if (value != null)
		{
			Requires.ValidOperation(value is not InstanceProviderSingleton, this);
		}
	}

	#endregion
}

}
