namespace EM.IoC
{

using Foundation;

public interface ICommandContainer
{
	void ReactTo<T>(object data = null);

	void ReactTo(object trigger,
		object data = null);

	ICommandBindingLifeTime Bind<T>();

	ICommandBindingLifeTime Bind(object key);

	bool Unbind<T>();

	bool Unbind(object key);

	void UnbindAll();

	void Unbind(LifeTime lifeTime);
}

}
