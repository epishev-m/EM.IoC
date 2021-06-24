namespace EM.IoC
{
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Context : MonoBehaviour
{
	private static Context _mainContext;

	private static IDiContainer _container;

	private static ISignalCommandContainer _signalCommandContainer;

	#region MonoBehaviour

	private void Awake()
	{
		if (_mainContext == null)
		{
			_mainContext = this;

			var reflector = new Reflector();
			_container = new DiContainer(reflector);
			_signalCommandContainer = new SignalCommandContainer(_container);
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

		if (_mainContext == this)
		{
			_signalCommandContainer.UnbindAll();
			_container.UnbindAll();
		}
		else
		{
			_signalCommandContainer.Unbind(LifeTime.Local);
			_container.Unbind(LifeTime.Local);
		}
	}

	#endregion
	#region Context

	public static bool IsExistedMainContext => _mainContext != null;

	protected abstract void Initialize();

	protected abstract void Release();

	protected abstract void Run();

	#endregion
}

}
