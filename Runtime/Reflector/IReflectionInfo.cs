namespace EM.IoC
{
using System;
using System.Collections.Generic;
using System.Reflection;

public interface IReflectionInfo
{
	ConstructorInfo ConstructorInfo
	{
		get;
	}

	IEnumerable<Type> ParameterTypes
	{
		get;
	}
}

}
