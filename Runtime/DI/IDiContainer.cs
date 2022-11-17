namespace EM.IoC
{

using System;
using Foundation;

public interface IDiContainer
{
	object Resolve(Type type);

	T Resolve<T>()
		where T : class;

	IDiBindingLifeTime Bind<T>()
		where T : class;

	bool Unbind<T>()
		where T : class;

	void Unbind(LifeTime lifeTime);

	void UnbindAll();
}

}
