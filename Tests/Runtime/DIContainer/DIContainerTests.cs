using EM.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using EM.Foundation;

internal sealed class DiContainerTests
{
	#region Common

	[Test]
	public void DIContainer_Constructor_Exception()
	{
		// Arrange
		var actual = false;

		// Act
		try
		{
			var unused = new DiContainer(null);
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
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		// Act
		var actual = container.Bind<Test>();

		//Assert
		Assert.IsNotNull(actual);
	}

	#endregion

	#region GetInstance

	[Test]
	public void DIContainer_GetInstanceGeneral_ReturnNull()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		// Act
		var actual = container.GetInstance<Test>();

		//Assert
		Assert.AreEqual(null, actual);
	}

	[Test]
	public void DIContainer_GetInstanceGeneral()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<Test>()
			.InGlobal()
			.To<Test>();

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
		var container = new DiContainer(reflector);

		container.Bind<Test>()
			.InGlobal()
			.To<Test>();

		// Act
		try
		{
			var unused = container.GetInstance(null);
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
		var container = new DiContainer(reflector);

		// Act
		var actual = container.Unbind<string>();

		//Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void DIContainer_UnbindGeneric_ReturnTrue()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<Test>()
			.InGlobal()
			.To<Test>();

		// Act
		var actual = container.Unbind<Test>();

		//Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIContainer_UnbindAndGetInstance_ReturnNull()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<Test>()
			.InGlobal()
			.To<Test>();

		// Act
		var unused = container.Unbind<Test>();
		var actual = container.GetInstance<Test>();

		//Assert
		Assert.IsNull(actual);
	}

	[Test]
	public void DIContainer_UnbindAll_GetInstance()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<Test>()
			.InGlobal()
			.To<Test>();

		// Act
		container.UnbindAll();
		var actual = container.GetInstance<Test>();

		//Assert
		Assert.IsNull(actual);
	}

	[Test]
	public void DIContainer_UnbindLifeTime_GetInstance_ReturnNull()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<Test>()
			.InGlobal()
			.To<Test>();

		// Act
		container.Unbind(LifeTime.Global);
		var actual = container.GetInstance<Test>();

		//Assert
		Assert.IsNull(actual);
	}

	[Test]
	public void DIContainer_UnbindLifeTime_GetInstance_ReturnNotNull()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<Test>()
			.InLocal()
			.To<Test>();

		// Act
		container.Unbind(LifeTime.Global);
		var actual = container.GetInstance<Test>();

		//Assert
		Assert.IsNotNull(actual);
	}

	#endregion

	#region Nested

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
	private sealed class Test
	{
		public Test(
			TestParam param)
		{
		}
	}

	private sealed class TestParam
	{
	}

	private sealed class Reflector :
		IReflector
	{
		public IReflectionInfo GetReflectionInfo<T>()
		{
			return GetReflectionInfo(typeof(T));
		}

		public IReflectionInfo GetReflectionInfo(
			Type type)
		{
			var constructors = type.GetConstructors(BindingFlags.FlattenHierarchy |
													BindingFlags.Public |
													BindingFlags.Instance |
													BindingFlags.InvokeMethod);

			var constructorInfo = constructors[0];

			var types = new List<Type>
			{
				typeof(TestParam)
			};

			return new ReflectionInfo(constructorInfo, types, null, null);
		}
	}

	#endregion
}