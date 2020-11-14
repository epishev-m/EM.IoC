
namespace EM.IoC
{
	public interface ICommandBindingLifeTime
	{
		LifeTime LifeTime
		{
			get;
		}

		ICommandBindingComposite InGlobal();

		ICommandBindingComposite InLocal();
	}
}
