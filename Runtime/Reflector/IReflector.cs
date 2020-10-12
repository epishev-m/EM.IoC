using System;

namespace EM.IoC
{
	public interface IReflector
	{
		IReflectionInfo GetReflectionInfo<T>();

		IReflectionInfo GetReflectionInfo(Type type);
	}
}
