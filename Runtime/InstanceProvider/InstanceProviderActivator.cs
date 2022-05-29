﻿namespace EM.IoC
{

using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class InstanceProviderActivator :
	IInstanceProvider
{
	private readonly Type type;

	private readonly IReflector reflector;

	private readonly IDiContainer diContainer;

	#region IInstanceProvider

	public object GetInstance()
	{
		var reflectionInfo = reflector.GetReflectionInfo(type);

		Requires.NotNull(reflectionInfo.ConstructorInfo, nameof(reflectionInfo.ConstructorInfo));

		var args = reflectionInfo.ConstructorParametersTypes
			.Select(t => diContainer.GetInstance(t))
			.ToArray();

		var instance = Activator.CreateInstance(type, args);

		return instance;
	}

	#endregion

	#region InstanceProviderActivator

	public InstanceProviderActivator(Type type,
		IReflector reflector,
		IDiContainer diContainer)
	{
		Requires.NotNull(type, nameof(type));
		Requires.NotNull(reflector, nameof(reflector));
		Requires.NotNull(diContainer, nameof(diContainer));

		this.type = type;
		this.reflector = reflector;
		this.diContainer = diContainer;
	}

	#endregion
}

}
