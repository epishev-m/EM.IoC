
namespace EM.IoC
{
	public interface IDIBindingLifeTime
	{
		LifeTime LifeTime
		{
			get;
		}

		IDIBinding InGlobal();

		IDIBinding InScene();
	}
}
