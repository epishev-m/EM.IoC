﻿using EM.Foundation;
using EM.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

internal sealed class DIBindingTests
{
	#region Constructor

	[Test]
	public void DIBinding_ConstructorParam1_Exeption()
	{
		// Arrange
		var actual = false;
		var reflector = default(Reflector);
		var container = new DIContainer();
		var key = typeof(Test);

		// Act
		try
		{
			var unused = new DIBinding(reflector, container, key, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ConstructorParam2_Exeption()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = default(DIContainer);
		var key = typeof(Test);

		// Act
		try
		{
			var unused = new DIBinding(reflector, container, key, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ConstructorParam3_Exeption()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = default(Type);

		// Act
		try
		{
			var unused = new DIBinding(reflector, container, key, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region To

	[Test]
	public void DIBinding_To_IsNotNullException()
	{
		// Arrange
		var actual = false;
		var instance = default(Test);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.To(instance);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_To_IsNullException()
	{
		// Arrange
		var actual = false;
		var instance = new Test(null);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.To(instance);
			binding.To(instance);
		}
		catch (ArgumentException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_To_IsReferenceTypeException()
	{
		// Arrange
		var actual = false;
		var instance = new TestStruct();
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.To(instance);
		}
		catch (ArgumentException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToAndGetValues()
	{
		// Arrange
		var expected = typeof(InstanceProvider);
		var instance = new Test(null);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		binding.To(instance);
		var provider = binding.Values.FirstOrDefault();
		var actual = provider.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_To_Resolver()
	{
		// Arrange
		var actual = false;
		var instance = new Test(null);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, Resolver);

		// Act
		binding.To(instance);

		void Resolver(IBinding unused) => actual = true;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToGeneric_IsNullException()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.To<Test>();
			binding.To<Test>();
		}
		catch (ArgumentException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToGenericAndGetValues()
	{
		// Arrange
		var expected = typeof(InstanceProviderActivator);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		binding.To<Test>();
		var provider = binding.Values.FirstOrDefault();
		var actual = provider.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToGeneric_ReturnIDIBinder()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, null);
		var expected = binding;

		// Act
		var actual = binding.To<Test>();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToGeneric_Resolver()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, Resolver);

		// Act
		binding.To<Test>();

		void Resolver(IBinding unused) => actual = true;

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region ToFactory

	[Test]
	public void DIBinding_ToFactory_IsNotNullException()
	{
		// Arrange
		var actual = false;
		var instance = default(TestFactory);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.ToFactory(instance);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToFactory_IsNullException()
	{
		// Arrange
		var actual = false;
		var instance = new TestFactory();
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.ToFactory(instance);
			binding.ToFactory(instance);
		}
		catch (ArgumentException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToFactoryAndGetValues()
	{
		// Arrange
		var expected = typeof(InstanceProviderFactory);
		var instance = new TestFactory();
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		binding.ToFactory(instance);
		var provider = binding.Values.FirstOrDefault();
		var actual = provider.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToFactory_Resolver()
	{
		// Arrange
		var actual = false;
		var instance = new TestFactory();
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, Resolver);

		// Act
		binding.ToFactory(instance);

		void Resolver(IBinding unused) => actual = true;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToFactoryGeneric_IsNullException()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.ToFactory<TestFactory>();
			binding.ToFactory<TestFactory>();
		}
		catch (ArgumentException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToFactoryGenericAndGetValues()
	{
		// Arrange
		var expected = typeof(InstanceProviderFactory);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		binding.ToFactory<TestFactory>();
		var provider = binding.Values.FirstOrDefault();
		var actual = provider.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToFactoryGeneric_ReturnIDIBinder()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, null);
		var expected = binding;

		// Act
		var actual = binding.ToFactory<TestFactory>();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToFactoryGeneric_Resolver()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, Resolver);

		// Act
		binding.ToFactory<TestFactory>();

		void Resolver(IBinding unused) => actual = true;

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region ToSingleton

	[Test]
	public void DIBinding_ToAndTo_IsNotNullException()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		IDIBindingSingleton binding = new DIBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.ToSingleton();
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToGenericAndToSingletonAndGetValues()
	{
		// Arrange
		var expected = typeof(InstanceProviderSingleton);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(Test);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		binding.To<Test>().ToSingleton();
		var provider = binding.Values.FirstOrDefault();
		var actual = provider.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToFactoryGenericAndToSingletonAndGetValues()
	{
		// Arrange
		var expected = typeof(InstanceProviderSingleton);
		var reflector = new Reflector();
		var container = new DIContainer();
		var key = typeof(TestFactory);
		var binding = new DIBinding(reflector, container, key, null, null);

		// Act
		binding.ToFactory<TestFactory>().ToSingleton();
		var provider = binding.Values.FirstOrDefault();
		var actual = provider.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	#endregion
	#region Nested

	internal sealed class TestFactory :
		IFactory
	{
		public bool TryCreate(out object instance)
		{
			throw new NotImplementedException();
		}
	}

	internal struct TestStruct
	{
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

	#endregion
}
