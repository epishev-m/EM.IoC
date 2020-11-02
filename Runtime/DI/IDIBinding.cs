
namespace EM.IoC
{
	using EM.Foundation;
	
	public interface IDIBinding
	{
		IDIBindingSingleton To<T>()
			where T : class;

		void To(
			object obj);

		IDIBindingSingleton ToFactory<T>()
			where T : class, IFactory;

		IDIBindingSingleton ToFactory(
			IFactory factory);
	}
}