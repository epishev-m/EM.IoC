
namespace EM.IoC
{
	using System;
	
	public interface IDIContainer
	{
		object GetInstance(
			Type type);

		T GetInstance<T>()
			where T : class;

		IDIBindingLifeTime Bind<T>()
			where T : class;

		bool Unbind<T>()
			where T : class;

		void Unbind(LifeTime lifeTime);

		void UnbindAll();
	}
}
