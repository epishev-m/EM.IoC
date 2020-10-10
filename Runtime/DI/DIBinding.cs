using EM.Foundation;

namespace EM.IoC
{
	public sealed class DIBinding : IDIBinding, IDIBindingSingleton
	{
		#region IDIBindingSingleton

		public void ToSingleton()
		{
			throw new System.NotImplementedException();
		}

		#endregion
		#region IDIBinding

		public IDIBindingSingleton To<T>() where T : class, new()
		{
			throw new System.NotImplementedException();
		}

		public IDIBindingSingleton To(object obj)
		{
			throw new System.NotImplementedException();
		}

		public IDIBindingSingleton ToFactory<T>() where T : class, IFactory, new()
		{
			throw new System.NotImplementedException();
		}

		public IDIBindingSingleton ToFactory(IFactory factory)
		{
			throw new System.NotImplementedException();
		}

		#endregion
		#region DIBinding

		private readonly IBinding _binding;

		public DIBinding(object key, object name, Resolver resolver)
		{
			_binding = new Binding(key, name, resolver);
		}

		#endregion
	}
}
