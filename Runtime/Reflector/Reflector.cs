using EM.Foundation;

namespace EM.IoC
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public sealed class Reflector :
	IReflector
{
	private readonly Type postConstructorAttributeType;

	private readonly Dictionary<Type, IReflectionInfo> reflectionInfoCache;

	#region IReflector

	public IReflectionInfo GetReflectionInfo<T>()
	{
		return GetReflectionInfo(typeof(T));
	}

	public IReflectionInfo GetReflectionInfo(Type type)
	{
		IReflectionInfo reflectionInfo;

		if (reflectionInfoCache.TryGetValue(type, out var value))
		{
			reflectionInfo = value;
		}
		else
		{
			var constructorInfo = GetConstructorInfo(type);
			var constructorParameters = constructorInfo.GetParameters();
			var constructorParametersTypes = constructorParameters.Select(param => param.ParameterType);
			var postConstructorParametersTypes = default(IEnumerable<Type>);

			if (TryGetPostConstructorInfo(type, out var postConstructorInfo))
			{
				var postConstructorParameters = postConstructorInfo.GetParameters();
				postConstructorParametersTypes = postConstructorParameters.Select(param => param.ParameterType);
			}

			reflectionInfo = new ReflectionInfo(constructorInfo, constructorParametersTypes,
				postConstructorInfo, postConstructorParametersTypes);
			reflectionInfoCache.Add(type, reflectionInfo);
		}

		return reflectionInfo;
	}

	#endregion
	#region Reflector

	public Reflector(Type postConstructorAttributeType)
	{
		Requires.NotNull(postConstructorAttributeType, nameof(postConstructorAttributeType));

		this.postConstructorAttributeType = postConstructorAttributeType;
		reflectionInfoCache = new Dictionary<Type, IReflectionInfo>();
	}

	private static ConstructorInfo GetConstructorInfo(Type type)
	{
		var constructors = type.GetConstructors(
			BindingFlags.FlattenHierarchy |
			BindingFlags.Public |
			BindingFlags.Instance |
			BindingFlags.InvokeMethod);

		var result = constructors.Length switch
		{
			0 => default,
			> 1 => throw new InvalidOperationException($"Type {type} has several constructors."),
			_ => constructors[0]
		};

		return result;
	}

	private bool TryGetPostConstructorInfo(Type type, out MethodInfo postConstructor)
	{
		var postConstructorCached = default(MethodInfo);
		var methods = type.GetMethods(
			BindingFlags.FlattenHierarchy |
			BindingFlags.NonPublic |
			BindingFlags.Instance |
			BindingFlags.InvokeMethod);

		foreach (var methodInfo in methods)
		{
			var attributes = methodInfo.GetCustomAttributes(postConstructorAttributeType, true);

			if (attributes.Length <= 0)
			{
				continue;
			}

			if (postConstructorCached != default)
			{
				throw new InvalidOperationException($"Type {type} has several post-constructors.");
			}

			postConstructorCached = methodInfo;
		}

		postConstructor = postConstructorCached;
		var result = postConstructor != null;

		return result;
	}

	#endregion
}

}
