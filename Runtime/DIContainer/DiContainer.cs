namespace EM.IoC
{
using Foundation;
using System;
using System.Linq;

public sealed class DiContainer :
	Binder,
	IDiContainer
{
	private readonly IReflector reflector;

	#region IDiContainer

	public IDiBindingLifeTime Bind<T>()
		where T : class
	{
		return base.Bind<T>() as IDiBindingLifeTime;
	}

	public object GetInstance(Type type)
	{
		var binding = GetBinding(type);

		if (binding == null)
		{
			return null;
		}

		var valuesArray = binding.Values.ToArray();

		if (valuesArray.Length <= 0)
		{
			return null;
		}

		var instanceProvider = (IInstanceProvider) valuesArray.First();
		var result = instanceProvider.GetInstance();

		return result;
	}

	public T GetInstance<T>()
		where T : class
	{
		return GetInstance(typeof(T)) as T;
	}

	public void Inject(object obj)
	{
		Requires.NotNull(obj, nameof(obj));

		var type = obj.GetType();
		var reflectionInfo = reflector.GetReflectionInfo(type);

		var postConstructorInfo = reflectionInfo.PostConstructorInfo;
		var args = reflectionInfo.PostConstructorParametersTypes
			.Select(GetInstance)
			.ToArray();

		if (postConstructorInfo == null)
		{
			return;
		}

		postConstructorInfo.Invoke(obj, args);
	}

	public bool Unbind<T>()
		where T : class
	{
		return base.Unbind<T>();
	}

	public void Unbind(LifeTime lifeTime)
	{
		Unbind(binding =>
		{
			var diBinding = (DiBinding) binding;
			var result = diBinding.LifeTime == lifeTime;

			return result;
		});
	}

	#endregion

	#region Binder

	protected override IBinding GetRawBinding(object key,
		object name)
	{
		return new DiBinding(reflector, this, key, name, BindingResolver);
	}

	#endregion

	#region DIContainer

	public DiContainer(IReflector reflector)
	{
		Requires.NotNull(reflector, nameof(reflector));

		this.reflector = reflector;
	}

	#endregion
}

}