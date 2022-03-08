namespace EM.IoC
{
using Foundation;
using System;
using System.Collections.Generic;
using System.Reflection;

public sealed class ReflectionInfo :
	IReflectionInfo
{
	#region IReflectionInfo

	public ConstructorInfo ConstructorInfo
	{
		get;
	}

	public IEnumerable<Type> ConstructorParametersTypes
	{
		get;
	}

	public MethodInfo PostConstructorInfo
	{
		get;
	}

	public IEnumerable<Type> PostConstructorParametersTypes
	{
		get;
	}

	#endregion
	#region ReflectionInfo

	public ReflectionInfo(
		ConstructorInfo constructorInfo,
		IEnumerable<Type> parameterTypes,
		MethodInfo postConstructorInfo,
		IEnumerable<Type> postConstructorParametersTypes)
	{
		Requires.NotNull(constructorInfo, nameof(constructorInfo));
		Requires.NotNull(parameterTypes, nameof(parameterTypes));

		ConstructorInfo = constructorInfo;
		ConstructorParametersTypes = parameterTypes;
		PostConstructorInfo = postConstructorInfo;
		PostConstructorParametersTypes = postConstructorParametersTypes;
	}

	#endregion
}

}
