namespace EM.IoC
{
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Context :
	MonoBehaviour
{
	private static Context mainContext;

	protected static IDiContainer Container;

	protected static ISignalCommandContainer SignalCommandContainer;

	#region MonoBehaviour

	private void Awake()
	{
		if (mainContext == null)
		{
			mainContext = this;

			var reflector = new Reflector();
			Container = new DiContainer(reflector);
			SignalCommandContainer = new SignalCommandContainer(Container);
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
			SignalCommandContainer.UnbindAll();
			Container.UnbindAll();
		}
		else
		{
			SignalCommandContainer.Unbind(LifeTime.Local);
			Container.Unbind(LifeTime.Local);
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
