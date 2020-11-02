using EM.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

internal sealed class InstanceProviderActivatorTests
{
	[Test]
	public void InstanceProviderActivator_ConstructorParam1_Exeption()
	{
		// Arrange
		var actual = false;
		var type = default(Type);
		var reflector = new Reflector();
		var container = new DIContainer();

		// Act
		try
		{
			var unused = new InstanceProviderActivator(type, reflector, container);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void InstanceProviderActivator_ConstructorParam2_Exeption()
	{
		// Arrange
		var actual = false;
		var type = typeof(Test);
		var reflector = default(Reflector);
		var container = new DIContainer();

		// Act
		try
		{
			var unused = new InstanceProviderActivator(type, reflector, container);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void InstanceProviderActivator_ConstructorParam3_Exeption()
	{
		// Arrange
		var actual = false;
		var type = typeof(Test);
		var reflector = new Reflector();
		var container = default(DIContainer);

		// Act
		try
		{
			var unused = new InstanceProviderActivator(type, reflector, container);
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
		var expected = true;
		var type = typeof(Test);
		var reflector = new Reflector();
		var container = new DIContainer();

		// Act
		var provider = new InstanceProviderActivator(type, reflector, container);
		var instance = provider.GetInstance();
		var actual = instance is Test;

		// Assert
		Assert.IsNotNull(instance);
		Assert.AreEqual(expected, actual);
	}

	internal sealed class Test
	{
		public Test(
			A param)
		{
		}
	}

	internal sealed class A
	{
	}

	internal sealed class Reflector :
		IReflector
	{
		public IReflectionInfo GetReflectionInfo<T>()
		{
			return GetReflectionInfo(typeof(T));
		}

		public IReflectionInfo GetReflectionInfo(
			Type type)
		{
			var constructors = type.GetConstructors(
				BindingFlags.FlattenHierarchy |
				BindingFlags.Public |
				BindingFlags.Instance |
				BindingFlags.InvokeMethod);

			var constructorInfo = constructors[0];
			var types = new List<Type>()
			{
				typeof(A)
			};

			return new ReflectionInfo(constructorInfo, types);
		}
	}

	internal sealed class DIContainer :
		IDIContainer
	{
		public IDIBinding Bind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public object GetInstance(
			Type type)
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

		public void Unbind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}
	}
}