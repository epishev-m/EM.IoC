
namespace EM.IoC
{
	using EM.Foundation;

	public interface ISignalCommandContainer
	{
		ISignalCommandBindingComposite Bind<T>()
			where T : class, ISignal;

		bool Unbind<T>()
			where T : class, ISignal;
	}
}
