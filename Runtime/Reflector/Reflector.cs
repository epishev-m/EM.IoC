
namespace EM.IoC
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public sealed class Reflector :
		IReflector
	{
		#region IReflector

		public IReflectionInfo GetReflectionInfo<T>()
		{
			return GetReflectionInfo(typeof(T));
		}

		public IReflectionInfo GetReflectionInfo(
			Type type)
		{
			IReflectionInfo reflectionInfo;

			if (reflectionInfoCache.TryGetValue(type, out var value))
			{
				reflectionInfo = value as IReflectionInfo;
			}
			else
			{
				var constructorInfo = GetConstructorInfo(type);
				var parameters = constructorInfo.GetParameters();
				var parameterTypes = parameters.Select(param => param.ParameterType);
				reflectionInfo = new ReflectionInfo(constructorInfo, parameterTypes);
				reflectionInfoCache.Add(type, reflectionInfo);
			}

			return reflectionInfo;
		}

		#endregion
		#region Reflector

		private readonly Dictionary<Type, IReflectionInfo> reflectionInfoCache;

		public Reflector()
		{
			reflectionInfoCache = new Dictionary<Type, IReflectionInfo>();
		}

		private ConstructorInfo GetConstructorInfo(
			Type type)
		{
			var constructors = type.GetConstructors(
				BindingFlags.FlattenHierarchy |
				BindingFlags.Public |
				BindingFlags.Instance |
				BindingFlags.InvokeMethod);

			ConstructorInfo result;

			if (constructors.Length == 1)
			{
				result = constructors[0];
			}
			else if (constructors.Length <= 0)
			{
				throw new InvalidOperationException($"Type {type} has several constructors.");
			}
			else
			{
				throw new InvalidOperationException($"Type {type} has no constructor.");
			}

			return result;
		}

		#endregion
	}
}
