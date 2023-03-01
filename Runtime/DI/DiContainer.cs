﻿namespace EM.IoC
{

using System;
using System.Linq;
using Foundation;

public sealed class DiContainer : Binder,
	IDiContainer
{
	private readonly IReflector _reflector;

	#region IDiContainer

	public IDiBindingLifeTime Bind<T>()
		where T : class
	{
		return base.Bind<T>() as IDiBindingLifeTime;
	}

	public object Resolve(Type type)
	{
		var binding = GetBinding(type);

		if (binding == null)
		{
			return null;
		}

		var values = binding.Values.ToArray();

		if (!values.Any())
		{
			return null;
		}

		var instanceProvider = (IInstanceProvider) values.First();
		var result = instanceProvider.GetInstance();

		if (result.Failure)
		{
			return null;
		}

		var instance = result.Data;

		return instance;
	}

	public T Resolve<T>()
		where T : class
	{
		return Resolve(typeof(T)) as T;
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
		return new DiBinding(_reflector, this, key, name, BindingResolver);
	}

	#endregion

	#region DIContainer

	public DiContainer(IReflector reflector)
	{
		Requires.NotNullParam(reflector, nameof(reflector));

		_reflector = reflector;
	}

	#endregion
}

}