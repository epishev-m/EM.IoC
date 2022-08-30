namespace EM.IoC
{

using Foundation;
using System;
using System.Linq;

public sealed class InstanceProviderActivator :
	IInstanceProvider
{
	private readonly Type _type;

	private readonly IReflector _reflector;

	private readonly IDiContainer _diContainer;

	#region IInstanceProvider

	public object GetInstance()
	{
		var reflectionInfo = _reflector.GetReflectionInfo(_type);

		Requires.NotNull(reflectionInfo.ConstructorInfo, nameof(reflectionInfo.ConstructorInfo));

		var args = reflectionInfo.ConstructorParametersTypes
			.Select(t => _diContainer.GetInstance(t))
			.ToArray();

		var instance = Activator.CreateInstance(_type, args);

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

		_type = type;
		_reflector = reflector;
		_diContainer = diContainer;
	}

	#endregion
}

}
