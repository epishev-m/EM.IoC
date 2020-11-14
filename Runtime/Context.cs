
namespace EM.IoC
{
	using UnityEngine;

	public abstract class Context : MonoBehaviour
	{
		#region MonoBehaviour

		private void Awake()
		{
			if (mainContext == null)
			{
				mainContext = this;

				var reflector = new Reflector();
				container = new DIContainer(reflector);
				signalCommandContainer = new SignalCommandContainer(container);
			}

			Initialize();
		}

		private void Start()
		{
			Run();
		}

		private void OnDestroy()
		{
			Release();

			if (mainContext == this)
			{
				signalCommandContainer.UnbindAll();
				container.UnbindAll();
			}
			else
			{
				signalCommandContainer.Unbind(LifeTime.Local);
				container.Unbind(LifeTime.Local);
			}
		}

		#endregion
		#region Context

		protected static Context mainContext;

		protected static IDIContainer container;

		protected static ISignalCommandContainer signalCommandContainer;

		public static bool IsExistedMainContext => mainContext != null;

		protected abstract void Initialize();

		protected abstract void Release();

		protected abstract void Run();

		#endregion
	}
}
