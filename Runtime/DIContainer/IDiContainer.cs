namespace EM.IoC
{

using System;

public interface IDiContainer
{
	object GetInstance(Type type);

	T GetInstance<T>()
		where T : class;

	void Inject(object obj);

	IDiBindingLifeTime Bind<T>()
		where T : class;

	bool Unbind<T>()
		where T : class;

	void Unbind(LifeTime lifeTime);

	void UnbindAll();
}

}
