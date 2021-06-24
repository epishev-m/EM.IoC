namespace EM.IoC
{

public interface IDiBindingLifeTime
{
	LifeTime LifeTime
	{
		get;
	}

	IDiBinding InGlobal();

	IDiBinding InLocal();
}

}