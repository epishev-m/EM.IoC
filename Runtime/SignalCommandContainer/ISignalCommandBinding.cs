namespace EM.IoC
{

using Foundation;

public interface ISignalCommandBinding
{
	ISignalCommandBinding To<T>()
		where T : ICommand;

	void Execute(
		object data = null);
}

}