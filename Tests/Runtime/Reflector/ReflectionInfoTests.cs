using EM.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;

internal sealed class ReflectionInfoTests
{
	[Test]
	public void ReflectionInfo_ConstructorParam1_Exception()
	{
		// Arrange
		var actual = false;
		var methodsParams = typeof(Test).GetMethods();

		// Act
		try
		{
			var unused = new ReflectionInfo(null, new List<Type>(), null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ReflectionInfo_ConstructorParam2_Exception()
	{
		// Arrange
		var actual = false;
		var param = typeof(Test).GetConstructor(Type.EmptyTypes);

		// Act
		try
		{
			var unused = new ReflectionInfo(param, null, null, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#region Nested

	private sealed class Test
	{
	}

	#endregion
}