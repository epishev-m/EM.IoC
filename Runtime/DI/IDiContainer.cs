namespace EM.IoC
{

using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;

public interface IDiContainer
{
	object Resolve(Type type);

	T Resolve<T>()
		where T : class;
	
	IList ResolveAll(Type type);

	List<T> ResolveAll<T>()
		where T : class;

	IDiBinding Bind<T>()
		where T : class;

	bool Unbind<T>()
		where T : class;

	void Unbind(LifeTime lifeTime);

	void UnbindAll();
}

}
