namespace EM.IoC
{

using Foundation;

public interface IDiBinding
{
	IDiBindingSingleton To<T>()
		where T : class;

	void To(object obj);

	IDiBindingSingleton ToFactory<T>()
		where T : class, IFactory;

	IDiBindingSingleton ToFactory(IFactory factory);
}

}