using System;
using System.Collections.Generic;
using System.Reflection;

namespace EM.IoC
{
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
