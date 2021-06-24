namespace EM.IoC
{
using Foundation;

public interface ICommandBinding :
	ICommandBindingExecute
{
	ICommandBinding To<T>()
		where T : ICommand;
}

}
