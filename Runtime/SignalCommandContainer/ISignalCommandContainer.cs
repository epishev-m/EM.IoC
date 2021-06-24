namespace EM.IoC
{
using Foundation;

public interface ISignalCommandContainer
{
	ISignalCommandBindingLifeTime Bind<T>()
		where T : class, ISignal;

	bool Unbind<T>()
		where T : class, ISignal;

	void Unbind(
		LifeTime lifeTime);

	void UnbindAll();
}

}
