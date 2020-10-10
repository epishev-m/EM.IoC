using EM.Foundation;

namespace EM.IoC
{
	public interface IDIBinding
	{
		IDIBindingSingleton To<T>() where T : class, new();

		IDIBindingSingleton To(object obj);

		IDIBindingSingleton ToFactory<T>() where T : class, IFactory, new();

		IDIBindingSingleton ToFactory(IFactory factory);
	}
}