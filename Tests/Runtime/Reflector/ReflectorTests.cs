using EM.IoC;
using NUnit.Framework;
using System;
using System.Linq;

internal sealed class ReflectorTests
{
	#region Exception

	[Test]
	public void Reflector_GetReflectionInfo_ExceptionNoConstructor()
	{
		// Arrange
		var actual = false;
		var type = typeof(TestNoConstructor);
		var reflector = new Reflector();

		// Act
		try
		{
			var unused = reflector.GetReflectionInfo(type);
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Reflector_GetReflectionInfo_ExceptionManyConstructors()
	{
		// Arrange
		var actual = false;
		var type = typeof(TestManyConstructors);
		var reflector = new Reflector();

		// Act
		try
		{
			var unused = reflector.GetReflectionInfo(type);
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Reflector_GetReflectionInfoGeneric_ExceptionNoConstructor()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();

		// Act
		try
		{
			var unused = reflector.GetReflectionInfo<TestNoConstructor>();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Reflector_GetReflectionInfoGeneric_ExceptionManyConstructors()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector();

		// Act
		try
		{
			var unused = reflector.GetReflectionInfo<TestManyConstructors>();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region GetReflectionInfo

	[Test]
	public void Reflector_GetReflectionInfo_CountParams()
	{
		// Arrange
		var type = typeof(Test);
		var reflector = new Reflector();

		// Act
		var reflectionInfo = reflector.GetReflectionInfo(type);
		var actual = reflectionInfo.ParameterTypes.Count();

		// Assert
		Assert.AreEqual(2, actual);
	}

	[Test]
	public void Reflector_GetReflectionInfoGeneric_CountParams()
	{
		// Arrange
		var reflector = new Reflector();

		// Act
		var reflectionInfo = reflector.GetReflectionInfo<Test>();
		var actual = reflectionInfo.ParameterTypes.Count();

		// Assert
		Assert.AreEqual(2, actual);
	}

	#endregion
	#region Nested

	private sealed class Test
	{
		public Test(int param1, int param2)
		{
		}
	}

	private sealed class TestNoConstructor
	{
		private TestNoConstructor()
		{
		}
	}

	private sealed class TestManyConstructors
	{
		public TestManyConstructors(int param)
		{
		}

		public TestManyConstructors(int param1, int param2)
		{
		}
	}

	#endregion
}
