using System;
using System.Collections.Generic;
using System.Reflection;

namespace EM.IoC
{
	public sealed class ReflectionInfo : IReflectionInfo
	{
		#region IReflectionInfo

		public ConstructorInfo ConstructorInfo => _constructorInfo;

		public IEnumerable<Type> ParameterTypes => _parameterTypes;

		#endregion
		#region ReflectionInfo

		private readonly ConstructorInfo _constructorInfo;

		private readonly IEnumerable<Type> _parameterTypes;

		public ReflectionInfo(ConstructorInfo constructorInfo, IEnumerable<Type> parameterTypes)
		{
			_constructorInfo = constructorInfo ??
				throw new ArgumentNullException(nameof(constructorInfo));

			_parameterTypes = parameterTypes ??
				throw new ArgumentNullException(nameof(parameterTypes));
		}

		#endregion
	}
}
