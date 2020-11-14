
namespace EM.IoC
{
	using EM.Foundation;

	public interface ICommandBinding : ICommandBindingExecute
	{
		ICommandBinding To<T>()
			where T : ICommand;
	}
}
