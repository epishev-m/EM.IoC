
namespace EM.IoC
{
	using EM.Foundation;
	using System;
	using System.Linq;

	public sealed class DIBinding :
		IDIBinding,
		IDIBindingSingleton
	{
		#region IDIBindingSingleton

		public void ToSingleton()
		{
			throw new System.NotImplementedException();
		}

		#endregion
		#region IDIBinding

		public IDIBindingSingleton ToSelf()
		{
			throw new NotImplementedException();
		}

		public IDIBindingSingleton To<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public void To(
			object obj)
		{
			throw new System.NotImplementedException();
		}

		public IDIBindingSingleton ToFactory<T>()
			where T : class, IFactory
		{
			throw new System.NotImplementedException();
		}

		#endregion
		#region DIBinding

		private readonly IReflector reflector;

		private readonly IBinding binding;

		public DIBinding(
			IReflector reflector,
			object key,
			object name,
			Resolver resolver)
		{
			Requires.IsNotNull(reflector, nameof(reflector));
			Requires.IsNotNull(key, nameof(key));

			this.reflector = reflector;
			binding = new Binding(key, name, resolver);
		}

		private void CheckValues()
		{
			if (binding.Values != null)
			{
				throw new InvalidOperationException("value already set");
			}
		}

		#endregion
	}
}
