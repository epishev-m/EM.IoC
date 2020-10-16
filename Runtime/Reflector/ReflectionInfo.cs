
namespace EM.IoC
{
	using EM.Foundation;
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public sealed class ReflectionInfo :
		IReflectionInfo
	{
		#region IReflectionInfo

		public ConstructorInfo ConstructorInfo => constructorInfo;

		public IEnumerable<Type> ParameterTypes => parameterTypes;

		#endregion
		#region ReflectionInfo

		private readonly ConstructorInfo constructorInfo;

		private readonly IEnumerable<Type> parameterTypes;

		public ReflectionInfo(
			ConstructorInfo constructorInfo,
			IEnumerable<Type> parameterTypes)
		{
			Requires.IsNotNull(constructorInfo, nameof(constructorInfo));
			Requires.IsNotNull(parameterTypes, nameof(parameterTypes));

			this.constructorInfo = constructorInfo;
			this.parameterTypes = parameterTypes;
		}

		#endregion
	}
}
