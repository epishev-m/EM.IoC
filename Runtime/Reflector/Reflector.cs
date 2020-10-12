using EM.Foundation;
using System;
using System.Linq;
using System.Reflection;
using Binder = EM.Foundation.Binder;

namespace EM.IoC
{
	public sealed class Reflector : IReflector
	{
		#region IReflector

		public IReflectionInfo GetReflectionInfo<T>()
		{
			return GetReflectionInfo(typeof(T));
		}

		public IReflectionInfo GetReflectionInfo(Type type)
		{
			IReflectionInfo reflectionInfo;
			var binding = _binder.GetBinding(type);

			if (binding == null)
			{
				var constructorInfo = GetConstructorInfo(type);
				var parameters = constructorInfo.GetParameters();
				var parameterTypes = parameters.Select(param => param.ParameterType);
				reflectionInfo = new ReflectionInfo(constructorInfo, parameterTypes);
				_binder.Bind(type).To(reflectionInfo);
			}
			else
			{
				reflectionInfo = binding.Values.FirstOrDefault() as IReflectionInfo;
			}

			return reflectionInfo;
		}

		#endregion
		#region Reflector

		private readonly IBinder _binder = new Binder();

		private ConstructorInfo GetConstructorInfo(Type type)
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
