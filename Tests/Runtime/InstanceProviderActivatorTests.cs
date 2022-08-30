using EM.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using EM.Foundation;

internal sealed class InstanceProviderActivatorTests
{
	[Test]
	public void InstanceProviderActivator_ConstructorParam1_Exception()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DiContainer();

		// Act
		try
		{
			var unused = new InstanceProviderActivator(null, reflector, container);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void InstanceProviderActivator_ConstructorParam2_Exception()
	{
		// Arrange
		var actual = false;
		var type = typeof(Test);
		var container = new DiContainer();

		// Act
		try
		{
			var unused = new InstanceProviderActivator(type, null, container);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void InstanceProviderActivator_ConstructorParam3_Exception()
	{
		// Arrange
		var actual = false;
		var type = typeof(Test);
		var reflector = new Reflector();

		// Act
		try
		{
			var unused = new InstanceProviderActivator(type, reflector, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void InstanceProviderActivator_GetInstance()
	{
		// Arrange
		var type = typeof(Test);
		var reflector = new Reflector();
		var container = new DiContainer();

		// Act
		var provider = new InstanceProviderActivator(type, reflector, container);
		var instance = provider.GetInstance();
		var actual = instance is Test;

		// Assert
		Assert.IsNotNull(instance);
		Assert.IsTrue(actual);
	}

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	private sealed class Test
	{
		public Test(A param)
		{
		}
	}

	private sealed class A
	{
	}

	private sealed class Reflector :
		IReflector
	{
		public IReflectionInfo GetReflectionInfo<T>()
		{
			return GetReflectionInfo(typeof(T));
		}

		public IReflectionInfo GetReflectionInfo(Type type)
		{
			return new ReflectionInfo(type);
		}
	}

	private sealed class DiContainer :
		IDiContainer
	{
		public IDiBindingLifeTime Bind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public object GetInstance(Type type)
		{
			var result = default(object);

			if (type == typeof(A))
			{
				result = new A();
			}

			return result;
		}

		public T GetInstance<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public bool Unbind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public void UnbindAll()
		{
			throw new NotImplementedException();
		}

		public void Unbind(LifeTime lifeTime)
		{
			throw new NotImplementedException();
		}
	}
}