using EM.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

internal sealed class DIContainerTests
{
	#region Common

	[Test]
	public void DIContainer_Constructor_Exception()
	{
		// Arrange
		var actual = false;
		var reflector = default(Reflector);

		// Act
		try
		{
			var unused = new DIContainer(reflector);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIContainer_Bind()
	{
		// Arrange
		var expected = true;
		var reflector = new Reflector();

		// Act
		var container = new DIContainer(reflector);
		var binding = container.Bind<Test>();
		var actual = binding is IDIBinding;

		//Assert
		Assert.IsNotNull(binding);
		Assert.AreEqual(expected, actual);
	}

	#endregion
	#region GetInstance

	[Test]
	public void DIContainer_GetInstanceGeneral_ReturnNull()
	{
		// Arrange
		var expected = default(Test);
		var reflector = new Reflector();
		var container = new DIContainer(reflector);

		// Act
		var actual = container.GetInstance<Test>();

		//Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void DIContainer_GetInstanceGeneral()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DIContainer(reflector);
		container.Bind<Test>().To<Test>();

		// Act
		var instance = container.GetInstance<Test>();

		//Assert
		Assert.IsNotNull(instance);
	}

	[Test]
	public void DIContainer_GetInstance_Exception()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();
		var type = default(Type);
		var container = new DIContainer(reflector);
		container.Bind<Test>().To<Test>();

		// Act
		try
		{
			var unused = container.GetInstance(type);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		//Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Unbind

	[Test]
	public void DIContainer_UnbindGeneric_ReturnFalse()
	{
		// Arrange
		var reflector = new Reflector();

		// Act
		var container = new DIContainer(reflector);
		var actual = container.Unbind<string>();

		//Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void DIContainer_UnbindGeneric_ReturnTrue()
	{
		// Arrange
		var reflector = new Reflector();

		// Act
		var container = new DIContainer(reflector);
		var binding = container.Bind<Test>().To<Test>();
		var actual = container.Unbind<Test>();

		//Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIContainer_UnbindAndGetBinding_ReturnNull()
	{
		// Arrange
		var reflector = new Reflector();

		// Act
		var container = new DIContainer(reflector);
		var binding = container.Bind<Test>().To<Test>();
		var unused = container.Unbind<Test>();
		var actual = container.GetBinding<Test>();

		//Assert
		Assert.IsNull(actual);
	}

	#endregion
	#region Nested

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

	#endregion
}
