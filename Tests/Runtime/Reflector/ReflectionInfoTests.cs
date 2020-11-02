using EM.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

internal sealed class ReflectionInfoTests
{
	[Test]
	public void ReflectionInfo_ConstructorParam1_Exeprion()
	{
		// Arrange
		var actual = false;
		var param1 = default(ConstructorInfo);
		var param2 = new List<Type>();

		// Act
		try
		{
			var reflectionInfo = new ReflectionInfo(param1, param2);
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
		var param1 = typeof(Test).GetConstructor(new Type[0]);
		var param2 = default(IEnumerable<Type>);

		// Act
		try
		{
			var reflectionInfo = new ReflectionInfo(param1, param2);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#region Nested

	internal sealed class Test
	{
		public Test()
		{
		}
	}

	#endregion
}

