using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EM.Foundation;
using EM.IoC;
using NUnit.Framework;

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

	#region Resolve

	[Test]
	public void DIContainer_ResolveGeneric_ReturnNull()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		// Act
		var actual = container.Resolve<Test>();

		//Assert
		Assert.AreEqual(null, actual);
	}

	[Test]
	public void DIContainer_ResolveGeneric()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<Test>()
			.InGlobal()
			.To<Test>();

		// Act
		var instance = container.Resolve<Test>();

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
			var unused = container.Resolve(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		//Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void DIContainer_ResolveAllGeneric()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<ITest>()
			.InGlobal()
			.To<Test>()
			.To<Test>();

		// Act
		var instance = container.ResolveAll<ITest>();

		//Assert
		Assert.IsNotNull(instance);
		Assert.IsTrue(instance.Count == 2);
	}
	
	[Test]
	public void DIContainer_ResolveAllGeneric_Constructor()
	{
		// Arrange
		var reflector = new Reflector();
		var container = new DiContainer(reflector);

		container.Bind<TestParam>()
			.InGlobal()
			.To(new TestParam());

		container.Bind<ITest>()
			.InGlobal()
			.To<Test>();

		container.Bind<ITest>()
			.To(new TestToo());
		
		container.Bind<TestListParam>()
			.InGlobal()
			.To<TestListParam>()
			.AsSingle();

		// Act
		var instance = container.Resolve<TestListParam>();

		//Assert
		Assert.IsNotNull(instance);
		Assert.IsNotNull(instance.Param);
		Assert.IsNotNull(instance.List);
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
		var actual = container.Resolve<Test>();

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
		var actual = container.Resolve<Test>();

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
		var actual = container.Resolve<Test>();

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
		var actual = container.Resolve<Test>();

		//Assert
		Assert.IsNotNull(actual);
	}

	#endregion

	#region Nested
	
	private interface ITest
	{
	}

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
	private sealed class Test : ITest
	{
		public Test(TestParam param)
		{
		}
	}
	
	private sealed class TestToo : ITest
	{
	}

	private sealed class TestListParam
	{
		public readonly TestParam Param;

		public readonly List<ITest> List;

		public TestListParam(TestParam param,
			List<ITest> list)
		{
			Param = param;
			List = list;
		}
	}

	private sealed class TestParam
	{
	}

	private sealed class Reflector : IReflector
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

	#endregion
}