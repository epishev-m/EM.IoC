
namespace EM.IoC
{
	using EM.Foundation;
	using System.Linq;

	public sealed class DIBinding :
		Binding,
		IDIBindingSingleton,
		IDIBinding,
		IDIBindingLifeTime
	{
		#region IDIBindingLifeTime

		public LifeTime LifeTime => lifeTime;

		public IDIBinding InGlobal()
		{
			Requires.IsValidOperation(lifeTime == LifeTime.External, this, nameof(InGlobal));

			lifeTime = LifeTime.Global;

			return this;
		}

		public IDIBinding InScene()
		{
			Requires.IsValidOperation(lifeTime == LifeTime.External, this, nameof(InScene));

			lifeTime = LifeTime.Global;

			return this;
		}

		#endregion
		#region IDIBinding

		public new IDIBindingSingleton To<T>()
			where T : class
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(To));
			Requires.IsNull(Values, nameof(Values));

			var instanceProvider = new InstanceProviderActivator(
				typeof(T),
				reflector,
				container);

			return base.To(instanceProvider) as IDIBindingSingleton;
		}

		public new void To(
			object instance)
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(To));
			Requires.IsNull(Values, nameof(Values));
			Requires.IsNotNull(instance, nameof(instance));
			Requires.IsReferenceType(instance.GetType(), nameof(instance));

			var instanceProvider = new InstanceProvider(instance);

			var unused = base.To(instanceProvider);
		}

		public IDIBindingSingleton ToFactory<T>()
			where T : class, IFactory
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(ToFactory));
			Requires.IsNull(Values, nameof(Values));

			var instanceProvider = new InstanceProviderFactory(
				new InstanceProviderActivator(
					typeof(T),
					reflector,
					container));

			var unused = base.To(instanceProvider);

			return this;
		}

		public IDIBindingSingleton ToFactory(
			IFactory factory)
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(ToFactory));
			Requires.IsNull(Values, nameof(Values));
			Requires.IsNotNull(factory, nameof(factory));

			var instanceProvider = new InstanceProviderFactory(
				new InstanceProvider(
					factory));

			var unused = base.To(instanceProvider);

			return this;
		}

		#endregion
		#region IDIBindingSingleton

		public void ToSingleton()
		{
			Requires.IsValidOperation(lifeTime != LifeTime.External, this, nameof(ToFactory));
			Requires.IsNotNull(Values, nameof(Values));

			var value = Values.First();
			var instanceProvider = value as IInstanceProvider;
			instanceProvider = new InstanceProviderSingleton(instanceProvider);
			RemoveAllValues();

			var unused = base.To(instanceProvider);
		}

		#endregion
		#region DIBinding

		private readonly IReflector reflector;

		private readonly IDIContainer container;

		private LifeTime lifeTime = LifeTime.External;

		public DIBinding(
			IReflector reflector,
			IDIContainer container,
			object key,
			object name,
			Resolver resolver) :
			base(
				key,
				name,
				resolver)
		{
			Requires.IsNotNull(reflector, nameof(reflector));
			Requires.IsNotNull(container, nameof(container));

			this.container = container;
			this.reflector = reflector;
		}

		#endregion
	}
}
