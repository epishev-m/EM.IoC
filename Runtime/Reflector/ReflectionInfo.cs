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

	public ConstructorInfo ConstructorInfo { get; }

	public IEnumerable<Type> ParameterTypes { get; }

	#endregion
	#region ReflectionInfo

	public ReflectionInfo(
		ConstructorInfo constructorInfo,
		IEnumerable<Type> parameterTypes)
	{
		Requires.NotNull(constructorInfo, nameof(constructorInfo));
		Requires.NotNull(parameterTypes, nameof(parameterTypes));

		ConstructorInfo = constructorInfo;
		ParameterTypes = parameterTypes;
	}

	#endregion
}

}
