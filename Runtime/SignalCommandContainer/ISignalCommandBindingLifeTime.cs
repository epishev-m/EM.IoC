
namespace EM.IoC
{
	public interface ISignalCommandBindingLifeTime
	{
		LifeTime LifeTime
		{
			get;
		}

		ISignalCommandBindingComposite InGlobal();

		ISignalCommandBindingComposite InLocal();
	}
}
