
namespace EM.IoC
{
	using EM.Foundation;

	public interface ISignalCommandContainer
	{
		ISignalCommandBindingLifeTime Bind<T>()
			where T : class, ISignal;

		bool Unbind<T>()
			where T : class, ISignal;

		void Unbind(LifeTime lifeTime);

		void UnbindAll();
	}
}
