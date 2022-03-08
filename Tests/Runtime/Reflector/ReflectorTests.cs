using EM.IoC;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

internal sealed class ReflectorTests
{
	#region Exception

	[Test]
	public void Reflector_ConstructorParam1_Exception()
	{
		// Arrange
		var actual = false;

		// Act
		try
		{
			var unused = new Reflector(null);
		}
		catch (ArgumentNullException)
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
		var reflector = new Reflector(typeof(TestPostConstructorAttribute));

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
	public void Reflector_GetReflectionInfoGeneric_ExceptionManyConstructors()
	{
		// Arrange
		var actual = false;
		var reflector = new Reflector(typeof(TestPostConstructorAttribute));

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
		var reflector = new Reflector(typeof(TestPostConstructorAttribute));

		// Act
		var reflectionInfo = reflector.GetReflectionInfo(type);
		var actual = reflectionInfo.ConstructorParametersTypes.Count();

		// Assert
		Assert.AreEqual(2, actual);
	}

	[Test]
	public void Reflector_GetReflectionInfoGeneric_CountParams()
	{
		// Arrange
		var reflector = new Reflector(typeof(TestPostConstructorAttribute));

		// Act
		var reflectionInfo = reflector.GetReflectionInfo<Test>();
		var actual = reflectionInfo.ConstructorParametersTypes.Count();

		// Assert
		Assert.AreEqual(2, actual);
	}

	#endregion
	#region Nested

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	private sealed class Test
	{
		public Test(int param1, int param2)
		{
		}
	}

	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	private sealed class TestManyConstructors
	{
		public TestManyConstructors(int param)
		{
		}

		public TestManyConstructors(int param1, int param2)
		{
		}
	}

	private sealed class TestPostConstructorAttribute :
		Attribute
	{
	}

	#endregion
}
