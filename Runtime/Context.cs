namespace EM.IoC
{
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Context :
	MonoBehaviour
{
	private static Context mainContext;

	private static IDiContainer container;

	private static ISignalCommandContainer signalCommandContainer;

	#region MonoBehaviour

	private void Awake()
	{
		if (mainContext == null)
		{
			mainContext = this;

			var reflector = new Reflector();
			container = new DiContainer(reflector);
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

	public static bool IsExistedMainContext => mainContext != null;

	protected abstract void Initialize();

	protected abstract void Release();

	protected abstract void Run();

	#endregion
}

}
