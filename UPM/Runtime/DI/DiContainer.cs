namespace EM.IoC
{

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation;

public sealed class DiContainer : Binder,
	IDiContainer
{
	private readonly IReflector _reflector;

	#region IDiContainer

	public IDiBinding Bind<T>()
		where T : class
	{
		return base.Bind<T>() as IDiBinding;
	}

	public object Resolve(Type type)
	{
		if (typeof(IList).IsAssignableFrom(type))
		{
			var listContentType = type.GetGenericArguments()[0];
			var resultList = ResolveAll(listContentType);

			return resultList;
		}

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
		var instance = Resolve(instanceProvider);

		return instance;
	}

	public T Resolve<T>()
		where T : class
	{
		return Resolve(typeof(T)) as T;
	}

	public IList ResolveAll(Type type)
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

		var genericType = typeof(List<>).MakeGenericType(type);
		var resultList = (IList)Activator.CreateInstance(genericType);

		foreach (var value in values)
		{
			var instanceProvider = (IInstanceProvider) value;
			var instance = Resolve(instanceProvider);
			resultList.Add(instance);
		}

		return resultList;
	}

	public List<T> ResolveAll<T>() where T : class
	{
		return ResolveAll(typeof(T)) as List<T>;
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

	private static object Resolve(IInstanceProvider instanceProvider)
	{
		var result = instanceProvider.GetInstance();

		if (result.Failure)
		{
			return null;
		}

		var instance = result.Data;

		return instance;
	}

	#endregion
}

}