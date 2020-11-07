
namespace EM.IoC
{
	using EM.Foundation;
	using System;
	using System.Linq;

	public sealed class DIContainer :
		Binder,
		IDIContainer
	{
		#region IDIContainer

		public IDIBinding Bind<T>()
			where T : class
		{
			return base.Bind<T>() as IDIBinding;
		}

		public object GetInstance(
			Type type)
		{
			var result = default(object);
			var binding = GetBinding(type);

			if (binding != null)
			{
				var valuesArray = binding.Values;

				if (valuesArray.Count() > 0)
				{
					var instanceProvider = valuesArray.First() as IInstanceProvider;
					result = instanceProvider.GetInstance();
				}
			}

			return result;
		}

		public T GetInstance<T>()
			where T : class
		{
			return GetInstance(typeof(T)) as T;
		}

		public bool Unbind<T>()
			where T : class
		{
			return base.Unbind<T>();
		}

		#endregion
		#region Binder

		protected override IBinding GetRawBinding(
			object key,
			object name)
		{
			return new DIBinding(reflector, this, key, name, BindingResolver);
		}

		#endregion
		#region DIContainer

		private readonly IReflector reflector;

		public DIContainer(
			IReflector reflector)
		{
			Requires.IsNotNull(reflector, nameof(reflector));

			this.reflector = reflector;
		}

		#endregion
	}
}
