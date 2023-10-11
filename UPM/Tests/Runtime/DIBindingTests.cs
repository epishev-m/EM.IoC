using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EM.Foundation;
using EM.IoC;
using NUnit.Framework;

internal sealed class DiBindingTests
{
	#region Constructor

	[Test]
	public void DIBinding_ConstructorParam1_Exception()
	{
		// Arrange
		var actual = false;
		var container = new DiContainer();
		var key = typeof(Test);

		// Act
		try
		{
			var unused = new DiBinding(null, container, key, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ConstructorParam2_Exception()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var key = typeof(Test);

		// Act
		try
		{
			var unused = new DiBinding(reflector, null, key, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ConstructorParam3_Exception()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DiContainer();

		// Act
		try
		{
			var unused = new DiBinding(reflector, container, null, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion

	#region LifeTime

	[Test]
	public void DIBinding_InGlobal()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);


		// Act
		binding.InGlobal();
		var actual = binding.LifeTime;

		// Assert
		Assert.AreEqual(LifeTime.Global, actual);
	}

	[Test]
	public void DIBinding_InGlobal_IsInvalidOperationException()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);
		binding.InGlobal();

		// Act
		try
		{
			binding.InGlobal();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_InLocal()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		binding.InLocal();
		var actual = binding.LifeTime;

		// Assert
		Assert.AreEqual(LifeTime.Local, actual);
	}

	[Test]
	public void DIBinding_InLocal_IsInvalidOperationException()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);
		binding.InLocal();

		// Act
		try
		{
			binding.InLocal();
		}
		catch (InvalidOperationException)
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
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);
		binding.InGlobal();

		// Act
		try
		{
			binding.To(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_To_NotLifeTimeException()
	{
		// Arrange
		var actual = false;
		var instance = new Test(null);
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.To(instance);
		}
		catch (InvalidOperationException)
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
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);
		binding.InGlobal();

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
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		binding.InGlobal().To(instance);
		var provider = binding.Values.FirstOrDefault();
		var actual = provider?.GetType();

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
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, Resolver);

		// Act
		binding.InGlobal().To(instance);

		void Resolver(IBinding unused) => actual = true;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToGeneric_NotLifeTimeException()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		try
		{
			binding.To<Test>();
		}
		catch (InvalidOperationException)
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
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		binding.InGlobal().To<Test>();
		var provider = binding.Values.FirstOrDefault();
		var actual = provider?.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToGeneric_ReturnIDIBinder()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		var actual = binding.InGlobal().To<Test>();

		// Assert
		Assert.AreEqual(binding, actual);
	}

	[Test]
	public void DIBinding_ToGeneric_Resolver()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, Resolver);

		// Act
		binding.InGlobal().To<Test>();

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
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(TestFactory);
		var binding = new DiBinding(reflector, container, key, null, null);
		binding.InGlobal();

		// Act
		try
		{
			binding.ToFactory(null);
		}
		catch (ArgumentNullException)
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
		var container = new DiContainer();
		var key = typeof(TestFactory);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		binding.InGlobal().ToFactory(instance);
		var provider = binding.Values.FirstOrDefault();
		var actual = provider?.GetType();

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
		var container = new DiContainer();
		var key = typeof(TestFactory);
		var binding = new DiBinding(reflector, container, key, null, Resolver);

		// Act
		binding.InGlobal().ToFactory(instance);

		void Resolver(IBinding unused) => actual = true;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIBinding_ToFactoryGenericAndGetValues()
	{
		// Arrange
		var expected = typeof(InstanceProviderFactory);
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(TestFactory);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		binding.InGlobal().ToFactory<TestFactory>();
		var provider = binding.Values.FirstOrDefault();
		var actual = provider?.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToFactoryGeneric_ReturnIDIBinder()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(TestFactory);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		var actual = binding.InGlobal().ToFactory<TestFactory>();

		// Assert
		Assert.AreEqual(binding, actual);
	}

	[Test]
	public void DIBinding_ToFactoryGeneric_Resolver()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(TestFactory);
		var binding = new DiBinding(reflector, container, key, null, Resolver);

		// Act
		binding.InGlobal().ToFactory<TestFactory>();

		void Resolver(IBinding unused) => actual = true;

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion

	#region ToSingleton

	[Test]
	public void DIBinding_ToAndTo_IsInvalidOperationException()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);
		binding.InGlobal();

		// Act
		try
		{
			binding.AsSingle();
		}
		catch (InvalidOperationException)
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
		var container = new DiContainer();
		var key = typeof(Test);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		binding.InGlobal().To<Test>().AsSingle();
		var provider = binding.Values.FirstOrDefault();
		var actual = provider?.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIBinding_ToFactoryGenericAndToSingletonAndGetValues()
	{
		// Arrange
		var expected = typeof(InstanceProviderSingleton);
		var reflector = new Reflector();
		var container = new DiContainer();
		var key = typeof(TestFactory);
		var binding = new DiBinding(reflector, container, key, null, null);

		// Act
		binding.InGlobal().ToFactory<TestFactory>().AsSingle();
		var provider = binding.Values.FirstOrDefault();
		var actual = provider?.GetType();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	#endregion

	#region Nested

	private sealed class TestFactory :
		IFactory
	{
		public Result<object> Create()
		{
			throw new NotImplementedException();
		}
	}

	private struct TestStruct
	{
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
		public Result<IReflectionInfo> GetReflectionInfo<T>()
		{
			return GetReflectionInfo(typeof(T));
		}

		public Result<IReflectionInfo> GetReflectionInfo(Type type)
		{
			var reflectionInfo = new ReflectionInfo(type);

			return new SuccessResult<IReflectionInfo>(reflectionInfo);
		}
	}

	private sealed class DiContainer :
		IDiContainer
	{
		public List<T> ResolveAll<T>() where T : class
		{
			throw new NotImplementedException();
		}

		public IDiBinding Bind<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public object Resolve(
			Type type)
		{
			var result = default(object);

			if (type == typeof(A))
			{
				result = new A();
			}

			return result;
		}

		public T Resolve<T>()
			where T : class
		{
			throw new NotImplementedException();
		}

		public IList ResolveAll(Type type)
		{
			throw new NotImplementedException();
		}

		public void Inject(object obj)
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

	#endregion
}