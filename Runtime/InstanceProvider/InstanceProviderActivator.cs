
namespace EM.IoC
{
	using EM.Foundation;
	using System;
	using System.Collections.Generic;

	public sealed class InstanceProviderActivator :
		IInstanceProvider
	{
		#region IInstanceProvider

		public object GetInstance()
		{
			var reflectionInfo = reflector.GetReflectionInfo(type);
			var argsList = new List<object>(8);

			foreach (var type in reflectionInfo.ParameterTypes)
			{
				var arg = diContainer.GetInstance(type);
				argsList.Add(arg);
			}

			var instance = Activator.CreateInstance(type, argsList.ToArray());

			return instance;
		}

		#endregion
		#region InstanceProviderActivator

		private readonly Type type;

		private readonly IReflector reflector;

		private readonly IDIContainer diContainer;

		public InstanceProviderActivator(
			Type type,
			IReflector reflector,
			IDIContainer diContainer)
		{
			Requires.IsNotNull(type, nameof(type));
			Requires.IsNotNull(reflector, nameof(reflector));
			Requires.IsNotNull(diContainer, nameof(diContainer));

			this.type = type;
			this.reflector = reflector;
			this.diContainer = diContainer;
		}

		#endregion
	}
}
