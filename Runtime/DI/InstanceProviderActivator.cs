namespace EM.IoC
{

using System;
using System.Linq;
using Foundation;

public sealed class InstanceProviderActivator : IInstanceProvider
{
	private readonly Type _type;

	private readonly IReflector _reflector;

	private readonly IDiContainer _diContainer;

	#region IInstanceProvider

	public Result<object> GetInstance()
	{
		var resultReflectionInfo = _reflector.GetReflectionInfo(_type);

		if (resultReflectionInfo.Failure)
		{
			return new ErrorResult<object>(DiStringResources.FailedToCreate(this));
		}

		var result = resultReflectionInfo.Data.GetConstructorParamTypes();

		if (result.Failure)
		{
			return new ErrorResult<object>(DiStringResources.CouldNotFindConstructor(this));
		}

		var args = result.Data
			.Select(t => _diContainer.Resolve(t))
			.ToArray();

		var instance = Activator.CreateInstance(_type, args);

		return new SuccessResult<object>(instance);
	}

	#endregion

	#region InstanceProviderActivator

	public InstanceProviderActivator(Type type,
		IReflector reflector,
		IDiContainer diContainer)
	{
		Requires.NotNullParam(type, nameof(type));
		Requires.NotNullParam(reflector, nameof(reflector));
		Requires.NotNullParam(diContainer, nameof(diContainer));

		_type = type;
		_reflector = reflector;
		_diContainer = diContainer;
	}

	#endregion
}

}
