using EM.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;

internal sealed class ReflectionInfoTests
{
	[Test]
	public void ReflectionInfo_ConstructorParam1_Exeprion()
	{
		// Arrange
		var actual = false;
		var param = new List<Type>();

		// Act
		try
		{
			var unused = new ReflectionInfo(null, param);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ReflectionInfo_ConstructorParam2_Exeprion()
	{
		// Arrange
		var actual = false;
		var param = typeof(Test).GetConstructor(new Type[0]);

		// Act
		try
		{
			var unused = new ReflectionInfo(param, null);
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

