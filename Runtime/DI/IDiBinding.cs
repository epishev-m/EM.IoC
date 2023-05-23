namespace EM.IoC
{

using Foundation;

public interface IDiBinding
{
	LifeTime LifeTime
	{
		get;
	}

	IDiBinding InGlobal();

	IDiBinding InLocal();

	IDiBinding SetLifeTime(LifeTime lifeTime);
	
	IDiBinding To<T>()
		where T : class;

	IDiBinding To(object obj);

	IDiBinding ToFactory<T>()
		where T : class, IFactory;

	IDiBinding ToFactory(IFactory factory);

	void AsSingle();
}

}