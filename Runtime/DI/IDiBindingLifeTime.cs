namespace EM.IoC
{

using Foundation;

public interface IDiBindingLifeTime
{
	LifeTime LifeTime
	{
		get;
	}

	IDiBinding InGlobal();

	IDiBinding InLocal();

	IDiBinding SetLifeTime(LifeTime lifeTime);
}

}