
namespace EM.IoC
{
	using System;
	
	public interface IDIContainer
	{
		object GetInstance(
			Type type);

		T GetInstance<T>()
			where T : class;

		IDIBinding Bind<T>()
			where T : class;

		void Unbind<T>()
			where T : class;
	}
}
