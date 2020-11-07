
namespace EM.IoC
{
	using EM.Foundation;
	using System.Linq;

	public sealed class DIBinding :
		Binding,
		IDIBinding,
		IDIBindingSingleton
	{
		#region IDIBindingSingleton

		public void ToSingleton()
		{
			Requires.IsNotNull(Values, nameof(Values));

			var value = Values.First();
			var instanceProvider = value as IInstanceProvider;
			instanceProvider = new InstanceProviderSingleton(instanceProvider);
			RemoveAllValues();
			var unused = base.To(instanceProvider);
		}

		#endregion
		#region IDIBinding

		public new IDIBindingSingleton To<T>()
			where T : class
		{
			Requires.IsNull(Values, nameof(Values));

			var instanceProvider = new InstanceProviderActivator(
				typeof(T),
				reflector,
				container);

			var unused = base.To(instanceProvider);

			return this;
		}

		public new void To(
			object instance)
		{
			Requires.IsNull(Values, nameof(Values));
			Requires.IsNotNull(instance, nameof(instance));
			Requires.IsReferenceType(instance.GetType(), nameof(instance));

			var instanceProvider = new InstanceProvider(instance);
			var unused = base.To(instanceProvider);
		}

		public IDIBindingSingleton ToFactory<T>()
			where T : class, IFactory
		{
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
			Requires.IsNull(Values, nameof(Values));
			Requires.IsNotNull(factory, nameof(factory));

			var instanceProvider = new InstanceProviderFactory(
				new InstanceProvider(
					factory));

			var unused = base.To(instanceProvider);

			return this;
		}

		#endregion
		#region DIBinding

		private readonly IReflector reflector;

		private readonly IDIContainer container;

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
